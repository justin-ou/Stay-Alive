using UnityEngine;
using System.Collections;

public class Constants : MonoBehaviour {
	public static int GAME_NUM_OF_PLAYERS = 4;
	public static int GAME_NUM_OF_VOTE_OPTIONS = 6;
	public static int GAME_VOTE_COUNT_DURATION = 2;
	public static int GAME_MIN_NUM_OF_VOTE = 3;
	public static int GAME_STARTING_RATION	= 8;
	public static float GAME_FADE_SPEED = 0.01f;

	public static int UI_FLASH_COUNT = 5;
	public static float UI_FLASH_DURATION = 0.5f;

	public static float WHEEL_MIN_TURN_RADIUS = 80;
	public static float WHEEL_MAX_TURN_RADIUS = 290;
	public static float WHEEL_TURN_RADIUS = 180;

	public static float CHARACTER_EATEN_DURATION = 18f;

	public static int CHARACTER_RATION_RECOVERY	= 25;
	public static int CHARACTER_STARTING_HUNGER	= 60;

	public static int CHARACTER_MIN_DEATHRATE 	= 1;
	public static int CHARACTER_MAX_DEATHRATE 	= 2;

	public static int CHARACTER_MIN_HUNGERRATE 	= 22;
	public static int CHARACTER_MAX_HUNGERRATE 	= 28;

	public static int CHARACTER_FULL_MAX	= 100;
	public static int CHARACTER_FULL 		= 60;
	public static int CHARACTER_SATISFIED 	= 30;
	public static int CHARACTER_HUNGRY 		= 1;
	public static int CHARACTER_DYING		= 0;

	public static float CHARACTER_EMOTION_SCALERATE = 0.005f;
	public static float CHARACTER_EMOTION_MAX_SCALE = 0.999f;
	public static float CHARACTER_EMOTION_MIN_SCALE = 0.85f;

	public static int NUTRITION_HEALTHY	= 90;
	public static int NUTRITION_NORMAL = 60;
	public static int NUTRITION_DYING = 30;
	public static int NUTRITION_DEAD = 0;

	public static float HUNGER_MAX_ALPHA_CUTOFF = 1f;
	public static float HUNGER_MIN_ALPHA_CUTOFF = 0.001f;
	public static float HUNGER_DIFF_ALPHA_CUTOFF = 0.999f;

	public static int SHIP_ARRIVAL_RATE = 1;
	public static int SHIP_MIN_DAYS 	= 6;
	public static int SHIP_MAX_DAYS 	= 9;

	public static float VOTE_DISCUSS_DURATION = 10f;
	public static Vector3 VOTE_OPTION_FACE_SCALE = new Vector3(0.6f, 1f, 0.6f);
	public static Vector3 VOTE_OPTION_FACE_DESELECT_SCALE = new Vector3(0.4f, 1f, 0.4f);
	public static Vector3 VOTE_OPTION_SELECT_SCALE = new Vector3(0.9f, 1f, 0.6f);
	public static Vector3 VOTE_OPTION_DESELECT_SCALE = new Vector3(0.6f, 1f, 0.4f);

	public static float VIDEO_OPEN_0_DURATION = 26f;
	public static float VIDEO_FOOD_OUT_1_DURATION = 26f;
	public static float VIDEO_FIRST_BLOOD_2_DURATION = 21f;
}
