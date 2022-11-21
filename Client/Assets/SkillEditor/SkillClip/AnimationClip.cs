using Slate;
using UnityEngine;

namespace SkillEditor
{
    [Name("Animation Clip")]
    [Description("Animation Clip")]
    [Attachable(typeof(ActionTrack))]
    public class AnimationClip : SkillClip, ISubClipContainable
    {
        public override float length
        {
            get;
            set;
        } = 1f;

        public override float blendIn
        {
            get;
            set;
        }

        public override float blendOut
        {
            get;
            set;
        }
        
        public float subClipOffset { get; set; }
        public float subClipSpeed { get; }
        public float subClipLength { get; }

        public string AnimClipName;
        
        protected override void OnUpdate(float time, float previousTime)
        {
            
            Debug.LogError(2222);
        }
    }
}