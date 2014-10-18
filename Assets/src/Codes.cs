using UnityEngine;
using System.Collections;

public class InputCode {

	public string Horizontal {get; private set;}
	public string Vertical {get; private set;}
	public string AltHorizontal {get; private set;}
	public string AltVertical {get; private set;}
	public string PrimaryAction {get; private set;}
	public string Select {get; private set;}
	public string Cancel {get; private set;}
	public string LeftRightTrigger {get; private set;}
	public string Ability1 {get; private set;}

	public InputCode (int playerNumber){
		
		switch (playerNumber){
			case 1:
				Horizontal = "Horizontal";
				Vertical = "Vertical";
				AltHorizontal = "RightJoyHorizontal";
				AltVertical = "RightJoyVertical";
				PrimaryAction = "Fire1";
				Select = "return";
				Cancel = "escape";
				LeftRightTrigger = "LeftRightTrigger";
				Ability1 = "Player1Ability1";
				
				break;
		}
	}
	
	public const float AxisThresholdNegative = -0.2f;
	public const float AxisThresholdPositive = 0.2f;

}
