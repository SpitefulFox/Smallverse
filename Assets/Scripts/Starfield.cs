using UnityEngine;
using System.Collections;

public class Starfield : MonoBehaviour {

	public Vector3 beginning;
	public Vector3 end;
	public float speed = 10;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.MoveTowards(transform.position, end, speed);
		if(Vector3.Distance(transform.position, end) < 0.01f)
			transform.position = beginning;
	}
}
