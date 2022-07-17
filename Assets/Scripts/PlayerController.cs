using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    private MenuController menuController;
    public static PlayerController instance;

    private Camera cam;
    private Transform camTransform;
    private Rigidbody rb;
    private Unit playerUnit;

    private Vector3 worldMousePos;
    // Spear
    public Spear spear;

    // For bomb placement
    public GameObject BombAsset;
    public float BombCooldown = 5;
    private Bomb PlacedBomb;
    private float TimeSinceBombDetonated = 0;
    
    public float speed;

    [SerializeField]
    private float damping;

    [SerializeField]
    PlayerHUD playerHUD;


    private void Start()
    {
        menuController = FindObjectOfType<MenuController>();
        instance = this;

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

        rb.MovePosition(transform.position + (camR * move.x + move.z * camF) * Time.deltaTime * speed);

        // Spear attacks
        if (Input.GetMouseButtonDown(0))
        {
            spear.AttackIfPossible();
        }

        
        // Bomb placement / detonation
        TimeSinceBombDetonated += Time.deltaTime;
        
        if (Input.GetMouseButtonDown(1)) {

            // If there is already a bomb, detonate it. If not, place one.
            if (PlacedBomb)
            {
                TimeSinceBombDetonated = 0;
                PlacedBomb.Detonate();
            }
            else if (BombCooldown < TimeSinceBombDetonated)
            {
                GameObject bomb = GameObject.Instantiate(BombAsset);
                bomb.transform.position = worldMousePos;
                PlacedBomb = bomb.GetComponent<Bomb>();
            }
        }

    }

    public void Death ()
    {
        menuController.LoadMenuLevel();
    
    }
    
    public static void setSpeed(float speed)
    {
        PlayerController.instance.speed = speed;
    }
}