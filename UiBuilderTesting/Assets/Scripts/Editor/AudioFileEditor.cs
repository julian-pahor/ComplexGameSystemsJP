using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEngine;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine.Rendering.VirtualTexturing;


[CustomEditor(typeof(AudioFile))]
public class AudioFileEditor : Editor
{ 
    public VisualTreeAsset m_UXML;

    private VisualElement root;

    public Texture2D defaultTex;

    private AudioFile baseObj;

    private bool dirty = true;

    VisualElement waveTarget;

    public override bool RequiresConstantRepaint() => dirty;

    public override VisualElement CreateInspectorGUI()
    {
        root = new VisualElement();

        m_UXML.CloneTree(root);

        waveTarget = UQueryExtensions.Query(root, name = "WavePreview");

        VisualElement audioFile = UQueryExtensions.Query(root, name = "AudioFile");

        //VisualElement volumeField = UQueryExtensions.Query(root, name = "VolumeField");

        //volume.RegisterValueChangedCallback(OnPointerUp);

        audioFile.RegisterCallback<ChangeEvent<Object>, VisualElement>(WaveUpdate, waveTarget);

        baseObj = serializedObject.targetObject as AudioFile;

        if (baseObj.GetCacheTex() != null)
        {
            waveTarget.style.backgroundImage = baseObj.GetCacheTex();
            dirty = false;
        }
        else if (baseObj.soundFile != null)
        {
            baseObj.FillTex();
            waveTarget.style.backgroundImage = baseObj.GetCacheTex();
            dirty = false;
        }
        else
        {
            waveTarget.style.backgroundImage = defaultTex;
        }

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
            dirty = true;
        }

        if (!dirty)
        {
            return;
        }

        waveTarget.Clear();

        baseObj.FillTex();

        Texture2D waveTex = baseClass.m_texture;

        waveTarget.style.backgroundImage = waveTex;

        Repaint();

        dirty = false;
    }

    void OnPointerUp(ChangeEvent<float> f)
    {
        var baseClass = serializedObject.targetObject as AudioFile;

        dirty = true;

        waveTarget.Clear();

        baseObj.FillTex();

        waveTarget.style.backgroundImage = baseClass.GetCacheTex();

        Repaint();

        dirty = false;
    }
}


