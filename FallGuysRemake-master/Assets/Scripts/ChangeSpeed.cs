using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSpeed : MonoBehaviour
{
    public float newSpeed1;
    public float newSpeed2;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && gameObject.tag == "ChangeSpeed1")
        {
            Debug.Log("11111111");
            other.gameObject.GetComponent<PlayerControls>().setSpeed(newSpeed1);
        }

        if (other.CompareTag("Player") && gameObject.tag == "ChangeSpeed2")
        {
            other.gameObject.GetComponent<PlayerControls>().setSpeed(newSpeed2);
            Debug.Log("22222222");
        }
    }
}