using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

	public float HSens = 60f;
	public float VSens = 40f;
	public float camResetTime = 1f;
	Quaternion HRot;
	Quaternion VRot;
	Transform CameraObj;

	// Use this for initialization
	void Start () {
		CameraObj = transform.FindChild ("VertCamControl");
		HRot = transform.localRotation;
		VRot = CameraObj.transform.localRotation;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton(1)) {
			transform.Rotate (Vector3.forward, Input.GetAxis ("Mouse X") * Time.deltaTime * HSens);
			CameraObj.transform.Rotate(Vector3.left, -Input.GetAxis ("Mouse Y") * Time.deltaTime * VSens);
		}
		if (Input.GetMouseButton (2)) {
		//else {	
			transform.localRotation = HRot;
			CameraObj.transform.localRotation = VRot;
		}
	}
}
