using UnityEngine;
using System.Collections.Generic;

public class MouseController : MonoBehaviour {

    public GameObject circlCursorPrefab;

    Vector3 lastFramePosition;
    Vector3 currFramePosition;

    Vector3 dragStartPosition;
    List<GameObject> dragPreviwGameObjects;


    void Start ()
    {
        dragPreviwGameObjects = new List<GameObject>();

        SimplePool.Preload(circlCursorPrefab, 100);
	}

	void Update ()
    {
        currFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currFramePosition.z = 0;

        //UpdateCursor();
        UpdateDragging();
        UpdateCameraMovement();
 
        lastFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lastFramePosition.z = 0;
    }

    //void UpdateCursor()
    //{
    //    //Update the circle cursor position.
    //    Tile tileUnderMouse = WorldController.Instance.GetTileAtWorldCoord(currFramePosition);
    //    if (tileUnderMouse != null)
    //    {
    //        circlCursor.SetActive(true);
    //        Vector3 cursorPosition = new Vector3(tileUnderMouse.X, tileUnderMouse.Y, 0);
    //        circlCursor.transform.position = cursorPosition;
    //    }
    //    else
    //    {
    //        circlCursor.SetActive(false);
    //    }
    //}

    void UpdateDragging()
    {
        //Star Drag.
        if (Input.GetMouseButtonDown(0))
        {
            dragStartPosition = currFramePosition;
        }

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
        //Clean up old drag previews
        while (dragPreviwGameObjects.Count > 0)
        {
            GameObject go = dragPreviwGameObjects[0];
            dragPreviwGameObjects.RemoveAt(0);
            SimplePool.Despawn(go);
        }

        if (Input.GetMouseButton(0))
        {
            //Display a preview of the drag area.
            for (int x = start_x; x <= end_x; x++)
            {
                for (int y = start_y; y <= end_y; y++)
                {
                    Tile t = WorldController.Instance.World.GetTileAt(x, y);
                    if (t != null)
                    {
                        //Disdplay the building hint on top of this position.
                        GameObject go = SimplePool.Spawn(circlCursorPrefab, new Vector3(x, y, 0), Quaternion.identity);
                        go.transform.SetParent(this.transform, true);
                        dragPreviwGameObjects.Add(go);
                    }
                }
            }

        }

        //End Drag.
        if (Input.GetMouseButtonUp(0))
        {
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
    }

    void UpdateCameraMovement()
    {
        //Handle scree dragging.
        if (Input.GetMouseButton(2) || Input.GetMouseButton(1))
        {
            Vector3 diff = lastFramePosition - currFramePosition;
            Camera.main.transform.Translate(diff);
        }

        Camera.main.orthographicSize -= Camera.main.orthographicSize * Input.GetAxis("Mouse ScrollWheel");

        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 3f, 25f);
    }
}
