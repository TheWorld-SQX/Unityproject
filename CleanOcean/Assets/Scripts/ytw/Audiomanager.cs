using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audiomanager : MonoBehaviour {
	private static Audiomanager _instance;
	public static Audiomanager Instance
	{
		get{
			return _instance;
		}
	}  
	private bool isMute = false;
	public AudioSource bgmAudioSource;
	public AudioClip seaWaveClip;
	public AudioClip goldClip;
	public AudioClip rewardClip;
	public AudioClip fireClip;
	public AudioClip changeGunCilp;
	public AudioClip LvUpClip;
	void Awake()
	{
		_instance = this;
		isMute = (PlayerPrefs.GetInt("mute",0)==0)?false:true;
		Domute ();
	}
	public bool IsMute
	{
		get
		{
			return isMute;
		}
	}
	public void SwithMuteState(bool isOn)
	{
		isMute = !isOn;
		Domute ();
	}
	void Domute()
	{
		if (isMute) 
		{
			bgmAudioSource.Pause();
		} 
		else 
		{
			bgmAudioSource.Play();
		}
	}
	public void PlayEffectSound(AudioClip auClip)
	{
		if (!isMute) {
			AudioSource.PlayClipAtPoint (auClip, Vector3.zero);
		}

	}
}
