using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum SelectionStyle {
	tag, layer
}

public class ColliderCheck : MonoBehaviour {

	public GameObject currentObject;
	public int collisionLayer;
	public SelectionStyle selectionStyle = SelectionStyle.tag;
	public string collisionTag;
	public GameObject player;
	public List<GameObject> contactingObjects;
	public bool isActive = false;
	public bool useThreshold = true;
	public float grabThreshold = 6f;

	// Use this for initialization
	void Start () {
		player = HelperFunctions.findParentWithTag("Player", transform).gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log("lol");
	}

	bool checkType(Collider col) {
		if (selectionStyle == SelectionStyle.tag) {
			if (col.tag == collisionTag)
				return true;
			else
				return false;
		} else if(selectionStyle == SelectionStyle.layer){
			if (col.gameObject.layer == collisionLayer)
				return true;
			else
				return false;
		} else {
			return false;
		}
	}

	bool checkThreshold(Collider col) {
		ObstacleClass obstacle = col.GetComponent<ObstacleClass>();
		if (obstacle != null) {
			if (obstacle.weight < grabThreshold) {
				return true;
			}
		}
		return false;
	}

	void OnTriggerEnter(Collider col) {
		if (checkType(col)) {
			if (isActive) {
				if (useThreshold && checkThreshold(col)) {
					contactingObjects.Add(col.gameObject);
					currentObject = pickClosest();
				}
			}
		}
	}
	
	void OnTriggerStay(Collider col) {
		//Debug.Log ("on trigger");
//		if (col.gameObject.layer == collisionLayer) {
//			if (isActive) {
//			
//			}
//		}
	}
	
	void OnTriggerExit(Collider col) {
		//Debug.Log ("on trigger exit");
		//Debug.Log (col.name);

		contactingObjects.Remove(col.gameObject);
		currentObject = pickClosest();
	}

	public GameObject pickClosest() {
		GameObject closestObject;
		float minDist = 0;
		if (contactingObjects.Count > 0) {
			closestObject = contactingObjects[0];
			foreach(GameObject obj in contactingObjects) {
				if ((obj.transform.position - player.transform.position).magnitude < minDist) {
					minDist = (obj.transform.position - player.transform.position).magnitude;
					closestObject = obj;
				}
			}
		} else {
			closestObject = null;
		}
		return closestObject;
	}

}
