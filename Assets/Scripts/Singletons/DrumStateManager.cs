using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrumStateManager : Singleton<DrumStateManager> {
	
	private DrumState _drumState;	
	private DrumVotingController _drumVotingController;
	private DrumVotingController _voteYesNoController;
	private DrumVotingController _voteFourController;
	private DrumVotingController _voteCharacterController;
	private VoteOptions _defaultVoteOption;
	private JamODrumManager	_jamODrumManager;

	void Start(){

	}

	public void Init(){
		_drumState = DrumState.READY_TO_START;
		_defaultVoteOption = VoteOptions.NONE;
		InitVotingControllers();

		_jamODrumManager = GameObject.Find ("JamODrum").GetComponent<JamODrumManager>();
	}
	void InitVotingControllers(){
		SetDrumVotingController(null);
		_voteYesNoController = new VoteYesNoController();
		_voteFourController = new VoteFourController();
		_voteCharacterController = new VoteCharacterController();
	}

	public void Reset(){
		_defaultVoteOption = VoteOptions.NONE;
		_jamODrumManager = null;
	}

	// State machine to check the settings for the next voting phase
	// Set _isVoteComplete to false only when the next scene is loaded
	// Initialise the next state and complete the action for the current state
	public void StartNextState(){
		switch(_drumState){
		case DrumState.READY_TO_START:
			Debug.Log ("Start Game!");
			ReadyToStartAction();
			break;
		case DrumState.VOTE_TO_EAT:
			Debug.Log ("Vote to Eat!");
			VoteToEatAction();
			break;
		case DrumState.AMOUNT_TO_EAT:
			Debug.Log ("Amount of rations to eat!");
			AmountToEatAction();
			break;
		case DrumState.VOTE_TO_EAT_CHARACTER:
			Debug.Log ("Which character to eat?");
			VoteToEatCharacterAction();
			break;
		default:
			Debug.Log ("Invalid Drum State Error."); 
			break;
		}
	}

	/*********************
	 *  Initialisation for each state
	 */
	public void InitialiseVotingState(){
		// Method Init() must be called before this
		InitVoteToEat();
	}
	void InitReadyToStart(){
		_drumState = DrumState.READY_TO_START;
		_defaultVoteOption = VoteOptions.NONE;
		UIManager.Instance.UpdateInstruction(InstructionState.TAP_TO_START_0);
		SetDrumVotingController(null);
	}
	void InitVoteToEat(){
		_defaultVoteOption = VoteOptions.NO;
		SetDrumVotingController(_voteYesNoController);
		UIManager.Instance.UpdateInstruction(InstructionState.SHOULD_EAT_TODAY_1);
		// Check if any character is dying to update their instructions
		CharacterManager.Instance.CheckCharacterDying();
		ShowDrumVotingController();
		VoteManager.Instance.InitVote();
	}
	void InitAmountToEat(){
		_defaultVoteOption = VoteOptions.ZERO;
		SetDrumVotingController(_voteFourController);
		ShowDrumVotingController();
		UIManager.Instance.UpdateInstruction(InstructionState.VOTE_TO_EAT_3);
		VoteManager.Instance.InitVote();
	}
	void InitCharacterToEat(){
		_defaultVoteOption = VoteOptions.ZERO;
		SetDrumVotingController(_voteCharacterController);
		ShowDrumVotingController();
		UIManager.Instance.UpdateInstruction(InstructionState.OUT_OF_FOOD_5);
		VoteManager.Instance.InitVote();
	}

	/*********************
	 *  Action to run for each state
	 */
	void ReadyToStartAction(){
		GameManager.Instance.StartGame();
		_drumState = DrumState.VOTE_TO_EAT;
	}
	void VoteToEatAction(){
		VoteOptions vote = VoteManager.Instance.GetMajorityVote(_defaultVoteOption);
		if(vote == VoteOptions.YES){
			if(CharacterManager.Instance.HasRationRemaining()){
				_drumState = DrumState.AMOUNT_TO_EAT;
				InitAmountToEat();
			}else{
				_drumState = DrumState.VOTE_TO_EAT_CHARACTER;
				InitCharacterToEat();
			}
		}else{
			_drumState = DrumState.VOTE_TO_EAT;
			UIManager.Instance.UpdateInstruction(InstructionState.VOTE_NOT_TO_EAT_2);
			GameManager.Instance.StartNewDay();
		}
	}
	void AmountToEatAction(){
		_drumState = DrumState.VOTE_TO_EAT;
		CharacterManager.Instance.AddHunger();
		GameManager.Instance.StartNewDay();
	}
	void VoteToEatCharacterAction(){
		VoteOptions vote = VoteManager.Instance.GetMajorityVote(_defaultVoteOption);
		if(vote != _defaultVoteOption){
			CharacterManager.Instance.FlagCharacterToEat(vote);
		}else{
			UIManager.Instance.UpdateInstruction(InstructionState.VOTE_NOT_TO_EAT_2);
		}
		_drumState = DrumState.VOTE_TO_EAT;
		GameManager.Instance.StartNewDay();
	}
	
	void SetDrumVotingController(DrumVotingController drumVotingController){
		if(_drumVotingController != null){
			_drumVotingController.SetUIVisibleState(false);
		}
		_drumVotingController = drumVotingController;
	}
	void ShowDrumVotingController(){
		if(_drumVotingController != null){
			_drumVotingController.SetUIVisibleState(true);
			_drumVotingController.ResetAllButtonState();
			_jamODrumManager.ResetHitState();
		}
	}
	public void ResetDrumVotingController(){
		if(_drumVotingController != null){
			_drumVotingController.ResetAllButtonState();
			_drumVotingController.SetUIVisibleState(false);
			_jamODrumManager.ResetHitState();

			_voteYesNoController = null;
			_voteFourController = null;
			_voteCharacterController = null;
		}
	}

	// Get the vote only if a controller is specified
	// @return VoteOptions: // @return VoteOptions: the selected vote for each guest
	public VoteOptions GetSelectedVoteOption(float angle, int index){
		return (_drumVotingController != null) ? _drumVotingController.GetSelectedVote(angle, index) : VoteOptions.NONE;
	}
	public bool LockInDrumVote(int index){
		if(_drumVotingController != null){
			return _drumVotingController.LockButtonState(index);
		}
		return false;
	}
	public void ResetDrumVote(int index){
		if(_drumVotingController != null){
			_drumVotingController.ResetButtonState(index);
		}
	}

	public DrumState GetDrumState()	{ 	return _drumState; }
	public VoteOptions GetDefaultVoteOption() { return _defaultVoteOption; }
}
