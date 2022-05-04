using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameData : MonoBehaviour
{
    public int param=-1;
 
    void Awake()
    {
        DontDestroyOnLoad(this);
    }
}