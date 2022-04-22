using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndFlag : MonoBehaviour
{
    public Text rankText1; //存放1-25名
    public Text rankText2; //存放25-50名

    private ArrayList rankList = new ArrayList();
    //private string rankList1;
    //private string rankList2;

    // Start is called before the first frame update
    void Start()
    {
        //rankList1 = "";
        //rankList2 = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            string player = other.transform.Find("NameText").GetComponent<TextMesh>().text;
            rankList.Add(player);
        }

        if (other.CompareTag("Enemy"))
        {
            string enemy = other.transform.Find("NameText").GetComponent<TextMesh>().text;
            rankList.Add(enemy);
        }

        if(rankList.Count <= 25)
        {
            string rankList1 = "";
            for (int i = 0; i < rankList.Count; i++)
            {
                rankList1 += (i + 1).ToString() + "-" + rankList[i] + "\n";
            }
            rankText1.text = rankList1;
        }
        else
        {
            string rankList2 = "";
            for (int i = 25; i < rankList.Count; i++)
            {
                rankList2 += (i + 1).ToString() + "-" + rankList[i] + "\n";
            }
            rankText2.text = rankList2;
        }
    }
}
