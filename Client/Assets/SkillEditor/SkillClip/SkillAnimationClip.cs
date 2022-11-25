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
        
        private bool m_isEnter = false;
        
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
            }
        }

        protected override void OnEnter()
        {
            
        }

        protected override void OnExit()
        {

        }


        protected override void OnUpdate(float time, float previousTime)
        {
            m_skeletonAnimation.AnimationState.Tracks.Items[0].TrackTime = time;
            m_skeletonAnimation.LateUpdate();
            
            //AnimationClip.SampleAnimation(AnimatorTest.gameObject, time);
            //SkeletonMecanim.Update();
            //m_skeletonAnimation.state.SetAnimation(0, AnimationName, false).TrackTime = time;
            //m_skeletonAnimation.Update(time - previousTime);
            //m_skeletonAnimation.Update(0);
        }
    }
}