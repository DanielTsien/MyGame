using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using Slate;
using UnityEngine;

namespace SkillEditor
{
    public class Skill : MonoBehaviour, IDirector
    {

        ///<summary>What happens when skill stops</summary>
        public enum StopMode
        {
            Skip,
            Rewind,
            Hold,
            SkipRewindNoUndo
        }

        ///<summary>Update modes for skill</summary>
        public enum UpdateMode
        {
            Normal,
            AnimatePhysics,
            UnscaledTime,
            Manual
        }

        ///<summary>Raised when any skill starts playing.</summary>
        public static event Action<Skill> OnSkillStarted;

        ///<summary>Raised when any skill stops playing.</summary>
        public static event Action<Skill> OnSkillStopped;


        ///<summary>Raised when a cutscene section has been reached.</summary>
        public event Action<Section> OnSectionReached;

        ///<summary>Raised when a global message has been send by this skill.</summary>
        public event Action<string, object> OnGlobalMessageSend;


        ///<summary>Raised when the skill is stopped. Important: Subscribers are cleared once the event is raised.</summary>
        public event Action OnStop;

        private UpdateMode _updateMode;
        private StopMode _defaultStopMode;
        private float _playbackSpeed = 1f;
        private bool _playOnStart;
        private bool _explicitActiveLayers;
        private LayerMask _activeLayers = -1;

        public List<SkillTrack> tracks = new();

        private float _length = 20f;
        private float _viewTimeMin = 0f;
        private float _viewTimeMax = 21f;


        private float _currentTime;
        private float _playTimeMin;
        private float _playTimeMax;

        private List<IDirectableTimePointer> timePointers;
        private List<IDirectableTimePointer> unsortedStartTimePointers;
        private Dictionary<GameObject, bool> affectedLayerGOStates;
        private static Dictionary<string, Skill> allSkills = new();
        private bool preInitialized;
        private bool _isReSampleFrame;

        [SerializeField]
        private GameObject m_actor;
        public GameObject Actor
        {
            get => m_actor;
            set
            {
                m_actor = value;
            }
        }

        public UpdateMode updateMode
        {
            get { return _updateMode; }
            set { _updateMode = value; }
        }

        public StopMode defaultStopMode
        {
            get { return _defaultStopMode; }
            set { _defaultStopMode = value; }
        }

        public bool playOnStart
        {
            get { return _playOnStart; }
            set { _playOnStart = value; }
        }

        public bool explicitActiveLayers
        {
            get { return _explicitActiveLayers; }
            set { _explicitActiveLayers = value; }
        }

        public LayerMask activeLayers
        {
            get { return _activeLayers; }
            set { _activeLayers = value; }
        }

        public float currentTime
        {
            get { return _currentTime; }
            set { _currentTime = Mathf.Clamp(value, 0, length); }
        }

        public float length
        {
            get { return _length; }
            set { _length = Mathf.Max(value, 0.1f); }
        }

        public float viewTimeMin
        {
            get { return _viewTimeMin; }
            set
            {
                if (viewTimeMax > 0) _viewTimeMin = Mathf.Min(value, viewTimeMax - 0.25f);
            }
        }

        public float viewTimeMax
        {
            get { return _viewTimeMax; }
            set { _viewTimeMax = Mathf.Max(value, viewTimeMin + 0.25f, 0); }
        }

        public float playTimeMin
        {
            get { return _playTimeMin; }
            set { _playTimeMin = Mathf.Clamp(value, 0, playTimeMax); }
        }

        public float playTimeMax
        {
            get { return _playTimeMax; }
            set { _playTimeMax = Mathf.Clamp(value, playTimeMin, length); }
        }

        public float playbackSpeed
        {
            get { return _playbackSpeed; }
            set { _playbackSpeed = value; }
        }

        public List<IDirectable> directables { get; private set; }

        ///<summary>Is skill playing? (Note: it can be paused and isActive still be true)</summary>
        public bool isActive { get; private set; }

        ///<summary>Is skill paused?</summary>
        public bool isPaused { get; private set; }

        ///<summary>The last sampled time</summary>
        public float previousTime { get; private set; }

        ///<summary>internal use. will be true when Sampling due to ReSample call </summary>
        bool IDirector.isReSampleFrame => _isReSampleFrame;

        ///<summary>internal use. check this null for UnityObject comparer</summary>
        GameObject IDirector.context => this != null ? this.gameObject : null;

        ///<summary>The tracks</summary>
        IEnumerable<IDirectable> IDirector.children => tracks.Cast<IDirectable>();

        ///<summary>The remaining playing time.</summary>
        public float remainingTime => playTimeMax - currentTime;

        ///<summary>The root on which groups are added for organization</summary>
        private Transform _tracksRoot;
        public Transform TracksRoot {
            get
            {
                if ( _tracksRoot == null ) {
                    _tracksRoot = transform.Find("__TracksRoot__");
                    if ( _tracksRoot == null ) {
                        _tracksRoot = new GameObject("__TracksRoot__").transform;
                        _tracksRoot.SetParent(this.transform);
                    }

#if UNITY_EDITOR
                    //don't show in hierarchy
                    _tracksRoot.gameObject.hideFlags = Prefs.showTransforms ? HideFlags.None : HideFlags.HideInHierarchy;
#endif
                    _tracksRoot.gameObject.SetActive(false); //we dont need it or it's children active at all
                }

                return _tracksRoot;
            }
        }

        #region UNTIY CALLBACK

        protected void Awake()
        {
            Validate();
            allSkills[name] = this;
        }

        protected void Start()
        {
            if (playOnStart)
            {
                Play();
            }
        }

        protected void OnDestroy()
        {
            if (isActive)
            {
                Stop(StopMode.Rewind);
            }

            StopAllCoroutines();
            isActive = false;
            allSkills.Remove(name);
            foreach (var directable in directables)
            {
                directable.RootDestroyed();
            }
        }

        protected void LateUpdate()
        {
            if (isActive && (updateMode == UpdateMode.Normal || updateMode == UpdateMode.UnscaledTime))
            {
                if (isPaused)
                {
                    Sample();
                    return;
                }

                var dt = updateMode == UpdateMode.Normal ? Time.deltaTime : Time.unscaledDeltaTime;
                UpdateSkill(dt);
            }
        }

        protected void FixedUpdate()
        {
            if (isActive && updateMode == UpdateMode.AnimatePhysics)
            {
                if (isPaused)
                {
                    Sample();
                    return;
                }

                UpdateSkill(Time.fixedDeltaTime);
            }
        }

        #endregion


        public IEnumerable<GameObject> GetAffectedActors()
        {
            throw new System.NotImplementedException();
        }

        ///<summary>Get the start/end times of all directables, optionally excluding a specific directable</summary>
        public float[] GetPointerTimes()
        {
            if (timePointers == null)
            {
                InitializeTimePointers();
            }

            return timePointers.Select(t => t.time).ToArray();
        }

        public void Play() => Play(0);
        public void Play(System.Action callback) => Play(0, callback);
        public void Play(float startTime) => Play(startTime, length);
        public void Play(float startTime, Action callback) => Play(startTime, length, callback);

        public void Play(float startTime, float endTime, Action callback = null)
        {
            if (startTime > endTime)
            {
                Debug.LogError("End Time must be greater than Start Time.", gameObject);
                return;
            }

            if (isPaused)
            {
                //if it's paused resume.
                Debug.LogWarning("Play called on a Paused Skill. Skill will now resume instead.", gameObject);
                Resume();
                return;
            }

            if (isActive)
            {
                Debug.LogWarning("Skill is already Running.", gameObject);
                return;
            }

            playTimeMin = 0; //for mathf.clamp setter

            playTimeMax = endTime;
            playTimeMin = startTime;
            currentTime = startTime;

            if (currentTime >= playTimeMax)
            {
                currentTime = playTimeMin;
            }

            isActive = true;
            isPaused = false;

            OnStop = callback ?? OnStop;

            Sample(); //immediately do a preliminary sample first at wherever the currentTime currently is at.

            SendGlobalMessage("OnCutsceneStarted", this);
            OnSkillStarted?.Invoke(this);
        }

        public void Stop()
        {
            Stop(defaultStopMode);
        }

        public void Stop(StopMode stopMode)
        {

            if (!isActive)
            {
                return;
            }

            isActive = false;
            isPaused = false;

            if (stopMode == StopMode.Skip)
            {
                Sample(playTimeMax);
            }

            if (stopMode == StopMode.Rewind)
            {
                Sample(playTimeMin);
            }

            if (stopMode == StopMode.Hold)
            {
                Sample();
            }

            if (stopMode == StopMode.SkipRewindNoUndo)
            {
                Sample(playTimeMax);
                RewindNoUndo();
            }

            SendGlobalMessage("OnCutsceneStopped", this);
            OnSkillStopped?.Invoke(this);

            if (OnStop != null)
            {
                OnStop();
                OnStop = null;
            }
        }

        public void Pause() => isPaused = true;
        public void Resume() => isPaused = false;

        ///<summary>Skip the skill to the end</summary>
        public void SkipAll()
        {
            if (isActive) Stop(StopMode.Skip);
            else Sample(length);
        }

        ///<summary>Rewinds the cutscene to it's initial 0 time state without undoing anything, thus keeping current state as finalized.</summary>
        public void RewindNoUndo()
        {
            if (isActive)
            {
                Stop(StopMode.Hold);
            }

            currentTime = 0;
            previousTime = currentTime; //this is why no undo is happening
            Sample();
        }

        public void Sample() => Sample(currentTime);

        public void Sample(float time)
        {
            currentTime = time;

            //ignore same minmax times
            if ((currentTime == 0 || currentTime == length) && previousTime == currentTime)
            {
                return;
            }

            //Initialize time pointers if required.
            if (!preInitialized && currentTime > 0 && previousTime == 0)
            {
                InitializeTimePointers();
            }

            //Sample started
            if (currentTime > 0 && currentTime < length && (previousTime == 0 || previousTime == length))
            {
                OnSampleStarted();
            }

            //Sample pointers
            if (timePointers != null)
            {
                Internal_SamplePointers(currentTime, previousTime);
            }

            //Sample ended
            if ((currentTime == 0 || currentTime == length) && previousTime > 0 && previousTime < length)
            {
                OnSampleEnded();
            }

            previousTime = currentTime;
        }

        //Samples the initialized pointers.
        void Internal_SamplePointers(float currentTime, float previousTime)
        {
            //Update timePointers triggering forwards
            if (!Application.isPlaying || currentTime > previousTime)
            {
                for (var i = 0; i < timePointers.Count; i++)
                {
                    try
                    {
                        timePointers[i].TriggerForward(currentTime, previousTime);
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogException(e);
                    }
                }
            }

            //Update timePointers triggering backwards
            if (!Application.isPlaying || currentTime < previousTime)
            {
                for (var i = timePointers.Count - 1; i >= 0; i--)
                {
                    try
                    {
                        timePointers[i].TriggerBackward(currentTime, previousTime);
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogException(e);
                    }
                }
            }

            //Update timePointers
            if (unsortedStartTimePointers != null)
            {
                for (var i = 0; i < unsortedStartTimePointers.Count; i++)
                {
                    try
                    {
                        unsortedStartTimePointers[i].Update(currentTime, previousTime);
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogException(e);
                    }
                }
            }
        }

        public void ReSample()
        {

            if (Application.isPlaying)
            {
                return;
            }

            if (currentTime > 0 && currentTime < length && timePointers != null)
            {
                _isReSampleFrame = true;

#if UNITY_EDITOR
                List<SkillEditorUtility.ChangedParameterCallbacks> cache = null;
                if (!Prefs.autoKey)
                {
                    cache = SkillEditorUtility.changedParameterCallbacks.Values.ToList();
                }
#endif

                Internal_SamplePointers(0, currentTime);
                Internal_SamplePointers(currentTime, 0);

#if UNITY_EDITOR
                if (!Prefs.autoKey && cache != null)
                {
                    foreach (var param in cache)
                    {
                        param.Restore();
                    }
                }
#endif

                _isReSampleFrame = false;
            }
        }

        public void Validate()
        {
            if (TracksRoot.transform.parent != transform)
            {
                TracksRoot.transform.parent = transform;
            }

            directables = new List<IDirectable>();
            foreach (IDirectable track in tracks.AsEnumerable().Reverse())
            {
                directables.Add(track);
                try
                {
                    track.Validate(this, null);
                }
                catch (System.Exception e)
                {
                    Debug.LogException(e);
                }

                foreach (IDirectable clip in track.children)
                {
                    directables.Add(clip);
                    try
                    {
                        clip.Validate(this, track);
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogException(e);
                    }
                }
            }
        }

        //Initialize the time pointers (in/out). Bottom to top.
        //Time pointers dectate all directables execution order. All pointers are collapsed into a list and ordered by their time.
        //Reverse() is used for in case pointers have same time. This is mostly true for groups and tracks.
        //(Group Enter -> Track Enter -> Clip Enter | Clip Exit -> Track Exit -> Group Exit)
        void InitializeTimePointers()
        {
            timePointers = new List<IDirectableTimePointer>();
            unsortedStartTimePointers = new List<IDirectableTimePointer>();

            foreach (IDirectable track in tracks.AsEnumerable().Reverse())
            {
                if (track.isActive && track.Initialize())
                {
                    var p1 = new StartTimePointer(track);
                    timePointers.Add(p1);

                    foreach (IDirectable clip in track.children)
                    {
                        if (clip.isActive && clip.Initialize())
                        {
                            var p2 = new StartTimePointer(clip);
                            timePointers.Add(p2);

                            unsortedStartTimePointers.Add(p2);
                            timePointers.Add(new EndTimePointer(clip));
                        }
                    }
                    unsortedStartTimePointers.Add(p1);
                    timePointers.Add(new EndTimePointer(track));
                }
            }

            timePointers = timePointers.OrderBy(p => p.time).ToList();
        }

        //When Sample begins
        void OnSampleStarted()
        {
            SetLayersActive();
            if (DirectorCamera.current == null)
            {
                Debug.LogWarning(
                    "Director Camera is null. Have you disabled the AutoCreateDirectorCamera in Preferences?");
            }

            if (DirectorGUI.current)
            {
                DirectorGUI.current.enabled = true;
            }

            for (var i = 0; i < directables.Count; i++)
            {
                try
                {
                    directables[i].RootEnabled();
                }
                catch (System.Exception e)
                {
                    Debug.LogException(e, gameObject);
                }
            }
#if UNITY_EDITOR
            transform.hideFlags = HideFlags.NotEditable;
#endif
        }

        //When Sample ends
        void OnSampleEnded()
        {
            RestoreLayersActive();
            if (DirectorGUI.current)
            {
                DirectorGUI.current.enabled = false;
            }

            for (var i = 0; i < directables.Count; i++)
            {
                try
                {
                    directables[i].RootDisabled();
                }
                catch (System.Exception e)
                {
                    Debug.LogException(e, gameObject);
                }
            }
#if UNITY_EDITOR
            transform.hideFlags = HideFlags.None;
#endif
        }


        //use of active layers to toggle root object on or off during skill
        void SetLayersActive()
        {
            if (explicitActiveLayers)
            {
                var rootObjects = this.gameObject.scene.GetRootGameObjects();
                affectedLayerGOStates = new Dictionary<GameObject, bool>();
                for (var i = 0; i < rootObjects.Length; i++)
                {
                    var o = rootObjects[i];
                    affectedLayerGOStates[o] = o.activeInHierarchy;
                    o.SetActive((activeLayers.value & (1 << o.layer)) > 0);
                }
            }
        }

        //restore layer object states.
        void RestoreLayersActive()
        {
            if (affectedLayerGOStates != null)
            {
                foreach (var pair in affectedLayerGOStates)
                {
                    if (pair.Key != null)
                    {
                        pair.Key.SetActive(pair.Value);
                    }
                }
            }
        }

        ///<summary>Update the skill by delta</summary>
        void UpdateSkill(float delta)
        {

            //update time
            delta *= playbackSpeed;
            currentTime += delta;

            if (currentTime >= playTimeMax)
            {
                Stop();
                return;
            }

            //finally sample
            Sample();
        }

        ///<summary>Play a skill of specified name that exists either in the Resources folder or in the scene. In that order.</summary>
        public static Skill Play(string name, Action callback = null)
        {
            var skill = FindFromResources(name);
            if (skill != null)
            {
                var instance = (Skill) Instantiate(skill);
                Debug.Log("Instantiating Skill from Resources");
                instance.Play(() =>
                {
                    Destroy(instance.gameObject);
                    Debug.Log("Instantiated Skill Destroyed");
                    if (callback != null)
                    {
                        callback();
                    }
                });
                return skill;
            }

            skill = Find(name);
            if (skill != null)
            {
                skill.Play(callback);
                return skill;
            }

            return null;
        }

        ///<summary>Find a skill from Resources folder</summary>
        public static Skill FindFromResources(string name)
        {
            var go = Resources.Load(name, typeof(GameObject)) as GameObject;
            if (go != null)
            {
                var skill = go.GetComponent<Skill>();
                if (skill != null)
                {
                    return skill;
                }
            }

            Debug.LogWarning($"Skill of name '{name}' does not exists in the Resources folder");
            return null;
        }

        ///<summary>Find a cutscene of specified name that exists in the scene</summary>
        public static Skill Find(string name)
        {
            if (allSkills.TryGetValue(name, out Skill skill))
            {
                return skill;
            }

            Debug.LogError($"Skill of name '{name}' does not exists in the scene");
            return null;
        }

        ///<summary>Stop all running skills found in the scene</summary>
        public static void StopAllSkills()
        {
            foreach (var skill in FindObjectsOfType<Skill>())
            {
                if (skill.isActive)
                {
                    skill.Stop();
                }
            }
        }

        public void SendGlobalMessage(string message, object value)
        {
            this.gameObject.SendMessage(message, value, SendMessageOptions.DontRequireReceiver);
            foreach (var actor in GetAffectedActors())
            {
                if (actor != null)
                {
                    actor.SendMessage(message, value, SendMessageOptions.DontRequireReceiver);
                }
            }

            if (OnGlobalMessageSend != null)
            {
                OnGlobalMessageSend(message, value);
            }

#if UNITY_EDITOR
            Debug.Log(string.Format("<b>({0}) Global Message Send:</b> '{1}' ({2})", name, message, value), gameObject);
#endif
        }

        ///<summary>Set the target actor of an Actor Group by the group's name.</summary>
        public void SetGroupActorOfName(string groupName, GameObject newActor)
        {

            if (currentTime > 0)
            {
                Debug.LogError("Setting a Group Actor is only allowed when the Cutscene is not active and is rewinded",
                    gameObject);
                return;
            }

            var track = tracks.OfType<SkillTrack>().FirstOrDefault(g => g.name.ToLower() == groupName.ToLower());
            if (track == null)
            {
                Debug.LogError(string.Format("Actor Group with name '{0}' doesn't exist in cutscene", groupName),
                    gameObject);
                return;
            }
        }

        ///<summary>Providing a path to the element in the order of Group->Track->Clip, like for example ("MyGroup/MyTrack/MyClip"), returns that element.</summary>
        public T FindElement<T>(string path) where T : IDirectable
        {
            return (T) FindElement(path);
        }

        ///<summary>Providing a path to the element in the order of Group->Track->Clip, like for example ("MyGroup/MyTrack/MyClip"), returns that element.</summary>
        public IDirectable FindElement(string path)
        {
            var split = path.Split('/');
            var result = tracks.FirstOrDefault(g => g.name.ToLower() == split[0].ToLower()) as IDirectable;
            if (result != null)
            {
                for (var i = 1; i < split.Length; i++)
                {
                    result = result.FindChild(split[i]);
                    if (result == null)
                    {
                        break;
                    }
                }
            }

            if (result == null)
            {
                Debug.LogWarning(string.Format("Cutscene element path to '{0}', was not found", path));
            }

            return result;
        }

        //...
        public override string ToString()
        {
            return string.Format("'{0}' Cutscene", name);
        }

        ///<summary>Get all names of SendGlobalMessage ActionClips</summary>
        public string[] GetDefinedEventNames()
        {
            return directables.OfType<IEvent>().Select(d => d.name).ToArray();
        }

        ///<summary>By default cutscene is initialized when it starts playing. You can pre-initialize it if you want so for performance in case there is any lag when cutscene is started.</summary>
        public void PreInitialize()
        {
            InitializeTimePointers();
            preInitialized = true;
        }


        ///<summary>Render the cutscene to an image sequence in runtime and get a Texture2D[] of the rendered frames. This operation will take several frames to complete. Use the callback parameter to get the result when rendering is done.</summary>
        public void RenderCutscene(int width, int height, int frameRate, System.Action<Texture2D[]> callback)
        {

            if (!Application.isPlaying)
            {
                Debug.LogError("Rendering Cutscene with RenderCutscene function is only meant for runtime", this);
                return;
            }

            if (isActive)
            {
                Debug.LogWarning(
                    "You called RenderCutscene to an actively playing Cutscene. The cutscene will now Stop.", this);
                Stop();
            }

            StartCoroutine(Internal_RenderCutscene(width, height, frameRate, callback));
        }

        //runtime rendering to Texture2D[]
        IEnumerator Internal_RenderCutscene(int width, int height, int frameRate, Action<Texture2D[]> callback)
        {
            var renderSequence = new List<Texture2D>();
            var sampleRate = 1f / frameRate;
            for (var i = sampleRate; i <= length; i += sampleRate)
            {
                Sample(i);
                yield return new WaitForEndOfFrame();
                var texture = new Texture2D(width, height, TextureFormat.RGB24, false);
                texture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
                texture.Apply();
                renderSequence.Add(texture);
            }

            callback(renderSequence.ToArray());
        }


        ///----------------------------------------------------------------------------------------------
        ///---------------------------------------UNITY EDITOR-------------------------------------------
#if UNITY_EDITOR

        [ContextMenu("Reset")] //override
        void Reset()
        {
            
        }

        [ContextMenu("Copy Component")] //override
        void CopyComponent()
        {
        }

        [ContextMenu("Remove Component")] //override
        void RemoveComponent()
        {
            Debug.LogWarning("Removing the Cutscene Component is not possible. Please delete the GameObject instead");
        }

        [ContextMenu("Show Transforms")]
        void ShowTransforms()
        {
            Prefs.showTransforms = true;
        }

        [ContextMenu("Hide Transforms")]
        void HideTransforms()
        {
            Prefs.showTransforms = false;
        }

        //UNITY CALLBACK
        protected void OnValidate()
        {
            if (!UnityEditor.EditorUtility.IsPersistent(this) && !Application.isPlaying &&
                !UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
            {
                Validate();
            }
            // UnityEditor.SceneManagement.EditorSceneManager.preventCrossSceneReferences = false;
        }

        //UNITY CALLBACK
        protected void OnApplicationQuit()
        {
            if (currentTime > 0)
            {
                Stop(StopMode.Rewind);
            }
        }

        //UNITY CALLBACK
        protected void OnDrawGizmos()
        {
            var l = Prefs.gizmosLightness;
            Gizmos.color = new Color(l, l, l);
            Gizmos.DrawSphere(transform.position, 0.025f);
            Gizmos.color = Color.white;

            if (SkillEditorUtility.cutsceneInEditor == this)
            {
                for (var i = 0; i < directables.Count; i++)
                {
                    var directable = directables[i];
                    directable.DrawGizmos(SkillEditorUtility.selectedObject == directable);
                }
            }
        }


        public static Skill Create(Transform parent = null)
        {
            var go = new GameObject("New Skill");
            var skill = go.AddComponent<Skill>();

            if (parent != null)
            {
                skill.transform.SetParent(parent, false);
            }

            skill.transform.localPosition = Vector3.zero;
            skill.transform.localRotation = Quaternion.identity;
            return skill;
        }
        
        ///<summary>Add a group to the cutscene.</summary>
        public T AddTrack<T>(GameObject targetActor = null) where T : SkillTrack { return (T)AddTrack(typeof(T), targetActor); }

        public SkillTrack AddTrack(System.Type type, GameObject targetActor = null)
        {
            if ( !CanAddTrackOfType(type) ) {
                return null;
            }
            
            var go = new GameObject(type.Name.SplitCamelCase());
            
            UnityEditor.Undo.RegisterCreatedObjectUndo(go, "New Track");
            var newTrack = UnityEditor.Undo.AddComponent(go, type) as SkillTrack;
            UnityEditor.Undo.SetTransformParent(newTrack.transform, TracksRoot, "New Track");
            UnityEditor.Undo.RegisterCompleteObjectUndo(this, "New Track");
            newTrack.transform.localPosition = Vector3.zero;

            var index = 0;
            
            // //well thats a bit of special case. I really want CameraTrack to stay on top :)
            // if ( tracks.FirstOrDefault() is CameraTrack ) { index = 1; }
            
            
            tracks.Insert(index, newTrack);

            newTrack.PostCreate(this);
            Validate();
            CutsceneUtility.selectedObject = newTrack;
            return newTrack;
        }
        
        ///<summary>Can track type be added in this skill?</summary>
        public bool CanAddTrackOfType(Type type) {
            if ( type == null || !type.IsSubclassOf(typeof(SkillTrack))) {
                return false;
            }
            if ( type.IsDefined(typeof(UniqueElementAttribute), true) && tracks.FirstOrDefault(t => t.GetType() == type) != null ) {
                return false;
            }
            
            return true;
        }
        
        ///<summary>Can track be added in this group?</summary>
        public bool CanAddTrack(SkillTrack track) {
            return track != null && CanAddTrackOfType(track.GetType());
        }
        
        ///<summary>Duplicate the track in this skill</summary>
        public SkillTrack DuplicateTrack(SkillTrack track) {

            if ( !CanAddTrack(track) ) {
                return null;
            }

            var newTrack = Instantiate(track);
            UnityEditor.Undo.RegisterCreatedObjectUndo(newTrack.gameObject, "Duplicate Track");
            UnityEditor.Undo.SetTransformParent(newTrack.transform, this.transform, "Duplicate Track");
            UnityEditor.Undo.RegisterCompleteObjectUndo(this, "Duplicate Track");
            newTrack.transform.localPosition = Vector3.zero;
            tracks.Add(newTrack);
            Validate();
            CutsceneUtility.selectedObject = newTrack;
            return newTrack;
        }
        
        public void DeleteTrack(SkillTrack track) {

            if ( !track.gameObject.IsSafePrefabDelete() ) {
                UnityEditor.EditorUtility.DisplayDialog("Delete Track", "This track is part of the prefab asset and can not be deleted from within the prefab instance. If you want to delete the track, please open the prefab asset for editing.", "OK");
                return;
            }

            UnityEditor.Undo.RegisterCompleteObjectUndo(this, "Delete Track");
            tracks.Remove(track);
            if ( ReferenceEquals(SkillEditorUtility.selectedObject, track) ) {
                SkillEditorUtility.selectedObject = null;
            }
            UnityEditor.Undo.DestroyObjectImmediate(track.gameObject);
            Validate();
        }
#endif

    }
}
