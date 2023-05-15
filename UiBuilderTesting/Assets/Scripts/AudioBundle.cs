using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(menuName = "AudioManagerName/AudioBundle", order = 3)]
public class AudioBundle : ScriptableObject
{


    public List<AudioFile> audioFiles = new List<AudioFile>();
    public bool randomize;
    public bool neverRepeat;

    public float volumeRange;
    public float pitchRange;

    public AudioMixer targetBus;




    //Audio FX stack (if I can get it working)
    

}
