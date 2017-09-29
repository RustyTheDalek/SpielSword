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

    static GameObject _Aura;

    public static GameObject Aura
    {
        get
        {
            if (_Aura == null)
            {
                _Aura = new GameObject();

                obj = Resources.Load("MageAura");

                _Aura = (GameObject)obj;
                _Aura.CreatePool(25);
            }

            return _Aura;
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
