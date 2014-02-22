using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Notes: TemporaryMute should be used for things like pausing, that end up resuming
// audio play after a short while. Normal mute is inteded for global "player doesn't want sound" muting.
// Since all non-looped clips are played through a single source as one-shots, individual clips cannot
// be stopped once started. Another audio source must be added if such control is needed.

public enum Audio{
	dribble = 0,
	shoot,
	destroy,
	dash,
	bump,
	stun,
	bounce,
	goal,
	barrierDestroy,
	powerShot,
	teleport,
	turn,
	blip,
	start,
	fuzz,
	implode,
	warning,
	menuConfirm,
	menuSwitch,
	Length
}

public enum AudioClass{
	Test = 0,
	Length
}

public class AudioHandler : BaseBehaviour {
	
	// Singleton reference
	public static AudioHandler Instance;

	[SerializeField] [HideInInspector]
	private bool editorInitialized;
	[SerializeField] [HideInInspector]
	private AudioClassSetting[] settings;
	[SerializeField] [HideInInspector]
	private AudioObject[] objects;

	// 2D
	public int poolSize2D;
	private GameObject Root_2D;
	private Queue<AudioSpeaker> speakers2D_Open;
	private LinkedList<AudioSpeaker> speakers2D_Playing;

	// 3D
	public int poolSize3D;
	private GameObject Root_3D;
	private Queue<AudioSpeaker> speakers3D_Open;
	private LinkedList<AudioSpeaker> speakers3D_Playing;

	// Utility list for recycling process
	List<LinkedListNode<AudioSpeaker>> toRemove;

	// Use this for initialization
	void Awake () {
		Init();
	}

	// Set up the AudioHandler singleton and internal vars
	public void Init(){
		if(Instance == null){

			Instance = this;

			Setup2D();
			Setup3D();

			InitAudioObjects();

			PreloadClips();
		}
		else if(Instance != this){
			GameObject.Destroy(this.gameObject);	
		}
	}

	private void Setup2D(){

		Root_2D = new GameObject("Root_2D");
		Root_2D.transform.position = transform.position;
		Root_2D.transform.parent = transform;

		speakers2D_Open = new Queue<AudioSpeaker>(poolSize2D);
		speakers2D_Playing = new LinkedList<AudioSpeaker>();
		
		AudioSource source;
		AudioSpeaker speaker;
		for(int i = 0; i < poolSize2D; i++){
			source = Root_2D.AddComponent<AudioSource>();
			source.enabled = false;
			speaker = new AudioSpeaker(source);
			speakers2D_Open.Enqueue(speaker);
		}
	}

	private void Setup3D(){

		Root_3D = new GameObject("Root_3D");
		Root_3D.transform.position = transform.position;
		Root_3D.transform.parent = transform;
		
		speakers3D_Open = new Queue<AudioSpeaker>(poolSize3D);
		speakers3D_Playing = new LinkedList<AudioSpeaker>();
		
		GameObject speaker3D;
		AudioSpeaker speaker;
		for(int i = 0; i < poolSize3D; i++){

			speaker3D = new GameObject("Speaker_3D", typeof(AudioSource));
			speaker3D.SetActive(false);
			speaker3D.transform.parent = Root_3D.transform;

			speaker = new AudioSpeaker(speaker3D.audio);

			speakers3D_Open.Enqueue(speaker);
		}
	}

	private void InitAudioObjects(){
		for(int i = 0; i < objects.Length; i++){
			objects[i].Init();
		}
	}

	private void PreloadClips(){
		for(int i = 0; i < (int) Audio.Length; i++){
			if(objects[i].preload && objects[i].nameInResources != ""){
				objects[i].LoadClip();
			}
		}
	}

	void Update(){

		RecycleAudio2D();
		RecycleAudio3D();
	}

	private void RecycleAudio2D(){
		if(speakers2D_Playing.Count > 0){
			
			toRemove = new List<LinkedListNode<AudioSpeaker>>();
			
			for(LinkedListNode<AudioSpeaker> speaker = speakers2D_Playing.First; speaker != speakers2D_Playing.Last.Next; speaker = speaker.Next){
				if(!speaker.Value.IsPlaying()){
					toRemove.Add(speaker);
				}
			}
			
			for(int i = 0; i < toRemove.Count; i++){
				speakers2D_Playing.Remove(toRemove[i]);
				toRemove[i].Value.SetActive(false);
				speakers2D_Open.Enqueue(toRemove[i].Value);
			}
		}
	}

	private void RecycleAudio3D(){
		if(speakers3D_Playing.Count > 0){
			
			toRemove = new List<LinkedListNode<AudioSpeaker>>();
			
			for(LinkedListNode<AudioSpeaker> speaker = speakers3D_Playing.First; speaker != speakers3D_Playing.Last.Next; speaker = speaker.Next){
				if(!speaker.Value.IsPlaying()){
					toRemove.Add(speaker);
				}
			}
			
			for(int i = 0; i < toRemove.Count; i++){
				speakers3D_Playing.Remove(toRemove[i]);
				toRemove[i].Value.SetGameobjectActive(false);
				speakers3D_Open.Enqueue(toRemove[i].Value);
			}
		}
	}
	
	// Individual play functions

	public static void Play(Audio sound){

		AudioSpeaker speaker = Instance.PullSpeaker2D();
		speaker.SetAudioObject(sound);

		speaker.Play();
	}

	public static void Play(Audio sound, Vector3 location){
		AudioSpeaker speaker = Instance.PullSpeaker3D();
		speaker.SetAudioObject(sound);
		speaker.SetPosition(location);
		speaker.Play();
	}

	public static AudioSpeaker PlayLooped(Audio sound){
		AudioSpeaker speaker = Instance.PullSpeaker2D();
		speaker.SetAudioObject(sound);
		speaker.SetLooped(true);
		speaker.Play();
		return speaker;
	}

	public static AudioSpeaker PlayLooped(Audio sound, Vector3 location){
		AudioSpeaker speaker = Instance.PullSpeaker3D();
		speaker.SetAudioObject(sound);
		speaker.SetLooped(true);
		speaker.SetPosition(location);
		speaker.Play();
		return speaker;
	}

	public AudioSpeaker PullSpeaker2D(){
		AudioSpeaker speaker = speakers2D_Open.Dequeue();
		speaker.SetActive(true);
		speakers2D_Playing.AddLast(speaker);
		return speaker;
	}

	public AudioSpeaker PullSpeaker3D(){
		AudioSpeaker speaker = speakers3D_Open.Dequeue();
		speaker.SetGameobjectActive(true);
		speakers3D_Playing.AddLast(speaker);
		return speaker;
	}

	private AudioObject RetrieveAudio(Audio sound){

		AudioObject audioInfo = objects[(int) sound];

		if(audioInfo.clip == null){
			audioInfo.LoadClip();
		}

		return audioInfo;
	}

	// Other

	public static void SetClassVolume(AudioClass audioClass, float volume){
		Instance.settings[(int) audioClass].SetVolume(volume);
	}

	public static void SetAudioVolume(Audio audio, float volume){
		Instance.objects[(int) audio].SetDefaultVolume(volume);
	}
	
	public static void MuteVolume(bool muteOn){
		Instance.SetMute(muteOn);
		GlobalData.mute = muteOn;
	}
	
	// Unlike the normal MuteVolume, this does not set the GlobalData.mute flag,
	// and thus does not override player set mute settings (since it first checks to ensure mute is off)
	public static void TemporaryMuteVolume(bool muteOn){
		Instance.SetMute(muteOn);
	}
	
	private void SetVolume(float volume){
		//TODO
	}
	
	private void SetMute(bool on){
		//TODO
	}

	[System.Serializable]
	public class AudioClassSetting{

		public bool mute;
		public float volume = 1.0f;

		public FloatParamDelegate UpdateVolume;
		public BoolParamDelegate UpdateMute;

		public void SetVolume(float volume){
			volume = Mathf.Clamp01(volume);
			if(UpdateVolume != null){ UpdateVolume(volume);}
		}

		public void Mute(bool state){
			mute = state;
			if(UpdateMute != null){ UpdateMute(state);}
		}
	}
	
	[System.Serializable]
	public class AudioObject{

		private Audio name;
		public AudioClass audioClass;
		public AudioClip clip;
		public string nameInResources;
		public bool preload;
		public float defaultVolume = 1.0f;
		public float currentVolume = 1.0f;

		public FloatParamDelegate UpdateVolume;

		public void Init(){
			Instance.settings[(int) audioClass].UpdateVolume += SetVolume;
		}

		public void LoadClip(){
			clip = (AudioClip) Resources.Load(nameInResources);
		}

		public void SetVolume(float percentOfDefaultVolume){
			currentVolume = defaultVolume * percentOfDefaultVolume;
			if(UpdateVolume != null){ UpdateVolume(currentVolume);}
		}

		public void SetDefaultVolume(float volume){
			defaultVolume = Mathf.Clamp01(volume);
			SetVolume(Instance.settings[(int) audioClass].volume);
		}

		public void Reset(){
			Debug.Log("Reset: " + nameInResources);
			audioClass = AudioClass.Length;
			clip = null;
			nameInResources = "";
			preload = false;
			defaultVolume = 1.0f;
			currentVolume = 1.0f;
		}
	}

	public class AudioSpeaker{

		private AudioSource speaker;
		private AudioObject info;

		public AudioSpeaker(AudioSource source){
			speaker = source;
		}

		public void SetAudioObject(Audio audio){

			info = Instance.objects[(int) audio];
			info.UpdateVolume += SetVolume;

			Instance.settings[(int) info.audioClass].UpdateMute += Mute;

			speaker.clip = info.clip;
			speaker.volume = info.currentVolume;
			speaker.mute = Instance.settings[(int) info.audioClass].mute;
		}

		public void SetVolume(float volume){
			speaker.volume = volume;
		}

		public void Mute(bool state){
			speaker.mute = state;
		}

		public void SetLooped(bool state){
			speaker.loop = state;
		}

		public bool IsPlaying(){
			return speaker.isPlaying;
		}

		public bool IsActive(){
			return speaker.gameObject.activeInHierarchy;
		}

		public void SetActive(bool state){
			speaker.enabled = state;
			ManageVolumeEvent(state);
		}

		public void SetGameobjectActive(bool state){
			speaker.gameObject.SetActive(state);
			ManageVolumeEvent(state);
		}

		private void ManageVolumeEvent(bool state){
			if(!state){ 
				info.UpdateVolume -= SetVolume;
				Instance.settings[(int) info.audioClass].UpdateMute -= Mute;
			}
		}

		public void SetPosition(Vector3 location){
			speaker.gameObject.transform.position = location;
		}

		public void Play(){
			speaker.Play();
		}

		public void Stop(){
			speaker.Stop();
		}
	}

	#region Editor

	public void EditorInit(){

		if(!editorInitialized){

			editorInitialized = true;

			Debug.Log("Audio Handler Editor Initialize");
			
			settings = new AudioClassSetting[(int) AudioClass.Length];
			for(int i = 0; i < (int) AudioClass.Length; i++){
				settings[i] = new AudioClassSetting();
			}
			
			objects = new AudioObject[(int) Audio.Length];
			for(int i = 0; i < (int) Audio.Length; i++){
				objects[i] = new AudioObject();
			}
		}
	}
	
	public void EditorRefresh(){
		Debug.Log("Audio Handler Editor Refresh");
		RefreshClassArray(ref settings);
		RefreshAudioArray(ref objects);
		CheckAudioClipExistance();
	}

	private void RefreshClassArray(ref AudioClassSetting[] array){
		int length = (int) AudioClass.Length;
		if(array.Length != length){
			AudioClassSetting[] newArray = new AudioClassSetting[length];
			if(length > array.Length){
				array.CopyTo(newArray, 0);
			}
			else{
				for(int i = 0; i < length; i++){
					newArray[i] = array[i];
				}
			}
			array = newArray;
		}
	}

	private void RefreshAudioArray(ref AudioObject[] array){
		int length = (int) Audio.Length;
		if(array.Length != length){
			AudioObject[] newArray = new AudioObject[length];
			if(length > array.Length){
				array.CopyTo(newArray, 0);
			}
			else{
				for(int i = 0; i < length; i++){
					newArray[i] = array[i];
				}
			}
			array = newArray;
		}
	}

	private void CheckAudioClipExistance(){
		for(int i = 0; i < objects.Length; i++){
			if(objects[i].nameInResources != ""){
				AudioClip clip = Resources.Load(objects[i].nameInResources) as AudioClip;
				if(clip == null){
					objects[i].Reset();
				}
			}
		}
	}

	public bool GetAudioClassMute(AudioClass audioClass){
		return settings[(int) audioClass].mute;
	}

	public float GetAudioClassVolume(AudioClass audioClass){
		return settings[(int) audioClass].volume;
	}

	public void SetAudioClassMute(AudioClass audioClass, bool state){
		settings[(int) audioClass].mute = state;
	}
	
	public void SetAudioClassVolume(AudioClass audioClass, float volume){
		settings[(int) audioClass].volume = Mathf.Clamp01(volume);
	}

	public AudioClass GetAudioObjectClass(Audio audio){
		return objects[(int) audio].audioClass;
	}

	public void SetAudioObjectClass(Audio audio, AudioClass audioClass){
		objects[(int) audio].audioClass = audioClass;
	}

	public AudioClip GetAudioObjectClip(Audio audio){
		return objects[(int) audio].clip;
	}

	public void SetAudioObjectClip(Audio audio, AudioClip clip){
		objects[(int) audio].clip = clip;
	}

	public string GetAudioObjectName(Audio audio){
		return objects[(int) audio].nameInResources;
	}
	
	public void SetAudioObjectName(Audio audio, string name){
		objects[(int) audio].nameInResources = name;
	}

	public bool GetAudioObjectPreload(Audio audio){
		return objects[(int) audio].preload;
	}

	public void SetAudioObjectPreload(Audio audio, bool state){
		objects[(int) audio].preload = state;
	}

	public float GetAudioObjectDefaultVolume(Audio audio){
		return objects[(int) audio].defaultVolume;
	}

	public void SetAudioObjectDefaultVolume(Audio audio, float volume){
		objects[(int) audio].defaultVolume = volume;
	}

	public void ResetAudioObject(Audio audio){
		objects[(int) audio].Reset();
	}

	#endregion
}
