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

    static GameObject _Hook;

    //TODO:Fix this as currently broken just an not used at present
    static GameObject Hook
    {
        get
        {
            if (_Hook == null)
            {
                _Hook = new GameObject();

                UnityEngine.Object obj = Resources.Load("Hook");

                _Hook = (GameObject)obj;
                _Hook.CreatePool(10);
            }

            return _Hook;
        }
    }

    #region Unity Methods

    protected override void Awake()
    {
        base.Awake();

        hookTrans = GameObject.Find(this.name + "/HookTrans").transform;
    }
    #endregion

    #region Public Methods

    //public override void OnSpecial(bool _PlayerSpecial)
    //{
    //    base.OnSpecial(_PlayerSpecial);

    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        Vector2 hookPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    //        curHook = Hook.Spawn(hookTrans.transform.position);

    //        curHook.GetComponent<HookRope>().destination = hookPoint;
    //        curHook.GetComponent<HookRope>().player = gameObject;
    //    }
    //}

    #endregion

    #region Private Methods
    #endregion
}
