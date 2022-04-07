using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    
    public BulletManager.BulletKind bulletKind;
    public int damage;

    void Awake()
    {
        GameManager.GameArrangement += BulletReturn;
    }

    void OnDestroy()
    {
        GameManager.GameArrangement -= BulletReturn;
    }

    void BulletReturn()
    {
        BulletManager.ReturnBullet(bulletKind, gameObject);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Border"))
            BulletManager.ReturnBullet(bulletKind, gameObject);
    }
}
