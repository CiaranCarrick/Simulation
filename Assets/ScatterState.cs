using UnityEngine;
using System.Collections;

public class ScatterState : State {
	GameObject enemy;
	public float StateTime, StateTimer = 5f; //A very simple change state condition!

	public ScatterState(GameObject myGameObject):base (myGameObject) //constructor takes same argument as State base class constructor
	{
		
	}
	
	public override void Enter() //Override base State classes Enter Method
	{
			enemy = GameObject.Find ("enemy"); //Find the leader
			myGameObject.GetComponent<SteeringBehaviours>().target = enemy; //and assign target to leader
			myGameObject.GetComponent<SteeringBehaviours> ().FleeBool = true;
			Debug.Log ("Break for it!");
	}
	
	public override void Update()//Override base State classes Update Method
	{
		//This is where we calculate stuff, like the condition to transition to the next state
		StateTime += Time.deltaTime;
		if(StateTime >= StateTimer)
		{
			myGameObject.GetComponent<StateMachine>().SwitchState (new FollowState(myGameObject));
		}	

		
	}
	
	public override void Exit() //override runs over the base class abstract method of the same name (abstract methods can't handle functionality, they are only a blueprint)
	{
		//Clear any active states
		myGameObject.GetComponent<SteeringBehaviours>().TurnOffAll();
	}
}
