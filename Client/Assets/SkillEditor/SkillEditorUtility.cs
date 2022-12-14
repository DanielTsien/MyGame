#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Linq;
using SkillEditor;
using Slate;
using UnityEditor;
using UnityEngine;

public static class SkillEditorUtility
{
    private static string copyJson;
    private static Type copyType;
    private static IDirectable _selectedObject;
    
    public static Dictionary<AnimatedParameter, ChangedParameterCallbacks> changedParameterCallbacks = new ();

    ///<summary>Raised when directable selection change</summary>
    public static event Action<IDirectable> onSelectionChange;

    ///<summary>Raised when animation editors refresh</summary>
    public static event Action<IAnimatableData> onRefreshAllAnimationEditors;

    public struct ChangedParameterCallbacks
    {
        public Action Restore;
        public Action Commit;

        public ChangedParameterCallbacks(Action restore, Action commit)
        {
            Restore = restore;
            Commit = commit;
        }
    }

    ///<summary>Returns the current cutscene that is being edited in the editor</summary>
    public static Cutscene cutsceneInEditor
    {
        get
        {
            var editor = ReflectionTools.GetType("CutsceneEditor").RTGetFieldOrProp("current")
                .RTGetFieldOrPropValue(null);
            if (editor != null)
            {
                return editor.GetType().RTGetFieldOrProp("cutscene").RTGetFieldOrPropValue(editor) as Cutscene;
            }

            return null;
        }
    }

    ///<summary>Currently selected directable element</summary>
    public static IDirectable selectedObject
    {
        get { return _selectedObject; }
        set
        {
            //select the root cutscene which in turns display the inspector of the object within it.
            if (value != null)
            {
                UnityEditor.Selection.activeObject = value.root.context;
            }

            _selectedObject = value;
            if (onSelectionChange != null)
            {
                onSelectionChange(value);
            }
        }
    }

    ///<summary>Refresh animation editors (dopesheet, curves) of targer animatable</summary>
    public static void RefreshAllAnimationEditorsOf(IAnimatableData animatable)
    {
        if (onRefreshAllAnimationEditors != null && animatable != null)
        {
            onRefreshAllAnimationEditors(animatable);
        }
    }

    ///<summary>Returns the currently copied clip type</summary>
    public static System.Type GetCopyType()
    {
        return copyType;
    }

    ///<summary>Flush the copy data</summary>
    public static void FlushCopy()
    {
        copyType = null;
        copyJson = null;
    }

    ///<summary>Copy a clip</summary>
    public static void CopyClip(SkillClip clip)
    {
        copyJson = JsonUtility.ToJson(clip, false);
        copyType = clip.GetType();
    }

    ///<summary>Cut a clip</summary>
    public static void CutClip(SkillClip clip)
    {
        copyJson = JsonUtility.ToJson(clip, false);
        copyType = clip.GetType();
        (clip.parent as SkillTrack).DeleteAction(clip);
    }

    ///<summary>Paste a previously copied clip. Creates a new clip with copied values within the target track.</summary>
    public static SkillClip PasteClip(SkillTrack track, float time)
    {
        if (copyType != null && !string.IsNullOrEmpty(copyJson))
        {
            var clip = track.AddAction(copyType, time);
            JsonUtility.FromJsonOverwrite(copyJson, clip);
            clip.startTime = time;

            var nextAction = track.clips.FirstOrDefault(a => a.startTime > clip.startTime);
            if (nextAction != null && clip.endTime > nextAction.startTime)
            {
                clip.endTime = nextAction.startTime;
            }

            return clip;
        }

        return null;
    }


    ///<summary>Copies the object's values to editor prefs json</summary>
    public static void CopyClipValues(SkillClip clip)
    {
        var json = JsonUtility.ToJson(clip);
        EditorPrefs.SetString("Slate_CopyDirectableValuesJSON", json);
    }

    ///<summary>Pastes the object's values from editor prefs json</summary>
    public static void PasteClipValues(SkillClip clip)
    {
        var json = EditorPrefs.GetString("Slate_CopyDirectableValuesJSON");
        var wasStartTime = clip.startTime;
        var wasEndTime = clip.endTime;
        var wasBlendIn = clip.blendIn;
        var wasBlendOut = clip.blendOut;
        JsonUtility.FromJsonOverwrite(json, clip);
        clip.startTime = wasStartTime;
        clip.endTime = wasEndTime;
        clip.blendIn = wasBlendIn;
        clip.blendOut = wasBlendOut;
    }

    ///<summary>Is any object copied?</summary>
    public static bool HasCopyDirectableValues()
    {
        var json = EditorPrefs.GetString("Slate_CopyDirectableValuesJSON");
        return !string.IsNullOrEmpty(json);
    }
}
#endif
