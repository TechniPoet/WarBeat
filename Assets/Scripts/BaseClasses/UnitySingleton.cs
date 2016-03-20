using UnityEngine;
using System.Collections;


/// <summary>
/// Class for creating monobehavior singletons.
/// Any new singletons should be added to MonoApplicationUtil.DestroyKnownSingletons.
/// Most code taken from: https://gist.github.com/mlhamlin/ed99ec0d75d2a3f2d436
/// </summary>
public class UnitySingleton<T> : MonoBehaviour
	where T : Component
{
	public static string objName;
	private static T instance;
	private static bool applicationIsQuitting = false;


	public static T Instance
	{
		get
		{
			if (applicationIsQuitting)
			{
				Debug.LogWarning("Application is quitting cannot access Singleton '" + typeof(T) + "'.");
				return null;
			}

			if (instance == null)
			{

				T[] objs = FindObjectsOfType<T>();

				if (objs.Length > 0)
				{
					instance = objs[0];
				}

				if (objs.Length > 1)
				{
					Debug.LogError("There is more than one " + typeof(T).Name + " in the scene.");
				}

				if (instance == null)
				{
					GameObject obj = new GameObject();
					obj.hideFlags = HideFlags.DontSave;
					instance = obj.AddComponent<T>();
				}
			}
			return instance;
		}
	}
	

	public void OnApplicationQuit()
	{
		applicationIsQuitting = true;
	}
}