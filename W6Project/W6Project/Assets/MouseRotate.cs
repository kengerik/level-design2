using UnityEngine;

public class PlayerRotationController : MonoBehaviour
{
    public Transform cameraTransform;

    void LateUpdate()
    {
        if (cameraTransform != null)
        {
            // Get the camera's Y-axis rotation
            Quaternion cameraYaw = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);

            // Apply this rotation to the player
            transform.rotation = cameraYaw;
        }
    }
}
