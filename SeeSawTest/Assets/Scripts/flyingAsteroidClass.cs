using UnityEngine;
using System.Collections;

public class flyingAsteroidClass : MonoBehaviour {
	public Vector3 initialVelocity;
	public GameObject explodeParticle;
	public bool shouldRotate;
	private bool destroyed;
	// Use this for initialization
	void Start () {
//		initialVelocity = new Vector3 ();
		destroyed = false;
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(initialVelocity * Time.deltaTime, Space.World);
		if (shouldRotate) {
				transform.Rotate (new Vector3 (6, 6, 6) * Time.deltaTime);
		}
	}

	void OnTriggerEnter(Collider c){
		if (c.gameObject.tag == "Wall") {
//			Destroy(this.gameObject);
			destroyed = true;
			Instantiate(explodeParticle, this.transform.position, Quaternion.identity);
		}
	}

	public bool isDestroyed(){
		return destroyed;
	}

}
