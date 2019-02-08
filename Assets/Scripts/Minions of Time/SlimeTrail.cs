using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeTrail : MonoBehaviour {

    public PhysicsMaterial2D slimeMaterial, stickyMaterial;

    float timer = 0;
    public float lifeTime = 1f;

    protected void OnEnable()
    {
        timer = lifeTime;
    }

    protected void Update()
    {
        timer -= Time.deltaTime;

        if(timer <= 0)
        {
            this.Recycle();
        }
    }

    //TODO:Set animator to be sliding animation
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
            collision.GetComponentInParent<GroundCharacter2D>().SetSliding(true, slimeMaterial);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
            collision.GetComponentInParent<GroundCharacter2D>().SetSliding(false, stickyMaterial);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            collision.GetComponentInParent<GroundCharacter2D>().SetSliding(true, slimeMaterial);

    }

}
