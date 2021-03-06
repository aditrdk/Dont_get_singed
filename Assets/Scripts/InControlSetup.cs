﻿using UnityEngine;
using InControl;
using System.Collections;

public class InControlSetup : MonoBehaviour {
	

	public InputDevice player1;
	public InputDevice player2;
	public InputDevice player3;
	public InputDevice player4;
	public int numDevices;


	// Use this for initialization
	void Start () {

		InputManager.Setup ();
	
		//InputDevice device = NULL;

	}
	
	public bool deviceConnected(int playerNum){
		if (playerNum <= numDevices) return true;
		else return false;
	
	
	}

	public bool getLeftTrigger(int playerNum){
		if (playerNum > numDevices || playerNum < 1) {
			return false;
		}
		
		if (playerNum == 1) {
			return player1.LeftTrigger.WasPressed;
		} 
		else if (playerNum == 2) {
			return player2.LeftTrigger.WasPressed;
		}
		else if (playerNum == 3) {
			return player3.LeftTrigger.WasPressed;
		}
		else if (playerNum == 4) {
			return player4.LeftTrigger.WasPressed;
		}
		else{
			return false;
		}


	}
	
	public bool getAction4(int playerNum){
		if (playerNum > numDevices || playerNum < 1) {
			return false;
		}
		
		if (playerNum == 1) {
			return player1.Action4.WasPressed;
		} 
		else if (playerNum == 2) {
			return player2.Action4.WasPressed;
		}
		else if (playerNum == 3) {
			return player3.Action4.WasPressed;
		}
		else if (playerNum == 4) {
			return player4.Action4.WasPressed;
		}
		else{
			return false;
		}
		
		
	}
	


	public Vector2 getLsv(int playerNum){
		if (playerNum > numDevices || playerNum < 1) {
			return new Vector2 (0, 0);
		}

		if (playerNum == 1) {
			return player1.LeftStickVector;
		} 
		else if (playerNum == 2) {
			return player2.LeftStickVector;
		}
		else if (playerNum == 3) {
			return player3.LeftStickVector;
		}
		else if (playerNum == 4) {
			return player4.LeftStickVector;
		}
		else{
			return player1.LeftStickVector;
		}

	}
	
	public Vector2 getRsv(int playerNum){
		if (playerNum > numDevices || playerNum < 1) {
			return new Vector2 (0, 0);
		}


		if (playerNum == 1) {
			return player1.RightStickVector;
		} 
		else if (playerNum == 2) {
			return player2.RightStickVector;
		}
		else if (playerNum == 3) {
			return player3.RightStickVector;
		}
		else if (playerNum == 4) {
			return player4.RightStickVector;
		}
		else{
			return player1.RightStickVector;
		}
		
	}

	
	// Update is called once per frame
	void Update () {
		InputManager.Update ();

		numDevices = InputManager.Devices.Count;

		if(numDevices>0) player1 = InputManager.Devices[0];
		if(numDevices>1) player2 = InputManager.Devices[1];
		if(numDevices>2) player3 = InputManager.Devices[2];
		if(numDevices>3) player4 = InputManager.Devices[3];
		

		//lsv = player1.LeftStickVector;
		//rsv = player1.RightStickVector;
		//Debug.Log ("Input");

		//Debug.Log (lsv);


		//for multiple players 
		//var player1 = InputManager.Devices[0];
		/*
		control.IsPressed;   // bool, is currently pressed
		control.WasPressed;  // bool, pressed since previous tick
		control.WasReleased; // bool, released since previous tick
		control.HasChanged;  // bool, has changed since previous tick
		control.State;       // bool, is currently pressed (same as IsPressed)
		control.Value;       // float, in range -1..1 for axes, 0..1 for buttons / triggers
		control.LastState;   // bool, previous tick state
		control.LastValue;   // float, previous tick value
		*/

	}
}
