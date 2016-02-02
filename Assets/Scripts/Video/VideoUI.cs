using UnityEngine;
using System.Collections;

public class VideoUI : MonoBehaviour {

	public Renderer videoMaterial;
	public AudioClip[] videoAudioList;
	public AudioSource videoAudioSource;
	private Material[] _videoList;
	private bool[] _videoPlayedList;		// Ensure that videos are only played once
	private float[] _videoDurationList;

	// Use this for initialization
	void Start () {
		_videoList = new Material[(int)VideoState.VIDEO_SIZE];
		_videoList[(int)VideoState.VIDEO_OPENING_0] = Resources.Load("Materials/Video/Video_Opening") as Material;
		_videoList[(int)VideoState.VIDEO_FOOD_RUN_OUT_1] = Resources.Load("Materials/Video/Video_FoodRunOut") as Material;
		_videoList[(int)VideoState.VIDEO_FIRST_BLOOD_2] = Resources.Load("Materials/Video/Video_FirstBlood") as Material;

		_videoDurationList = new float[(int)VideoState.VIDEO_SIZE];
		_videoDurationList[(int)VideoState.VIDEO_OPENING_0] = Constants.VIDEO_OPEN_0_DURATION;
		_videoDurationList[(int)VideoState.VIDEO_FOOD_RUN_OUT_1] = Constants.VIDEO_FOOD_OUT_1_DURATION;
		_videoDurationList[(int)VideoState.VIDEO_FIRST_BLOOD_2] = Constants.VIDEO_FIRST_BLOOD_2_DURATION;

		_videoPlayedList = new bool[(int)VideoState.VIDEO_SIZE];
		for(int i=0; i<(int)VideoState.VIDEO_SIZE; i++){
			_videoPlayedList[i] = false;
		}
		// Note: Set to inactive here instead of the main scene to load the resources
		this.gameObject.SetActive(false);
	}

	public bool PlayVideo(VideoState videoState){
		int index = (int)videoState;
		if(!_videoPlayedList[index]){
			this.gameObject.SetActive(true);
			videoMaterial.material = _videoList[index];
			StartPlayVideo();
			PlayAudio(videoState);
			_videoPlayedList[index] = true;
		}
		return _videoPlayedList[index];
	}
	public void StopVideo(){
		StopPlayVideo();
		StopAudio();
		this.gameObject.SetActive(false);
	}
	public float GetVideoDuration(VideoState videoState){
		return _videoDurationList[(int)videoState];
	}

	void StartPlayVideo(){
		MovieTexture video = ((MovieTexture)videoMaterial.material.mainTexture);
		if(video != null){
			video.Play();
		}
	}
	void StopPlayVideo(){
		MovieTexture video = ((MovieTexture)videoMaterial.material.mainTexture);
		if(video != null){
			video.Stop();
		}
	}
	void PlayAudio(VideoState videoState){
		videoAudioSource.clip = videoAudioList[(int)videoState];
		videoAudioSource.Play();
	}
	void StopAudio(){
		videoAudioSource.Stop();
	}
}
