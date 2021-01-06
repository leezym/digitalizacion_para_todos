using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnimOnFlipPage : MonoBehaviour
{
    public int pageToAnim;

    [Space(10)]
    public bool animScale;
    public float waitTimeG1;
    public GameObject[] group1;

    [Space(10)]
    public bool animTextAlpha;
    public float waitTimeG2;
    public TextMeshProUGUI[] group2;

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
        if (!start)
        {
            if (page == pageToAnim)
            {
                if (animScale)
                {
                    StartCoroutine(StartAnimScale());
                }

                if (animTextAlpha)
                {
                    StartCoroutine(StartAnimAlpha());
                }
            }
            else if(page == pageToAnim - 1 || page == pageToAnim + 1)
            {
                StopAllCoroutines();
                DOTween.PauseAll();

                foreach(GameObject component in group1)
                {
                    if (animScale)
                    {
                        component.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
                    }
                }
                
                foreach (TextMeshProUGUI component in group2)
                {
                    if (animScale)
                    {
                        component.alpha = 0;
                    }
                }
            }
        }
        else
        {

        }
    }

    private IEnumerator StartAnimScale()
    {
        yield return new WaitForSeconds(waitTimeG1);

        Sequence s = DOTween.Sequence();

        for (int i = 0; i < group1.Length; i++)
        {
            if (animScale)
            {
                s.Append(group1[i].transform.DOScale(new Vector3(1, 1, 1), 0.5f).SetEase(Ease.InOutQuad));
                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    private IEnumerator StartAnimAlpha()
    {
        yield return new WaitForSeconds(waitTimeG2);

        Sequence s = DOTween.Sequence();

        for (int i = 0; i < group2.Length; i++)
        {
            if (animTextAlpha)
            {
                s.Append(group2[i].DOFade(1, 0.5f).SetEase(Ease.InOutQuad));
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}
