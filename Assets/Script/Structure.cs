using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : MonoBehaviour
{
    [SerializeField] private int maxHealth = 25;
    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log($"La estructura recibió daño. Vida restante: {currentHealth}");

        if (currentHealth <= 0)
        {
            Debug.Log("La estructura ha sido destruida. Fin del juego.");
            // Aquí puedes cargar una escena de derrota o mostrar un mensaje
        }
    }
}
