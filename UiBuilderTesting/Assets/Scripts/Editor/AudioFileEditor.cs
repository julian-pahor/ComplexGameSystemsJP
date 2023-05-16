using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEngine;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(AudioFile))]
public class AudioFileEditor : Editor
{
    public VisualTreeAsset m_UXML;

    private VisualElement root;

    public Texture2D defaultTex;

    public override VisualElement CreateInspectorGUI()
    {
        root = new VisualElement();

        m_UXML.CloneTree(root);

        VisualElement waveTarget = UQueryExtensions.Query(root, name = "WavePreview");

        VisualElement audioFile = UQueryExtensions.Query(root, name = "AudioFile");

        audioFile.RegisterCallback<ChangeEvent<Object>, VisualElement>(WaveUpdate, waveTarget);

        var obj = serializedObject.targetObject as AudioFile;

        if(obj.GetCacheTex() != null)
        {
            waveTarget.style.backgroundImage = obj.GetCacheTex();
        }
        else
        {
            waveTarget.style.backgroundImage = defaultTex;
        }

        // Draw the default inspector

        //var foldout = new Foldout() { viewDataKey = "AudioFileDefaultInspector", text = "Default Inspector" };

        //InspectorElement.FillDefaultInspector(foldout, serializedObject, this);

        //root.Add(foldout);


        return root;
    }

    void WaveUpdate(ChangeEvent<Object> evt, VisualElement waveTarget)
    {
        var baseClass = serializedObject.targetObject as AudioFile;

        AudioClip clip = (AudioClip)evt.newValue;

        if(baseClass.GetCacheClip() == clip)
        {
            return;
        }
        else
        {
            baseClass.MakeDirty();
        }

        if (!baseClass.CheckDirty())
        {
            return;
        }

        waveTarget.Clear();

        baseClass.FillTex();

        Texture2D waveTex = baseClass.m_texture;

        waveTarget.style.backgroundImage = waveTex;

        baseClass.Clean();

    }
}
