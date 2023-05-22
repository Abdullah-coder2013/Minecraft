using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clouds : MonoBehaviour
{

    public int cloudHeight = 100;

    [SerializeField] private Texture2D cloudPattern = null;
    [SerializeField] private Material cloudMaterial = null;
    [SerializeField] private World world = null;
    bool[,] cloudData; // Array of bools representing where cloud is.

    int cloudTexWidth;

    int cloudTileSize;
    Vector3Int offset;

    Dictionary<Vector2Int, GameObject> clouds = new Dictionary<Vector2Int, GameObject>();

    private void Start()
    {

        cloudTexWidth = cloudPattern.width;
        cloudTileSize = VoxelData.ChunkWidth;
        offset = new Vector3Int(-(cloudTexWidth / 2), 0, -(cloudTexWidth / 2));

        transform.position = new Vector3(VoxelData.WorldCentre, cloudHeight, VoxelData.WorldCentre);

        LoadCloudData();
        CreateClouds();
    }

    private void LoadCloudData()
    {

        cloudData = new bool[cloudTexWidth, cloudTexWidth];
        Color[] cloudTex = cloudPattern.GetPixels();

        // Loop through colour array and set bools depending on opacity of colour.
        for (int x = 0; x < cloudTexWidth; x++)
        {
            for (int y = 0; y < cloudTexWidth; y++)
            {

                cloudData[x, y] = (cloudTex[y * cloudTexWidth + x].a > 0);

            }
        }
    }

    private void CreateClouds() {
        for (int x = 0; x < cloudTexWidth; x += cloudTileSize) {
            for (int y = 0; y < cloudTexWidth; y += cloudTileSize) {

                Vector3 position = new Vector3(x, cloudHeight, y);
                clouds.Add(CloudTilePosFromV3(position), createCloudTile(CreateCloudMesh(x, y), position));
            }
        }
    }

    public void UpdateClouds()
    {

        for (int x = 0; x < cloudTexWidth; x += cloudTileSize)
        {
            for (int y = 0; y < cloudTexWidth; y += cloudTileSize)
            {

                Vector3 position = world.player.position + new Vector3(x, 0, y) + offset;
                position = new Vector3(RoundToCloud(position.x), cloudHeight, RoundToCloud(position.z));
                Vector2Int cloudPosition = CloudTilePosFromV3(position);

                clouds[cloudPosition].transform.position = position;

            }
        }
    }

    private int RoundToCloud(float value)
    {

        return Mathf.FloorToInt(value / cloudTileSize) * cloudTileSize;

    }

    private Mesh CreateCloudMesh(int x, int z) {

        List<Vector3> vertices = new List<Vector3>();
        List<int> triagles = new List<int>();
        List<Vector3> normals = new List<Vector3>();
        int vertcount = 0;

        for (int xInc = 0; xInc < cloudTileSize; xInc++) {
            for (int zInc = 0; zInc < cloudTileSize; zInc++) {

                int xVal = xInc + x;
                int zVal = zInc + z;

                if (cloudData[xVal, zVal]) {
                    vertices.Add(new Vector3(xInc, 0, zInc));
                    vertices.Add(new Vector3(xInc, 0, zInc + 1));
                    vertices.Add(new Vector3(xInc + 1, 0, zInc + 1));
                    vertices.Add(new Vector3(xInc + 1, 0, zInc));

                    for (int i = 0; i < 4; i++)
                    {
                        normals.Add(Vector3.down);
                    }


                    triagles.Add(vertcount + 1);
                    triagles.Add(vertcount);
                    triagles.Add(vertcount + 2);

                    triagles.Add(vertcount + 2);
                    triagles.Add(vertcount);
                    triagles.Add(vertcount + 3);

                    vertcount += 4;
                }
            }
        }
        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triagles.ToArray();
        mesh.normals = normals.ToArray();
        return mesh;
    }

    private GameObject createCloudTile(Mesh mesh, Vector3 position) {
        GameObject newCloudTile = new GameObject();
        newCloudTile.transform.position = position;
        newCloudTile.transform.parent = transform;
        newCloudTile.name = "Cloud " + position.x + ", " + position.z;
        MeshFilter mF = newCloudTile.AddComponent<MeshFilter>();
        MeshRenderer mR = newCloudTile.AddComponent<MeshRenderer>();

        mR.material = cloudMaterial;
        mF.mesh = mesh;

        return newCloudTile;
    }

    private Vector2Int CloudTilePosFromV3(Vector3 pos)
    {

        return new Vector2Int(CloudTileCoordFromFloat(pos.x), CloudTileCoordFromFloat(pos.z));

    }

    private int CloudTileCoordFromFloat(float value)
    {

        float a = value / (float)cloudTexWidth; // Gets the position using cloudtexture width as units.
        a -= Mathf.FloorToInt(a); // Subtract whole numbers to get a 0-1 value representing position in cloud texture.
        int b = Mathf.FloorToInt((float)cloudTexWidth * a); // Multiply cloud texture width by a to get position in texture globally.

        return b;

    }

}