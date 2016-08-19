using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

public class WorldController : MonoBehaviour {

    public static WorldController Instance { get; protected set; }

    public Sprite floorSprite;

    Dictionary<Tile, GameObject> tileGameObjectMap;

    public World World { get; protected set; }

    void Start ()
    {
        if (Instance != null)
        {
            Debug.LogError("There should never be two world controllers.");
        }
        Instance = this;

        //Create the world with Empty tiles.
        World = new World();

        //Instantiate our dictionary that tracks which GameObject is rendering which Tile data.
        tileGameObjectMap = new Dictionary<Tile, GameObject>();

        //Create a GameObject for each of our tiles, so they show visually.
        for (int x = 0; x < World.Width; x++)
        {
            for (int y = 0; y < World.Height; y++)
            {
                GameObject tile_go = new GameObject();
                Tile tile_data = World.GetTileAt(x, y);

                //Add over Tile/GO pair to the dictionary.
                tileGameObjectMap.Add(tile_data, tile_go);

                tile_go.name = "Tile_" + x + "_" + y;
                tile_go.transform.position = new Vector3(tile_data.X, tile_data.Y, 0);
                tile_go.transform.SetParent(this.transform, true);

                //Add sprite renderer, but don't bother setting a sprite
                //because all the tiles are empty right now.
                tile_go.AddComponent<SpriteRenderer>();

                tile_data.RegisterTileChangedCallback(OnTileTypeChanged);
            }
        }

        World.RandomizeTiles();
	}
	
	void Update ()
    {
	
	}

    void DestroyAllTileGameObjects()
    {
        //This function might get called when we are changing floors/levels.
        //We need to destroy all visual **GameObjects** -- but not the actual tile_data!

        while (tileGameObjectMap.Count > 0)
        {
            Tile tile_data = tileGameObjectMap.Keys.First();
            GameObject tile_go = tileGameObjectMap[tile_data];

            //Remove the pair form the map.
            tileGameObjectMap.Remove(tile_data);

            //Unregister the callback!
            tile_data.UnregisterTileChangedCallback(OnTileTypeChanged);

            //Destroy the visul GameObject.
            Destroy(tile_go);
        }
        //Presumably, after this function gets called, we'd call another
        //function to build all the GameObjects for the tiles on the new floor/level.
    }

    void OnTileTypeChanged(Tile tile_data)
    {
        if (tileGameObjectMap.ContainsKey(tile_data) == false)
        {
            Debug.LogError("tileGameObjectMap doesn't contain the tile_data -- did you forget to add the tile to the dictionary? Or maybe forget to unregister a callback.");
            return;
        }

        GameObject tile_go = tileGameObjectMap[tile_data];

        if (tile_go == null)
        {
            Debug.LogError("tileGameObjectMap returned GameObject is null -- did you forget to add the tile to the dictionary? Or maybe forget to unregister a callback.");
            return;
        }

        if (tile_data.Type == TileType.Floor)
        {
            tile_go.GetComponent<SpriteRenderer>().sprite = floorSprite;
        }
        else if (tile_data.Type == TileType.Empty)
        {
            tile_go.GetComponent<SpriteRenderer>().sprite = null;
        }
        else
        {
            Debug.LogError("OnTileTypeChanged - Unrecognized tile type.");
        }
    }

    public Tile GetTileAtWorldCoord(Vector3 coord)
    {
        int x = Mathf.FloorToInt(coord.x);
        int y = Mathf.FloorToInt(coord.y);

        return WorldController.Instance.World.GetTileAt(x, y);
    }
}
