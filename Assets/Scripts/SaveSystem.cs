using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

public static class SaveSystem
{
    public static void SaveWorld(WorldData world) {
        string savePath = World.Instance.appPath + "/saves/" + world.worldName + "/";

        if (!Directory.Exists(savePath)) {
            Directory.CreateDirectory(savePath);
        }
        Debug.Log("Saving world: " + world.worldName);

        BinaryFormatter Formatter = new BinaryFormatter();
        FileStream stream = new FileStream(savePath + world.worldName + ".mcuworld", FileMode.Create);

        Formatter.Serialize(stream, world);
        stream.Close();

        Thread thread = new Thread(() => SaveChunks(world));
        thread.Start();

        
    }

    public static void SaveChunks(WorldData world) {
        List<ChunkData> chunks = new List<ChunkData>(world.modifiedChunks);
        world.modifiedChunks.Clear();
        int count = 0;
        foreach (ChunkData chunk in chunks) {
            SaveSystem.SaveChunk(chunk, world.worldName);
            count++;
        }
        Debug.Log("Saved " + count + " chunks");
    }

    public static void SaveChunk(ChunkData chunk, string worldName) {

        string chunkName = chunk.position.x + "," + chunk.position.y;
        string savePath = World.Instance.appPath + "/saves/" + worldName + "/chunks/";

        if (!Directory.Exists(savePath)) {
            Directory.CreateDirectory(savePath);
        }

        BinaryFormatter Formatter = new BinaryFormatter();
        FileStream stream = new FileStream(savePath + chunkName + ".mcuchunk", FileMode.Create);

        Formatter.Serialize(stream, chunk);
        stream.Close();
    }

    public static WorldData LoadWorld(string worldName, int seed = 0) {
        // string loadPath = World.Instance.appPath + "/saves/" + worldName + "/";
        if (File.Exists(World.Instance.appPath + "/saves/" + worldName + ".mcuworld")) {
            Debug.Log("Loading world: " + worldName);
            BinaryFormatter Formatter = new BinaryFormatter();
            FileStream stream = new FileStream(World.Instance.appPath + "/saves/" + worldName + ".mcuworld", FileMode.Open);
            WorldData world = Formatter.Deserialize(stream) as WorldData;
            stream.Close();
            return new WorldData(world);
        }
        else {
            Debug.Log("World not found: " + worldName + ", creating new world");

            WorldData world = new WorldData(worldName, seed);
            SaveWorld(world);
            return world;
        }
    }
    public static ChunkData LoadChunk (string worldName, Vector2Int position) {

        string chunkName = position.x + "," + position.y;

        // Get the path to our world saves.
        string loadPath = World.Instance.appPath + "/saves/" + worldName + "/chunks/" + chunkName + ".mcuchunk";

        // Check if a save exists for the name we were passed.
        if (File.Exists(loadPath)) {

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(loadPath, FileMode.Open);

            ChunkData chunkData = formatter.Deserialize(stream) as ChunkData;
            stream.Close();

            return chunkData;

        }

        // If we didn't find the chunk in our folder, return null and our WorldData script
        // will make a new one.
        return null;
        
    }
}
