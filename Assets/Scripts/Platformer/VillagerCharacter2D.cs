using System.Collections;

/// <summary>
/// Villager Character controller that handles Player specific actions
/// Created by : Ian Jones - 06/10/17
/// Updated by : Ian Jones - 06/04/18
/// </summary>
public class VillagerCharacter2D : GroundCharacter2D
{
    public override void Move(Hashtable animData)
    {
        if ((bool)animData["Martyed"] && !m_Anim.GetNextAnimatorStateInfo(0).IsName("Marty") &&
            TimeObjectManager.timeState == TimeState.Forward)

        {
            m_Anim.SetTrigger("Martyed");
        }

        if (TimeObjectManager.timeState == TimeState.Backward)
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
