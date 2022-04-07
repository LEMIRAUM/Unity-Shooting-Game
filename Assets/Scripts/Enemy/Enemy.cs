using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    
    Rigidbody2D enemyRB;
    SpriteRenderer enemySR;
    protected float velocity;

    public Transform mask;
    float maskSize;
    protected int durability;
    int currentDurability;

    Rigidbody2D playerRB;
    protected float fireCycle;
    protected List<Vector2> muzzleDistance;
    protected BulletManager.BulletKind bulletKind;
    protected float bulletVelocity;

    protected int score;

    static int itemSpawnWeight;

	// Use this for initialization
	virtual protected void Awake () {        
        enemyRB = GetComponent<Rigidbody2D>();
        enemySR = GetComponent<SpriteRenderer>();

        maskSize = mask.localScale.x;

        muzzleDistance = new List<Vector2>();

        itemSpawnWeight = 10;
    }
	
    protected void Start()
    {
        currentDurability = durability;

        playerRB = GameManager.GetPrefab(GameManager.PrefabType.PLAYER).GetComponent<Rigidbody2D>();

        InvokeRepeating("Fire", 0.2f, fireCycle);

        Player.DurabilityModify += ItemSpawnModify;
        GameManager.GameArrangement += ArrangeMent;
    }

	// Update is called once per frame
	protected void Update () {
        Move();
	}

    protected void OnDestroy()
    {
        Player.DurabilityModify -= ItemSpawnModify;
        GameManager.GameArrangement -= ArrangeMent;
    }

    void Move()
    {
        enemyRB.velocity = new Vector2(0, velocity);
    }

    void Fire()
    {
        for(int i = 0; i < muzzleDistance.Count; i++)
        {
            GameObject bullet = BulletManager.RentalBullet(bulletKind);

            bullet.transform.position = enemyRB.position + muzzleDistance[i];
            bullet.GetComponent<Rigidbody2D>().velocity = (enemyRB.position - playerRB.position).normalized * -bulletVelocity;
        }
    }

    void Damaged(int damage)
    {
        currentDurability -= damage;
        enemySR.color = new Color(1, 1, 1, 0.5f);

        mask.localScale = new Vector3(maskSize * currentDurability / durability, 1, 1);

        if (currentDurability <= 0)
            Eliminated();

        Invoke("Restore", 0.1f);
    }

    void Restore()
    {
        enemySR.color = new Color(1, 1, 1, 1);
    }

    void Eliminated()
    {
        if (Random.Range(0, itemSpawnWeight) == 0)
            Instantiate(GameManager.GetPrefab(GameManager.PrefabType.ITEM), enemyRB.position, Quaternion.identity);

        ArrangeMent();

        GameManager.Score += score;
    }

    void ArrangeMent()
    {
        CancelInvoke("Restore");

        Destroy(gameObject);
    }

    void ItemSpawnModify(int remainPercent)
    {
        if (remainPercent <= 10)
            itemSpawnWeight = 3;
        else if (remainPercent <= 50)
            itemSpawnWeight = 5;
        else
            itemSpawnWeight = 10;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Border") && enemyRB.position.y < 0)
            enemyRB.position = new Vector2(enemyRB.position.x, 5.5f);
        else if (collision.gameObject.tag.Equals("PlayerBullet"))
        {
            Damaged(collision.gameObject.GetComponent<Bullet>().damage);
            BulletManager.ReturnBullet(collision.gameObject.GetComponent<Bullet>().bulletKind, collision.gameObject);
        }
    }
}
