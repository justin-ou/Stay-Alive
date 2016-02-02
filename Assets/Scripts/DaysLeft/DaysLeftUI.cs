using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DaysLeftUI {
	
	private Text[] _textPrefabList;

	public DaysLeftUI(){	
		_textPrefabList = new Text[(int)CharacterIndex.SIZE];
	}

	public void Init(){
		GameObject weekNumberTextPrefab = Resources.Load("Prefabs/WeekNumberText") as GameObject;
		InitText(weekNumberTextPrefab);
	}
	public void Reset(){

	}

	public void SetText(int amountRemaining){
		for(int i=0; i<_textPrefabList.Length; i++){
			if(_textPrefabList[i] != null){
				_textPrefabList[i].text = "Week "+amountRemaining;
			}
		}
	}
	public void HideText(){
		if(_textPrefabList != null){
			for(int i=0; i<4; i++){
				if(_textPrefabList[i] != null){
					Color color = _textPrefabList[i].color;
					color.a = 0;
					_textPrefabList[i].color = color;
				}
			}
		}
	}
	public void ShowText(){
		for(int i=0; i<4; i++){
			if(_textPrefabList[i] != null){
				Color color = _textPrefabList[i].color;
				color.a = 1;
				_textPrefabList[i].color = color;
			}
		}
	}
	void InitText(GameObject textPrefab){
		int numOfCharacters = (int)CharacterIndex.SIZE;
		for(int i=0; i<numOfCharacters; i++){
			GameObject textObject = UIManager.Instance.InstantiatePrefab(textPrefab, UIManager.Instance.GetCanvasPosition((CharacterIndex)i));
			_textPrefabList[i] = textObject.GetComponent<Text>();
			_textPrefabList[i].text = "Week 1";
		}
	}
	void HideUIDisplay(bool hide){
		Color newUIColor = new Color(1f, 1f, 1f, 1f);
		if(hide){
			newUIColor.a = 0f;
		}
		for(int i=0; i<(int)CharacterIndex.SIZE; i++){
			_textPrefabList[i].color = newUIColor;
		}
	}
}
