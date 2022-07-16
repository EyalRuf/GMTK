using UnityEngine;

public class Player : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit = RayShooter.ShootRayToMousePosition();
            Vector3 pos = new(hit.point.x, transform.position.y, hit.point.z);
            transform.position = pos;
        }
    }
}
