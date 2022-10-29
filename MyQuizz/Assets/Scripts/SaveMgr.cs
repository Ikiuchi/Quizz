using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;

public class SaveMgr : MonoBehaviour
{
	public static SaveMgr Instance;
	//Application.dataPath

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);

		DontDestroyOnLoad(Instance);
	}

	public void Save(string fileName)
	{
		
	}

	public void Load(string fileName)
	{
		
	}

	public static void LoadFromFile(string filepath)
	{
		XmlSerializer serializer = new XmlSerializer(typeof(ClosestAnswerMgr));
		using (FileStream stream = new FileStream(filepath, FileMode.Open))
		{
			ClosestAnswerMgr c;
			c = serializer.Deserialize(stream) as ClosestAnswerMgr;
		}
	}
}
