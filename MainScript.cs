using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MainScript : MonoBehaviour
{
	public Slider line;
	public RectTransform lineRectTransform;
	public RectTransform scoreLoseTransform;

	public GameObject theResultPanel;
	public GameObject thePausePanel;
	public GameObject theGreenHighlight;
	public GameObject theRedHighlight;
	public GameObject theWinTitle;
	public GameObject theLoseTitle;
	public GameObject theLevelUnlockTitle;
	
	public Button theGameButton;
	public Button theBlockButton;

	public Text levelTitleText;
	public Text message;
	public Text healthText;
	public Text blockText;
	public Text scoreText;
	public Text scoreTextBest;
	public Text timerText;
	public Text resultScoreText;
	public Text scoreLoseText;

	public int health;

	public int block;

	public float counterLengthMin;
	public float counterLengthMax;
	private float counter;

	public float increaseRatio;
	public float decreaseRatio;

	private float score;
	private int scoreMax;

	public float scoreIncreaseRatio;
	public float scoreDecreaseRatio;

	public bool isGameStarted;
	public bool victory;
	public bool gameOver;
	public bool isButtonHited;
	private bool isBlockActivated;

	private bool isEraseMessageCoroutineRunning;

	public Animator shakeAnim;



	void Start()
	{
		levelTitleText.text = "LEVEL " + Level_Select_Manager.selectedLevel;

		healthText.text = "Health: " + health;
		blockText.text = "Blocks: " + block;

		score = 0;
		scoreText.text = "Score: " + score;

		message.text = "";

		scoreMax = GetMaxScore();
		PlayerPrefs.SetInt("maxScoreLevel" + Level_Select_Manager.selectedLevel, scoreMax);
		PrepareLine();

		counter = Mathf.RoundToInt(Random.Range(counterLengthMin, counterLengthMax));		
		timerText.text = "" + counter;

		StartCoroutine("gameStart");		
	}


	void Update()
	{

		if (gameOver == true || victory == true || isGameStarted == false)
		{
			return;
		}


		// управление линией
		if (isButtonHited == true)
		{
			if (line.value < line.maxValue)
			{
				line.value += increaseRatio;
				score += scoreIncreaseRatio;
			}
			else if (line.value >= line.maxValue)
			{
				if (isEraseMessageCoroutineRunning == true)
				{
					isEraseMessageCoroutineRunning = false;
					StopCoroutine("eraseMessage");
				}

				victory = true;
				message.text = "VICTORY !";
				theGreenHighlight.SetActive(true);
				StartCoroutine(exitToMenu("win"));
			}
		}
		else if (isButtonHited == false)
		{
			if (line.value > 0f)
			{
				line.value -= decreaseRatio;
				score -= scoreDecreaseRatio;
			}
			else if (line.value <= 0f)
			{
				if (isEraseMessageCoroutineRunning == true)
				{
					theGreenHighlight.SetActive(false);
					isEraseMessageCoroutineRunning = false;
					StopCoroutine("eraseMessage");
				}

				gameOver = true;
				message.text = "GAME OVER";
				theRedHighlight.SetActive(true);
				StartCoroutine(exitToMenu("lose"));
			}
		}


		// управление отсчетом
		if (counter > 0)
		{
			counter -= Time.deltaTime;
		}

		else if (counter <= 0)
		{
			if (isButtonHited == false)
			{
				AudioManager.instance.PlaySFX(6);

				counter = Mathf.RoundToInt(Random.Range(counterLengthMin, counterLengthMax));
				message.text = "Reset!";
				theGreenHighlight.SetActive(true);
				StartCoroutine("eraseMessage");
			}

			else if (isButtonHited == true)
			{
				if (isBlockActivated == true)
				{
					AudioManager.instance.PlaySFX(7);

					isBlockActivated = false;
					if(block > 0)
					{
						theBlockButton.interactable = true;
					}

					counter = Mathf.RoundToInt(Random.Range(counterLengthMin, counterLengthMax));
					message.text = "BLOCKED";
					theGreenHighlight.SetActive(true);
					StartCoroutine("eraseMessage");
				}

				else if (isBlockActivated == false) 
				{
					health -= 1;
					healthText.text = "Health: " + health;

					AudioManager.instance.StopSFX(5);
					AudioManager.instance.PlaySFX(3);

					if (health < 1)
					{
						gameOver = true;
						theRedHighlight.SetActive(true);
						StartCoroutine(typeGameOver());
					}

					else if (health >= 1)
					{
						counter = Mathf.RoundToInt(Random.Range(counterLengthMin, counterLengthMax));
						theRedHighlight.SetActive(true);
						StartCoroutine("eraseMessage");

						CamShake();

						isButtonHited = false;
						theGameButton.interactable = false;
						StartCoroutine(activateGameButton());
					}

					message.text = "Reset!";
				}
			}
		}

		scoreText.text = "Score: " + Mathf.FloorToInt(score);
		timerText.text = "" + Mathf.RoundToInt(counter);
	}


	public void CamShake()
	{
		shakeAnim.SetTrigger("shake");
	}


	// стирание надписи в поле message
	IEnumerator eraseMessage()
	{
		isEraseMessageCoroutineRunning = true;

		yield return new WaitForSeconds(1f);

		message.text = "";
		theGreenHighlight.SetActive(false);
		theRedHighlight.SetActive(false);
		isEraseMessageCoroutineRunning = false;
	}


	// выведение в поле message надписи Game Over
	IEnumerator typeGameOver()
	{
		yield return new WaitForSeconds(1f);

		message.text = "GAME OVER";
		StartCoroutine(exitToMenu("lose"));
	}


	// активация игровой кнопки после кулдауна
	IEnumerator activateGameButton()
	{
		yield return new WaitForSeconds(1.3f);

		theGameButton.interactable = true;
	}


	// активация панели после победы/проигрыша
	IEnumerator exitToMenu(string result)
	{
		if(result == "win")
		{
			AudioManager.instance.PlaySFX(1);
		}
		else if(result == "lose")
		{
			AudioManager.instance.PlaySFX(2);
		}

		string currentLevelName = "Level" + Level_Select_Manager.selectedLevel;
		string bestScoreCurrentLevel = "bestScore" + currentLevelName;

		yield return new WaitForSeconds(2f);

		if (Mathf.FloorToInt(score) > PlayerPrefs.GetInt(bestScoreCurrentLevel))
		{
			PlayerPrefs.SetInt(bestScoreCurrentLevel, Mathf.FloorToInt(score));
		}

		theResultPanel.SetActive(true);
		
		if(result == "win")
		{
			theWinTitle.SetActive(true);

			HandleLevelUnlock();
		}
		else if(result == "lose")
		{
			theLoseTitle.SetActive(true);
		}

		resultScoreText.text = "Score: " + Mathf.FloorToInt(score) + " / " + scoreMax;
		scoreTextBest.text = "Best Score: " + PlayerPrefs.GetInt(bestScoreCurrentLevel);
	}



	// запуск игры спустя временной лаг после нажатия кнопки Старт
	IEnumerator gameStart()
	{
		yield return new WaitForSeconds(1f);

		isGameStarted = true;
		message.text = "GO !";
		StartCoroutine("eraseMessage");
	}


	int GetMaxScore()
	{
		float w = lineRectTransform.rect.width / 2; //длина линии для заполнения
		float v = increaseRatio * 60; //сколько единиц заполненяется за секунду (из расчёта, что частота обновления кадров - 60 в сек)
		float t = w / v; //максимально короткое время для заполнения половины линии (объём заполнения - 0.5 - делится на количество заполнения в секунду)
		return Mathf.RoundToInt(scoreIncreaseRatio * 60 * t); // единицы за одну итерацию * кол-во итераций в секунду * кол-во секунд
	}

	public void PrepareLine()
	{
		line.maxValue = lineRectTransform.rect.width;
		line.value = lineRectTransform.rect.width / 2;
	}



	//ФУНКЦИИ ДЛЯ КНОПОК

	// выход в главное меню при нажатии кнопки
	public void ExitToMainMenu()
	{
		StartCoroutine(EnumExitToMainMenu());
	}

	public IEnumerator EnumExitToMainMenu()
	{
		AudioManager.instance.PlaySFX(0);

		yield return new WaitForSeconds(0.5f);

		SceneManager.LoadScene("Main_Menu");
	}

	public void PauseGame()
	{
		AudioManager.instance.PlaySFX(0);

		isGameStarted = false;
		thePausePanel.SetActive(true);		
	}

	public void ContinueGame()
	{
		AudioManager.instance.PlaySFX(0);

		isGameStarted = true;
		thePausePanel.SetActive(false);
	}

	public void RestartGame()
	{
		StartCoroutine(EnumRestartGame());
	}

	public IEnumerator EnumRestartGame()
	{
		AudioManager.instance.PlaySFX(0);

		yield return new WaitForSeconds(0.5f);

		SceneManager.LoadScene("Level" + Level_Select_Manager.selectedLevel);
	}

	public void HandleLevelUnlock()
	{
		int nextLevel = Level_Select_Manager.selectedLevel + 1;
		string nextLevelFullName = "Level" + nextLevel;

		if (PlayerPrefs.GetInt(nextLevelFullName) == 0 && nextLevel < 7)
		{
			theLevelUnlockTitle.SetActive(true);

			PlayerPrefs.SetInt(nextLevelFullName, 1);
		}
	}

	public void UseBlock()
	{
		AudioManager.instance.PlaySFX(4);

		isBlockActivated = true;

		score -= 10;
		block -= 1;
		blockText.text = "Blocks: " + block;

		theBlockButton.interactable = false;

		Instantiate(scoreLoseText, scoreLoseTransform);
	}


	public void HitButton()
	{
		AudioManager.instance.PlaySFX(5);

		if (theGameButton.interactable == true)
		{
			isButtonHited = true;
		}		
	}

	public void ReleaseButton()
	{
		AudioManager.instance.StopSFX(5);

		if (theGameButton.interactable == true)
		{
			isButtonHited = false;
		}
	}
}
