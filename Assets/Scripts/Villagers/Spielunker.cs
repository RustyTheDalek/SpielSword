using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spielunker : Villager 
{
    #region Public Fields

    #endregion

    #region Protected Fields
    #endregion

    #region Private Fields

    GameObject curHook;

    Transform hookTrans;
    #endregion

    #region Unity Methods

    public override void Awake()
    {
        base.Awake();

        hookTrans = GameObject.Find(this.name + "/HookTrans").transform;
    }
    #endregion

    #region Public Methods

    public override void OnSpecial(bool _PlayerSpecial)
    {
        base.OnSpecial(_PlayerSpecial);

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 hookPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            curHook = AssetManager.Hook.Spawn(hookTrans.transform.position);

            curHook.GetComponent<HookRope>().destination = hookPoint;
            curHook.GetComponent<HookRope>().player = gameObject;
        }
    }

    #endregion

    #region Private Methods
    #endregion
}
