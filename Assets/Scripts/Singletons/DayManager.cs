using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DayManager : Singleton<DayManager> {

	private GameObject _daysDisplayPrefab;
	private Image _imageObject;
	private Text[] _messageText;
	private int _daysToRescueShip, _fixedRescueShip, _currentDay;
	private float _fadeSpeed;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void NextDay(){
		SoundManager.Instance.BGMThemesStop();
		SoundManager.Instance.BGMBeatStop();
		_daysToRescueShip--;
		_fixedRescueShip--;
		_currentDay++;
		Debug.Log ("Day "+_currentDay+" | Rescue Ship in "+_daysToRescueShip);
		SetMessageText("Week "+_currentDay);

		// Fade out display and fade in text
		FadeToBlack();
	}

	public bool HasShipArrived(){
		if(_daysToRescueShip == 0){
			return true;
		}
		return false;
	}
	
	public void Init(){
		_currentDay = 0;
		_fixedRescueShip = Constants.SHIP_MIN_DAYS;
		_daysToRescueShip =  Random.Range(Constants.SHIP_MIN_DAYS, Constants.SHIP_MAX_DAYS);
		if(Config.IS_EMULATOR){
			_daysToRescueShip = 3;
		}
		_daysDisplayPrefab = Resources.Load("Prefabs/DaysDisplay") as GameObject;
		GameObject daysDisplay = Instantiate(_daysDisplayPrefab) as GameObject;
		daysDisplay.transform.SetParent(UIManager.Instance.GetUICanvas(), false);
		_imageObject = daysDisplay.GetComponentInChildren<Image>();
		_fadeSpeed = Constants.GAME_FADE_SPEED;

		InitText(daysDisplay.GetComponentsInChildren<Text>());
	}

	public void FadeDayOut() { 
		StartCoroutine("CoFadeOut"); 
	}
	public void FadeToBlack() { 
		StartCoroutine("CoFadeOutImageOnly"); 
	}
	public void FadeDayIn() { 
		SetAlpha(1f);
		StartCoroutine("CoFadeIn"); 
	}
	public void FadeSurvivors(int numOfSurvivors){
		if(numOfSurvivors == 1){
			SetMessageText(numOfSurvivors+" person was rescued..");
		}else{
			SetMessageText(numOfSurvivors+" people were rescued..");
		}
		FadeDayOut();
	}
	public void FadeGameOver(){
		SetMessageText("Nobody survived..");
		FadeDayOut();
	}
	public void FadeShipText(){		
		StartCoroutine("CoFadeInOutShipText");
	}
	public void FadeToShowText(){
		StartCoroutine("CoFadeOutTextOnly");
	}

	void SetAlpha(float alpha){
		Color imageColor = _imageObject.color;	
		imageColor.a = alpha;
		_imageObject.color = imageColor;
	}
	void InitText(Text[] textInObj){
		int pointer = 0;
		_messageText = new Text[2];
		foreach(Text textObj in textInObj){
			if(textObj.name == "Message"){
				_messageText[pointer] = textObj;
				pointer++;
			}
		}
	}
	void SetMessageText(string newText){
		foreach(Text textObj in _messageText){
			textObj.text = newText;
		}
	}
	void SetMessageColor(Color newColor){
		foreach(Text textObj in _messageText){
			textObj.color = newColor;
		}
	}
	Color GetMessageColor(){
		if(_messageText.Length > 0)
			return _messageText[0].color;
		return Color.black;
	}
	string GetShipArrivingText(){
		string shipRescueText = "Rescue ship should come in \n"+_fixedRescueShip+" weeks..\n";;
		if(_fixedRescueShip == 1){
			shipRescueText = "Rescue ship should come in \n1 WEEK..\n";
		}else if(_fixedRescueShip <= 0){
			_fixedRescueShip = 0;
			shipRescueText = "Rescue ship is not here yet..";
		}
		return shipRescueText;
	}

	IEnumerator CoFadeOut(){
		while(true){
			Color imageColor = _imageObject.color;
			Color textColor = GetMessageColor();

			imageColor.a += _fadeSpeed;
			textColor.a += _fadeSpeed;

			yield return new WaitForSeconds(0.005f);
						
			if(imageColor.a >= 0.99f){
				imageColor.a = 1;
				textColor.a = 1;
				StopCoroutine("CoFadeOut");
			}

			_imageObject.color = imageColor;
			SetMessageColor(textColor);
		}
	}
	IEnumerator CoFadeIn(){
		while(true){
			Color imageColor = _imageObject.color;
			Color textColor = GetMessageColor();
			
			imageColor.a -= _fadeSpeed;
			textColor.a -= _fadeSpeed;

			yield return new WaitForSeconds(0.005f);
			
			if(imageColor.a < 0.01f){
				imageColor.a = 0;
				textColor.a = 0;
				StopCoroutine("CoFadeIn");
			}

			_imageObject.color = imageColor;
			SetMessageColor(textColor);
		}
	}
	IEnumerator CoFadeOutImageOnly(){
		while(true){
			Color imageColor = _imageObject.color;
			
			imageColor.a += _fadeSpeed;
			
			yield return new WaitForSeconds(0.005f);
			
			if(imageColor.a >= 0.99f){
				imageColor.a = 1;
				StopCoroutine("CoFadeOutImageOnly");
			}
			
			_imageObject.color = imageColor;
		}
	}
	IEnumerator CoFadeOutTextOnly(){
		while(true){
			Color textColor = GetMessageColor();
			
			textColor.a += _fadeSpeed;
			
			yield return new WaitForSeconds(0.005f);
			
			if(textColor.a >= 0.99f){
				textColor.a = 1;
				StopCoroutine("CoFadeOutTextOnly");
			}
			
			SetMessageColor(textColor);
		}
	}
	IEnumerator CoFadeInOutShipText(){
		while(true){
			Color textColor = GetMessageColor();

			textColor.a -= _fadeSpeed;
			
			yield return new WaitForSeconds(0.005f);
			
			if(textColor.a >= 0.995f){
				textColor.a = 1;
				_fadeSpeed *= -1;
				StopCoroutine("CoFadeInOutShipText");
			}else if(textColor.a < 0.005f){
				_fadeSpeed *= -1;
				SetMessageText(GetShipArrivingText());
			}

			SetMessageColor(textColor);
		}
	}

	public int GetCurrentDay() {	return _currentDay; }
}
