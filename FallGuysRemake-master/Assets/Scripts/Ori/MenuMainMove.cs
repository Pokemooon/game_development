using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMainMove : MonoBehaviour
{
    public GameObject menu;
    public void HiddenClick()
    {
        menu.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(250, 0, 0);
    }

    public void AppearClick()
    {
        menu.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
    }
    
}

 
