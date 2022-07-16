using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {

    private Rigidbody rb;
    private Ray groundedRay;
    

    private float height;
    private bool isGrounded;
    
    [SerializeField] private float speed;
    [SerializeField] private Transform cam;
    [SerializeField] MeshRenderer render;
    
    



    void Start() {
        rb = GetComponent<Rigidbody>();
        height = render.bounds.size.y;
    }

    // Update is called once per frame
    void Update() {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        float smash = Input.GetAxis("Jump");

        Vector3 move = new Vector3(horizontal, 0f, vertical);

        Vector3 camF = cam.forward;
        Vector3 camR = cam.right;

        camF.y = 0;
        camR.y = 0;
        camF = camF.normalized;
        camR = camR.normalized;

        if(move.magnitude >= 0.1f) {
            rb.MovePosition(transform.position + (camR * move.x + move.z * camF) * Time.deltaTime * speed);
        }

        if(Physics.Raycast(rb.position, Vector3.down, height)) {
            isGrounded = true;
            Debug.Log("Grounded");
        }
        else {
            isGrounded = false;
            Debug.Log("Not Grounded!");
        }

        if(!isGrounded) {
            if(smash >= 0) {
                print("SMASH!!");
            }
        }

    }

    private void OnTriggerStay(Collider other) {
        if (other.transform.tag == "Ground") {
            isGrounded = true;
            Debug.Log("Grounded");
        }
        else{
            isGrounded = false;
            Debug.Log("Not Grounded!");
        }
    }

}
