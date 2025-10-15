using UnityEngine;

public class cheesesound : MonoBehaviour
{
    public AudioSource audioSource; // Assign your AudioSource in the Inspector


    void OnTriggerEnter(Collider collision) // Use OnCollisionEnter2D for 2D
    {
        // Check if the colliding object has the specified tag
        if (collision.gameObject.CompareTag("Player"))
        {
            audioSource.Play();
        }
    }
}
