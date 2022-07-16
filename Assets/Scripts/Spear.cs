using System;
using UnityEngine;
using System.Collections;

public class Spear : MonoBehaviour
{
    public Transform Player;
    public Transform SpearOrigin;

    private Vector3 AttackPosition;
    private Vector3 AttackDirection;

    public float AttackDistance;
    public float PrepareAttackDuration;
    public float AttackDuration;
    public float AfterAttackDuration;

    private bool IsAttacking;
    
    private void Update()
    {
        transform.position = SpearOrigin.position;
    }
    
    IEnumerator Attack()
    {
        IsAttacking = true;
        
        Vector3 preparePosition = transform.position = Player.position;
        transform.rotation = Player.rotation;
        
        yield return new WaitForSeconds(PrepareAttackDuration);

        for (float time = 0; time < AttackDuration; time += Time.deltaTime)
        {
            float alpha = time / AttackDuration;

            transform.position = preparePosition + transform.forward * AttackDistance * alpha;

            yield return null;
        }

        transform.position = preparePosition + transform.forward * AttackDistance;
        
        yield return new WaitForSeconds(AfterAttackDuration);
        
        IsAttacking = false;
    }
    
    public void AttackIfPossible()
    {
        if (!IsAttacking)
        {
            StartCoroutine(Attack());
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        throw new NotImplementedException();
    }
}
