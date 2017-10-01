using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ShootingPlayer : NetworkBehaviour {
    [SyncVar]
    public int playerIndex;
    public GameObject bulletPrefab;
    public Transform bulletOrigin;

    public Text infoText;
    public bool canShoot = false;

    public AudioSource music;
    public AudioSource gunShot;

    public override void OnStartLocalPlayer()
    {
        music.Play();
        music.volume = 0.5f;
    }

    void Update () {

        if (!isLocalPlayer)
            return;

        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) && canShoot)
        {
            CmdShootBullet();
        }

        if (OVRInput.GetDown(OVRInput.RawButton.X) && !canShoot)
        {
            CmdSetRead();
            gunShot.Play();
        }
	}

    [Command]
    public void CmdShootBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletOrigin.position, bulletOrigin.rotation);
        bullet.GetComponent<Rigidbody>().velocity = bulletOrigin.forward * 25;

        NetworkServer.Spawn(bullet);

        Destroy(bullet, 5f);
    }

    [ClientRpc]
    public void RpcUpdateToReadyState()
    {
        if (!isLocalPlayer)
            return;

        infoText.text = "Press X to Mark as ready!";
    }

    [Command]
    public void CmdSetRead()
    {
        GameObject netManGameObj = GameObject.FindGameObjectWithTag("NetMan");

        if (playerIndex == 1)
            netManGameObj.GetComponent<NetworkStateContainer>().readyPlayer1 = true;
        else if(playerIndex == 2)
            netManGameObj.GetComponent<NetworkStateContainer>().readyPlayer2 = true;

        netManGameObj.GetComponent<VRNetworkManager>().CheckReadyState();
    }

    [ClientRpc]
    public void RpcUpdateToStartState()
    {
        StartCoroutine(InfoPanelDelay());
    }

    IEnumerator InfoPanelDelay()
    {
        infoText.text = "Ready";
        yield return new WaitForSeconds(2f);
        infoText.text = "Set...";
        yield return new WaitForSeconds(2f);
        infoText.text = "Go!";
        yield return new WaitForSeconds(2f);

        canShoot = true;

        infoText.transform.parent.gameObject.SetActive(false);
    }
}
