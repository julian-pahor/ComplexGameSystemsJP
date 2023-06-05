using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEngine;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine.Rendering.VirtualTexturing;


[CustomEditor(typeof(AudioLoop))]
public class AudioLoopEditor : Editor
{
    public VisualTreeAsset m_UXML;

    private VisualElement root;

    private AudioLoop baseObj;

    private bool dirty = true;

    VisualElement waveTarget;

    public override bool RequiresConstantRepaint() => dirty;

    public override VisualElement CreateInspectorGUI()
    {
        root = new VisualElement();

        m_UXML.CloneTree(root);

        waveTarget = UQueryExtensions.Query(root, name = "WavePreview");

        VisualElement audioFile = UQueryExtensions.Query(root, name = "AudioFile");

        audioFile.RegisterCallback<ChangeEvent<Object>, VisualElement>(WaveUpdate, waveTarget);

        baseObj = serializedObject.targetObject as AudioLoop;

        if (baseObj.GetCacheTex() != null)
        {
            waveTarget.style.backgroundImage = baseObj.GetCacheTex();
            dirty = false;
        }

        return root;
    }

    void WaveUpdate(ChangeEvent<Object> evt, VisualElement waveTarget)
    {
        var baseClass = serializedObject.targetObject as AudioLoop;

        AudioClip clip = (AudioClip)evt.newValue;

        if (baseClass.GetCacheClip() == clip)
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
}
