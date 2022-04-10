using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishAttr : MonoBehaviour {
	//this script used to record information of fish
	public int exp;
	public int money;
	public int maxNum;
	public int maxSpeed;
	public int HP;
	public GameObject diePrefab;
    public GameObject goldPrefab;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "bound")
        {
            Destroy(gameObject);
        }
    }
	//网已经将值传过来 下面是对伤害的处理
	void BeDamaged(int damage){
		HP -= damage;
		if (HP<=0) {
            GameControl.Instance.money += money;
            GameControl.Instance.exp += exp;

            GameObject die = Instantiate (diePrefab);
			die.transform.SetParent (gameObject.transform.parent, false);
			die.transform.position = transform.position;
			die.transform.rotation = transform.rotation;
            GameObject gold = Instantiate(goldPrefab);
            gold.transform.SetParent(gameObject.transform.parent, false);
            gold.transform.position = transform.position;
            gold.transform.rotation = transform.rotation;
			if (gameObject.GetComponent<Ef_PlayEffect>()!=null) {
				gameObject.GetComponent<Ef_PlayEffect>().PlayEffect();
			}
			Destroy (gameObject);
		}
	}

}
