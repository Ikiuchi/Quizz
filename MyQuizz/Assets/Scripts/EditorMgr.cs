using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorMgr : MonoBehaviour
{
	#region Canvas

	[SerializeField]
	private GameObject editorCanvas;
	[SerializeField]
	private GameObject mainMenuCanvas;
	[SerializeField]
	private GameObject closestAnswerCanvas;
	[SerializeField]
	private GameObject againstTimeCanvas;
	[SerializeField]
	private GameObject mysteryBoxCanvas;

	#endregion

	private void Start()
	{
		editorCanvas.SetActive(false);
	}

	public void OpenEditor()
	{
		editorCanvas.SetActive(true);
	}

	public void CloseEditor()
	{
		editorCanvas.SetActive(false);
	}

	public void OpenMainMenu()
	{
		mainMenuCanvas.SetActive(true);
	}

	public void CloseMainMenu()
	{
		mainMenuCanvas.SetActive(false);
	}

	public void OpenClosestAnswerCanvas()
	{
		CloseCanvas();
		closestAnswerCanvas.SetActive(true);
	}

	public void OpenAgainstTimeCanvas()
	{
		CloseCanvas();
		againstTimeCanvas.SetActive(true);
	}

	public void OpenMysteryBoxCanvas()
	{
		CloseCanvas();
		mysteryBoxCanvas.SetActive(true);
	}

	public void CloseCanvas()
	{
		mainMenuCanvas.SetActive(false);
		closestAnswerCanvas.SetActive(false);
		againstTimeCanvas.SetActive(false);
		mysteryBoxCanvas.SetActive(false);
	}
}
