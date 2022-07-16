using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    [SerializeField] private DiceRoller playerDiceRoller;
    [SerializeField] private float upForce = 3f;
    [SerializeField] private float horizontalForce = 3f;
    [SerializeField] private float RotationForce = 3f;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            playerDiceRoller.ExplosionDiceRoll(transform.position, upForce, horizontalForce, RotationForce);
        }
    }
}
