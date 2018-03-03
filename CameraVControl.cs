using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraVControl : MonoBehaviour {

	//public float Hsensivity = 10f;
	Quaternion VRot;

	// Use this for initialization
	void Start () {
		VRot = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetMouseButton(1)) {
			transform.RotateAround (Vector3.left, Input.GetAxis ("Mouse Y") * Time.deltaTime);
		}
		if (Input.GetMouseButton (2)) {
			transform.rotation = VRot;
		}

	}
}
