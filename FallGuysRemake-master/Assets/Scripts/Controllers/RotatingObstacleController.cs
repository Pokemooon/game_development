using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingObstacleController : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody mRigid;
    private Transform visual;
    public char choose = 'r';
    private void Awake()
    {
        mRigid = GetComponent<Rigidbody>();
        visual = transform.GetChild(0);
    }

    private void FixedUpdate()
    {
        Vector3 pos = mRigid.position;
        if(choose =='r')
            mRigid.position += Vector3.right * speed * Time.fixedDeltaTime;
        else if (choose == 'f')
            mRigid.position += Vector3.forward * speed * Time.fixedDeltaTime;
        mRigid.MovePosition(pos);
        visual.Rotate(0, 0, speed * 10 * Time.fixedDeltaTime);
    }
}
