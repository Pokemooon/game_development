using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    [HideInInspector] public UnityEvent startEvent = new UnityEvent();
    [HideInInspector] public EndGameEvent endGameEvent = new EndGameEvent();


    #region Singleton
    public static LevelManager instance = null;
    private void Awake()
    {
        if (instance == null) instance = this;
    }
    #endregion

    private void Start()
    {
        Instantiate(Resources.Load<GameObject>("levels/level-1"));
    }
    public void StartGame()
    {
        startEvent.Invoke();
    }
    public void Success()
    {
        endGameEvent.Invoke(true);
    }
    public void Fail()
    {
        endGameEvent.Invoke(false);
    }
}

public class EndGameEvent : UnityEvent<bool> { }