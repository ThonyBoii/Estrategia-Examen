using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class Player : MonoBehaviourPun
{
    private static GameObject localInstance;

    [SerializeField] private TextMeshPro playerNameText;
    [SerializeField] private GameObject bulletPrefab;

    private Rigidbody rb;
    [SerializeField] private float speed;

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
        Shoot();
    }

    void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        rb.velocity = new Vector3(horizontal * speed, rb.velocity.y, vertical * speed);

        if (horizontal != 0 || vertical != 0)
        {
            transform.forward = new Vector3(horizontal, 0, vertical);
        }

    }

    void Shoot()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            GameObject obj = PhotonNetwork.Instantiate(bulletPrefab.name, transform.position, Quaternion.identity);
            obj.GetComponent<Bullet>().SetUp(transform.forward, photonView.ViewID);
        }
    }

    
}
