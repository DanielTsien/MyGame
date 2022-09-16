using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Utilities
{
    public static class UGUIExtension
    {
        private static string GrayMaterialName = "UI-Gray";
        private static Material m_grayMaterial;
        
        public static void SetGray(this Graphic self, bool isGray)
        {
            if (isGray)
            {
                self.material = GetGrayMaterial();
                self.SetMaterialDirty();
            }
            else
            {
                if(self.material != null && self.material.name == GrayMaterialName)
                {
                    self.material = null;
                    self.SetMaterialDirty();
                }
            }
        }

        private static Material GetGrayMaterial()
        {
            if (m_grayMaterial == null)
            {
                m_grayMaterial = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/UI-Gray.mat");
            }
            return m_grayMaterial;
        }
    }
}