using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviourPun
{
    // Atributos 
    [SerializeField] private float moveSpeed = 3f; 
    [SerializeField] private int damageToStructure = 1; 

    private Transform target;

    private void Update()
    {
        if (!photonView.IsMine) 
        {
            return;
        }
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    // Método 
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Structure"))
        {
            GameControllers gameController = FindObjectOfType<GameControllers>();
            if (gameController != null)
            {
                gameController.DamageStructure(damageToStructure);
            }
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
