/// <summary>
/// Allows Flight minion to contain reset logic bit without unesecarry parts (May 
/// highlight issue with current system)
/// </summary>
public class FlightMinionTimeObject : MinionTimeObject
{
    protected override void Awake()
    {
        base.Awake();

        OnPlayFrame -= PlaySpriteSheetFrame;
        OnTrackFrame -= TrackSpriteSheetFrame;

        OnFinishReverse -= OnSpriteFinishReverse;

        OnStartPlayback -= OnSpriteStartPlayback;

        OnTrackFrame -= TrackSpriteSheetFrame;
        OnPlayFrame -= PlaySpriteSheetFrame;
        OnFinishPlayback -= OnFinishSpriteSheetPlayback;
        OnStartPlayback -= OnSpriteSheetStartPlayback;
        OnStartReverse -= OnSpriteSheetStartReverse;

        OnPlayFrame -= PlaySpriteFrame;
        OnTrackFrame -= TrackSpriteFrame;

        OnFinishReverse -= OnSpriteFinishReverse;

        OnStartPlayback -= OnSpriteStartPlayback;
    }
}