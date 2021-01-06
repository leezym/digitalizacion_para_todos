using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class IdeaForce : MonoBehaviour
{
    public Button button_idea;
    public Sprite sprite_knowmore_normal;
    public Sprite sprite_knowmore_hover;
    public Sprite sprite_knowmore_clicked;

    public TextMeshProUGUI text_idea;

    public TextMeshProUGUI title_idea2;
    public TextMeshProUGUI text_idea2;
    public Image fill_line;

    private bool onStartAnim;

    private void Start()
    {
        fill_line.fillAmount = 0;
        text_idea.alpha = 0;
        if(title_idea2 != null) title_idea2.alpha = 0;
        if (text_idea2 != null) text_idea2.alpha = 0;

        button_idea.GetComponent<Image>().sprite = sprite_knowmore_normal;
        button_idea.onClick.AddListener(StartAnim);
    }

    private void StartAnim()
    {
        if (onStartAnim)
            return;

        onStartAnim = true;

        StartCoroutine(Anim());
    }

    IEnumerator Anim()
    {
        Sequence s = DOTween.Sequence();

        //button_idea.GetComponent<Image>().sprite = sprite_knowmore_hover;
        button_idea.GetComponent<Image>().sprite = sprite_knowmore_clicked;

        s.Append(fill_line.DOFillAmount(1, 0.5f).SetEase(Ease.InOutQuad));

        yield return s.WaitForCompletion(true);

        //button_idea.GetComponent<Image>().sprite = sprite_knowmore_clicked;

        text_idea.DOFade(1, 1.5f).SetEase(Ease.InOutQuad);
        if (title_idea2 != null) title_idea2.DOFade(1, 1.5f).SetEase(Ease.InOutQuad);
        if (text_idea2 != null) text_idea2.DOFade(1, 1.5f).SetEase(Ease.InOutQuad);
    }
}
