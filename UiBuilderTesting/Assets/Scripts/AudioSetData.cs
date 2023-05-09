using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSetData : MonoBehaviour
{
    public AudioSource _as;

    private AudioClip clip;

    // Start is called before the first frame update
    void Start()
    {
        clip = _as.clip;
        _as.loop = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            _as.Play();
        }

        if(Input.GetKeyDown(KeyCode.P))
        {
            float[] samples = new float[clip.samples * clip.channels];
            _as.clip.GetData(samples, 0);

            for(int i = 0; i < samples.Length - 1; i++)
            {
                samples[i] = samples[i] * 0.75f;
            }

            _as.clip.SetData(samples, 0);
        }
    }
}
