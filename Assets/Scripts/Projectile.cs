using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    Rigidbody2D rb;

    public float throwForce;

    /// <summary>
    /// Angle which object is thrown at right
    /// </summary>
    public float rightAngle;

    public float leftAngle
    {
        get
        {
            return 360 - rightAngle;
        }
    }

    protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        rightAngle = transform.rotation.eulerAngles.z;
    }

    protected void Start()
    {
        Throw();
    }

    public void Throw()
    {
        rb.AddForce(-transform.up * throwForce * Time.deltaTime, ForceMode2D.Impulse);
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
