/// <summary>
/// Allows Flight minion to contain reset logic bit without unesecarry parts (May 
/// highlight issue with current system)
/// </summary>
public class FlightMinionTimeObject : MinionTimeObject
{
    protected override void Awake()
    {
        base.Awake();

        //TimeObject
        OnTrackFrame -= TrackTransform;
        OnPlayFrame -= PlayTransormFrame;

        //SpriteObject
        OnPlayFrame -= PlaySpriteFrame;
        OnTrackFrame -= TrackSpriteFrame;
        OnFinishReverse -= OnSpriteFinishReverse;
        OnStartPlayback -= OnSpriteStartPlayback;

        //AnimatorObject
        OnTrackFrame -= TrackSpriteSheetFrame;
        OnPlayFrame -= PlaySpriteSheetFrame;
        OnFinishPlayback -= OnFinishSpriteSheetPlayback;
        OnStartPlayback -= OnSpriteSheetStartPlayback;
    }
}