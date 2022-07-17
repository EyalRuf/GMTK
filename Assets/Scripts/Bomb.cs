using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField]
    float sqrBombRadius = 2f;

    [SerializeField]
    float expolsionForce = 2f;
    [SerializeField]
    private float upForce = 3f;
    [SerializeField]
    private float horizontalForce = 3f;
    [SerializeField]
    private float rotationForce = 3f;

    public float height;
    Ray ray;
    MeshRenderer renda;


    public EventReference bombSizzlingRef;
    public EventReference bombExplosionRef;
    public EventReference bombThrowRef;
    private EventInstance bombSizzling;
    private EventInstance bombExplosion;
    private EventInstance bombThrow;

    private void Start() {
        bombSizzling = RuntimeManager.CreateInstance(bombSizzlingRef);
        bombExplosion = RuntimeManager.CreateInstance(bombExplosionRef);
        bombThrow = RuntimeManager.CreateInstance(bombThrowRef);
        renda = GetComponent<MeshRenderer>();
        bombSizzling.start();
        bombSizzling.release();
        bombThrow.start();
        bombThrow.release();
    }

    void OnTriggerStay(Collider other) {
        if (other.transform.tag == "Ground")
        {
            GetComponent<Rigidbody>().isKinematic = false;
            transform.Find("BombRing").gameObject.SetActive(true);
            Debug.Log("hello");
        }
    }

    public void Detonate()
    {
        bombExplosion.start();
        bombExplosion.release();
        DiceRoller[] dice = Object.FindObjectsOfType<DiceRoller>();

        Vector3 position = transform.position;
        
        foreach (DiceRoller die in dice) {

            Vector3 offset = position - die.transform.position;
            offset.y = 0;

            if (Vector3.SqrMagnitude(offset) < sqrBombRadius)
            {
                die.ExplosionDiceRoll(transform.position, upForce, horizontalForce, rotationForce);
            }
        }

        Destroy(gameObject);
    }
}
