using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (CharacterUI))]
public class Character : MonoBehaviour {
	
	private int _hungerLevel;			// Current hunger level
	private int _deathLevel; 			// Duration the character can survive at max hunger
	private int _originalDeathLevel;	// Original value of death level
	private int _hungerDecreaseRate;	// Rate at which the hunger level decreases
	private int _nutritionAmount;		// How much food is this character worth?

	private int _characterIndex;
	private int _votedEatAmount;
	private float _votedTime;

	private Transform _parentTransform;
	private CharacterState _characterState;
	private CharacterUI _characterUI;
	private VoteOptions _selfVoteOption;

	// Use this for initialization
	void Start () {
		_hungerLevel = Constants.CHARACTER_STARTING_HUNGER;
		_hungerDecreaseRate = Random.Range(Constants.CHARACTER_MIN_HUNGERRATE, Constants.CHARACTER_MAX_HUNGERRATE);
		_originalDeathLevel = Random.Range(Constants.CHARACTER_MIN_DEATHRATE, Constants.CHARACTER_MAX_DEATHRATE);
		_deathLevel = _originalDeathLevel;
		InitEatAmount();
	
		// Instantiate a prefab here
		_characterUI = GetComponent<CharacterUI>();
		_characterUI.InitCharacterUI(_characterIndex, _parentTransform);
		SetCharacterState();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	// Set the character state based on its hunger level
	void SetCharacterState(){
		if(!IsCharacterDead()){
			_characterUI.SetHungerLevelUI(_hungerLevel);
			// Debug.Log ("Hunger Level: "+_hungerLevel);
			if(_hungerLevel > Constants.CHARACTER_FULL){
				_characterState = CharacterState.FULL;
				_nutritionAmount = Constants.NUTRITION_HEALTHY;
			}else if(_hungerLevel >= Constants.CHARACTER_SATISFIED){
				_characterState = CharacterState.SATISFIED;
				_nutritionAmount = Constants.NUTRITION_HEALTHY;
			}else if(_hungerLevel >= Constants.CHARACTER_HUNGRY){
				_characterState = CharacterState.HUNGRY;
				_nutritionAmount = Constants.NUTRITION_NORMAL;
			}else if(_hungerLevel <= Constants.NUTRITION_DYING){
				_characterState = CharacterState.DYING;
				_nutritionAmount = Constants.NUTRITION_DYING;
			}
			_characterUI.SetCharacterStateUI(_characterState);
		}
	}
	void SetCharacterDeath(bool isEaten){
		if(!IsCharacterDead()){
			_nutritionAmount = 0;
			_characterState = CharacterState.DEAD;
			_characterUI.HideCharacterUI(isEaten);
			VoteManager.Instance.ReduceMinVoteCount();
			UIManager.Instance.HideDrumPosition(_characterIndex);
		}
	}
	public void DieFromCannibalism(){
		SetCharacterDeath(true);

		// Do something when eaten
		// Play animation or sound effect
	}
	public void ReduceDeath(){
		if(!IsCharacterDead()){
			if(_deathLevel > 0){
				_deathLevel -= 1;
			}else{
				SetCharacterDeath(false);
				GameManager.Instance.StartPlayingVideo(VideoState.VIDEO_FIRST_BLOOD_2);
			}
		}
	}
	public void ReduceHunger(){
		if(!IsCharacterDead()){
			if(_characterState != CharacterState.DEAD){
				SetHunger(-_hungerDecreaseRate);
				SetCharacterState();
			}
		}
	}
	public int AddHunger(int rationRemaining){
		if(!IsCharacterDead()){
			//Debug.Log ("Character "+_characterIndex+" AddHunger: "+_votedEatAmount);
			if(_votedEatAmount > rationRemaining){
				_votedEatAmount = rationRemaining; // Reduce the amount to eat if there is not enough left
				UIManager.Instance.UpdateInstruction(InstructionState.FOOD_SNATCHED_9, (CharacterIndex)_characterIndex);
			}
			AddFixedHunger(Constants.CHARACTER_RATION_RECOVERY * _votedEatAmount);
			return _votedEatAmount;
		}
		return 0;
	}
	public void AddFixedHunger(int amount){
		if(!IsCharacterDead()){
			SetHunger(amount);
			
			// Reset death level everytime food is given to the character
			_deathLevel = _originalDeathLevel;		
			SetCharacterState();
		}
	}
	void SetHunger(int amount){
		if(!IsCharacterDead()){
			_hungerLevel += amount;
			if(_hungerLevel > Constants.CHARACTER_FULL_MAX){
				_hungerLevel = Constants.CHARACTER_FULL_MAX;
			}
			if(_hungerLevel < Constants.CHARACTER_DYING){
				_hungerLevel = Constants.CHARACTER_DYING;
				ReduceDeath ();
			}
		}
	}

	public void InitEatAmount(){
		_votedEatAmount = 0;
		_votedTime = 0f;
	}
	public void Init(int index, Transform parent, VoteOptions selfVoteOption){
		_characterIndex = index;
		_parentTransform = parent;
		_selfVoteOption = selfVoteOption;
	}

	public void SetEatAmount(int amount){
		_votedEatAmount = amount;
		_votedTime = Time.time;
	}

	public bool IsCharacterDead() { return _characterState == CharacterState.DEAD; }
	public bool IsCharacterDying() { return _characterState == CharacterState.DYING; }

	public int GetNutritionAmount() { return _nutritionAmount; }
	public int GetCharacterIndex() { return _characterIndex; }
	public int GetEatAmount() { return _votedEatAmount; }

	public VoteOptions GetSelfVoteOption() { return _selfVoteOption; }

	public CharacterState GetCharacterState()	{ return _characterState;	}
	public float GetVotedTime()					{ return _votedTime;		}
}
