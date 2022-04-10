using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTo : MonoBehaviour {
    private GameObject goldCollector;

	// Use this for initialization
	void Start () {
		goldCollector = GameObject.Find("GoldCollector");
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = Vector3.MoveTowards(transform.position, goldCollector.transform.position, 10 * Time.deltaTime);
	}
}
