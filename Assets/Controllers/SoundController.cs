using UnityEngine;
using System.Collections;

public class SoundController : MonoBehaviour {

    float soundCooldown = 0;

	// Use this for initialization
	void Start () {
        WorldController.Instance.World.RegisterFurnitureCreated(OnFurnitureCreated);
        WorldController.Instance.World.RegisterTileChanged(OnTileCanged);
	}

    void Update()
    {
        soundCooldown -= Time.deltaTime;
    }

    void OnTileCanged(Tile tile_data)
    {
        if (soundCooldown > 0)
            return;

        AudioClip ac = Resources.Load<AudioClip>("Sounds/Floor_OnCreated");
        AudioSource.PlayClipAtPoint(ac, Camera.main.transform.position);
        soundCooldown = 0.1f;
    }

    public void OnFurnitureCreated(Furniture furn)
    {
        if (soundCooldown > 0)
            return;

        AudioClip ac = Resources.Load<AudioClip>("Sounds/" + furn.objectType + "_OnCreated");

        if (ac == null)
        {
            ac = Resources.Load<AudioClip>("Sounds/Wall_OnCreated");
        }
        AudioSource.PlayClipAtPoint(ac, Camera.main.transform.position);
        soundCooldown = 0.1f;
    }
}
