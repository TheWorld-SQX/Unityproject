﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldCollect : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag =="gold")
        {
			Audiomanager.Instance.PlayEffectSound (Audiomanager.Instance.goldClip);
            Destroy(collision.gameObject);
        }
    }
	
}
