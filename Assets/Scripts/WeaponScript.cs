using UnityEngine;
using System.Collections;

public class WeaponScript : MonoBehaviour {

	public GameObject hand;
	public int anglePercision = 10;
	public float turnTorque = 80f;
	private Vector2 pointDir = new Vector2(0f,0f);
	private Transform hand_transform;
	private Rigidbody2D hand_rb;
	private HingeJoint2D _hingejoint;
	private Quaternion q;



	// Use this for initialization
	void Start () {


	
	}
	
	// Update is called once per frame
	void Update () {

		if (hand != null){
			//Cache references to the hand and its rigidbody
			hand_transform = hand.transform;
			hand_rb = hand.GetComponent<Rigidbody2D>();
			_hingejoint = this.GetComponent<HingeJoint2D> ();
			_hingejoint.connectedBody = hand_rb;
		
			//Set parent of weapon to the hand holding it so that it's position is now relative to the hand
			transform.parent = hand_transform;

			//Direction you are trying to point the weapon in is based on position of the hand
			pointDir = hand_transform.localPosition;

			//Calculate the angle trying to point in
			var targetAngle = Mathf.Atan2(pointDir.y, pointDir.x) * Mathf.Rad2Deg - 90;



			//Convert angle to positive number 
			if (targetAngle < 0)
							targetAngle += 360;

			Debug.Log (targetAngle);

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

		//rigidbody2D.transform.rotation = Quaternion.RotateTowards(transform.rotation, q, turnSpeed); 
		}
	}

	void onCollisionEnter(Collision collision){
		Debug.Log (collision.gameObject);
		//q = rigidbody2D.transform.rotation;

	}
}
