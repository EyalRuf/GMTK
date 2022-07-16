using UnityEngine;

public class PosConstraint : MonoBehaviour
{
    private Vector3 offset;
    private void Awake()
    {
        offset = transform.localPosition / 2;
    }

    private void FixedUpdate()
    {
        transform.position = transform.parent.position + offset;
    }
}
