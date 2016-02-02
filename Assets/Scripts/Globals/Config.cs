using UnityEngine;
using System.Collections;

public class Config : MonoBehaviour {

	public static bool IS_EMULATOR = false;
	public static bool CAN_PLAY_VIDEO = true;

	public static void TOGGLE_CAN_PLAY_VIDEO(){
		if(CAN_PLAY_VIDEO){
			CAN_PLAY_VIDEO = false;
		}else{
			CAN_PLAY_VIDEO = true;
		}
	}
}
