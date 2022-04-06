using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideRankList : MonoBehaviour
{
    public GameObject rankText;
    public GameObject rankList1;
    public GameObject rankList2;

    private bool flag;

    // Start is called before the first frame update
    void Start()
    {
        flag = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            flag = !flag;
        }

        rankText.SetActive(flag);
        rankList1.SetActive(flag);
        rankList2.SetActive(flag);

    }
}
