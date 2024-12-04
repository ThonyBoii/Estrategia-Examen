using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameControllers : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject enemyPrefab; // Prefab del enemigo
    [SerializeField] private Transform spawner; // Objeto vacío que será el spawner móvil
    [SerializeField] private float spawnerSpeed = 2f; // Velocidad del movimiento del spawner
    [SerializeField] private float spawnerLeftLimit = -4f; // Límite izquierdo del spawner
    [SerializeField] private float spawnerRightLimit = 4f; // Límite derecho del spawner
    [SerializeField] private int structureHealth = 15; // Vida inicial de la estructura
    [SerializeField] private Text timerText; // Texto del temporizador
    [SerializeField] private Text structureHealthText; // Texto de la vida de la estructura
    [SerializeField] private float gameDuration = 30f; // Duración del juego en segundos

    private float timer;
    private bool gameOver;
    private bool movingRight = true; // Dirección del movimiento del spawner

    void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady && Player.LocalInstance == null)
        {
            PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0, 10, 0), Quaternion.identity);
        }

        timer = gameDuration;
        UpdateStructureHealthUI();
        StartCoroutine(SpawnEnemies());
    }

    void Update()
    {
        if (gameOver) return;

        UpdateTimer();
        MoveSpawner(); // Mover el spawner de izquierda a derecha
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

        timerText.text = "Time: " + Mathf.Ceil(timer).ToString();
    }

    private IEnumerator SpawnEnemies()
    {
        while (!gameOver)
        {
            yield return new WaitForSeconds(3f); // Spawnea cada 3 segundos

            // Instancia un enemigo desde la posición del spawner
            PhotonNetwork.Instantiate(enemyPrefab.name, spawner.position, Quaternion.identity);
        }
    }

    private void MoveSpawner()
    {
        // Movimiento del spawner entre los límites
        if (movingRight)
        {
            spawner.position += Vector3.right * spawnerSpeed * Time.deltaTime;
            if (spawner.position.x >= spawnerRightLimit)
            {
                movingRight = false; // Cambia la dirección
            }
        }
        else
        {
            spawner.position += Vector3.left * spawnerSpeed * Time.deltaTime;
            if (spawner.position.x <= spawnerLeftLimit)
            {
                movingRight = true; // Cambia la dirección
            }
        }
    }

    public void DamageStructure(int damage)
    {
        structureHealth -= damage;
        UpdateStructureHealthUI();

        if (structureHealth <= 0)
        {
            structureHealth = 0;
            CheckGameOver();
        }
    }

    private void UpdateStructureHealthUI()
    {
        structureHealthText.text = "Structure Health: " + structureHealth.ToString();
    }

    private void CheckGameOver()
    {
        gameOver = true;

        if (structureHealth > 0 && timer <= 0)
        {
            // Victoria
            PhotonNetwork.LoadLevel("VictoryScene");
        }
        else
        {
            // Derrota
            PhotonNetwork.LoadLevel("LoseScene");
        }
    }
}
