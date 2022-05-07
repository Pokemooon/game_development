using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class HorizontalController : MonoBehaviour
{
    // Start is called before the first frame update
    public float moveSpeed = 3.0f;
    private float count = 0;
    public float leftDistance = 10;
    public float rightDistance = 10;
    public char choose = 'x';
    private Rigidbody mRigid;
    private Vector3 moveDirection;

    private void Awake()
    {
        mRigid = GetComponent<Rigidbody>();
    }
    
    private void FixedUpdate()
    {
        float x = 0, y = 0, z = 0;
        if (choose == 'x') x = moveSpeed;
        else if (choose == 'y') y = moveSpeed;
        else if (choose == 'z') z = moveSpeed;
        if (count <= leftDistance&&count>=-1*rightDistance)
        {
            // transform.Translate(x,y,z);
            moveDirection = new Vector3(x, y, z);
            mRigid.MovePosition(transform.position+moveDirection*Time.fixedDeltaTime);
            count += moveSpeed * Time.fixedDeltaTime;
        }
        else
        {
            moveSpeed = -1 * moveSpeed;
            if (count >= 0) count = leftDistance;
            else count = -1*rightDistance;
        }
    }
}
