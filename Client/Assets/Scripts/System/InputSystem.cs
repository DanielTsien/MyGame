using System;
using DefaultNamespace;
using UnityEngine;

namespace MyGame
{
    public class UnityInputListener : MonoBehaviour
    {
        public event Action OnFixedUpdate;
        private void FixedUpdate()
        {
            OnFixedUpdate?.Invoke();
        }
    }
    
    public interface IInputSystem : ISystem
    {
        void SetController(GameObject character);
    }
    
    public class InputSystem : SystemBase,  IInputSystem
    {
        private Vector2 m_dir;
        private GameObject m_character;
        
        protected override void OnInit()
        {
            var go = GameObject.Instantiate(new GameObject(nameof(UnityInputListener)));
            var updateBehaviour = go.AddComponent<UnityInputListener>();
            GameObject.DontDestroyOnLoad(go);

            updateBehaviour.OnFixedUpdate += OnFixedUpdate;
        }

        public void SetController(GameObject character)
        {
            m_character = character;
        }

        private void OnFixedUpdate()
        {
            if (m_character == null)
            {
                return;
            }
            MoveOperate();
        }

        private void MoveOperate()
        {
            m_dir = Vector2.zero;
            if (Input.GetKey(KeyCode.W))
            {
                m_dir += Vector2.up;
            }
            if (Input.GetKey(KeyCode.S))
            {
                m_dir += Vector2.down;
            }
            if (Input.GetKey(KeyCode.A))
            {
                m_dir += Vector2.left;
            }
            if (Input.GetKey(KeyCode.D))
            {
                m_dir += Vector2.right;
            }

            if (m_dir != Vector2.zero)
            {
                
            }
        }
    }
}