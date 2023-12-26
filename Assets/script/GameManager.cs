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
    public GameObject countObj;
    public GameObject failTxtObj;
    Text failTxt = null;
    public GameObject card;
    
    float time = 0.0f;
    float maxTime = 60f;
    bool isOpen = false;
    int count = 0;
    int tryCount = 0;
    string cardName = "";
    card card1;
    public bool isCheck = false;
    public delegate IEnumerator OpenDelegate();
    public AudioManager AM;

    private void Awake()
    {
        I = this;
        Time.timeScale = 1;
    }

    // Start is called before the first frame update
    void Start()
    {
        failTxt = failTxtObj.GetComponent<Text>();
        time = maxTime;

        int[] rtan1 = { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
        int[] rtans = new int[(rtan1.Length - 1) * 2];
        
        rtan1 = rtan1.OrderBy(item => Random.Range(-1f, 1f)).ToArray();

        for(int i = 0; i < rtan1.Length - 1; i++)
        {
            rtans[i] = rtan1[i];
            rtans[i + 8] = rtan1[i];
        }
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
        if (time < 0f)
            GameOver();
        else if (time < (maxTime / 4))
        {
            timeTxt.color = new Color(255 / 255, 0, 0);
        }
        if (count > 7)
            StartCoroutine(GameSuccess());
        else
            time -= Time.deltaTime;

        timeTxt.text = time.ToString("N2");
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
            tryCount++;
            if (cardName != _card.cardName)
            {
                failTxtObj.SetActive(true);
                failTxt.text = "½ÇÆÐ!";
                isCheck = true;
                time -= 5f;
                if (time > 0)
                    AM.MatchFail();
                else
                    time = 0f;
                StartCoroutine(card1.CloseCard());
                StartCoroutine(close());
            }
            else
            {
                failTxtObj.SetActive(true);
                AM.Match();
                failTxt.text = "ÆÀ¿ø : " + cardName + "¸ÅÄª ¼º°ø";
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
        isCheck = true;
        countObj.GetComponent<Text>().text = "¸ÅÄª È½¼ö : " + tryCount.ToString();
        countObj.SetActive(true);
        gameOver.SetActive(true);
        AM.GameOver();
        Time.timeScale = 0f;
    }

    public void MatchTxtOff()
    {
        failTxtObj.SetActive(false);
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
