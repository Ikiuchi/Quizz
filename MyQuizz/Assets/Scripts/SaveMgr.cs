using System.Xml.Serialization;
using System.IO;
using UnityEngine;
using System.Runtime.CompilerServices;

public class SaveMgr
{

	public static void Serialize(object item, string path)
	{
		XmlSerializer serializer = new XmlSerializer(item.GetType());
		StreamWriter writer = new StreamWriter(path);
		serializer.Serialize(writer.BaseStream, item);
		writer.Close();
	}

	public static T Deserialize<T>(string path)
	{
		if (!File.Exists(path))
			return default;

		XmlSerializer serializer = new XmlSerializer(typeof(T));
		StreamReader reader = new StreamReader(path);
		T deserialized = (T)serializer.Deserialize(reader.BaseStream);
		reader.Close();
		return deserialized;
	}

	public static string RemoveSpace(string str)
	{
		return str.Replace(' ', '_');
	}

	public static string RecoverSpace(string str)
	{
		return str.Replace('_', ' ');
	}
}