using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class control : MonoBehaviour {

	public float speedStep = 0.2f;
	public float rotationHorSpeed = 0.05f;
	public float rotationVertSpeed = 0.1f;
	public float reverseSpeed = 1f;
	public int maxGear = 5;
	public float stabilize = 0.3f;

	Rigidbody rb;
	float speed;
	float maxSpeed;
	bool engine;
	int gear;

	//Particle system for thrusters
	ParticleSystem.MainModule LtT;
	ParticleSystem.MainModule RtT;

	float startLifeTime;

	//Rotate thrusters
	ParticleSystem LtRotThrust;
	ParticleSystem RtRotThrust;

	//Step thrusters
	ParticleSystem LtThruster;
	ParticleSystem RtThruster;

	//Reverse thrusters
	ParticleSystem LtRevThruster;
	ParticleSystem RtRevThruster;

	//V rot thrusters
	ParticleSystem LtBVTTh;
	ParticleSystem RtBVTTh;
	ParticleSystem LtFVTh;
	ParticleSystem RtFVTh;

	//Sounds
	//sRotateThrusters
	AudioSource sLtRotThrust;
	AudioSource sRtRotThrust;

	//sThrusters
	AudioSource sLtThruster;
	AudioSource sRtThruster;

	//sReverse thrusters
	AudioSource sLtRevThruster;
	AudioSource sRtRevThruster;

	//Turn thrusters
	AudioSource sLtBVTTh;
	AudioSource sRtBVTTh;
	AudioSource sLtFVTh;
	AudioSource sRtFVTh;

	AudioSource ship;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();

		engine = false;
		gear = 0;
		speed = 0f;
		maxSpeed = maxGear * speedStep;

		LtRotThrust = transform.FindChild ("LtRotThrust").GetComponent<ParticleSystem>();
		LtRotThrust.emissionRate = 0;
		RtRotThrust = transform.FindChild ("RtRotThrust").GetComponent<ParticleSystem>();

		LtThruster = transform.FindChild ("LtThrust").GetComponent<ParticleSystem>();
		LtT = LtThruster.main;
		RtThruster = transform.FindChild ("RtThrust").GetComponent<ParticleSystem>();
		RtT = RtThruster.main;

		startLifeTime = LtT.startLifetimeMultiplier;

		LtRevThruster = transform.FindChild ("LtRevThrust").GetComponent<ParticleSystem>();
		RtRevThruster = transform.FindChild ("RtRevThrust").GetComponent<ParticleSystem>();

		LtBVTTh = transform.FindChild ("LtBVTTh").GetComponent<ParticleSystem>();
		RtBVTTh = transform.FindChild ("RtBVTTh").GetComponent<ParticleSystem>();

		LtFVTh = transform.FindChild ("LtFVTh").GetComponent<ParticleSystem>();
		RtFVTh = transform.FindChild ("RtFVTh").GetComponent<ParticleSystem>();


		//Sounds
		//sRotateThrusters
		sLtRotThrust = transform.FindChild("LtRotThrust").GetComponent<AudioSource>();
		sRtRotThrust = transform.FindChild("RtRotThrust").GetComponent<AudioSource>();

		//sThrusters
		sLtThruster = transform.FindChild("LtThrust").GetComponent<AudioSource>();
		sRtThruster = transform.FindChild("RtThrust").GetComponent<AudioSource>();
		sLtThruster.mute = true;
		sRtThruster.mute = true;

		//sReverse thrusters
		sLtRevThruster = transform.FindChild ("LtRevThrust").GetComponent<AudioSource>();
		sRtRevThruster = transform.FindChild ("RtRevThrust").GetComponent<AudioSource>();

		//Turn thrusters
		sLtBVTTh = transform.FindChild ("LtBVTTh").GetComponent<AudioSource>();
		sRtBVTTh = transform.FindChild ("RtBVTTh").GetComponent<AudioSource>();
		sLtFVTh = transform.FindChild ("LtFVTh").GetComponent<AudioSource>();
		sRtFVTh = transform.FindChild ("RtFVTh").GetComponent<AudioSource>();

		ship = transform.FindChild ("ship").GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("f")) {
			//Start engine
			if (!engine) {
				engine = true;
				ThrustOn ();
			} else {
				engine = false;
				gear = 0;
				speed = 0f;
				ThrustOff ();
			}
		}
		if (Input.GetAxis("Mouse ScrollWheel")>0f && engine && gear < maxGear) {
			gear++;
			speed += speedStep;
			ThrustPowerUp ();
			if (speed > maxSpeed) {speed = maxSpeed;}
		}
		if (Input.GetAxis("Mouse ScrollWheel")<0f && engine && gear > 0) {
			gear--;
			speed -= speedStep;
			ThrustPowerDown ();
			if (speed < 0.00f) {speed = 0.00f;}
		}

		//Reverse
		if (Input.GetKey(KeyCode.Space)) {
			rb.AddForce (-transform.up*reverseSpeed, ForceMode.Force);
			Reverse ();
			sLtRevThruster.mute = false;
			sRtRevThruster.mute = false;
		}
		if (Input.GetKeyUp (KeyCode.Space)) {
			sLtRevThruster.mute = true;
			sRtRevThruster.mute = true;
		}
		//Debug.DrawRay (LtRotThrust.transform.position, LtRotThrust.transform.forward, Color.green);
		//Debug.DrawRay (RtRotThrust.transform.position, RtRotThrust.transform.forward, Color.red);

		//Turns
		if (Input.GetKey (KeyCode.W)) {
			turnDown ();
			sRtFVTh.mute = false;
			sLtFVTh.mute = false;
		}
		if (Input.GetKeyUp (KeyCode.W)) {
			sRtFVTh.mute = true;
			sLtFVTh.mute = true;
		}

		if (Input.GetKey (KeyCode.S)) {
			turnUp ();
			sRtBVTTh.mute = false;
			sLtBVTTh.mute = false;
		}
		if (Input.GetKeyUp (KeyCode.S)) {
			turnUp ();
			sRtBVTTh.mute = true;
			sLtBVTTh.mute = true;
		}
			
		if (Input.GetKey (KeyCode.A)) {
			turnLeft ();
			sLtRotThrust.mute = false;
		} 
		if (Input.GetKeyUp(KeyCode.A)){
			sLtRotThrust.mute = true;
		}

		if (Input.GetKey (KeyCode.D)) {
			turnRight ();
			sRtRotThrust.mute = false;
		} 
		if (Input.GetKeyUp (KeyCode.D)) {
			sRtRotThrust.mute = true;
		}

		if (Input.GetKey (KeyCode.A) && Input.GetKey (KeyCode.D)) {
			rb.AddForce (-transform.forward*reverseSpeed, ForceMode.Force);
		}

		//Stabilize
		if (Input.GetKey (KeyCode.Q) && !Input.GetKey (KeyCode.A) && !Input.GetKey (KeyCode.D)) {
			if (rb.angularVelocity.magnitude != 0) {
				rb.angularVelocity = Vector3.Lerp (rb.angularVelocity, Vector3.zero, Time.deltaTime*stabilize);
			}
		}
		if (Input.GetKey (KeyCode.E) && gear==0) {
			if (rb.velocity.magnitude != 0) {
				rb.velocity = Vector3.Lerp (rb.velocity, Vector3.zero, Time.deltaTime*stabilize);
			}
		}

		if (engine) {
			flight ();
		}


		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit ();
		}
	}

	void ThrustOn() {
		LtThruster.Play ();
		RtThruster.Play ();
		sLtThruster.mute = false;
		sRtThruster.mute = false;
		ship.mute = false;
	}
	void ThrustOff() {
		LtThruster.Stop ();
		RtThruster.Stop ();
		LtT.startLifetimeMultiplier = startLifeTime;
		RtT.startLifetimeMultiplier = startLifeTime;
		sLtThruster.mute = true;
		sRtThruster.mute = true;
		ship.mute = true;
	}
	void ThrustPowerUp() {
		LtT.startLifetimeMultiplier += 0.1f;
		RtT.startLifetimeMultiplier += 0.1f;
		sLtThruster.pitch += 0.1f;
		sRtThruster.pitch += 0.1f;
	}
	void ThrustPowerDown() {
		LtT.startLifetimeMultiplier -= 0.1f;
		RtT.startLifetimeMultiplier -= 0.1f;
		sLtThruster.pitch -= 0.1f;
		sRtThruster.pitch -= 0.1f;
	}

	//Turns
	void turnLeft() {
		rb.AddRelativeTorque(Vector3.up*rotationVertSpeed, ForceMode.Impulse);
		LRTh ();
	}
	void turnRight() {
		rb.AddRelativeTorque(-Vector3.up*rotationVertSpeed, ForceMode.Impulse);
		RRTh ();
	}
	void turnUp() {
		rb.AddRelativeTorque(-Vector3.left*rotationHorSpeed, ForceMode.Impulse);
		VTurnThU ();
	}
	void turnDown() {
		rb.AddRelativeTorque(Vector3.left*rotationHorSpeed, ForceMode.Impulse);
		VTurnThD ();
	}

	//Thrusters
	void LRTh() {
		LtRotThrust.Emit (1);
	}
	void RRTh() {
		RtRotThrust.Emit (1);
	}
	void VTurnThU() {
		LtBVTTh.Emit (1);
		RtBVTTh.Emit (1);
	}
	void VTurnThD() {
		LtFVTh.Emit (1);
		RtFVTh.Emit (1);
	}
	void Reverse() {
		LtRevThruster.Emit (1);
		RtRevThruster.Emit (1);
	}

	//Moving Forward
	void flight() {
		rb.AddForce (transform.up*speed, ForceMode.Force);
	}


	void OnGUI() {
		GUI.TextField (new Rect(10, 10, 200, 20), "Engine: " + (engine?"On":"Off"));
		GUI.TextField (new Rect(10, 40, 200, 20), "Gear: " + gear.ToString());
		GUI.TextField (new Rect(10, 70, 200, 20), "Velocity: " + rb.velocity);
		GUI.TextField (new Rect(10, 100, 200, 20), "Speed: " + speed);
		GUI.TextField (new Rect(10, 130, 200, 20), "AngVelocity: " + rb.angularVelocity);

		GUI.TextField (new Rect (10, 350, 200, 30), "F - start engine");
		GUI.TextField (new Rect (10, 380, 200, 30), "q - stabilize angular roration");
		GUI.TextField (new Rect (10, 410, 200, 30), "e - deceleration");
	}
}
