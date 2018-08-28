using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SaveGamePnl : MonoBehaviour
{
    public Text nameTxt, hatsTxt, levelsTxt, storyProgressTxt;

    public Image vilPreview, hatPreview;

    public Button createSaveBtn;

    public RectTransform infoPnl;

    void Awake()
    {
    }

    // Use this for initialization
    void Start()
    {
        GameManager.gManager.OnNewGame += Disable;
    }

    public void LoadInfo(SaveData saveInfo)
    {
        try
        {
            hatsTxt.text = "Hats : " + saveInfo.Hats.Count;
            levelsTxt.text = "Levels: " + saveInfo.Levels.Count;
            storyProgressTxt.text = "Story Progress: " + saveInfo.StoryProgress;
            //Find Favourite hat
            if (saveInfo.Hat != null)
            {
                Hat favHat = GameManager.gManager.hats.LoadAsset<Hat>(saveInfo.Hat);
                Debug.Log("I've found me a hat");

                if (favHat != null)
                {
                    hatPreview.sprite = favHat.hatDesign;
                }
                else
                {
                    Debug.LogWarning("Cannot find :" + saveInfo.Hat + "hat in the collection");
                }
            }
            else
            {
                Debug.Log("No hat for me");
                hatPreview.enabled = false;
            }

            infoPnl.gameObject.SetActive(true);

            createSaveBtn.gameObject.SetActive(false);

        }
        catch
        {
            if(saveInfo == null)
                Debug.Log("Save info is null.");
            else
            {
                Debug.Log("Save info isn't null");
            }
        }
    }

    private void Disable()
    {
        enabled = false;
    }

    private void OnDestroy()
    {
        GameManager.gManager.OnNewGame -= Disable;
    }
}
