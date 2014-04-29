using UnityEngine;
using System.Collections;

public class WeaponScript : MonoBehaviour {

	public GameObject holder;
	public GameObject hand;
	public int anglePercision = 10;
	public float turnTorque = 80f;
	private Vector2 pointDir = new Vector2(0f,0f);
	private Transform hand_transform;
	private Rigidbody2D hand_rb;
	private HingeJoint2D _hingejoint;
	private Quaternion q;
	private CircleCollider2D c_collider2d;



	// Use this for initialization
	void Start () {
		_hingejoint = this.GetComponent<HingeJoint2D> ();
	
	}
	
	// Update is called once per frame
	void Update () {
		if (holder != null) {
			if (_hingejoint.connectedBody == null) {

				//THIS IS THE PART THAT WE NEED TO FIGURE OUT TO DO COLLISION STUFFS
				c_collider2d = holder.GetComponent<CircleCollider2D>();
				//Physics.IgnoreCollision(collider, c_collider2d);

				hand = holder.transform.FindChild ("hand").gameObject;

				//Cache references to the hand and its rigidbody
				hand_rb = hand.GetComponent<Rigidbody2D>();
				hand_transform = hand.transform;

				//Set parent of weapon to the hand holding it so that it's position is now relative to the hand
				transform.parent = hand_transform;

				_hingejoint.connectedBody = hand_rb;
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
						rigidbody2D.AddTorque (turnTorque * angleDiff / 360);

				} else if (angleDiff < -anglePercision) {
						rigidbody2D.AddTorque (turnTorque * angleDiff / 360);
				}

			}
		}
	}

	//What is this i dont even
	void onCollisionEnter(Collision collision){
		Debug.Log (collision.gameObject);
		//q = rigidbody2D.transform.rotation;

	}
}
