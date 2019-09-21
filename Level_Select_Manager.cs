using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

public class Level_Select_Manager : MonoBehaviour {

	private int selectedLevel;

	public Text theDescriptionText;


	
	void Start () {
		selectedLevel = 10; // задаю начальное значение, чтобы оно не было равно номеру ни одного уровня
	}
	
	
	void Update () {
		
	}


	public void LevelSelection()
	{

		// для следущего нужно исползовать System и UnityEngine.EventSystems
		selectedLevel = Convert.ToInt32(EventSystem.current.currentSelectedGameObject.name);
		PrintDescription(selectedLevel);

		Debug.Log(selectedLevel);		
	}


	public void Back()
	{
		SceneManager.LoadScene("Main_Menu");
	}


	public void Play()
	{
		if (selectedLevel != 10)
		{
			SceneManager.LoadScene("Level" + selectedLevel);
		}
		else
		{
			Debug.Log("Select Level !");
		}
	}


	public void PrintDescription(int lvl)
	{
		//string txt;

		switch (lvl)
		{
			case 0:				
				theDescriptionText.text = "Reset in: 4 sec\nCountdown available: yes\nLine lenght: short";
				break;

			case 2:
				theDescriptionText.text = "Reset in: classified\nCountdown available: none\nLine lenght: short";
				break;
		}
	}
}
