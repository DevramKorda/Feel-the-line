using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;


public class Level_Select_Manager : MonoBehaviour {

	public static int selectedLevel;

	public Text theDescriptionText;

	public Button[] levelButtons;

	public int[] quantityOfLevels;

	public Image medalGold;
	public Image medalSilver;
	public Image medalBronze;



	void Start ()
	{
		//делаем активной кнопку выбора 0 уровня
		if (PlayerPrefs.GetInt("Level0") == 0)
		{
			PlayerPrefs.SetInt("Level0", 1);
		}

		//обновляем визуальное отображение активности/неактивности кнопок уровней
		for (int i = 0; i < quantityOfLevels.Length; i++)
		{
			string levelName = "Level" + quantityOfLevels[i];

			if (PlayerPrefs.GetInt(levelName) == 0)
			{
				levelButtons[i].interactable = false;
			}
			else
			{
				levelButtons[i].interactable = true;

				UpdateMedals(levelButtons[i], levelName);
			}
		}


		// задаём начальное значение, чтобы оно не было равно номеру ни одного уровня
		selectedLevel = 10;
	}

	public void UpdateMedals(Button btn, string levelName)
	{
		GameObject medal = btn.transform.GetChild(0).gameObject;

		if (PlayerPrefs.GetInt("bestScore" + levelName) != 0)
		{
			medal.SetActive(true);

			if (ChoseMedalSprite(levelName) == "gold")
			{
				medal.GetComponent<Image>().sprite = medalGold.sprite;				
			}
			else if (ChoseMedalSprite(levelName) == "silver")
			{
				medal.GetComponent<Image>().sprite = medalSilver.sprite;
			}
			else if (ChoseMedalSprite(levelName) == "bronze")
			{
				medal.GetComponent<Image>().sprite = medalBronze.sprite;
			}
		}
	}

	public string ChoseMedalSprite(string levelName)
	{
		float x = PlayerPrefs.GetInt("bestScore" + levelName);
		float y = PlayerPrefs.GetInt("maxScore" + levelName);
		float success = x / y;

		if (success >= 0.9f)
		{
			return "gold";
		}
		else if (success >= 0.7f)
		{
			return "silver";
		}
		else
		{
			return "bronze";
		}
	}


	void Update ()
	{

	}


	public void LevelSelection()
	{
		AudioManager.instance.PlaySFX(0);

		selectedLevel = Convert.ToInt32(EventSystem.current.currentSelectedGameObject.name);
		PrintDescription(selectedLevel);
	}

	public void Back()
	{
		StartCoroutine(EnumBack());	
	}

	public IEnumerator EnumBack()
	{
		AudioManager.instance.PlaySFX(0);

		yield return new WaitForSeconds(0.5f);

		SceneManager.LoadScene("Main_Menu");
	}

	public void Play()
	{
		StartCoroutine(EnumPlay());	
	}

	public IEnumerator EnumPlay()
	{
		AudioManager.instance.PlaySFX(0);

		yield return new WaitForSeconds(0.5f);

		if (selectedLevel != 10)
		{
			SceneManager.LoadScene("Level" + selectedLevel);
		}
		else
		{
			Debug.Log("Select Level !");
		}
	}

	public void ShowTutorial()
	{
		StartCoroutine(EnumShowTutorial());
	}

	public IEnumerator EnumShowTutorial()
	{
		AudioManager.instance.PlaySFX(0);

		yield return new WaitForSeconds(0.5f);

		SceneManager.LoadScene("Tutorial");
	}


	public void PrintDescription(int lvl)
	{
		theDescriptionText.text = "Level " + lvl + "\nBest Score: " + PlayerPrefs.GetInt("bestScoreLevel" + lvl) + "/" + PlayerPrefs.GetInt("maxScoreLevel" + lvl);
	}
}
