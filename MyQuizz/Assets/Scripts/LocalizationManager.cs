using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;

/* 
 * English Id = 0 
 * French Id = 1
 */

public class LocalizationManager : MonoBehaviour
{
	public static LocalizationManager Instance;
	public bool initialized = false;
	private bool active = false;
	private string localizationKey = "LocalizationKey";

	private int id;
	public int GetId { get { return id; } }
	public bool Initialized { get { return initialized; } }

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else 
			Destroy(gameObject);

		DontDestroyOnLoad(Instance);
	}

	private void Start()
	{
		id = PlayerPrefs.GetInt(localizationKey, 0);
		ChangeLocal(id);
		initialized = true;
	}

	private IEnumerator SetLocal(int _localId)
	{
		active = true;
		yield return LocalizationSettings.InitializationOperation;
		LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_localId];
		PlayerPrefs.SetInt(localizationKey, _localId);
		active = false;
	}

	public void ChangeLocal(int _localId)
	{
		if (active)
			return;
		StartCoroutine(SetLocal(_localId));
	}
}
