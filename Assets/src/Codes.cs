using UnityEngine;
using System.Collections;

public class InputCode {

	
	public string ButtonA {get; private set;}
	public string ButtonB {get; private set;}
	public string ButtonX {get; private set;}
	public string ButtonY {get; private set;}
	
	public string ButtonBack {get; private set;}
	public string ButtonStart {get; private set;}
	
	public string LeftStickX {get; private set;}
	public string LeftStickY {get; private set;}
	public string LeftStickPress {get; private set;}
	
	public string RightStickX {get; private set;}
	public string RightSticky {get; private set;}
	public string RightStickPress {get; private set;}
	
	public string LeftBumper {get; private set;}
	public string RightBumber {get; private set;}
	
	public string LeftRightTrigger {get; private set;}	
	

	public InputCode (int playerNumber){
		
		ButtonA = string.Format("Player{0}A", playerNumber);
		ButtonB = string.Format("Player{0}B", playerNumber);
		ButtonX = string.Format("Player{0}X", playerNumber);
		ButtonY = string.Format("Player{0}Y", playerNumber);
		ButtonBack = string.Format("Player{0}Back", playerNumber);
		ButtonStart = string.Format("Player{0}Start", playerNumber);
		LeftStickX = string.Format("Player{0}LeftStickX", playerNumber);
		LeftStickY = string.Format("Player{0}LeftStickY", playerNumber);
		LeftStickPress = string.Format("Player{0}LeftStickPress", playerNumber);
		RightStickX = string.Format("Player{0}RightStickX", playerNumber);
		RightSticky = string.Format("Player{0}RightStickY", playerNumber);
		RightStickPress = string.Format("Player{0}RightStickPress", playerNumber);
		LeftBumper = string.Format("Player{0}LeftBumper", playerNumber);
		RightBumber = string.Format("Player{0}RightBumper", playerNumber);
		LeftRightTrigger = string.Format("Player{0}LeftRightTrigger", playerNumber);

	}
	
	public const float AxisThresholdNegative = -0.2f;
	public const float AxisThresholdPositive = 0.2f;

}
