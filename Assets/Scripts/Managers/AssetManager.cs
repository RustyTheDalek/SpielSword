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
    //            _Asset.CreatePool(0);
    //        }

    //        return _Asset;
    //    }
    //}
    #endregion

    static GameObject _Projectile;

    public static GameObject Projectile
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

    static Dictionary<string, GameObject> _Villagers;

    public static Dictionary<string, GameObject> Villagers
    {
        get
        {
            if (_Villagers == null)
            {
                _Villagers = new Dictionary<string, GameObject>();

                objs = Resources.LoadAll("Villagers");

                GameObject gObj;

                foreach (object obj in objs)
                {
                    if (obj as GameObject != null)
                    {
                        gObj = (GameObject)obj;

                        _Villagers.Add(gObj.name, gObj);
                        _Villagers[gObj.name].CreatePool(50);
                    }
                }
            }

            return _Villagers;
        }
    }

    static Dictionary<string, Sprite> _VillagerSprites;

    public static Dictionary<string, Sprite> VillagerSprites
    {
        get
        {
            if (_VillagerSprites == null)
            {
                _VillagerSprites = new Dictionary<string, Sprite>();

                objs = Resources.LoadAll("Sprites");
                    
                Sprite sprite;

                foreach (object obj in objs)
                {
                    if (obj as Sprite != null)
                    {
                        sprite = (Sprite)obj;

                        //Debug.Log(sprite.name);
                        _VillagerSprites.Add(sprite.name, sprite);
                    }
                }
            }

            return _VillagerSprites;
        }
    }

    static Dictionary<string, GameObject> _Wards;

    public static Dictionary<string, GameObject> Wards
    {
        get
        {
            if (_Wards == null)
            {
                _Wards = new Dictionary<string, GameObject>();

                objs = Resources.LoadAll("Wards");

                GameObject gObj;

                foreach (object obj in objs)
                {
                    if (obj as GameObject != null)
                    {
                        gObj = (GameObject)obj;

                        _Wards.Add(gObj.name, gObj);
                        _Wards[gObj.name].CreatePool(50);
                    }
                }
            }

            return _Wards;
        }
    }

    static GameObject _MageAura;

    public static GameObject MageAura
    {
        get
        {
            if (_MageAura == null)
            {
                _MageAura = new GameObject();

                obj = Resources.Load("MageAura");

                _MageAura = (GameObject)obj;
                _MageAura.CreatePool(25);
            }

            return _MageAura;
        }
    }

    static GameObject _PriestAura;

    public static GameObject PriestAura
    {
        get
        {
            if (_PriestAura == null)
            {
                _PriestAura = new GameObject();

                obj = Resources.Load("PriestAura");

                _PriestAura = (GameObject)obj;
                _PriestAura.CreatePool(25);
            }

            return _PriestAura;
        }
    }

    static List<Material> _SpriteMaterials;

    public static List<Material> SpriteMaterials
    {
        get
        {
            if (_SpriteMaterials == null)
            {
                _SpriteMaterials = new List<Material>();

                objs = Resources.LoadAll("Materials");

                foreach (object obj in objs)
                {
                    _SpriteMaterials.Add((Material)obj);
                }
            }

            return _SpriteMaterials;
        }
    }

    static GameObject _WarlockImp;

    public static GameObject WarlockImp
    {
        get
        {
            if (_WarlockImp == null)
            {
                _WarlockImp = new GameObject();

                obj = Resources.Load("Imp");

                _WarlockImp = (GameObject)obj;
                _WarlockImp.CreatePool(100);
            }

            return _WarlockImp;
        }
    }
}
