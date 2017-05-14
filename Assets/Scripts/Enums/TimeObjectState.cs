using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TimeObjectState
{
    Present,        //Object that began life this iteration and is currently tracking it's life
    PastStart,      //Object that has alreadt lived it's life and waiting to replay
    PastPlaying,    //Object that has already been created and is simply reliving it's existence
    PastFinished,   //Object that is in the past and finished it's playback
    Void,           //Not to be tracked
    PresentDead,    //Spawnable objects use this state when they have 'lived'
};
