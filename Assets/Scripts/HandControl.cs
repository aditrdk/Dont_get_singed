using UnityEngine;
using System.Collections;

public class HandControl : MonoBehaviour {
	
	public GameObject ControlInput;
	private InControlSetup controllerData;
	public float radius = 1.5f;
	public float minradius = 1f;


	public float dampTime = 0.1f; //offset from the viewport center to fix damping
	public float maxspeed = 20;

	private float yvel = 0f;
	private float xvel = 0f;

	public int playerNum;
	private Vector2 rsv;

	private Vector2 lastPos = new Vector2(0,1);
	

	// Use this for initialization
	void Start () {
		controllerData = ControlInput.GetComponent<InControlSetup> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		rsv = controllerData.getRsv (playerNum);

		Vector2 curPos = transform.localPosition;

		float angle = Mathf.Atan2(rsv.x, rsv.y) * Mathf.Rad2Deg;
		Debug.Log (angle);
		/*
		if (targetPos.magnitude < minradius) {
			targetPos = lastPos;
			targetPos.Normalize ();
			targetPos *= minradius;
		}
		*/
		//calculate run force and apply
//		float newX = Mathf.SmoothDamp(curPos.x,targetPos.x, ref xvel, dampTime, maxspeed);
//		float newY = Mathf.SmoothDamp(curPos.y,targetPos.y, ref yvel, dampTime, maxspeed);
//		Vector2 newPos = new Vector2 (newX, newY);
//
//
//		rigidbody2D.transform.localPosition = newPos;
//
//		//Trying to point in a new direction
//		if (curPos.magnitude > 0)
//						lastPos = curPos;
	}
}