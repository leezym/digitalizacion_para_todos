using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlusInfoOnClick : MonoBehaviour
{
    public Button btn_plus;
    public CanvasGroup group;
    public CanvasGroup groupOtherPage;
    public GameObject hoverClick;
    public PlusInfoOnClick nextPlusInfo;

    public int page;
    public bool enableInfo;
    public bool disableButtonOnCLick = true;
    public bool changeSpriteButtonOnClick = true;

    private Sprite spr_plus_normal;
    private Sprite spr_plus_hover;
    private Sprite spr_plus_clicked;

    bool isChecked;

    [Space(10)]

    public UnityEvent OnClick;

    void Start()
    {
        spr_plus_normal = Resources.Load<Sprite>("button_plus_normal");
        spr_plus_hover = Resources.Load<Sprite>("button_plus_hover");
        spr_plus_clicked = Resources.Load<Sprite>("button_plus_clicked");

        btn_plus.interactable = false;
        
        if (hoverClick != null)
        {
            hoverClick.SetActive(false);
        }

        group.alpha = 0;
        group.interactable = false;
        group.blocksRaycasts = false;

        if(groupOtherPage != null)
        {
            groupOtherPage.alpha = 0;
            groupOtherPage.interactable = false;
            groupOtherPage.blocksRaycasts = false;
        }

        btn_plus.onClick.AddListener(ActiveOnButtonPressed);
    }

    private void OnEnable()
    {
        BookManager.OnFlipPage += OnFlipPage;
    }

    private void OnDisable()
    {
        BookManager.OnFlipPage -= OnFlipPage;
    }

    public void OnFlipPage(bool start, int page)
    {
        if (!isChecked)
        {
            if (this.page == page)
            {
                if (enableInfo)
                {
                    EnableButton();
                }
            }
            else
            {
                if (hoverClick != null)
                {
                    StopAllCoroutines();
                    hoverClick.SetActive(false);
                }
            }
        }
    }
    
    public void EnableButton()
    {
        enableInfo = true;

        if (hoverClick != null)
        {
            StartCoroutine(Loop());
        }
        btn_plus.interactable = true;
    }

    public void ActiveOnButtonPressed()
    {
        isChecked = true;
        
        if (hoverClick != null)
        {
            StopAllCoroutines();
            hoverClick.gameObject.SetActive(false);
        }

        if (changeSpriteButtonOnClick)
            btn_plus.GetComponent<Image>().sprite = spr_plus_clicked;

        if (disableButtonOnCLick)
            btn_plus.interactable = false;

        group.alpha = 1;
        group.interactable = true;
        group.blocksRaycasts = true;

        if (groupOtherPage != null)
        {
            groupOtherPage.alpha = 1;
            groupOtherPage.interactable = true;
            groupOtherPage.blocksRaycasts = true;
        }

        if (nextPlusInfo != null)
        {
            nextPlusInfo.EnableButton();
        }

        if (OnClick != null)
        {
            OnClick.Invoke();
        }
    }

    public void InactiveOnButtonPressed()
    {
        group.alpha = 0;
        group.interactable = false;
        group.blocksRaycasts = false;

        if (groupOtherPage != null)
        {
            groupOtherPage.alpha = 0;
            groupOtherPage.interactable = false;
            groupOtherPage.blocksRaycasts = false;
        }
    }

    private IEnumerator Loop()
    {
        hoverClick.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        hoverClick.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(Loop());
    }
}
