using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build;
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

    public bool clipEnable = false;
    //Second clipping (Time clipped from start + end of audio clip)
    public float startTime = 0;
    public float endTime = 0;

    private static float width = 2000;
    private static float height = 125;
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
        if(soundFile == null)
        {
            m_texture = new Texture2D((int)width, (int)height, TextureFormat.RGBA32, false);

            Color32 b = new Color32(0, 0, 0, 255);

            var dat = m_texture.GetRawTextureData<Color32>();

            for (int x = 0; x < m_texture.width; x++)
            {
                for (int y = 0; y < m_texture.height; y++)
                {
                    int index = (x + (y * (int)width));
                    dat[index] = b;
                }
            }

            m_texture.Apply();

            cacheTex = m_texture;

            cacheClip = null;

            return;
        }

        m_texture = new Texture2D((int)width, (int)height, TextureFormat.RGBA32, false);

        float[] samples = new float[soundFile.samples * soundFile.channels];

        ///pcm data of audioclip (Range: -1<->1)
        soundFile.GetData(samples, 0);

        sampleBundle = samples.Length / width + 1;

        float[] texSamples = new float[(int)width];

        int f = 0;

        var data = m_texture.GetRawTextureData<Color32>();
        // https://docs.unity3d.com/ScriptReference/Texture2D.GetRawTextureData.html

        //Scale down pcm data to match length of texture width
        //Also scale pcm data to range (0<->100)
        for (int i = 0; i < samples.Length; i += (int)sampleBundle)
        {
            float waa = samples[i];
            waa *= height / 2;
            waa *= m_volume;
            texSamples[f] = waa;
            f++;
        }

        Color32 black = new Color32(0, 0, 0, 255);
        Color32 orange = new Color32(255, 165, 0, 255);
        Color32 cyan = new Color32(154, 244, 249, 255);

        //drawing index = bottom left -> bottom right -> up row
        for (int x = 0; x < m_texture.width; x++)
        {
            int i = 0;

            bool trigger = false;

            if (texSamples[x] > 0)
            {
                //drawing from peak
                for (int y = m_texture.height - 1; y > -1; y--)
                {
                    int index = (x + (y * (int)width));

                    data[index] = y == (int)texSamples[x] + ((int)height / 2 + i) ? cyan : black;
                    
                    if(y == (int)texSamples[x] + ((int)height / 2))
                    {
                        trigger = true;
                    }
                    else if (texSamples[x] + i < 0)
                    {
                        trigger = false;
                    }

                    if (trigger)
                    {
                        i--;
                    }
                }
            }
            else if (texSamples[x] < 0)
            {
                //drawing from dip
                for (int y = 0; y < m_texture.height; y++)
                {
                    int index = (x + (y * (int)width));

                    data[index] = y == (int)texSamples[x] + (int)height / 2 + i ? cyan : black;

                    if (y == (int)texSamples[x] + ((int)height / 2))
                    {
                        trigger = true;
                    }
                    else if (texSamples[x] + i > 0)
                    {
                        trigger = false;
                    }

                    if (trigger)
                    {
                        i++;
                    }
                }
            }
            else
            {
                int half = (int)height / 2;
                //drawing a 0 sample value
                for (int y = 0; y < m_texture.height; y ++)
                {
                    int index = (x + (y * (int)width));

                    data[index] = y == half ? cyan : black;
                }
            }
        }

        m_texture.Apply();

        cacheTex = m_texture;

        cacheClip = soundFile;
    }
}
