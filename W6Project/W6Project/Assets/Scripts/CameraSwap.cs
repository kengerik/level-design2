using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public GameObject cameraOne; // Assign your first camera here in the Inspector
    public GameObject cameraTwo; // Assign your second camera here in the Inspector

    void Start()
    {
        // Ensure only one camera is active at the start
        cameraOne.SetActive(true);
        cameraTwo.SetActive(false);
    }

    void Update()
    {
        // Example: Switch cameras on a key press (e.g., 'C' key)
        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleCameras();
        }
    }

    public void ToggleCameras()
    {
        // Toggle the active state of both cameras
        cameraOne.SetActive(!cameraOne.activeSelf);
        cameraTwo.SetActive(!cameraTwo.activeSelf);
    }
}
