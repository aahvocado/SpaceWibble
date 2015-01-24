using UnityEngine;
using System.Collections;

public class SeeSaw : MonoBehaviour {
	public GameObject seesaw;

	private Vector3 centerPoint;//location of the pivot
	public float rotationPower;//multiplicative power of rotation blah

	private ArrayList leftObjects;
	private ArrayList rightObjects;
	private string[] tagsThatAffectSeesaw;
	// Use this for initialization
	void Start () {
		centerPoint = seesaw.transform.position;
		tagsThatAffectSeesaw = new string[]{"AffectSeesaw", "Player"};
		leftObjects = new ArrayList();
		rightObjects = new ArrayList();
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log ("Left Size: " + leftObjects.Count + " vs Right Size: " + rightObjects.Count);

		float leftWeight = 0;
		float rightWeight = 0;
		foreach(GameObject gameobj in leftObjects){
			leftWeight = leftWeight + getDist(centerPoint, gameobj.transform.position);
		}
		foreach(GameObject gameobj in rightObjects){
			rightWeight = rightWeight + getDist(centerPoint, gameobj.transform.position);
		}
		Debug.Log ("Left Weight: " + leftWeight + " vs Right Weight: " + rightWeight);

		Vector3 axis = Mathf.Abs(leftWeight) > Mathf.Abs(rightWeight) ? new Vector3(leftWeight,0,0):new Vector3(rightWeight,0,0);
		seesaw.transform.RotateAround(centerPoint, axis, 10*Time.deltaTime);
	}

	void OnTriggerEnter(Collider col){
		if (isAffectsSeesaw(col.transform.tag)) {
			int dir = getObjDirection(col.gameObject.transform.position);
			if(dir > 0){//right 
				rightObjects.Add(col.gameObject);
			}else if(dir < 0){
				leftObjects.Add (col.gameObject);
			}
		}
	}
	//
	void OnTriggerExit(Collider col){
		if (isAffectsSeesaw(col.transform.tag)) {
			int dir = getObjDirection(col.gameObject.transform.position);
			if(dir > 0){//right 
				rightObjects.Remove(col.gameObject);
			}else if(dir < 0){
				leftObjects.Remove(col.gameObject);
			}
		}
	}

	//gets the distance, in case we like switch formulas or normalize these things
	float getDist(Vector3 a, Vector3 b){
		return (b.z - a.z);
	}

	//returns 1 if right of seesaw center, -1 if left
	int getObjDirection(Vector3 objPoint){
		//Vector3 objPos = objPoint.gameObject.transform.position;
		float dist = getDist(objPoint, centerPoint);
		if(dist != 0){
			int dir = dist > 0 ? -1:1;
			return dir;
		}else{
			return 0;
		}
	}
	//is this one of the tags that affects the seesaw
	bool isAffectsSeesaw(string tagName){
		foreach(string item in tagsThatAffectSeesaw){
			if(tagName == item){
				return true;
			}
		}
		return false;
	}
}


