using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Painter : MonoBehaviour
{
    private InputManager inputManager;
    public Texture2D brushTexture;
    public Color brushColor;
    private void Start()
    {
        inputManager = InputManager.instance;
    }
    private void FixedUpdate()
    {
        if (inputManager.eventData == null) return;

        if(inputManager.hit.collider != null)
        {
            PaintArea pa = inputManager.hit.collider.GetComponent<PaintArea>();
            if(pa != null)
            {
                pa.PaintOn(inputManager.hit.textureCoord, brushTexture, brushColor);
            }
        }
    }
}
