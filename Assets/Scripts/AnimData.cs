using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AnimData
{
    //public Dictionary<string, float> floatAnims;
    //public Dictionary<string, int> intAnims;
    //public Dictionary<string, bool> boolAnims;
    //TODO: Add Triggers if needed

    public Hashtable floatas;

    public void init()
    {
        floatas = new Hashtable();
        //floatAnims = new Dictionary<string, float>();
        //intAnims = new Dictionary<string, int>();
        //boolAnims = new Dictionary<string, bool>();
    }
}
