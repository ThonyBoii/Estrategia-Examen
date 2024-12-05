using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviourPun
{
    private int ownerId;
    private Rigidbody rb;
    [SerializeField] private float bulletSpeed = 20f; 
    [SerializeField] private int damageToEnemy = 1; 
    


    private void Awake()
    {
        
        Destroy(gameObject, 3f);
        rb = GetComponent<Rigidbody>();
    }

    public void SetUp(Vector3 direction, int ownerId)
    {
        this.ownerId = ownerId;

        if (rb != null)
        {
            rb.velocity = direction.normalized * bulletSpeed;
        }
    }

    private void Update()
    {
        if(!photonView.IsMine || !PhotonNetwork.IsConnected)
        {
            return;
        }
  
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) 
        {
            PhotonNetwork.Destroy(other.gameObject);
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
