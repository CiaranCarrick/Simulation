﻿using UnityEngine;
using System.Collections;

public class StalkState : State {

	public GameObject boid; //Reference to the Leader gameobject

	public StalkState(GameObject myGameObject):
		base (myGameObject) //constructor takes same argument as State base class constructor
	{
	}
	
	public override void Enter() //override runs over the base class abstract method of the same name (abstract methods can't handle functionality, they are only a blueprint)
	{

		if (myGameObject.GetComponent<SteeringBehaviours> ().target == null) {
			boid = GameObject.FindGameObjectWithTag ("Boid");
			myGameObject.GetComponent<SteeringBehaviours> ().target = boid; //assign the leader to the target
		}
		Debug.Log ("Stalking");
		//myGameObject.GetComponent<SteeringBehaviours>().OffsetPursuitBool = true;
		myGameObject.GetComponent<SteeringBehaviours>().PursueBool= true;
	}
	public override void Update() //override runs over the base class abstract method of the same name (abstract methods can't handle functionality, they are only a blueprint)
	{
		if (myGameObject.GetComponent<SteeringBehaviours> ().target == null) { //assign the leader to the target
				Enter ();//Recall Enter
		} else

		if (boid) { //checks to see if we have a target assigned
			float distance = (myGameObject.transform.position - boid.transform.position).magnitude; 
			if (distance < 3f) { 
				myGameObject.GetComponent<SteeringBehaviours> ().PursueBool = false;
				myGameObject.GetComponent<SteeringBehaviours> ().FleeBool = true;
			}
			if(distance>50){
				myGameObject.GetComponent<SteeringBehaviours> ().FleeBool = false;
				myGameObject.GetComponent<SteeringBehaviours> ().PursueBool = true;
			}
		}
	}
	public override void Exit() //override runs over the base class abstract method of the same name (abstract methods can't handle functionality, they are only a blueprint)
	{
		//Clear any active states
		myGameObject.GetComponent<SteeringBehaviours>().TurnOffAll();
	}
}
