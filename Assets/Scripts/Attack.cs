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
        if (coll.gameObject.layer == (LayerMask.NameToLayer("Villager")) && this.enabled)
        {
            coll.gameObject.GetComponent<Villager>().health--;
        }
    }
}
