using UnityEngine;
using System.Collections;

public class StateMachine : MonoBehaviour {

	State CurrentState; //instance state is stored here 
	
	void Update () 
	{
		if(CurrentState != null)
		{
			CurrentState.Update(); //run update method from CurrentState
		}
	}
	
	public void SwitchState(State newState) //Call this for switching state and passing the newState
	{
		if(CurrentState != null)
		{
			CurrentState.Exit(); //exits CurrentState
		}
		
		CurrentState = newState; //set currentState to newState, which is passed over when we call the SwitchState method
		if(newState != null)
		{
			CurrentState.Enter();//run the Enter method from the currentState
		}
	}
}
