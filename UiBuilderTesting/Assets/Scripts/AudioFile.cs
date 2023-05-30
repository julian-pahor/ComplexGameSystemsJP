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

        ///pcm data of audioclip (Range: -1<->1)
        soundFile.GetData(samples, 0);

        sampleBundle = samples.Length / width + 1;

        float[] texSamples = new float[(int)width];
        bool[] wavePix = new bool[(int)width * (int)height];
        
        int f = 0;

        //Scale down pcm data to match length of texture width
        //Also scale pcm data to range (0<->150)
        for (int i = 0; i < samples.Length; i += (int)sampleBundle)
        {
            float waa = samples[i];
            waa *= height / 2;
            waa *= m_volume;
            texSamples[f] = waa;
            f++;
        }

        //setting true at each index where the wave will be drawn
        for (int i = 0; i < texSamples.Length; i++)
        {
            wavePix[i + (int)((texSamples[i] + height / 2) * width)] = true;

            int y = 0;

            switch (texSamples[i] > 0)
            {
                case (true):
                    while (texSamples[i] + y > 0)
                    {
                        wavePix[i + (int)((texSamples[i] + height / 2 + y) * width)] = true;
                        y--;
                    }

                    break;
                case (false):
                    while (texSamples[i] + y < 0)
                    {
                        wavePix[i + (int)((texSamples[i] + height / 2 + y) * width)] = true;
                        y++;
                    }
                    break;
                default:
            }
        }

        Color32 black = new Color32(0, 0, 0, 255);
        Color32 orange = new Color32(255, 165, 0, 255);


        // https://docs.unity3d.com/ScriptReference/Texture2D.GetRawTextureData.html

        var data = m_texture.GetRawTextureData<Color32>();
        //int audioIndex = 0;

        //drawing index = bottom left -> bottom right -> up row
        for (int x = 0; x < m_texture.width; x++)
        {
            for (int y = 0; y < m_texture.height; y++)
            {
                int index = (x + (y * (int)width));
                data[index] = wavePix[index] ? orange : black;
            }
        }

        m_texture.Apply();

        cacheTex = m_texture;

        cacheClip = soundFile;
    }
}
