using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class VRNetworkManager : NetworkManager{

    NetworkStateContainer networkStateContainer;
    List<GameObject> listOfPlayerGOs;
    List<Transform> sPosition;

    public EnemySpawner enemySpawner;

    private void Start()
    {
        networkStateContainer = GetComponent<NetworkStateContainer>();
        listOfPlayerGOs = new List<GameObject>();
        sPosition = startPositions;
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        
        var player = (GameObject)GameObject.Instantiate(playerPrefab, sPosition[networkStateContainer.currentNumberOfPlayers].position, Quaternion.identity);
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
        listOfPlayerGOs.Add(player);

        if ((++networkStateContainer.currentNumberOfPlayers) == 2)
        {
            networkStateContainer.twoPlayers = true;
            foreach(GameObject go in listOfPlayerGOs)
            {
                go.GetComponent<ShootingPlayer>().RpcUpdateToReadyState();
            }
        }

        player.GetComponent<ShootingPlayer>().playerIndex = networkStateContainer.currentNumberOfPlayers;  
    }

    public void CheckReadyState()
    {
        if(networkStateContainer.readyPlayer1 && networkStateContainer.readyPlayer2)
        {
            foreach (GameObject go in listOfPlayerGOs)
            {
                go.GetComponent<ShootingPlayer>().RpcUpdateToStartState();
                enemySpawner.beginMovement = true;
            }
        }
    }
}
