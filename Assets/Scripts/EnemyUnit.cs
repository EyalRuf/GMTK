using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace Assets.Scripts
{
    public class EnemyUnit : Unit
    {
        void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                other.GetComponent<Unit>().Damage(35);
            }
        }

        public override void Death()
        {
            // Anim + SOund
            PlayerHUD plyrHud = FindObjectOfType<PlayerHUD>();
            if (plyrHud)
            {
                plyrHud.KillCounter();
            }

            // This destroys game object so do it after
            base.Death();
        }
    }
}