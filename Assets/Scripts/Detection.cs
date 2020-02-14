using UnityEngine;

public class Detection : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wand"))
        {
            var wand = other.GetComponent<Wand>();
            wand.Danger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Wand"))
        {
            var wand = other.GetComponent<Wand>();
            wand.Danger = false;
        }
    }
}