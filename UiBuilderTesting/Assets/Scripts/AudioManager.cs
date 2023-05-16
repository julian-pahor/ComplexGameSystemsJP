using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
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

    public List<AudioBundle> bundles = new List<AudioBundle>();

    public Texture2D tex;
    public void SetTex(Texture2D tx)
    {
        tex = tx;
    }
    public Texture2D GetTex()
    {
        return tex;
    }

    //Used to find all created Bundles and have an easy reference to them within the AudioManager script
    [ContextMenu("GetBundles")]
    private void GetBundles()
    {
        bundles.Clear();

        List<AudioBundle> ab = new List<AudioBundle>();

        string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(AudioBundle)));

        for(int i = 0; i < guids.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
            AudioBundle thisBundle = AssetDatabase.LoadAssetAtPath<AudioBundle>(assetPath);

            if(thisBundle != null)
            {
                bundles.Add(thisBundle);
            }
        }
    }
}
