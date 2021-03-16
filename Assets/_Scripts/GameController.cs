using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GUI = UnityEngine.GUI;

public class GameController : MonoBehaviour
{
    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 100, 30), "Start"))
        {
            SceneManager.LoadScene("Main");
        }
    }
}
