using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{

	public int startingHealth = 20;

	private int currentHealth;

	public GameObject deathEffect;

	public bool isInvincible = false;

	public HealthBarController healthBarController;

    private void Start()
    {
		this.currentHealth = startingHealth;
		this.healthBarController.SetMaxHealth(this.startingHealth);
    }

    public void TakeDamage(int damage)
	{
		if (isInvincible)
			return;

		this.currentHealth -= damage;

		this.healthBarController.SetHealth(this.currentHealth);

		if (this.currentHealth <= 10)
		{
			GetComponent<Animator>().SetBool("Stage2", true);
		}

		if (this.currentHealth <= 0)
		{
			Die();
		}
	}

	void Die()
	{
		Instantiate(deathEffect, transform.position, Quaternion.identity);
		Destroy(gameObject);
	}

}
