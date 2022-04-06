using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaiDanTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && gameObject.tag == "Trigger1")
        {
            Debug.Log("Trigger1 is OK");
            GameCtrl.trigger1++;
        }

        if (other.CompareTag("Player") && gameObject.tag == "Trigger2")
        {
            Debug.Log("Trigger2 is OK");
            GameCtrl.trigger2++;
        }
    }
}
