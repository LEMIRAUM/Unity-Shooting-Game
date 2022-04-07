using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

    Rigidbody2D itemRB;
    RaycastHit2D circleRay;
    Transform playerTF;

    float velocity;
    bool track;

	// Use this for initialization
	void Start () {
        itemRB = GetComponent<Rigidbody2D>();

        velocity = -3f;
        track = false;

        itemRB.velocity = new Vector2(0, velocity);
    }
	
	// Update is called once per frame
	void Update () {       
        if(track == false)
        {
            circleRay = Physics2D.CircleCast(itemRB.position, 1.2f, Vector2.zero, 0, LayerMask.GetMask("Player"));
            if (circleRay.collider != null)
            {
                playerTF = circleRay.collider.gameObject.transform;
                track = true;
            }
        }
        else
        {
            itemRB.position = Vector3.MoveTowards(itemRB.position, playerTF.position, 0.1f);   
        }
	}

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Border") && itemRB.position.y < 0)
            Destroy(gameObject);
    }
}
