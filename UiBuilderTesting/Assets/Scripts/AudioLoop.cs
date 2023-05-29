using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct ALPoints
{
    public double start;
    public double duration;
}


[CreateAssetMenu(menuName = "AudioManagerName/AudioLoop", order = 4)]
public class AudioLoop : ScriptableObject
{
    public AudioClip soundFile;
    public AudioClip audio;
    [Header("Local Volume + Pitch Settings")]
    public float m_volume = 1f;
    public float m_pitch = 1f;

    public bool spatialPosition;

    public bool looping;

    public bool clipEnable;
    //Second clipping (Time clipped from start + end of audio clip)
    public float startTime = 0;
    public float endTime = 0;

    public bool fadeEnable;
    public float fadeInDuration;
    public float fadeOutDuration;

    private static float width = 1500;
    private static float height = 75;
    private float sampleBundle;

    public Texture2D m_texture;

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

    private void OnValidate()
    {
        if(clipEnable)
        {
            startTime = Mathf.Clamp(startTime, 0.0f, soundFile.length);
            endTime = Mathf.Clamp(endTime, 0.0f, soundFile.length);
        }
        
        if(fadeEnable)
        {
            fadeInDuration = Mathf.Clamp(fadeInDuration, 0.0f, soundFile.length / 2);
            fadeOutDuration = Mathf.Clamp(fadeOutDuration, 0.0f, soundFile.length / 2);
        }
    }


    //public ALPoints GetClipTimes()
    //{
    //    ALPoints points = new ALPoints();

    //    double duration = soundFile.samples / soundFile.frequency;

    //    return points;
    //}

}
