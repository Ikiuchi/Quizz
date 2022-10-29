using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMgr : MonoBehaviour
{
	public static GameMgr Instance;

	#region Canvas

	[SerializeField]
	private GameObject gameCanvas;
	[SerializeField]
	private GameObject mainMenuCanvas;
	[SerializeField]
	private GameObject closestAnswerCanvas;
	[SerializeField]
	private GameObject againstTimeCanvas;
	[SerializeField]
	private GameObject mysteryBoxCanvas;

	List<GameObject> canvasGameList = new List<GameObject>();

	#endregion

	#region Game

	private int currentGame = 0;

	#endregion

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);

		DontDestroyOnLoad(Instance);
	}

	// Start is called before the first frame update
	void Start()
    {
		gameCanvas.SetActive(true);
		EnabledCanvas(mainMenuCanvas);

		canvasGameList.Add(mainMenuCanvas);
		canvasGameList.Add(closestAnswerCanvas);
		canvasGameList.Add(againstTimeCanvas);
		canvasGameList.Add(mysteryBoxCanvas);

		EnabledCanvas(mainMenuCanvas);
	}

	public void OpenGame()
	{
		gameCanvas.SetActive(true);
	}

	public void CloseGame()
	{
		gameCanvas.SetActive(false);
	}

	public void EnabledCanvas(GameObject go)
	{
		foreach (GameObject canvas in canvasGameList)
		{
			if (canvas != go)
				canvas.SetActive(false);
		}

		go.SetActive(true);
	}

	public void NextGame()
	{
		if (currentGame != canvasGameList.Count - 1)
			EnabledCanvas(canvasGameList[++currentGame]);
	}

	public void PreviousGame()
	{
		if (currentGame != 0)
			EnabledCanvas(canvasGameList[--currentGame]);
	}
}
