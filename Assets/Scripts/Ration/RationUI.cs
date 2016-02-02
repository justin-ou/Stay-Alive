using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class RationUI : MonoBehaviour {

	private int _uiFlashCounter;
	private Text[] _rationText;
	private SpriteRenderer[] _rationLeft;
	private GameObject _rationPrefab;
	private List<GameObject> _rationList;

	// Use this for initialization
	void Start () {

	}	
	public void Init(){
		_uiFlashCounter = Constants.UI_FLASH_COUNT;

		_rationPrefab = Resources.Load("Prefabs/Banana") as GameObject;
		GameObject rationLeftPrefab = Resources.Load("Prefabs/RationLeft") as GameObject;
		GameObject rationTextPrefab = Resources.Load("Prefabs/RationText") as GameObject;

		_rationList = new List<GameObject>();
		InitRationText(rationTextPrefab);
		InitRationLeft(rationLeftPrefab);
		for(int i=0; i<Constants.GAME_STARTING_RATION; i++){
			_rationList.Add(AddNewRation());
		}
	}
	public void Reset(){
		RemoveAllRations();
	}
	public void RemoveRation(int amount){
		if(_rationList != null){
			for(int i=0; i<amount; i++){
				RemoveSingleRation();
			}
		}
	}
	void RemoveAllRations(){
		if(_rationList != null){
			for(int i=0; i<_rationList.Count; i++){
				Destroy(_rationList[i]);
			}
		}
	}
	void RemoveSingleRation(){
		if(_rationList.Count > 0){
			GameObject lastRation = _rationList[_rationList.Count-1];
			_rationList.RemoveAt(_rationList.Count-1);
			Destroy(lastRation);
		}
	}

	public void SetRationText(int amountRemaining){
		if(amountRemaining < 0) 
			amountRemaining = 0;
		for(int i=0; i<_rationText.Length; i++){
			_rationText[i].text = amountRemaining+"x";
		}
		StartCoroutine("CoFlashRationUI");
	}
	public void HideRation(){
		if(_rationText != null){
			for(int i=0; i<4; i++){
				Color color = _rationText[i].color;
				color.a = 0;
				_rationText[i].color = color;
			}
		}
	}
	public void ShowRation(){
		for(int i=0; i<4; i++){
			Color color = _rationText[i].color;
			color.a = 1;
			_rationText[i].color = color;
		}
	}
	void InitRationText(GameObject rationTextPrefab){
		int numOfCharacters = (int)CharacterIndex.SIZE;
		_rationText = new Text[numOfCharacters];
		for(int i=0; i<numOfCharacters; i++){
			GameObject rationText = UIManager.Instance.InstantiatePrefab(rationTextPrefab, UIManager.Instance.GetCanvasPosition((CharacterIndex)i));
			_rationText[i] = rationText.GetComponent<Text>();
			_rationText[i].text = Constants.GAME_STARTING_RATION+"x";
		}
	}

	void InitRationLeft(GameObject rationLeftPrefab){
		int numOfCharacters = (int)CharacterIndex.SIZE;
		_rationLeft = new SpriteRenderer[numOfCharacters];
		for(int i=0; i<numOfCharacters; i++){
			_rationLeft[i] = AddRationLeft(i, rationLeftPrefab);
		}
	}
	SpriteRenderer AddRationLeft(int index, GameObject rationLeftPrefab){
		GameObject rationLeft = Instantiate(rationLeftPrefab) as GameObject;
		rationLeft.transform.SetParent(UIManager.Instance.GetDrumObjectPosition(index), false);
		return rationLeft.transform.GetComponentInChildren<SpriteRenderer>();
	}
	GameObject AddNewRation(){
		float xPosition = Random.Range(-1.5f, 0f);
		float zPosition = Random.Range(0f, 2f);
		float yRotation = Random.Range(0f, 360f);
		Vector3 rationPos = new Vector3(xPosition, 0f, zPosition);
		Quaternion rationRot = Quaternion.Euler(new Vector3(90f, yRotation, 0f));
		GameObject ration = UIManager.Instance.InstantiatePrefab(_rationPrefab, this.transform);
		ration.transform.localPosition = rationPos;
		ration.transform.localRotation = rationRot;
		return ration;
	}
	void HideRationUIDisplay(bool hide){
		Color newUIColor = new Color(1f, 1f, 1f, 1f);
		if(hide){
			newUIColor.a = 0f;
		}
		for(int i=0; i<(int)CharacterIndex.SIZE; i++){
			_rationLeft[i].color = newUIColor;
			_rationText[i].color = newUIColor;
		}
	}
	IEnumerator CoFlashRationUI(){
		while(true){
			// Hide UI
			HideRationUIDisplay(false);

			// Wait for 0.05sec
			yield return new WaitForSeconds(Constants.UI_FLASH_DURATION);

			// Show UI
			HideRationUIDisplay(true);

			yield return new WaitForSeconds(Constants.UI_FLASH_DURATION);

			// Stop coroutine
			if(_uiFlashCounter == 0){
				_uiFlashCounter = Constants.UI_FLASH_COUNT;
				StopCoroutine("CoFlashRationUI");
			}else{
				_uiFlashCounter--;
			}
		}
	}
}
