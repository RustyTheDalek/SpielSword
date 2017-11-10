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

    static GameObject _Villager;

    public static GameObject Villager
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

    static GameObject _Ward;

    public static GameObject Ward
    {
        get
        {
            if (_Ward == null)
            {
                _Ward = new GameObject();

                obj = Resources.Load("Ward");

                _Ward = (GameObject)obj;
                _Ward.CreatePool(25);
            }

            return _Ward;
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

    static Dictionary<string, RuntimeAnimatorController> _VillagerAnimators;

    public static Dictionary<string, RuntimeAnimatorController> VillagerAnimators
    {
        get
        {
            if (_VillagerAnimators == null)
            {
                _VillagerAnimators = new Dictionary<string, RuntimeAnimatorController>();

                objs = Resources.LoadAll<RuntimeAnimatorController>("VAnimators");

                RuntimeAnimatorController temp;

                foreach (object obj in objs)
                {
                    temp = (RuntimeAnimatorController)obj;
                    Debug.Log(temp.name);
                    _VillagerAnimators.Add(temp.name, (RuntimeAnimatorController)obj);
                }
            }

            return _VillagerAnimators;
        }
    }
}
