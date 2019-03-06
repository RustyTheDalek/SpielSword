using System;

[Flags]
public enum ObjectTrackType
{
    FrameTracking = 1,                          //Tracks changes on a per frame basis (can also include event tracking)
    EventTracking = 2,                          //Tracks events useful for adjusting components at certain points
    Both = FrameTracking | EventTracking  //Tracks Both
};
