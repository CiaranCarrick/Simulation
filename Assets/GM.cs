using UnityEngine;
using System.Collections;

public class GM : MonoBehaviour {
	GameObject leader;
	public GameObject[] boids;

	void Start () 
	{
		leader = GameObject.Find ("Leader");
		FindBoids();
		BoidsStartState();
	}
	
	void FindBoids()
	{
		boids = GameObject.FindGameObjectsWithTag("Boid");
	}
	
	void BoidsStartState()
	{
		foreach(GameObject boid in boids)
		{
			boid.GetComponent<StateMachine>().SwitchState (new FollowState(boid));
		}
		leader.GetComponent<StateMachine>().SwitchState (new SearchState(leader));
	}
}
