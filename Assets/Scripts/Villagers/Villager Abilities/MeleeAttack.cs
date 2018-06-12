using UnityEngine;
using System.Collections;

public class MeleeAttack : MonoBehaviour
{
    int _Damage = 3;
    public int damageMult = 1;

    public int Damage
    {
        get
        {
            return _Damage * damageMult;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (TimeObjectManager.timeState == TimeState.Forward)
        {
            if (collision.name == "Head")
            {
                collision.GetComponent<Head>().OnHit(Damage);
            }
            else if (collision.GetComponent<GroundMinions>())
            {
                collision.GetComponent<GroundMinions>().OnHit();
            }
        }
    }
}
