using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITargetController : MonoBehaviour
{
    public bool safe = true;

    [SerializeField] private float spacing = 0f;

    private MeshRenderer meshRenderer;
    private float nextTime = 0;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void Update()
    {
        if (spacing != 0)
        {
            if (Time.time > nextTime)
            {
                safe = !safe;
                meshRenderer.material.color = safe ? Color.green : Color.red;
                nextTime = Time.time + spacing;
            }
        }
    }



}
