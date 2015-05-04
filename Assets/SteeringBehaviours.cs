using UnityEngine;
using System.Collections.Generic;

public class SteeringBehaviours : MonoBehaviour {
	
	public Vector3 Force, velocity, offsetPursuitOffset;
	public float MaxSpeed=150f, Mass=1f, overlapRadius = 0.5f; // Speed limiter, objects weight and our overlap radius
	public GameObject target;
	public Vector3 SeekTarget;
	public List<GameObject> tagged  = new List<GameObject>();
	public bool SeekBool, FleeBool, PursueBool, ArriveBool, OffsetPursuitBool, PathBool;

	CreatePath path=new CreatePath();
	
	// Use this for initialization
	void Start() {
		if (PathBool != false) {
			path.PlotPath ();// Only want to plot path for leader
		} else
			PathBool = false;

		offsetPursuitOffset = transform.position; //set the offsetPursuitOffset vector to the starting position of each follower (it's easier 

	}
	public void TurnOffAll()// Used for exit method of the StateMachine
	{
		SeekBool = false;
		FleeBool= false;
		PathBool = false;
		PursueBool = false;
		ArriveBool = false;
		OffsetPursuitBool= false;
	}
	
	// Update is called once per frame
	void Update () {
		ForceIntegrator ();
	}
	
	void ForceIntegrator(){
		Vector3 Acceleration = Force / Mass; //Create Acceleration
		velocity = velocity + Acceleration * Time.deltaTime;//add gameobjects accelaration to velocity over time
		transform.position = transform.position + velocity * Time.deltaTime;//set position to our position + the velocity over time
		Force = Vector3.zero;
		if(velocity.magnitude > float.Epsilon) //if we have speed
		{
			transform.forward= Vector3.Normalize(velocity); //set our forward direction to use our velocity (difference between us and target) so to always point at target
		}
		velocity *= 0.99f; //Dampings velocity each frame to slow down
		Force = CalculateWeightedPrioritised ();
		NoOverlap(); //Prevent overlapping followers
	}
	
	Vector3 CalculateWeightedPrioritised() //weight and calculate the combined forces from active steering behaviours. 
	{
		Vector3 steeringForce = Vector3.zero;
		if(SeekBool)
		{
			Force = Seek(SeekTarget) * 0.8f;
			if(AccumulateForce(ref steeringForce, Force) == false) //Steeringforce is passed to AccumulateForce, TotalFroce is then calculated and returned to the ref parameter. We also pass in the Force we want to add (notice the line above, where Force = SteeringBehaviour(target) etc
			{
				return steeringForce; //return the method, only returning the steeringForce at THIS POINT
			}
		}
		if(FleeBool)
		{
			Force = Flee(target.transform.position) * 0.6f;
			if(AccumulateForce(ref steeringForce, Force) == false) //if the code gets this far and Seek is also active, then we are updating steeringForce with not only the Flee Force, but also the Seek Force (assuming there is remaining Force)
			{
				return steeringForce; 
			}
		}
		if(ArriveBool)
		{
			Force = Arrive(target.transform.position) * 0.3f; 
			if(AccumulateForce(ref steeringForce, Force) == false)
			{
				return steeringForce;
			}
		}
		if(PursueBool)
		{
			Force = Pursue(target) * 0.3f;
			if(AccumulateForce(ref steeringForce, Force) == false)
			{
				return steeringForce;
			}
		}
		int taggedCount=0;
		if (OffsetPursuitBool) {
			taggedCount = FindBoids (25);//Parameter is the Radius for locating boids
		}
		if (OffsetPursuitBool && taggedCount > 0) {
			Force = OffsetPursuit (offsetPursuitOffset) * 0.6f;
			if (AccumulateForce (ref steeringForce, Force) == false) {
				return steeringForce;
			}
		}
		if(PathBool)
		{
			Force = PathFollow() * 0.6f;
			if(AccumulateForce(ref steeringForce, Force) == false)
			{
				return steeringForce;
			}
		}
		return steeringForce; //Returns the force variable from accumlateforce, which is stored in the remaining variable
	}
	
	bool AccumulateForce(ref Vector3 Totalforce, Vector3 force) //This method takes the force from the above method (eg. force = Seek(target.transform.position)) and passes it down to be checked against the remaining force available. 
	{
		float soFar = Totalforce.magnitude; //soFar = Totalforce amount
		float remaining = MaxSpeed - soFar; // Compares the total force allowed to the force at the time
		if(remaining <= 0) //add no more if remaining is 0
		{
			return false;
		}
		float ForcetoAdd = force.magnitude; //store force magnitude toadd
		if(ForcetoAdd < remaining) //if toAdd is less than what is remaining
		{
			Totalforce += Force; //add the passed force to the running total
		}
		else //otherwise
		{
			Totalforce += Force.normalized * remaining; //take the PART of toAdd that will bring us up to maxForce and add that to runningTotal, effectively chopping off or truncating the rest (this is why we use remaining)
		}
		return true; //return true! we can still add more force
	}
	
	Vector3 Seek(Vector3 Target)
	{
		Vector3 DesiredVelocity = Target - transform.position;//first we find the desired Velocity - the different between the target and ourselves. This is the vector we need to add Force to, which will steer us towards our target
		DesiredVelocity.Normalize(); //let's normalize the vector, so that it keeps it's direction but is of max length 1, so we have control over it's speed (To normalize, divide each x, y, z of the vector by it's own magnitude. Magnitude is calculated by adding the squared values of x,y,z together, then getting the squared root of the result. So (5, 0, 5) = (25, 0, 25) = 50. Square root of 50 is 7.04. This is the magnitude. To normalize, (5 / 7.04, 0 / 7.04, 5 / 7.04) = (0.71, 0, 0.71) Try one yourself!
		DesiredVelocity *= MaxSpeed; //Multiply normalized vector by maxSpeed to make it move faster
		return DesiredVelocity - velocity;//we return the difference of our desired velocity and velocity so that we are steered in the direction of the targets direction
	}
	Vector3 Flee(Vector3 Target)
	{
		Vector3 DesiredVelocity = Target - transform.position;//first we find the desired Velocity - the different between the target and ourselves. This is the vector we need to add Force to, which will steer us towards our target
		float distance= DesiredVelocity.magnitude;
		if (distance < 20f) {
			DesiredVelocity.Normalize (); // keeps the same direction, sets length to 1.0, helps control speed
			DesiredVelocity *= MaxSpeed; //Multiply normalized vector by maxSpeed to increase speed of agent
			return velocity - DesiredVelocity;//Return difference of desired velocity and velocity to steer towards target
		} 
		else //if greater than 5f
		{
			return Vector3.zero; //Return (0,0,0) Stop moving
		}
	}
	Vector3 Pursue(GameObject target) //Pursue is useful for calculating a future interception position of a target. Used with Seek
	{
		Vector3 desiredVel = target.transform.position - transform.position;//find the distance between agent and target
		float distance = desiredVel.magnitude; //Store length of Vector3 as float 
		float lookAhead = distance / MaxSpeed; //Adds some distance to tracked position, The distance is divided by maxSpeed ensuring it always scales as they change
		Vector3 desPos = target.transform.position+(lookAhead * target.GetComponent<SteeringBehaviours>().velocity); //our final vector, we tell our agent to Seek the targets position with the added lookAhead value, multiplied by the targets velocity, so that the look ahead can always be calculated in the correct direction
		return Seek (desPos); //Return Vector3 through Seek method which runs with interception information.
	}
	Vector3 Arrive(Vector3 targetPos) //
	{
		Vector3 toTarget = targetPos - transform.position;
		float distance = toTarget.magnitude;
		if(distance <= 1f)
		{
			return Vector3.zero;
		}
		float slowingDistance = 8.0f; //at what radius from the target does gameobject begin to slow down
		float decelerateTweaker = MaxSpeed / 10f; 
		float rampedSpeed = MaxSpeed * (distance / slowingDistance * decelerateTweaker); //ramped speed scales based on the distance to our target 
		float newSpeed = Mathf.Min (rampedSpeed, MaxSpeed); //returns the smaller of the two speeds
		Vector3 desiredVel = newSpeed * toTarget.normalized; //use the newSpeed * by normalized toTarget vector
		return desiredVel - velocity; //return the difference between desiredVel and our velocity and apply a Force to it
	}
	Vector3 OffsetPursuit(Vector3 offset)//This controls the offset of gameobject, manage offsets from inspector
	{
		Vector3 desiredVel = Vector3.zero;
		desiredVel = target.transform.TransformPoint(offset);
		float distance = (desiredVel - transform.position).magnitude;
		float lookAhead = distance / MaxSpeed; //the lookAhead is how much we should look in front of our target
		desiredVel = desiredVel +(lookAhead * target.GetComponent<SteeringBehaviours>().velocity);
		return Arrive (desiredVel);
		
	}



	Vector3 PathFollow()
	{
		float distance = (transform.position - path.NextPoint()).magnitude;
		Debug.DrawLine(transform.position, path.Points[path.Next], Color.white);
		
		if(distance < 0.5f)
		{
			path.AdvanceWaypoint();
		}
		if(!path.loop && path.IsLastCheckpoint())
		{
			return Arrive (path.NextPoint());// If not looping and are arriving at final checkpoint, dock slowly via arrive
		}
		else
		{
			return Seek (path.NextPoint());//otherwise continue to next point
		}
	}
	void NoOverlap() //make sure our boids can't overlap with each other
	{
		foreach(GameObject boid in tagged)
		{
			Vector3 toOther = boid.transform.position - transform.position;//Store distance for each boid
			float distance = toOther.magnitude;
			toOther.Normalize();
			float overlap = overlapRadius + boid.GetComponent<SteeringBehaviours>().overlapRadius - distance;
			if(overlap >= 0)
			{
				boid.transform.position = boid.transform.position + toOther * overlap;//Repel other agents by toOther and overlap amount
			}
		}
	}
	int FindBoids(float radius) //Locate Boids
	{
		tagged.Clear ();//Clear list
		GameObject[] AllBoids = GameObject.FindGameObjectsWithTag("Boid"); //fill gameobject list with tagged gameobjects named Boid
		foreach(GameObject boid in AllBoids)
		{
			if(boid != this.gameObject) //If it is not this object
			{
				if((this.transform.position - boid.transform.position).magnitude < radius) //If the object is within the leaders radius
				{
					tagged.Add (boid);// Add tagged gameobject to list
				}
			}
		}
		return tagged.Count;//Return List
	}

//	public void kill(GameObject player){//Used with Enemy stalkState, wanted to have enemy kill off each of the flock till leader remained
//		Destroy (player);
//	}
}
