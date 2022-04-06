using UnityEngine;

public class DataManager : MonoBehaviour
{
    private readonly string LEVEL_DATA = "level";
    private readonly string MONEY_DATA = "money";
    private readonly string SOUND_DATA = "sound";
    private readonly string VIBRATION_DATA = "vibration";

    public int level;
    public int money;
    public bool sound;
    public bool vibration;

    #region Singleton
    public static DataManager instance = null;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
            GetDatas();
        }
        else
        {
            DestroyImmediate(this);
        }
    }
    #endregion

    private void GetDatas()
    {
        level = PlayerPrefs.GetInt(LEVEL_DATA, 1);
        money = PlayerPrefs.GetInt(MONEY_DATA, 0);
        sound = PlayerPrefs.GetInt(SOUND_DATA, 1) == 1;
        vibration = PlayerPrefs.GetInt(VIBRATION_DATA, 1) == 1;
    }

    public void SetLevel(int _level)
    {
        level = _level;
        PlayerPrefs.SetInt(LEVEL_DATA, level);
        PlayerPrefs.Save();
    }

    public void SetMoney(int _money)
    {
        money = _money;
        PlayerPrefs.SetInt(MONEY_DATA, money);
        PlayerPrefs.Save();
    }

    public void SetSound(bool isOn)
    {
        PlayerPrefs.SetInt(SOUND_DATA, isOn ? 1 : 0);
        PlayerPrefs.Save();
        sound = isOn;
    }

    public void SetVibration(bool isOn)
    {
        PlayerPrefs.SetInt(VIBRATION_DATA, isOn ? 1 : 0);
        PlayerPrefs.Save();
        vibration = isOn;
    }
}