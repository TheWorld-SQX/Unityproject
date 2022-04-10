using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebAttribute : MonoBehaviour {
	public float disappearTime;
	public int damage;
	void Start(){
		Destroy (gameObject,disappearTime);
	}
	//通知鱼受伤
	private void OnTriggerEnter2D(Collider2D collision){
		if (collision.tag =="Fish") {
			collision.SendMessage ("BeDamaged", damage);
		}
	}
}
