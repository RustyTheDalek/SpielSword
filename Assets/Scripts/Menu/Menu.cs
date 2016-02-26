/// <summary>
/// Menu.
/// All the behaviours that need to be used in the village menu
/// Created: 26/02/2016
/// Updated by: Ross Stowell
/// </summary>
using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

	public CanvasGroup BSCanvas, SHCanvas, CHCanvas, TACanvas, TGCanvas;
	public CanvasGroup BSButton, SHButton, CHButton, TAButton, TGButton;

	// Use this for initialization
	void Start () {
		BSCanvas.alpha = 0;
		SHCanvas.alpha = 0;
		CHCanvas.alpha = 0;
		TACanvas.alpha = 0;
		TGCanvas.alpha = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnBSButton()
	{
		BSCanvas.alpha = 1;

		SHButton.alpha = 0;
		CHButton.alpha = 0;
		TAButton.alpha = 0;
		TGButton.alpha = 0;
	}

	public void OnSHButton()
	{
		SHCanvas.alpha = 1;

		BSButton.alpha = 0;
		CHButton.alpha = 0;
		TAButton.alpha = 0;
		TGButton.alpha = 0;
	}

	public void OnCHButton()
	{
		CHCanvas.alpha = 1;

		SHButton.alpha = 0;
		BSButton.alpha = 0;
		TAButton.alpha = 0;
		TGButton.alpha = 0;
	}

	public void OnTAButton()
	{
		TACanvas.alpha = 1;

		SHButton.alpha = 0;
		CHButton.alpha = 0;
		BSButton.alpha = 0;
		TGButton.alpha = 0;
	}

	public void OnTGButton()
	{
		TGCanvas.alpha = 1;

		SHButton.alpha = 0;
		CHButton.alpha = 0;
		TAButton.alpha = 0;
		BSButton.alpha = 0;
	}

}
