using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunTime : MonoBehaviour
{
    private Transform sunTransform;

    private void Start()
    {
        sunTransform = GetComponent<Transform>();
    }

    void Update()
    {
        sunTransform.RotateAround(Vector3.zero, Vector3.right, 1f * Time.deltaTime);
    }
}
