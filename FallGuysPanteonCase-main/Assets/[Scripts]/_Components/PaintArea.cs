using UnityEngine;

public class PaintArea : MonoBehaviour
{
    private int textureWidth;
    private int textureHeight;
    public float paintRate { get { return (float)paintedPixelCount / (float)totalPixelCount; } }
    private readonly Color c_color = new Color(0, 0, 0, 0);
    private Texture2D m_texture;
    private bool isEnabled;
    private int totalPixelCount { get { return textureWidth * textureHeight; } }
    private int paintedPixelCount;

    private void Start()
    {
        Construct(256,256,false);
    }
    public void Construct(int _width = 256,int _height = 256,bool _setLayer = true)
    {
        textureWidth = _width;
        textureHeight = _height;

        Material m_material = GetComponent<Renderer>().material;
        m_texture = new Texture2D(textureWidth, textureHeight, TextureFormat.RGBA32, false);
        for (int x = 0; x < textureWidth; x++)
        {
            for (int y = 0; y < textureHeight; y++)
            {
                m_texture.SetPixel(x, y, c_color);
            }
        }
        m_texture.Apply();

        Color c = m_material.GetColor("_DrawingColor"); c.a = 1;
        m_material.SetColor("_DrawingColor", c);
        m_material.SetTexture("_DrawingTexture", m_texture);

        isEnabled = true;
        if(GetComponent<MeshCollider>() == null)
        {
            gameObject.AddComponent<MeshCollider>();
        }
        else
        {
            GetComponent<MeshCollider>().enabled = true;
        }
    }
   
    public void PaintOn(Vector2 textureCoord, Texture2D brushTexture, Color brushColor)
    {
        if (isEnabled)
        {
            int x = (int)(textureCoord.x * textureWidth) - (brushTexture.width / 2);
            int y = (int)(textureCoord.y * textureHeight) - (brushTexture.height / 2);

            for (int i = 0; i < brushTexture.width; i++)
            {
                for (int j = 0; j < brushTexture.height; j++)
                {
                    Color existingColor = m_texture.GetPixel((x + i), (y + j));
                    float alpha = brushTexture.GetPixel(i, j).a;
                    Color result = Color.Lerp(existingColor, brushColor, alpha);
                    result.a = existingColor.a + alpha;
                    if (((x + i) >= 0 && (x + i) <= textureWidth) && ((y + j) >= 0 && (y + j) <= textureHeight))
                    {
                        m_texture.SetPixel((x + i), (y + j), result);
                        if (existingColor.a == 0f && result.a != 0) paintedPixelCount++;
                    }
                }
            }
            m_texture.Apply();
        }
    }
   
    public void RemoveOn(Vector2 textureCoord, Texture2D brushTexture)
    {
        if (isEnabled)
        {
            int x = (int)(textureCoord.x * textureWidth) - (brushTexture.width / 2);
            int y = (int)(textureCoord.y * textureHeight) - (brushTexture.height / 2);
            for (int i = 0; i < brushTexture.width; i++)
            {
                for (int j = 0; j < brushTexture.height; j++)
                {
                    Color c = brushTexture.GetPixel(i, j);
                    if (c.a > 0f)
                    {
                        m_texture.SetPixel((x + i), (y + j), c_color);
                    }
                }
            }
            m_texture.Apply();
        }
    }
}
