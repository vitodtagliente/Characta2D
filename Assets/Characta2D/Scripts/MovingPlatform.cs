using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	bool goup = true;
	int i = 0;

	// Update is called once per frame
	void Update () {
		transform.position += 
			new Vector3(0f, ((goup)?1f:-1)*2f * Time.deltaTime, 0f);

		i++;
		if (i == 50) {
			i = 0;
			goup = !goup;
		}
	}
}
