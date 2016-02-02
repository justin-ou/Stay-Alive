using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class JamODrumManager : MonoBehaviour {

	public JamoDrum jod;
	public GameObject star;

	public GameObject[] spinners 		= new GameObject[4];
	public Material[] 	starMaterials 	= new Material[4];
	public float[] 		degPerTick 		= new float[4];
	public float[] 		spinnerAngle	= new float[4];


	private VoteOptions[]	_voteOptions = new VoteOptions[4];

	[SerializeField]
	private bool[] _isDrumHit = new bool[4];

	[SerializeField]
	private float[] _initAngle = new float[4]; 

	private int _currentDrumIndex = 0;

	// Use this for initialization
	void Start () {
		for(int i=0; i<4; i++) {
			_initAngle[i] = spinners[i].transform.rotation.eulerAngles.y;
			_isDrumHit[i] = false;
		}
		
		jod.AddHitEvent(HitHandler);
		jod.AddSpinEvent(SpinHandler);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyUp(KeyCode.Escape)){
			Application.Quit();
		}
	}

	void ToggleDrumHit(int controllerID){
		if(_isDrumHit[controllerID-1]){
			_isDrumHit[controllerID-1] = false;
		}else{
			_isDrumHit[controllerID-1] = true;
		}
	}

	public void SpinHandler(int controllerID, int delta) {
		//Debug.Log("SPIN EVENT "+(controllerID-1));
		int i = controllerID - 1;
		//Debug.Log("Controller "+controllerID+": "+_isDrumHit[i]);
		if(IsControllerActive(controllerID) && IsCharacterControllerActive(controllerID)){
			float updatedAngle = spinnerAngle[i] + delta * degPerTick[i];

			if(updatedAngle < Constants.WHEEL_MIN_TURN_RADIUS || updatedAngle > Constants.WHEEL_MAX_TURN_RADIUS){
				spinnerAngle[i] = updatedAngle;
				spinnerAngle[i] = Mathf.Repeat(spinnerAngle[i], 360);
				Vector3 rot = spinners[i].transform.rotation.eulerAngles;
				rot.y = _initAngle[i] + spinnerAngle[i];
				spinners[i].transform.rotation = Quaternion.Euler(rot);
				
				_voteOptions[i] = DrumStateManager.Instance.GetSelectedVoteOption(spinnerAngle[i], i);
			}
		}
	}
	
	public void HitHandler(int controllerID) {
		int i = controllerID - 1;
		_currentDrumIndex = i;
		//Debug.Log("HIT EVENT "+(controllerID-1)+" | "+_isDrumHit[i]);
		if(VoteManager.Instance.IsVoteEnabled() && IsCharacterControllerActive(controllerID)){
			_voteOptions[i] = DrumStateManager.Instance.GetSelectedVoteOption(spinnerAngle[i], i);
			if(VoteManager.Instance.IsVoteValid(_voteOptions[i], controllerID)){
				if(IsControllerActive(controllerID)){
					VoteManager.Instance.AddVoteCount(_voteOptions[i], controllerID);
					UpdateTapToStartInstruction(true, i);
				}else{
					VoteManager.Instance.ReduceVoteCount(_voteOptions[i], controllerID);
					UpdateTapToStartInstruction(false, i);
				}

				// Change the state of the drum. Can't hit the drum twice
				ToggleDrumHit(controllerID); 
			}
		}
	}

	public void ResetHitState(){

		for(int i=0; i<_isDrumHit.Length; i++){
			_isDrumHit[i] = false;
			//_voteOptions[i] = DrumStateManager.Instance.GetSelectedVoteOption(spinnerAngle[i], i);
		}
	}

	void UpdateTapToStartInstruction(bool isSelected, int index){
		if(!GameManager.Instance.IsStartGame()){
			if(isSelected){
				UIManager.Instance.UpdateInstruction(InstructionState.READY_6, (CharacterIndex) index);
			}else{
				UIManager.Instance.UpdateInstruction(InstructionState.TAP_TO_START_0, (CharacterIndex) index);
			}
		}
	}
	bool IsControllerActive(int controllerID){
		if(GameManager.Instance.IsGameOver()){
			return false;
		}
		return !_isDrumHit[controllerID-1];
	}
	bool IsCharacterControllerActive(int controllerID){
		if(GameManager.Instance.IsStartGame()){
			//Debug.Log("Drum "+controllerID+" Hit: "+_isDrumHit[controllerID-1]);
			return !CharacterManager.Instance.IsCharacterDead(controllerID);
		}
		return true;
	}
}
