using UnityEngine;
using System.Collections;

public class SeeSaw : MonoBehaviour {
	public GameObject seesaw;

	private Vector3 centerPoint;
	public float rotationPower;
	// Use this for initialization
	void Start () {
		centerPoint = seesaw.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay(Collider col){
		if (col.gameObject.tag == "AffectSeesaw") {
			Vector3 objPos = col.gameObject.transform.position;
			float dist = centerPoint.z - objPos.z;
			float dir = dist > 0 ? -1:1;

//			seesaw.transform.Rotate(new Vector3(0,0,zDiff * rotationPower) * Time.deltaTime );
			Vector3 axis = new Vector3(20*dir,0,0);
			seesaw.transform.RotateAround(centerPoint,axis, 10*Time.deltaTime);
			Debug.Log ("ouch " + dist);

		}
	}

}


