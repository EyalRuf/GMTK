using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{

    private Transform cam;
    private Rigidbody rb;
    private Unit playerUnit;

    [SerializeField]
    private float speed;

    [SerializeField]
    PlayerHUD playerHUD;

    [SerializeField]
    private Transform objectTransform;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main.transform;

        playerUnit = GetComponent<Unit>();
        playerHUD.SetHUD(playerUnit);
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // PROBLEM: Need to change what is 'forward' when dice is rotated. Lookat won't work anymore.
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