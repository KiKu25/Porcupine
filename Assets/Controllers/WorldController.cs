using UnityEngine;
using System.Collections;
using System;

public class WorldController : MonoBehaviour {

    public Sprite floorSprite;

    World world;

	void Start () { 
        //Create the world with Empty tiles.
        world = new World();

        //Create a GameObject for each of our tiles, so they show visually.
        for (int x = 0; x < world.Width; x++)
        {
            for (int y = 0; y < world.Height; y++)
            {
                GameObject tile_go = new GameObject();
                Tile tile_data = world.GetTileAt(x, y);
                tile_go.name = "Tile_" + x + "_" + y;
                tile_go.transform.position = new Vector3(tile_data.X, tile_data.Y, 0);
                tile_go.transform.SetParent(this.transform, true);

                //Add sprite renderer, but don't bother setting a sprite
                //because all the tiles are empty right now.
                tile_go.AddComponent<SpriteRenderer>();

                tile_data.RegisterTileChangedCallback( (tile) => { OnTileTypeChanged(tile, tile_go); } );
            }
        }

        world.RandomizeTiles();
	}
	
	void Update () {
	
	}

    void OnTileTypeChanged(Tile tile_data, GameObject tile_go)
    {
        if (tile_data.Type == Tile.TileType.Floor)
        {
            tile_go.GetComponent<SpriteRenderer>().sprite = floorSprite;
        }
        else if (tile_data.Type == Tile.TileType.Empty)
        {
            tile_go.GetComponent<SpriteRenderer>().sprite = null;
        }
        else
        {
            Debug.LogError("OnTileTypeChanged - Unrecognized tile type.");
        }
    }
}
