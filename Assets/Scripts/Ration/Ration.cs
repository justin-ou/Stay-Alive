using UnityEngine;
using System.Collections;

public class Ration {

	private int _rationAmount;

	public Ration(){
		// Initialise ration prefab and update prefabs remaining
		_rationAmount = Constants.GAME_STARTING_RATION;
	}

	public void EatRation(int amountConsumed){
		if(_rationAmount > 0){
			_rationAmount -= amountConsumed;
			UIManager.Instance.UpdateRations(amountConsumed, _rationAmount);
		}
		if(_rationAmount <= 0){
			GameManager.Instance.StartPlayingVideo(VideoState.VIDEO_FOOD_RUN_OUT_1);
		}
	}

	public bool HasRation() { return _rationAmount > 0; }
	public int GetRationAmount() { return _rationAmount; }
}
