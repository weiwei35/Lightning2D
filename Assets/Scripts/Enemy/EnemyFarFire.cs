using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFarFire : MonoBehaviour
{
    public EnemyFarMove enemyFarMove;
    public EnemyRandomFire enemyRandomFire;
    // List<GameObject> bulletList = new List<GameObject>();
    public void Fire() {
        var curBullet = GameObjectPoolTool.GetFromPoolForce(true,"Assets/Resources/Bullet.prefab");
        // var curBullet = Instantiate(bullet);
        curBullet.transform.position = enemyFarMove.bulletPos;
        Rigidbody bulletRb = curBullet.GetComponent<Rigidbody>();
        BulletController bulletController = curBullet.GetComponent<BulletController>();
        bulletController.hurt = enemyFarMove.bulletHurt;
        Vector3 direction = enemyFarMove.target.position - transform.position;
        direction.Normalize();
        bulletRb.velocity = direction * enemyFarMove.bulletSpeed;
        bulletController.bulletSpeed = enemyFarMove.bulletSpeed;
        bulletController.direction = direction;
        bulletController.center = enemyFarMove.center.transform.position;
        bulletController.length = 15;
        enemyFarMove.bulletList.Add(curBullet);
    }

    public void RandomFire() {
        var curBullet = GameObjectPoolTool.GetFromPoolForce(true,"Assets/Resources/Bullet.prefab");
        // var curBullet = Instantiate(bullet);
        curBullet.transform.position = enemyRandomFire.bulletPos;
        Rigidbody bulletRb = curBullet.GetComponent<Rigidbody>();
        BulletController bulletController = curBullet.GetComponent<BulletController>();
        bulletController.hurt = enemyRandomFire.bulletHurt;
        Vector3 direction = enemyRandomFire.target.position - transform.position;
        direction.Normalize();
        bulletRb.velocity = direction * enemyRandomFire.bulletSpeed;
        bulletController.bulletSpeed = enemyRandomFire.bulletSpeed;
        bulletController.direction = direction;
        bulletController.center = enemyRandomFire.center.transform.position;
        bulletController.length = 15;
        enemyRandomFire.bulletList.Add(curBullet);
    }
}
