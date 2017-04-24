using UnityEngine;
using System.Collections;

public class SelfDestruct : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Invoke("Destruct", 1f);
	}
	
	public void Destruct(){
		Destroy(gameObject);
	}
	
	
}
