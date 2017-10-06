using UnityEngine;

public class VillagerCharacter2D : PlatformerCharacter2D
{

    public void Move(VillagerAnimData animData)
    {
        base.Move(animData);

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
