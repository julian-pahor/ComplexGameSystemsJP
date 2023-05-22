using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "AudioManagerName/AudioFile", order = 2)]
public class AudioFile : ScriptableObject
{
    public AudioClip soundFile;
    [Header("Local Volume + Pitch Settings")]
    [Range(0f, 1f)]
    public float m_volume = 1f;
    [Range(-1f, 2f)]
    public float m_pitch = 1f;

    public bool spatialPosition;

    private static float width = 1500;
    private static float height = 75;
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

        var data = m_texture.GetRawTextureData<Color32>();

        int index = 0;

        for(int y = 0; y < m_texture.height; y++)
        {
            for (int x = 0; x < m_texture.width; x++)
            {
                data[index++] = black;
            }
        }

        //Defaulting texture to have a black background
        //for (int x = 0; x < width; x++)
        //{
        //    for (int y = 0; y < width; y++)
        //    {
        //        m_texture.SetPixel(x, y, Color.black);
        //    }
        //}

        Color color = Color.red;

        // https://docs.unity3d.com/ScriptReference/Texture2D.GetRawTextureData.html

        for (int i = 0; i < texSamples.Length; i++)
        {
            m_texture.SetPixel(i, (int)texSamples[i] + (int)height / 2, color);

            int y = 0;

            switch (texSamples[i] > 0)
            {
                case (true):
                    while (texSamples[i] + y > 0)
                    {
                        m_texture.SetPixel(i, (int)texSamples[i]  + (int)height / 2 + y, color);
                        y--;
                    }

                    break;
                case (false):
                    while (texSamples[i] + y < 0)
                    {
                        m_texture.SetPixel(i, (int)texSamples[i] + (int)height / 2 + y, color);
                        y++;
                    }


                    break;
                default:

            }
        }

        m_texture.Apply();

        cacheTex = m_texture;

        cacheClip = soundFile;
    }
}
