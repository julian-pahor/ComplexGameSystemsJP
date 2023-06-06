using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "AudioManagerName/AudioLoopBundle", order = 5)]
public class AudioLoopBundle : ScriptableObject
{
    public List<AudioLoop> audioLoops = new List<AudioLoop>();

    private GameObject packet;

    public void FireAudio(Transform t)
    {
        packet = AudioManager.Instance.FireAL(GetAudio(), t, this.name);
    }

    public void PauseAudio()
    {
        if(packet != null)
        {
            AudioManager.Instance.PauseAL(packet);
        }
        else
        {
            Debug.LogWarning($"Child Packet no created for {this.name} AudioLoopBundle");
        }
    }

    public void ResumeAudio()
    {
        if (packet != null)
        {
            AudioManager.Instance.ResumeAL(packet);
        }
        else
        {
            Debug.LogWarning($"Child Packet no created for {this.name} AudioLoopBundle");
        }
    }

    public void StopAudio()
    {
        if (packet != null)
        {
            AudioManager.Instance.StopAL(packet);
            packet = null;
        }
        else
        {
            Debug.LogWarning($"Child Packet no created for {this.name} AudioLoopBundle");
        }
    }

    public AudioInfo[] GetAudio()
    {
        List <AudioInfo> list = new List <AudioInfo>();

        AudioInfo ai = new AudioInfo();

        foreach (AudioLoop loop in audioLoops)
        {
            ai.clip = loop.audio;
            ai.volume = loop.m_volume;
            ai.pitch = loop.m_pitch;
            ai.spatial = loop.spatialPosition;
            ai.looping = loop.looping;

            list.Add(ai);
        }

        return list.ToArray();
    }
}
