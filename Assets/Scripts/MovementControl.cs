using UnityEngine;
using System.Collections;
using InControl;

public class MovementControl : MonoBehaviour {

	public GameObject ControlInput;
	private InControlSetup controllerData;
	public GameObject body;
	public GameObject hand;
	private Rigidbody2D _rb;
	private Rigidbody2D hand_rb;

	public float moveSpeed = 18;
	public float friction = .92f;
	private Vector2 runForce = new Vector2(0f,0f);
	public float turnSpeed = 40;
	private Vector2 lookDir = new Vector2(0f,0f);

	public float handradius = .7f;
	public float angleRange = 85;
	public float reach = .5f;

	public int playerNum;


	public float dampTime = 0.1f; //offset from the viewport center to fix damping
	public float maxspeed = 200;
	
	private float handYVel = 0f;
	private float handXVel = 0f;

	private float handangle = 0f;

	public int boostCooldown = 30;
	private int boostTimer = 0;
	private bool canBoost = true;


	// Use this for initialization
	void Start () {
		_rb = rigidbody2D;
		hand_rb = hand.rigidbody2D;
		controllerData = ControlInput.GetComponent<InControlSetup> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		Vector2 lsv = controllerData.getLsv (playerNum);
		Vector2 rsv = controllerData.getRsv (playerNum);

		//calculate run force and apply
		runForce.x = lsv.x * moveSpeed;
		runForce.y = lsv.y * moveSpeed;

		_rb.AddForce(runForce);

		//get current velocity
		var vel = rigidbody2D.velocity;
		vel.x *= friction; // modify x and y components
		vel.y *= friction;
		rigidbody2D.velocity = vel;

		if (Mathf.Abs(vel.x) > 0 || Mathf.Abs (vel.y) > 0)
						lookDir = vel;

		float targetAngleFacing = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90;
		var q = Quaternion.AngleAxis(targetAngleFacing, Vector3.forward);

		body.transform.rotation = Quaternion.RotateTowards(body.transform.rotation, q, turnSpeed); 
//		float actualAngleFacing = body.transform.eulerAngles.z * -1;
//		if (actualAngleFacing < -180)
//						actualAngleFacing += 360;

		float angleFacing = -1 * (targetAngleFacing);
		if (angleFacing > 180)
						angleFacing -= 360;

		float rsvangle = Mathf.Atan2(rsv.x, rsv.y) * Mathf.Rad2Deg;

		if (rsv.magnitude > 0) {
						handangle = rsvangle;
				}
						
		float angleDelta = Mathf.DeltaAngle (angleFacing, handangle);

		if (angleDelta > angleRange && angleDelta > 0) {
			handangle = angleFacing + angleRange;

						
		} else if (angleDelta < -angleRange) {
			handangle = angleFacing - angleRange;
						
		}

		if (handangle > 180)
						handangle -= 360;

		if (handangle < -180)
						handangle += 360;
						

		float radius = rsv.magnitude * reach + handradius;

		Vector2 curHandPos = hand_rb.transform.localPosition;
		float targetHandX = radius * Mathf.Cos ((handangle-90) * Mathf.Deg2Rad);
		float targetHandY = -radius * Mathf.Sin ((handangle-90) * Mathf.Deg2Rad);

		float newHandX = Mathf.SmoothDamp(curHandPos.x, targetHandX, ref handXVel, dampTime, maxspeed);
		float newHandY = Mathf.SmoothDamp(curHandPos.y, targetHandY, ref handYVel, dampTime, maxspeed);
		Vector2 newHandPos = new Vector2 (newHandX, newHandY);

		hand.transform.localPosition  = newHandPos;

//
//		float velocity.x - targetV_X)
//		transform.Translate(moveX, moveY, 0);
		//BOOST

		if (canBoost && controllerData.getLeftTrigger(playerNum)) {

			//Boost code goes here!
			canBoost = false;
			boostTimer = 0;

		} else if(!canBoost){
			boostTimer ++;
			if(boostTimer >= boostCooldown){
				canBoost = true;
			}
		}
	
	}
}
