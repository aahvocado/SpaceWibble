using UnityEngine;
using System.Collections;

public class ObstacleClass : MonoBehaviour {
	public float weight;
	private float minDist = 2.5f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (this.transform.position.x > minDist) {
			this.renderer.material.color = Color.blue;
		}else if(this.transform.position.x < -minDist){
			this.renderer.material.color = Color.green;
		} else {
			this.renderer.material.color = Color.white;
		}

	}

	public bool isDestroyed(){
		return false;
	}

	public float getWeight(){
			return weight;
	}
}
