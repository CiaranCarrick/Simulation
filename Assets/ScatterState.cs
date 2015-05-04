using UnityEngine;
using System.Collections;

public class ScatterState : State {
	GameObject enemy;
	public float changeStateTime, changeStateTimer = 3f; //A very simple change state condition!

	public ScatterState(GameObject myGameObject):base (myGameObject) //constructor takes same argument as State base class constructor
	{
		
	}
	
	public override void Enter() //override runs over the base class abstract method of the same name (abstract methods can't handle functionality, they are only a blueprint)
	{
			enemy = GameObject.Find ("enemy"); //Find the leader
			myGameObject.GetComponent<SteeringBehaviours>().target = enemy; //and assign target to leader
			myGameObject.GetComponent<SteeringBehaviours> ().FleeBool = true;
			Debug.Log ("Break for it!");

	}
	
	public override void Update() //override runs over the base class abstract method of the same name (abstract methods can't handle functionality, they are only a blueprint)
	{
		//This is where we calculate stuff, like the condition to transition to the next state
		changeStateTime += Time.deltaTime;
		if(changeStateTime >= changeStateTimer)
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
