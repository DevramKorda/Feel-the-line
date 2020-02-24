using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreLoseTextControl : MonoBehaviour
{
	private float moveSpeed = 100f;
	private float fadeSpeed = 1.5f;
	private float counter = 2f;
	private RectTransform theTransform;
	private Text theText;


    // Start is called before the first frame update
    void Start()
    {
		theTransform = GetComponent<RectTransform>();
		theText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
		theTransform.localPosition += new Vector3(0f, moveSpeed * Time.deltaTime, 0f);

		counter -= Time.deltaTime;

		if(counter <= 1.5f)
		{
			theText.color = new Color(theText.color.r, theText.color.g, theText.color.b, Mathf.MoveTowards(theText.color.a, 0f, fadeSpeed * Time.deltaTime));
		}

		if (counter <= 0f)
		{
			Destroy(gameObject);
		}
    }
}
