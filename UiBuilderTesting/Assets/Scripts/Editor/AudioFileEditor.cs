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

        //volumeField.RegisterCallback<ChangeEvent<Object>, VisualElement>(WaveUpdate, waveTarget);

        audioFile.RegisterCallback<ChangeEvent<Object>, VisualElement>(WaveUpdate, waveTarget);

        waveTarget.RegisterCallback<PointerDownEvent>(OnPointerDown);

        baseObj = serializedObject.targetObject as AudioFile;

        if (baseObj.GetCacheTex() != null)
        {
            waveTarget.style.backgroundImage = baseObj.GetCacheTex();
            dirty = false;
        }

        return root;
    }

    void WaveUpdate(ChangeEvent<Object> evt, VisualElement waveTarget)
    {
        var baseClass = serializedObject.targetObject as AudioFile;

        AudioClip clip = (AudioClip)evt.newValue;

        if(clip == null)
        {
            dirty = true;
        }

        if(baseClass.GetCacheClip() == clip && !dirty)
        {
            return;
        }
        else
        {
            dirty = true;
        }

        waveTarget.Clear();

        baseObj.FillTex();

        Texture2D waveTex = baseClass.m_texture;

        waveTarget.style.backgroundImage = waveTex;

        Repaint();

        dirty = false;
    }

    private void OnPointerDown(PointerDownEvent evt)
    {
        if(evt.button == 0)//Left Click
        {
            Debug.Log((evt.localPosition.x /waveTarget.resolvedStyle.width) * 100 + "Percentage In Clip");

        }
        else if(evt.button == 1) //Right Button
        {

        }
    }

}


