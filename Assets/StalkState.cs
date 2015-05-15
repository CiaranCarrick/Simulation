using UnityEngine;
using System.Collections;

public class StalkState : State {

	public GameObject[] boid;//Reference to the Leader gameobject
	public GameObject target;
	public float StateTime, StateTimer = 5f; //A very simple change state condition!

	public StalkState(GameObject myGameObject): base (myGameObject) //constructor takes same argument as State base class constructor
	{
	}
	
	public override void Enter()
	{
		myGameObject.GetComponent<SteeringBehaviours> ().target = null;
		if (boid == null) {
			boid = GameObject.FindGameObjectsWithTag("Boid");//Fill GameObject array with gameobjects tagged Boid
		}
		if (boid.Length > 0) {
			if (myGameObject.GetComponent<SteeringBehaviours> ().target == null) {
				target = boid [Random.Range (0, boid.Length)];
				if (target.name == "Leader") {
					target = boid [Random.Range (0, boid.Length)];
					myGameObject.GetComponent<SteeringBehaviours> ().target = target; //assign the leader to the target
				} 
				else
					myGameObject.GetComponent<SteeringBehaviours> ().target = target; //assign the leader to the target
			}
		} 
		else
			return;
		//Debug.Log ("Stalking");
		//myGameObject.GetComponent<SteeringBehaviours>().OffsetPursuitBool = true;
		myGameObject.GetComponent<SteeringBehaviours>().PursueBool= true;
	}
	public override void Update() //override runs over the base class abstract method of the same name (abstract methods can't handle functionality, they are only a blueprint)
	{
		if (target != null) { //checks to see if we have a target assigned
			float distance = (myGameObject.transform.position - target.transform.position).magnitude; 
			if (distance < 2f) {
				myGameObject.GetComponent<SteeringBehaviours> ().kill(target);
				myGameObject.GetComponent<StateMachine> ().SwitchState (new EscapeState(myGameObject));
			}
		}
	}

	public override void Exit() //override runs over the base class abstract method of the same name (abstract methods can't handle functionality, they are only a blueprint)
	{
		//Clear any active states
		myGameObject.GetComponent<SteeringBehaviours>().TurnOffAll();
	}
}
