using UnityEngine;
using System.Collections;
using InControl;

public class MovementControl : MonoBehaviour {


	//Controller input
	public int playerNum;
	public bool controllerConnected;
	private Vector2 lsv;
	private Vector2 rsv;
	
	//public GameObject ControlInput;
	private InControlSetup controllerData;

	
	
	
	//Movement physics
	public GameObject body;
	private Rigidbody2D _rb;
	public float moveSpeed = 18;
	public float friction = .92f;
	public float turnSpeed = 40;
	public Vector2 startPos;
	
	private Vector2 lookDir = new Vector2(0f,0f);
	private Vector2 runForce = new Vector2(0f,0f);
	
	//Hand
	private GameObject hand;
	public float handradius = .7f;
	public float angleRange = 75;
	public float reach = .5f;
	public float handMaxSpeed = 200;
	
	
	private Rigidbody2D hand_rb;
	private float handDamp = 0.01f;
	private float handYVel = 0f;
	private float handXVel = 0f;
	private float handangle = 0f;
	
	
	
	//Boost
	public int boostCooldown = 20;
	public float boostAmt = 20;
	
	private int boostTimer = 0;
	private bool canBoost = true;
	
	//Player script reference
	public PlayerScript playerScript;
	

	// Use this for initialization
	void Awake () {
	
		
	
		controllerConnected = true;
		playerScript = GetComponent<PlayerScript> ();
		_rb = rigidbody2D;
		hand = playerScript.hand;
		hand_rb = hand.rigidbody2D;

		
		controllerData = GameObject.Find("InputManager").GetComponent<InControlSetup> ();
		startPos = new Vector2(transform.localPosition.x, transform.localPosition.y);
		
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if(!controllerData.deviceConnected(playerNum)){
				noController();
		}
		else{
		
			//Runs first time when controller is connected
			if(!controllerConnected){
				transform.localPosition = startPos;
				controllerConnected = true;
				playerScript._bodyrenderer.enabled = true;
				playerScript._handrenderer.enabled = true;
				collider2D.enabled = true;
			}
		
			
			lsv = controllerData.getLsv (playerNum);
			rsv = controllerData.getRsv (playerNum);
			checkBoost();
	
	
			//get current velocity
			var vel = _rb.velocity;
			vel.x *= friction; // modify x and y components
			vel.y *= friction;
			_rb.velocity = vel;
	
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
	
			Vector2 curHandPos = hand.transform.localPosition;
			float targetHandX = radius * Mathf.Cos ((handangle-90) * Mathf.Deg2Rad);
			float targetHandY = -radius * Mathf.Sin ((handangle-90) * Mathf.Deg2Rad);
	
			float newHandX = Mathf.SmoothDamp(curHandPos.x, targetHandX, ref handXVel, handDamp, handMaxSpeed);
			float newHandY = Mathf.SmoothDamp(curHandPos.y, targetHandY, ref handYVel, handDamp, handMaxSpeed);
			Vector2 newHandPos = new Vector2 (newHandX, newHandY);
	
			hand.transform.localPosition  = newHandPos;
			
			
			//calculate run force and apply
			runForce.x = lsv.x * moveSpeed;
			runForce.y = lsv.y * moveSpeed;
			
			_rb.AddForce(runForce);
		
		}

	}
	
	
	void noController(){
		if(controllerConnected){
			Debug.Log (playerNum + " disconnected");
			controllerConnected = false;
			if(playerScript.currentWeapon != null) playerScript.currentWeapon.gameObject.SendMessage("onDrop");

			playerScript._handrenderer.enabled = false;
			playerScript._bodyrenderer.enabled = false;
			collider2D.enabled = false;
		}
		
		

		
		
	
	}
	void checkBoost(){
		if (canBoost && controllerData.getLeftTrigger(playerNum)) {
			Debug.Log ("Boost");
			
			Vector2 boostForce;
			if(lsv.magnitude < .2){
				boostForce = new Vector2(lookDir.normalized.x * boostAmt, lookDir.normalized.y * boostAmt);
			}else{
				boostForce = new Vector2(lsv.x * boostAmt, lsv.y * boostAmt);
			}
			_rb.velocity = boostForce;
			
			canBoost = false;
			boostTimer = 0;
			
		} else if(!canBoost){
			boostTimer ++;
			if(boostTimer >= boostCooldown){
				canBoost = true;
			}
		}
	
	
	
	}
	
	void OnTriggerStay2D(Collider2D other){
		if(other.gameObject.tag == "weapon" && other.gameObject.layer == 0){
			
			if(controllerData.getAction4(playerNum)){
			
				if(playerScript.currentWeapon != null) playerScript.currentWeapon.SendMessage("onDrop");
				
				other.gameObject.SendMessage("getPicked", this.gameObject);
				
				playerScript.currentWeapon = other.gameObject;
			}
		}		
	}
	

//	void OnTriggerExit2D(Collider2D other){
//		
//		
//		if(other.gameObject.tag == "weapon" && other.gameObject.layer == 0){
//			Debug.Log ("cannot pick up");
//			
//			if(controllerData.getAction4(playerNum)){
//				Debug.Log ("picked");
//				other.gameObject.SendMessage("getPicked", this.gameObject);
//			}
//			
//		}
//		//q = rigidbody2D.transform.rotation;
//		
//	}


}
