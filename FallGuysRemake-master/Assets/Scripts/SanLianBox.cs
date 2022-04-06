using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanLianBox : MonoBehaviour
{
    public GameObject gb;
    public Material mt;
    
    private MeshRenderer mr;


    // Start is called before the first frame update
    void Start()
    {
        mr = gb.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && gameObject.tag == "Like")
        {
            mr.material = mt;
            GameCtrl.isLike = true;
            Debug.Log("GameCtrl.isLike = true;");
        }

        if (other.CompareTag("Player") && gameObject.tag == "Coin")
        {
            mr.material = mt;
            GameCtrl.isCoin = true;
            Debug.Log("GameCtrl.isCoin = true;");
        }

        if (other.CompareTag("Player") && gameObject.tag == "Collect")
        {
            mr.material = mt;
            GameCtrl.isCollect = true;
            Debug.Log("GameCtrl.isCollect = true;");
        }
    }
}
