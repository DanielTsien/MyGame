using Slate;
using UnityEngine;

namespace SkillEditor
{
    [Name("Effect Clip")]
    [Description("Effect Clip")]
    [Attachable(typeof(ActionTrack))]
    public class SkillEffectClip : SkillClip
    {
        [SerializeField]
        private GameObject m_effect;

        protected override void OnEnter()
        {
            var ps = m_effect.GetComponent<ParticleSystem>();
            ps.Play(true);
        }
        
    }
}