using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public float forceWithDoor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Door"))
        {
            //Debug.Log("Player和门发生碰撞");
            Vector3 diffDoorPlayer = (transform.position - other.transform.position).normalized;
            //Vector3 diff = new Vector3(diffDoorPlayer.x, 0f, diffDoorPlayer.z);
            other.rigidbody.AddForce(-diffDoorPlayer * forceWithDoor);
        }
    }
}
