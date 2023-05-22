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

    public List<AudioFileBundle> bundles = new List<AudioFileBundle>();

    //Used to find all created Bundles and have an easy reference to them within the AudioManager script
    [ContextMenu("GetBundles")]
    private void GetBundles()
    {
        bundles.Clear();

        List<AudioFileBundle> ab = new List<AudioFileBundle>();

        string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(AudioFileBundle)));

        for(int i = 0; i < guids.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
            AudioFileBundle thisBundle = AssetDatabase.LoadAssetAtPath<AudioFileBundle>(assetPath);

            if(thisBundle != null)
            {
                bundles.Add(thisBundle);
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

    private IEnumerator CleanUp(AudioSource audio)
    {
        yield return new WaitUntil(() => !audio.isPlaying);
        Destroy(audio.gameObject);
    }
}
