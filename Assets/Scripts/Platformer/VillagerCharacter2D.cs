using UnityEngine;
using System.Collections;

public class VillagerCharacter2D : PlatformerCharacter2D
{

    public override void Move(Hashtable animData)
    {

        if ((bool)animData["Martyed"] && !m_Anim.GetNextAnimatorStateInfo(0).IsName("Marty") &&
            Game.timeState == TimeState.Forward)
        {
            m_Anim.SetTrigger("Martyed");
        }

        if (Game.timeState == TimeState.Backward)
        {
            m_Anim.SetTrigger("UnMarty");
        }


        if ((bool)animData["PlayerSpecialIsTrigger"])
        {
            if ((bool)animData["PlayerSpecial"])
                m_Anim.SetTrigger("PlayerSpecial");
        }
        else
        {
            m_Anim.SetBool("PlayerSpecial", (bool)animData["PlayerSpecial"]);
        }
        m_Anim.SetBool("CanSpecial", (bool)animData["CanSpecial"]);

        base.Move(animData);
    }

}
