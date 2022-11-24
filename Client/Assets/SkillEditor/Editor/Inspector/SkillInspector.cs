using System.Collections.Generic;
using System.Reflection;
using Slate;
using UnityEditor;
using UnityEngine;

namespace SkillEditor
{
    [CustomEditor(typeof(Skill))]
    public class SkillInspector : Editor
    {
        private Skill m_skill;
        private static Editor m_curDirectableEditor;
        private static Dictionary<Object, Editor> m_directableEditors = new();

        private SerializedProperty m_actor;

        private void OnEnable()
        {
            m_actor = serializedObject.FindProperty("m_actor");
            m_skill = (Skill)target;
            m_curDirectableEditor = null;
        }

        private void OnDisable()
        {
            foreach (var pair in m_directableEditors)
            {
                DestroyImmediate(pair.Value);
            }
            m_directableEditors.Clear();
            
            m_curDirectableEditor = null;
            m_skill = null;
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            
            if ( GUILayout.Button("EDIT IN SKILL EDITOR WINDOW") ) {
                SkillEditorWindow.ShowWindow(m_skill);
            }
            GUILayout.Space(5);

            serializedObject.Update();
            EditorGUILayout.PropertyField(m_actor);

            DoSkillInspector();
            DoSelectionInspector();
            
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
        }

        private void DoSkillInspector()
        {
            
        }

        private void DoSelectionInspector()
        {
            var selection = SkillEditorUtility.selectedObject as Object;
            if (m_curDirectableEditor != null && (selection == null || m_curDirectableEditor.target != selection))
            {
                var disableMethod = m_curDirectableEditor.GetType().GetMethod("OnDisable",
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic |
                    BindingFlags.FlattenHierarchy);
                disableMethod?.Invoke(m_curDirectableEditor, null);
            }

            if (selection == null)
            {
                m_curDirectableEditor = null;
                return;
            }

            Editor newEditor = null;
            if (!m_directableEditors.TryGetValue(selection, out newEditor))
            {
                m_directableEditors[selection] = newEditor = CreateEditor(selection);
            }

            if (m_curDirectableEditor != newEditor)
            {
                var enableMethod = newEditor.GetType().GetMethod("OnEnable",
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic |
                    BindingFlags.FlattenHierarchy);
                enableMethod?.Invoke(newEditor, null);
                m_curDirectableEditor = newEditor;
            }
            
            EditorTools.BoldSeparator();
            GUILayout.Space(4);
            m_curDirectableEditor.OnInspectorGUI();
        }
    }
}