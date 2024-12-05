using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameControllers : MonoBehaviourPunCallbacks
{

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform spawner;
    // Atributos 
    [SerializeField] private int structureHealth = 15;
    [SerializeField] private Text timerText;
    [SerializeField] private Text structureHealthText;
    [SerializeField] private float gameDuration = 30f;
    //Variables
    private float timer;
    private bool gameOver;

    void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady && Player.LocalInstance == null)
        {
            PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0, 10, 0), Quaternion.identity);
        }

        timer = gameDuration;
        UpdateStructureHealthUI();
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(SpawnEnemies());
        }
    }

    void Update()
    {
        if (gameOver)
        {
            return;
        }

        if (PhotonNetwork.IsMasterClient)
        {
            timer -= Time.deltaTime;
            photonView.RPC("UpdateTimerUI", RpcTarget.All, timer);
            if (timer <= 0)
            {
                CheckGameOver();
            }
        }

    }

    public override void OnJoinedRoom()
    {
        if (Player.LocalInstance == null)
        {
            PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0, 10, 0), Quaternion.identity);
        }
    }

    public bool IsStructureAlive()
    {
        return structureHealth > 0;
    }

    private void UpdateTimer()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            timer = 0;
            CheckGameOver();
        }
    }
    [PunRPC]
    private void UpdateTimerUI(float currentTime)
    {
        timerText.text = "Time: " + Mathf.Ceil(currentTime).ToString();
    }

    private IEnumerator SpawnEnemies()
    {
        while (!gameOver)
        {
            yield return new WaitForSeconds(3f); // Spawnea cada 3 segundos

            // Instancia un enemigo desde la posición del spawner
            GameObject enemyObject = PhotonNetwork.Instantiate(enemyPrefab.name, spawner.position, Quaternion.identity);
            Enemy enemyScript = enemyObject.GetComponent<Enemy>();

            if (enemyScript != null)
            {
                // Asigna el objetivo (estructura)
                GameObject structure = GameObject.FindWithTag("Structure");
                if (structure != null)
                {
                    enemyScript.SetTarget(structure.transform);
                }
            }
        }
    }


    public void DamageStructure(int damage = 0)
    {
        photonView.RPC("ApplyDamage", RpcTarget.All, damage);
    }

    [PunRPC]
    private void ApplyDamage(int damage)
    {
        structureHealth -= damage;
        UpdateStructureHealthUI();

        if (structureHealth <= 0)
        {
            CheckGameOver();
        }
    }

    private void UpdateStructureHealthUI()
    {
        structureHealthText.text = "Structure: " + structureHealth;
    }

    private void CheckGameOver()
    {
        gameOver = true;

        if (structureHealth > 0 && timer <= 0)
        {
            photonView.RPC("LoadScene", RpcTarget.All, "Victory");
        }
        else
        {
            photonView.RPC("LoadScene", RpcTarget.All, "GameOver");
        }
    }

    [PunRPC]
    private void LoadScene(string sceneName)
    {
        PhotonNetwork.LoadLevel(sceneName);
    }

}