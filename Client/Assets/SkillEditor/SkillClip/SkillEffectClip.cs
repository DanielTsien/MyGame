using System.Collections.Generic;
using Slate;
using UnityEditor;
using UnityEngine;
using Utilities;

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

        public GameObject EffectGo
        {
            get => m_effectGo;
            set
            {
                DestroyParticleSystem();
                m_effectGo = value;
            }
        }

        [SerializeField]
        private float m_speed = 1f;
        [SerializeField]
        private float m_interval = 1f; 
        
        private List<ParticleSystem> m_particleSystems = new();

        protected override void OnUpdate(float time)
        {
            int index = Mathf.FloorToInt(time / m_interval);
            for (int i = 0; i <= index; i++)
            {
                if (m_particleSystems.Count <= i)
                {
                    var ps = GetParticleSystemItem();
                    m_particleSystems.Add(ps);
                }
                m_particleSystems[i].transform.position = Vector3.down * i * m_interval * m_speed;
                m_particleSystems[i].Simulate(time - i * m_interval, true);
            }
            for (int i = index + 1; i < m_particleSystems.Count; i++)
            {
                m_particleSystems[i].Simulate(0, true);
            }
            SceneView.RepaintAll();
        }

        protected override void OnExit()
        {
            DestroyParticleSystem();
        }
        

        private void DestroyParticleSystem()
        {
            foreach (var ps in m_particleSystems)
            {
                if (ps != null)
                {
                    DestroyImmediate(ps.gameObject);
                }
            }
            m_particleSystems.Clear();
        }

        private ParticleSystem GetParticleSystemItem()
        {
            var go = Instantiate(m_effectGo);
            go.transform.Reset(true);
            return go.GetComponent<ParticleSystem>();
        }
    }
}