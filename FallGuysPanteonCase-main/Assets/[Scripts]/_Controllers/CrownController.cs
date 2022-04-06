using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrownController : MonoBehaviour
{
    [HideInInspector] public Transform target = null;

    private void FixedUpdate()
    {
        transform.position = target != null ? target.position + (Vector3.up * 3) : Vector3.one * 255f; 
    }
}
