using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Displays Hat information
public class HatDisplay : MonoBehaviour {

    public Hat hat;

    public Text nameTxt, descrptionTxt, statsTxt;

    public Image hatImg;

    public void LoadInfo(Hat _Hat)
    {
        hat = _Hat;

        nameTxt.text = hat.name;
        descrptionTxt.text = hat.description;
        descrptionTxt.text = hat.description;

        hatImg.enabled = true;
        hatImg.sprite = hat.hatDesign;

    }
}
