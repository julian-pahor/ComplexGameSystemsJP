using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEngine;

[CustomEditor(typeof(AudioFile))]
public class AudioFileEditor : Editor
{

    public VisualTreeAsset m_UXML;

    private VisualElement root;

    public override VisualElement CreateInspectorGUI()
    {
        root = new VisualElement();

        m_UXML.CloneTree(root);

        VisualElement waveTarget = UQueryExtensions.Query(root, name = "WavePreview");

        SerializedProperty tex = serializedObject.FindProperty("m_texture");

        Texture2D waveTex = (Texture2D)tex.objectReferenceValue;

        if (waveTex != null && waveTarget != null)
        {
            waveTarget.style.backgroundImage = waveTex;
        }

        // Draw the default inspector

        var foldout = new Foldout() { viewDataKey = "AudioFileDefaultInspector", text = "Default Inspector" };
        
        InspectorElement.FillDefaultInspector(foldout, serializedObject, this);

        root.Add(foldout);

        return root;
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();


    }

}
