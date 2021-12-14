/*
 * LeaderBoard - Handles communication to database
 * Created by : Allan N. Murillo
 * Last Edited : 7/24/2020
 */

using TMPro;
using System;
using System.Linq;
using UnityEngine;
using System.Collections;
using ANM.Framework.Managers;
using System.Collections.Generic;

#pragma warning disable 0618

namespace ANM.Scriptables
{
    [CreateAssetMenu(menuName = "Single Instance/Leaderboard")]
    public class LeaderBoard : ScriptableObject
    {
        private const string UrlWebServer = "http://dreamlo.com/lb/";
        private const string PrivateCode = "F09FVdYmwUSPB_QKcGU8HgT8APnce-lUuyzKNp4B4K3g";


        public static void EnqueueScoreToDatabase(string inputFieldName, int score)
        {
            GameManager.Instance.StartCoroutine(AddScoreToDatabase(inputFieldName, score));
        }

        public static IEnumerator PopulateHighScoreList(TMP_Text scoreList)
        {
            var www = new WWW(UrlWebServer + PrivateCode + "/pipe");
            yield return www;
            if (www.text == string.Empty) yield break;
            scoreList.text = string.Empty;
            IEnumerable<string> myData = ParseNewLineToStringArray(www.text);
            List<ScoreData> myScoreDataList = myData.Select(ParsePipeToScoreData).ToList();
            for (var x = 0; x < myScoreDataList.Count && x < 10; x++)
            {
                scoreList.text += myScoreDataList[x].GetParsedString();
            }
        }

        private static IEnumerator AddScoreToDatabase(string playerName, int score)
        {
            var www = new WWW(UrlWebServer + PrivateCode + "/add-pipe/" +
                              WWW.EscapeURL(playerName) + "/" + score + "/" + DateTime.Now.ToString("HH"));
            yield return www;
        }

        private static IEnumerable<string> ParseNewLineToStringArray(string dataArray)
        {
            string[] rows;
            switch (dataArray)
            {
                case null:
                case "":
                    return null;

                default:
                    rows = dataArray.Split(new[] {'\n'}, StringSplitOptions.RemoveEmptyEntries);
                    break;
            }

            return rows;
        }

        private static ScoreData ParsePipeToScoreData(string stringData)
        {
            var temp = new ScoreData();
            string[] rows = stringData.Split(new[] {'|'}, StringSplitOptions.RemoveEmptyEntries);
            for (var x = 0; x < rows.Length; x++)
            {
                switch (x)
                {
                    case 0:
                        temp.playerName = rows[x];
                        break;
                    case 1:
                        temp.score = ParseIntFromString(rows[x]);
                        break;
                    case 3:
                        temp.date = rows[x];
                        break;
                    case 4:
                        temp.index = ParseIntFromString(rows[x]);
                        break;
                }
            }

            return temp;
        }

        private static int ParseIntFromString(string s)
        {
            int.TryParse(s, out var x);
            return x;
        }

        private struct ScoreData
        {
            public int index;
            public int score;
            public string playerName;
            public string date;

            public string GetParsedString()
            {
                var temp = index + 1 + " : " + playerName + " - " + score + " | " + date + "\n";
                return temp;
            }
        }
    }
}
