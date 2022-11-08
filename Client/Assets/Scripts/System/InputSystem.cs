using System;
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

    }
    
    public class InputSystem : SystemBase,  IInputSystem
    {
        protected override void OnInit()
        {
            var go = GameObject.Instantiate(new GameObject(nameof(UnityInputListener)));
            var updateBehaviour = go.AddComponent<UnityInputListener>();
            GameObject.DontDestroyOnLoad(go);

            updateBehaviour.OnFixedUpdate += OnFixedUpdate;
        }

        private void OnFixedUpdate()
        {
            
        }
    }
}