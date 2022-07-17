using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : Unit
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
    public SpearSUm spearsum;

    // For bomb placement
    
    public GameObject BombAsset;
    public float BombCooldown = 5;
    public Material InactiveBombMaterial;
    public Material ActiveBombMaterial;
    public float BombInactiveDuration = 2f;
    private Bomb PlacedBomb;
    private float TimeSinceBombDetonated = 10;
    private float TimeSinceBombThrown = 0;
    
    public float speed;

    [SerializeField]
    private float damping;

    [SerializeField]
    PlayerHUD playerHUD;

    [SerializeField]
    float throwForce = 2;

    [SerializeField]
    float upwardForce = 2;

    float cd = 0;
    public float spearCD = 1;

    private void Awake() => instance = this;
    private void Start()
    {
        menuController = FindObjectOfType<MenuController>();

        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
        camTransform = cam.transform;

        playerHUD = FindObjectOfType<PlayerHUD>();

        if (playerHUD != null)
        {
            playerUnit = GetComponent<Unit>();
            playerHUD.SetHUD(playerUnit);
        }
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Ray mousePosRay = cam.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(mousePosRay, out RaycastHit rayInfo, Mathf.Infinity, 11)) {

            worldMousePos = rayInfo.point;
            var rotation = Quaternion.LookRotation (new Vector3(worldMousePos.x, transform.position.y, worldMousePos.z) - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
        }

        Vector3 move = new Vector3(horizontal, 0f, vertical);

        // Prevent speed strafing
        if (1 < move.magnitude)
            move = move.normalized;
        
        Vector3 camF = camTransform.forward;
        Vector3 camR = camTransform.right;

        camF.y = 0;
        camR.y = 0;
        camF = camF.normalized;
        camR = camR.normalized;

        rb.MovePosition(rb.position + (camR * move.x + move.z * camF) * Time.deltaTime * speed);

        // Spear attacks
        if (Input.GetMouseButtonDown(0))
        {
            if (cd < 0)
            {
                spear.AttackIfPossible();
                cd = spearCD;
            }
        }

        cd -= Time.deltaTime;

        // Bomb placement / detonation
        TimeSinceBombDetonated += Time.deltaTime;
        TimeSinceBombThrown += Time.deltaTime;
        if (PlacedBomb && BombInactiveDuration < TimeSinceBombThrown)
            PlacedBomb.GetComponent<MeshRenderer>().material = ActiveBombMaterial;
        
        if (Input.GetMouseButtonDown(1)) {

            // If there is already a bomb, detonate it. If not, place one.
            if (PlacedBomb)
            {
                if (BombInactiveDuration < TimeSinceBombThrown)
                {
                    TimeSinceBombDetonated = 0;
                    PlacedBomb.Detonate();
                    playerHUD?.DetonateBomb(BombCooldown);
                    PlacedBomb = null;
                }
            }
            else if (BombCooldown < TimeSinceBombDetonated)
            {
                TimeSinceBombThrown = 0;
                
                GameObject bomb = GameObject.Instantiate(BombAsset, rb.position + (transform.forward * 1),  Quaternion.identity);
                Rigidbody bombRb = bomb.GetComponent<Rigidbody>();
                
                bomb.GetComponent<MeshRenderer>().material = InactiveBombMaterial;
                
                //bombRb.position = rb.position + (transform.forward * 2);
                Vector3 forceToAdd = transform.forward * throwForce + transform.up * upwardForce;
                bombRb.AddForce(forceToAdd, ForceMode.Impulse);
                
                PlacedBomb = bomb.GetComponent<Bomb>();
                playerHUD?.SetBomb();
            }
        }

    }

    public override void Death()
    {
        if (menuController == null)
        {
            Destroy(gameObject);
        }
        else
        {
            menuController.LoadMenuLevel();
        }
    }

    public override void Damage(int amount)
    {
        base.Damage(amount);
        // animation & sound

        playerHUD?.UpdateHeartUI((float) currentHealth / (float) maxHealth);
    }

    public static void setSpeed(float speed)
    {
        PlayerController.instance.speed = speed;
    }
}