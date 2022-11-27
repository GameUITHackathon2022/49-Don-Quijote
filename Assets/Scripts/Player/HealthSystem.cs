using LittleFoxLite;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LittleFoxLite
{
    public delegate void HealthAction();
    public delegate void UIHealthChange(float value);

[Serializable]
    public class HealthSystem<T>
    {
       
        public HealthAction takeDamageAction;
        public HealthAction dieAction;
        public UIHealthChange changeUIHealth;
        int health;
        int maxHealth;
        T objectPrefer;
        public int Healt
        {
            get=>health;
            private set => health = value;
        }
        public HealthSystem(T prefer, int MaxHealth)
        {
            objectPrefer = prefer;
            maxHealth = MaxHealth;
        }
        
        public void TakeDamage(int damage)
        {
            health -= damage;
            changeUIHealth?.Invoke(health / maxHealth);
            if (health<=0)
            {
                Die();
                takeDamageAction?.Invoke();
            }

        }
        private  void Die()
        {
            dieAction?.Invoke();
        }
    }
}