using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AudioManagerName/AudioLoopBundle", order = 5)]
public class AudioLoopBundle : ScriptableObject
{
    public List<AudioLoop> audioLoops = new List<AudioLoop>();
}
