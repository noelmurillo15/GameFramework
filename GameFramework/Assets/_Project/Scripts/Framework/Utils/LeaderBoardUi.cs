/*
 * LeaderBoardUi - Handles viewing / editing high scores in scene
 * Created by : Allan N. Murillo
 * Last Edited : 8/5/2020
 */

using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ANM.Scriptables;
using ANM.Framework.Managers;
using ANM.Framework.Extensions;

namespace ANM.Ui
{
    public class LeaderBoardUi : MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputField = null;
        [SerializeField] private TMP_Text currentScore = null;
        [SerializeField] private Button addHighScoreButton = null;
        [SerializeField] private TMP_Text highScores = null;
        private int _score;


        private void Awake()
        {
            _score = GameManager.Instance.GetScore();
            addHighScoreButton.interactable = false;
            currentScore.text = "Final Score : " + _score;
            StartCoroutine(LeaderBoard.PopulateHighScoreList(highScores));
        }

        private void LateUpdate()
        {
            if (_score <= 0) return;
            addHighScoreButton.interactable = inputField.text.Length >= 3;
        }

        public void AddHighScoreBtn()
        {
            LeaderBoard.EnqueueScoreToDatabase(inputField.text, _score);
            addHighScoreButton.interactable = false;
            inputField.text = string.Empty;
            _score = 0;
            currentScore.text = "Final Score : " + _score;
        }

        public void PlayAgainBtn()
        {
            GameManager.Instance.SoftReset();
            StartCoroutine(SceneExtension.ForceMenuSceneSequence(true));
        }
    }
}
