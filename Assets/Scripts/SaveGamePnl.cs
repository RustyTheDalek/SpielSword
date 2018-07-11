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
        createSaveBtn.gameObject.SetActive(false);

        hatsTxt.text = "Hats : " + saveInfo.Hats.Count;
        levelsTxt.text = "Levels: " + saveInfo.Levels.Count;
        storyProgressTxt.text = "Story Progress: " + saveInfo.StoryProgress;
        //Find Favourite hat

        infoPnl.gameObject.SetActive(true);
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
