using UnityEngine;
using System.Collections;

public class Attack : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        Debug.Log("Collided with " + coll.gameObject.name);
        if (coll.gameObject.layer == (LayerMask.NameToLayer("Villager")) && this.enabled
            && !Game.GodMode)
        {
            coll.gameObject.GetComponent<Villager>().OnHit();
        }
        else if (this.name.Contains("Range"))
        {
            Destroy(this.gameObject);
        }

    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (this.name.Contains("Range"))
        {
            if (coll.gameObject.GetComponent<Head>())
            {
                coll.gameObject.GetComponent<Head>().OnHit();
            }

            Destroy(this.gameObject);
        }
    }
}
