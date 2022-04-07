using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    Rigidbody2D playerRB;
    SpriteRenderer playerSR;
    public GameManager gameManager;

    public Transform HPmask;
    float maskSize;
    int durability;
    int currentDurability;
    bool immune;

    float targetVelocity;
    Vector3 playerVelocity;
    public Sprite[] playerSprites;

    float bulletVelocity;
    int reloadTime, currentReloadTime;
    bool bulletUpgrade;

    public GameObject[] itemIcon;
    int itemCount;
    int maxItemCount;

    public delegate void DurabilityModifyFunction(int remainPercent);
    public static event DurabilityModifyFunction DurabilityModify;

    void Awake()
    {
        playerRB = GetComponent<Rigidbody2D>();
        playerSR = GetComponent<SpriteRenderer>();

        maskSize = HPmask.localScale.x;
        durability = currentDurability = 100;
        immune = false;

        playerVelocity = new Vector3(0, 0, 0);

        bulletVelocity = 5f;
        reloadTime = currentReloadTime = 15;

        itemCount = 0;
        maxItemCount = 3;
    }
	
	// Update is called once per frame
	void Update () {
        Move();

        Fire();
        Reload();

        Repair();
    }

    void Move()
    {
        if (Input.GetKey(KeyCode.LeftShift)) targetVelocity = 5f;
        else targetVelocity = 3f;

        if (Input.GetKey(KeyCode.LeftArrow)) {
            playerVelocity.x = -targetVelocity;
            playerSR.sprite = playerSprites[1];

        }
        else if (Input.GetKey(KeyCode.RightArrow)) {
            playerVelocity.x = targetVelocity;
            playerSR.sprite = playerSprites[2];
        }
        else {
            playerVelocity.x = 0;
            playerSR.sprite = playerSprites[0];
        }

        if (Input.GetKey(KeyCode.DownArrow)) playerVelocity.y = -targetVelocity;
        else if (Input.GetKey(KeyCode.UpArrow)) playerVelocity.y = targetVelocity;
        else playerVelocity.y = 0;

        playerRB.velocity = playerVelocity;
    }

    void Fire()
    {
        if(Input.GetKey(KeyCode.Space) && currentReloadTime >= reloadTime)
        {
            GameObject bullet;

            if(bulletUpgrade == false)
                bullet = BulletManager.RentalBullet(BulletManager.BulletKind.PLAYER_NORMAL);
            else
                bullet = BulletManager.RentalBullet(BulletManager.BulletKind.PLAYER_UPGRADE);

            bullet.transform.position = playerRB.position + new Vector2(0, 0.5f);
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector3(0, bulletVelocity, 0);

            currentReloadTime = 0;
        }
    }

    void Reload()
    {
        if (currentReloadTime < reloadTime)
            currentReloadTime++;
    }

    void Repair()
    {
        if (Input.GetKeyDown(KeyCode.P) && itemCount >= maxItemCount)
        {
            currentDurability = currentDurability <= durability / 2 ? currentDurability + durability / 2 : durability;
            HPmask.localScale = new Vector3(maskSize * currentDurability / durability, 1, 1);
            bulletUpgrade = true;
            try {
                DurabilityModify(currentDurability * 100 / durability);
            }
            catch(System.Exception) {
                DurabilityModify = null;
            }
            for(int i = 0; i < maxItemCount; i++)
                itemIcon[i].SetActive(false);
            itemCount = 0;
        }            
    }

    void Damaged(int damage)
    {
        if (immune)
            return;

        currentDurability -= damage;
        HPmask.localScale = new Vector3(maskSize * currentDurability / durability, 1, 1);
        playerSR.color = new Color(1, 1, 1, 0.5f);
        bulletUpgrade = false;
        immune = true;

        try {
            DurabilityModify(currentDurability * 100 / durability);
        }
        catch (System.Exception) {
            DurabilityModify = null;
        }

        if (currentDurability < 0)
        {
            HPmask.localScale = new Vector3(0, 1, 1);
            gameManager.Calculate();
            gameObject.SetActive(false);
        }

        Invoke("Restore", 0.3f);
    }

    void Restore()
    {
        playerSR.color = new Color(1, 1, 1, 1);
        immune = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("EnemyBullet"))
        {
            Damaged(collision.gameObject.GetComponent<Bullet>().damage);
            BulletManager.ReturnBullet(collision.gameObject.GetComponent<Bullet>().bulletKind, collision.gameObject);
        }
        else if (collision.gameObject.tag.Equals("Item"))
        {
            bulletUpgrade = true;
            Destroy(collision.gameObject);
            
            itemCount = itemCount >= maxItemCount ? maxItemCount : itemCount + 1;
            itemIcon[itemCount - 1].SetActive(true);
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Enemy"))
            Damaged(10);
    }
}
