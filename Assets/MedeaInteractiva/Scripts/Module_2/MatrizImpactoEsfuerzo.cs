using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class MatrizImpactoEsfuerzo : MonoBehaviour
{
    public Button[] buttons = new Button[4];
    public CanvasGroup[] group = new CanvasGroup[4];
    public GameObject[] hoverClick = new GameObject[4];

    public int page;

    int current = 0;
    bool isTutorial = true;

    void Start()
    {
        foreach(Button component in buttons)
        {
            component.interactable = false;
        }

        for(int i = 0; i < 4; i++)
        {
            hoverClick[i].SetActive(false);
            group[i].alpha = 0;
            group[i].interactable = false;
            group[i].blocksRaycasts = false;
        }
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
        if (isTutorial)
        {
            if (this.page == page)
            {
                EnableButton();
            }
            else
            {
                StopAllCoroutines();
                for (int i = 0; i < 4; i++)
                {
                    hoverClick[i].SetActive(false);
                }
            }
        }
        else
        {
            foreach (Button component in buttons)
            {
                component.interactable = true;
            }
        }
    }

    public void EnableButton()
    {
        StartCoroutine(Loop());
        buttons[current].interactable = true;
    }

    public void ActiveOnButtonPressed(int index)
    {
        foreach (CanvasGroup component in group)
        {
            component.alpha = 0;
            component.interactable = false;
            component.blocksRaycasts = false;
        }

        if (isTutorial)
        {
            StopAllCoroutines();
            for (int i = 0; i < 4; i++)
            {
                hoverClick[i].gameObject.SetActive(false);
            }

            foreach (Button component in buttons)
            {
                component.interactable = false;
            }
        }

        if (!isTutorial)
        {
            current = index;
        }
        group[current].alpha = 1;
        group[current].interactable = true;
        group[current].blocksRaycasts = true;

        if (isTutorial)
        {
            if (current < 3)
            {
                current++;
                EnableButton();
            }
            else
            {
                foreach (Button component in buttons)
                {
                    component.interactable = true;
                }
                BookManager.Instance.OnPlusInfoChecked();
                isTutorial = false;
            }
        }
    }

    private IEnumerator Loop()
    {
        hoverClick[current].SetActive(true);
        yield return new WaitForSeconds(0.5f);
        hoverClick[current].SetActive(false);
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(Loop());
    }
}
