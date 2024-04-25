using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
  public PlayableDirector CamDirector;
  public PlayableDirector MenuDirector;
  public PlayableDirector SettingsDirector;
  public void PlayGame()
  {
    CamDirector.Play();
    MenuDirector.Play();
    Debug.Log("Play Game");
  }
  public void Settings()
  {
    MenuDirector.Play();
    SettingsDirector.Play();
    Debug.Log("Settings");
  }
  public void QuitGame()
  {
    Application.Quit();
  }
}