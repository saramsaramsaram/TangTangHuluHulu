using UnityEngine;

public class BreathDamage : MonoBehaviour
{
    public float damagePerSecond = 10f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("브레스 맞음");

        }
    }
}