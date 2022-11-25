using System;
using System.Collections.Generic;
using System.Linq;
using Spine.Unity;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace SkillEditor
{
    [CustomEditor(typeof(SkillAnimationClip))]
    public class SkillAnimationClipInspector : SkillClipInspector
    {
        private SkillAnimationClip m_clip;
        private SkeletonAnimation m_skeletonAnimation;
        
        private int _selectedIndex;
        private float _sliderValue;

        private SerializedProperty animationName;
        private List<string> m_animNames;
        private List<float> m_animDurations;

        private void OnEnable()
        {
            //需要研究下 怎么用出下拉框
            animationName = serializedObject.FindProperty("_animationName");
            
            m_clip = target as SkillAnimationClip;
            m_skeletonAnimation = m_clip.actor.GetComponent<SkeletonAnimation>();
            m_skeletonAnimation.Initialize(true);
            
            m_animNames = m_skeletonAnimation.skeleton.Data.Animations.Select(anim => anim.Name).ToList();
            m_animDurations = m_skeletonAnimation.skeleton.Data.Animations.Select(anim => anim.Duration).ToList();

            _selectedIndex = m_animNames.FindIndex(anim => anim == m_clip.AnimationName);
        }

        protected override void CustomInspectorGUI()
        {
            // string[] displayedOptions = _animationClips.Select(clip => clip.name).ToArray();
            // _selectedIndex = EditorGUILayout.Popup("Clip Name", _selectedIndex, displayedOptions);
            // _clip.endTime = _clip.startTime + _animationClips[_selectedIndex].length;
            
            _selectedIndex = EditorGUILayout.Popup("Animation Name", _selectedIndex, m_animNames.ToArray());
            m_clip.AnimationName = m_animNames[_selectedIndex];
            m_clip.endTime = m_clip.startTime + m_animDurations[_selectedIndex];
            
            // _sliderValue = EditorGUILayout.Slider("time", _sliderValue, 0.0f, _clip.AnimationClip.length);
            // //_clip.AnimationClip.SampleAnimation(_clip.AnimatorTest.gameObject, _sliderValue);
            // _clip.SkeletonAnimation.AnimationName = _clip.AnimationClip.name;
        }
    }
}