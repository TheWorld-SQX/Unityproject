using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ef_WaveMove : MonoBehaviour {
	private Vector3 temp;
	// Use this for initialization
	void Start () {
		temp.x = transform.position.x;
		temp.y = -transform.position.y;
		temp.z = transform.position.z;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.MoveTowards(transform.position, -temp, 10 * Time.deltaTime);
	}
}
