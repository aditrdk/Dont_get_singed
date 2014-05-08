using UnityEngine;
using System.Collections;

public class healthmeter : MonoBehaviour {


	public GameObject player;
	private PlayerScript playerScript;
	private MovementControl movementControl;
	
	public float displayedHealth; 
	public Sprite[] sprites;
	private SpriteRenderer spriteRenderer;
	private float health;

	// Use this for initialization
	void Start () {
	
		spriteRenderer = renderer as SpriteRenderer;
		playerScript = player.GetComponent<PlayerScript>();
		movementControl = player.GetComponent<MovementControl>();
		
		
		
	}
	
	// Update is called once per frame
	void Update () {
		if(movementControl.controllerConnected){
			displayedHealth = 10 - playerScript.health;
			if (displayedHealth > 10) displayedHealth = 10;
			else if (displayedHealth < 0) displayedHealth = 0;
			int index = (int)Mathf.Floor(displayedHealth);
			spriteRenderer.sprite = sprites[index];
		}
		else{
			spriteRenderer.sprite = sprites[10];
		}
	
	}
}
