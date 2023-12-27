using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class card : MonoBehaviour
{
    public Animator anim;
    public string cardName = "";
    Button btn;

    public AudioClip flip;
    public AudioSource audioSource;
    SpriteRenderer cardBack;
    Canvas canvas;
    // Start is called before the first frame update
    void Start()
    {
        btn = GetComponent<Button>();
        cardBack = transform.Find("back").GetComponent<SpriteRenderer>();
        cardBack.color = new Color(255 / 255, 255 / 255, 255 / 255);
        canvas = transform.Find("back").transform.Find("Canvas").GetComponent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.I.isCheck)
        {
            btn.interactable = false;
        }else
            btn.interactable = true;

    }
    public void OpenCard()
    {
        audioSource.PlayOneShot(flip);
        anim.SetBool("isOpen", true);
        GameManager.I.CardOpen(this, CloseCard, successCard);
    }

    public IEnumerator CloseCard()
    {
        yield return new WaitForSecondsRealtime(1f);
        anim.SetBool("isOpen", false);
        cardBack.color = new Color(211 / 255f, 211 / 255f, 211 / 255f);
        GameManager.I.isCheck = false;
        GameManager.I.MatchTxtOff();
        yield break;
    }

    public IEnumerator successCard()
    {
        yield return new WaitForSecondsRealtime(1f);
        anim.SetBool("isSuccess", true);
        GameManager.I.MatchTxtOff();
        Destroy(gameObject ,1.5f);
        yield break;
    }

    public void CardCheck()
    {
        if(anim.GetBool("isOpen"))
        {
            cardBack.sortingOrder = 1;
            canvas.sortingOrder = 1;
        }
        else
        {
            cardBack.sortingOrder = 3;
            canvas.sortingOrder = 4;
        }
    }
}
