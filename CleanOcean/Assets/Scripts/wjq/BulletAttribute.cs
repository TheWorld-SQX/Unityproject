using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAttribute : MonoBehaviour {
    public int speed;
    public int damage;
	public GameObject websPrefab;
	private void OnTriggerEnter2D(Collider2D collision){
		if (collision.tag == "bound") {
			Destroy(gameObject);
		}
		if (collision.tag == "Fish") {
			GameObject web = Instantiate(websPrefab);
			web.transform.SetParent (gameObject.transform.parent,false);
			web.transform.position = gameObject.transform.position;
			web.GetComponent<WebAttribute> ().damage = damage;
			Destroy(gameObject);
		}
	}
}
