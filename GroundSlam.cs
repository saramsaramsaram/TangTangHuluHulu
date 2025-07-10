using System;
using UnityEngine;

public class GroundSlam : MonoBehaviour
{
    void Update()
    {
        if (transform.parent == GameManager.singleton.projFolder)
        {
            Invoke("DestroyEffect", 1f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            PlayerStatHandler player = other.GetComponent<PlayerStatHandler>();
            player.TakeDamage(10);
        }
    }

    void DestroyEffect()
    {
        Destroy(gameObject);
    }
}
