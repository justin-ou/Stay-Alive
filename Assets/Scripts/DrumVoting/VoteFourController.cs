using UnityEngine;
using System.Collections;

public class VoteFourController : DrumVotingController {
	
	public VoteFourController() : base(){
		// Initialise voting prefab here
		GameObject prefab = Resources.Load("Prefabs/FourVote") as GameObject;
		_voteUIPrefabArr[0] = InstantiatePrefab(prefab, UIManager.Instance.GetDrumObjectPosition(0)) as GameObject; 
		_voteUIPrefabArr[1] = InstantiatePrefab(prefab, UIManager.Instance.GetDrumObjectPosition(1)) as GameObject;
		_voteUIPrefabArr[2] = InstantiatePrefab(prefab, UIManager.Instance.GetDrumObjectPosition(2)) as GameObject;
		_voteUIPrefabArr[3] = InstantiatePrefab(prefab, UIManager.Instance.GetDrumObjectPosition(3)) as GameObject;
		SetUIVisibleState(false);
	}
	
	public override VoteOptions GetSelectedVote(float angle, int index){ 
		float rightHalfAngle = 45f;
		float leftHalfAngle = 315f;

		if(angle < Constants.WHEEL_MIN_TURN_RADIUS){
			if(angle >= rightHalfAngle){
				SetOptionNumber(index, 3);
				return VoteOptions.THREE;
			}else if(angle >= 0){
				SetOptionNumber(index, 2);
				return VoteOptions.TWO;
			}
		}
		if(angle > leftHalfAngle && angle <= 360){
			SetOptionNumber(index, 1);
			return VoteOptions.ONE;
		}
		SetOptionNumber(index, 0);
		return VoteOptions.ZERO;		
	}

	private void SetOptionNumber(int index, int number){
		_voteUIPrefabArr[index].GetComponent<VoteFour>().UpdateButtonState(number);
	}
	public override bool LockButtonState(int index){
		return _voteUIPrefabArr[index].GetComponent<VoteFour>().LockButtonState();
	}
	public override void ResetButtonState(int index){
		_voteUIPrefabArr[index].GetComponent<VoteFour>().ResetButtonState();
	}
}
