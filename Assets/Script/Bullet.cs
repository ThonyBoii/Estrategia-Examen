using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviourPun
{
    [SerializeField] private float bulletSpeed = 20f;
    [SerializeField] private int damageToEnemy = 1;

    private Rigidbody rb;

    private void Awake()
    {
        Destroy(gameObject, 3f);
        rb = GetComponent<Rigidbody>();
    }

    public void SetUp(Vector3 direction)
    {
        if (rb != null)
        {
            rb.velocity = direction.normalized * bulletSpeed;
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
