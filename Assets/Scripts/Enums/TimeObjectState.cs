using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TimeObjectState
{
    Present,    //Object that began life this iteration and is currently tracking it's life
    Past,       //Object that has already been created and is simply reliving it's existence
};
