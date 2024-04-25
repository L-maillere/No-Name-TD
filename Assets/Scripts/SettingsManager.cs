using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SettingsManager : MonoBehaviour
{
  public bool isMusicOn = true;
  public TMP_Text musicButtonText;
  public TMP_Text difficultyButtonText;
  public DifficultyLevel currentDifficulty = DifficultyLevel.Normal;
  public enum DifficultyLevel
  {
    Easy,
    Normal,
    Hard
  }

  public void ToggleMusic()
  {
    isMusicOn = !isMusicOn;
    musicButtonText.text = "Music : " + (isMusicOn ? "Yes" : "No");
  }
  public void ToggleDifficulty()
  {
    switch (currentDifficulty)
    {
      case DifficultyLevel.Easy:
        currentDifficulty = DifficultyLevel.Normal;
        difficultyButtonText.text = "Difficulty : Normal";
        break;
      case DifficultyLevel.Normal:
        currentDifficulty = DifficultyLevel.Hard;
        difficultyButtonText.text = "Difficulty : Hard";
        break;
      case DifficultyLevel.Hard:
        currentDifficulty = DifficultyLevel.Easy;
        difficultyButtonText.text = "Difficulty : Easy";
        break;
    }
  }
}