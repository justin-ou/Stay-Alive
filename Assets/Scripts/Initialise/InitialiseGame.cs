using UnityEngine;
using System.Collections;

public class InitialiseGame : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Debug.Log("GameManager: "+GameManager.Instance);
		GameManager.Instance.Init();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("m")){
			Config.TOGGLE_CAN_PLAY_VIDEO();
		}
	}
}
