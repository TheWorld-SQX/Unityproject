using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ef_WaterWave : MonoBehaviour {
	public Texture[] texture;
	private Material material;
	private int index = 0;
	// Use this for initialization
	void Start () {
		material = GetComponent<MeshRenderer>().material;
		InvokeRepeating ("ChangeTexture", 0, 0.1f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void ChangeTexture()
	{
		material.mainTexture = texture[index];
		index = (index + 1) % texture.Length;
	}

}