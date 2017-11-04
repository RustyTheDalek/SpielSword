using UnityEngine;

public class VillagerCharacter2D : PlatformerCharacter2D
{

    public void Move(VillagerAnimData animData)
    {
        base.Move(animData);

        if (animData.martyed && !m_Anim.GetNextAnimatorStateInfo(0).IsName("Marty") &&
            Game.timeState == TimeState.Forward)
        {
            m_Anim.SetTrigger("Martyed");
        }

        if (Game.timeState == TimeState.Backward)
        {
            m_Anim.SetTrigger("UnMarty");
        }


        if (animData.playerSpecialIsTrigger)
        {
            if (animData.playerSpecial)
                m_Anim.SetTrigger("PlayerSpecial");
        }
        else
        {
            m_Anim.SetBool("PlayerSpecial", animData.playerSpecial);
        }
        m_Anim.SetBool("CanSpecial", animData.canSpecial);
    }

}
