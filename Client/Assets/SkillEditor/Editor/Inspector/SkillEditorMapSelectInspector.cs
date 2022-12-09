using System;
using UnityEditor;
using UnityEngine;

namespace SkillEditor
{
    [CustomEditor(typeof(SkillEditorMapSelect))]
    public class SkillEditorMapSelectInspector : Editor
    {
        private SerializedProperty m_mapGo;

        private SkillEditorMapSelect m_mapSelect;
        
        private void OnEnable()
        {
            m_mapSelect = target as SkillEditorMapSelect;
            
            m_mapGo = serializedObject.FindProperty("m_map");
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(m_mapGo);
            if (EditorGUI.EndChangeCheck())
            {
                m_mapSelect.Map = m_mapGo.objectReferenceValue as GameObject;
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}