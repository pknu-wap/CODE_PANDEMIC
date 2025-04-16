using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    Camera _camera;
    void Start()
    {
        _camera = gameObject.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
