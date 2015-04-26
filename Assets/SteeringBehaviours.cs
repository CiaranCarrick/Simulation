using UnityEngine;
using System.Collections;

public class SteeringBehaviours : MonoBehaviour {

	public Vector3 Force, velocity, offsetPursuitOffset;
	public float MaxSpeed=10f, Mass=1f;
	public GameObject target;
	public bool SeekBool, FleeBool;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		ForceIntegrator ();
	}

	void ForceIntegrator(){
		Vector3 Acceleration = Force / Mass;
		velocity = velocity + Acceleration * Time.deltaTime;//add gameobjects accelaration to the velocity over time
		transform.position = transform.position + velocity * Time.deltaTime;//set position to our position + the velocity over time
		Force = Vector3.zero;
		if(velocity.magnitude > float.Epsilon) //if we have speed
		{
			transform.forward= Vector3.Normalize(velocity); //set our forward direction to use our velocity (difference between us and target) so to always point at target
		}
		velocity *= 0.99f; //Dampings velocity each frame to slow down

		if(SeekBool)
		{
			Force += Seek(target.transform.position); //Seek is a functon that returns a Vector3, and we also pass down our targets position
		}
		if(FleeBool)
		{
			Force += Flee(target.transform.position); //Seek is a functon that returns a Vector3, and we also pass down our targets position
		}
	}

	Vector3 Seek(Vector3 Target)
	{
		Vector3 DesiredVelocity = Target - transform.position;//first we find the desired Velocity - the different between the target and ourselves. This is the vector we need to add force to, which will steer us towards our target
		DesiredVelocity.Normalize(); //let's normalize the vector, so that it keeps it's direction but is of max length 1, so we have control over it's speed (To normalize, divide each x, y, z of the vector by it's own magnitude. Magnitude is calculated by adding the squared values of x,y,z together, then getting the squared root of the result. So (5, 0, 5) = (25, 0, 25) = 50. Square root of 50 is 7.04. This is the magnitude. To normalize, (5 / 7.04, 0 / 7.04, 5 / 7.04) = (0.71, 0, 0.71) Try one yourself!
		DesiredVelocity *= MaxSpeed; //Multiply normalized vector by maxSpeed to make it move faster
		return DesiredVelocity - velocity;//we return the difference of our desired velocity and velocity so that we are steered in the direction of the targets direction
	}
	Vector3 Flee(Vector3 Target)
	{
		Vector3 DesiredVelocity = Target - transform.position;//first we find the desired Velocity - the different between the target and ourselves. This is the vector we need to add force to, which will steer us towards our target
		float distance= DesiredVelocity.magnitude;
		if (distance < 5f) {
			DesiredVelocity.Normalize (); //let's normalize the vector, so that it keeps it's direction but is of max length 1, so we have control over it's speed (To normalize, divide each x, y, z of the vector by it's own magnitude. Magnitude is calculated by adding the squared values of x,y,z together, then getting the squared root of the result. So (5, 0, 5) = (25, 0, 25) = 50. Square root of 50 is 7.04. This is the magnitude. To normalize, (5 / 7.04, 0 / 7.04, 5 / 7.04) = (0.71, 0, 0.71)
			DesiredVelocity *= MaxSpeed; //Multiply normalized vector by maxSpeed to make it move faster
			return velocity - DesiredVelocity;//we return the difference of our desired velocity and velocity so that we are steered in the direction of the targets direction
		} 
		else //if greater than 5f
		{
			return Vector3.zero; //Return (0,0,0) Stop moving
		}
	}
}
