using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public Rigidbody2D rb;

    public Vector2 throwDirection;

    public float throwForce;

    protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected void Start()
    {
        Throw();
    }

    protected void Update()
    {
        transform.right = Vector3.Slerp(transform.right, rb.velocity.normalized, Time.deltaTime * 15);
    }

    public void Throw(Vector2 dirOveride)
    {
        throwDirection *= dirOveride;

        Throw();
    }

    public void Throw()
    {
        transform.right = throwDirection;
        rb.AddForce(throwDirection * throwForce * Time.deltaTime, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            rb.bodyType = RigidbodyType2D.Static;
            rb.simulated = false;
            GetComponent<Collider2D>().enabled = false;
        }
    }
}
