using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : Singleton<UIManager> {

	private GameObject _uiManagerPrefab;
	private RationUI _rationUI;
	private VideoUI _videoUI;
	private DaysLeftUI _daysLeftUI;

	private GameObject _boatUI, _uiManagerObj, _redSpotLightObj;
	private Image _redSpotLightImage;
	private InstructionUI _instructionUI;
	private Transform _drumPosition, _drumPosition2, _drumPosition3, _drumPosition4;
	private Transform[] _drumPositions, _canvasPositions;
	private Transform _uiCanvas;

	void Start(){

	}

	void Awake(){

	}

	public void Init(){
		_drumPositions = new Transform[4];
		_drumPositions[(int)CharacterIndex.CHAR1] = GameObject.Find("JamODrum/Position1").transform;
		_drumPositions[(int)CharacterIndex.CHAR2] = GameObject.Find("JamODrum/Position2").transform;
		_drumPositions[(int)CharacterIndex.CHAR3] = GameObject.Find("JamODrum/Position3").transform;
		_drumPositions[(int)CharacterIndex.CHAR4] = GameObject.Find("JamODrum/Position4").transform;
		_canvasPositions = new Transform[4];
		_canvasPositions[(int)CharacterIndex.CHAR1] = GameObject.Find("UICanvas/Position1").transform;
		_canvasPositions[(int)CharacterIndex.CHAR2] = GameObject.Find("UICanvas/Position2").transform;
		_canvasPositions[(int)CharacterIndex.CHAR3] = GameObject.Find("UICanvas/Position3").transform;
		_canvasPositions[(int)CharacterIndex.CHAR4] = GameObject.Find("UICanvas/Position4").transform;
		_uiCanvas = GameObject.Find ("UICanvas").transform;

		// Add UIManager component
		_uiManagerPrefab = Resources.Load("Prefabs/UIManager") as GameObject;
		_uiManagerObj = Instantiate(_uiManagerPrefab);
		_instructionUI = _uiManagerObj.GetComponent<InstructionUI>();
		_videoUI = _uiManagerObj.GetComponentInChildren<VideoUI>();

		_boatUI = GameObject.Find("Environment/Boat");
		GameObject rationPrefab = Resources.Load("Prefabs/Rations") as GameObject;
		GameObject rationObject = InstantiatePrefab(rationPrefab, _boatUI.transform);
		_rationUI = rationObject.GetComponent<RationUI>();
		_boatUI.SetActive(false);

		_daysLeftUI = new DaysLeftUI();
	}
	public void Reset(){
		_rationUI.Reset();
	}

	public void StartGameInit(){
		_boatUI.SetActive(true);
		_rationUI.Init();
		_daysLeftUI.Init();
	}
	
	public void UpdateRations(int amount, int amountLeft){
		if(amount < 0) amount = 0;
		_rationUI.RemoveRation(amount);
		_rationUI.SetRationText(amountLeft);
	}	
	public void HideInstructionList(){
		_instructionUI.HideInstructionList();
	}
	public void UpdateInstruction(InstructionState instructionState){
		_instructionUI.UpdateInstruction(instructionState);
	}
	public void UpdateInstruction(InstructionState instructionState, CharacterIndex charIndex){
		_instructionUI.UpdateInstruction(instructionState, charIndex);
	}
	public void UpdateInstructionSize(bool isSelected, CharacterIndex charIndex){
		_instructionUI.UpdateInstructionSize(isSelected, charIndex);
	}
	public void UpdateWeeksNumber(int weeksNumber){
		_daysLeftUI.SetText(weeksNumber);
	}
	public bool PlayVideo(VideoState videoState){
		_rationUI.HideRation();
		_daysLeftUI.HideText();
		SetAllCanvasVisibility(false);
		return _videoUI.PlayVideo(videoState);
	}
	public void StopVideo(){
		SetAllCanvasVisibility(true);
		_rationUI.ShowRation();
		_daysLeftUI.ShowText();
		_videoUI.StopVideo();
	}
	public float GetVideoDuration(VideoState videoState){
		return _videoUI.GetVideoDuration(videoState);
	}
	public GameObject InstantiatePrefab(GameObject prefab, Transform parent){
		GameObject newObject = Instantiate(prefab) as GameObject;
		newObject.transform.SetParent(parent, false);
		return newObject;
	}

	public void HideDrumPosition(int index){
		_drumPositions[index].FindChild("DrumObjects").gameObject.SetActive(false);
		//Debug.Log ("Hide Canvas: "+_canvasPositions[index].gameObject.activeInHierarchy);
		_canvasPositions[index].gameObject.SetActive(false);
		//Debug.Log ("Is Canvas Hidden: "+_canvasPositions[index].gameObject.activeInHierarchy);
	}

	public Transform GetDrumObjectPosition(int index){
		return _drumPositions[index].FindChild("DrumObjects");
	}
	public Transform GetDrumPosition(int index){
		return _drumPositions[index];
	}
	public Transform GetCanvasPosition(CharacterIndex index){
		return _canvasPositions[(int)index];
	}
	public Transform GetCharacterPosition(int index){
		int position = index + 1;
		return _boatUI.transform.FindChild("Position"+position);
	}
	public Transform GetUICanvas() { return _uiCanvas; }


	public void AddRedSpotLight(){
		// Add red spot light
		GameObject redSpotLightPrefab = Resources.Load("Prefabs/RedSpotLight") as GameObject;
		_redSpotLightObj = InstantiatePrefab(redSpotLightPrefab, _uiCanvas);
		_redSpotLightImage = _redSpotLightObj.GetComponent<Image>();
		Color color = _redSpotLightImage.color;
		color.a = 0f;
		_redSpotLightImage.color = color;
	}
	public void SetActiveSpotLight(bool isActive, Vector3 position){
		_redSpotLightObj.transform.localPosition = position;
		if(isActive){
			StartCoroutine("CoShowSpotLight");
		}else{
			StartCoroutine("CoHideSpotLight");
		}
	}

	void SetAllCanvasVisibility(bool isShow){
		foreach(Transform canvasTransform in _canvasPositions){
			canvasTransform.gameObject.SetActive(isShow);
		}
	}

	IEnumerator CoShowSpotLight(){
		while(true){
			Color imageColor = _redSpotLightImage.color;
			
			imageColor.a += 0.0015f;
			
			yield return new WaitForSeconds(0.005f);
			
			if(imageColor.a >= 0.99f){
				imageColor.a = 1;
				StopCoroutine("CoShowSpotLight");
			}
			
			_redSpotLightImage.color = imageColor;
		}
	}
	IEnumerator CoHideSpotLight(){
		while(true){
			Color imageColor = _redSpotLightImage.color;
			
			imageColor.a -= 0.005f;
			
			yield return new WaitForSeconds(0.005f);
			
			if(imageColor.a < 0.01f){
				imageColor.a = 0;
				StopCoroutine("CoHideSpotLight");
			}
			
			_redSpotLightImage.color = imageColor;
		}
	}
}
