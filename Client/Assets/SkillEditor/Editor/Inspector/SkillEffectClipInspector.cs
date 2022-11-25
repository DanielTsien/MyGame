using System;
using UnityEditor;
using UnityEngine;

namespace SkillEditor
{
    [CustomEditor(typeof(SkillEffectClip))]
    public class SkillEffectClipInspector : SkillClipInspector
    {
        private SerializedProperty m_effectGo;
        private SkillEffectClip m_clip;
        private ParticleSystem m_ps;

        private float m_sliderValue;

        private void OnEnable()
        {
            m_clip = target as SkillEffectClip;
            m_effectGo = serializedObject.FindProperty("m_effectGo");
        }

        protected override void CustomInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(m_effectGo);
            if (EditorGUI.EndChangeCheck())
            {
                m_clip.EffectGo = m_effectGo.objectReferenceValue as GameObject;
                serializedObject.ApplyModifiedProperties();
            }
            
            m_ps = m_clip.EffectGo != null ? m_clip.EffectGo.GetComponent<ParticleSystem>() : null;
            if (m_ps != null)
            {
                m_sliderValue = EditorGUILayout.Slider("time", m_sliderValue, 0.0f, m_ps.main.duration);
                m_ps.Simulate(m_sliderValue, true);
                SceneView.RepaintAll();
            }
            
        }
    }
}