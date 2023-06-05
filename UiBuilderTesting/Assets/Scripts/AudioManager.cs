using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public struct AudioInfo
{
    public AudioClip clip;
    public float volume;
    public float pitch;
    public bool spatial;
    public bool looping;
}

public enum CallType
{
    OnAwake,
    OnStart,
    OnStop,
    OnEnable,
    OnDisable,
    OnDestroy,
    OnColliderEnter,
    OnColliderExit,
    OnTriggerEnter,
    OnTriggerExit,
}

public class AudioManager : MonoBehaviour
{

    [HideInInspector]
    public GameObject asPreFab;

    public static AudioManager Instance;

    private void OnEnable()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Awake()
    {
        GetBundles();

        foreach(AudioFileBundle afb in AFBundles)
        {
            foreach(AudioFile af in afb.audioFiles)
            {
                Initialise(af);
            }
        }

        foreach (AudioLoopBundle alb in ALBundles)
        {
            foreach (AudioLoop af in alb.audioLoops)
            {
                Initialise(af);
            }
        }
    }

    public List<AudioFileBundle> AFBundles = new List<AudioFileBundle>();

    public List<AudioLoopBundle> ALBundles = new List<AudioLoopBundle>();

    //Used to find all created Bundles and have an easy reference to them within the AudioManager script
    [ContextMenu("GetBundles")]
    private void GetBundles()
    {
        AFBundles.Clear();
        ALBundles.Clear();

        List<AudioFileBundle> ab = new List<AudioFileBundle>();
        List<AudioLoopBundle> ab2 = new List<AudioLoopBundle>();

        string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(AudioFileBundle)));
        string[] guids2 = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(AudioLoopBundle)));

        for(int i = 0; i < guids.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
            AudioFileBundle thisBundle = AssetDatabase.LoadAssetAtPath<AudioFileBundle>(assetPath);

            if(thisBundle != null)
            {
                AFBundles.Add(thisBundle);
            }
        }

        for (int i = 0; i < guids2.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guids2[i]);
            AudioLoopBundle thisBundle = AssetDatabase.LoadAssetAtPath<AudioLoopBundle>(assetPath);

            if (thisBundle != null)
            {
                ALBundles.Add(thisBundle);
            }
        }
    }

    public void FireAS(AudioInfo ai, Transform t)
    {
        GameObject go = Instantiate(asPreFab, this.transform);
        go.transform.position = t.position;
        AudioSource audio = go.GetComponent<AudioSource>();
        audio.clip = ai.clip;
        audio.volume = ai.volume;
        audio.pitch = ai.pitch;
        audio.loop = false;
        audio.spatialBlend = ai.spatial ? 1 : 0;
        audio.Play();
        StartCoroutine(CleanUp(audio));
    }

    //Hosts a group of AudioLoops under packet name of the ScriptableObject
    //PROBLEM: Currently leaves hanging go's if needing to fire an AL multiple times
    public GameObject FireAL(AudioInfo[] ai, Transform t, string s)
    {
        GameObject packet = new GameObject(s);
        packet.transform.parent = this.transform;

        foreach(AudioInfo audio in ai)
        {
            GameObject go = Instantiate(asPreFab, packet.transform);
            go.transform.position = t.position;
            AudioSource source = go.GetComponent<AudioSource>();
            source.clip = audio.clip;
            source.volume = audio.volume;
            source.pitch = audio.pitch;
            source.loop = audio.looping; //Swapping to sample accurate looping
            source.spatialBlend = audio.spatial ? 1 : 0;
            source.Play();
            
            if(!source.loop)
            {
                StartCoroutine(CleanUp(source));
            }
        }

        return packet;
    }

    public void PauseAL(GameObject go)
    {
        foreach (Transform obj in go.transform)
        {
            obj.GetComponent<AudioSource>().Pause();
        }
    }

    public void ResumeAL(GameObject go)
    {
        foreach (Transform obj in go.transform)
        {
            obj.GetComponent<AudioSource>().UnPause();
        }
    }

    public void StopAL(GameObject go)
    {
        foreach(Transform obj in go.transform)
        {
            obj.GetComponent<AudioSource>().Stop();
        }

        Destroy(go);
    }

    //To Add:
    //Logarithmic Fading 

    //Read Clipping info and create new audio from that
    public static void Initialise(AudioLoop al)
    {
        if(!al.clipEnable)
        {
            return;
        }

        float[] samples = new float[al.soundFile.samples * al.soundFile.channels];
        al.soundFile.GetData(samples, 0);

        double startSample = al.startTime * al.soundFile.frequency;
        double endSample = al.endTime * al.soundFile.frequency;

        int newSampleLength = samples.Length - ((int)startSample * al.soundFile.channels) - ((int)endSample * al.soundFile.channels);

        float[] newSamples = new float[newSampleLength];

        int j = 0;
        for (int i = (int)startSample * al.soundFile.channels; i < samples.Length - ((int)endSample * al.soundFile.channels); i++)
        {
            newSamples[j] = samples[i];
            j++;
        }

        al.audio = AudioClip.Create(al.soundFile.name + "_audio", newSamples.Length, al.soundFile.channels, al.soundFile.frequency, false);

        al.audio.SetData(newSamples, 0);
    }

    public static void Initialise(AudioFile af)
    {
        if (!af.clipEnable)
        {
            return;
        }

        float[] samples = new float[af.soundFile.samples * af.soundFile.channels];
        af.soundFile.GetData(samples, 0);

        double startSample = af.startTime * af.soundFile.frequency;
        double endSample = af.endTime * af.soundFile.frequency;

        int newSampleLength = samples.Length - ((int)startSample * af.soundFile.channels) - ((int)endSample * af.soundFile.channels);

        float[] newSamples = new float[newSampleLength];

        int j = 0;
        for (int i = (int)startSample * af.soundFile.channels; i < samples.Length - ((int)endSample * af.soundFile.channels); i++)
        {
            newSamples[j] = samples[i];
            j++;
        }

        af.audio = AudioClip.Create(af.soundFile.name + "_audio", newSamples.Length, af.soundFile.channels, af.soundFile.frequency, false);

        af.audio.SetData(newSamples, 0);
    }

    private IEnumerator CleanUp(AudioSource audio)
    {
        yield return new WaitUntil(() => !audio.isPlaying);
        Destroy(audio.gameObject);
    }
}
