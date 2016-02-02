using UnityEngine;
using System.Collections;

public class EventManager : Singleton<EventManager> {

	public delegate void NotifyVoteChange(DrumState drumState);
	public static event NotifyVoteChange OnVoteChange;

}
