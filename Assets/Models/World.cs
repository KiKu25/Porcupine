using UnityEngine;
using System.Collections.Generic;
using System;

public class World
{

    Tile[,] tiles;

    Dictionary<string, InstalledObject> installedObjectPrototyps;

    public int Width { get; protected set; }
    public int Height { get; protected set; }

    Action<InstalledObject> cbInstalldedObjectCreated;

    public World(int width = 100, int height = 100)
    {
        this.Width = width;
        this.Height = height;

        tiles = new Tile[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                tiles[x, y] = new Tile(this, x, y);
            }
        }

        Debug.Log("World created with " + (width * height) + " tiles.");

        CreateInstalledObjectPrototypes();
    }

    void CreateInstalledObjectPrototypes()
    {
        installedObjectPrototyps = new Dictionary<string, InstalledObject>();

        installedObjectPrototyps.Add("Wall", InstalledObject.CreatePrototype("Wall", 0, 1, 1));
    }

    public void RandomizeTiles()
    {
        Debug.Log("RadomizeTiles.");
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (UnityEngine.Random.Range(0, 2) == 0)
                {
                    tiles[x, y].Type = TileType.Empty;
                }
                else
                {
                    tiles[x,y].Type = TileType.Floor;
                }
            }
        }
    }

    public Tile GetTileAt(int x, int y)
    {
        if (x > Width || x < 0 || y > Width || y < 0)
        {
            Debug.LogError("Tile ( " + x + ", " + y + ") is out of range.");
            return null;
        }
        return tiles[x, y];
    }

    public void PlaceInstalledObject(string objectType, Tile t)
    {
        //Debug.Log("PlaceInstalledObject");
        //TODO: This function assumes 1x1 tiles.

        if (installedObjectPrototyps.ContainsKey(objectType) == false)
        {
            Debug.Log("installeObjectPrototyps doesn't contain a proto for key: " + objectType);
            return;
        }

        InstalledObject obj = InstalledObject.PlaceInstance(installedObjectPrototyps[objectType], t);

        if (obj == null)
        {
            // Faild to palce a oject -- most likly was already something there.   
            return;
        }

        if (cbInstalldedObjectCreated != null)
        {
            cbInstalldedObjectCreated(obj);
        }
    }

    public void RegisterInstalledObjectCreated(Action<InstalledObject> callbackfunc)
    {
        cbInstalldedObjectCreated += callbackfunc;
    }

    public void UnregisterInstalledObjectCreated(Action<InstalledObject> callbackfunc)
    {
        cbInstalldedObjectCreated -= callbackfunc;
    }
}
