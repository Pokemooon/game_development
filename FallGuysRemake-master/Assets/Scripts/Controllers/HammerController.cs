using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerController : MonoBehaviour
{
    // Start is called before the first frame update
    public float rotateSpeed = 5;
    public float count=0;
    public float rightRotate = 20;
    public float leftRotate = 20;
    public char choose = 'y';
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
        if (choose == 'x') x = rotateSpeed;
        else if (choose == 'y') y = rotateSpeed;
        else if (choose == 'z') z = rotateSpeed;
        if (count <= rightRotate && count >= -1*leftRotate)
        {
            moveDirection = new Vector3(x, y, z);
            Quaternion deltaRotation = Quaternion.Euler(moveDirection * Time.fixedDeltaTime);
            mRigid.MoveRotation(mRigid.rotation*deltaRotation);
            count += rotateSpeed * Time.fixedDeltaTime;
        }
        else
        {
            rotateSpeed = -1*rotateSpeed;
            if(count>=0)count = rightRotate;
            else count = -1*leftRotate;
        }
    }
}
