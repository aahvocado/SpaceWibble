using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	public GameObject ship;//seesaw
	public float speed;
	public float maxRotation;//the same value as the one used on seesaw

	private ArrayList spaceObjects;//the objects that will move
	// Use this for initialization
	void Start () {
		spaceObjects = new ArrayList ();

	}
	
	// Update is called once per frame
	void Update () {
		//remove destroyed objects
		checkDestroyedSpaceObjects ();
		//movement of object stuff
		float shipRotation = ship.transform.rotation.eulerAngles.x;
		if (shipRotation < 360 - maxRotation/3 && shipRotation > 360 - maxRotation) {//ship lean left
				foreach (GameObject so in spaceObjects) {
						so.transform.Translate (Vector3.right * Time.deltaTime * speed, Space.World);
				}
		} else if (shipRotation < maxRotation && shipRotation > maxRotation/3) {//ship lean right
				foreach (GameObject so in spaceObjects) {
						so.transform.Translate (Vector3.left * Time.deltaTime * speed, Space.World);
				}
		} else {

		}
		//spawn timing
//		Debug.Log (Time.time);
		if (Time.time % 50 == 0) {
			Vector3 asteroidSpawnLocation = new Vector3(0,0,30);
			spawnAsteroid(asteroidSpawnLocation);
		}
		if (Time.time % 30 == 0) {
			Vector3 blueSpawnLocation = new Vector3(-20,0,20);
			Vector3 greenSpawnLocation = new Vector3(20,0,20);

			spawnBlue(blueSpawnLocation);
			spawnGreen(greenSpawnLocation);
		}
	}

	void OnTriggerEnter(Collider c){
		foreach (GameObject so in spaceObjects) {
			if(so == c.gameObject){
				if(c.gameObject.name == "BlueFuel"){
					print ("gotblue");
				}else if(c.gameObject.name == "GreenFuel"){
					print ("gotgreen");
				}
			}
		}
	}

	void checkDestroyedSpaceObjects(){
		foreach (GameObject so in spaceObjects) {
			if(so.GetComponent<flyingAsteroidClass>().isDestroyed()){
				Destroy(so);
				spaceObjects.Remove (so);
			}
		}
	}

	void spawnGreen(Vector3 location){
		GameObject objSpawn = (GameObject)Instantiate(Resources.Load("Prefabs/greenFuel", typeof(GameObject)),location,Quaternion.identity);
		objSpawn.transform.localEulerAngles = new Vector3 (-90, 0, 0);
		spaceObjects.Add (objSpawn);
	}
	void spawnBlue(Vector3 location){
		GameObject objSpawn = (GameObject)Instantiate(Resources.Load("Prefabs/blueFuel", typeof(GameObject)),location,Quaternion.identity); 
		objSpawn.transform.localEulerAngles = new Vector3 (-90, 0, 0);
		spaceObjects.Add (objSpawn);
	}

	void spawnAsteroid(Vector3 location){
		GameObject asteroidSpawn = (GameObject)Instantiate(Resources.Load("Prefabs/asteroid", typeof(GameObject)),location,Quaternion.identity); 
		spaceObjects.Add (asteroidSpawn);
	}
}
