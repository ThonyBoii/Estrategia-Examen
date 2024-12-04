using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviourPun
{
    [SerializeField] private float moveSpeed = 3f; // Velocidad del enemigo
    [SerializeField] private int damageToStructure = 1; // Da�o que causa al tocar la estructura

    private void Update()
    {
        if (!photonView.IsMine) return; // Aseg�rate de que solo el enemigo controlado por Photon se mueva

        Move();
    }

    private void Move()
    {
        // Movimiento del enemigo de izquierda a derecha
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
    }

    // M�todo para hacer da�o a la estructura al colisionar con ella
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Structure")) // Aseg�rate de que la estructura tiene el tag "Structure"
        {
            GameControllers gameController = FindObjectOfType<GameControllers>();
            if (gameController != null)
            {
                gameController.DamageStructure(damageToStructure); // Llama al m�todo DamageStructure de GameControllers
            }

            // Destruir el enemigo despu�s de tocar la estructura
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
