using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    private Camera cam;
    private Transform camTransform;
    private Rigidbody rb;
    private Unit playerUnit;

    // For bomb placement
    public GameObject BombAsset;
    private Bomb PlacedBomb;
    
    [SerializeField]
    private float speed;

    [SerializeField]
    PlayerHUD playerHUD;

    [SerializeField]
    private Transform objectTransform;

    [SerializeField]
    float damping = 2;

    private Vector3 worldMousePos;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
        camTransform = cam.transform;

        playerUnit = GetComponent<Unit>();
        playerHUD.SetHUD(playerUnit);
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Ray mousePosRay = cam.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(mousePosRay, out RaycastHit rayInfo, maxDistance: 500)) {
            worldMousePos = rayInfo.point;
            var rotation = Quaternion.LookRotation (new Vector3(worldMousePos.x, transform.position.y, worldMousePos.z) - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
        }


        Vector3 move = new Vector3(horizontal, 0f, vertical);

        Vector3 camF = camTransform.forward;
        Vector3 camR = camTransform.right;

        camF.y = 0;
        camR.y = 0;
        camF = camF.normalized;
        camR = camR.normalized;

        if (move.magnitude >= 0.1f)
        {
            rb.MovePosition(transform.position + (camR * move.x + move.z * camF) * Time.deltaTime * speed);
        }
        
        // Bomb placement / detonation
        if (Input.GetMouseButtonDown(0)) {

            // If there is already a bomb, detonate it. If not, place one.
            if (PlacedBomb)
                PlacedBomb.Detonate();
            else
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                
                int layermask = LayerMask.GetMask("Ground");
                
                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, layermask))
                {
                    GameObject bomb = GameObject.Instantiate(BombAsset);
                    bomb.transform.position = hitInfo.point;

                    PlacedBomb = bomb.GetComponent<Bomb>();
                }
            }
        }

    }
}