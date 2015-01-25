using UnityEngine;
using System.Collections;

public class PlayerCharacterController : MonoBehaviour {

	CharacterController charController;// = this.GetComponent<CharacterController>();
	public GameObject seeSaw;

	//Player Mode
	public enum PlayerMode {Small, Big};
	public PlayerMode mode = PlayerMode.Small;
	private PlayerMode oldMode;

	//Gravity and Jumping
	public ColliderCheck GroundCollider;
	//public GameObject groundCollider;
	public bool isGrounded;
	public bool isInGround;
	public bool isColliding;
	public float verticalSpeed;
	private Vector3 verticalConstant;

	private RaycastHit hit;
	private float rayDistance;

	public float timeCheck;

	public float force;
	private Vector3 originUp;
	public float gravity = -10;

	public float verticalStab = 0.1f;
	public float verticalStabSpeed = 2;
	//	public GUIText countText;
	
	//Speed
//	private float timeCountSpeed;
//	private float timerIntervalSpeed = 1;
//	private Vector3 oldPos;
	Vector3 movement;
	public float movementMag;
	public float maxSpeed;
	public float currentSpeed;
	public float acceleration;
	public float linearDampening;

	private int groundLayer = 8;



	void Start() {
		seeSaw = GameObject.Find("seesaw");

		originUp = transform.up;
		charController = this.GetComponent<CharacterController>();
		//GroundCollider = transform.GetComponentInChildren<ColliderCheck>();
		GroundCollider = GameObject.Find("GroundCollider").GetComponent<ColliderCheck>();
//		groundCollider = new GameObject();
//		groundCollider.AddComponent<SphereCollider>();
//		groundCollider.transform.position = transform.position;
//		groundCollider.transform.parent = transform;
	}
	
	void Update() {
		//Game mode
		GameMode();

		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis("Vertical");
		
		Vector3 movementInput = new Vector3(moveHorizontal, 0, moveVertical);
		movementInput = movementInput.normalized;
		//Debug.Log (seeSaw.transform.localRotation.eulerAngles.x);
		Vector3 moveDir = Quaternion.AngleAxis(-seeSaw.transform.rotation.eulerAngles.x, Vector3.forward) * movementInput;
		Debug.DrawLine(new Vector3(0,2,0), new Vector3(moveDir.x, 2 + moveDir.y, moveDir.z), Color.green);
		Debug.DrawLine(transform.position, (((transform.position + moveDir.normalized) - transform.position) * 3) + transform.position, Color.green);
		
		//	Debug.DrawLine(target + transform.position, transform.position, Color.green);
		//	Debug.DrawLine(movement*30 + transform.position, transform.position, Color.red);


		//CalculateSpeed ();
		//movement = movement ;//e * Time.deltaTime;// * 2 * speedSustain);

		if (Input.GetKeyDown(KeyCode.LeftShift)) {
			//currentSpeed *= 1.4f;
			currentSpeed = Mathf.Lerp(currentSpeed * 1.2f, 1, acceleration * Time.deltaTime);
		}
		if (Input.GetKeyUp(KeyCode.LeftShift)) {
			//currentSpeed /= 1.4f;
			currentSpeed = Mathf.Lerp(0, currentSpeed / 1.2f, acceleration * Time.deltaTime);
		}

		if (movementInput.magnitude > 0) {
			RotateToMovement(movementInput.x, movementInput.z, 12);
			currentSpeed = Mathf.Lerp(currentSpeed, 0.7f, acceleration * Time.deltaTime);
		} else {
			currentSpeed = Mathf.Lerp(0, currentSpeed, acceleration *  Time.deltaTime);
		}

		movementMag = movement.magnitude;
			//			moveDir = Quaternion.AngleAxis(seeSaw.transform.rotation.z, Vector3.forward) * moveDir;
			//Debug.DrawLine(transform.position, (transform.position + new Vector3(moveDir.x, 0, moveDir.z)) * 2, Color.green);
		moveDir *= currentSpeed * Time.deltaTime;
		movement += moveDir; //Quaternion.FromToRotation(Vector3.forward, -target) *

		movement += (linearDampening * -movement);

		ApplyGravity ();
		checkJumping();

		charController.Move(movement);
		movement = new Vector3(movement.x, verticalSpeed, movement.z);

		//transform.position = Vector3.Lerp(transform.position, movement + transform.position, movementSmoothing);
		Vector3.ClampMagnitude(movement, maxSpeed);
		charController.Move(movement);
	}

	public void checkJumping() {
		if (Input.GetKeyDown(KeyCode.Space)) {
			if (charController.isGrounded) {
				if (mode == PlayerMode.Small) {
					verticalSpeed = 0.15f;
				} else if (mode == PlayerMode.Big) {
					verticalSpeed = 0.05f;
				}
			}
		}
	}

	public void ApplyGravity () {
		if (timeCheck < Time.time) {
			//Debug.Log("isGrounded: " + isGrounded);
			timeCheck = Time.time + 10;
		}
		if (!charController.isGrounded) {
			verticalSpeed += 0.2f * gravity * Time.deltaTime;
		} else if (charController.isGrounded) {
			verticalSpeed = 0;
		} else {
			verticalConstant = transform.position;
			verticalSpeed -= gravity * Time.deltaTime;
		}
	}

	void RotateToMovement (float horizontal, float vertical, float turnSmoothing)
	{
		Vector3 targetDirection = new Vector3(horizontal, 0f, vertical);
		Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
		Quaternion newRotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSmoothing * Time.deltaTime);
		transform.rotation = newRotation;
	}


	public void GameMode() {
		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			mode = PlayerMode.Small;
		}
		if (Input.GetKeyDown(KeyCode.Alpha2)){
			mode = PlayerMode.Big;
		}
		if (oldMode != mode) {
			transform.position += Vector3.up * 2f;
			if (mode == PlayerMode.Small) {

			} else {

			}
			oldMode = mode;
		}
	}

	public void CalculateSpeed () {
		if (currentSpeed < maxSpeed) {
			currentSpeed += acceleration;
		} else if (currentSpeed > 0) {
			currentSpeed -= linearDampening;
		}
	}

	public bool IsGrounded () {
		//GroundCollider.transform.position = new Vector3 (transform.position.x, transform.position.y -0.1f, transform.position.z);
		isGrounded = GroundCollider.GroundCheck();
		return isGrounded;
	}

	public bool IsInGrounded () {
		//GroundCollider.transform.position = new Vector3 (transform.position.x, transform.position.y -0.1f, transform.position.z);
		isInGround = GroundCollider.InGroundCheck();
		return isInGround;
	}
}

