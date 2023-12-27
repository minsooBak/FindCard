using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public static GameManager I;
    public GameObject cam;
    public Text timeTxt;
    public GameObject gameOver;
    public GameObject countObj;
    public GameObject failTxtObj;
    Text failTxt = null;
    public GameObject card;
    public int cardCount = 8;
    public int stage = 1;
    float time = 0.0f;
    float maxTime = 60f;
    float selectTime = 5f;
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

        int[] rtan1 = null;
        if (cam == null)
            Debug.Log("CameraNull");
        if (!PlayerPrefs.HasKey("Stage"))
            Debug.Log("StageKey Null");
        stage = PlayerPrefs.GetInt("Stage");
        switch (stage)
        {
            case 1:
                {
                    cam.GetComponent<Camera>().orthographicSize = 5;
                    maxTime = 60f;
                    rtan1 = new int[cardCount];
                    for (int i = 0; i < cardCount; i++)
                    {
                        rtan1[i] = i;
                    }
                    break;
                }
            case 2:
                {
                    cam.GetComponent<Camera>().orthographicSize = 6;
                    maxTime = 120f;
                    rtan1 = new int[cardCount + 7];
                    for (int i = 0; i < cardCount + 7; i++)
                    {
                        rtan1[i] = i;
                    }
                    cardCount += 7;
                        break;
                }
        }
        time = maxTime;

        //int[] rtan1 = { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
        int[] rtans = new int[rtan1.Length * 2];
        
        //rtan1 = rtan1.OrderBy(item => Random.Range(-1f, 1f)).ToArray();

        for (int i = 0; i < rtan1.Length; i++)
        {
            rtans[i] = rtan1[i];
            rtans[i + cardCount] = rtan1[i];
        }

        rtan1 = ShuffleList(rtan1.ToList());
        rtans = ShuffleList(rtans.ToList());

        for (int i = 0; i < rtans.Length; i++)
        {
            GameObject newCard = Instantiate(card);
            newCard.transform.parent = GameObject.Find("cards").transform;
            float x = 0, y = 0;
            switch (stage)
            {
                case 1:
                    {
                        x = (i / 4) * 1.4f - 2.1f;
                        y = (i % 4) * 1.4f - 3f;
                        break;
                    }
                case 2:
                    {
                        x = (i / 6) * 1.4f - 2.8f;
                        y = (i % 6) * 1.4f - 4.5f;
                        break;
                    }
            }

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
        if(isOpen)
        {
            selectTime -= Time.deltaTime;
            if(selectTime < 0)
            {
                isOpen = false;
                StartCoroutine(card1.CloseCard());
                selectTime = 5f;
            }
        }

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
    
    T[] ShuffleList<T>(List<T> list)
    {
        int r1, r2;

        T tmp;

        for(int i = 0; i < list.Count; i++)
        {
            r1 = Random.Range(0, list.Count);
            r2 = Random.Range(0, list.Count);

            tmp = list[r1];
            list[r1] = list[r2];
            list[r2] = tmp;
        }
        return list.ToArray();
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
            selectTime = 5f;
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
        PlayerPrefs.SetInt("Stage1Clear", 1);
        Time.timeScale = 0;
        yield break;
    }

    void GameOver()
    {
        isCheck = true;
        int score = ((int)time * 15) + (count * 10) - (count * 5);
        if (score < 0)
            score = 0;
        countObj.GetComponent<Text>().text = "¸ÅÄª È½¼ö : " + tryCount.ToString() + "\nÁ¡¼ö : " + score.ToString();
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
       SceneManager.LoadScene("StartScene");
    }
}
