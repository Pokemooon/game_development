using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EndPanel : Panel
{
    public EndPanelContainer success;
    public EndPanelContainer fail;
    private EndPanelContainer activePanel;

    public void Success()
    {
        activePanel = success;
        Appear(0.5f);
    }

    public void Fail()
    {
        activePanel = fail;
        Appear(0.5f);
    }

    private void Appear(float duration = 0.75f)
    {
        float targetPos = activePanel.title.anchoredPosition.y;

        activePanel.title.anchoredPosition += new Vector2(0f, 1000f);
        activePanel.continueButton.localScale = Vector3.zero;

        activePanel.self.gameObject.SetActive(true);

        activePanel.title.DOAnchorPosY(targetPos, duration);
        activePanel.continueButton.DOScale(1f, duration).SetEase(Ease.OutBack);
    }

    public void OnPressRestart()
    {
        GameManager.instance.RestartScene();
    }
}

[System.Serializable]
public struct EndPanelContainer
{
    public RectTransform self;
    public RectTransform title;
    public RectTransform continueButton;
}