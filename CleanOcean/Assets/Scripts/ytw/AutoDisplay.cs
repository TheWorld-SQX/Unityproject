using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDisplay : MonoBehaviour {
    public GameObject gameHelpTip;
    public void OnSettingButtonDown()
    {
        gameHelpTip.SetActive(true);
    }
    public void OnCloseButtonDown()
    {
        gameHelpTip.SetActive(false);
    }
}
