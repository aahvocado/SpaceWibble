using UnityEngine;
using System.Collections;

public class HelperFunctions : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static Transform findParentWithTag(string tag, Transform currentObj) {
		while (currentObj.tag != tag) {
			currentObj = currentObj.transform.parent;
		}
		return currentObj;
	}
}
