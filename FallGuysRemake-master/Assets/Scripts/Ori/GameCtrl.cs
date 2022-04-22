using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCtrl : MonoBehaviour
{
    public GameObject caiDanBridge;
    public GameObject endPoint;
    public GameObject weilan;
    public GameObject threeLian;

    public static bool isLv1Open = false;
    public static bool isLv2Open = false;
    public static bool isLv3Open = false;
    public static bool isLv4Open = false;
    public static bool isLv5Open = false;
    public static bool isLv6Open = false;
    public static bool isLv7Open = false;

    public static bool isLike = false;
    public static bool isCoin = false;
    public static bool isCollect = false;

    public static int trigger1 = 0;
    public static int trigger2 = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isLike && isCoin && isCollect)
        {
            weilan.SetActive(false);
            endPoint.SetActive(true);
            isLike = isCoin = isCollect = false;
        }

        if(trigger1 == 3 && trigger2 == 3)
        {
            //打开彩蛋
            caiDanBridge.SetActive(true);
            //打开3连模块，打开终点围栏，关闭终点
            weilan.SetActive(true);
            endPoint.SetActive(false);
            threeLian.SetActive(true);

            trigger1 = 0;
            trigger2 = 0;
        }
    }
}
