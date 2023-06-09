using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Chunk {

    public ChunkCoord coord;

    GameObject chunkObject;
	MeshRenderer meshRenderer;
	MeshFilter meshFilter;

	int vertexIndex = 0;
	List<Vector3> vertices = new List<Vector3> ();
	List<int> triangles = new List<int> ();
    List<int> transparentTriangles = new List<int>();
    Material[] materials = new Material[2];
	List<Vector2> uvs = new List<Vector2> ();
    List<Color> colors = new List<Color>();
    List<Vector3> normals = new List<Vector3>();

    public Vector3 position;
   
    private bool _isActive;

    ChunkData chunkData;

    public Chunk (ChunkCoord _coord) {

        coord = _coord;

        chunkObject = new GameObject();
        meshFilter = chunkObject.AddComponent<MeshFilter>();
        meshRenderer = chunkObject.AddComponent<MeshRenderer>();

        materials[0] = World.Instance.material;
        materials[1] = World.Instance.transparentMaterial;
        meshRenderer.materials = materials;

        chunkObject.transform.SetParent(World.Instance.transform);
        chunkObject.transform.position = new Vector3(coord.x * VoxelData.ChunkWidth, 0f, coord.z * VoxelData.ChunkWidth);
        chunkObject.name = "Chunk " + coord.x + ", " + coord.z;
        position = chunkObject.transform.position;

        chunkData = World.Instance.worldData.RequestChunk(new Vector2Int((int)position.x, (int)position.z), true);
        chunkData.chunk = this;

        World.Instance.AddChunkToUpdate(this);
        
    }

	public void UpdateChunk () {

        ClearMeshData();

		for (int y = 0; y < VoxelData.ChunkHeight; y++) {
			for (int x = 0; x < VoxelData.ChunkWidth; x++) {
				for (int z = 0; z < VoxelData.ChunkWidth; z++) {

                    if (World.Instance.blocktypes[chunkData.map[x,y,z].id].isSolid)
					    UpdateMeshData (new Vector3(x, y, z));

				}
			}
		}

        World.Instance.chunksToDraw.Enqueue(this);


	}

    void ClearMeshData () {

        vertexIndex = 0;
        vertices.Clear();
        triangles.Clear();
        transparentTriangles.Clear();
        uvs.Clear();
        colors.Clear();
        normals.Clear();

    }

    public bool isActive {

        get { return _isActive; }
        set {

            _isActive = value;
            if (chunkObject != null)
                chunkObject.SetActive(value);

        }

    }

    public void EditVoxel (Vector3 pos, byte newID) {

        int xCheck = Mathf.FloorToInt(pos.x);
        int yCheck = Mathf.FloorToInt(pos.y);
        int zCheck = Mathf.FloorToInt(pos.z);

        xCheck -= Mathf.FloorToInt(chunkObject.transform.position.x);
        zCheck -= Mathf.FloorToInt(chunkObject.transform.position.z);

        chunkData.ModifyVoxel(new Vector3Int(xCheck, yCheck, zCheck), newID);

        UpdateSurroundingVoxels(xCheck, yCheck, zCheck);


    }

    void UpdateSurroundingVoxels (int x, int y, int z) {

        Vector3 thisVoxel = new Vector3(x, y, z);

        for (int p = 0; p < 6; p++) {

            Vector3 currentVoxel = thisVoxel + VoxelData.faceChecks[p];

            if (!chunkData.IsVoxelInChunk((int)currentVoxel.x, (int)currentVoxel.y, (int)currentVoxel.z)) {

                World.Instance.AddChunkToUpdate(World.Instance.GetChunkFromVector3(currentVoxel + position), true);

            }

        }

    }
    
    public VoxelState GetVoxelFromGlobalVector3 (Vector3 pos) {

        int xCheck = Mathf.FloorToInt(pos.x);
        int yCheck = Mathf.FloorToInt(pos.y);
        int zCheck = Mathf.FloorToInt(pos.z);

        xCheck -= Mathf.FloorToInt(position.x);
        zCheck -= Mathf.FloorToInt(position.z);

        return chunkData.map[xCheck, yCheck, zCheck];

    }

    void UpdateMeshData (Vector3 pos) {

        int x = Mathf.FloorToInt (pos.x);
		int y = Mathf.FloorToInt (pos.y);
		int z = Mathf.FloorToInt (pos.z);

        VoxelState voxel = chunkData.map[x, y, z];

		for (int p = 0; p < 6; p++) {
            
            VoxelState neighbor = chunkData.map[x, y, z].neighbours[p];
            if (neighbor == null)
                neighbor = chunkData.map[x, y, z].neighbours[p];

			if (neighbor != null && World.Instance.blocktypes[neighbor.id].renderNeighborFaces) {

				vertices.Add (pos + VoxelData.voxelverts [VoxelData.voxelTris [p, 0]]);
				vertices.Add (pos + VoxelData.voxelverts [VoxelData.voxelTris [p, 1]]);
				vertices.Add (pos + VoxelData.voxelverts [VoxelData.voxelTris [p, 2]]);
				vertices.Add (pos + VoxelData.voxelverts [VoxelData.voxelTris [p, 3]]);

                for (int i = 0; i < 4; i++)
                    normals.Add(VoxelData.faceChecks[p]);

                AddTexture(voxel.properties.GetTextureID(p));

                float lightLevel = neighbor.lightAsFloat;

                

                colors.Add(new Color(0, 0, 0, lightLevel));
                colors.Add(new Color(0, 0, 0, lightLevel));
                colors.Add(new Color(0, 0, 0, lightLevel));
                colors.Add(new Color(0, 0, 0, lightLevel));

                if (!neighbor.properties.renderNeighborFaces) {
				    triangles.Add (vertexIndex);
				    triangles.Add (vertexIndex + 1);
				    triangles.Add (vertexIndex + 2);
				    triangles.Add (vertexIndex + 2);
				    triangles.Add (vertexIndex + 1);
				    triangles.Add (vertexIndex + 3);
                } else {
                    transparentTriangles.Add (vertexIndex);
				    transparentTriangles.Add (vertexIndex + 1);
				    transparentTriangles.Add (vertexIndex + 2);
				    transparentTriangles.Add (vertexIndex + 2);
				    transparentTriangles.Add (vertexIndex + 1);
				    transparentTriangles.Add (vertexIndex + 3);
                }

                vertexIndex += 4;

			}
		}

	}

	public void CreateMesh () {

		Mesh mesh = new Mesh ();
		mesh.vertices = vertices.ToArray ();

        mesh.subMeshCount = 2;
        mesh.SetTriangles(triangles.ToArray(), 0);
        mesh.SetTriangles(transparentTriangles.ToArray(), 1);
        //mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray ();
        mesh.colors = colors.ToArray();
        mesh.normals = normals.ToArray();

		meshFilter.mesh = mesh;

	}

    void AddTexture (int textureID) {

        float y = textureID / VoxelData.TextureAtlasSizeInBlocks;
        float x = textureID - (y * VoxelData.TextureAtlasSizeInBlocks);

        x *= VoxelData.NormalizedBlockTextureSize;
        y *= VoxelData.NormalizedBlockTextureSize;

        y = 1f - y - VoxelData.NormalizedBlockTextureSize;

        uvs.Add(new Vector2(x, y));
        uvs.Add(new Vector2(x, y + VoxelData.NormalizedBlockTextureSize));
        uvs.Add(new Vector2(x + VoxelData.NormalizedBlockTextureSize, y));
        uvs.Add(new Vector2(x + VoxelData.NormalizedBlockTextureSize, y + VoxelData.NormalizedBlockTextureSize));


    }

}

public class ChunkCoord {

    public int x;
    public int z;

    public ChunkCoord () {

        x = 0;
        z = 0;

    }

    public ChunkCoord (int _x, int _z) {

        x = _x;
        z = _z;

    }

    public ChunkCoord (Vector3 pos) {

        int xCheck = Mathf.FloorToInt(pos.x);
        int zCheck = Mathf.FloorToInt(pos.z);

        x = xCheck / VoxelData.ChunkWidth;
        z = zCheck / VoxelData.ChunkWidth;

    }

    public bool Equals (ChunkCoord other) {

        if (other == null)
            return false;
        else if (other.x == x && other.z == z)
            return true;
        else
            return false;

    }

}