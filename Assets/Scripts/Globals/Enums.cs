public enum GameState {
	GAME_INTRO,
	GAME_START
}

public enum CharacterIndex {
	CHAR1 = 0,
	CHAR2 = 1,
	CHAR3 = 2,
	CHAR4 = 3,
	SIZE
}

public enum InstructionState {
	TAP_TO_START_0,
	SHOULD_EAT_TODAY_1,
	VOTE_NOT_TO_EAT_2,
	VOTE_TO_EAT_3,
	RESCUE_SHIP_LONG_4,
	OUT_OF_FOOD_5,
	READY_6,
	FIRST_COME_FIRST_SERVE_7,
	DISCUSS_8,
	FOOD_SNATCHED_9,
	STARVING_DIE_10,
	INSTRUCTION_SIZE
}

public enum VideoState {
	VIDEO_OPENING_0,
	VIDEO_FOOD_RUN_OUT_1,
	VIDEO_FIRST_BLOOD_2,
	VIDEO_SIZE
}

public enum CharacterState {
	FULL,
	SATISFIED,
	HUNGRY,
	DYING,
	DEAD
}

public enum DrumState {
	READY_TO_START,
	READY_TO_VOTE,
	VOTE_TO_EAT,
	AMOUNT_TO_EAT,
	VOTE_TO_EAT_CHARACTER
}

public enum VoteOptions {
	ZERO = 0,
	ONE = 1,
	TWO = 2,
	THREE = 3,
	CHAR_1,
	CHAR_2,
	CHAR_3,
	CHAR_4,
	YES,
	NO,
	NONE
}

public enum HungerBarState {
	HUNGER_BAR_0 = 0,
	HUNGER_BAR_30 = 1,
	HUNGER_BAR_60 = 2,
	HUNGER_BAR_100 = 3,
	HUNGER_BAR_SIZE = 4
}

