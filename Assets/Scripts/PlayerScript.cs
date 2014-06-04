using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {


	public float health = 10.0f;
	public GameObject currentWeapon;
	public int playerNum;
	public GameObject hand;
	
	private MovementControl movementControl;
	
	public int wins;
	public int kills;
	public int deaths;
	
	public int lifeSteal = 2;
	
	public GameObject body;
	public bool dead;
	public Sprite deadSprite;
	public Sprite aliveSprite;
	
	//Renderers
	public SpriteRenderer _handrenderer;
	public SpriteRenderer _bodyrenderer;
	
	public GameObject startingWeapon;
	
	private GameObject lastAttacker;
	
	
	// Use this for initialization
	void Awake () {
		movementControl = GetComponent<MovementControl>();
		playerNum = movementControl.playerNum;
		_bodyrenderer = body.renderer as SpriteRenderer;
		_handrenderer = hand.renderer as SpriteRenderer;
		_bodyrenderer.sprite = aliveSprite;
		
		
		currentWeapon = Instantiate(startingWeapon, transform.localPosition, transform.localRotation) as GameObject;
		startingWeapon = currentWeapon;
		//currentWeapon.SendMessage ("getPicked", this.gameObject);
		respawn ();
		
		
		
		
	}
	
	// Update is called once per frame
	void Update () {
		//Check if Dead
		if(health <= 0f){
			if(!dead){
				playerDie();
			}
			_bodyrenderer.sprite = deadSprite;
			_handrenderer.enabled = false;
		}
	}
	
	void Damage(float damage){
		health -= damage;
	}
	
	void Attacker(GameObject attacker){
		lastAttacker = attacker;
	}
	
	//Called only once on death
	void playerDie(){
	
		lastAttacker.GetComponent<PlayerScript>().kills ++;
		lastAttacker.GetComponent<PlayerScript>().health += lifeSteal;
	
		dead = true;
		deaths++;
		rigidbody2D.velocity = new Vector2(0f, 0f);
		if(currentWeapon != null) currentWeapon.SendMessage("onDrop");
		collider2D.enabled = false;
		_bodyrenderer.sortingLayerID = 0;
		movementControl.enabled = false;

		
	
	}
	
	void respawn(){
	
		health = 10;
		_bodyrenderer.sprite = aliveSprite;
		dead = false;
		collider2D.enabled = true;
		movementControl.enabled = true;
		_bodyrenderer.sortingLayerID = 2;
		transform.localPosition = movementControl.startPos;
		if(movementControl.controllerConnected){
			_handrenderer.enabled = true;
		
			currentWeapon.gameObject.SendMessage ("onDrop");
			startingWeapon.gameObject.SendMessage ("getPicked", this.gameObject);
		}
		
		
	
	}
	
	void OnGUI() {
		//GUI.Label(new Rect(10 + 20*playerNum, 10, 20, 20), health.ToString());
	}

	
}
