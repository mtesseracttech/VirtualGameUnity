﻿using UnityEngine;
using System.Collections;

public class bulletImpactScript : MonoBehaviour {
    private float time;
	// Use this for initialization
	void Start () {
        time = 0;
	}
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;
        if (time >= 3f)
        {
            Destroy(gameObject);
        }
	}
}
