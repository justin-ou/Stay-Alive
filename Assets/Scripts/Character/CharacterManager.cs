using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterManager : Singleton<CharacterManager> {

	private Ration _ration;
	private GameObject _characterPrefab;
	private List<Character> _characterList;
	private Character _characterToEat;

	void Awake(){
		_characterPrefab = Resources.Load ("Prefabs/Character") as GameObject;
	}

	public void Init(){
		_characterList = new List<Character>();
		_ration = new Ration();

		// Add characters to this list
		_characterList.Add(AddCharacter(0, VoteOptions.CHAR_1));
		_characterList.Add(AddCharacter(1, VoteOptions.CHAR_2));
		_characterList.Add(AddCharacter(2, VoteOptions.CHAR_3));
		_characterList.Add(AddCharacter(3, VoteOptions.CHAR_4));

		// Initial vote options for Characters
		_characterToEat = null;
	}

	public void Reset(){
		_characterList.Clear();
		_characterToEat = null;
		_characterList = null;
		_ration = null;
	}

	Character AddCharacter(int index, VoteOptions selfVoteOption){
		GameObject characterObject = UIManager.Instance.InstantiatePrefab(_characterPrefab, UIManager.Instance.GetCharacterPosition(index));
		Character characterScript = characterObject.GetComponent<Character>();
		characterScript.Init(_characterList.Count, UIManager.Instance.GetDrumObjectPosition(index), selfVoteOption);
		return characterScript;
	}

	public void SetEatAmount(int index, int amountToEat){
		_characterList[index].SetEatAmount(amountToEat);
	}

	public void ReduceHunger(){
		foreach(Character character in _characterList){
			character.ReduceHunger();
		}
	}

	public void AddHunger(){
		float[] votedTimeArray = new float[4];
		int[] characterIndexArray = new int[4];

		for(int i=0; i<4; i++){
			votedTimeArray[i] = _characterList[i].GetVotedTime();
			characterIndexArray[i] = i;
		}

		// Loop through to sort the votedTimeArray
		for(int i=0; i<4; i++){
			int minIndex = i;
			float minTime = votedTimeArray[i];

			for(int j=i+1; j<4; j++){
				if(votedTimeArray[j] < minTime){
					minTime = votedTimeArray[j];
					minIndex = j;
				}
			}

			int tempCharacterIndex = characterIndexArray[i];
			float tempTime = votedTimeArray[i];
			votedTimeArray[i] = minTime;
			characterIndexArray[i] = characterIndexArray[minIndex];
			votedTimeArray[minIndex] = tempTime;
			characterIndexArray[minIndex] = tempCharacterIndex;

			//Debug.Log("Swap Character "+characterIndexArray[minIndex]+" | Time: "+minTime);
			//Debug.Log("Character "+tempCharacterIndex+" | Time: "+tempTime);
		}

		// Add hunger for whoever chooses first
		for(int i=0; i<4; i++){
			int consumed = _characterList[characterIndexArray[i]].AddHunger(_ration.GetRationAmount());
			Debug.Log("Character "+characterIndexArray[i]+" Eat: "+consumed);
			_ration.EatRation(consumed);
			SetEatAmount (characterIndexArray[i], 0); // Reset the eat amount for the character
		}
	}
	public void FlagCharacterToEat(VoteOptions majorityVote){
		switch(majorityVote){
		case VoteOptions.CHAR_1:
			_characterToEat = _characterList[0];
			break;
		case VoteOptions.CHAR_2:
			_characterToEat = _characterList[1];
			break;
		case VoteOptions.CHAR_3:
			_characterToEat = _characterList[2];
			break;
		case VoteOptions.CHAR_4:
			_characterToEat = _characterList[3];
			break;
		default:
			break;
		}
	}
	public bool CheckEatCharacter(){
		if(_characterToEat != null){
			int nutritionAmount = _characterToEat.GetNutritionAmount();
			for(int i=0; i<4; i++){
				if(_characterList[i] != _characterToEat){
					_characterList[i].AddFixedHunger(nutritionAmount);
				}
			}
			_characterToEat.DieFromCannibalism();
			// Set characterToEat to null after eating
			_characterToEat = null;
			return true;
		}
		return false;
	}
	public int GetEatCharacterIndex(){
		if(_characterToEat != null){
			return _characterToEat.GetCharacterIndex();
		}
		return -1;
	}
	public void CheckCharacterDying(){
		if(_characterList != null){
			for(int i=0; i<4; i++){
				if(_characterList[i].IsCharacterDying()){
					UIManager.Instance.UpdateInstruction(InstructionState.STARVING_DIE_10, (CharacterIndex)i);
				}
			}
		}
	}

	public int GetNumOfCharactersAlive(){
		int count = 0;
		if(_characterList != null){
			foreach(Character character in _characterList){
				if(!character.IsCharacterDead()){
					count++;
				}
			}
		}
		Debug.Log("Characters Alive: "+count);
		return count;
	}

	public Character GetCharacter(int index){
		if(index < 0 || index >= _characterList.Count)
			return null;
		return _characterList[index];
	}
	public bool IsCharacterDead(int controllerID) { 
		return _characterList[controllerID-1].IsCharacterDead();
	}
	public bool HasRationRemaining() { return _ration.HasRation(); }
	public int GetCharacterCount() { return _characterList.Count; }
}
