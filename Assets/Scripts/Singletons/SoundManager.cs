using UnityEngine;
using System.Collections;

public class SoundManager : Singleton<SoundManager>
{

    protected SoundManager() { }

    public Transform soundManager;

    public AudioSource bgOceanAS;
    public AudioSource bgmThemeAS;
    public AudioSource bgmBeatAS;
    public AudioSource sfxHeartbeatAS;
    public AudioSource[] dialogAS;

    public AudioClip bgOceanC;
    public AudioClip bgmThemeDay34C;
    public AudioClip bgmThemeDay56C;
    public AudioClip bgmThemeDay78C;
    public AudioClip bgmBeatDay56C;
    public AudioClip bgmBeatDay78C;
    public AudioClip[] sfxEatC;
    public AudioClip sfxEatPplC;
    public AudioClip sfxHeartbeatC;
    public AudioClip dialogEndingC;
    public AudioClip dialogEndingHappyC;

    public AudioClip[] dialogC;
    //test
    AudioSource testAS;
    GameObject testGO;

    void Start()
    {
		// Debug.Log("Sound Manager start working");
        bgOceanAS.clip = bgOceanC;
        sfxHeartbeatAS.clip = sfxHeartbeatC;

		if(!SoundManager.IsCreated){
			DontDestroyOnLoad(this.gameObject);
			SoundManager.IsCreated = true;
		}else{
			Destroy(this.gameObject);
		}
    }

    void Update()
    {
        if (Input.GetKeyDown("down"))  {
            bgOceanAS.volume -= 0.1f;
            bgmThemeAS.volume -= 0.1f;
            bgmBeatAS.volume -= 0.1f;
        }
        if (Input.GetKeyDown("up"))  {
            bgOceanAS.volume += 0.1f;
            bgmThemeAS.volume += 0.1f;
            bgmBeatAS.volume += 0.1f;
        }

        //test

//        if (Input.GetKeyDown("o")) BGOceanPlay();

        //if (Input.GetKeyDown("1")) BGMThemePlay(1);
        //if (Input.GetKeyDown("2")) BGMThemePlay(2);
        //if (Input.GetKeyDown("3")) BGMThemePlay(3);
        //if (Input.GetKeyDown("4")) BGMThemePlay(4);
        //if (Input.GetKeyDown("5")) BGMThemePlay(5);
        //if (Input.GetKeyDown("6")) BGMThemePlay(6);
        //if (Input.GetKeyDown("7")) BGMThemePlay(7);
        //if (Input.GetKeyDown("8")) BGMThemePlay(8);
        //if (Input.GetKeyDown("9")) BGMThemePlay(9);
        //if (Input.GetKeyDown("0")) BGMThemePlay(0);

        //if (Input.GetKeyDown("s")) BGMThemesStop();

//        if (Input.GetKeyDown("e")) SFXEat();
//        if (Input.GetKeyDown("h")) SFXHeartbeat();
//        if (Input.GetKeyDown("p")) SFXEatPPL();
//
//        if (Input.GetKeyDown("t")) DialogEnding(true);
//        if (Input.GetKeyDown("f")) DialogEnding(false);


    }

    //bgm
    public void BGOceanPlay()
    {
        bgOceanAS.volume = 0.5f;
        bgOceanAS.Play();
    }

    public void BGOceanStop()
    {
        StartCoroutine(MusicFadeOut(bgOceanAS, 0.5f,0.01f,true));
    }

    public void BGMThemePlay(int day)
    {
        Debug.Log("SoundManager.BGMThemePlay(day) day is playing.");
        StartCoroutine(MusicFadeOut(bgOceanAS, 1f, 0.2f, false));
        if (day <= 2)
        {       
            return;
        }
        else if (day == 3 || day == 4)
        {
            bgmThemeAS.clip = bgmThemeDay34C;
        }
        else if (day == 5 || day == 6)
        {
            bgmThemeAS.clip = bgmThemeDay56C;
        }
        else if (day >= 7)
        {
            bgmThemeAS.clip = bgmThemeDay78C;
        }
        else
        {
            Debug.Log("SoundManager.BGMThemePlay(day) day illegal.");
        }
        MusicFadeInReplay(bgmThemeAS, 10f, 0.7f);
    }

    public void BGMThemesStop()
    {
        StartCoroutine(MusicFadeIn(bgOceanAS,1f,0.5f,false));
        StartCoroutine(MusicFadeOut(bgmThemeAS, 1f, 0.01f,true));
    }

    public void BGMBeatPlay(int day)
    {
        if (day == 6)
        {
            bgmBeatAS.clip = bgmBeatDay56C;
        }
        else if (day >= 8)
        {
            bgmBeatAS.clip = bgmBeatDay78C;
        }
		else {
			return;
		}
        MusicFadeInReplay(bgmBeatAS, 10f, 0.7f);

    }

    public void BGMBeatStop()
    {
        StartCoroutine(MusicFadeOut(bgmBeatAS, 1f,0.01f,true));
    }

    //sfx
    public void SFXEat()
    {
        int clip;
        float pitch;
        clip = Random.Range(0, 3);
        pitch = Random.Range(0.7f, 1.3f);
        AudioPlay(sfxEatC[clip], soundManager, 0.4f, pitch);
    }

    public void SFXHeartbeat()
    {
        BGMBeatStop();
        BGMThemesStop();
        MusicFadeInReplay(sfxHeartbeatAS, 2f, 0.5f);
    }

    public void SFXEatPPL()
    {
		StartCoroutine(SFXEatPPLCoRoutine());
    }

	IEnumerator SFXEatPPLCoRoutine()
	{
		SFXHeartbeat();
		yield return new WaitForSeconds(5f);
		StartCoroutine(SFXHeartBeatRaisePitch());
		AudioPlay(sfxEatPplC, soundManager, 0.8f);
	}

    IEnumerator SFXHeartBeatRaisePitch()
    {
        StartCoroutine(RaisePitch(sfxHeartbeatAS, 0.5f, 3f));
        yield return new WaitForSeconds(4.5f);
        sfxHeartbeatAS.Stop();
    }

    IEnumerator RaisePitch(AudioSource audios, float raiseSpeed, float maxPitch)
    {
        while (audios.pitch < maxPitch)
        {
            audios.pitch += raiseSpeed * Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }


    //dialog
    public void DialogEnding(bool happyEnding)
    {
        StartCoroutine(DialogEndingCoRoutine(happyEnding));
    }

    IEnumerator DialogEndingCoRoutine(bool happyEnding)
    {
        if (happyEnding)
        {
            AudioPlay(dialogEndingHappyC, soundManager, 1f);
            yield return new WaitForSeconds(28f);
            BGOceanStop();
        }
        else
        {
            AudioPlay(dialogEndingC, soundManager, 1f);
            yield return new WaitForSeconds(29.4f);
            BGOceanStop();
        }
    }

    IEnumerator DialogPlay(int RoleID, int dialogID, float volume)
    {
        while (dialogAS[RoleID].isPlaying)
        {
            yield return new WaitForSeconds(Time.deltaTime);
        }
        dialogAS[RoleID].clip = dialogC[dialogID];
        dialogAS[RoleID].volume = volume;
        dialogAS[RoleID].Play();
        // Debug.Log (dialogAS[RoleID].clip);
    }

    IEnumerator MusicFadeIn(AudioSource audios, float fadeInSpeed, float maxVolumn, bool restart)
    {
        //Debug.Log("Start fade in.");
        if (restart)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            audios.Play();
        }
        while (audios.volume < maxVolumn)
        {
            audios.volume += fadeInSpeed * Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        //		Debug.Log("Complete fade in.");
    }

    void MusicFadeInReplay(AudioSource audios, float fadeInSpeed, float maxVolumn)
    {
        audios.Stop();
        audios.volume = 0;
        audios.pitch = 1f;;
        StartCoroutine(MusicFadeIn(audios, fadeInSpeed, maxVolumn, true));
    }

    IEnumerator MusicFadeOut(AudioSource audios, float fadeOutSpeed, float minVolumn,bool stop)
    {
        //		Debug.Log("Start fade out.");
        while (audios.volume >= minVolumn)
        {
            audios.volume -= fadeOutSpeed * Time.deltaTime;
            if (audios.isPlaying)
            {
                //Debug.Log("audios.isPlaying = " + audios.isPlaying);
                yield return new WaitForSeconds(Time.deltaTime);
            }
                
        }
        if (stop)
        {
            audios.Stop();
        }
    }

    /// Plays a sound by creating an empty game object with an AudioSource
    /// and attaching it to the given transform (so it moves with the transform). Destroys it after it finished playing.
    void AudioPlay(AudioClip clip, Transform emitter, float volume, float pitch)
    {
        //Create an empty game object
        GameObject go = new GameObject("Audio: " + clip.name);
        go.transform.position = emitter.position;
        go.transform.parent = emitter;

        //Create the source
        AudioSource source = go.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;
        source.Play();
        Destroy(go, clip.length);
    }

    void AudioPlay(AudioClip clip)
    {
        AudioPlay(clip, soundManager, 1f, 1f);
    }

    void AudioPlay(AudioClip clip, Transform emitter)
    {
        AudioPlay(clip, emitter, 1f, 1f);
    }

    void AudioPlay(AudioClip clip, Transform emitter, float volume)
    {
        AudioPlay(clip, emitter, volume, 1f);
    }

    /// Plays a sound at the given point in space by creating an empty game object with an AudioSource
    /// in that place and destroys it after it finished playing.
    void AudioPlay(AudioClip clip, Vector3 point, float volume, float pitch)
    {
        //Create an empty game object
        GameObject go = new GameObject("Audio: " + clip.name);
        go.transform.position = point;

        //Create the source
        AudioSource source = go.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;
        source.Play();
        Destroy(go, clip.length);
    }

    void AudioPlay(AudioClip clip, Vector3 point)
    {
        AudioPlay(clip, point, 1f, 1f);
    }

    void AudioPlay(AudioClip clip, Vector3 point, float volume)
    {
        AudioPlay(clip, point, volume, 1f);
    }
}
