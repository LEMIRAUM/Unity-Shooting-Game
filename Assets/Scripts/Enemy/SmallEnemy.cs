using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallEnemy : Enemy {

	// Use this for initialization
	protected override void Awake() {
        base.Awake();
        velocity = Random.Range(-3.5f, -4f);

        durability = 8;

        fireCycle = 1f;
        muzzleDistance.Add(new Vector2(0, -0.3f));
        bulletKind = BulletManager.BulletKind.ENEMY_SMALL;
        bulletVelocity = 5f;

        score = 20;
    }
}
