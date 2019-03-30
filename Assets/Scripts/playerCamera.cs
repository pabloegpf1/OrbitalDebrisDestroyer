using UnityEngine;
using System.Collections;

  public class playerCamera : MonoBehaviour {
  
      public float turnSpeed = 4.0f;
      public GameObject player;
  
      void LateUpdate(){
          transform.position = player.transform.position + Vector3.back*5; 
          transform.rotation = player.transform.rotation;
          //transform.LookAt(player.transform.position);
      }
  }

