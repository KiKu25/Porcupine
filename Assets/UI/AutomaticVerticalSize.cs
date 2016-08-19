using UnityEngine;
using System.Collections;

public class AutomaticVerticalSize : MonoBehaviour {

    public float childSize = 35f;

	// Use this for initialization
	void Start () {
        AdjustSize();
	}

    public void AdjustSize()
    {
        Vector2 size = this.GetComponent<RectTransform>().sizeDelta;
        size.y = this.transform.childCount * childSize;
        this.GetComponent<RectTransform>().sizeDelta = size;
    }
}
