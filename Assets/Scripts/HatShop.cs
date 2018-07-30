using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatShop : MonoBehaviour {

    public Hat[] hats;

    public HatDisplay hatdisplayPrefab;

    public RectTransform hatContent;

    public Basic villager;

	// Use this for initialization
	void Start ()
    {
        GameManager.gManager.OnLoadSave += LoadHats;

        hats = Resources.LoadAll<Hat>("Hats");
	}

    public void LoadHats(SaveData saveLoaded)
    {
        foreach (Hat hat in hats)
        {
            HatDisplay newDisplay = Instantiate(hatdisplayPrefab, hatContent, false);
            newDisplay.LoadInfo(hat);
            newDisplay.HatSelectBtn.onClick.AddListener(delegate { SetHat(hat); });
        }

        if(saveLoaded.Hat != null)
            villager.hat.sprite = GameManager.gManager.Hats[saveLoaded.Hat].hatDesign;
    }

    public void SetHat(Hat newHat)
    {
        villager.hat.sprite = newHat.hatDesign;
        Debug.Log(newHat.name + " Saved as new Favourite");
        GameManager.gManager.currentSave.Hat = newHat.name;
        GameManager.gManager.Save();
    }

    private void OnDestroy()
    {
        GameManager.gManager.OnLoadSave -= LoadHats;
    }
}
