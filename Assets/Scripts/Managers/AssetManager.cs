using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AssetManager
{
    static Object obj;
    static Object[] objs;

    #region templates
    //static List<GameObject> _AssetList;

    //public static List<GameObject> AssetList
    //{
    //    get
    //    {
    //        if (_AssetList == null)
    //        {
    //            _AssetList = new List<GameObject>();

    //            objs = Resources.LoadAll("Filename");

    //            foreach (object obj in objs)
    //            {
    //                _AssetList.Add((GameObject)obj);
    //            }
    //        }

    //        return _AssetList;
    //    }
    //}

    //static GameObject _Asset;

    //public static GameObject Asset
    //{
    //    get
    //    {
    //        if (_Asset == null)
    //        {
    //            _Asset = new GameObject();

    //            obj = Resources.Load("Filename");

    //            _Asset = (GameObject)obj;
    //        }

    //        return _Asset;
    //    }
    //}
    #endregion

    static GameObject _Projectile;

    public static GameObject projectile
    {
        get
        {
            if (_Projectile == null)
            {
                _Projectile = new GameObject();

                obj = Resources.Load("Range");

                _Projectile = (GameObject)obj;
                _Projectile.CreatePool(100);
            }

            return _Projectile;
        }
    }

    static GameObject _Villager;

    public static GameObject Vilager
    {
        get
        {
            if (_Villager == null)
            {
                _Villager = new GameObject();

                obj = Resources.Load("Spiel");

                _Villager = (GameObject)obj;
                _Villager.CreatePool(30);
            }

            return _Villager;
        }
    }
}
