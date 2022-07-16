using System;
using UnityEngine;
using System.Collections;

public class Spear : MonoBehaviour
{
    public Transform SpearTransform;
    public Transform Player;
    public Transform SpearOrigin;

    private bool IsAttacking = false;
    private Vector3 CurrentVelocity = Vector3.zero;

    private Vector3 TargetPosition;
    private Quaternion TargetRotation;
    private float PositionSmoothTime = 0.05f;
    private float RotationSmoothFactor = 0.05f;

    private void Start()
    {
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
        
        TargetPosition = Player.position;
        Vector3 attackDirection = Player.forward;
        TargetRotation = Quaternion.LookRotation(attackDirection);
        
        PositionSmoothTime = 0.5f;
        RotationSmoothFactor = 0.05f;
        
        yield return new WaitForSeconds(0.25f);

        TargetPosition += attackDirection * 1;
        
        PositionSmoothTime = 0.1f;
        RotationSmoothFactor = 1f;
        
        yield return new WaitForSeconds(0.5f);
        
        PositionSmoothTime = 0.05f;
        RotationSmoothFactor = 0.05f;
        
        IsAttacking = false;
    }
    
    public void AttackIfPossible()
    {
        if (!IsAttacking)
            StartCoroutine(Attack());
    }

    private void OnTriggerStay(Collider other)
    {
        int enemyLayer = LayerMask.NameToLayer("Enemy");
        
        // TODO: Add check if the current enemy can actually be killed
        if (IsAttacking && other.gameObject.layer == enemyLayer)
        {
            Debug.Log("Die, die, die!");
            
            Destroy(other.gameObject);
        }
    }
}
