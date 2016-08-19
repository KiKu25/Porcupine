using UnityEngine;
using System.Collections;

public class MouseController : MonoBehaviour {

    public GameObject circlCursor;

    Vector3 lastFramePosition;
    Vector3 dragStartPosition;

	void Start ()
    {
	
	}

	void Update ()
    {
        Vector3 currFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currFramePosition.z = 0;

        //Update the circle cursor position.
        Tile tileUnderMouse = GetTileAtWorldCoord(currFramePosition);
        if (tileUnderMouse != null)
        {
            circlCursor.SetActive(true);
            Vector3 cursorPosition = new Vector3(tileUnderMouse.X, tileUnderMouse.Y, 0);
            circlCursor.transform.position = cursorPosition;
        }
        else
        {
            circlCursor.SetActive(false);
        }

        //Star Drag.
        if (Input.GetMouseButtonDown(0))
        {
            dragStartPosition = currFramePosition;
        }

        //End Drag.
        if (Input.GetMouseButtonUp(0))
        {
            int start_x = Mathf.FloorToInt(dragStartPosition.x);
            int end_x = Mathf.FloorToInt(currFramePosition.x);
            int start_y = Mathf.FloorToInt(dragStartPosition.y);
            int end_y = Mathf.FloorToInt(currFramePosition.y);

            if (end_x < start_x)
            {
                int temp = end_x;
                end_x = start_x;
                start_x = temp;
            }

            if (end_y < start_y)
            {
                int temp = end_y;
                end_y = start_y;
                start_y = temp;
            }

            for (int x = start_x; x <= end_x; x++)
            {
                for (int y = start_y; y <= end_y; y++)
                {
                    Tile t = WorldController.Instance.World.GetTileAt(x, y);
                    if (t != null)
                    {
                        t.Type = Tile.TileType.Floor;
                    }
                }
            }
        }

        //Handle scree dragging.
        if (Input.GetMouseButton(2) || Input.GetMouseButton(1))
        {
            Vector3 diff = lastFramePosition - currFramePosition;
            Camera.main.transform.Translate(diff);
        }

        lastFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lastFramePosition.z = 0;
    }

    Tile GetTileAtWorldCoord(Vector3 coord)
    {
        int x = Mathf.FloorToInt(coord.x);
        int y = Mathf.FloorToInt(coord.y);

        return WorldController.Instance.World.GetTileAt(x, y);
    }
}
