using System;
using UnityEditor;

namespace SkillEditor
{
    [CustomEditor(typeof(SkillEffectClip))]
    public class SkillEffectClipInspector : SkillClipInspector
    {
        private SerializedProperty m_effect;

        private void OnEnable()
        {
            m_effect = serializedObject.FindProperty("m_effect");
        }

        protected override void CustomInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(m_effect);
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}