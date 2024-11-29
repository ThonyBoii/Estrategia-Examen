using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int health = 1;      
    [SerializeField] private int damage = 1;     

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Structure"))
        {
            Structure structure = other.GetComponent<Structure>();
            if (structure != null)
            {
                structure.TakeDamage(damage); 
            }
            PhotonNetwork.Destroy(gameObject); 
        }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            PhotonNetwork.Destroy(gameObject); 
        }
    }
}
