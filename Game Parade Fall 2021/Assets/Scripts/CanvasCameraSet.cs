using UnityEngine;

public class CanvasCameraSet : MonoBehaviour
{
    void Start()
    {
        var canvas = GetComponent<Canvas>();
	    canvas.worldCamera = Camera.main;
        canvas.planeDistance = 0.35f;
    }
}
