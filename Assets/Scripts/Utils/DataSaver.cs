using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ProjectName.Utils
{
    public sealed class DataSaver
    {
        public static T LoadData<T>(string relativePath) where T : class
		{
			var file = Resources.Load<TextAsset>(relativePath);
			if (file != null)
			{
				var jsonString = file.text;

				Debug.Log(jsonString);
				var data = JsonUtility.FromJson<T>(jsonString);

				return data;
			}
			else
			{
				Debug.LogWarning($"file not found, path: {relativePath}");
			}

			return null;
		}

		public static void SaveData<T>(T data, string path)
		{
			var jsonString = JsonUtility.ToJson(data, true);
			var dataPath = Path.Combine(Application.dataPath, path);
			Debug.Log(jsonString);
			try
			{
				using (StreamWriter streamWriter = File.CreateText(dataPath))
				{
					Debug.Log(dataPath);
					streamWriter.Write(jsonString);
					streamWriter.Close();
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning(ex.Message);
			}
		}

		public static LevelData LoadLevel(int level)
		{
			var file = Resources.Load<TextAsset>($"level{level}");
			Debug.Log(file);
			if (file != null)
			{
				var jsonString = file.text;
				Debug.Log(jsonString);
				var config = JsonUtility.FromJson<LevelData>(jsonString);
				
				Debug.Log(config);
				return config;
			}
			
			return null;
		}

		public static void DataSave(int level, LevelData data)
		{
			data.TimeLimit = 30;
			var jsonString = JsonUtility.ToJson(data, true);
			var dataPath = Path.Combine(Application.dataPath, "Resources", $"level{level}.json");
			Debug.Log(jsonString);
			try
			{
				using (StreamWriter streamWriter = File.CreateText(dataPath))
				{
					Debug.Log(dataPath);
					streamWriter.Write(jsonString);
					streamWriter.Close();
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning(ex.Message);
			}
		}
    }
}