using UnityEngine;
using System.Collections;


[System.Serializable]
public class CameraFlightFollow : MonoBehaviour {

	public Transform target; //What the camera looks at. Generally the targeter.
	public PlayerFlightControl control; //The PlayerFlightControl script that is in play.
	
	public float follow_distance = 3.0f; //How far behind the camera will follow the targeter.
	public float camera_elevation = 3.0f; //How high the camera will rise above the targeter's Z axis.
	public float follow_tightness = 5.0f; //How closely the camera will follow the target. Higher values are snappier, lower results in a more lazy follow.

	public float rotation_tightness = 10.0f; //How closely the camera will react to rotations, similar to above.
	public float yawMultiplier = 0.005f; //Curbs the extremes of input. This should be a really small number. Might need to be tweaked, but do it as a last resort.
		
	public static CameraFlightFollow instance; //The instance of this class. Should only be one.
	
	void Awake() {
	
		instance = this;
	
	}
	
	void FixedUpdate () {
		
		//Calculate where we want the camera to be.
		Vector3 newPosition = target.TransformPoint(control.yaw * yawMultiplier, camera_elevation, -follow_distance);

		//Get the difference between the current location and the target's current location.
		Vector3 positionDifference = target.position - transform.position;

		//Move the camera towards the new position.
		transform.position = Vector3.Lerp (transform.position, newPosition, Time.deltaTime * follow_tightness);
		
		Quaternion newRotation = Quaternion.LookRotation(positionDifference, target.up);
		transform.rotation = Quaternion.Slerp (transform.rotation, newRotation, Time.deltaTime * rotation_tightness);

	}
}