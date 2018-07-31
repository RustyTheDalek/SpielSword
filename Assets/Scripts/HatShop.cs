using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatShop : MonoBehaviour {


    public HatDisplay hatdisplayPrefab;

    public RectTransform hatContent;

    public Basic villager;

	// Use this for initialization
	void Start ()
    {
        GameManager.gManager.OnLoadSave += LoadHats;
	}

    public void LoadHats(SaveData saveLoaded)
    {
        foreach (string hat in saveLoaded.Hats)
        {
            HatDisplay newDisplay = Instantiate(hatdisplayPrefab, hatContent, false);
            newDisplay.LoadInfo(GameManager.gManager.Hats[hat]);
            newDisplay.HatSelectBtn.onClick.AddListener(delegate { SetHat(GameManager.gManager.Hats[hat]); });
        }

        if(saveLoaded.Hat != null)
            villager.hat.sprite = GameManager.gManager.Hats[saveLoaded.Hat].hatDesign;
    }

    public void SetHat(Hat newHat)
    {
        if (newHat.name == GameManager.gManager.currentSave.Hat)
        {
            villager.hat.sprite = null;
            GameManager.gManager.currentSave.Hat = null;
            Debug.Log("No favourite");
        }
        else
        {
            villager.hat.sprite = newHat.hatDesign;
            Debug.Log(newHat.name + " Saved as new Favourite");
            GameManager.gManager.currentSave.Hat = newHat.name;
        }

        GameManager.gManager.Save();
    }

    private void OnDestroy()
    {
        GameManager.gManager.OnLoadSave -= LoadHats;
    }
}
