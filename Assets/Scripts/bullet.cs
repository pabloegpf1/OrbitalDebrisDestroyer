using UnityEngine;
using System.Collections;

public class bullet : MonoBehaviour {

	public GameObject explo;
	private float deleteTime = 5.0f;
	
	void Update () {
		deleteTime -= Time.deltaTime;
     	if ( deleteTime < 0 ){
         	Destroy(gameObject);
     	}
	}
	
	void OnCollisionEnter(Collision col) {
		GameObject.Instantiate(explo, col.contacts[0].point, Quaternion.identity);
		Destroy(gameObject);
	}
	
	
}
