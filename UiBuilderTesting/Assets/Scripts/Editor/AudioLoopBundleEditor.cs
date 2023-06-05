using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

[CustomEditor(typeof(AudioLoopBundle))]
public class AudioLoopBundleEditor : Editor
{
    public VisualTreeAsset m_UXML;

    private VisualElement root;

    public override VisualElement CreateInspectorGUI()
    {
        root = new VisualElement();

        m_UXML.CloneTree(root);

        return root;
    }
}
