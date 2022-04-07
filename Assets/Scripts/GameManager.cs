using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public enum PrefabType { PLAYER, ITEM };

    public static GameManager gameManager;

    float enemySpawnCycle;

    float[] enemySpawnXpointRange;
    float enemySpawnYpoint;

    public GameObject[] enemys;
    public Transform playerTransform;
    public GameObject item;
    public GameObject calculateScreen;

    public Text scoreText;
    public static int Score { get; set; }

    public delegate void GameArrangementFunction();
    public static event GameArrangementFunction GameArrangement;

    void Awake ()
    {
        gameManager = this;
    }

	// Use this for initialization
	void Start () {
        enemySpawnXpointRange = new float[2] { -2.5f, 2.5f };
        enemySpawnYpoint = 5.5f;

        calculateScreen.SetActive(false);

        Invoke("EnemySpawn", 1f);
	}
	
	// Update is called once per frame
	void Update () {
        
    }

    void EnemySpawn()
    {   
        Instantiate(enemys[Random.Range(0, enemys.Length)], new Vector2(Random.Range(enemySpawnXpointRange[0], enemySpawnXpointRange[1]), enemySpawnYpoint), Quaternion.identity);

        if (Score > 1000)
            enemySpawnCycle = Random.Range(1f, 1.5f);
        else if (Score > 500)
            enemySpawnCycle = Random.Range(1.5f, 2f);
        else
            enemySpawnCycle = Random.Range(2f, 2.5f);

        Invoke("EnemySpawn", enemySpawnCycle);
    }    

    public void Calculate()
    {
        calculateScreen.SetActive(true);

        scoreText.text += Score;

        CancelInvoke();

        GameArrangement();
    }

    public static GameObject GetPrefab(PrefabType type)
    {
        if (type == PrefabType.ITEM)
            return gameManager.item;
        else if (type == PrefabType.PLAYER)
            return gameManager.playerTransform.gameObject;

        return null;
    }
}
