using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviourPun
{
    [SerializeField] private float bulletSpeed = 20f; // Velocidad de la bala
    [SerializeField] private int damageToEnemy = 1; // Daño que causa al enemigo

    private void Start()
    {
        // Destruir la bala después de 3 segundos si no ha colisionado con nada
        Destroy(gameObject, 3f);
    }

    private void Update()
    {
        // Mover la bala hacia adelante
        transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);
    }

    // Detectar colisión con el enemigo
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) // Suponiendo que el enemigo tiene el tag "Enemy"
        {
            // Destruir al enemigo
            PhotonNetwork.Destroy(other.gameObject);

            // Destruir la bala
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
