using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    #region Health Settings

    [Header("Health Settings")]
    [Tooltip("The var to check the health if it isn't 0")]
    private int m_zeroHealth = 0;

    [Tooltip("The current health of the player")]
    public int CurrentHealth;

    #endregion

    public void ModifyHealth()
    {
        // Takes of the damage from the current health
        CurrentHealth--;

        // If the current health is equal or lower then zero health
        if (CurrentHealth <= m_zeroHealth)
        {
            //Plays sound when the player dies
            SoundManager.Instance.Play("CrockDeath");

            // Reloads the game scene to retry it
            GameManager.Instance.GameOver();
        }
    }

}
