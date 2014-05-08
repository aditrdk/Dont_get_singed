using UnityEngine;
using System.Collections;

public class WeaponScript : MonoBehaviour {

	
	public GameObject holder;
	public GameObject hand;
	public int anglePercision = 10;
	public float turnTorque = 80f;
	
	public float attackDamage = 1;
	public int attackDelay = 25;
	
	
	private Vector2 pointDir = new Vector2(0f,0f);
	private Transform hand_transform;
	private Rigidbody2D hand_rb;
	private HingeJoint2D _hingejoint;
	private Quaternion q;
	private Transform defaultParent;
	
	private int attackDelayTimer = 0;
	private bool canAttack = true;
	
	
	
	



	// Use this for initialization
	void Start () {
		Physics2D.IgnoreLayerCollision(8, 8, true);
		Physics2D.IgnoreLayerCollision(9, 9, true);
		Physics2D.IgnoreLayerCollision(10, 10, true);
		Physics2D.IgnoreLayerCollision(11, 11, true);
		/*
		for(int i = 0; i < 12; i++){
			Physics2D.IgnoreLayerCollision(0, i, true);
		}
		*/
		_hingejoint = this.GetComponent<HingeJoint2D> ();
		defaultParent = transform.parent;
	}
	
	// Update is called once per frame
	void Update () {
		//Being Held
		if (holder != null) {
			if (_hingejoint.connectedBody == null) {
				
				//gameObject.layer = holder.layer;
				
				hand = holder.transform.FindChild ("hand").gameObject;
				holder.GetComponent<PlayerScript>().currentWeapon = this.gameObject;
				
				//Cache references to the hand and its rigidbody
				hand_rb = hand.GetComponent<Rigidbody2D>();
				hand_transform = hand.transform;

				//Set parent of weapon to the hand holding it so that it's position is now relative to the hand
				transform.parent = hand_transform;

				_hingejoint.connectedBody = hand_rb;
				_hingejoint.enabled = true;
				
				collider2D.isTrigger = false;
				
			} 
			else {


				//Direction you are trying to point the weapon in is based on position of the hand
				pointDir = hand_transform.localPosition;

				//Calculate the angle trying to point in
				var targetAngle = Mathf.Atan2 (pointDir.y, pointDir.x) * Mathf.Rad2Deg - 90;



				//Convert angle to positive number 
				if (targetAngle < 0)
						targetAngle += 360;

				//Debug.Log (targetAngle);

				//Unused, temporaily commented out in case needed
				//q = Quaternion.AngleAxis(angle, Vector3.forward);

				//Calculate the angle difference between the weapon now and its target angle
				float angleDiff = targetAngle - transform.localEulerAngles.z;

				//Convert the angle difference to the smaller angle between
				if (angleDiff > 180)
						angleDiff -= 360;
				if (angleDiff < -180)
						angleDiff += 360;

				//If the weapon is off from tis target by 'anglePercision' degrees, then apply torque
				if (angleDiff > anglePercision) {
						rigidbody2D.AddTorque (turnTorque * angleDiff);

				} else if (angleDiff < -anglePercision) {
						rigidbody2D.AddTorque (turnTorque * angleDiff);
				}

			}
		}else{

			collider2D.isTrigger = true;


		}
		
		if(!canAttack){
			attackDelayTimer++;
			if(attackDelayTimer >= attackDelay){
				canAttack = true;
				attackDelayTimer = 0;
			}
		}
		
		
	}
	
	
	void onDrop(){
		
		holder = null;
		hand = null;
		
		rigidbody2D.velocity = new Vector2(0f, 0f);
		
		_hingejoint.connectedBody = null;	
		_hingejoint.enabled = false;

		transform.parent = null;
				
		collider2D.isTrigger = true;
		gameObject.layer = 0;
		
		//Debug.Log ("DROP");
		
		
	
	}
	
	void getPicked(GameObject newHolder){
		
		
		//Debug.Log ("picked by" + newHolder);
		holder = newHolder;
		
		
		hand = holder.transform.FindChild ("hand").gameObject;
		holder.GetComponent<PlayerScript>().currentWeapon = this.gameObject;
		
		hand_rb = hand.GetComponent<Rigidbody2D>();
		hand_transform = hand.transform;
		
		//Set parent of weapon to the hand holding it so that it's position is now relative to the hand
		transform.parent = hand_transform;
		
		_hingejoint.connectedBody = hand_rb;
		_hingejoint.enabled = true;
		
		collider2D.isTrigger = false;
		gameObject.layer = holder.layer;
		
	
	}
	
	void OnCollisionEnter2D(Collision2D other){
	
		if(other.gameObject.tag == "Player"){
			//Debug.Log (other.gameObject);
			if(canAttack){
				other.gameObject.SendMessage("Attacker", holder);
				
				other.gameObject.SendMessage ("Damage", attackDamage);
				canAttack = false;
			}
		}
		
	}
	


}
