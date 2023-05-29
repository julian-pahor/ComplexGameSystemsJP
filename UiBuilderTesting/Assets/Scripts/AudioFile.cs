using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "AudioManagerName/AudioFile", order = 2)]
public class AudioFile : ScriptableObject
{
    public AudioClip soundFile;
    public AudioClip audio;
    [Header("Local Volume + Pitch Settings")]
    [Range(0f, 1f)]
    public float m_volume = 1f;
    [Range(-1f, 2f)]
    public float m_pitch = 1f;

    public bool spatialPosition;

    private static float width = 2000;
    private static float height = 150;
    private float sampleBundle;

    //Waveform texture loading
    public Texture2D m_texture;
    //Fade in / Out Editing
    //Trim Capability 
    private AudioClip cacheClip;
    public AudioClip GetCacheClip()
    {
        return cacheClip;
    }
    private Texture2D cacheTex;
    public Texture2D GetCacheTex()
    {
        return cacheTex;
    }

    [ContextMenu("FillTex")]
    public void FillTex()
    {
        m_texture = new Texture2D((int)width, (int)height, TextureFormat.RGBA32, false);

        float[] samples = new float[soundFile.samples * soundFile.channels];

        soundFile.GetData(samples, 0);

        sampleBundle = samples.Length / width + 1;

        float[] texSamples = new float[(int)width];

        int f = 0;

        for (int i = 0; i < samples.Length; i += (int)sampleBundle)
        {
            texSamples[f] = samples[i] * (height / 2) * m_volume;
            f++;
        }

        Color32 black = new Color32(0, 0, 0, 255);
        Color32 orange = new Color32(255, 165, 0, 255);
        Color color = Color.magenta;

        var data = m_texture.GetRawTextureData<Color32>();
        //int audioIndex = 0;

        //drawing index = bottom left -> bottom right -> up row

        //for (int x = 0; x < m_texture.width; x++)
        //{
        //    for (int y = 0; y < m_texture.height; y++)
        //    {
        //        data[index++] = index > 125000 && index < 175000  ? black : orange;
        //    }
        //}

        for(int x = 0; x < m_texture.width - 1; x++)
        {
            for(int y = 0; y < m_texture.height - 1; y++)
            {
                int index = (y + (x * (int)height));
                data[index] = index % height == 0 ? black : orange;
            }
        }

        // https://docs.unity3d.com/ScriptReference/Texture2D.GetRawTextureData.html

        //for (int i = 0; i < texSamples.Length; i++)
        //{
        //    m_texture.SetPixel(i, (int)texSamples[i] + (int)height / 2, color);

        //    int y = 0;

        //    switch (texSamples[i] > 0)
        //    {
        //        case (true):
        //            while (texSamples[i] + y > 0)
        //            {
        //                m_texture.SetPixel(i, (int)texSamples[i]  + (int)height / 2 + y, color);
        //                y--;
        //            }

        //            break;
        //        case (false):
        //            while (texSamples[i] + y < 0)
        //            {
        //                m_texture.SetPixel(i, (int)texSamples[i] + (int)height / 2 + y, color);
        //                y++;
        //            }


        //            break;
        //        default:

        //    }
        //}

        m_texture.Apply();

        cacheTex = m_texture;

        cacheClip = soundFile;
    }
}
