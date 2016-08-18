using UnityEngine;
using System.Collections;
using System;

public class Tile
{

    public enum TileType { Empty, Floor };

    TileType type = TileType.Empty;

    Action<Tile> cbTileTypeChanged;

    LooseObject looseObject;
    InstalledObject instaledObject;

    World world;
    int x;
    int y;

    public Tile(World world, int x, int y)
    {
        this.world = world;
        this.x = x;
        this.y = y;
    }

    public void RegisterTileChangedCallback(Action<Tile> callback)
    {
        cbTileTypeChanged += callback;
    }

    public void UnregisterTileChangedCallback(Action<Tile> callback)
    {
        cbTileTypeChanged -= callback;
    }

    public int X
    {
        get
        {
            return x;
        }

        set
        {
            x = value;
        }
    }

    public int Y
    {
        get
        {
            return y;
        }
    }

    public TileType Type
    {
        get
        {
            return type;
        }

        set
        {
            TileType oldType = type;
            type = value;
            //Call the callback and let things know we've changed.
            if (cbTileTypeChanged != null && oldType != type)
                cbTileTypeChanged(this);
        }
    }
}
