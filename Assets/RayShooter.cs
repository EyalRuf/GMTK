using UnityEngine;

/// <summary>
/// For debugging
/// </summary>
public class RayShooter : MonoBehaviour
{
    private Camera cam;
    private void Start() => cam = Camera.main;

    /// <summary>
    /// Get a raycast shot from the camera in the direction of the mouse position at the point this function is called.
    /// </summary>
    /// <returns>The raycast hit info</returns>
    public RaycastHit ShootRayToMousePosition()
    {
        if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
        {
            return hit;
        }

        return default;
    }
}
