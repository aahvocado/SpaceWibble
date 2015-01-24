using UnityEngine;
using System.Collections;

public class flyingAsteroidClass : MonoBehaviour {
	public Vector3 initialVelocity;
	public GameObject explodeParticle;
	// Use this for initialization
	void Start () {
//		initialVelocity = new Vector3 ();
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(initialVelocity * Time.deltaTime, Space.World);
		transform.Rotate (new Vector3 (6, 6, 6) * Time.deltaTime);
	}

	void OnTriggerEnter(Collider c){
		if (c.gameObject.tag == "Wall") {
			Destroy(this.gameObject);

			Instantiate(explodeParticle, this.transform.position, Quaternion.identity);
		}
	}
}
