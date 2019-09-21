using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class HealthBar : MonoBehaviour
{
	public GameObject theResultPanel;
	public GameObject thePausePanel;
	public GameObject theGreenBG;
	public GameObject theRedBG;	

	public Text timerText;
	public Text message;

	public float counterLengthMin;
	public float counterLengthMax;
	private float counter;

	public float increaseRatio;
	public float decreaseRatio;

	public bool isGameStarted;
	public bool victory;
	public bool gameOver;	
	public bool isButtonHited;

	private bool isEraseMessageCoroutineRunning;



	void Start ()
	{
		StartGame();
	}


	void Update ()
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
				gameOver = true;
				message.text = "Reset!";
				theRedBG.SetActive(true);
				StartCoroutine(typeGameOver());				
			}
		}

		timerText.text = "" + Mathf.Round(counter);
	}


	// стирание надписи в поле message
	IEnumerator eraseMessage()
	{
		isEraseMessageCoroutineRunning = true;

		yield return new WaitForSeconds(1f);

		message.text = "";
		theGreenBG.SetActive(false);
		isEraseMessageCoroutineRunning = false;
	}


	// выведение в поле message надписи Game Over
	IEnumerator typeGameOver()
	{
		yield return new WaitForSeconds(1f);

		message.text = "GAME OVER";
		StartCoroutine(exitToMenu());
	}


	// активация панели после победы/проигрыша
	IEnumerator exitToMenu()
	{
		yield return new WaitForSeconds(2f);

		theResultPanel.SetActive(true);
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

		theGreenBG.SetActive(false);
		theRedBG.SetActive(false);
		theResultPanel.SetActive(false);
		thePausePanel.SetActive(false);

		transform.localScale = new Vector3(0.5f, transform.localScale.y, transform.localScale.z);
		counter = Mathf.Floor(Random.Range(counterLengthMin, counterLengthMax));

		message.text = "";
		timerText.text = "" + counter;

		isEraseMessageCoroutineRunning = false;

		StartCoroutine("gameStart");
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




	public void HitButton()
	{
		isButtonHited = true;
	}

	public void ReleaseButton()
	{
		isButtonHited = false;
	}
}
