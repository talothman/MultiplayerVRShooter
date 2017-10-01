using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EnemySpawner : NetworkBehaviour {

    public GameObject enemyPrefab;
    public int numEnemies;

    float randomTimeToPass;
    float cycleTime = 0;

    Vector3 beginningPos;
    int beginningNumEnemies;
    public override void OnStartServer()
    {
        beginningPos = transform.position;
        beginningNumEnemies = numEnemies;
        SetUp();
    }

    void SetUp()
    {
        transform.position = beginningPos;
        numEnemies = beginningNumEnemies;

        for (int i = 0; i < numEnemies; i++)
        {
            var pos = new Vector3(
                Random.Range(-5.0f, 5.0f),
                Random.Range(0.5f, 2.0f),
                Random.Range(0, 5.0f)
                );

            var rotation = Quaternion.Euler(Random.Range(0, 180), Random.Range(0, 180), Random.Range(0, 180));

            var enemy = (GameObject)Instantiate(enemyPrefab, pos, rotation);
            enemy.transform.parent = transform;
            NetworkServer.Spawn(enemy);
        }
    }

    private void Start()
    {
        randomTimeToPass = Random.Range(0f, 3f);
    }

    private void Update()
    {
        if(cycleTime > randomTimeToPass)
        {
            cycleTime = 0;
            transform.position += (-Vector3.forward * 0.2f);
            randomTimeToPass = Random.Range(0f, 3f);
        }

        cycleTime += Time.deltaTime;
    }

    public void CheckState()
    {
        if(numEnemies == 0)
        {
            SetUp();
        }
    }
}
