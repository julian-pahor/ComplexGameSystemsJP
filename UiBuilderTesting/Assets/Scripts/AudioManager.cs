using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public List<AudioBundle> bundles = new List<AudioBundle>();


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
