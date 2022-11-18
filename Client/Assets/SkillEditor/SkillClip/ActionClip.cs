using Slate;
using UnityEngine;

namespace SkillEditor
{
    [Name("Action Clip")]
    [Description("Skill Action Clip")]
    [Attachable(typeof(ActionTrack))]
    public class ActionClip : SkillClip, ISubClipContainable
    {
        public float clipOffset;

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
        
        float ISubClipContainable.subClipOffset {
            get { return clipOffset; }
            set { clipOffset = value; }
        }

        float ISubClipContainable.subClipLength {
            get { return  0; }
        }

        float ISubClipContainable.subClipSpeed {
            get { return 1; }
        }
        
        protected override void OnUpdate(float time, float previousTime)
        {
            Debug.LogError(11111);
        }
    }
}