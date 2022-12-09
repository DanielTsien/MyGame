using System;
using UnityEditor;
using UnityEngine;

namespace SkillEditor
{
    [CustomEditor(typeof(SkillEffectClip))]
    public class SkillEffectClipInspector : SkillClipInspector
    {
        private SerializedProperty m_effectGo;
        private SerializedProperty m_speed;
        private SerializedProperty m_interval;
        
        private SkillEffectClip m_clip;


        private void OnEnable()
        {
            m_clip = target as SkillEffectClip;
            m_effectGo = serializedObject.FindProperty("m_effectGo");
            m_speed = serializedObject.FindProperty("m_speed");
            m_interval = serializedObject.FindProperty("m_interval");
        }

        protected override void CustomInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(m_effectGo);
            if (EditorGUI.EndChangeCheck())
            {
                var go = m_effectGo.objectReferenceValue as GameObject;
                m_clip.EffectGo = go;
                serializedObject.ApplyModifiedProperties();
            }
            
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(m_speed);
            EditorGUILayout.PropertyField(m_interval);
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
            

            // if (m_ps != null)
            // {
            //     m_sliderValue = EditorGUILayout.Slider("time", m_sliderValue, 0.0f, m_ps.main.duration);
            //     m_ps.Simulate(m_sliderValue, true);
            //     SceneView.RepaintAll();
            // }
        }
    }
}