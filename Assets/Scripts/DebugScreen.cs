using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugScreen : MonoBehaviour
{

    World world;
    Text text;
    float frameRate;
    float timer;
    int halfWorldSizeVoxels;
    int halfWorldSizeChunks;
    // Start is called before the first frame update
    void Start()
    {
        world = GameObject.Find("World").GetComponent<World>();
        text = GetComponent<Text>();
        halfWorldSizeVoxels = VoxelData.WorldSizeInVoxels / 2;
        halfWorldSizeChunks = VoxelData.WorldSizeInChunks / 2; 
    }

    // Update is called once per frame
    void Update()
    {
        string debugText = "Made with Unity. By Abdullah. With the help of b3agz' Tutorial";
        debugText += "\n";
        debugText += frameRate + " fps";
        debugText += "\n\n";
        debugText += "XYZ: " + (Mathf.FloorToInt(world.player.transform.position.x) - halfWorldSizeVoxels) + "/" + Mathf.FloorToInt(world.player.transform.position.y) + "/" + (Mathf.FloorToInt(world.player.transform.position.z) - halfWorldSizeVoxels);
        debugText += "\n";
        debugText += "Chunk: " + (world.playerChunkCoord.x - halfWorldSizeChunks) + "/" + (world.playerChunkCoord.z - halfWorldSizeChunks);
        debugText += "\n\n";
        debugText += "Seed: " + world.settings.seed;

        text.text = debugText;

        if (timer > 1f) {
            frameRate = (int)(1f / Time.unscaledDeltaTime);
            timer = 0;
        }
        else {
            timer += Time.deltaTime;
        }
        
    }
}
