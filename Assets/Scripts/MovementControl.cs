using UnityEngine;
using System.Collections;
using InControl;

public class MovementControl : MonoBehaviour {

	public GameObject ControlInput;
	private InControlSetup controllerData;
	public GameObject body;
	private Rigidbody2D _rb;

	public float moveSpeed = 50;
	public float friction = .95f;
	private Vector2 runForce = new Vector2(0f,0f);
	public float turnSpeed = 40;
	private Vector2 lookDir = new Vector2(0f,0f);

	public int playerNum;

	// Use this for initialization
	void Start () {
		_rb = rigidbody2D;
		controllerData = ControlInput.GetComponent<InControlSetup> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		Vector2 lsv = controllerData.getLsv (playerNum);
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
		var angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg -90;
		var q = Quaternion.AngleAxis(angle, Vector3.forward);

		body.transform.rotation = Quaternion.RotateTowards(body.transform.rotation, q, turnSpeed); 


//
//		float velocity.x - targetV_X)
//		transform.Translate(moveX, moveY, 0);

	
	}
}
