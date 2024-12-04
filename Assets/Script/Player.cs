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
    [SerializeField] private GameObject bulletPrefab; // Prefab de la bala
    [SerializeField] private Transform bulletSpawnPoint; // Punto desde donde se dispara
    [SerializeField] private float bulletSpeed = 20f; // Velocidad de la bala
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

        DontDestroyOnLoad(gameObject);
        rb = GetComponent<Rigidbody>();

        // Obtener la referencia al GameControllers en la escena
        gameController = FindObjectOfType<GameControllers>();
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
        if (Input.GetMouseButtonDown(0)) // Clic izquierdo para disparar
        {
            Shoot();
        }

        // Verificar la condición de victoria
        CheckWinCondition();
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
        GameObject bullet = PhotonNetwork.Instantiate(bulletPrefab.name, bulletSpawnPoint.position, Quaternion.identity);
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        bulletRb.velocity = transform.forward * bulletSpeed;
    }

    private void CheckWinCondition()
    {
        if (gameController != null && gameController.IsStructureAlive()) // Llamada correcta a la función
        {
            photonView.RPC("LoadVictoryScene", RpcTarget.All);
        }
    }

    [PunRPC]
    private void LoadVictoryScene()
    {
        PhotonNetwork.LoadLevel("VictoryScene");
    }
}
