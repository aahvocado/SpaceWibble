using UnityEngine;
using System.Collections;

public class PlayerCharacterController : MonoBehaviour {

	CharacterController charController;// = this.GetComponent<CharacterController>();

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
	public GameObject mainCam;
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
	Vector3 oldPos;
	public float maxSpeed;
	public float acceleration;
	public float currentSpeed;
	public float linearDampening;
	public float movementSmoothing;

	private int groundLayer = 9;



	void Start() {
		oldPos = transform.position;
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

		//	Debug.DrawLine(target + transform.position, transform.position, Color.green);
		//	Debug.DrawLine(movement*30 + transform.position, transform.position, Color.red);

		if (movement.magnitude < maxSpeed) {
			movement += (new Vector3(movementInput.x, 0, movementInput.z)) * (currentSpeed); //Quaternion.FromToRotation(Vector3.forward, -target) * 
		}
		//CalculateSpeed ();
		//movement = movement ;//e * Time.deltaTime;// * 2 * speedSustain);

		if (Input.GetKeyDown(KeyCode.LeftShift)) {
			currentSpeed *= 2;
		}
		if (Input.GetKeyUp(KeyCode.LeftShift)) {
			currentSpeed /= 2;
		}

		if (Input.GetKeyDown(KeyCode.Space)) {
			if (charController.isGrounded) {
				if (mode == PlayerMode.Small) {
					verticalSpeed = 0.15f;
				} else if (mode == PlayerMode.Big) {
					verticalSpeed = 0.05f;
				}
			}
		}

		//Dampening
		//Vector3 dampen = new Vector3(oldPos.x, 0, oldPos.z) - new Vector3(transform.position.x, 0, transform.position.z);
		//dampen *= linearDampening;
		movement += (linearDampening * -movement);


		isGrounded = false;
		//IsGrounded ();
		//IsInGrounded ();
		ApplyGravity ();
		//RotateSphere ();
		//IsColliding();

		movement = new Vector3(movement.x, verticalSpeed, movement.z);
		
		//transform.position = Vector3.Lerp(transform.position, movement + transform.position, movementSmoothing);
		charController.Move(movement);
		oldPos = transform.position;
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

	public void ApplyGravity () {
		if (timeCheck < Time.time) {
			//Debug.Log("isGrounded: " + isGrounded);
			timeCheck = Time.time + 10;
		}
		if (isInGround) {

//			if (Physics.Raycast(GroundCollider.transform.position, -GroundCollider.transform.up  , out hit, rayDistance)) {
////				if (Vector3.Angle(hit.normal, Vector3.up) > slideLimit)
////					sliding = true;
//				Debug.Log("ray size: " + rayDistance);
//				Debug.DrawLine(GroundCollider.transform.position, hit.point, Color.blue);
//				if (rayDistance > 1f) {
//					verticalSpeed += rayDistance -1f;
//				}
//			} else {
				verticalSpeed += 0.2f * gravity * Time.deltaTime;
			//}

		} else if (isGrounded) {
			verticalSpeed = 0;
		} else {
			verticalConstant = transform.position;
			verticalSpeed -= gravity * Time.deltaTime;
		}
	}



	public bool IsGrounded () {
		GroundCollider.transform.position = new Vector3 (transform.position.x, transform.position.y -0.1f, transform.position.z);
		isGrounded = GroundCollider.GroundCheck();
		return isGrounded;
	}

	public bool IsInGrounded () {
		GroundCollider.transform.position = new Vector3 (transform.position.x, transform.position.y -0.1f, transform.position.z);
		isInGround = GroundCollider.InGroundCheck();
		return isInGround;
	}

	public void IsColliding() {
		if (isColliding) {
			//transform.position += Vector3.up * verticalStab;
			verticalSpeed = verticalStab;
		}
	}

	void OnTriggerStay(Collider col) {
		Debug.Log ("on trigger");
		if (col.gameObject.layer == groundLayer) {
			//Debug.Log ("on trigger ground");
			isGrounded = true;
			isColliding = true;

		} else {
			//Debug.Log ("on trigger not ground");
			isColliding = false;
		}
	}
}
//	public void ApplyJumping ()
//	{
//		// Prevent jumping too fast after each other
//		if (lastJumpTime + jumpRepeatTime > Time.time)
//			return;
//		
//		if (IsGrounded()) {
//			// Jump
//			// - Only when pressing the button down
//			// - With a timeout so you can press the button slightly before landing		
//			if (canJump && Time.time < lastJumpButtonTime + jumpTimeout) {
//				verticalSpeed = CalculateJumpVerticalSpeed (jumpHeight);
//				SendMessage("DidJump", SendMessageOptions.DontRequireReceiver);
//			}
//		}
//	}
//
//
//	public void ApplyGravity ()
//	{
//		if (isControllable)	// don't move player at all if not controllable.
//		{
//			// Apply gravity
//			var jumpButton = Input.GetButton("Jump");
//			
//			
//			// When we reach the apex of the jump we send out a message
//			if (jumping && !jumpingReachedApex && verticalSpeed <= 0.0)
//			{
//				jumpingReachedApex = true;
//				SendMessage("DidJumpReachApex", SendMessageOptions.DontRequireReceiver);
//			}
//			
//			if (IsGrounded ())
//				verticalSpeed = 0.0;
//			else
//				verticalSpeed -= gravity * Time.deltaTime;
//		}
//	}
//
//	public void CalculateJumpVerticalSpeed (targetJumpHeight : float)
//	{
//		// From the jump height and gravity we deduce the upwards speed 
//		// for the character to reach at the apex.
//		return Mathf.Sqrt(2 * targetJumpHeight * gravity);
//	}
//
//	public void DidJump ()
//	{
//		jumping = true;
//		jumpingReachedApex = false;
//		lastJumpTime = Time.time;
//		lastJumpStartHeight = transform.position.y;
//		lastJumpButtonTime = -10;
//		
//		_characterState = CharacterState.Jumping;
//	}
