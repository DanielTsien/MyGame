using System;
using MyGame;
using DefaultNamespace;
using UnityEngine;

namespace UI
{
    public class PanelBase : MonoBehaviour, IController
    {
        private IArchitecture m_architecture;
        public IArchitecture GetArchitecture()
        {
            if (m_architecture == null)
            {
                m_architecture = Game.Interface;
            }

            return m_architecture;
        }
    }
}