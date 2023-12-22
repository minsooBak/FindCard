using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class card : MonoBehaviour
{
    public Animator anim;
    public string cardName = "";
    Button btn;

    // Start is called before the first frame update
    void Start()
    {
        btn = GetComponent<Button>();
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
        anim.SetBool("isOpen", true);
        transform.Find("front").gameObject.SetActive(true);
        transform.Find("back").gameObject.SetActive(false);
        GameManager.I.CardOpen(this, CloseCard, successCard);
    }

    public IEnumerator CloseCard()
    {
        yield return new WaitForSecondsRealtime(2f);
        anim.SetBool("isOpen", false);
        transform.Find("front").gameObject.SetActive(false);
        transform.Find("back").gameObject.SetActive(true);
        GameManager.I.isCheck = false;
        yield break;
    }

    public IEnumerator successCard()
    {
        yield return new WaitForSecondsRealtime(1f);
        anim.SetBool("isSuccess", true);
        Destroy(this,1.5f);
        yield break;
    }
}
