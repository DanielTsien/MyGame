using Slate;
using UnityEngine;
using Utilities;

namespace SkillEditor
{
    [Name("Collision Detection Clip")]
    [Description("Collision Detection Clip")]
    [Attachable(typeof(ActionTrack))]
    public class CollisionDetectionClip : SkillClip
    {
        //明日任务
        //碰撞检测 圆形和矩形， 可以先用unity自带
        //clip增加触发前提
        public override float length { get; set; } = 1f;
        
        public float speed;
        private CircleCollider2D m_collider;
        private ContactFilter2D m_contactFilter2D;
        public LayerMask layerMask;
        public float interval;
        public int triggerCount;

        private void Awake()
        {
            m_contactFilter2D = new ContactFilter2D
            {
                useLayerMask = true,
                useTriggers = true,
                layerMask = layerMask
            };
            m_collider = new CircleCollider2D
            {
                offset = Vector2.zero,
                radius = 1
            };
        }


        protected override void OnEnter()
        {
            m_contactFilter2D.layerMask = layerMask;
        }

        protected override void OnUpdate(float time)
        {
            if (null == m_collider)
            {
                var go = new GameObject("Col");
                go.transform.Reset();
                var addComponent = go.AddComponent<Rigidbody2D>();
                addComponent.simulated = false;
                var col = go.AddComponent<CircleCollider2D>();
                col.radius = 1;
                col.offset = Vector2.zero;
                col.isTrigger = true;
                m_collider = col;
            }
            m_collider.offset = Vector2.down * time * speed;
            if (m_collider.IsTouchingLayers())
            {
                Debug.Log(111111111111);
            }
        }

        protected override void OnAfterValidate()
        {
            base.OnAfterValidate();
        }
    }
}