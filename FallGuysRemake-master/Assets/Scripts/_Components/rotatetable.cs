using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotatetable : MonoBehaviour
{
    public float rotateSpeed = 60; //角速度，可以在unity上进行修改
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(0, 0,rotateSpeed * Time.deltaTime, Space.Self);

        
    }
}
