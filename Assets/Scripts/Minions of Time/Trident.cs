using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trident : MonoBehaviour {

    Rigidbody2D rb;

    public Vector2 direction;

    public float throwForce;
	// Use this for initialization
	void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        ThrowTrident();
    }

    public void ThrowTrident()
    {

        rb.AddForce(direction * throwForce * Time.deltaTime, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            rb.bodyType = RigidbodyType2D.Static;
            rb.simulated = false;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            GetComponent<Collider2D>().enabled = false;
        }
    }
}
