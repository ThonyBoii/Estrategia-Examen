using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviourPun
{
    private int ownerId;
    private Rigidbody rb;
    [SerializeField] private float speed;
    private Vector3 direction;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SetUp(Vector3 direction, int ownerId)
    {
        this.direction = direction;
        this.ownerId = ownerId;
    }

    private void Update()
    {
        if(!photonView.IsMine || !PhotonNetwork.IsConnected)
        {
            return;
        }
        rb.velocity = direction.normalized * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy1"))
        {
            PhotonNetwork.Destroy(other.gameObject); 
        }
        else
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
