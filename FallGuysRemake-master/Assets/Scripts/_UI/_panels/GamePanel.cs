using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GamePanel : Panel
{
    public Slider processSlider;
    public Text processText;
    public Text placementText;
    public GameObject placementContainer;

    public void AppaearSlider()
    {
        processSlider.transform.localScale = Vector3.zero;
        processSlider.gameObject.SetActive(true);
        processSlider.transform.DOScale(Vector3.one, 0.3f);
        placementContainer.SetActive(false);
    }
}