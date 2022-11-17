#if UNITY_EDITOR

using UnityEditor;

namespace SkillEditor
{
    public static class Commands
    {
        public static Skill CreateSkill() {
            var skill = Skill.Create();
            SkillEditorWindow.ShowWindow(skill);
            Selection.activeObject = skill;
            return skill;
        }
        
        [MenuItem("Tools/SkillEditor", false, 500)]
        public static void OpenDirectorWindow() {
            SkillEditorWindow.ShowWindow(null);
        }
    }
}

#endif