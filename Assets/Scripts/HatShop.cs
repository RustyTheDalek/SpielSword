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
        GameManager.gManager.OnUnlockHat += LoadHat;
	}

    public void LoadHats(SaveData saveLoaded)
    {
        foreach (string hat in saveLoaded.Hats)
        {
            HatDisplay newDisplay = Instantiate(hatdisplayPrefab, hatContent, false);
            newDisplay.LoadInfo(GameManager.gManager.hats.LoadAsset<Hat>(hat));
            newDisplay.HatSelectBtn.onClick.AddListener(
                delegate { SetHat(GameManager.gManager.hats.LoadAsset<Hat>(hat)); });
        }

        if(saveLoaded.Hat != null && villager.hat)
            villager.hat.sprite = GameManager.gManager.hats.LoadAsset<Hat>(saveLoaded.Hat).hatDesign;
    }

    /// <summary>
    /// Load Single new Hat into shop
    /// </summary>
    /// <param name="hat">Hat to unlock</param>
    public void LoadHat(Hat hat)
    {
        HatDisplay newDisplay = Instantiate(hatdisplayPrefab, hatContent, false);
        newDisplay.LoadInfo(hat);
        newDisplay.HatSelectBtn.onClick.AddListener(delegate { SetHat(hat); });
    }

    public void SetHat(Hat newHat)
    {
        if (villager.hat == null)
        {
            Debug.LogWarning("No hat to change");
            return;
        }

        if (newHat.name == GameManager.gManager.CurrentSave.Hat)
        {
            villager.hat.sprite = null;
            GameManager.gManager.CurrentSave.Hat = null;
            Debug.Log("No favourite");
        }
        else
        {
            villager.hat.sprite = newHat.hatDesign;
            Debug.Log(newHat.name + " Saved as new Favourite");
            GameManager.gManager.CurrentSave.Hat = newHat.name;
        }

        GameManager.gManager.Save();
    }

    private void OnDestroy()
    {
        GameManager.gManager.OnLoadSave -= LoadHats;
        GameManager.gManager.OnUnlockHat -= LoadHat;
    }
}
