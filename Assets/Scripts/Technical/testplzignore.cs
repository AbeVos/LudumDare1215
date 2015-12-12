using UnityEngine;
using System.Collections;

public class testplzignore : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.localPosition = new Vector3(0, 0.8f * Mathf.Sin(Time.realtimeSinceStartup*2.5f),0);
	}
}
