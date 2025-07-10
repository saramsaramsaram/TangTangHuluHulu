using System;
using UnityEngine;

public class EnergyBall : MonoBehaviour
{
    
    private void Update()
    {
        if (transform.parent == GameManager.singleton.projFolder.transform)
        {
            Invoke("DestroyEffect", 5f);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStatHandler player = other.GetComponent<PlayerStatHandler>();
            player.TakeDamage(10);
            DestroyEffect();
        }
    }

    private void DestroyEffect()
    {
        Destroy(gameObject);
    }
}
