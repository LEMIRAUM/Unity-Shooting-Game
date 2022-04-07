using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediumEnemy : Enemy {

	// Use this for initialization
	protected override void Awake() {
        base.Awake();
        velocity = Random.Range(-2f, -3f);

        durability = 15;

        fireCycle = 1.6f;
        muzzleDistance.Add(new Vector2(0, -0.5f));
        bulletKind = BulletManager.BulletKind.ENEMY_LARGE;
        bulletVelocity = 4f;

        score = 30;
    }
}
