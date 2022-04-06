using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownText : MonoBehaviour
{
    public float coldTime;
    public int totalTime;
    public Text countText;

    private float time;
    private bool flag;
    // Start is called before the first frame update
    void Start()
    {
        flag = true;
        time = coldTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(flag)
        {
            GameCountDown();
        }
    }

    //3.Time.deltaTime
    void GameCountDown()
    {
        if (coldTime > 0)
        {
            coldTime -= Time.deltaTime;
        }
        else
        {
            if(totalTime > 0)
            {
                //显示
                countText.text = totalTime.ToString();
                totalTime--;
                coldTime = time;
            }
            else
            {
                countText.text = "GO!";
                flag = false;
                DontShowText();
            }
        }
    }

    //1.协程
    void DontShowText()
    {
        StartCoroutine(DontShowCount(1.0f));
    }

    IEnumerator DontShowCount(float t)
    {
        yield return new WaitForSeconds(t);
        countText.text = "";
        EnemyControls.flag = true;
        PlayerControls.flag = true;
    }
}
