using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class HealthBarLvl2 : MonoBehaviour
{
	public GameObject theResultPanel;
	public GameObject thePausePanel;
	public GameObject theGreenBG;
	public GameObject theRedBG;
	
	public Button theGameButton;
	public Button theBlockButton;

	public Text message;
	public Text healthText;
	public Text blockText;
	public Text scoreText;
	public Text scoreTextBest;
	public Text timerText;
	public Text resultScoreText;

	public int health;
	private int healthCurrent;

	public int block;
	private int blockCurrent;

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



	void Start()
	{
		StartGame();
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
			if (transform.localScale.x < 1)
			{
				transform.localScale += new Vector3(increaseRatio, 0, 0);

				score += scoreIncreaseRatio;

			}
			else if (transform.localScale.x >= 1)
			{
				victory = true;
				message.text = "VICTORY !";
				theGreenBG.SetActive(true);
				StartCoroutine(exitToMenu());
			}
		}
		else if (isButtonHited == false)
		{
			if (transform.localScale.x > 0)
			{
				transform.localScale -= new Vector3(decreaseRatio, 0, 0);

				score -= scoreDecreaseRatio;

			}
			else if (transform.localScale.x <= 0)
			{
				if (isEraseMessageCoroutineRunning == true)
				{
					theGreenBG.SetActive(false);
					isEraseMessageCoroutineRunning = false;
					StopCoroutine("eraseMessage");
				}

				gameOver = true;
				message.text = "GAME OVER";
				theRedBG.SetActive(true);
				StartCoroutine(exitToMenu());
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
				counter = Mathf.Floor(Random.Range(counterLengthMin, counterLengthMax));
				message.text = "Reset!";
				theGreenBG.SetActive(true);
				StartCoroutine("eraseMessage");
			}

			else if (isButtonHited == true)
			{

				if (isBlockActivated == true)
				{
					isBlockActivated = false;

					counter = Mathf.Floor(Random.Range(counterLengthMin, counterLengthMax));
					message.text = "BLOCKED";
					theGreenBG.SetActive(true);
					StartCoroutine("eraseMessage");
				}

				else if (isBlockActivated == false) 
				{
					healthCurrent -= 1;
					healthText.text = "Health: " + healthCurrent;

					if (healthCurrent < 1)
					{
						gameOver = true;
						theRedBG.SetActive(true);
						StartCoroutine(typeGameOver());
					}

					else if (healthCurrent >= 1)
					{
						counter = Mathf.Floor(Random.Range(counterLengthMin, counterLengthMax));
						theRedBG.SetActive(true);
						StartCoroutine("eraseMessage");

						isButtonHited = false;
						theGameButton.interactable = false;
						StartCoroutine(activateGameButton());
					}

					message.text = "Reset!";
				}

			}
		}

		scoreText.text = "Score: " + Mathf.FloorToInt(score);
		timerText.text = "" + Mathf.Round(counter);

	}


	// стирание надписи в поле message
	IEnumerator eraseMessage()
	{
		isEraseMessageCoroutineRunning = true;

		yield return new WaitForSeconds(1f);

		message.text = "";
		theGreenBG.SetActive(false);
		theRedBG.SetActive(false);
		isEraseMessageCoroutineRunning = false;
	}


	// выведение в поле message надписи Game Over
	IEnumerator typeGameOver()
	{
		yield return new WaitForSeconds(1f);

		message.text = "GAME OVER";
		StartCoroutine(exitToMenu());
	}


	// активация игровой кнопки после кулдауна
	IEnumerator activateGameButton()
	{
		yield return new WaitForSeconds(1.5f);

		theGameButton.interactable = true;
	}


	// активация панели после победы/проигрыша
	IEnumerator exitToMenu()
	{
		yield return new WaitForSeconds(2f);

		if (score > PlayerPrefs.GetInt("scoreBest"))
		{
			PlayerPrefs.SetInt("scoreBest", Mathf.FloorToInt(score));
		}

		theResultPanel.SetActive(true);
		resultScoreText.text = "Score: " + Mathf.FloorToInt(score) + " / " + scoreMax;
	}



	// запуск игры спустя временной лаг после нажатия кнопки Старт
	IEnumerator gameStart()
	{
		yield return new WaitForSeconds(1f);

		isGameStarted = true;
		message.text = "GO !";
		StartCoroutine("eraseMessage");
	}


	// запуск/перезапуск уровня
	public void StartGame()
	{
		isButtonHited = false;
		victory = false;
		gameOver = false;
		isGameStarted = false;
		isBlockActivated = false;

		theBlockButton.interactable = true;

		theGreenBG.SetActive(false);
		theRedBG.SetActive(false);
		theResultPanel.SetActive(false);
		thePausePanel.SetActive(false);		

		transform.localScale = new Vector3(0.5f, transform.localScale.y, transform.localScale.z);

		healthCurrent = health;
		healthText.text = "Health: " + healthCurrent;

		blockCurrent = block;
		blockText.text = "Blocks: " + blockCurrent;

		score = 0;
		scoreText.text = "Score: " + score;

		scoreMax = GetMaxScore();

		scoreTextBest.text = "Best Score: " + PlayerPrefs.GetInt("scoreBest");

		message.text = "";

		counter = Mathf.Floor(Random.Range(counterLengthMin, counterLengthMax));
		timerText.text = "" + Mathf.Round(counter);

		isEraseMessageCoroutineRunning = false;

		StartCoroutine("gameStart");
	}


	int GetMaxScore()
	{
		float v = increaseRatio * 60; // скорость заполнения линии пунктов/сек (предполагается, что частота обновления кадров 60 в сек)
		float t = 0.5f / v; // максимально короткое время для заполнения линии
		return Mathf.FloorToInt(scoreIncreaseRatio * 60 * t);
	}


	// выход в главное меню при нажатии кнопки
	public void ExitGame()
	{
		SceneManager.LoadScene("Main_Menu");
	}


	public void PauseGame()
	{
		isGameStarted = false;
		thePausePanel.SetActive(true);
	}


	public void ContinueGame()
	{
		isGameStarted = true;
		thePausePanel.SetActive(false);
	}

	public void UseBlock()
	{
		isBlockActivated = true;

		score -= 10;
		blockCurrent -= 1;
		blockText.text = "Blocks: " + blockCurrent;

		if (blockCurrent == 0)
		{
			theBlockButton.interactable = false;
		}

	}




	public void HitButton()
	{
		if (theGameButton.interactable == true)
		{
			isButtonHited = true;
		}

		
	}

	public void ReleaseButton()
	{
		if (theGameButton.interactable == true)
		{
			isButtonHited = false;
		}
	}
}
