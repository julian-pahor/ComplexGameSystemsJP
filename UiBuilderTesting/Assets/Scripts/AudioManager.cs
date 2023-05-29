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
        audio.spatialBlend = ai.spatial ? 1 : 0;
        audio.Play();
        StartCoroutine(CleanUp(audio));
    }

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

    }

    private IEnumerator CleanUp(AudioSource audio)
    {
        yield return new WaitUntil(() => !audio.isPlaying);
        Destroy(audio.gameObject);
    }
}
