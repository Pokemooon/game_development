using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class StepController : MonoBehaviour
{
    // Start is called before the first frame update
    public float moveSpeed = 3.0f;
    private float count = 0;
    public float lDistance = 10;
    public float rDistance = 10;
    public char choose = 'x';

    // Update is called once per frame
    void Update()
    {
        float x = 0, y = 0, z = 0;
        if (choose == 'x') x = moveSpeed * Time.deltaTime;
        else if (choose == 'y') y = moveSpeed * Time.deltaTime;
        else if (choose == 'z') z = moveSpeed * Time.deltaTime;
        if (count <= lDistance&&count>=-1*rDistance)
        {
            transform.Translate(x,y,z);
            count += moveSpeed * Time.deltaTime;
        }
        else
        {
            moveSpeed = -1 * moveSpeed;
            if (count >= 0) count = lDistance;
            else count = -1*rDistance;
        }
    }
}