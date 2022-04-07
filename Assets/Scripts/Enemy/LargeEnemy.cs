using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeEnemy : Enemy {

	// Use this for initialization
	override protected void Awake() {
        base.Awake();
        velocity = Random.Range(-1.5f, -2f);

        durability = 20;

        fireCycle = 2.2f;
        muzzleDistance.Add(new Vector2(-0.2f, -0.5f));
        muzzleDistance.Add(new Vector2(0.2f, -0.5f));
        bulletKind = BulletManager.BulletKind.ENEMY_SMALL;
        bulletVelocity = 3f;

        score = 50;
    }
}
