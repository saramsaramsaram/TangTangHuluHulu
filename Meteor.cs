using UnityEngine;

public class Meteor : MonoBehaviour
{
    public float damage = 20f;
    //public GameObject explosionEffect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("플레이어가 메테오 맞음! 데미지 적용");
        }
        
        //if (explosionEffect != null)
        //{
        //    Instantiate(explosionEffect, transform.position, Quaternion.identity);
        //}

        Destroy(gameObject); // 메테오 제거
    }
}