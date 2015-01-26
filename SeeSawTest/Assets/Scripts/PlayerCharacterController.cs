using UnityEngine;
using System.Collections;

public class PlayerCharacterController : MonoBehaviour {

	public int id = 1;
	public bool allowKeyboardInput = false;

	CharacterController charController;// = this.GetComponent<CharacterController>();
	public GameObject seeSaw;
	public Transform playerBody;

	//Grabbing / Throwing / Pushing
	public string seeSawMoverTag;
	public float throwStrength = 20f;
	public float grabThreshold = 6f;
	public ColliderCheck grabSphere;
	public GameObject grabbedObject;
	private bool grabKeyDown = false;
	private bool grabKeyUp = false;
	public bool grabChanged = false;

	public float pushPower = 300f;

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
	Vector3 moveDir;
	public float movementMag;
	public float maxSpeed;
	public float currentSpeed;
	public float acceleration;
	public float linearDampening;

	private int groundLayer = 8;



	void Start() {
		seeSaw = GameObject.Find("seesaw");
		seeSawMoverTag = "AffectSeesaw";
		playerBody = transform.Find("Body").transform;
		grabSphere = transform.Find("grabSphere").GetComponent<ColliderCheck>();
		grabSphere.grabThreshold = grabThreshold;

		originUp = transform.up;
		charController = this.GetComponent<CharacterController>();
	}
	
	void Update() {
		//Game mode
		GameMode();

		float moveHorizontal;
		float moveVertical;

		if (allowKeyboardInput) {
			moveVertical = Input.GetAxis("Vertical");
			moveHorizontal = Input.GetAxis("Horizontal");
		} else {
			moveVertical = ControllerInput.LeftAnalog_Y_Axis(id, 0.2f); 
			moveHorizontal = ControllerInput.LeftAnalog_X_Axis(id, 0.2f);
		}
		 
		Vector3 movementInput = new Vector3(moveHorizontal, 0, moveVertical);
		movementInput = movementInput.normalized;
		//Debug.Log (seeSaw.transform.localRotation.eulerAngles.x);
		moveDir = Quaternion.AngleAxis(-seeSaw.transform.rotation.eulerAngles.x, Vector3.forward) * movementInput;
		Debug.DrawLine(new Vector3(0,2,0), new Vector3(moveDir.x, 2 + moveDir.y, moveDir.z), Color.green);
		Debug.DrawLine(transform.position, (((transform.position + moveDir.normalized) - transform.position) * 3) + transform.position, Color.green);


		if (ControllerInput.A_ButtonDown(id) || (allowKeyboardInput && Input.GetKeyDown(KeyCode.LeftShift))) {
			//currentSpeed *= 1.4f;
			currentSpeed = Mathf.Lerp(currentSpeed * 1.2f, 1, acceleration * Time.deltaTime);
		}
		if (Input.GetKeyUp(KeyCode.LeftShift)) {
			//currentSpeed /= 1.4f;
			currentSpeed = Mathf.Lerp(0, currentSpeed / 1.2f, acceleration * Time.deltaTime);
		}

		if (movementInput.magnitude > 0) {
			RotateToMovement(playerBody, movementInput.x, movementInput.z, 12);
			currentSpeed = Mathf.Lerp(currentSpeed, 0.7f, acceleration * Time.deltaTime);
		} else {
			currentSpeed = Mathf.Lerp(0, currentSpeed, acceleration *  Time.deltaTime);
		}

		movementMag = movement.magnitude;
			//			moveDir = Quaternion.AngleAxis(seeSaw.transform.rotation.z, Vector3.forward) * moveDir;
			//Debug.DrawLine(transform.position, (transform.position + new Vector3(moveDir.x, 0, moveDir.z)) * 2, Color.green);
		moveDir *= currentSpeed * Time.deltaTime;
		movement += moveDir; //Quaternion.FromToRotation(Vector3.forward, -target) *

		movement += (linearDampening * -new Vector3(movement.x, 0, movement.z));

		ApplyGravity ();
		isGrounded = charController.isGrounded;
		checkJumping();

		//charController.Move(movement);
		//Vector3.ClampMagnitude(movement, maxSpeed);
		movement = new Vector3(movement.x, verticalSpeed, movement.z);

		//transform.position = Vector3.Lerp(transform.position, movement + transform.position, movementSmoothing);

		Debug.DrawLine(transform.position, (((transform.position + movement.normalized) - transform.position) * movement.magnitude * 1.4f) + transform.position, Color.red);
		charController.Move(movement);

		grabObject();
		throwObject();
	}

	void OnControllerColliderHit(ControllerColliderHit hit) {
		Rigidbody body = hit.collider.attachedRigidbody;
		if (body == null || body.isKinematic)
			return;
		
		if (hit.gameObject.tag == seeSawMoverTag) {
		
			//Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
			Vector3 pushDir = Quaternion.AngleAxis(-seeSaw.transform.rotation.eulerAngles.x, Vector3.forward) * playerBody.forward;

			if (body.GetComponent<ObstacleClass>().weight > grabThreshold) {
				body.velocity = pushDir * pushPower;
			} else {
				body.velocity = pushDir * pushPower * 1.7f;
			}
		}
	}

	public void grabObject() {
		if (grabbedObject != null)
			grabbedObject.transform.position = Vector3.Lerp(grabbedObject.transform.position, transform.position + (playerBody.forward * grabbedObject.transform.localScale.x * 1.5f), 4);
		
		if (ControllerInput.X_Button(id) || (allowKeyboardInput && Input.GetMouseButton(1))) {
			grabSphere.renderer.enabled = true;
			grabSphere.transform.position = transform.position + (playerBody.forward * 1);

			if (grabbedObject == null) {
				if (!grabChanged) {
					grabSphere.isActive = true;
					grabbedObject = grabSphere.currentObject;
					if (grabbedObject != null) {
						grabbedObject.rigidbody.isKinematic = true;
					}
				}
			} else {
			}

			if (ControllerInput.X_ButtonDown(id) || (allowKeyboardInput && Input.GetMouseButtonDown(1))) {
				grabKeyDown = true;
				grabKeyUp = false;
				if (grabbedObject != null) {
					grabbedObject.rigidbody.isKinematic = false;
					grabbedObject = null;
					grabChanged = true;
				}
			}
		} else {
			grabSphere.renderer.enabled = false;
			grabSphere.isActive = false;
		}
		if (ControllerInput.X_ButtonUp(id) || (allowKeyboardInput && Input.GetMouseButtonUp(1))) {
			grabKeyUp = true;
			grabKeyDown = false;
			grabChanged = false;
		}
	}

	public void throwObject() {
		if (ControllerInput.B_ButtonDown(id) || (allowKeyboardInput && Input.GetMouseButtonDown(0))) {
			//grabSphere.renderer.enabled = true;
			//grabSphere.transform.position = transform.position + (playerBody.forward * 1);
			if (grabbedObject != null) {
				grabbedObject.rigidbody.isKinematic = false;
				Vector3 throwDir = Quaternion.AngleAxis(-seeSaw.transform.rotation.eulerAngles.x, Vector3.forward) * playerBody.forward;
				//Vector3 throwDir = playerBody.forward;
				Vector3.Normalize(throwDir);
				throwDir += Vector3.up;
				grabbedObject.rigidbody.AddForce(throwDir * throwStrength);
				grabbedObject = null;
			}
			}
	}
	
	public void checkJumping() {
		if (ControllerInput.A_ButtonDown(id) || (allowKeyboardInput && timeCheck < Time.time)) {
			if (charController.isGrounded) {
				if (mode == PlayerMode.Small) {
					verticalSpeed = verticalStab;
				} else if (mode == PlayerMode.Big) {
					verticalSpeed = verticalStab * 0.5f;
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
			verticalSpeed +=  gravity * Time.deltaTime;
		} else if (charController.isGrounded) {
			verticalSpeed =  gravity * Time.deltaTime;
		} else {
			verticalConstant = transform.position;
			verticalSpeed -= gravity * Time.deltaTime;
		}
	}

	void RotateToMovement (Transform body, float horizontal, float vertical, float turnSmoothing)
	{
		Vector3 targetDirection = new Vector3(horizontal, 0f, vertical);
		Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
		Quaternion newRotation = Quaternion.Lerp(body.rotation, targetRotation, turnSmoothing * Time.deltaTime);
		body.rotation = newRotation;
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
}

