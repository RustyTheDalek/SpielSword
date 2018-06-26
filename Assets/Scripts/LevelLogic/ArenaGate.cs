using UnityEngine;

/// <summary>
/// Manages Closing of Gate when Player Enters Arena
/// When player enters the arena the Gate moves down but behind the player then slides
/// into place.
/// TODO: Find more elegant solution, could use a cutscene at the start of the fight to move gate.
/// Created by Ian Jones : 03/04/18
/// </summary>
public class ArenaGate : MonoBehaviour {

    public Vector3 closingPos, closePos;

    public float speed;

    bool closing = false;

	// Use this for initialization
	public void Setup(ArenaEntry arenaEntry, TimeObjectManager timeManager)
    {
        arenaEntry.OnPlayerEnterArena += CloseGate;

        timeManager.OnRestartLevel += ForceCloseGate;
	}
	
	void CloseGate()
    {
        Debug.Log("Closing Gate");
        transform.localPosition = closingPos;
        closing = true;
    }

    //This ensures the gate is properly close in case player somehow dies before it 
    //close. Bit defensive but best to be sure
    void ForceCloseGate()
    {
        transform.localPosition = closePos;
        closing = false;
    }

    private void Update()
    {
        if(closing)
        {
            transform.Translate(Vector3.right * Time.deltaTime * speed);

            if (Vector3.Distance(transform.localPosition, closePos) < .1f)
            {
                transform.localPosition = closePos;
                closing = false;
            }
        }
    }

    public void Unsubscribe(ArenaEntry arenaEntry, TimeObjectManager timeManager)
    {
        arenaEntry.OnPlayerEnterArena -= CloseGate;
        timeManager.OnRestartLevel -= ForceCloseGate;
    }
}
