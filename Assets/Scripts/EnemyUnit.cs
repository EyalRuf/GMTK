using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class EnemyUnit : Unit
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

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

            // This destroys game object so do it after
            base.Death();
        }
    }
}