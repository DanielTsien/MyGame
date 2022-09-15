using System;
using UnityEngine;

namespace Utility
{
    public class UpdateBehaviour : MonoBehaviour
    {
        public event Action OnUpdate;

        private void Update()
        {
            OnUpdate?.Invoke();
        }
    }
}