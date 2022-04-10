using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ef_PlayEffect : MonoBehaviour {
    //1.持有要播放的特效
    public GameObject[] effectPerfabs;
    public void PlayEffect()
    {
        foreach (GameObject effectPerfab in effectPerfabs)
        {
            Instantiate(effectPerfab);
        }
    }
}
