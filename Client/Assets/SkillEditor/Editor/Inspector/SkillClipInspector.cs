using UnityEditor;

namespace SkillEditor
{
    [CustomEditor(typeof(SkillClip))]
    public class SkillClipInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            CustomInspectorGUI();
        }

        protected virtual void CustomInspectorGUI()
        {
            
        }
    }
}