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
    
    // Spear
    public Spear spear;
    
    // For bomb placement
    public GameObject BombAsset;
    public float BombCooldown = 5;
    private Bomb PlacedBomb;
    private float TimeSinceBombDetonated = 0;
    
    public float speed;
    

    [SerializeField]
    PlayerHUD playerHUD;

    [SerializeField]
    private Transform diceTransform;

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

        if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity, LayerMask.NameToLayer("Player")))
        {
            Vector3 pos = new(hit.point.x, transform.position.y, hit.point.z);
            transform.LookAt(pos);
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
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                
                int layermask = LayerMask.GetMask("Default");
                
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

    public void Death ()
    {
        menuController.LoadMenuLevel();
    
    }
    
    public static void setSpeed(float speed)
    {
        PlayerController.instance.speed = speed;
    }
}