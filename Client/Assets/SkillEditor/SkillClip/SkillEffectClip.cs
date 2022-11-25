using Slate;
using UnityEditor;
using UnityEngine;

namespace SkillEditor
{
    [Name("Effect Clip")]
    [Description("Effect Clip")]
    [Attachable(typeof(ActionTrack))]
    public class SkillEffectClip : SkillClip
    {
        public override float length { get; set; } = 1f;

        [SerializeField]
        private GameObject m_effectGo;

        public GameObject EffectGo {
            get
            {
                return m_effectGo;
            }
            set
            {
                m_effectGo = value;
                m_ps = m_effectGo.GetComponent<ParticleSystem>();
            }
        }

        private ParticleSystem m_ps;

        protected override void OnUpdate(float time, float previousTime)
        {
            if (m_ps == null)
            {
                return;
            }
            
            m_ps.Simulate(time, true);
            SceneView.RepaintAll();
        }
    }
}