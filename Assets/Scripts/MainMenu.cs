using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
  public void PlayGame()
  {
    Debug.Log("Play Game");
  }

  public void QuitGame()
  {
    Application.Quit();
  }
}