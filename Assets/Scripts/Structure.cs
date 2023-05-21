using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Structure
{
    public static Queue<VoxelMod> GenerateMajorFlora(int index, Vector3 position, int minTrunkHeight, int maxTrunkHeight) {
        switch(index) {
            case 0:
                return MakeTree(position, minTrunkHeight, maxTrunkHeight, false);
            case 1:
                return MakeCacti(position, minTrunkHeight, maxTrunkHeight);
            case 2:
                return MakeSpruceTree(position, minTrunkHeight, maxTrunkHeight);
            case 3:
                return MakeTree(position, minTrunkHeight, maxTrunkHeight, true);
        }
        return new Queue<VoxelMod>();
    }
    public static Queue<VoxelMod> MakeTree(Vector3 position, int minTrunkHeight, int maxTrunkHeight, bool isBirch)
    {

        Queue<VoxelMod> queue = new Queue<VoxelMod>();

        int height = (int)(maxTrunkHeight * Noise.Get2DPerlin(new Vector2(position.x, position.z), 250f, 3f));

        if (height < minTrunkHeight)
            height = minTrunkHeight;

        if (isBirch != true) {

            for (int i = 1; i < height; i++)
                queue.Enqueue(new VoxelMod(new Vector3(position.x, position.y + i, position.z), 7));

            for (int x = -2; x < 3; x++)
            {
                for (int y = 0; y < 2; y++)
                {
                    for (int z = -2; z < 3; z++)
                    {
                        queue.Enqueue(new VoxelMod(new Vector3(position.x + x, position.y + height + y, position.z + z), 15));
                    }
                }
            }
            for (int x = -1; x < 2; x++)
            {
                for (int y = 0; y < 2; y++)
                {
                    for (int z = -1; z < 2; z++)
                    {
                        queue.Enqueue(new VoxelMod(new Vector3(position.x + x, position.y + height + y + 2, position.z + z), 15));
                    }
                }
            }
        }
        else if (isBirch) {
            for (int i = 1; i < height; i++)
                queue.Enqueue(new VoxelMod(new Vector3(position.x, position.y + i, position.z), 38));

            for (int x = -2; x < 3; x++)
            {
                for (int y = 0; y < 2; y++)
                {
                    for (int z = -2; z < 3; z++)
                    {
                        queue.Enqueue(new VoxelMod(new Vector3(position.x + x, position.y + height + y, position.z + z), 40));
                    }
                }
            }
            for (int x = -1; x < 2; x++)
            {
                for (int y = 0; y < 2; y++)
                {
                    for (int z = -1; z < 2; z++)
                    {
                        queue.Enqueue(new VoxelMod(new Vector3(position.x + x, position.y + height + y + 2, position.z + z), 40));
                    }
                }
            }
        }

        return queue;

    }

    public static Queue<VoxelMod> MakeCacti(Vector3 position, int minTrunkHeight, int maxTrunkHeight)
    {

        Queue<VoxelMod> queue = new Queue<VoxelMod>();

        int height = (int)(maxTrunkHeight * Noise.Get2DPerlin(new Vector2(position.x, position.z), 4000f, 2f));

        if (height < minTrunkHeight)
            height = minTrunkHeight;

        for (int i = 1; i <= height; i++)
            queue.Enqueue(new VoxelMod(new Vector3(position.x, position.y + i, position.z), 22));

        return queue;

    }

    public static Queue<VoxelMod> MakeSpruceTree(Vector3 position, int minTrunkHeight, int maxTrunkHeight)
    {

        Queue<VoxelMod> queue = new Queue<VoxelMod>();

        int height = (int)(maxTrunkHeight * Noise.Get2DPerlin(new Vector2(position.x, position.z), 250f, 3f));

        if (height < minTrunkHeight)
            height = minTrunkHeight;

        for (int i = 1; i < height; i++)
            queue.Enqueue(new VoxelMod(new Vector3(position.x, position.y + i, position.z), 35));

        for (int x = -1; x < 2; x++)
        {
            for (int y = 0; y < 1; y++)
            {
                for (int z = -1; z < 2; z++)
                {
                    queue.Enqueue(new VoxelMod(new Vector3(position.x + x, position.y + height + y, position.z + z), 36));
                }
            }
        }

        for (int x = -2; x < 3; x++)
        {
            for (int y = 0; y < 2; y++)
            {
                for (int z = -2; z < 3; z++)
                {
                    queue.Enqueue(new VoxelMod(new Vector3(position.x + x, position.y + height + y + 1, position.z + z), 36));
                }
            }
        }
        for (int x = -1; x < 2; x++)
        {
            for (int y = 0; y < 2; y++)
            {
                for (int z = -1; z < 2; z++)
                {
                    queue.Enqueue(new VoxelMod(new Vector3(position.x + x, position.y + height + y + 3, position.z + z), 36));
                }
            }
        }
        for (int x = -2; x < 3; x++)
        {
            for (int y = 0; y < 1; y++)
            {
                for (int z = -2; z < 3; z++)
                {
                    queue.Enqueue(new VoxelMod(new Vector3(position.x + x, position.y + height + y + 5, position.z + z), 36));
                }
            }
        }
        for (int x = -1; x < 2; x++)
        {
            for (int y = 0; y < 1; y++)
            {
                for (int z = -1; z < 2; z++)
                {
                    queue.Enqueue(new VoxelMod(new Vector3(position.x + x, position.y + height + y + 6, position.z + z), 36));
                }
            }
        }
        queue.Enqueue(new VoxelMod(new Vector3(position.x, position.y + height + 7, position.z), 35));

        for (int x = -1; x < 2; x++)
        {
            for (int y = 0; y < 1; y++)
            {
                for (int z = -1; z < 2; z++)
                {
                    queue.Enqueue(new VoxelMod(new Vector3(position.x + x, position.y + height + y + 8, position.z + z), 36));
                }
            }
        }
        queue.Enqueue(new VoxelMod(new Vector3(position.x, position.y + height + 9, position.z), 36));
        

        return queue;

    }

}