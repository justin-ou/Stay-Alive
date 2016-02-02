using UnityEngine;
using System.Collections;

public class VoteYesNoController : DrumVotingController {

	public VoteYesNoController() : base(){	
		// Initialise voting prefab here
		GameObject prefab = Resources.Load("Prefabs/YesNoVote") as GameObject;
		_voteUIPrefabArr[0] = InstantiatePrefab(prefab, UIManager.Instance.GetDrumObjectPosition(0)) as GameObject; 
		_voteUIPrefabArr[1] = InstantiatePrefab(prefab, UIManager.Instance.GetDrumObjectPosition(1)) as GameObject;
		_voteUIPrefabArr[2] = InstantiatePrefab(prefab, UIManager.Instance.GetDrumObjectPosition(2)) as GameObject;
		_voteUIPrefabArr[3] = InstantiatePrefab(prefab, UIManager.Instance.GetDrumObjectPosition(3)) as GameObject;
		SetUIVisibleState(false);
	}

	public override VoteOptions GetSelectedVote(float angle, int index){ 
		if(angle < Constants.WHEEL_MIN_TURN_RADIUS && angle >= 0){
			SetOptionNumber(index, 0); // 0 represents Yes
			return VoteOptions.YES;
		}
		SetOptionNumber(index, 1); // 1 represents No
		return VoteOptions.NO;		
	}
	private void SetOptionNumber(int index, int number){
		_voteUIPrefabArr[index].GetComponent<VoteYesNo>().UpdateButtonState(number);
	}
	public override bool LockButtonState(int index){
		return _voteUIPrefabArr[index].GetComponent<VoteYesNo>().LockButtonState();
	}
	public override void ResetButtonState(int index){
		_voteUIPrefabArr[index].GetComponent<VoteYesNo>().ResetButtonState();
	}
}
