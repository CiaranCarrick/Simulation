using UnityEngine;
using System.Collections;
//Any classes, states that inherit from this must have the same members as it e.g. methods, constructors
public abstract class State {
	
	protected GameObject myGameObject;//protected allows inherited classes access to myGameObject
	public State(GameObject gameobject) //constructor assigns myGameObject
	{
		this.myGameObject = gameobject;
	}
	public abstract void Update(); //All abstract methods must be added to states and classes which inherit from this class
	public abstract void Enter();
	public abstract void Exit();
}