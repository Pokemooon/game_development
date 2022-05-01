using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class helloworld : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("hello world");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = this.transform.localPosition;
        pos.x += 0.02f;

        this.transform.localPosition =pos;
        
    }
}
