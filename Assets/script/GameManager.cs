using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager I;

    public Text timeTxt;
    public GameObject gameOver;
    public GameObject card;
    
    float time = 0.0f;
    bool isOpen = false;
    int count = 0;
    string cardName = "";
    card card1;
    public bool isCheck = false;
    public delegate IEnumerator OpenDelegate();

    public AudioClip match;
    public AudioSource audioSource;

    private void Awake()
    {
        I = this;
        Time.timeScale = 1;
    }

    // Start is called before the first frame update
    void Start()
    {
        int[] rtans = { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7 };

        rtans = rtans.OrderBy(item => Random.Range(-1f, 1f)).ToArray();
        for (int i = 0; i < 16; i++)
        {
            GameObject newCard = Instantiate(card);
            newCard.transform.parent = GameObject.Find("cards").transform;

            float x = (i / 4) * 1.4f - 2.1f;
            float y = (i % 4) * 1.4f - 3f;
            newCard.transform.position = new Vector3(x, y, 0);

            string rtanName = "rtan" + rtans[i].ToString();
            newCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(rtanName);
            card1 = newCard.GetComponent<card>();
            card1.cardName = rtanName;
        }
        card1 = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (time > 3f)
            GameOver();
        if (count > 7)
            StartCoroutine(GameSuccess());
        else
            time += Time.deltaTime;

        timeTxt.text = time.ToString("N2");
    }

    public IEnumerator CardStart()
    {
        yield return new WaitForSecondsRealtime(4f);


        yield break;
    }

    public void CardOpen(card _card, OpenDelegate close, OpenDelegate success)
    {
        if(!isOpen)
        {
            isOpen = true;
            cardName = _card.cardName;
            card1 = _card;
        }
        else
        {
            isOpen = false;
            if (cardName != _card.cardName)
            {
                isCheck = true;
                StartCoroutine(card1.CloseCard());
                StartCoroutine(close());
            }
            else
            {
                audioSource.PlayOneShot(match);
                StartCoroutine(card1.successCard());
                StartCoroutine(success());
                count++;
            }
        }    
    }
    public IEnumerator GameSuccess()
    {
        yield return new WaitForSecondsRealtime(2f);
        gameOver.SetActive(true);
        Time.timeScale = 0;
        yield break;
    }

    void GameOver()
    {
        gameOver.SetActive(true);
        Time.timeScale = 0;
    }
    public void ReGame()
    {
        adsManager.I.ShowRewardAd();
    }

    public void retryGame()
    {
        SceneManager.LoadScene("MainScene");
    }
}
