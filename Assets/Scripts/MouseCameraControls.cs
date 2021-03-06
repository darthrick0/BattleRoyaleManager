using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCameraControls : MonoBehaviour
{
    public Vector3 storeLocationForDrag;
    public float deltaPos;
    public float dragSpeed;
    public float zoomScaler;
    public Camera thisCam;

    private void Start()
    {
        thisCam = GetComponent<Camera>();
    }


    private void Update()
    {

        if (Input.GetMouseButtonDown(2))
        {
            storeLocationForDrag = Input.mousePosition;
            deltaPos = 1;
        }

        
        if (Input.GetMouseButton(2))
        {
            var pos = thisCam.ScreenToViewportPoint(Input.mousePosition - storeLocationForDrag)*deltaPos;
            var move = new Vector3(pos.x * dragSpeed, pos.y * dragSpeed, 0);
            transform.Translate(-move, Space.World);
        }
    
        if (thisCam.orthographicSize > 1 && thisCam.orthographicSize < 10)
        { 
            thisCam.orthographicSize -= Input.mouseScrollDelta.y * zoomScaler;
        }
    
    }
}
