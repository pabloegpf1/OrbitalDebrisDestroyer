using UnityEngine;
using System.Collections;

[System.Serializable]
public class PlayerFlightControl : MonoBehaviour{

	public GameObject actual_model; 
	public Transform weapon_hardpoint_1; //"Weapon Hardpoint", "Transform for the barrel of the weapon"
	public GameObject bullet;

	public float speed = 0.0f; //"Base Speed", "Primary flight speed, without afterburners or brakes"
	public float thrustSpeed = 25f; //Afterburner Speed", "Speed when the button for positive thrust is being held down"
	public float turnspeed = 15.0f; //"Turn/Roll Speed", "How fast turns and rolls will be executed "
	public float rollSpeed = 7; //"Roll Speed", "Multiplier for roll speed. Base roll is determined by turn speed"
	public float pitchYaw_strength = 0.5f; //"Pitch/Yaw Multiplier", "Controls the intensity of pitch and yaw inputs"
	public float screen_clamp = 500; //"Screen Clamp (Pixels)", "Once the pointer is more than this many pixels from the center, the input in that direction(s) will be treated as the maximum value."

	[HideInInspector]
	public float roll, yaw, pitch, thrust; 
	[HideInInspector]
	public bool slow_Active = false; //True if brakes are on
	
	float distFromVertical; //Distance in pixels from the vertical center of the screen.
	float distFromHorizontal; //Distance in pixel from the horizontal center of the screen.

	Vector2 mousePos = new Vector2(0,0); 
	
	float DZ = 0; 
	float currentMag = 0f; 
	
	bool thrust_exists = true;
	bool roll_exists = true;
	
	void Start() {

		mousePos = new Vector2(0,0);	
		DZ = CustomPointer.instance.deadzone_radius;
		roll = 0;

	}
	
	void FixedUpdate () {
		
		updateCursorPosition();
		
		pitch = Mathf.Clamp(distFromVertical, -screen_clamp - DZ, screen_clamp  + DZ) * pitchYaw_strength;
		yaw = Mathf.Clamp(distFromHorizontal, -screen_clamp - DZ, screen_clamp  + DZ) * pitchYaw_strength;

		roll = (Input.GetAxis("Horizontal") * rollSpeed);
		thrust = (Input.GetAxis("Vertical") * thrustSpeed);
			
		currentMag = GetComponent<Rigidbody>().velocity.magnitude;
		
		currentMag = Mathf.Lerp(currentMag, thrust, Time.deltaTime);				
				
		GetComponent<Rigidbody>().AddRelativeTorque(
			(pitch * turnspeed * Time.deltaTime),
			(yaw * turnspeed * Time.deltaTime),
			(roll * turnspeed *  (rollSpeed / 2) * Time.deltaTime));
		
		GetComponent<Rigidbody>().velocity = transform.forward * currentMag; //Apply speed
		
	}		
		
	void updateCursorPosition() {

		mousePos = CustomPointer.pointerPosition;
		
		float distV = Vector2.Distance(mousePos, new Vector2(mousePos.x, Screen.height / 2));
		float distH = Vector2.Distance(mousePos, new Vector2(Screen.width / 2, mousePos.y));
		
		if (Mathf.Abs(distV) < DZ)
			distV = 0;
		else 
			distV -= DZ; 
			
		if (Mathf.Abs(distH) < DZ)
			distH = 0;	
		else 
			distH -= DZ;
			
		//Clamping distances to the screen bounds.	
		distFromVertical = Mathf.Clamp(distV, 0, (Screen.height));
		distFromHorizontal = Mathf.Clamp(distH,	0, (Screen.width));	
	
		if (mousePos.x < Screen.width / 2 && distFromHorizontal != 0) {
			distFromHorizontal *= -1;
		}
		if (mousePos.y >= Screen.height / 2 && distFromVertical != 0) {
			distFromVertical *= -1;
		}
	}

	void Update() {
	
		if (Input.GetMouseButtonDown(0)) {
			fireShot();
		}	
	}
	
	public void fireShot() {

		Ray vRay;
		RaycastHit hit;
		GameObject shot1 = (GameObject) GameObject.Instantiate(bullet, weapon_hardpoint_1.position, Quaternion.identity);
		
		if (!CustomPointer.instance.center_lock)
			vRay = Camera.main.ScreenPointToRay(CustomPointer.pointerPosition);
		else
			vRay = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2f, Screen.height / 2f));
		
		//If we make contact with something in the world, we'll make the shot actually go to that point.
		if (Physics.Raycast(vRay, out hit)) {
			shot1.transform.LookAt(hit.point);
			shot1.GetComponent<Rigidbody>().AddForce((shot1.transform.forward) * 9000f);
		} else {
			shot1.GetComponent<Rigidbody>().AddForce((vRay.direction) * 9000f);
		}
	
	}
}