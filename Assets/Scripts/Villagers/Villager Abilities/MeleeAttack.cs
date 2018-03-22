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
        if (collision.name == "Head" && Game.timeState == TimeState.Forward)
        {
            collision.GetComponent<Head>().OnHit(Damage);
        }
    }
}
