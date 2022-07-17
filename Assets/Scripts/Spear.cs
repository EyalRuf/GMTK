using System;
using UnityEngine;
using System.Collections;
using FMOD.Studio;
using FMODUnity;

public class Spear : MonoBehaviour
{
    [Header("Setup")]
    public Transform SpearTransform;
    public Transform AttackDirection;
    public DiceRoller Player;
    public Transform SpearOrigin;

    [Header("Configuration")]
    public float AttackDistance;
    
    [Range(0.0f, 0.2f)]
    public float StandardPositionSmoothTime;
    [Range(0.0f, 1.0f)]
    public float StandardRotationSmoothFactor;

    public float PrepareDuration;
    [Range(0.0f, 0.2f)]
    public float PreparePositionSmoothTime;
    [Range(0.0f, 1.0f)]
    public float PrepareRotationSmoothFactor;

    public float AttackDuration;
    [Range(0.0f, 0.2f)]
    public float AttackPositionSmoothTime;
    [Range(0.0f, 1.0f)]
    public float AttackRotationSmoothFactor;

    public bool OnlyKillOne;

    private bool HasKilled;
    
    private bool IsAttacking = false;
    private Vector3 CurrentVelocity = Vector3.zero;

    private Vector3 TargetPosition;
    private Quaternion TargetRotation;
    
    private float PositionSmoothTime = 0.05f;
    private float RotationSmoothFactor = 0.05f;

    [Header("Animation")]
    public Animator attack;
    public Animator spearJab;
    public Animator spearJab2;

    public EventReference EventReference;
    private EventInstance spearJabSound;

    public EventReference deathSoundReference;
    private EventInstance deathSound;

    private void Start()
    {
        spearJabSound = RuntimeManager.CreateInstance(EventReference);
        deathSound = RuntimeManager.CreateInstance(deathSoundReference);

        PositionSmoothTime = StandardPositionSmoothTime;
        RotationSmoothFactor = StandardRotationSmoothFactor;

        SpearTransform.position = SpearOrigin.position;
        SpearTransform.rotation = SpearOrigin.rotation;
    }

    private void Update()
    {
        if (!IsAttacking)
        {
            TargetPosition = SpearOrigin.position;
            TargetRotation = SpearOrigin.rotation;
        }
            
        SpearTransform.position = Vector3.SmoothDamp(SpearTransform.position, TargetPosition, ref CurrentVelocity, PositionSmoothTime);
        SpearTransform.rotation = Quaternion.Lerp(SpearTransform.rotation, TargetRotation, RotationSmoothFactor);
    }
    
    IEnumerator Attack()
    {
        IsAttacking = true;
        HasKilled = false;
        
        //play animation
        spearJab.SetTrigger("Jab");
        spearJab2.SetTrigger("Jab");

        spearJabSound.start();

        TargetPosition = AttackDirection.position;
        Vector3 attackDirection = AttackDirection.forward;
        TargetRotation = Quaternion.LookRotation(attackDirection);
        
        PositionSmoothTime = PreparePositionSmoothTime;
        RotationSmoothFactor = PrepareRotationSmoothFactor;
        
        yield return new WaitForSeconds(PrepareDuration);

        TargetPosition += attackDirection * AttackDistance;
        
        PositionSmoothTime = AttackPositionSmoothTime;
        RotationSmoothFactor = AttackRotationSmoothFactor;
        
        yield return new WaitForSeconds(AttackDuration);
        
        PositionSmoothTime = StandardPositionSmoothTime;
        RotationSmoothFactor = StandardRotationSmoothFactor;
        
        IsAttacking = false;
    }
    
    public void AttackIfPossible()
    {
        if (!IsAttacking)
            StartCoroutine(Attack());
    }

    private void OnTriggerStay(Collider other)
    {
        DiceRoller enemy = other.gameObject.GetComponent<DiceRoller>();

        if (enemy)
        {
            int enemyNumber = (int)enemy.GetNumber();
            int playerNumber = (int)Player.GetNumber();
        
            int enemyLayer = LayerMask.NameToLayer("Enemy");

            if (IsAttacking && other.gameObject.layer == enemyLayer)
            {
                if (enemyNumber <= playerNumber && (!HasKilled || !OnlyKillOne))
                {
                    //Debug.Log("Die, die, die!");
                    GameManager.AmountOfActiveEnemies--;  // REMOVE FROM ACTIVE ENEMIES
                    enemy.GetComponent<Unit>().Damage(100);
                    deathSound.start();

                    //play an animation
                    attack.SetTrigger("attackGood");

                    HasKilled = true;
                } else
                {
                    //Debug.Log("Oopsie!");
                    attack.SetTrigger("attackBad");
                }
            }
        }
    }
}
