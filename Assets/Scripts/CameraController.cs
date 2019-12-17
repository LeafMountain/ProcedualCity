using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform anchor = null;
    public float speed = 1;
    public float distance = 10;

    private void Start()
    {
        transform.position = anchor.position - (transform.forward * distance);
    }

    private void Update()
    {
        transform.RotateAround(anchor.position, Vector3.up, Time.deltaTime * speed);
    }
}
