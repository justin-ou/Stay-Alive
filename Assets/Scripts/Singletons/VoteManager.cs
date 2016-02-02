using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VoteManager : Singleton<VoteManager> {

	private int _voteCount, _voteCountDuration, _minVoteCount;
	private bool _isVoteComplete;
	private Object semaphore;

	private Dictionary<VoteOptions, int> _voteOptionsCount;

	public void Init(){
		semaphore = new Object();
		_minVoteCount = Constants.GAME_MIN_NUM_OF_VOTE;
		AddVoteOptions();
		InitVote ();
	}

	void AddVoteOptions(){
		_voteOptionsCount = new Dictionary<VoteOptions, int>();
		_voteOptionsCount.Add(VoteOptions.YES, 0);
		_voteOptionsCount.Add(VoteOptions.ZERO, 0);
		_voteOptionsCount.Add(VoteOptions.ONE, 0);
		_voteOptionsCount.Add(VoteOptions.TWO, 0);
		_voteOptionsCount.Add(VoteOptions.THREE, 0);
		_voteOptionsCount.Add(VoteOptions.CHAR_1, 0);
		_voteOptionsCount.Add(VoteOptions.CHAR_2, 0);
		_voteOptionsCount.Add(VoteOptions.CHAR_3, 0);
		_voteOptionsCount.Add(VoteOptions.CHAR_4, 0);
	}

	public void InitVote(){
		_voteCount = 0;
		_voteCountDuration = Constants.GAME_VOTE_COUNT_DURATION;
		_isVoteComplete = false;
				
		_voteOptionsCount[VoteOptions.NO] = 0;
		_voteOptionsCount[VoteOptions.YES] = 0;
		_voteOptionsCount[VoteOptions.ZERO] = 0;
		_voteOptionsCount[VoteOptions.ONE] = 0;
		_voteOptionsCount[VoteOptions.TWO] = 0;
		_voteOptionsCount[VoteOptions.THREE] = 0;
		_voteOptionsCount[VoteOptions.CHAR_1] = 0;
		_voteOptionsCount[VoteOptions.CHAR_2] = 0;
		_voteOptionsCount[VoteOptions.CHAR_3] = 0;
		_voteOptionsCount[VoteOptions.CHAR_4] = 0;
	}

	// Increase the vote count when a guest his the drum
	// Increment vote option only when it is a required
	// Start coroutine countdown when all votes are in
	public void AddVoteCount(VoteOptions voteOption, int controllerID){
		// Debug.Log (_voteCount+" | "+voteOption);
		if(!_isVoteComplete && _voteCount < Constants.GAME_NUM_OF_PLAYERS){
			_voteCount++;
			Debug.Log ("Min Vote: "+_minVoteCount+" | Vote Count: "+_voteCount);
			if(_voteCount == _minVoteCount+1){
				StartCoroutine("CoVoteDecisionCountdown");
			}

			if(voteOption != VoteOptions.NONE){
				_voteOptionsCount[voteOption]++;
				Debug.Log ("Vote Count: "+voteOption+" | "+_voteOptionsCount[voteOption]);
			}

			// If currently voting to eat, 
			// let the character store how much they wish to eat
			if(GetDrumState() == DrumState.AMOUNT_TO_EAT){
				CharacterManager.Instance.SetEatAmount(controllerID-1, (int)voteOption);
			}
		}
	}

	// Reduce the vote count when a guest undo his vote
	// Decrement vote option only when it is a required
	public void ReduceVoteCount(VoteOptions voteOption, int controllerID){
		if(!_isVoteComplete && _voteCount > 0){
			_voteCount--;
			StopCoVoteDecisionCountdown();

			if(voteOption != VoteOptions.NONE){
				_voteOptionsCount[voteOption]--;
				DrumStateManager.Instance.ResetDrumVote(controllerID-1);
			}
		}
	}

	public bool IsVoteValid(VoteOptions voteOption, int controllerID){
		if(voteOption == VoteOptions.NONE)
			return true;

		return DrumStateManager.Instance.LockInDrumVote(controllerID-1);
	}

	// Positive votes must be more than or equals to 3 (GAME_MIN_NUM_OF_VOTE) for it to pass
	// Default otherwise
	// @return VoteOptions: the selected vote result
	public VoteOptions GetMajorityVote(VoteOptions defaultVote){
		VoteOptions majorityVote = defaultVote;
		foreach(VoteOptions key in _voteOptionsCount.Keys){
			int value = _voteOptionsCount[key];
			if(value >= GetRequireMajorityVoteCount()){
				majorityVote = key;
			}
		}
		return majorityVote;
	}

	IEnumerator CoVoteDecisionCountdown(){
		while(true){
			Debug.Log (_voteCountDuration+"!");
			yield return new WaitForSeconds(1f);
			_voteCountDuration--;
		
			if(_voteCountDuration == 0){
				_isVoteComplete = true;
				DrumStateManager.Instance.StartNextState();
				StopCoVoteDecisionCountdown();			
			}
		}
	}

	void StopCoVoteDecisionCountdown(){
		_voteCountDuration = Constants.GAME_VOTE_COUNT_DURATION;
		StopCoroutine("CoVoteDecisionCountdown");
	}
	int GetRequireMajorityVoteCount(){
		if(_minVoteCount == 1){
			return 2;
		}
		return _minVoteCount;
	}
	public void ReduceMinVoteCount() {
		_minVoteCount--; 
	}
	public DrumState GetDrumState()	{ 	return DrumStateManager.Instance.GetDrumState(); }
	public bool IsVoteEnabled()		{ 	return !_isVoteComplete; }
}
