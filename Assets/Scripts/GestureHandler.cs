using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GestureRecognizer;

public class GestureHandler : MonoBehaviour
{
	public Text textResult;
	public GameObject image;
	public Transform reference;
	public MagicController magicController;
	private List<GameObject> imageList;
	private DrawDetector drawDetector;
    private void Start()
    {
		imageList = new List<GameObject>();
		drawDetector = GetComponent<DrawDetector>();
    }
    public void OnRecognize(RecognitionResult result)
	{
		StopAllCoroutines();
		if (result != RecognitionResult.Empty)
		{
			textResult.text = result.gesture.id + "\n" + Mathf.RoundToInt(result.score.score * 100) + "%";
			if(result.score.score >= 0.85f)
            {
				GameObject imageObj = Instantiate(image, reference);
				imageList.Add(imageObj);
				GesturePatternDraw gpd = imageObj.GetComponentInChildren<GesturePatternDraw>();
				gpd.pattern = RuneLetter.runeLetterDict[result.gesture.id].Pattern;
				drawDetector.ClearLines();
				magicController.magicManager.RegistMagic(RuneLetter.runeLetterDict[result.gesture.id].MagicLaunch);
            }
		}
		else
		{
			textResult.text = "?";
		}
	}
    private void OnDisable()
    {
		foreach (GameObject imageObj in imageList)
			Destroy(imageObj);
		imageList.Clear();
		drawDetector.ClearLines();
	}
}
