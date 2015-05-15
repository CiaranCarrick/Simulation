using UnityEngine;
using System.Collections;

public class EscapeState : State{
	float StateTime, StateTimer=3f;
	public GameObject boid; //Reference to the Leader gameobject
	
	public EscapeState(GameObject myGameObject):
		base (myGameObject)
	{
	}
	
	public override void Enter()
	{
		Debug.Log ("run");
		myGameObject.GetComponent<SteeringBehaviours>().FleeBool= true;
	}
	public override void Update()
	{
		StateTime += Time.deltaTime;
		if(StateTime >= StateTimer)
		{
		myGameObject.GetComponent<StateMachine> ().SwitchState (new StalkState(myGameObject));
		}	
	}
	public override void Exit()
	{
		//Clear any active states
		myGameObject.GetComponent<SteeringBehaviours>().TurnOffAll();
	}
}
