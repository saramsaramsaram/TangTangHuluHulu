using System.Security.Cryptography;
using UnityEngine;

public class Boss1 : MonoBehaviour
{
    [SerializeField] private CharacterController cc;
    private float MoveSpeed = 8f;
    private int health = 400;
    [SerializeField] private GameObject GroundSlamEffect;
    [SerializeField] private GameObject EnergyBall;
    private float energyBallForce = 5f;
    [SerializeField] private Animator animator;
    private bool attacking = false;
    private float Skill1Cooldown = 5f;
    private float Skill2Cooldown = 10f;
    private float Skill3Cooldown = 15f;
    private float lastSkill1;
    private float lastSkill2;
    private float lastSkill3;
    
    void start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!attacking)
        {
            Vector3 MoveDir = GameManager.singleton.player.transform.position - transform.position;
            transform.LookAt(GameManager.singleton.player.transform);
            if (MoveDir.magnitude > 5f)
            {
                MoveDir.Normalize();
                cc.Move(MoveDir * (Time.deltaTime * MoveSpeed));
                animator.SetBool("isMoving", true);
            }
            else
            {
                if (Time.time - lastSkill2 >= Skill2Cooldown)
                {
                    lastSkill2 = Time.time;
                    cc.Move(Vector3.zero);
                    animator.SetBool("isMoving", false);
                    attacking = true;
                    Invoke("EnergyBallEffect", 2f);
                }
            }
            
            if (Time.time - lastSkill1 >= Skill1Cooldown)
            {
                lastSkill1 = Time.time;
                cc.Move(Vector3.zero);
                
                attacking = true;
                animator.SetBool("isMoving", false);
                Invoke("GroundSlam", 0.9f);   
            }
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Debug.Log("으아아악");
            Destroy(gameObject);
        }
    }
    
    private void GroundSlam()
    {
        animator.SetTrigger("GroundSlam");
        Invoke("SpawnGroundSlamEffect", 1f);
    }

    private void SpawnGroundSlamEffect()
    {
        GameObject gs = Instantiate(GroundSlamEffect, transform.position, Quaternion.identity);
        gs.transform.parent = GameManager.singleton.projFolder.transform;
        FinishAttacking();
    }
    
    private void EnergyBallEffect()
    {
        animator.SetTrigger("EnergyBall");
        GameObject eb = Instantiate(EnergyBall, transform.position, Quaternion.identity);
        eb.transform.parent = GameManager.singleton.projFolder.transform;
        Rigidbody ebrb = eb.GetComponent<Rigidbody>();
        ebrb.AddForce(GameManager.singleton.player.transform.position *  energyBallForce, ForceMode.Impulse);
        Invoke("FinishAttacking", 1f);
    }



    private void FinishAttacking()
    {
        attacking = false;
    }
    
}
