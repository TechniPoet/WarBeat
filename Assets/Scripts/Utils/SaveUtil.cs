using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public static class SaveUtil
{
	const string delimeter = "@#$";


	#region Save functions

	public static void SaveList<T>(List<T> list, string key)
	{
		string saveString = "";
		if (list.Count > 0)
		{
			for (int i = 0; i < list.Count; i++)
			{
				saveString += SerializeToString<T>(list[i]);
				if (i != list.Count - 1)
					saveString += delimeter;
			}
			PlayerPrefs.SetString(key, saveString);
		}
	}

	public static List<T> LoadList<T>(string key)
	{

		if (PlayerPrefs.HasKey(key))
		{
			List<T> retList = new List<T>();
			string[] strings = PlayerPrefs.GetString(key).Split(new String[] { delimeter }, System.StringSplitOptions.None);
			for (int i = 0; i < strings.Length; i++)
			{
				retList.Add(DeserializeFromString<T>(strings[i]));
			}
			return retList;
		}
		else
		{
			return null;
		}
	}

	public static List<T> LoadSafeList<T>(string key)
	{

		if (PlayerPrefs.HasKey(key))
		{
			List<T> retList = new List<T>();
			string[] strings = PlayerPrefs.GetString(key).Split(new String[] { delimeter }, System.StringSplitOptions.None);
			for (int i = 0; i < strings.Length; i++)
			{
				retList.Add(DeserializeFromString<T>(strings[i]));
			}
			return retList;
		}
		else
		{
			return new List<T>();
		}
	}

	private static TData DeserializeFromString<TData>(string settings)
	{
		byte[] b = Convert.FromBase64String(settings);
		using (var stream = new MemoryStream(b))
		{
			var formatter = new BinaryFormatter();
			stream.Seek(0, SeekOrigin.Begin);
			return (TData)formatter.Deserialize(stream);
		}
	}

	private static string SerializeToString<TData>(TData settings)
	{
		using (var stream = new MemoryStream())
		{
			var formatter = new BinaryFormatter();
			formatter.Serialize(stream, settings);
			stream.Flush();
			stream.Position = 0;
			return Convert.ToBase64String(stream.ToArray());
		}
	}


	#endregion
}
