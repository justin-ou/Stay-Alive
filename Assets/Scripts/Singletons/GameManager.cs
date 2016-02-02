using UnityEngine;
using System.Collections;

public class GameManager : Singleton<GameManager> {

	bool _isFirstGame;
	bool _isGameOver, _isGameStart;
	bool _isVideoPlaying;
	VideoState _currentVideoState;

	// Singleton init
	VoteManager _voteManager;
	UIManager _uiManager;
	DayManager _dayManager;
	DrumStateManager _drumStateManager;
	CharacterManager _characterManager;
	SoundManager _soundManager;

	// Use this for initialization
	void Start () {
		_isFirstGame = true;

		// Hacks: Create the SoundManager prefab in here as this will only be called once
		Instantiate(Resources.Load("Prefabs/Sound Manager"));
		_soundManager = SoundManager.Instance;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// Initialise all the classes again after restarting the game
	public void Init(){
		_isFirstGame = false;
		_isGameOver = false;
		_isGameStart = false;
		_isVideoPlaying = false;

		_voteManager = VoteManager.Instance;
		_uiManager = UIManager.Instance;
		_dayManager = DayManager.Instance;
		_drumStateManager = DrumStateManager.Instance;
		_characterManager = CharacterManager.Instance;

		_voteManager.Init();
		_uiManager.Init(); // UIManager before DayManager for draw order
		_dayManager.Init(); 
		_uiManager.AddRedSpotLight();
		_drumStateManager.Init();
	}

	// Start playing the video after this round of voting
	// @VideoState: specify which video should be played
	public void StartPlayingVideo(VideoState videoState){
		if(!Config.IS_EMULATOR && Config.CAN_PLAY_VIDEO){
			_isVideoPlaying = true;
			_currentVideoState = videoState;
		}else{
			_isVideoPlaying = false;
		}
		// Debug.Log("Start Playing Video: "+_isVideoPlaying+" | "+videoState);		
	}

	// Play the video only when it is specified to be played
	// Stop the video after the day has faded in
	IEnumerator CoStartPlayingVideo(){
		// Debug.Log("Start Playing Video: "+_isVideoPlaying);
		if(_isVideoPlaying){
			bool canPlayVideo = _uiManager.PlayVideo(_currentVideoState);
			Debug.Log("Can Play Video: "+canPlayVideo);
			if(canPlayVideo){
				_dayManager.FadeDayIn();
				yield return new WaitForSeconds(_uiManager.GetVideoDuration(_currentVideoState));
			}
		}
		yield return 0;
	}

	// Start the game with the opening video
	public void StartGame(){
		// Update UI, fade out to show cutscene if any
		// Fade in to game before calling Init() methods for Managers
		_isGameStart = true;
		StartPlayingVideo(VideoState.VIDEO_OPENING_0);
		_soundManager.BGOceanStop();
		StartCoroutine("CoStartFirstDay");
	}

	// Start a new day 
	public void StartNewDay(){
		StartCoroutine("CoStartNewDay");
	}

	IEnumerator CoStartFirstDay(){
		_soundManager.BGOceanPlay();					// Start BGM
		yield return StartCoroutine("CoStartPlayingVideo");		// Start playing the intro video

		_dayManager.NextDay();							// Start the first day 0 -> 1
		
		yield return new WaitForSeconds(2f);

		// Show the week number
		_dayManager.FadeToShowText();
		
		yield return new WaitForSeconds(3f);

		_dayManager.FadeShipText();
		
		yield return new WaitForSeconds(4f);

		// Initial Init
		_uiManager.StartGameInit();						// Initialise the UI that shows up only after the game starts
		
		_dayManager.FadeDayIn();

		_uiManager.StopVideo();							// Stop the video here because it will definitely be playing
		_isVideoPlaying = false;

		_drumStateManager.InitialiseVotingState();		// Initialise the first voting state	
		_characterManager.Init ();						// Initialise the characters and instantiate prefabs
		_voteManager.InitVote();						// Initalise the votes to start voting
	}

	IEnumerator CoStartNewDay(){

		if(Config.IS_EMULATOR){
			yield return new WaitForSeconds(1f);
		}else{
			yield return new WaitForSeconds(5f);
		}

		// Start the next day
		_dayManager.NextDay();

		yield return new WaitForSeconds(2f);

		// Night event, eg. Eating
		int eatenIndex = _characterManager.GetEatCharacterIndex();
		bool isEaten = _characterManager.CheckEatCharacter();
		if(isEaten){
			// Eating sounds
			// Red light on person eaten
			_soundManager.SFXEatPPL();
			Vector3 eatenPosition = _uiManager.GetCanvasPosition((CharacterIndex) eatenIndex).localPosition;
			if(eatenIndex >= 0){
				_uiManager.SetActiveSpotLight(true, eatenPosition);
			}
			yield return new WaitForSeconds(Constants.CHARACTER_EATEN_DURATION);
			_uiManager.SetActiveSpotLight(false, eatenPosition);
			yield return new WaitForSeconds(2f); // Fade out duration
		}

		// Show the week number
		_dayManager.FadeToShowText();
		
		yield return new WaitForSeconds(3f);

		if(!_dayManager.HasShipArrived()){
			_dayManager.FadeShipText();		
			yield return new WaitForSeconds(4f);
		}

		_characterManager.ReduceHunger();

		// Check if the video should be played
		yield return StartCoroutine("CoStartPlayingVideo");

		// Check if the game is over
		int numOfSurviors = _characterManager.GetNumOfCharactersAlive();
		if(numOfSurviors == 0){
			_isGameOver = true;
		}

		_uiManager.UpdateWeeksNumber(_dayManager.GetCurrentDay());

		yield return new WaitForSeconds(3f);

		if(!_isGameOver){
			_dayManager.FadeDayIn();

			if(_isVideoPlaying){
				_uiManager.StopVideo();
				_isVideoPlaying = false;
			}

			if(!_dayManager.HasShipArrived()){
				_soundManager.BGMThemePlay(_dayManager.GetCurrentDay());
				_soundManager.BGMBeatPlay(_dayManager.GetCurrentDay());

				_drumStateManager.InitialiseVotingState();
				_voteManager.InitVote();
			}else{
				// Hide voting UI and instructions
				_drumStateManager.ResetDrumVotingController();
				_uiManager.HideInstructionList();

				float endingSoundDuration = 5f;
				float showEndStateDuration = 6f;

				if(numOfSurviors == 4){
					SoundManager.Instance.DialogEnding(true);
					endingSoundDuration = 24f; // 29 - endState + 1
				}else{
					SoundManager.Instance.DialogEnding(false);
					endingSoundDuration = 28f; // 33 - endState + 1
				}

				yield return new WaitForSeconds(showEndStateDuration);

				// Survivors are saved
				// Go To new scene
				_dayManager.FadeSurvivors(numOfSurviors);

				yield return new WaitForSeconds(endingSoundDuration);

				ResetGame();
			}
		}else{
			// Game Over State, Change Scene
			_dayManager.FadeGameOver();
			yield return new WaitForSeconds(5f);
			ResetGame();
		}
	}	
		
	void ResetGame(){
		// Reset all states in Singletons
		_uiManager.Reset();
		_drumStateManager.Reset();
		_characterManager.Reset();
		Application.LoadLevel(Application.loadedLevel);
	}

	public bool IsGameOver(){ return _isGameOver; }
	public bool IsStartGame(){ return _isGameStart; }
}
