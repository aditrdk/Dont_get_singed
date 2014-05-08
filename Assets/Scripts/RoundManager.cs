using UnityEngine;
using System.Collections;

public class RoundManager : MonoBehaviour {

	public GameObject[] players;
	public PlayerScript[] playerScripts;
	public MovementControl[] movementControl;
	public int roundRestartDelay = 60;
	private int restartTimer = 0;

	// Use this for initialization
	void Start () {
		playerScripts = new PlayerScript[players.Length];
		movementControl = new MovementControl[players.Length];
		
		for(int i = 0; i < players.Length; i++){
			playerScripts[i] = players[i].GetComponent<PlayerScript> ();
			movementControl[i] = players[i].GetComponent<MovementControl> ();
			
		}
	}
	
	// Update is called once per frame
	void Update () {
		int numAlive = 0;
		int winner = 0;
		for(int i = 0; i < players.Length; i++){
			if(!(playerScripts[i].dead) && movementControl[i].controllerConnected){
				numAlive++;
				winner = i+1;
			} 
		}
		
		
		if(numAlive <= 1){
			restartTimer++;
			if(restartTimer >= roundRestartDelay){
				endRound(winner);
				restartTimer = 0;
			}
		}
	}
	
	
	void endRound(int winner){
		

		
		if(winner >= 1) playerScripts[winner - 1].wins ++;
		for(int i = 0; i < players.Length; i++){
			players[i].SendMessage("respawn");
			
		}
	}
}
