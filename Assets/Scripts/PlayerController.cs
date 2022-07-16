using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {

    private Rigidbody rb;
    
    [SerializeField] 
    private float speed;
    private Transform cam;
    [SerializeField]
    private Transform objectTransform;
    private void Start() 
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main.transform;
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
        {
            Vector3 pos = new(hit.point.x, transform.position.y, hit.point.z);
            objectTransform.LookAt(pos);
        }

        Vector3 move = new Vector3(horizontal, 0f, vertical);

        Vector3 camF = cam.forward;
        Vector3 camR = cam.right;

        camF.y = 0;
        camR.y = 0;
        camF = camF.normalized;
        camR = camR.normalized;

        if (move.magnitude >= 0.1f)
        {
            rb.MovePosition(transform.position + (camR * move.x + move.z * camF) * Time.deltaTime * speed);
        }

    }
}
