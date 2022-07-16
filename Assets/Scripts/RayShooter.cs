using UnityEngine;

/// <summary>
/// For debugging
/// </summary>
public static class RayShooter
{
    /// <summary>
    /// Get a raycast shot from the camera in the direction of the mouse position at the point this function is called.
    /// </summary>
    /// <returns>The raycast hit info</returns>
    public static RaycastHit ShootRayToMousePosition()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
        {
            return hit;
        }

        return default;
    }
}
