using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InstructionUI : MonoBehaviour {

	private GameObject _instructionPrefab;
	private Sprite[] _instructionSpriteList;
	private SpriteRenderer[] _instructionList;
	
	// Use this for initialization
	void Start () {
		_instructionPrefab = Resources.Load("Prefabs/Instruction") as GameObject;
		InitInstructionSpriteList();
		InitInstructionList();
	}
	public void HideInstructionList(){
		if(_instructionList != null){
			for(int i=0; i<(int)CharacterIndex.SIZE; i++){
				_instructionList[i].gameObject.SetActive(false);
			}
		}
	}
	public void UpdateInstruction(InstructionState instructionState){
		for(int i=0; i<(int)CharacterIndex.SIZE; i++){
			_instructionList[i].sprite = _instructionSpriteList[(int)instructionState];
		}
	}
	public void UpdateInstruction(InstructionState instructionState, CharacterIndex charIndex){
		_instructionList[(int)charIndex].sprite = _instructionSpriteList[(int)instructionState];
	}
	public void UpdateInstructionSize(bool isSelected, CharacterIndex charIndex){
		//_instructionList[(int)charIndex].sprite = _instructionSpriteList[(int)instructionState];
		if(isSelected){
			IncreaseInstructionSize(charIndex);
		}else{
			ResetInstructionSize(charIndex);
		}
	}
	void IncreaseInstructionSize(CharacterIndex charIndex){
		_instructionList[(int)charIndex].gameObject.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
	}
	void ResetInstructionSize(CharacterIndex charIndex){
		_instructionList[(int)charIndex].gameObject.transform.localScale = new Vector3(2f, 2f, 2f);
	}
	void InitInstructionList(){
		_instructionList = new SpriteRenderer[(int)CharacterIndex.SIZE];
		for(int i=0; i<(int)CharacterIndex.SIZE; i++){
			_instructionList[i] = AddNewInstruction(UIManager.Instance.GetDrumObjectPosition(i));
			_instructionList[i].sprite = _instructionSpriteList[(int)InstructionState.TAP_TO_START_0];
		}
	}
	void InitInstructionSpriteList(){
		_instructionSpriteList = new Sprite[(int)InstructionState.INSTRUCTION_SIZE];
		_instructionSpriteList[(int)InstructionState.TAP_TO_START_0] = Resources.Load<Sprite>("Textures/Instructions/0");
		_instructionSpriteList[(int)InstructionState.SHOULD_EAT_TODAY_1] = Resources.Load<Sprite>("Textures/Instructions/1");
		_instructionSpriteList[(int)InstructionState.VOTE_NOT_TO_EAT_2] = Resources.Load<Sprite>("Textures/Instructions/2");
		_instructionSpriteList[(int)InstructionState.VOTE_TO_EAT_3] = Resources.Load<Sprite>("Textures/Instructions/3");
		_instructionSpriteList[(int)InstructionState.RESCUE_SHIP_LONG_4] = Resources.Load<Sprite>("Textures/Instructions/4");
		_instructionSpriteList[(int)InstructionState.OUT_OF_FOOD_5] = Resources.Load<Sprite>("Textures/Instructions/5");
		_instructionSpriteList[(int)InstructionState.READY_6] = Resources.Load<Sprite>("Textures/Instructions/6");
		_instructionSpriteList[(int)InstructionState.FIRST_COME_FIRST_SERVE_7] = Resources.Load<Sprite>("Textures/Instructions/7");
		_instructionSpriteList[(int)InstructionState.DISCUSS_8] = Resources.Load<Sprite>("Textures/Instructions/8");
		_instructionSpriteList[(int)InstructionState.FOOD_SNATCHED_9] = Resources.Load<Sprite>("Textures/Instructions/9");
		_instructionSpriteList[(int)InstructionState.STARVING_DIE_10] = Resources.Load<Sprite>("Textures/Instructions/10");
	}
	SpriteRenderer AddNewInstruction(Transform parent){
		if(_instructionPrefab == null){
			_instructionPrefab = Resources.Load("Prefabs/Instruction") as GameObject;
		}
		GameObject instruction = UIManager.Instance.InstantiatePrefab(_instructionPrefab, parent);
		return instruction.GetComponent<SpriteRenderer>();
	}
}
