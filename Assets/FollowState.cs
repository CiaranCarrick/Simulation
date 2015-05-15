using UnityEngine;
using System.Collections.Generic;

public class FollowState : State {//FollowState Inherits from the state class rather than MonoBehaviour
	GameObject enemy;
	GameObject leader; //Reference to the Leader gameobject
	float distance;
	public FollowState(GameObject myGameObject):base (myGameObject) //constructor takes same argument as State base class constructor
	{
		
	}
	
	public override void Enter()
	{
		enemy = GameObject.Find ("enemy"); //Find the enemy
		leader = GameObject.Find ("Leader"); //Find the leader
		myGameObject.GetComponent<SteeringBehaviours>().target = leader; //and assign target to leader

		//Toggle SteeringBehaviours
		myGameObject.GetComponent<SteeringBehaviours>().OffsetPursuitBool = true;
		//Debug.Log("Follow the leader!");
	}
	
	public override void Update()
	{
		if (enemy != null) {
			Vector3 desiredVel = enemy.transform.position - myGameObject.transform.position;//find the distance between agent and target
			distance = desiredVel.magnitude; //Store length of Vector3 as float 
			if (distance < 20f) { //and assign target to leader
				myGameObject.GetComponent<StateMachine> ().SwitchState (new ScatterState (myGameObject));// If Enemy enters within 20 units switch states
			}
		}
		if (myGameObject.GetComponent<SteeringBehaviours> ().tagged.Count == 0 && distance>20f){//If outside the flock, regroup to leader
			//Debug.Log ("Lonely:(");
			myGameObject.GetComponent<SteeringBehaviours> ().SeekBool = true;
		}
		else
			myGameObject.GetComponent<SteeringBehaviours> ().SeekBool = false;// Exit seek, return to flock

	}

	public override void Exit() //override runs over the base class abstract method of the same name (abstract methods can't handle functionality, they are only a blueprint)
	{
		//Clear any active states
		myGameObject.GetComponent<SteeringBehaviours>().TurnOffAll();
	}
}
