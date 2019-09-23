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

//		public static void LoadLevel(int level)
//		{
//			var file = Resources.Load<TextAsset>($"level{level}");
//			Debug.Log(file);
//			if (file != null)
//			{
//				var jsonString = file.text;
//				Debug.Log(jsonString);
//				var config = JsonUtility.FromJson<LevelData>(jsonString);
//
//				Debug.Log(config);
//			}
//		}
//
//		public static void DataSave(int level)
//		{
//			var levelData = new LevelData();
//			List<CellRow> grid = new List<CellRow>();
//
//			CellData sell1 = new CellData
//			{
//				CellState = CellState.HasCoin
//			};
//
//			CellData sell2 = new CellData
//			{
//				CellState = CellState.Blocked
//			};
//
//			List<CellState> cells = new List<CellState>
//			{
//				CellState.HasCoin,
//				CellState.Blocked
//			};
//
//			var list1 = new CellRow
//			{
//				CellsList = cells
//			};
//
//			var list2 = new CellRow
//			{
//				CellsList = cells
//			};
//
//			grid.Add(list1);
//			grid.Add(list2);
//			levelData.InitialGrid = grid;
//			levelData.ExpectedGrid = grid;
//			levelData.MaxStepsAmount = 1;
//			levelData.TimeLimit = 3;
//
//			var jsonString = JsonUtility.ToJson(levelData, true);
//			var dataPath = Path.Combine(Application.dataPath, "Resources", $"level{level}.json");
//			Debug.Log(jsonString);
//			try
//			{
//				using (StreamWriter streamWriter = File.CreateText(dataPath))
//				{
//					Debug.Log(dataPath);
//					streamWriter.Write(jsonString);
//					streamWriter.Close();
//				}
//			}
//			catch (Exception ex)
//			{
//				Debug.LogWarning(ex.Message);
//			}
//		}
    }
}