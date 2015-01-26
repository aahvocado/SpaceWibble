using UnityEngine;
using System.Collections;

public class SeeSaw : MonoBehaviour {
	public GameObject seesaw;
	
	private Vector3 centerPoint;//location of the pivot
	public float maxRotation;//in degrees I think
	public float rotationPower;//multiplicative power of rotation blah
	public Vector3 axis;
	public float minDistanceFromCenter;//the minimum distance an object has to be from the center to affect the seesaw

	private ArrayList contactObjects;
	private string[] tagsThatAffectSeesaw;
	
	public enum seesawTypes {evenRotation, weightBased, weightDifference};
	public seesawTypes seesawType = seesawTypes.evenRotation;
	// Use this for initialization
	void Start () {
		centerPoint = seesaw.transform.position;
		tagsThatAffectSeesaw = new string[]{"AffectSeesaw", "Player"};
		contactObjects = new ArrayList();
	}
	
	// Update is called once per frame
	void Update () {
				ArrayList leftObjects = new ArrayList ();
				ArrayList rightObjects = new ArrayList ();
				foreach (GameObject ssobj in contactObjects) {
						int dir = getObjDirection (ssobj.transform.position);
						if (dir < 0) {
								leftObjects.Add (ssobj);
						} else if (dir > 0) {
								rightObjects.Add (ssobj);
						}
				}
//		Debug.Log ("Left: " + leftObjects.Count + " vs Right: " + rightObjects.Count);
				if (leftObjects.Count > 0 || rightObjects.Count > 0) {
						float leftWeight = 0;
						float rightWeight = 0;
						foreach (GameObject gameobj in leftObjects) {
								leftWeight = leftWeight + gameobj.GetComponent<ObstacleClass>().getWeight ();
						}
						foreach (GameObject gameobj in rightObjects) {
								rightWeight = rightWeight + gameobj.GetComponent<ObstacleClass> ().getWeight ();
						}
//						Debug.Log ("Left wt: " + leftWeight + " vs Righ wt: " + rightWeight);
			
						axis = Vector3.zero;
						float weightPower = 0;
						if (leftWeight != rightWeight) {
								switch (seesawType) {
								case seesawTypes.evenRotation://rotates the same amount regardless of weight
										weightPower = 5;
										break;
								case seesawTypes.weightBased://rotates the heaviest side by amount based on weight
										if (Mathf.Abs (leftWeight) > Mathf.Abs (rightWeight)) {//left heavy
												weightPower = Mathf.Abs (leftWeight);
										} else {//right heavy
												weightPower = Mathf.Abs (rightWeight);
										}
										break;
								case seesawTypes.weightDifference://rotates the heaviest side by the difference between both sides
										if (Mathf.Abs (leftWeight) > Mathf.Abs (rightWeight)) {//left heavy
												weightPower = Mathf.Abs (leftWeight - rightWeight);
										} else {//right heavy
												weightPower = Mathf.Abs (rightWeight - leftWeight);
										}
										break;
								}
								//rotation direction
								float angle = seesaw.transform.rotation.eulerAngles.x;
								if (Mathf.Abs (leftWeight) > Mathf.Abs (rightWeight)) {//left heavy
									if(angle < 360-maxRotation && angle>180){
									}else{
										axis = new Vector3 (0, 0, 1);
									}
								} else {//right heavy
									if(angle > maxRotation && angle<180){
									}else{
										axis = new Vector3 (0, 0, -1);
									}
								}
								//don't rotate if it's not that much
								if (Mathf.Abs (weightPower) < .5) {
										weightPower = 0;
								}
								seesaw.transform.RotateAround (centerPoint, axis, weightPower * rotationPower * Time.deltaTime);
						}
		}
	}
	
	void OnTriggerEnter(Collider col){
		if (isAffectsSeesaw(col.transform.tag)) {
			contactObjects.Add(col.gameObject);
		}
	}
	//
	void OnTriggerExit(Collider col){
		if (isAffectsSeesaw(col.transform.tag)) {
			contactObjects.Remove(col.gameObject);
		}
	}
	
	//gets the distance, in case we like switch formulas or normalize these things
	float getDist(Vector3 a, Vector3 b){
		return (b.x - a.x);
	}
	
	//returns 1 if right of seesaw center, -1 if left
	int getObjDirection(Vector3 objPoint){
		//Vector3 objPos = objPoint.gameObject.transform.position;
		float dist = getDist(objPoint, centerPoint);
		if(dist != 0 && Mathf.Abs (dist)>minDistanceFromCenter){
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

