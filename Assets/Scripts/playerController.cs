using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    private Rigidbody rigidBody;
    private Transform transform;
    public float maxSpeed = 10.0f;
    public float moveSpeed = 10.0f;
    public float rotateSpeed = 100.0f;
    public float hoverSpeed = 50.0f;
    private float xInput,zInput,mouseX,mouseY;
    private bool hoverUp,HoverDown = false;

    public float TurningSpeed = 10;
    Vector3 LookAtPos;
    Vector3 SmoothedLookAtPos;

    void Start (){
        rigidBody = GetComponent<Rigidbody>();
        transform = GetComponent<Transform>();
    }

    void Update(){
        xInput = Input.GetAxis("Horizontal");
        zInput = Input.GetAxis("Vertical");
        mouseX = Input.GetAxis("Mouse X") * rotateSpeed;
        mouseY = -Input.GetAxis("Mouse Y") * rotateSpeed;
        hoverUp =  Input.GetKeyDown("left shift");
        HoverDown = Input.GetKeyDown("left ctrl");
    }

    void FixedUpdate(){
          
        Vector3 inputVector = new Vector3 (xInput, 0.0f, zInput);

        if(hoverUp){
            inputVector += Vector3.up * hoverSpeed;
        }else if(HoverDown){
            inputVector -= Vector3.up * hoverSpeed;
        }
        
        //transform.Rotate(mouseY * Time.fixedDeltaTime, mouseX * Time.fixedDeltaTime , 0);
        LookAtPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 100));
        SmoothedLookAtPos = Vector3.Lerp(SmoothedLookAtPos, LookAtPos, Time.deltaTime * 5);
        transform.LookAt(SmoothedLookAtPos);

        if(rigidBody.velocity.magnitude <= maxSpeed){
            rigidBody.AddRelativeForce (inputVector * moveSpeed);
        }

    }
}
