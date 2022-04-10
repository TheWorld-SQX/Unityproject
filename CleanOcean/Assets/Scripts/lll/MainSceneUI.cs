using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainSceneUI : MonoBehaviour {
	public GameObject settingPanel;
	public Toggle muteToggle;
	void Start()
	{
		muteToggle.isOn = !Audiomanager.Instance.IsMute;
	}
	//游戏中音效数值发生改变，就调用该方法，并传递isOn的值
	public void SwitchMute(bool isOn){
		Audiomanager.Instance.SwithMuteState (isOn);
	}
	//数据的保存
	public void OnBackButtonDown(){
		PlayerPrefs.SetInt ("money",GameControl.Instance.money);
		PlayerPrefs.SetInt ("lv",GameControl.Instance.LV);
		PlayerPrefs.SetFloat ("scd",GameControl.Instance.smallTime);
		PlayerPrefs.SetFloat ("bcd",GameControl.Instance.bigTimer);
		PlayerPrefs.SetInt ("exp",GameControl.Instance.exp);

		int tmp = (Audiomanager.Instance.IsMute == false) ? 0 : 1;
		PlayerPrefs.SetInt ("mute",tmp);

		//先加载下标为0（第一个）场景
		UnityEngine.SceneManagement.SceneManager.LoadScene (0);
	}
	public void OnSettingButtonDown(){
		settingPanel.SetActive (true);
	}
	public void OnCloseButtonDown(){
		settingPanel.SetActive (false);
	}
}
