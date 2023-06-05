using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(menuName = "AudioManagerName/AudioBundle", order = 3)]
public class AudioFileBundle : ScriptableObject
{
    public AudioFile[] audioFiles;
    
    private int index = -1;
    public bool randomize;
    public bool neverRepeat;

    
    public float volumeMin;
    public float volumeMax;
    public float pitchMin;
    public float pitchMax;

    private AudioFile lastAF;
    private List<AudioFile> clipList = new List<AudioFile>();

    public AudioMixer targetBus;
    //Audio FX stack (if I can get it working)

    public void FireAudio(Transform t)
    {
        AudioManager.Instance.FireAS(GetAudio(), t);
    }

    public AudioInfo GetAudio()
    {
        if(index == -1)
        {
            Initialise();
        }

        AudioInfo ai = new AudioInfo();

        ai.clip = clipList[index].audio;
        ai.volume = clipList[index].m_volume * Random.Range(volumeMin, volumeMax);
        ai.pitch = clipList[index].m_pitch * Random.Range(pitchMin, pitchMax);
        ai.spatial = clipList[index].spatialPosition;

        index++;

        if(index == clipList.Count)
        {
            Initialise();
        }

        return ai;
    }

    [ContextMenu("TryRandomise")]
    private void Initialise()
    {
        index = 0;

        if (neverRepeat)
        {
            lastAF = clipList[clipList.Count - 1];
        }

        clipList.Clear();

        foreach(AudioFile af in audioFiles)
        {
            clipList.Add(af);
        }

        //Randomize List
        for(int i = 0; i < clipList.Count - 1; i ++)
        {
            int ran = Random.Range(i, clipList.Count);
            var tmp = clipList[i];
            clipList[i] = clipList[ran];
            clipList[ran] = tmp;
        }

        //Ensure Clip will not be a repeating one
        if (lastAF != null && clipList[0] == lastAF )
        {
            var tmp = clipList[0];
            clipList[0] = clipList[clipList.Count - 1];
            clipList[clipList.Count - 1] = tmp;
        }
    }

}
