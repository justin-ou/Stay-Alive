using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VoteCharacterController : DrumVotingController {

	private float _divisionAngle;
	private VoteOptions[] _voteOptions;

	public VoteCharacterController() : base(){
		// Initialise voting prefab here
		GameObject prefab = Resources.Load("Prefabs/CharacterVote") as GameObject;
		_voteUIPrefabArr[(int)CharacterIndex.CHAR1] = InstantiatePrefab(prefab, UIManager.Instance.GetDrumObjectPosition(0)) as GameObject; 
		_voteUIPrefabArr[(int)CharacterIndex.CHAR2] = InstantiatePrefab(prefab, UIManager.Instance.GetDrumObjectPosition(1)) as GameObject;
		_voteUIPrefabArr[(int)CharacterIndex.CHAR3] = InstantiatePrefab(prefab, UIManager.Instance.GetDrumObjectPosition(2)) as GameObject;
		_voteUIPrefabArr[(int)CharacterIndex.CHAR4] = InstantiatePrefab(prefab, UIManager.Instance.GetDrumObjectPosition(3)) as GameObject;
		SetUIVisibleState(false);

		_voteOptions = new VoteOptions[5];
		_voteOptions[0] = VoteOptions.ZERO;
		_voteOptions[1] = VoteOptions.CHAR_1;
		_voteOptions[2] = VoteOptions.CHAR_2;
		_voteOptions[3] = VoteOptions.CHAR_3;
		_voteOptions[4] = VoteOptions.CHAR_4;

		_divisionAngle = Constants.WHEEL_TURN_RADIUS/5f;
	}

	public override VoteOptions GetSelectedVote(float angle, int index){ 
		float convertedAngle = angle;
		float minAngle = -90f;

		// Get Angle from -90 ~ 90 degree
		if(convertedAngle > Constants.WHEEL_MAX_TURN_RADIUS && convertedAngle <= 360){
			convertedAngle -= 360;
		}
		for(int i=0; i<_voteOptions.Length; i++){
			float nextAngle = minAngle + _divisionAngle;
			if(convertedAngle >= minAngle && convertedAngle < nextAngle){
				SetOptionNumber(index, i);
				return _voteOptions[i];
			}
			minAngle = nextAngle;
		}
		return VoteOptions.ZERO;
	}		
	private void SetOptionNumber(int index, int number){
		_voteUIPrefabArr[index].GetComponent<VoteCharacter>().UpdateButtonState(number);
	}
	public override bool LockButtonState(int index){
		return _voteUIPrefabArr[index].GetComponent<VoteCharacter>().LockButtonState();
	}
	public override void ResetButtonState(int index){
		_voteUIPrefabArr[index].GetComponent<VoteCharacter>().ResetButtonState();
	}
}
