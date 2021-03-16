using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlPanelController : MonoBehaviour
{
    public RectTransform rectTransform;

    public Vector2 offScreenPosition;
    public Vector2 onScreenPosition;

    [Range(0.1f, 10.0f)] 
    public float speed = 1.0f;
    public float timer = 0.0f;
    public bool isOnScreen = false;

    [Header("Player Settings")]
    public PlayerBehaviour player;
    public CameraController playerCamera;

    public Pauseable pausable;

    [Header("Scene Data")]
    public SceneDataSO sceneData;

    public GameObject gameStateElement;

    // Start is called before the first frame update
    void Start()
    {
        pausable = FindObjectOfType<Pauseable>();
        player = FindObjectOfType<PlayerBehaviour>();
        playerCamera = FindObjectOfType<CameraController>();
        rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = offScreenPosition;
        timer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Tab))
        //{
        //    ToggleControlPanel();
        //}

        if (isOnScreen)
        {
            MoveControlPanelDown();
        }
        else
        {
            MoveControlPanelUp();
        }

        gameStateElement.SetActive(pausable.isGamePaused);
    }

    void ToggleControlPanel()
    {
        isOnScreen = !isOnScreen;
        timer = 0.0f;

        if (isOnScreen)
        {
            //Cursor.lockState = CursorLockMode.None;
            playerCamera.enabled = false;
        }
        else
        {

            //Cursor.lockState = CursorLockMode.Locked;
            playerCamera.enabled = true;
        }
    }

    private void MoveControlPanelDown()
    {
        rectTransform.anchoredPosition = Vector2.Lerp(offScreenPosition, onScreenPosition, timer);
        if (timer < 1.0f)
        {
            timer += Time.deltaTime * speed;
        }
    }

    private void MoveControlPanelUp()
    {
        rectTransform.anchoredPosition = Vector2.Lerp(onScreenPosition, offScreenPosition, timer);
        if (timer < 1.0f)
        {
            timer += Time.deltaTime * speed;
        }

        if (pausable.isGamePaused)
        {
           pausable.TogglePause();
        }
    }

    public void OnControlButtonPressed()
    {
        ToggleControlPanel();
    }

    public void OnLoadButtonPressed()
    {
        player.controller.enabled = false;
        player.transform.position = sceneData.playerPosition;
        player.controller.enabled = true;

        player.health = sceneData.playerHealth;
        player.healthBar.SetHealth(sceneData.playerHealth);
    }

    public void OnSaveButtonPressed()
    {
        sceneData.playerPosition = player.transform.position;
        sceneData.playerHealth = player.health;
    }
}
