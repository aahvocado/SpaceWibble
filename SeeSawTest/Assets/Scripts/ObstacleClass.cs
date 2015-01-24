using UnityEngine;
using System.Collections;

public class ObstacleClass : MonoBehaviour {
	public float weight;
	private float minDist = 1.5f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (this.transform.position.z > minDist || this.transform.position.z < -minDist) {
			this.renderer.material.color = Color.green;
		} else {
			this.renderer.material.color = Color.white;
		}

	}

	public float getWeight(){
			return weight;
	}
}
