using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterUI : MonoBehaviour {

	public SpriteRenderer spriteRenderer;
	public SpriteRenderer emotionRenderer;
	public Transform emotionTransform;

	private Transform _parentTransform;
	private GameObject _hungerLevelPrefab, _hungerLevelTextPrefab;
	private Material _hungerLevelBar, _hungerLevelControl;
	private Texture[] _hungerBar;
	private Sprite[] _characterSprite, _emotionSprite, _hungerHUDSprite;
	private Sprite _bloodSprite;
	private Text _hungerLevelText;
	private float _emotionSize, _emotionScaleRate;

	private int _uiFlashCounter;

	// Use this for initialization
	void Start () {
		// Load Resources
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void SetCharacterStateUI(CharacterState characterState){
		switch(characterState){
		case CharacterState.FULL:
			spriteRenderer.sprite = _characterSprite[3];
			emotionRenderer.sprite = _emotionSprite[3];
			SetEmotionScaleRate(0.005f);
			_hungerLevelBar.mainTexture = _hungerBar[(int)HungerBarState.HUNGER_BAR_100];
			break;
		case CharacterState.SATISFIED:
			spriteRenderer.sprite = _characterSprite[2];
			emotionRenderer.sprite = _emotionSprite[2];
			SetEmotionScaleRate(0.006f);
			_hungerLevelBar.mainTexture = _hungerBar[(int)HungerBarState.HUNGER_BAR_60];
			break;
		case CharacterState.HUNGRY:
			spriteRenderer.sprite = _characterSprite[1];
			emotionRenderer.sprite = _emotionSprite[1];
			SetEmotionScaleRate(0.008f);
			_hungerLevelBar.mainTexture = _hungerBar[(int)HungerBarState.HUNGER_BAR_30];
			break;
		case CharacterState.DYING:
			spriteRenderer.sprite = _characterSprite[0];
			emotionRenderer.sprite = _emotionSprite[0];
			SetEmotionScaleRate(0.012f);
			_hungerLevelBar.mainTexture = _hungerBar[(int)HungerBarState.HUNGER_BAR_0];
			break;
		default:
			break;
		}
	}
	public void SetHungerLevelUI(int hungerLevel){
		float hungerPercent = (float) hungerLevel/Constants.CHARACTER_FULL_MAX;
		SetHungerLevelControl(Constants.HUNGER_MIN_ALPHA_CUTOFF + hungerPercent*Constants.HUNGER_DIFF_ALPHA_CUTOFF);
		_hungerLevelText.text = hungerLevel+"%";
		StartCoroutine("CoFlashCharacterUI");

	}
	public void InitCharacterUI(int index, Transform parent){
		_uiFlashCounter = Constants.UI_FLASH_COUNT;
		_parentTransform = parent;
		_emotionSize = Constants.CHARACTER_EMOTION_MAX_SCALE;
		_emotionScaleRate = -Constants.CHARACTER_EMOTION_SCALERATE;
		_bloodSprite = Resources.Load<Sprite>("Textures/Character/blood");
		InitCharacterSprite(index);
		InitHungerBarSprite();
		InitEmotionSprite();
		InitHungerLevelUI(parent, index);
		ScaleEmotion();
	}
	public void HideCharacterUI(bool isEaten){
		if(isEaten){
			spriteRenderer.sprite = _bloodSprite;
		}else{
			spriteRenderer.sprite = null;
		}
		emotionRenderer.sprite = null;
		StopCoroutine("CoScaleEmotion");
	}

	void InitCharacterSprite(int index){
		switch(index){
		case (int)CharacterIndex.CHAR1:
			LoadSprite("Textures/Character/Character 1");
			break;
		case (int)CharacterIndex.CHAR2:
			LoadSprite("Textures/Character/Character 2");
			break;
		case (int)CharacterIndex.CHAR3:
			LoadSprite("Textures/Character/Character 3");
			break;
		case (int)CharacterIndex.CHAR4:
			LoadSprite("Textures/Character/Character 4");
			break;
		default:
			break;
		}
	}
	void InitEmotionSprite(){
		_emotionSprite = new Sprite[4];
		_emotionSprite[0] = Resources.Load<Sprite>("Textures/Character/dying status");
		_emotionSprite[1] = Resources.Load<Sprite>("Textures/Character/bad status");
		_emotionSprite[2] = Resources.Load<Sprite>("Textures/Character/medium status");
		_emotionSprite[3] = Resources.Load<Sprite>("Textures/Character/happy status");
	}
	void InitHungerBarSprite(){
		_hungerBar = new Texture[(int)HungerBarState.HUNGER_BAR_SIZE];
		_hungerBar[(int)HungerBarState.HUNGER_BAR_0] = Resources.Load ("Textures/Drum/fullbar 0") as Texture;
		_hungerBar[(int)HungerBarState.HUNGER_BAR_30] = Resources.Load ("Textures/Drum/fullbar 30") as Texture;
		_hungerBar[(int)HungerBarState.HUNGER_BAR_60] = Resources.Load ("Textures/Drum/fullbar 60") as Texture;
		_hungerBar[(int)HungerBarState.HUNGER_BAR_100] = Resources.Load ("Textures/Drum/fullbar 100") as Texture;
	}
	void InitHungerLevelHUD(int index){
		_hungerHUDSprite = new Sprite[4];
		_hungerHUDSprite[(int)CharacterIndex.CHAR1] = Resources.Load<Sprite>("Textures/Character/Heads/Character1 HUD") as Sprite;
		_hungerHUDSprite[(int)CharacterIndex.CHAR2] = Resources.Load<Sprite>("Textures/Character/Heads/Character2 HUD") as Sprite;
		_hungerHUDSprite[(int)CharacterIndex.CHAR3] = Resources.Load<Sprite>("Textures/Character/Heads/Character3 HUD") as Sprite;
		_hungerHUDSprite[(int)CharacterIndex.CHAR4] = Resources.Load<Sprite>("Textures/Character/Heads/Character4 HUD") as Sprite;

		_hungerLevelTextPrefab = Resources.Load("Prefabs/HungerLevelText") as GameObject;
		GameObject hungerLevelText = UIManager.Instance.InstantiatePrefab(_hungerLevelTextPrefab, UIManager.Instance.GetCanvasPosition((CharacterIndex)index));
		_hungerLevelText = hungerLevelText.GetComponent<Text>();
	}
	void InitHungerLevelUI(Transform parent, int index){
		InitHungerLevelHUD(index);

		_hungerLevelPrefab = Resources.Load ("Prefabs/HungerLevel") as GameObject;
		GameObject hungerLevelObject = UIManager.Instance.InstantiatePrefab(_hungerLevelPrefab, UIManager.Instance.GetDrumObjectPosition(index));
		_hungerLevelBar = hungerLevelObject.transform.Find("Bar").GetComponent<Renderer>().material;
		_hungerLevelControl = hungerLevelObject.transform.Find("Control").GetComponent<Renderer>().material;
		hungerLevelObject.transform.Find("HUD").GetComponent<SpriteRenderer>().sprite = _hungerHUDSprite[index];
		SetHungerLevelControl(Constants.HUNGER_MAX_ALPHA_CUTOFF);
	}
	void LoadSprite(string pathName){
		_characterSprite = new Sprite[4];
		_characterSprite[0] = Resources.Load<Sprite>(pathName+"/0");
		_characterSprite[1] = Resources.Load<Sprite>(pathName+"/1-30");
		_characterSprite[2] = Resources.Load<Sprite>(pathName+"/31-60");
		_characterSprite[3] = Resources.Load<Sprite>(pathName+"/61-100");
	}
	void SetHungerLevelControl(float alphaCutoff){
		_hungerLevelControl.SetFloat("_Cutoff", alphaCutoff);
	}
	void SetEmotionScaleRate(float scaleRate){
		if(_emotionScaleRate < 0){
			_emotionScaleRate = -scaleRate;
		}else{
			_emotionScaleRate = scaleRate;
		}
	}
	void ScaleEmotion(){
		StartCoroutine("CoScaleEmotion");
	}
	IEnumerator CoScaleEmotion(){
		while(true){
			if(emotionTransform != null){
				_emotionSize += _emotionScaleRate;

				if(_emotionSize > Constants.CHARACTER_EMOTION_MAX_SCALE && _emotionScaleRate > 0){
					_emotionScaleRate *= -1;
				}else if(_emotionSize < Constants.CHARACTER_EMOTION_MIN_SCALE && _emotionScaleRate < 0){
					_emotionScaleRate *= -1;
				}
				emotionTransform.localScale = new Vector3(_emotionSize, _emotionSize, _emotionSize);
			}
			yield return new WaitForSeconds(0.05f);
		}
	}

	void HideCharacterUIDisplay(bool hide){
		Color newUIColor = new Color(1f, 1f, 1f, 1f);
		if(hide){
			newUIColor.a = 0f;
		}
		_hungerLevelBar.color = newUIColor;
		_hungerLevelText.color = newUIColor;
	}
	IEnumerator CoFlashCharacterUI(){
		while(true){
			// Hide UI
			HideCharacterUIDisplay(false);
			
			// Wait for 0.05sec
			yield return new WaitForSeconds(Constants.UI_FLASH_DURATION);
			
			// Show UI
			HideCharacterUIDisplay(true);
			
			yield return new WaitForSeconds(Constants.UI_FLASH_DURATION);
			
			// Stop coroutine
			if(_uiFlashCounter == 0){
				_uiFlashCounter = Constants.UI_FLASH_COUNT;
				StopCoroutine("CoFlashCharacterUI");
			}else{
				_uiFlashCounter--;
			}
		}
	}
}
