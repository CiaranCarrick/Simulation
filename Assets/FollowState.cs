using UnityEngine;
using System.Collections;

public class FollowState : State {

	public float changeStateTime, changeStateTimer = 5f; //A very simple change state condition!
	
	public FollowState(GameObject myGameObject):base (myGameObject) //constructor takes same argument as State base class constructor
	{
		
	}
	
	public override void Enter() //override runs over the base class abstract method of the same name (abstract methods can't handle functionality, they are only a blueprint)
	{
		//This is where you toggle the steering behaviours ON!
		myGameObject.GetComponent<SteeringBehaviours>().OffsetPursuitBool = true;
		
		Debug.Log("boids are following leader!");
	}
	
	public override void Update() //override runs over the base class abstract method of the same name (abstract methods can't handle functionality, they are only a blueprint)
	{
	}
	
	public override void Exit() //override runs over the base class abstract method of the same name (abstract methods can't handle functionality, they are only a blueprint)
	{
		//this is where we turn off all steering behaviours, as the new state we transition to will enable only the ones it needs
		myGameObject.GetComponent<SteeringBehaviours>().TurnOffAll();
	}
}
