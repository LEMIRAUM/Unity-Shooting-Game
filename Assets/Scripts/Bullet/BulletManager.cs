using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour {

    public enum BulletKind { PLAYER_NORMAL, PLAYER_UPGRADE, ENEMY_LARGE, ENEMY_SMALL };

    public static BulletManager bulletManager;

    public GameObject[] bulletPrefabs;
    Dictionary<BulletKind, Queue<GameObject>> bulletQueue;

    // Use this for initialization
    void Awake () {
        bulletManager = this;

        bulletQueue = new Dictionary<BulletKind, Queue<GameObject>>();

        InitQueue(50);
	}

    void InitQueue(int count)
    {
        bulletQueue.Add(BulletKind.PLAYER_NORMAL, new Queue<GameObject>());
        bulletQueue.Add(BulletKind.PLAYER_UPGRADE, new Queue<GameObject>());
        bulletQueue.Add(BulletKind.ENEMY_LARGE, new Queue<GameObject>());
        bulletQueue.Add(BulletKind.ENEMY_SMALL, new Queue<GameObject>());

        foreach (BulletKind bulletKind in bulletQueue.Keys)
        {
            for (int i = 0; i < count; i++)            
                bulletQueue[bulletKind].Enqueue(CreateBullet(bulletKind));            
        }        
    }

    GameObject CreateBullet(BulletKind bulletKind)
    {
        GameObject temp = Instantiate(bulletPrefabs[(int)bulletKind], transform);
        temp.SetActive(false);

        return temp;
    }

    public static GameObject RentalBullet(BulletKind bulletKind)
    {
        if (bulletManager.bulletQueue[bulletKind].Count > 0)
        {
            GameObject temp = bulletManager.bulletQueue[bulletKind].Dequeue();
            temp.SetActive(true);

            return temp;
        }
        else
        {
            GameObject temp = bulletManager.CreateBullet(bulletKind);
            temp.SetActive(true);

            return temp;
        }
    }

    public static void ReturnBullet(BulletKind bulletKind, GameObject bullet)
    {
        bullet.SetActive(false);
        bulletManager.bulletQueue[bulletKind].Enqueue(bullet);
    }
}
