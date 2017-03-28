using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {

	public static HUDManager Instance;


	bool showUI = true;
	Canvas myCanvas;
	public Slider speedSlider;
	public Toggle insuranceToggle;
	public Text SpeedText;

	// Use this for initialization
	void Start () {
		Instance = this;
		myCanvas = this.GetComponent<Canvas>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.H)){
			showUI = !showUI;
			myCanvas.enabled = showUI;
		}
		Overlord.Instance.generationSpeed = speedSlider.value;
		Overlord.Instance.pathmakersInsured = insuranceToggle.isOn;
		SpeedText.text = "Speed: " + speedSlider.value;
	}
}
