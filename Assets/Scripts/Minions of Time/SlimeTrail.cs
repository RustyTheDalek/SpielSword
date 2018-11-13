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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.sharedMaterial = slimeMaterial;
        collision.GetComponent<GroundCharacter2D>().m_Sliding = true;

        Debug.Log(collision.name + ": In the Slime");
        collision.GetComponent<SpriteRenderer>().color = new Color(
            collision.GetComponent<SpriteRenderer>().color.r, 
            collision.GetComponent<SpriteRenderer>().color.g, 
            collision.GetComponent<SpriteRenderer>().color.b, .75f);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collision.sharedMaterial = stickyMaterial;
        collision.GetComponent<GroundCharacter2D>().m_Sliding = false;

        Debug.Log(collision.name + ": Out the slime");

        collision.GetComponent<SpriteRenderer>().color = new Color(
            collision.GetComponent<SpriteRenderer>().color.r,
            collision.GetComponent<SpriteRenderer>().color.g,
            collision.GetComponent<SpriteRenderer>().color.b, 1f);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        collision.sharedMaterial = slimeMaterial;
        collision.GetComponent<GroundCharacter2D>().m_Sliding = true;
        Debug.Log(collision.name + ": In the Slime");
    }

}
