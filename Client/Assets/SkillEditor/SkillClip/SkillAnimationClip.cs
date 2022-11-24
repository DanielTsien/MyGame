using System;
using Slate;
using Spine.Unity;
using UnityEngine;

namespace SkillEditor
{
    [Name("Animation Clip")]
    [Description("Animation Clip")]
    [Attachable(typeof(ActionTrack))]
    public class SkillAnimationClip : SkillClip
    {
        public override float length { get; set; }
        
        private SkeletonAnimation m_skeletonAnimation;
        
        [SerializeField]
        private string _animationName;
        public string AnimationName {
            get
            {
                return _animationName;
            }
            set
            {
                _animationName = value;
                m_skeletonAnimation = actor.GetComponent<SkeletonAnimation>();
                m_skeletonAnimation.ClearState();
                m_skeletonAnimation.AnimationName = value;
            }
        }
        
        protected override void OnUpdate(float time, float previousTime)
        {
            //AnimationClip.SampleAnimation(AnimatorTest.gameObject, time);
            //SkeletonMecanim.Update();
            
            m_skeletonAnimation.Update(time - previousTime);
            m_skeletonAnimation.LateUpdate();
            Debug.Log($"time:{time}, previousTime:{previousTime}");
        }
    }
}