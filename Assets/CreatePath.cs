using UnityEngine;
using System.Collections.Generic;

public class CreatePath : MonoBehaviour {
	public List<Vector3> Points = new List<Vector3>(); //A vector list to store food from for loop
	public int Next = 0, food = 10;//Points will represent the food for fish the seek out
	public bool loop = true;
	public Transform target; //assign this in the inspector
	
	public void PlotPath()
	{
		for(int i = 0; i < food; i++)
		{
			Points.Add (Random.insideUnitSphere * 50f); // this generates spots amount and stores waypoints in Point list based from multiplied distance of a sphere, in this case, 100
			GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			sphere.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
			sphere.name="Food";
			sphere.transform.position=(Points[i]);//Spawn a sphere everywhere there is a point
		}

	}

	public Vector3 NextPoint(){
		return Points [Next];//iterates through Points when called from other class
	}

	public bool IsLastCheckpoint()
	{
		return(Next == Points.Count-1);
	}
	public void AdvanceWaypoint()
	{
		if(loop)
		{

			Next = (Next + 1) % Points.Count; //If loop=true, Next interates 1st until it reachs last index of Points and resets to 0 due to modulus
		}
		else
		{
			if(!IsLastCheckpoint())// While not the last index of point, interate through list
			{

				Next++;//Increment next
			}
		}
	}


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (target) { //checks to see if we have a target assigned
			target.gameObject.GetComponent<Renderer> ().material.color = Color.yellow;
			float distance = (this.transform.position - target.transform.position).magnitude; 
			if (distance < .5f) { 
				Destroy (target.gameObject); //destroy target gameobject
			}
		}
		else{
			if(GameObject.Find ("Food"))
			target = GameObject.Find ("Food").transform; //looks for a new target 
		}
	}
	
}
