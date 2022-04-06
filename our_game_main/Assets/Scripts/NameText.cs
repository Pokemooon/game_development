using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameText : MonoBehaviour
{
    public GameObject nameText;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(nameText != null)
        {
            nameText.transform.LookAt(Camera.main.transform.position);
            nameText.transform.Rotate(0, 180, 0);
        }

    }
}
