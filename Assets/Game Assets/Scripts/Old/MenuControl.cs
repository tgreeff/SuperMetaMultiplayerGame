using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour {

	public GameObject optionsPanel;
	public Button newGame, loadGame, options, quit, done;
	public Slider gamma;
	public Light menuLight;
	public static float gammaValue;

	//Example found here: https://docs.unity3d.com/ScriptReference/UI.Button-onClick.html
	void Start() {
		optionsPanel.SetActive(false);
		newGame.onClick.AddListener(newGameClick);
		loadGame.onClick.AddListener(loadGameClick);
		options.onClick.AddListener(optionsClick);
		done.onClick.AddListener(doneClick);
		quit.onClick.AddListener(quitClick);
		gamma.onValueChanged.AddListener(delegate { sliderChange(); });
	}

	void newGameClick() {
			SceneManager.LoadScene(1);
	}

	void loadGameClick() {	
		try {
			string file = "Saves/Save.sav";
			//Example found from: https://msdn.microsoft.com/en-us/library/system.io.file.exists(v=vs.110).aspx
			if (!File.Exists(file)) {
				return;
			}
			StreamReader reader = new StreamReader(file);
			SceneManager.LoadScene(1);
			while (!reader.EndOfStream) {
				String line = reader.ReadLine();
				line.TrimEnd();
				if (line.Contains("gamma=")) {
					WorldProperties.gamma = int.Parse(line.Substring(6));
				} else if (line.Contains("money=")) {
					HUDController.money = int.Parse(line.Substring(6));
				} else if (line.Contains("health=")) {
					HUDController.health = int.Parse(line.Substring(6));
				}
			}
			reader.Close();	
		}
		catch (Exception e) {
			Debug.Log("Loading File Failed");
			Debug.Log(e.Message);
		}
	}

	void optionsClick() {
		optionsPanel.SetActive(true);
	}

	void doneClick() {
		optionsPanel.SetActive(false);
	}

	void quitClick() {
		Application.Quit();
	}

	public void sliderChange() {
		gammaValue = gamma.value * 10;
		menuLight.intensity = gammaValue;
	}
}
