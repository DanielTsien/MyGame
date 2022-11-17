using Slate;
using UnityEngine;

namespace SkillEditor
{
    [Name("Action Clip")]
    [Description("Skill Action Clip")]
    [Attachable(typeof(ActionTrack))]
    public class ActionClip : SkillClip
    {
        protected override void OnUpdate(float time, float previousTime)
        {
            Debug.LogError(11111);
        }
    }
}