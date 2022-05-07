using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatetableController : MonoBehaviour
{
    // Start is called before the first frame update
    public float rotateSpeed = 60; //角速度，可以在unity上进行修改
    public char choose = 'z';
    private Rigidbody mRigid;
    private Vector3 moveDirection;
    private void Awake()
    {
        mRigid = GetComponent<Rigidbody>();
    }
    
    
    // Update is called once per frame
    private void FixedUpdate()
    {
       
        float x = 0, y = 0, z = 0;
        if (choose == 'x') {
            x = rotateSpeed;
        }
        else if (choose == 'y')
        {
            y = rotateSpeed;
        }
        else if (choose == 'z')
        {
            z = rotateSpeed;
        }
        moveDirection = new Vector3(x, y, z);
        Quaternion deltaRotation = Quaternion.Euler(moveDirection * Time.fixedDeltaTime);
        mRigid.MoveRotation(mRigid.rotation*deltaRotation);
    }
}
