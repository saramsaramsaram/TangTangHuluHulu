using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerStatHandler : MonoBehaviour // PlayerStat -> PlayerStatHandler
{
    public enum PlayerStat
    {
        Health,
        Damage,
        Range,
        Speed
    }

    public float maxHealth = 100f;
    public float baseDamage = 25f;
    public float baseRange = 2f;
    public float moveSpeed = 5.0f;
    public float currentHp;
    public void Awake()
    {
        currentHp = maxHealth;
    }
    
    public void IncreaseStat(PlayerStat stat, float value)
    {
        switch (stat)
        {
            case PlayerStat.Health:
                maxHealth += value;
                currentHp += maxHealth / maxHealth;
                break;
            case PlayerStat.Damage:
                baseDamage += value;
                break;
            case PlayerStat.Range:
                baseRange += value;
                break;
            case PlayerStat.Speed:
                moveSpeed += moveSpeed * 0.5f;
                break;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHp -= damage;

        if (currentHp <= 0)
        {
            Die();
        }
    }

    public void Heal()
    {
        currentHp += maxHealth * 0.3f;
    }
    public void Die()
    {
        Time.timeScale = 0;
        Destroy(gameObject, 5f);
    }
    
    
}
