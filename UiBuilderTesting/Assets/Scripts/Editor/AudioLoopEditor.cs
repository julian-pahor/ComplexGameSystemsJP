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

        VisualElement volumeField = UQueryExtensions.Query(root, name = "VolumeField");

        PropertyField volumeProperty = (PropertyField)volumeField;

        VisualElement clipButton = UQueryExtensions.Query(root, name = "ClipButton");

        UnityEngine.UIElements.Button clipReset = (UnityEngine.UIElements.Button)clipButton;

        VisualElement clipToggle = UQueryExtensions.Query(root, name = "ClipEnable");

        PropertyField clipProperty = (PropertyField)clipToggle;

        clipReset.clicked += OnButton;

        volumeProperty.RegisterValueChangeCallback(ReDraw);

        clipProperty.RegisterValueChangeCallback(ReDraw);

        audioFile.RegisterCallback<ChangeEvent<Object>, VisualElement>(WaveUpdate, waveTarget);

        waveTarget.RegisterCallback<PointerDownEvent>(OnPointerDown);

        baseObj = serializedObject.targetObject as AudioLoop;

        if (baseObj.GetCacheTex() != null)
        {
            waveTarget.style.backgroundImage = baseObj.GetCacheTex();
            dirty = false;
        }
        else if (baseObj.soundFile != null)
        {
            baseObj.FillTex();
            waveTarget.style.backgroundImage = baseObj.GetCacheTex();
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

    private void OnPointerDown(PointerDownEvent evt)
    {
        //returns early if clip is not enabled
        //will need to add fadeEnable check later on
        if (!baseObj.clipEnable)
        {
            return;
        }

        float per = evt.localPosition.x / waveTarget.resolvedStyle.width;
        Debug.Log(per);

        if (evt.modifiers == EventModifiers.Shift)
        {
            if (evt.button == 0)//Left Click
            {
                //Fade Set
                EditorUtility.SetDirty(baseObj);
            }
            else if (evt.button == 1) //Right Button
            {
                //Fade Set
                EditorUtility.SetDirty(baseObj);
            }
        }
        else if (evt.modifiers == EventModifiers.None)
        {

            if (evt.button == 0)//Left Click
            {
                baseObj.SetClipStart(per);
                EditorUtility.SetDirty(baseObj);
            }
            else if (evt.button == 1) //Right Button
            {
                baseObj.SetClipEnd(per);
                EditorUtility.SetDirty(baseObj);
            }

            WaveUpdate(new ChangeEvent<Object>(), waveTarget);
        }
    }

    private void ReDraw(SerializedPropertyChangeEvent evt)
    {
        WaveUpdate(new ChangeEvent<Object>(), waveTarget);
    }

    private void OnButton()
    {
        baseObj.ResetClipping();
        WaveUpdate(new ChangeEvent<Object>(), waveTarget);
    }
}
