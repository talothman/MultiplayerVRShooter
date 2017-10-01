using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour {

    private void OnCollisionEnter(Collision collision)
    {
        var hit = collision.gameObject;
        var hitPlayer = hit.GetComponent<Enemy>();
        if (hitPlayer != null)
        {
            hitPlayer.GetComponentInParent<EnemySpawner>().numEnemies--;
            hitPlayer.GetComponentInParent<EnemySpawner>().CheckState();
            Destroy(hitPlayer.gameObject);
            Destroy(gameObject);
        }
    }
}
