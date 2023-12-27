using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    public GameObject stage;
    public Button button;
    GameObject image;
    bool clear = false;
    // Start is called before the first frame update
    void Start()
    {
        image = button.transform.Find("Image").gameObject;
        clear = PlayerPrefs.HasKey("Stage1Clear");
        if (clear)
        {
            image.SetActive(false);
            button.transform.Find("Stage2").GetComponent<Text>().color = new Color(0, 0, 0, 255);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!clear)
            button.interactable = false;
        else
            button.interactable = true;
    }

    public void GoStage1()
    {
        PlayerPrefs.SetInt("Stage", 1);
        SceneManager.LoadScene("MainScene");
    }
    public void GoStage2()
    {
        PlayerPrefs.SetInt("Stage", 2);
        SceneManager.LoadScene("MainScene");
    }
    public void Exit()
    {
        stage.SetActive(false);
    }
}
