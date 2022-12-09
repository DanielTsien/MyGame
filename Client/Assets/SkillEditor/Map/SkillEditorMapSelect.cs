using UnityEngine;

namespace SkillEditor 
{
    public class SkillEditorMapSelect : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_map;

        private GameObject m_mapGo;
        public GameObject Map
        {
            get => m_map;
            set
            {
                if (m_mapGo != null)
                {
                    DestroyImmediate(m_mapGo, true);
                }
                m_map = value;
                m_mapGo = Instantiate(m_map, MapRoot);
            }
        }

        private Transform m_mapRoot;

        public Transform MapRoot
        {
            get
            {
                if (null == m_mapRoot)
                {
                    m_mapRoot = transform.Find("Map");
                }

                return m_mapRoot;
            }
        }
    }
}