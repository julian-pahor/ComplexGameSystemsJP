using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class TextureWriting : MonoBehaviour
{

    public AudioClip clip;

    public float wodth, heeght;

    public Renderer projectionPlane;

    private float sampleBundle;

    // Start is called before the first frame update
    void Start()
    {
        
        float[] samples = new float[clip.samples * clip.channels];

        clip.GetData(samples, 0);

        sampleBundle = samples.Length / wodth + 1;

        Texture2D tex = new Texture2D((int)wodth, (int)heeght);

        float[] texSamples = new float[(int)wodth];

        int f = 0;

        for(int i = 0; i < samples.Length; i += (int)sampleBundle)
        {
            texSamples[f] = samples[i] * (heeght / 2);
            f++;
        }


        projectionPlane.material.mainTexture = tex;

        //Defaulting texture to have a black background
        for (int x = 0; x < wodth; x++)
        {
            for(int y = 0; y < heeght; y++)
            {
                tex.SetPixel(x, y, Color.black);
            }
        }

        Color color = Color.green;



        for (int i = 0; i < texSamples.Length; i++)
        {
            tex.SetPixel(i, (int)texSamples[i] + (int)heeght / 2, color);

            int y = 0;

            switch(texSamples[i] > 0)
            {
                case (true):
                    while (texSamples[i] + y > 0)
                    {
                        tex.SetPixel(i, (int)texSamples[i] + (int)heeght / 2 + y, color);
                        y--;
                    }

                    break;
                case (false):
                    while (texSamples[i] + y < 0)
                    {
                        tex.SetPixel(i, (int)texSamples[i] + (int)heeght / 2 + y, color);
                        y++;
                    }


                    break;
                default:
                    
            }
        }


        tex.Apply();
    }

    // Update is called once per frame
    // pee pee poo poo
    void Update()
    {
        
    }

}
