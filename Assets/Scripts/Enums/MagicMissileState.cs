public enum MagicMissileState
{
    Rising,             //At start of life MM will briefly fly directly up.
    Tracking,   
    ToDiversionAngle,   //When rotation Angle is incrementing to Diversion angle
    ToOriginAngle,      //Returning to original angle
};
