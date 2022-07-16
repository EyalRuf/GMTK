using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {

    private Rigidbody rb;
    private Unit playerUnit;
    
    [SerializeField] private float speed;
    [SerializeField] private Transform cam;
    [SerializeField] PlayerHUD playerHUD; 


    void Start() {
        rb = GetComponent<Rigidbody>();
        playerUnit = GetComponent<Unit>(); 
        playerHUD.SetHUD(playerUnit);
    }

    // Update is called once per frame
    void Update() {

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

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

    }
}
