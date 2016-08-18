using UnityEngine;
using System.Collections;

public class MouseController : MonoBehaviour {

    public GameObject circleCursor;

    Vector3 lastFramePosition;

	void Start ()
    {
	
	}

	void Update ()
    {
        Vector3 currFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currFramePosition.z = 0;

        //Update the circle cursor position.
        circleCursor.transform.position = currFramePosition;

        //Handle scree dragging.
        if (Input.GetMouseButton(2) || Input.GetMouseButton(1))
        {
            Vector3 diff = lastFramePosition - currFramePosition;
            Camera.main.transform.Translate(diff);
        }

        lastFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lastFramePosition.z = 0;
    }
}
