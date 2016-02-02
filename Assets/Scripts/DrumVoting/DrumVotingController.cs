using UnityEngine;
using System.Collections;

public class DrumVotingController {

	protected float _divisionAngle;
	protected int _index;
	protected GameObject[] _voteUIPrefabArr;

	public DrumVotingController(){	
		_voteUIPrefabArr = new GameObject[4];
	}
	public virtual VoteOptions GetSelectedVote(float angle, int index){ 
		return VoteOptions.NO;
	}
	protected GameObject InstantiatePrefab(GameObject prefab, Transform parent){
		return UIManager.Instance.InstantiatePrefab(prefab, parent);
	}
	public void SetUIVisibleState(bool visible){
		for(int i=0; i<4; i++){
			if(_voteUIPrefabArr[i] != null)
				_voteUIPrefabArr[i].SetActive(visible);
		}
	}
	public virtual bool LockButtonState(int index){
		return _voteUIPrefabArr[index].GetComponent<VoteYesNo>().LockButtonState();
	}
	public virtual void ResetButtonState(int index){
		_voteUIPrefabArr[index].GetComponent<VoteYesNo>().ResetButtonState();
	}
	public void ResetAllButtonState(){
		for(int i=0; i<4; i++){
			ResetButtonState(i);
		}
	}
}
