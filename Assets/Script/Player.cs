using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;

public class Player : MonoBehaviourPun
{
    private static GameObject localInstance;

    [SerializeField] private TextMeshPro playerNameText;
    [SerializeField] private GameObject bulletPrefab; 
    [SerializeField] private Transform bulletSpawnPoint; 
    [SerializeField] private float bulletSpeed = 20f; 
    private Rigidbody rb;
    [SerializeField] private float speed;

    private GameControllers gameController;

    public static GameObject LocalInstance { get { return localInstance; } }

    private void Awake()
    {
        if (photonView.IsMine)
        {
            playerNameText.text = GameData.playerName;
            photonView.RPC("SetName", RpcTarget.AllBuffered, GameData.playerName);
            localInstance = gameObject;
        }
        else
        {
            GetComponent<Player>().enabled = false;
        }

        //DontDestroyOnLoad(gameObject);
        //rb = GetComponent<Rigidbody>();

        //// Obtener la referencia al GameControllers en la escena
        //gameController = FindObjectOfType<GameControllers>();
    }

    [PunRPC]
    private void SetName(string playerName)
    {
        playerNameText.text = GameData.playerName;
    }

    void Update()
    {
        if (!photonView.IsMine || !PhotonNetwork.IsConnected)
        {
            return;
        }
        Move();
        //if (Input.GetMouseButtonDown(0)) 
        //{
        //    Shoot();
        //}
        //CheckWinCondition();
    }

    void Move()
    {
        float rotationInput = Input.GetAxisRaw("Horizontal");
        float movementInput = Input.GetAxisRaw("Vertical");

        transform.Rotate(0, rotationInput * speed * Time.deltaTime, 0);

        Vector3 movement = transform.forward * movementInput * speed * Time.deltaTime;
        rb.MovePosition(rb.position + movement);
    }

    private void Shoot()
    {
        if (bulletPrefab == null || bulletSpawnPoint == null)
        {
            Debug.LogError("BulletPrefab o BulletSpawnPoint no están configurados.");
            return;
        }

        GameObject obj = PhotonNetwork.Instantiate(bulletPrefab.name, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        photonView.RPC("SetupBullet", RpcTarget.All, obj.GetComponent<PhotonView>().ViewID);

    }

    private void CheckWinCondition()
    {
        if (gameController != null && gameController.IsStructureAlive())
        {
            photonView.RPC("LoadVictoryScene", RpcTarget.All); // Usa RPC para cargar la victoria en todos los clientes
        }
    }

    [PunRPC]
    private void SetupBullet(int bulletViewID)
    {
        PhotonView bulletView = PhotonView.Find(bulletViewID);
        if (bulletView != null)
        {
            Bullet bulletScript = bulletView.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.SetUp(transform.forward, photonView.ViewID);
            }
        }
    }
}
