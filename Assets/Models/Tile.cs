using UnityEngine;
using System.Collections;
using System;

public enum TileType { Empty, Floor };

public class Tile
{
    TileType type = TileType.Empty;

    Action<Tile> cbTileTypeChanged;

    LooseObject LooseObject;
    public InstalledObject instaledObject { get; protected set; }

    World world;
    public int X { get; protected set; }
    public int Y { get; protected set; }

    public Tile(World world, int x, int y)
    {
        this.world = world;
        this.X = x;
        this.Y = y;
    }

    public void RegisterTileChangedCallback(Action<Tile> callback)
    {
        cbTileTypeChanged += callback;
    }

    public void UnregisterTileChangedCallback(Action<Tile> callback)
    {
        cbTileTypeChanged -= callback;
    }

    public TileType Type
    {
        get { return type; }
        set
        {
            TileType oldType = type;
            type = value;
            //Call the callback and let things know we've changed.
            if (cbTileTypeChanged != null && oldType != type)
                cbTileTypeChanged(this);
        }
    }

    public bool PlaceObject(InstalledObject objInstance)
    {
        if (objInstance == null)
        {
            //We are uninstalling whatever was here befor.
            instaledObject = null;
            return true;
        }

        if (instaledObject != null)
        {
            Debug.LogError("Trying to assign an installed object to a tile that alredy has one!");
            return false;
        }
        instaledObject = objInstance;
        return true;
    }
}
