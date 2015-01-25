using UnityEngine;
using System.Collections;

public class ColliderCheck : MonoBehaviour {

	public bool isGrounded;
	public bool isInGround;
	private int groundLayer = 8;
	public GameObject player;
	private Vector3 contactVector;

	// Use this for initialization
	void Start () {
		player = GameObject.Find("_Player2");
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log("lol");
	}

	void OnTriggerEnter(Collider col) {
		//Debug.Log ("on trigger");
		Debug.Log ("enter trigger");
		Debug.Log (col.name);
		Debug.Log (col.gameObject.layer);
		
		if (col.gameObject.layer == groundLayer) {
			isGrounded = true;
		//	Debug.Log ("isGround True");
			
			Vector3 colPoint = col.ClosestPointOnBounds(player.transform.position);
			contactVector = colPoint - player.transform.position;
		//	Debug.DrawLine(player.transform.position, colPoint);
		}
	}
	
	void OnTriggerStay(Collider col) {
		//Debug.Log ("on trigger");
		if (col.gameObject.layer == groundLayer) {
			isInGround = true;
		//	Debug.Log ("isGround True");
			
			Vector3 colPoint = col.ClosestPointOnBounds(player.transform.position);
			contactVector = colPoint - player.transform.position;
		//	Debug.DrawLine(player.transform.position, colPoint);
		}
	}
	
	void OnTriggerExit(Collider col) {
		//Debug.Log ("on trigger exit");
		Debug.Log (col.name);
		if (col.gameObject.layer == groundLayer) {
			isGrounded = false;
			isInGround = false;
			
			Vector3 colPoint = col.ClosestPointOnBounds(player.transform.position);
			contactVector = colPoint - player.transform.position;
			Debug.DrawLine(player.transform.position, colPoint);
		}
	}



	public bool GroundCheck() {
		return isGrounded;
	}

	public bool InGroundCheck() {
		return isInGround;
	}

	public Vector3 GetContactVector() {
		return contactVector;
	}
}
