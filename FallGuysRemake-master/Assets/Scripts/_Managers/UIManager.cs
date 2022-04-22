using UnityEngine;

public class UIManager : MonoBehaviour
{
    public MainPanel mainPanel;
    public GamePanel gamePanel;
    public EndPanel endPanel;
    public TutorialPanel tutorialPanel;

    #region Singleton
    public static UIManager instance = null;
    private void Awake()
    {
        if (instance == null) instance = this;
    }
    #endregion

    private void Start()
    {
        mainPanel.Active(true);
        gamePanel.Active(false);
        endPanel.Active(false);
        tutorialPanel.Active(false);

        LevelManager.instance.startEvent.AddListener(StartGame);
        LevelManager.instance.endGameEvent.AddListener(EndGame);
    }

    public void StartGame()
    {
        gamePanel.ActiveSmooth(true);
        mainPanel.ActiveSmooth(false);
    }

    public void EndGame(bool success)
    {
        endPanel.ActiveSmooth(true);
        gamePanel.ActiveSmooth(false);

        if (success) endPanel.Success();
        else endPanel.Fail();
    }
}