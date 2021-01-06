using BookCurlPro;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BookManager : UIContent
{
    private static BookManager instance;
    public static BookManager Instance { get => instance; set => instance = value; }

    public delegate void FlipPage(bool start, int page);
    public static event FlipPage OnFlipPage;

    [DllImport("__Internal")]
    private static extern void OpenNewUrl(string url);

    [Header("Config")]
    public float PageFlipTime = 0.1f;
    public string video_url;
    private int currentCover;
    private float refPageFlipTime;

    private bool audioIsPlaying;
    private bool isActivity;
    private bool tuto1;
    private bool tuto2;

    [SerializeField]
    private bool isAdmin;
    [SerializeField]
    private bool isChilean;

    Image audioOff;

    public int currentModule = 1;

    public int preguntaNumber;

    #region UNITY CALLBACK
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        GlobalData.CurrentModule = currentModule;
    }

    void Start()
    {
        refPageFlipTime = PageFlipTime;
        btn_next.onClick.AddListener(NextPage);
        btn_prev.onClick.AddListener(PrevPage);

        panelAnimation.SetActive(true);
    }

    private void OnEnable()
    {
        OnFlipPage += onFlipPage;
    }

    private void OnDisable()
    {
        OnFlipPage -= onFlipPage;
    }
    #endregion UNITY
    
    #region BOOK
    public void NextPage()
    {
        feedBackConnection.SetActive(false);

        //PageFlipTime = 0.5f;
        //book.GetComponent<AutoFlip2>().PageFlipTime = PageFlipTime;

        if (!isAdmin)
        {
            #region ------------------------------------------------------START MODULO 1---------------------------------------------------------
            if (book.CurrentPaper == 2)
            {
                if (!tuto1 && !rightHotSpot.raycastTarget)
                {
                    return;
                }
            }
            else if (book.CurrentPaper == 3)
            {
                if (!tuto2 && !rightHotSpot.raycastTarget)
                {
                    return;
                }
            }
            else if (book.CurrentPaper == 8)
            {
                if (string.IsNullOrEmpty(GlobalData.PAD_Identifica) ||
                    string.IsNullOrEmpty(GlobalData.PAD_DefinePropositos) ||
                    string.IsNullOrEmpty(GlobalData.PAD_IdentificaYSenala) ||
                    string.IsNullOrEmpty(GlobalData.PAD_EstableceIndicadores))
                {
                    return;
                }
            }
            else if (book.CurrentPaper == 9)
            {
                if (string.IsNullOrEmpty(GlobalData.Objetivo_curso))
                {
                    return;
                }
            }
            else if (book.CurrentPaper == 10)
            {
                if (string.IsNullOrEmpty(GlobalData.Reflexion))
                {
                    isActivity = true;
                    return;
                }
            }
            else if (book.CurrentPaper == 11)
            {
                if (GlobalData.Estado != "Completado")
                {
                    return;
                }
            }
            #endregion ----------------------------------------------------END MODULO 1----------------------------------------------------------
            #region ------------------------------------------------------START MODULO 2---------------------------------------------------------
            if (!isChilean)
            {
                if (book.CurrentPaper == 13)
                {
                    if (!GlobalData.LineaDeTiempoCMVCompletado)
                    {
                        return;
                    }
                }
                else if (book.CurrentPaper == 14)
                {
                    if (!GlobalData.SignificadoDeTerminoCompletado)
                    {
                        return;
                    }
                }
                else if (book.CurrentPaper == 20)
                {
                    if (!GlobalData.MatrizImpactoEsfuerzoCompletado)
                    {
                        return;
                    }
                }
                else if (book.CurrentPaper == 22)
                {
                    if (!GlobalData.MindFulnessCompletado)
                    {
                        return;
                    }
                }
                else if (book.CurrentPaper == 23)
                {
                    if (!GlobalData.EjercicioMatrizCompletado)
                    {
                        return;
                    }
                }
                else if (book.CurrentPaper == 24)
                {
                    if (string.IsNullOrEmpty(GlobalData.VUCA_Reflexion))
                    {
                        return;
                    }
                }
            }
            #endregion ----------------------------------------------------END MODULO 2----------------------------------------------------------
            #region ------------------------------------------------------START MODULO 3---------------------------------------------------------
            if (book.CurrentPaper == 26)
            {
                if (!GlobalData.EncuestaM3Completada)
                {
                    return;
                }
            }
            else if (book.CurrentPaper == 27)
            {
                if (!GlobalData.PergaminoVisto)
                {
                    return;
                }
            }
            else if (book.CurrentPaper == 41)
            {
                if (!GlobalData.SesgoCompletado)
                {
                    return;
                }
            }
            else if (book.CurrentPaper == 49)
            {
                if (!GlobalData.EjercicioM3Completado)
                {
                    return;
                }
            }
            else if (book.CurrentPaper == 50)
            {
                if (!GlobalData.ReflexionM3Completada)
                {
                    return;
                }
            }
            #endregion ----------------------------------------------------END MODULO 3----------------------------------------------------------
            #region ------------------------------------------------------START MODULO 4---------------------------------------------------------
            if (book.CurrentPaper == 63)
            {
                if (!GlobalData.EjercicioM4Completado)
                {
                    return;
                }
            }
            else if (book.CurrentPaper == 64)
            {
                if (!GlobalData.ReflexionM4Completada)
                {
                    return;
                }
            }
            #endregion ----------------------------------------------------END MODULO 4----------------------------------------------------------
            #region ------------------------------------------------------START MODULO 5---------------------------------------------------------
            if (book.CurrentPaper == 85)
            {
                if (!GlobalData.EjercicioM5Completado)
                {
                    return;
                }
            }
            else if (book.CurrentPaper == 86)
            {
                if (!GlobalData.ReflexionM5Completada)
                {
                    return;
                }
            }
            #endregion ----------------------------------------------------END MODULO 5----------------------------------------------------------
        }

        book.GetComponent<AutoFlip2>().FlipRightPage();
        btn_next.interactable = false;
    }

    public void PrevPage()
    {
        feedBackConnection.SetActive(false);
        PageFlipTime = 0.5f;
        book.GetComponent<AutoFlip2>().PageFlipTime = PageFlipTime;
        book.GetComponent<AutoFlip2>().FlipLeftPage();
        btn_prev.interactable = false;
    }

    public void OnChangePage()
    {
        feedBackConnection.SetActive(false);
        pergaminoAnim.SetActive(false);

        OnFlipPage(false, book.CurrentPaper);

        switch(book.currentPaper)
        {
            case 11:
                StartCoroutine(DatabaseManager.Instance.CheckStatus(1));    
                break;
            case 25:
                StartCoroutine(DatabaseManager.Instance.CheckStatus(2));
                break;
            case 51:
                StartCoroutine(DatabaseManager.Instance.CheckStatus(3));
                break;
            case 65:
                StartCoroutine(DatabaseManager.Instance.CheckStatus(4));
                break;
            case 87:
                StartCoroutine(DatabaseManager.Instance.CheckStatus(5));
                break;
        }

        #region ------------------------------------------------------START MODULO 1---------------------------------------------------------
        if (book.CurrentPaper == 0)
        {
            btn_changeCover.SetActive(true);
        }
        else if (book.CurrentPaper == 1)
        {
            shadowLeft.SetActive(true);
        }
        else if (book.CurrentPaper == 2)
        {
            GlobalData.indexModule = 1;

            if (GlobalData.Estado == "Sin iniciar")
            {
                GlobalData.Estado = "Iniciado";

                StartCoroutine(DatabaseManager.Instance.UpdateData());
            }

            if (string.IsNullOrEmpty(GlobalData.Objetivo_curso) && !tuto1)
            {
                tutorial1.GetComponent<Tutorial>().EnterTutorial(true);
                tuto1 = true;
                rightHotSpot.raycastTarget = false;
            }
        }
        else if (book.CurrentPaper == 3)
        {
            if (string.IsNullOrEmpty(GlobalData.Objetivo_curso) && !tuto2)
            {
                tutorial2.GetComponent<Tutorial>().EnterTutorial(true);
                tuto2 = true;
                rightHotSpot.raycastTarget = false;
            }
        }
        else if (book.CurrentPaper == 4)
        {
            rightHotSpot.raycastTarget = true;
        }
        else if (book.CurrentPaper == 5)
        {
            rightHotSpot.raycastTarget = true;
        }
        else if (book.CurrentPaper == 6)
        {
            rightHotSpot.raycastTarget = true;
        }
        else if (book.CurrentPaper == 7)
        {
            rightHotSpot.raycastTarget = true;
        }
        else if (book.CurrentPaper == 8)
        {
            if (string.IsNullOrEmpty(GlobalData.PAD_Identifica) ||
                string.IsNullOrEmpty(GlobalData.PAD_DefinePropositos) ||
                string.IsNullOrEmpty(GlobalData.PAD_IdentificaYSenala) ||
                string.IsNullOrEmpty(GlobalData.PAD_EstableceIndicadores))
            {
                rightHotSpot.raycastTarget = false;
            }
            else
            {
                rightHotSpot.raycastTarget = true;
            }
        }
        else if (book.CurrentPaper == 9)
        {
            StopCoroutine(EnableBtnReflexion());
            animator_reflexion.Rebind();
            animator_reflexion.enabled = false;
            audioMeditation.gameObject.SetActive(false);
            //currentPopup = 3;
            if (string.IsNullOrEmpty(GlobalData.Objetivo_curso))
            {
                rightHotSpot.raycastTarget = false;
            }
            else
            {
                rightHotSpot.raycastTarget = true;
            }
        }
        else if (book.CurrentPaper == 10)
        {
            audioOff = audioOff1;
            if (string.IsNullOrEmpty(GlobalData.Reflexion))
            {
                rightHotSpot.raycastTarget = false;
                popupReflexion.SetActive(true);
            }
            else
            {
                popupReflexion.SetActive(false);
                rightHotSpot.raycastTarget = true;
                AnimReflexion();
            }
        }
        else if (book.CurrentPaper == 11)
        {
            GlobalData.indexModule = 1;
            StopCoroutine(EnableBtnReflexion());
            animator_reflexion.Rebind();
            animator_reflexion.enabled = false;
            audioMeditation.gameObject.SetActive(false);

            if (GlobalData.Estado == "Completado")
            {
                GlobalData.CurrentModule = 2;
                currentModule = 2;
                btn_topbar[1].interactable = true;
                rightHotSpot.raycastTarget = true;
            }
            else
            {
                rightHotSpot.raycastTarget = false;
            }
        }
        #endregion ----------------------------------------------------END MODULO 1----------------------------------------------------------
        #region ------------------------------------------------------START MODULO 2---------------------------------------------------------
        else if (book.CurrentPaper == 12)
        {
            popupReflexion.SetActive(false);
            rightHotSpot.raycastTarget = true;

            GlobalData.indexModule = 2;

            if (GlobalData.EstadoM2 == "Sin iniciar")
            {
                GlobalData.EstadoM2 = "Iniciado";

                StartCoroutine(DatabaseManager.Instance.UpdateData());
            }
        }
        else if (book.CurrentPaper == 13)
        {
            if (!GlobalData.LineaDeTiempoCMVCompletado)
            {
                rightHotSpot.raycastTarget = false;
            }
            else
            {
                rightHotSpot.raycastTarget = true;
            }
        }
        else if (book.CurrentPaper == 14)
        {
            if (!GlobalData.SignificadoDeTerminoCompletado)
            {
                rightHotSpot.raycastTarget = false;
            }
            else
            {
                rightHotSpot.raycastTarget = true;
            }
        }
        else if (book.CurrentPaper == 19)
        {
            rightHotSpot.raycastTarget = true;
        }
        else if (book.CurrentPaper == 20)
        {
            if (!GlobalData.MatrizImpactoEsfuerzoCompletado)
            {
                rightHotSpot.raycastTarget = false;
            }
            else
            {
                rightHotSpot.raycastTarget = true;
            }
        }
        else if (book.CurrentPaper == 22)
        {
            if (!GlobalData.MindFulnessCompletado)
            {
                rightHotSpot.raycastTarget = false;
            }
            else
            {
                rightHotSpot.raycastTarget = true;
            }
        }
        else if (book.CurrentPaper == 23)
        {
            audioMeditation.gameObject.SetActive(false);
            if (!GlobalData.EjercicioMatrizCompletado)
            {
                rightHotSpot.raycastTarget = false;
            }
            else
            {
                rightHotSpot.raycastTarget = true;
            }
        }
        else if (book.CurrentPaper == 24)
        {
            audioOff = audioOff2;
            audioIsPlaying = true;
            audioMeditation.gameObject.SetActive(true);

            if (string.IsNullOrEmpty(GlobalData.VUCA_Reflexion))
            {
                rightHotSpot.raycastTarget = false;
                popupReflexionVUCA.SetActive(true);
            }
            else
            {
                rightHotSpot.raycastTarget = true;
                popupReflexionVUCA.SetActive(false);
                AudioPlay();
            }
        }
        else if (book.CurrentPaper == 25)
        {
            audioMeditation.gameObject.SetActive(false);
            GlobalData.indexModule = 2;

            if (GlobalData.EstadoM2 == "Completado" || isAdmin || isChilean)
            {
                GlobalData.CurrentModule = 3;
                currentModule = 3;
                btn_topbar[1].interactable = true;
                btn_topbar[2].interactable = true;
                rightHotSpot.raycastTarget = true;
            }
            else
            {
                rightHotSpot.raycastTarget = false;
            }
        }
        #endregion ----------------------------------------------------END MODULO 2----------------------------------------------------------------------
        #region ------------------------------------------------------START MODULO 3---------------------------------------------------------
        else if (book.CurrentPaper == 26)
        {
            popupReflexionVUCA.SetActive(false);
            rightHotSpot.raycastTarget = true;
            pergamino_M3.SetActive(false);
            pergaminoAnim.SetActive(false);
            GlobalData.indexModule = 3;

            if (GlobalData.EstadoM3 == "Sin iniciar")
            {
                GlobalData.EstadoM3 = "Iniciado";

                StartCoroutine(DatabaseManager.Instance.UpdateData());
            }

            if (!GlobalData.EncuestaM3Completada)
            {
                rightHotSpot.raycastTarget = false;
                encuesta_M3.SetActive(true);
            }
            else
            {
                encuesta_M3.SetActive(false);
            }
        }
        else if (book.CurrentPaper == 27)/////Pergamino
        {
            pergamino_M3.SetActive(true);

            if (!GlobalData.PergaminoVisto)
            {
                rightHotSpot.raycastTarget = false;
            }
            else
            {
                pergamino_M3.SetActive(false);
                pergaminoAnim.SetActive(true);
                rightHotSpot.raycastTarget = true;
            }
        }
        else if (book.CurrentPaper == 28)
        {
            pergamino_M3.SetActive(false);
            pergaminoAnim.SetActive(false);
        }
        else if (book.CurrentPaper == 40)
        {
            rightHotSpot.raycastTarget = true;
        }
        else if (book.CurrentPaper == 41)///////Input Sesgo
        {
            if (!GlobalData.SesgoCompletado)
            {
                rightHotSpot.raycastTarget = false;
            }
            else
            {
                rightHotSpot.raycastTarget = true;
            }
        }
        else if (book.CurrentPaper == 48)
        {
            rightHotSpot.raycastTarget = true;

        }
        else if (book.CurrentPaper == 49)//////Ejercicio
        {
            audioMeditation.gameObject.SetActive(false);

            if (!GlobalData.EjercicioM3Completado)
            {
                rightHotSpot.raycastTarget = false;
            }
            else
            {
                rightHotSpot.raycastTarget = true;
            }
        }
        else if (book.CurrentPaper == 50)////Reflexion
        {
            audioOff = audioOff3;
            audioIsPlaying = true;
            audioMeditation.gameObject.SetActive(true);

            if (string.IsNullOrEmpty(GlobalData.Mod3_Reflexion))
            {
                rightHotSpot.raycastTarget = false;
                popupReflexionMod3.SetActive(true);
            }
            else
            {
                rightHotSpot.raycastTarget = true;
                popupReflexionMod3.SetActive(false);
                AudioPlay();
            }
        }
        else if (book.CurrentPaper == 51)
        {
            audioMeditation.gameObject.SetActive(false);
            GlobalData.indexModule = 3;

            if (GlobalData.EstadoM3 == "Completado" || isAdmin || isChilean)
            {
                GlobalData.CurrentModule = 4;
                currentModule = 4;
                btn_topbar[1].interactable = true;
                btn_topbar[2].interactable = true;
                btn_topbar[3].interactable = true;
                rightHotSpot.raycastTarget = true;
            }
            else
            {
                rightHotSpot.raycastTarget = false;
            }
        }

        #endregion ----------------------------------------------------END MODULO 3----------------------------------------------------------------------
        #region ------------------------------------------------------START MODULO 4---------------------------------------------------------
        else if (book.CurrentPaper == 52) //INICIO M4
        {
            popupReflexionMod3.SetActive(false);
            rightHotSpot.raycastTarget = true;
            GlobalData.indexModule = 4;

            if (GlobalData.EstadoM4 == "Sin iniciar")
            {
                GlobalData.EstadoM4 = "Iniciado";

                StartCoroutine(DatabaseManager.Instance.UpdateData());
            }
        }
        else if (book.CurrentPaper == 63)//////Ejercicio M4
        {
            audioMeditation.gameObject.SetActive(false);

            if (!GlobalData.EjercicioM4Completado)
            {
                rightHotSpot.raycastTarget = false;
            }
            else
            {
                rightHotSpot.raycastTarget = true;
            }
        }
        else if (book.CurrentPaper == 64)////Reflexion M4
        {
            audioOff = audioOff4;
            audioIsPlaying = true;
            audioMeditation.gameObject.SetActive(true);

            if (string.IsNullOrEmpty(GlobalData.Mod4_Reflexion))
            {
                rightHotSpot.raycastTarget = false;
                popupReflexionMod4.SetActive(true);
            }
            else
            {
                rightHotSpot.raycastTarget = true;
                popupReflexionMod4.SetActive(false);
                AudioPlay();
            }
        }
        else if (book.CurrentPaper == 65) //////////Hacer en la ultima pagina cuando halla un modulo mas
        {
            audioMeditation.gameObject.SetActive(false);
            GlobalData.indexModule = 5;

            if (GlobalData.EstadoM4 == "Completado" || isAdmin || isChilean)
            {
                GlobalData.CurrentModule = 5;
                currentModule = 5;
                btn_topbar[1].interactable = true;
                btn_topbar[2].interactable = true;
                btn_topbar[3].interactable = true;
                btn_topbar[4].interactable = true;
                rightHotSpot.raycastTarget = true;
            }
            else
            {
                rightHotSpot.raycastTarget = false;
            }
        }
        #endregion ----------------------------------------------------END MODULO 4----------------------------------------------------------------------
        #region ------------------------------------------------------START MODULO 5---------------------------------------------------------
        else if (book.CurrentPaper == 66) //INICIO M5
        {
            popupReflexionMod4.SetActive(false);
            rightHotSpot.raycastTarget = true;
            GlobalData.indexModule = 5;

            if (GlobalData.EstadoM5 == "Sin iniciar")
            {
                GlobalData.EstadoM5 = "Iniciado";

                StartCoroutine(DatabaseManager.Instance.UpdateData());
            }
        }
        else if (book.CurrentPaper == 85)//////Ejercicio M5
        {
            audioMeditation.gameObject.SetActive(false);

            if (!GlobalData.EjercicioM5Completado)
            {
                rightHotSpot.raycastTarget = false;
            }
            else
            {
                rightHotSpot.raycastTarget = true;
            }
        }
        else if (book.CurrentPaper == 86)////Reflexion M5
        {
            audioOff = audioOff5;
            audioIsPlaying = true;
            audioMeditation.gameObject.SetActive(true);

            if (string.IsNullOrEmpty(GlobalData.Mod5_Reflexion))
            {
                rightHotSpot.raycastTarget = false;
                popupReflexionMod5.SetActive(true);
            }
            else
            {
                rightHotSpot.raycastTarget = true;
                popupReflexionMod5.SetActive(false);
                AudioPlay();
            }
        }
        else if (book.CurrentPaper == 87) //////////Hacer el IF en la ultima pagina cuando halla un modulo mas
        {
            rightHotSpot.raycastTarget = true;
            audioMeditation.gameObject.SetActive(false);
            GlobalData.indexModule = 5;
        }
        #endregion ----------------------------------------------------END MODULO 5----------------------------------------------------------------------

        if (isAdmin || GlobalData.EstadoM2 != "Completado" && isChilean)
        {
            rightHotSpot.raycastTarget = true;
        }

        if (book.CurrentPaper < book.papers.Length)
        {
            shadowRight.SetActive(true);
            topBar.SetActive(true);
        }

        //book.interactable = true;
    }
    #endregion BOOK

    #region MODULO 1
    public IEnumerator EnableBtnReflexion()
    {
        yield return new WaitForSeconds(20);
        btn_popup_reflexion.interactable = true;
    }

    public void OnInputValueChanged(int index)
    {
        GlobalData.indexModule = 1;

        if (index == 1)
        {
            input_PAD_Identifica.text = input_PAD_Identifica.text.Replace("\"", "");
            input_PAD_Identifica.text = input_PAD_Identifica.text.Replace("\'", "");

            input_PAD_DefinePropositos.text = input_PAD_DefinePropositos.text.Replace("\"", "");
            input_PAD_DefinePropositos.text = input_PAD_DefinePropositos.text.Replace("\'", "");

            input_PAD_IdentificaYSenala.text = input_PAD_IdentificaYSenala.text.Replace("\"", "");
            input_PAD_IdentificaYSenala.text = input_PAD_IdentificaYSenala.text.Replace("\'", "");

            input_PAD_EstableceIndicadores.text = input_PAD_EstableceIndicadores.text.Replace("\"", "");
            input_PAD_EstableceIndicadores.text = input_PAD_EstableceIndicadores.text.Replace("\'", "");

            if (string.IsNullOrEmpty(input_PAD_Identifica.text) ||
                string.IsNullOrEmpty(input_PAD_DefinePropositos.text) ||
                string.IsNullOrEmpty(input_PAD_IdentificaYSenala.text) ||
                string.IsNullOrEmpty(input_PAD_EstableceIndicadores.text))
            {
                btn_pad_save.gameObject.SetActive(false);
                return;
            }
            else
            {
                btn_pad_save.gameObject.SetActive(true);
            }
        }
        else if (index == 3)
        {
            input_Objetivo_curso.text = input_Objetivo_curso.text.Replace("\"", "");
            input_Objetivo_curso.text = input_Objetivo_curso.text.Replace("\'", "");
            GlobalData.Objetivo_curso = input_Objetivo_curso.text;
        }
        else if (index == 4)
        {
            input_Reflexion.text = input_Reflexion.text.Replace("\"", "");
            input_Reflexion.text = input_Reflexion.text.Replace("\'", "");
            if (string.IsNullOrEmpty(input_Reflexion.text))
            {
                btn_Reflexion.gameObject.SetActive(false);
                return;
            }
            else
            {
                btn_Reflexion.gameObject.SetActive(true);
            }
        }
    }

    public void PopupReflexion(bool open)
    {
        if (open)
        {
            contentInputReflexion.SetActive(true);
        }
        else
        {
            if (!string.IsNullOrEmpty(input_Reflexion.text))
            {
                GlobalData.Reflexion = input_Reflexion.text;
                GlobalData.Estado = "Completado";
                rightHotSpot.raycastTarget = true;
                btn_Reflexion.GetComponent<Image>().sprite = spr_reflexion_saved;

                StartCoroutine(DatabaseManager.Instance.UpdateData());
            }
        }
    }

    public void AnimReflexion()
    {
        StartCoroutine(EnableBtnReflexion());
        animator_reflexion.enabled = true;
        audioMeditation.gameObject.SetActive(true);
        audioIsPlaying = true;
        AudioPlay();
    }

    public void OpenHerramientaPAD(bool open)
    {
        if (open)
        {
            scrollSnapPAD.EnablePopup(true, true);
        }
        else
        {
            scrollSnapPAD.EnablePopup(false, true);
        }
    }

    public void SaveHerramientaPAD()
    {
        GlobalData.PAD_Identifica = input_PAD_Identifica.text;
        GlobalData.PAD_DefinePropositos = input_PAD_DefinePropositos.text;
        GlobalData.PAD_IdentificaYSenala = input_PAD_IdentificaYSenala.text;
        GlobalData.PAD_EstableceIndicadores = input_PAD_EstableceIndicadores.text;

        StartCoroutine(DatabaseManager.Instance.UpdateData());

        if (!string.IsNullOrEmpty(GlobalData.PAD_Identifica) &&
            !string.IsNullOrEmpty(GlobalData.PAD_DefinePropositos) &&
            !string.IsNullOrEmpty(GlobalData.PAD_IdentificaYSenala) &&
            !string.IsNullOrEmpty(GlobalData.PAD_EstableceIndicadores))
        {
            rightHotSpot.raycastTarget = true;
            btn_pad_open.GetComponent<Image>().sprite = spr_pad_saved;
        }

        OpenHerramientaPAD(false);
    }

    public void SaveObjetivo()
    {
        rightHotSpot.raycastTarget = true;

        if (isActivity)
        {
            scrollSnapPAD.EnablePopup(false, true);
        }

        isActivity = false;

        btn_Objetivo_curso.gameObject.SetActive(false);
        txt_contenido_habito_ok.SetActive(true);
        StartCoroutine(DatabaseManager.Instance.UpdateData());
    }

    public void AudioPlay()
    {
        audioIsPlaying = !audioIsPlaying;

        if (!audioIsPlaying)
        {
            audioOff.enabled = true;
            audioMeditation.Play();
        }
        else
        {
            audioOff.enabled = false;
            audioMeditation.Stop();
        }
    }
    #endregion MODULO 1

    #region MODULO 2

    public void OnPlusInfoChecked()
    {
        GlobalData.indexModule = 2;

        if (book.CurrentPaper == 13)
        {
            GlobalData.LineaDeTiempoCMVCompletado = true;
        }
        else if (book.CurrentPaper == 14)
        {
            GlobalData.SignificadoDeTerminoCompletado = true;
        }
        else if (book.CurrentPaper == 20)
        {
            GlobalData.MatrizImpactoEsfuerzoCompletado = true;
        }
        else if (book.CurrentPaper == 22)
        {
            GlobalData.MindFulnessCompletado = true;
        }
        rightHotSpot.raycastTarget = true;
    }

    public void OpenHerramientaVUCA(bool open)
    {
        if (open)
        {
            //phrasesPAD.LoadData();
            scrollSnapVUCA.EnablePopup(true, true);
        }
        else
        {
            scrollSnapVUCA.EnablePopup(false, true);
        }
    }

    public void OnInputValueChangeVUCA(int index)
    {
        if (index == 1)
        {
            input_VUCA_Oportunidades_A.text = input_VUCA_Oportunidades_A.text.Replace("\"", "");
            input_VUCA_Oportunidades_B.text = input_VUCA_Oportunidades_B.text.Replace("\"", "");
            input_VUCA_Oportunidades_A.text = input_VUCA_Oportunidades_A.text.Replace("\'", "");
            input_VUCA_Oportunidades_B.text = input_VUCA_Oportunidades_B.text.Replace("\'", "");

            input_VUCA_Tecnica_A.text = input_VUCA_Tecnica_A.text.Replace("\"", "");
            input_VUCA_Tecnica_B.text = input_VUCA_Tecnica_B.text.Replace("\"", "");
            input_VUCA_Tecnica_A.text = input_VUCA_Tecnica_A.text.Replace("\'", "");
            input_VUCA_Tecnica_B.text = input_VUCA_Tecnica_B.text.Replace("\'", "");

            input_VUCA_Sintomas_A.text = input_VUCA_Sintomas_A.text.Replace("\"", "");
            input_VUCA_Sintomas_B.text = input_VUCA_Sintomas_B.text.Replace("\"", "");
            input_VUCA_Sintomas_A.text = input_VUCA_Sintomas_A.text.Replace("\'", "");
            input_VUCA_Sintomas_B.text = input_VUCA_Sintomas_B.text.Replace("\'", "");

            input_VUCA_Iniciativa_A.text = input_VUCA_Iniciativa_A.text.Replace("\"", "");
            input_VUCA_Iniciativa_B.text = input_VUCA_Iniciativa_B.text.Replace("\"", "");
            input_VUCA_Iniciativa_A.text = input_VUCA_Iniciativa_A.text.Replace("\'", "");
            input_VUCA_Iniciativa_B.text = input_VUCA_Iniciativa_B.text.Replace("\'", "");

            DatabaseManager.Instance.VUCA_Oportunidades[0].text = input_VUCA_Oportunidades_A.text;
            DatabaseManager.Instance.VUCA_Oportunidades[1].text = input_VUCA_Oportunidades_B.text;
            DatabaseManager.Instance.VUCA_Tecnica[0].text = input_VUCA_Tecnica_A.text;
            DatabaseManager.Instance.VUCA_Tecnica[1].text = input_VUCA_Tecnica_B.text;
            DatabaseManager.Instance.VUCA_Sintomas[0].text = input_VUCA_Sintomas_A.text;
            DatabaseManager.Instance.VUCA_Sintomas[1].text = input_VUCA_Sintomas_B.text;
            DatabaseManager.Instance.VUCA_Iniciativa[0].text = input_VUCA_Iniciativa_A.text;
            DatabaseManager.Instance.VUCA_Iniciativa[1].text = input_VUCA_Iniciativa_B.text;

            if ((string.IsNullOrEmpty(input_VUCA_Oportunidades_A.text) && string.IsNullOrEmpty(input_VUCA_Oportunidades_B.text)) ||
                (string.IsNullOrEmpty(input_VUCA_Tecnica_A.text) && string.IsNullOrEmpty(input_VUCA_Tecnica_B.text)) ||
                (string.IsNullOrEmpty(input_VUCA_Sintomas_A.text) && string.IsNullOrEmpty(input_VUCA_Sintomas_B.text)) ||
                (string.IsNullOrEmpty(input_VUCA_Iniciativa_A.text) && string.IsNullOrEmpty(input_VUCA_Iniciativa_B.text)) ||
                inventoryPiecesExerciseVUCA.slotPieces.childCount > 12)
            {
                btn_vuca_save.gameObject.SetActive(false);
                return;
            }
            else
            {
                btn_vuca_save.gameObject.SetActive(true);
            }
        }
        else if (index == 2)
        {
            input_VUCA_Reflexion.text = input_VUCA_Reflexion.text.Replace("\"", "");
            input_VUCA_Reflexion.text = input_VUCA_Reflexion.text.Replace("\'", "");
            if (string.IsNullOrEmpty(input_VUCA_Reflexion.text))
            {
                btn_vuca_Reflexion.gameObject.SetActive(false);
                return;
            }
            else
            {
                btn_vuca_Reflexion.gameObject.SetActive(true);
            }
        }
    }

    public void SaveHerramientaVUCA()
    {
        phrasesPAD.SavePositionData();
        GlobalData.EjercicioMatrizCompletado = true;
        btn_vuca_open.GetComponent<Image>().sprite = spr_vuca_saved;

        StartCoroutine(DatabaseManager.Instance.UpdateData());

        OpenHerramientaVUCA(false);
    }

    public void CloseHerramientaVUCA()
    {
        OpenHerramientaVUCA(false);
    }

    public void SaveReflexionVUCA()
    {
        GlobalData.VUCA_Reflexion = input_VUCA_Reflexion.text;
        GlobalData.EstadoM2 = "Completado";
        rightHotSpot.raycastTarget = true;
        btn_vuca_Reflexion.GetComponent<Image>().sprite = spr_reflexion_saved;

        StartCoroutine(DatabaseManager.Instance.UpdateData());
    }

    #endregion MODULO 2

    #region MODULO 3
    public void EncuestaAnswers(bool positive)
    {
        GlobalData.indexModule = 3;

        for (int i = 0; i < DatabaseManager.Instance.encuestaM3.answer.Length; i++)
        {
            if (positive)
            {
                DatabaseManager.Instance.encuestaM3.answer[preguntaNumber] = "Si";
            }
            else
            {
                DatabaseManager.Instance.encuestaM3.answer[preguntaNumber] = "No";
            }
        }
        preguntaNumber++;
    }

    public void InputFieldEncuesta()
    {
        GlobalData.indexModule = 3;

        if (string.IsNullOrEmpty(inputFieldEncuesta.text))
        {
            btn_encuesta_M3.SetActive(false);
        }
        else
        {
            btn_encuesta_M3.SetActive(true);
        }

        inputFieldEncuesta.text = inputFieldEncuesta.text.Replace("\"", "");
        inputFieldEncuesta.text = inputFieldEncuesta.text.Replace("\'", "");
        DatabaseManager.Instance.encuestaM3.inputText = inputFieldEncuesta.text;
    }

    public void InputFieldSesgo()
    {
        if (string.IsNullOrEmpty(input_SesgosAreaExperiencia.text))
        {
            btn_Sesgo_Save_Mod3.SetActive(false);
        }
        else
        {
            btn_Sesgo_Save_Mod3.SetActive(true);
        }

        input_SesgosAreaExperiencia.text = input_SesgosAreaExperiencia.text.Replace("\"", "");
        input_SesgosAreaExperiencia.text = input_SesgosAreaExperiencia.text.Replace("\'", "");
        GlobalData.sesgosAreaExperiencia = input_SesgosAreaExperiencia.text;
        //Falta DB
    }

    public void InputFieldEjercicioM3()
    {
        if (!string.IsNullOrEmpty(input_Mod3_DeclaracionLiderazgo_A.text) && !string.IsNullOrEmpty(input_Mod3_SesgosInconscientes_A.text) && !string.IsNullOrEmpty(input_Mod3_PersonasIdentificadas_A.text) && !string.IsNullOrEmpty(input_Mod3_Decisiones_A.text))
        {
            btn_ejercicio_Save_M3.SetActive(true);
        }
        else
        {
            btn_ejercicio_Save_M3.SetActive(false);
        }

        input_Mod3_DeclaracionLiderazgo_A.text = input_Mod3_DeclaracionLiderazgo_A.text.Replace("\"", "");
        input_Mod3_DeclaracionLiderazgo_B.text = input_Mod3_DeclaracionLiderazgo_B.text.Replace("\"", "");
        input_Mod3_DeclaracionLiderazgo_A.text = input_Mod3_DeclaracionLiderazgo_A.text.Replace("\'", "");
        input_Mod3_DeclaracionLiderazgo_B.text = input_Mod3_DeclaracionLiderazgo_B.text.Replace("\'", "");

        input_Mod3_SesgosInconscientes_A.text = input_Mod3_SesgosInconscientes_A.text.Replace("\"", "");
        input_Mod3_SesgosInconscientes_B.text = input_Mod3_SesgosInconscientes_B.text.Replace("\"", "");
        input_Mod3_SesgosInconscientes_A.text = input_Mod3_SesgosInconscientes_A.text.Replace("\'", "");
        input_Mod3_SesgosInconscientes_B.text = input_Mod3_SesgosInconscientes_B.text.Replace("\'", "");

        input_Mod3_PersonasIdentificadas_A.text = input_Mod3_PersonasIdentificadas_A.text.Replace("\"", "");
        input_Mod3_PersonasIdentificadas_B.text = input_Mod3_PersonasIdentificadas_B.text.Replace("\"", "");
        input_Mod3_PersonasIdentificadas_A.text = input_Mod3_PersonasIdentificadas_A.text.Replace("\'", "");
        input_Mod3_PersonasIdentificadas_B.text = input_Mod3_PersonasIdentificadas_B.text.Replace("\'", "");

        input_Mod3_Decisiones_A.text = input_Mod3_Decisiones_A.text.Replace("\"", "");
        input_Mod3_Decisiones_B.text = input_Mod3_Decisiones_B.text.Replace("\"", "");
        input_Mod3_Decisiones_A.text = input_Mod3_Decisiones_A.text.Replace("\'", "");
        input_Mod3_Decisiones_B.text = input_Mod3_Decisiones_B.text.Replace("\'", "");

        DatabaseManager.Instance.Mod3_DeclaracionLiderazgo[0].text = input_Mod3_DeclaracionLiderazgo_A.text;
        DatabaseManager.Instance.Mod3_DeclaracionLiderazgo[1].text = input_Mod3_DeclaracionLiderazgo_B.text;
        DatabaseManager.Instance.Mod3_SesgosInconscientes[0].text = input_Mod3_SesgosInconscientes_A.text;
        DatabaseManager.Instance.Mod3_SesgosInconscientes[1].text = input_Mod3_SesgosInconscientes_B.text;
        DatabaseManager.Instance.Mod3_PersonasIdentificadas[0].text = input_Mod3_PersonasIdentificadas_A.text;
        DatabaseManager.Instance.Mod3_PersonasIdentificadas[1].text = input_Mod3_PersonasIdentificadas_B.text;
        DatabaseManager.Instance.Mod3_Decisiones[0].text = input_Mod3_Decisiones_A.text;
        DatabaseManager.Instance.Mod3_Decisiones[1].text = input_Mod3_Decisiones_B.text;
    }

    public void InputFieldReflexionM3()
    {
        if (string.IsNullOrEmpty(input_Mod3_Reflexion.text))
        {
            btn_reflexion_M3.SetActive(false);
        }
        else
        {
            btn_reflexion_M3.SetActive(true);
        }

        input_Mod3_Reflexion.text = input_Mod3_Reflexion.text.Replace("\"", "");
        input_Mod3_Reflexion.text = input_Mod3_Reflexion.text.Replace("\'", "");

        //Falta DB
    }

    public void OpenHerramientaMod3(bool open)
    {
        if (open)
        {
            //phrasesPAD.LoadData();
            scrollSnapMod3.EnablePopup(true, true);
        }
        else
        {
            scrollSnapMod3.EnablePopup(false, true);
        }
    }

    public void SaveHerramientaModulo3()
    {
        GlobalData.EjercicioM3Completado = true;
        btn_ejercicio_Open_M3.GetComponent<Image>().sprite = spr_vuca_saved;

        StartCoroutine(DatabaseManager.Instance.UpdateData());

        OpenHerramientaMod3(false);
    }

    public void CloseHerramientaEjercicio3()
    {
        StartCoroutine(DatabaseManager.Instance.UpdateData());
        OpenHerramientaMod3(false);
    }

    public void SaveReflexionMod3()
    {
        GlobalData.ReflexionM3Completada = true;
        GlobalData.Mod3_Reflexion = input_Mod3_Reflexion.text;
        GlobalData.EstadoM3 = "Completado";
        rightHotSpot.raycastTarget = true;
        btn_reflexion_M3.GetComponent<Image>().sprite = spr_reflexion_saved;

        StartCoroutine(DatabaseManager.Instance.UpdateData());
    }

    public void SaveEncuestaMod3()
    {
        GlobalData.EncuestaM3Completada = true;
        StartCoroutine(DatabaseManager.Instance.UpdateData());
    }
    public void SaveSesgoMod3()
    {
        GlobalData.SesgoCompletado = true;
        StartCoroutine(DatabaseManager.Instance.UpdateData());
    }

    #endregion MODULO 3

    #region MODULO 4

    public void InputFieldEjercicioM4()
    {
        GlobalData.indexModule = 4;

        if (!string.IsNullOrEmpty(input_Mod4_Pregunta1_A.text) && !string.IsNullOrEmpty(input_Mod4_Pregunta2_A.text) && !string.IsNullOrEmpty(input_Mod4_Pregunta3_A.text) && !string.IsNullOrEmpty(input_Mod4_Pregunta4_A.text) && !string.IsNullOrEmpty(input_Mod4_Pregunta5_A.text) && !string.IsNullOrEmpty(input_Mod4_Pregunta6_A.text))
        {
            btn_ejercicio_Save_M4.SetActive(true);
        }
        else
        {
            btn_ejercicio_Save_M4.SetActive(false);
        }

        input_Mod4_Pregunta1_A.text = input_Mod4_Pregunta1_A.text.Replace("\"", "");
        input_Mod4_Pregunta1_B.text = input_Mod4_Pregunta1_B.text.Replace("\"", "");
        input_Mod4_Pregunta1_A.text = input_Mod4_Pregunta1_A.text.Replace("\'", "");
        input_Mod4_Pregunta1_B.text = input_Mod4_Pregunta1_B.text.Replace("\'", "");

        input_Mod4_Pregunta2_B.text = input_Mod4_Pregunta2_B.text.Replace("\"", "");
        input_Mod4_Pregunta2_A.text = input_Mod4_Pregunta2_A.text.Replace("\"", "");
        input_Mod4_Pregunta2_A.text = input_Mod4_Pregunta2_A.text.Replace("\'", "");
        input_Mod4_Pregunta2_B.text = input_Mod4_Pregunta2_B.text.Replace("\'", "");

        input_Mod4_Pregunta3_A.text = input_Mod4_Pregunta3_A.text.Replace("\"", "");
        input_Mod4_Pregunta3_B.text = input_Mod4_Pregunta3_B.text.Replace("\"", "");
        input_Mod4_Pregunta3_A.text = input_Mod4_Pregunta3_A.text.Replace("\'", "");
        input_Mod4_Pregunta3_B.text = input_Mod4_Pregunta3_B.text.Replace("\'", "");

        input_Mod4_Pregunta4_A.text = input_Mod4_Pregunta4_A.text.Replace("\"", "");
        input_Mod4_Pregunta4_B.text = input_Mod4_Pregunta4_B.text.Replace("\"", "");
        input_Mod4_Pregunta4_A.text = input_Mod4_Pregunta4_A.text.Replace("\'", "");
        input_Mod4_Pregunta4_B.text = input_Mod4_Pregunta4_B.text.Replace("\'", "");

        input_Mod4_Pregunta5_A.text = input_Mod4_Pregunta5_A.text.Replace("\"", "");
        input_Mod4_Pregunta5_B.text = input_Mod4_Pregunta5_B.text.Replace("\"", "");
        input_Mod4_Pregunta5_A.text = input_Mod4_Pregunta5_A.text.Replace("\'", "");
        input_Mod4_Pregunta5_B.text = input_Mod4_Pregunta5_B.text.Replace("\'", "");

        input_Mod4_Pregunta6_A.text = input_Mod4_Pregunta6_A.text.Replace("\"", "");
        input_Mod4_Pregunta6_B.text = input_Mod4_Pregunta6_B.text.Replace("\"", "");
        input_Mod4_Pregunta6_A.text = input_Mod4_Pregunta6_A.text.Replace("\'", "");
        input_Mod4_Pregunta6_B.text = input_Mod4_Pregunta6_B.text.Replace("\'", "");

        DatabaseManager.Instance.Mod4_Pregunta1[0].text = input_Mod4_Pregunta1_A.text;
        DatabaseManager.Instance.Mod4_Pregunta1[1].text = input_Mod4_Pregunta1_B.text;
        DatabaseManager.Instance.Mod4_Pregunta2[0].text = input_Mod4_Pregunta2_A.text;
        DatabaseManager.Instance.Mod4_Pregunta2[1].text = input_Mod4_Pregunta2_B.text;
        DatabaseManager.Instance.Mod4_Pregunta3[0].text = input_Mod4_Pregunta3_A.text;
        DatabaseManager.Instance.Mod4_Pregunta3[1].text = input_Mod4_Pregunta3_B.text;
        DatabaseManager.Instance.Mod4_Pregunta4[0].text = input_Mod4_Pregunta4_A.text;
        DatabaseManager.Instance.Mod4_Pregunta4[1].text = input_Mod4_Pregunta4_B.text;
        DatabaseManager.Instance.Mod4_Pregunta5[0].text = input_Mod4_Pregunta5_A.text;
        DatabaseManager.Instance.Mod4_Pregunta5[1].text = input_Mod4_Pregunta5_B.text;
        DatabaseManager.Instance.Mod4_Pregunta6[0].text = input_Mod4_Pregunta6_A.text;
        DatabaseManager.Instance.Mod4_Pregunta6[1].text = input_Mod4_Pregunta6_B.text;
    }

    public void InputFieldReflexionM4()
    {
        GlobalData.indexModule = 4;
        if (string.IsNullOrEmpty(input_Mod4_Reflexion.text))
        {
            btn_reflexion_M4.SetActive(false);
        }
        else
        {
            btn_reflexion_M4.SetActive(true);
        }

        input_Mod4_Reflexion.text = input_Mod4_Reflexion.text.Replace("\"", "");
        input_Mod4_Reflexion.text = input_Mod4_Reflexion.text.Replace("\'", "");
    }

    public void OpenHerramientaMod4(bool open)
    {
        GlobalData.indexModule = 4;
        if (open)
        {
            scrollSnapMod4.EnablePopup(true, true);
        }
        else
        {
            scrollSnapMod4.EnablePopup(false, true);
        }
    }

    public void SaveHerramientaModulo4()
    {
        GlobalData.indexModule = 4;
        GlobalData.EjercicioM4Completado = true;
        btn_ejercicio_Open_M4.GetComponent<Image>().sprite = spr_vuca_saved;

        StartCoroutine(DatabaseManager.Instance.UpdateData());

        OpenHerramientaMod4(false);
    }

    public void CloseHerramientaEjercicio4()
    {   
        StartCoroutine(DatabaseManager.Instance.UpdateData());
        OpenHerramientaMod4(false);
    }

    public void SaveReflexionMod4()
    {
        GlobalData.indexModule = 4;
        GlobalData.ReflexionM4Completada = true;
        GlobalData.Mod4_Reflexion = input_Mod4_Reflexion.text;
        GlobalData.EstadoM4 = "Completado";
        rightHotSpot.raycastTarget = true;
        btn_reflexion_M4.GetComponent<Image>().sprite = spr_reflexion_saved;

        StartCoroutine(DatabaseManager.Instance.UpdateData());
    }
    #endregion MODULO 4

    #region MODULO 5

    public void InputFieldEjercicioM5()
    {
        GlobalData.indexModule = 5;

        if (!string.IsNullOrEmpty(input_Mod5_Pregunta1_A.text) && !string.IsNullOrEmpty(input_Mod5_Pregunta2_A.text) && !string.IsNullOrEmpty(input_Mod5_Pregunta3_A.text))
        {
            btn_ejercicio_Save_M5.SetActive(true);
        }
        else
        {
            btn_ejercicio_Save_M5.SetActive(false);
        }

        input_Mod5_Pregunta1_A.text = input_Mod5_Pregunta1_A.text.Replace("\"", "");
        input_Mod5_Pregunta1_B.text = input_Mod5_Pregunta1_B.text.Replace("\"", "");
        input_Mod5_Pregunta1_A.text = input_Mod5_Pregunta1_A.text.Replace("\'", "");
        input_Mod5_Pregunta1_B.text = input_Mod5_Pregunta1_B.text.Replace("\'", "");

        input_Mod5_Pregunta2_B.text = input_Mod5_Pregunta2_B.text.Replace("\"", "");
        input_Mod5_Pregunta2_A.text = input_Mod5_Pregunta2_A.text.Replace("\"", "");
        input_Mod5_Pregunta2_A.text = input_Mod5_Pregunta2_A.text.Replace("\'", "");
        input_Mod5_Pregunta2_B.text = input_Mod5_Pregunta2_B.text.Replace("\'", "");

        input_Mod5_Pregunta3_A.text = input_Mod5_Pregunta3_A.text.Replace("\"", "");
        input_Mod5_Pregunta3_B.text = input_Mod5_Pregunta3_B.text.Replace("\"", "");
        input_Mod5_Pregunta3_A.text = input_Mod5_Pregunta3_A.text.Replace("\'", "");
        input_Mod5_Pregunta3_B.text = input_Mod5_Pregunta3_B.text.Replace("\'", "");

        DatabaseManager.Instance.Mod5_Pregunta1[0].text = input_Mod5_Pregunta1_A.text;
        DatabaseManager.Instance.Mod5_Pregunta1[1].text = input_Mod5_Pregunta1_B.text;
        DatabaseManager.Instance.Mod5_Pregunta2[0].text = input_Mod5_Pregunta2_A.text;
        DatabaseManager.Instance.Mod5_Pregunta2[1].text = input_Mod5_Pregunta2_B.text;
        DatabaseManager.Instance.Mod5_Pregunta3[0].text = input_Mod5_Pregunta3_A.text;
        DatabaseManager.Instance.Mod5_Pregunta3[1].text = input_Mod5_Pregunta3_B.text;
    }

    public void InputFieldReflexionM5()
    {
        GlobalData.indexModule = 5;

        if (string.IsNullOrEmpty(input_Mod5_Reflexion.text))
        {
            btn_reflexion_M5.SetActive(false);
        }
        else
        {
            btn_reflexion_M5.SetActive(true);
        }

        input_Mod5_Reflexion.text = input_Mod5_Reflexion.text.Replace("\"", "");
        input_Mod5_Reflexion.text = input_Mod5_Reflexion.text.Replace("\'", "");
    }

    public void OpenHerramientaMod5(bool open)
    {
        GlobalData.indexModule = 5;

        if (open)
        {
            scrollSnapMod5.EnablePopup(true, true);
        }
        else
        {
            scrollSnapMod5.EnablePopup(false, true);
        }
    }

    public void SaveHerramientaModulo5()
    {
        GlobalData.indexModule = 5;
        GlobalData.EjercicioM5Completado = true;
        btn_ejercicio_Open_M5.GetComponent<Image>().sprite = spr_vuca_saved;

        StartCoroutine(DatabaseManager.Instance.UpdateData());

        OpenHerramientaMod5(false);
    }

    public void CloseHerramientaEjercicio5()
    {
        StartCoroutine(DatabaseManager.Instance.UpdateData());
        OpenHerramientaMod5(false);
    }

    public void SaveReflexionMod5()
    {
        GlobalData.indexModule = 5;
        GlobalData.ReflexionM5Completada = true;
        GlobalData.Mod5_Reflexion = input_Mod5_Reflexion.text;
        GlobalData.EstadoM5 = "Completado";
        rightHotSpot.raycastTarget = true;
        btn_reflexion_M5.GetComponent<Image>().sprite = spr_reflexion_saved;

        StartCoroutine(DatabaseManager.Instance.UpdateData());
    }
    #endregion MODULO 5

    #region BOOK - DATA - VIDEO
    public void onFlipPage(bool start, int page)
    {

    }

    public void SaveAll()
    {
        StartCoroutine(DatabaseManager.Instance.UpdateData(true));
    }

    public void LoadFeedBackConnection(bool estado)
    {
        feedBackConnection.SetActive(true);
        feedBackIcon_Good.SetActive(false);
        feedBackIcon_Wrong.SetActive(false);

        if (estado)
        {
            infoFeedbackText.text = string.Format("<color=#78BE20>{0}</color>", "La información que has registrado se ha almacenado correctamente.");
            infoFeedbackText.fontSize = 14;
            feedBackIcon_Good.SetActive(true);
        }
        else
        {
            infoFeedbackText.text = string.Format("<color=#E4002B>{0}</color><br><br><size=9px><color=#00ACF9>{1}</color></size>", "La información no se ha podido almacenar correctamente. Por favor, inténtalo nuevamente haciendo clic en 'Reintentar guardar'.", "Si el problema persiste, contacta al encargado del PAD en tu país.");
            infoFeedbackText.fontSize = 10;
            feedBackIcon_Wrong.SetActive(true);
        }
    }

    public void LoadData()
    {
        #region MODULO 1

        GlobalData.PAD_Identifica = GlobalData.PAD_Identifica.Replace("\"", "");
        GlobalData.PAD_Identifica = GlobalData.PAD_Identifica.Replace("\'", "");
        GlobalData.PAD_DefinePropositos = GlobalData.PAD_DefinePropositos.Replace("\"", "");
        GlobalData.PAD_DefinePropositos = GlobalData.PAD_DefinePropositos.Replace("\'", "");
        GlobalData.PAD_IdentificaYSenala = GlobalData.PAD_IdentificaYSenala.Replace("\"", "");
        GlobalData.PAD_IdentificaYSenala = GlobalData.PAD_IdentificaYSenala.Replace("\'", "");
        GlobalData.PAD_EstableceIndicadores = GlobalData.PAD_EstableceIndicadores.Replace("\"", "");
        GlobalData.PAD_EstableceIndicadores = GlobalData.PAD_EstableceIndicadores.Replace("\'", "");

        GlobalData.Reflexion = GlobalData.Reflexion.Replace("\"", "");
        GlobalData.Reflexion = GlobalData.Reflexion.Replace("\'", "");

        GlobalData.Objetivo_curso = GlobalData.Objetivo_curso.Replace("\"", "");
        GlobalData.Objetivo_curso = GlobalData.Objetivo_curso.Replace("\'", "");

        input_PAD_Identifica.text = GlobalData.PAD_Identifica;
        input_PAD_DefinePropositos.text = GlobalData.PAD_DefinePropositos;
        input_PAD_IdentificaYSenala.text = GlobalData.PAD_IdentificaYSenala;
        input_PAD_EstableceIndicadores.text = GlobalData.PAD_EstableceIndicadores;
        input_Reflexion.text = GlobalData.Reflexion;
        input_Objetivo_curso.text = GlobalData.Objetivo_curso;

        if (!string.IsNullOrEmpty(GlobalData.PAD_Identifica) &&
            !string.IsNullOrEmpty(GlobalData.PAD_DefinePropositos) &&
            !string.IsNullOrEmpty(GlobalData.PAD_IdentificaYSenala) &&
            !string.IsNullOrEmpty(GlobalData.PAD_EstableceIndicadores))
        {
            btn_pad_open.GetComponent<Image>().sprite = spr_pad_saved;
        }

        if (!string.IsNullOrEmpty(GlobalData.Objetivo_curso))
        {
            btn_Objetivo_curso.gameObject.SetActive(false);
            txt_contenido_habito_ok.SetActive(true);
        }

        if (!string.IsNullOrEmpty(input_Reflexion.text))
        {
            btn_Reflexion.GetComponent<Image>().sprite = spr_reflexion_saved;
        }
        #endregion MODULO 1

        #region MODULO 2

        string oportA ="";
        string oportB ="";
        string tecnA = "";
        string tecnB = "";
        string sintA = "";
        string sintB = "";
        string inicA = "";
        string inicB = "";

        if (DatabaseManager.Instance.VUCA_Oportunidades[0] != null)
        {
            DatabaseManager.Instance.VUCA_Oportunidades[0].text = DatabaseManager.Instance.VUCA_Oportunidades[0].text.Replace("\'", "");
            oportA = DatabaseManager.Instance.VUCA_Oportunidades[0].text;
        }

        if (DatabaseManager.Instance.VUCA_Oportunidades[1] != null)
        {
            DatabaseManager.Instance.VUCA_Oportunidades[1].text = DatabaseManager.Instance.VUCA_Oportunidades[1].text.Replace("\'", "");
            oportB = DatabaseManager.Instance.VUCA_Oportunidades[1].text;
        }

        if (DatabaseManager.Instance.VUCA_Tecnica[0] != null)
        {
            DatabaseManager.Instance.VUCA_Tecnica[0].text = DatabaseManager.Instance.VUCA_Tecnica[0].text.Replace("\'", "");
            tecnA = DatabaseManager.Instance.VUCA_Tecnica[0].text;
        }

        if (DatabaseManager.Instance.VUCA_Tecnica[1] != null)
        {
            DatabaseManager.Instance.VUCA_Tecnica[1].text = DatabaseManager.Instance.VUCA_Tecnica[1].text.Replace("\'", "");
            tecnB = DatabaseManager.Instance.VUCA_Tecnica[1].text;
        }

        if (DatabaseManager.Instance.VUCA_Sintomas[0] != null)
        {
            DatabaseManager.Instance.VUCA_Sintomas[0].text = DatabaseManager.Instance.VUCA_Sintomas[0].text.Replace("\'", "");
            sintA = DatabaseManager.Instance.VUCA_Sintomas[0].text;
        }

        if (DatabaseManager.Instance.VUCA_Sintomas[1] != null)
        {
            DatabaseManager.Instance.VUCA_Sintomas[1].text = DatabaseManager.Instance.VUCA_Sintomas[1].text.Replace("\'", "");
            sintB = DatabaseManager.Instance.VUCA_Sintomas[1].text;
        }

        if (DatabaseManager.Instance.VUCA_Iniciativa[0] != null)
        {
            DatabaseManager.Instance.VUCA_Iniciativa[0].text = DatabaseManager.Instance.VUCA_Iniciativa[0].text.Replace("\'", "");
            inicA = DatabaseManager.Instance.VUCA_Iniciativa[0].text;
        }

        if (DatabaseManager.Instance.VUCA_Iniciativa[1] != null)
        {
            DatabaseManager.Instance.VUCA_Iniciativa[1].text = DatabaseManager.Instance.VUCA_Iniciativa[1].text.Replace("\'", "");
            inicB = DatabaseManager.Instance.VUCA_Iniciativa[1].text;
        }

        input_VUCA_Oportunidades_A.text = oportA;
        input_VUCA_Oportunidades_B.text = oportB;
        input_VUCA_Tecnica_A.text = tecnA;
        input_VUCA_Tecnica_B.text = tecnB;
        input_VUCA_Sintomas_A.text = sintA;
        input_VUCA_Sintomas_B.text = sintB;
        input_VUCA_Iniciativa_A.text = inicA;
        input_VUCA_Iniciativa_B.text = inicB;

        GlobalData.VUCA_Reflexion = GlobalData.VUCA_Reflexion.Replace("\"", "");
        GlobalData.VUCA_Reflexion = GlobalData.VUCA_Reflexion.Replace("\'", "");

        input_VUCA_Reflexion.text = GlobalData.VUCA_Reflexion;

        
        //Debug.Log("3.input 0: " + DatabaseManager.Instance.VUCA_Oportunidades[0].text + " :1 " + DatabaseManager.Instance.VUCA_Oportunidades[1].text);

        if (GlobalData.EjercicioMatrizCompletado)
        {
            btn_vuca_save.gameObject.SetActive(true);
            btn_vuca_open.GetComponent<Image>().sprite = spr_vuca_saved;
        }

        if (!string.IsNullOrEmpty(GlobalData.VUCA_Reflexion))
        {
            btn_vuca_Reflexion.GetComponent<Image>().sprite = spr_reflexion_saved;
        }

        #endregion MODULO 2

        #region MODULO 3
        string decA = "";
        string decB = "";
        string sesA = "";
        string sesB = "";
        string perA = "";
        string perB = "";
        string desA = "";
        string desB = "";

        if (DatabaseManager.Instance.Mod3_DeclaracionLiderazgo[0] != null)
        {
            DatabaseManager.Instance.Mod3_DeclaracionLiderazgo[0].text = DatabaseManager.Instance.Mod3_DeclaracionLiderazgo[0].text.Replace("\'", "");
            decA = DatabaseManager.Instance.Mod3_DeclaracionLiderazgo[0].text;
        }

        if (DatabaseManager.Instance.Mod3_DeclaracionLiderazgo[1] != null)
        {
            DatabaseManager.Instance.Mod3_DeclaracionLiderazgo[1].text = DatabaseManager.Instance.Mod3_DeclaracionLiderazgo[1].text.Replace("\'", "");
            decB = DatabaseManager.Instance.Mod3_DeclaracionLiderazgo[1].text;
        }

        if (DatabaseManager.Instance.Mod3_SesgosInconscientes[0] != null)
        {
            DatabaseManager.Instance.Mod3_SesgosInconscientes[0].text = DatabaseManager.Instance.Mod3_SesgosInconscientes[0].text.Replace("\'", "");
            sesA = DatabaseManager.Instance.Mod3_SesgosInconscientes[0].text;
        }

        if (DatabaseManager.Instance.Mod3_SesgosInconscientes[1] != null)
        {
            DatabaseManager.Instance.Mod3_SesgosInconscientes[1].text = DatabaseManager.Instance.Mod3_SesgosInconscientes[1].text.Replace("\'", "");
            sesB = DatabaseManager.Instance.Mod3_SesgosInconscientes[1].text;
        }

        if (DatabaseManager.Instance.Mod3_PersonasIdentificadas[0] != null)
        {
            DatabaseManager.Instance.Mod3_PersonasIdentificadas[0].text = DatabaseManager.Instance.Mod3_PersonasIdentificadas[0].text.Replace("\'", "");
            perA = DatabaseManager.Instance.Mod3_PersonasIdentificadas[0].text;
        }

        if (DatabaseManager.Instance.Mod3_PersonasIdentificadas[1] != null)
        {
            DatabaseManager.Instance.Mod3_PersonasIdentificadas[1].text = DatabaseManager.Instance.Mod3_PersonasIdentificadas[1].text.Replace("\'", "");
            perB = DatabaseManager.Instance.Mod3_PersonasIdentificadas[1].text;
        }

        if (DatabaseManager.Instance.Mod3_Decisiones[0] != null)
        {
            DatabaseManager.Instance.Mod3_Decisiones[0].text = DatabaseManager.Instance.Mod3_Decisiones[0].text.Replace("\'", "");
            desA = DatabaseManager.Instance.Mod3_Decisiones[0].text;
        }

        if (DatabaseManager.Instance.Mod3_Decisiones[1] != null)
        {
            DatabaseManager.Instance.Mod3_Decisiones[1].text = DatabaseManager.Instance.Mod3_Decisiones[1].text.Replace("\'", "");
            desB = DatabaseManager.Instance.Mod3_Decisiones[1].text;
        }

        input_SesgosAreaExperiencia.text = GlobalData.sesgosAreaExperiencia;
        input_Mod3_DeclaracionLiderazgo_A.text = decA;
        input_Mod3_DeclaracionLiderazgo_B.text = decB;
        input_Mod3_SesgosInconscientes_A.text = sesA;
        input_Mod3_SesgosInconscientes_B.text = sesB;
        input_Mod3_PersonasIdentificadas_A.text = perA;
        input_Mod3_PersonasIdentificadas_B.text = perB;
        input_Mod3_Decisiones_A.text = desA;
        input_Mod3_Decisiones_B.text = desB;
        input_Mod3_Reflexion.text = GlobalData.Mod3_Reflexion;

        if (GlobalData.EjercicioM3Completado)
        {
            btn_ejercicio_Open_M3.GetComponent<Image>().sprite = spr_vuca_saved;
        }

        if (!string.IsNullOrEmpty(GlobalData.Mod3_Reflexion))
        {
            btn_reflexion_M3.GetComponent<Image>().sprite = spr_reflexion_saved;
        }
        #endregion MODULO 3

        #region MODULO 4
        string pre1A = "";
        string pre1B = "";
        string pre2A = "";
        string pre2B = "";
        string pre3A = "";
        string pre3B = "";
        string pre4A = "";
        string pre4B = "";
        string pre5A = "";
        string pre5B = "";
        string pre6A = "";
        string pre6B = "";


        if (DatabaseManager.Instance.Mod4_Pregunta1[0] != null)
        {
            DatabaseManager.Instance.Mod4_Pregunta1[0].text = DatabaseManager.Instance.Mod4_Pregunta1[0].text.Replace("\'", "");
            pre1A = DatabaseManager.Instance.Mod4_Pregunta1[0].text;
        }
        if (DatabaseManager.Instance.Mod4_Pregunta2[1] != null)
        {
            DatabaseManager.Instance.Mod4_Pregunta1[1].text = DatabaseManager.Instance.Mod4_Pregunta1[1].text.Replace("\'", "");
            pre1B = DatabaseManager.Instance.Mod4_Pregunta1[1].text;
        }

        if (DatabaseManager.Instance.Mod4_Pregunta2[0] != null)
        {
            DatabaseManager.Instance.Mod4_Pregunta2[0].text = DatabaseManager.Instance.Mod4_Pregunta2[0].text.Replace("\'", "");
            pre2A= DatabaseManager.Instance.Mod4_Pregunta2[0].text;
        }
        if (DatabaseManager.Instance.Mod4_Pregunta2[1] != null)
        {
            DatabaseManager.Instance.Mod4_Pregunta2[1].text = DatabaseManager.Instance.Mod4_Pregunta2[1].text.Replace("\'", "");
            pre2B = DatabaseManager.Instance.Mod4_Pregunta2[1].text;
        }

        if (DatabaseManager.Instance.Mod4_Pregunta3[0] != null)
        {
            DatabaseManager.Instance.Mod4_Pregunta3[0].text = DatabaseManager.Instance.Mod4_Pregunta3[0].text.Replace("\'", "");
            pre3A = DatabaseManager.Instance.Mod4_Pregunta3[0].text;
        }
        if (DatabaseManager.Instance.Mod4_Pregunta3[1] != null)
        {
            DatabaseManager.Instance.Mod4_Pregunta3[1].text = DatabaseManager.Instance.Mod4_Pregunta3[1].text.Replace("\'", "");
            pre3B = DatabaseManager.Instance.Mod4_Pregunta3[1].text;
        }

        if (DatabaseManager.Instance.Mod4_Pregunta4[0] != null)
        {
            DatabaseManager.Instance.Mod4_Pregunta4[0].text = DatabaseManager.Instance.Mod4_Pregunta4[0].text.Replace("\'", "");
            pre4A= DatabaseManager.Instance.Mod4_Pregunta4[0].text;
        }
        if (DatabaseManager.Instance.Mod4_Pregunta4[1] != null)
        {
            DatabaseManager.Instance.Mod4_Pregunta4[1].text = DatabaseManager.Instance.Mod4_Pregunta4[1].text.Replace("\'", "");
            pre4B = DatabaseManager.Instance.Mod4_Pregunta1[1].text;
        }

        if (DatabaseManager.Instance.Mod4_Pregunta5[0] != null)
        {
            DatabaseManager.Instance.Mod4_Pregunta5[0].text = DatabaseManager.Instance.Mod4_Pregunta5[0].text.Replace("\'", "");
            pre5A = DatabaseManager.Instance.Mod4_Pregunta5[0].text;
        }
        if (DatabaseManager.Instance.Mod4_Pregunta5[1] != null)
        {
            DatabaseManager.Instance.Mod4_Pregunta5[1].text = DatabaseManager.Instance.Mod4_Pregunta5[1].text.Replace("\'", "");
            pre5B = DatabaseManager.Instance.Mod4_Pregunta5[1].text;
        }

        if (DatabaseManager.Instance.Mod4_Pregunta6[0] != null)
        {
            DatabaseManager.Instance.Mod4_Pregunta6[0].text = DatabaseManager.Instance.Mod4_Pregunta6[0].text.Replace("\'", "");
            pre6A = DatabaseManager.Instance.Mod4_Pregunta6[0].text;
        }
        if (DatabaseManager.Instance.Mod4_Pregunta1[1] != null)
        {
            DatabaseManager.Instance.Mod4_Pregunta6[1].text = DatabaseManager.Instance.Mod4_Pregunta6[1].text.Replace("\'", "");
            pre6B = DatabaseManager.Instance.Mod4_Pregunta6[1].text;
        }


        input_Mod4_Pregunta1_A.text = pre1A;
        input_Mod4_Pregunta1_B.text = pre1B;
        input_Mod4_Pregunta2_A.text = pre2A;
        input_Mod4_Pregunta2_B.text = pre2B;
        input_Mod4_Pregunta3_A.text = pre3A;
        input_Mod4_Pregunta3_B.text = pre3B;
        input_Mod4_Pregunta4_A.text = pre4A;
        input_Mod4_Pregunta4_B.text = pre4B;
        input_Mod4_Pregunta5_A.text = pre5A;
        input_Mod4_Pregunta5_B.text = pre5B;
        input_Mod4_Pregunta6_A.text = pre6A;
        input_Mod4_Pregunta6_B.text = pre6B;

        input_Mod4_Reflexion.text = GlobalData.Mod4_Reflexion;

        if (GlobalData.EjercicioM4Completado)
        {
            btn_ejercicio_Open_M4.GetComponent<Image>().sprite = spr_vuca_saved;
        }

        if (!string.IsNullOrEmpty(GlobalData.Mod4_Reflexion))
        {
            btn_reflexion_M4.GetComponent<Image>().sprite = spr_reflexion_saved;
        }

        #endregion MODULO 4

        #region MODULO 5    
        string pre51A = "";
        string pre51B = "";
        string pre52A = "";
        string pre52B = "";
        string pre53A = "";
        string pre53B = "";

        if (DatabaseManager.Instance.Mod5_Pregunta1[0] != null)
        {
            DatabaseManager.Instance.Mod5_Pregunta1[0].text = DatabaseManager.Instance.Mod5_Pregunta1[0].text.Replace("\'", "");
            pre51A = DatabaseManager.Instance.Mod5_Pregunta1[0].text;
        }
        if (DatabaseManager.Instance.Mod5_Pregunta2[1] != null)
        {
            DatabaseManager.Instance.Mod5_Pregunta1[1].text = DatabaseManager.Instance.Mod5_Pregunta1[1].text.Replace("\'", "");
            pre51B = DatabaseManager.Instance.Mod5_Pregunta1[1].text;
        }

        if (DatabaseManager.Instance.Mod5_Pregunta2[0] != null)
        {
            DatabaseManager.Instance.Mod5_Pregunta2[0].text = DatabaseManager.Instance.Mod5_Pregunta2[0].text.Replace("\'", "");
            pre52A = DatabaseManager.Instance.Mod5_Pregunta2[0].text;
        }
        if (DatabaseManager.Instance.Mod5_Pregunta2[1] != null)
        {
            DatabaseManager.Instance.Mod5_Pregunta2[1].text = DatabaseManager.Instance.Mod5_Pregunta2[1].text.Replace("\'", "");
            pre52B = DatabaseManager.Instance.Mod5_Pregunta2[1].text;
        }

        if (DatabaseManager.Instance.Mod5_Pregunta3[0] != null)
        {
            DatabaseManager.Instance.Mod5_Pregunta3[0].text = DatabaseManager.Instance.Mod5_Pregunta3[0].text.Replace("\'", "");
            pre53A = DatabaseManager.Instance.Mod5_Pregunta3[0].text;
        }
        if (DatabaseManager.Instance.Mod5_Pregunta3[1] != null)
        {
            DatabaseManager.Instance.Mod5_Pregunta3[1].text = DatabaseManager.Instance.Mod5_Pregunta3[1].text.Replace("\'", "");
            pre53B = DatabaseManager.Instance.Mod5_Pregunta3[1].text;
        }

        input_Mod5_Pregunta1_A.text = pre51A;
        input_Mod5_Pregunta1_B.text = pre51B;
        input_Mod5_Pregunta2_A.text = pre52A;
        input_Mod5_Pregunta2_B.text = pre52B;
        input_Mod5_Pregunta3_A.text = pre53A;
        input_Mod5_Pregunta3_B.text = pre53B;

        input_Mod5_Reflexion.text = GlobalData.Mod5_Reflexion;

        if (GlobalData.EjercicioM5Completado)
        {
            btn_ejercicio_Open_M5.GetComponent<Image>().sprite = spr_vuca_saved;
        }

        if (!string.IsNullOrEmpty(GlobalData.Mod5_Reflexion))
        {
            btn_reflexion_M5.GetComponent<Image>().sprite = spr_reflexion_saved;
        }

        #endregion MODULO 5

        DatabaseManager.Instance.typeUser = GlobalData.TypeUser;

        txt_name[0].text = GlobalData.Nombre;
        txt_name[1].text = GlobalData.Nombre;
        txt_name[2].text = GlobalData.Nombre;
        feedbackName.text = "Hola " + GlobalData.Nombre;

        phrasesPAD.LoadData();

        btn_next.gameObject.SetActive(true);
        btn_prev.gameObject.SetActive(true);
        book.interactable = true;
        loginGroup.SetActive(false);
        topBar.SetActive(true);

        if (GlobalData.TypeUser == "Líder de Mentor" ||
            GlobalData.TypeUser == "Líder de mentor" ||
            GlobalData.TypeUser == "Mentor" ||
            GlobalData.TypeUser == "Administrador Chile" ||
            GlobalData.TypeUser == "Administrador Colombia" ||
            GlobalData.TypeUser == "Administrador El Salvador" ||
            GlobalData.TypeUser == "Administrador México" ||
            GlobalData.TypeUser == "Administrador Perú" ||
            GlobalData.TypeUser == "Administrador Uruguay" ||
            GlobalData.TypeUser == "Administrador Corporativo")
        {
            btn_lider.SetActive(true);
        }

        if (GlobalData.TypeUser == "Administrador Chile" ||
            GlobalData.TypeUser == "Administrador Colombia" ||
            GlobalData.TypeUser == "Administrador El Salvador" ||
            GlobalData.TypeUser == "Administrador México" ||
            GlobalData.TypeUser == "Administrador Perú" ||
            GlobalData.TypeUser == "Administrador Uruguay" ||
            GlobalData.TypeUser == "Administrador Corporativo")
        {
            isAdmin = true;
        }

        if (GlobalData.Pais == "Chile" && GlobalData.Estado == "Completado" && GlobalData.EstadoM2 != "Completado")
        {
            isChilean = true;
            btn_topbar[2].interactable = true;
        }

        if (GlobalData.Estado == "Completado")
        {
            GlobalData.CurrentModule = 2;
            currentModule = 2;
            btn_topbar[1].interactable = true;
        }
        else
        {
            if (isAdmin)
            {
                btn_topbar[1].interactable = true;
            }
        }

        if (GlobalData.EstadoM2 == "Completado")
        {
            GlobalData.CurrentModule = 3;
            currentModule = 3;
            btn_topbar[1].interactable = true;
            btn_topbar[2].interactable = true;
        }
        else
        {
            if (isAdmin)
            {
                btn_topbar[1].interactable = true;
                btn_topbar[2].interactable = true;
            }
        }

        if (GlobalData.EstadoM3 == "Completado")
        {
            GlobalData.CurrentModule = 4;
            currentModule = 4;
            btn_topbar[1].interactable = true;
            btn_topbar[2].interactable = true;
            btn_topbar[3].interactable = true;
        }
        else
        {
            if (isAdmin)
            {
                btn_topbar[1].interactable = true;
                btn_topbar[2].interactable = true;
                btn_topbar[3].interactable = true;
            }
        }

        if (GlobalData.EstadoM4 == "Completado")
        {
            GlobalData.CurrentModule = 5;
            currentModule = 5;
            btn_topbar[1].interactable = true;
            btn_topbar[2].interactable = true;
            btn_topbar[3].interactable = true;
            btn_topbar[4].interactable = true;
        }
        else
        {
            if (isAdmin)
            {
                btn_topbar[1].interactable = true;
                btn_topbar[2].interactable = true;
                btn_topbar[3].interactable = true;
                btn_topbar[4].interactable = true;
            }
        }

        switch (GlobalData.CurrentModule)
        {
            case 1:
                GlobalData.indexModule = 1;
                break;
            case 2:
                GlobalData.indexModule = 2;
                ChangePage(12);//23
                OnChangePage();
                shadowLeft.SetActive(true);
                btn_changeCover.SetActive(false);
                break;
            case 3:
                GlobalData.indexModule = 3;
                ChangePage(26);//51
                OnChangePage();
                shadowLeft.SetActive(true);
                btn_changeCover.SetActive(false);
                break;
            case 4:
                GlobalData.indexModule = 4;
                ChangePage(52);//103
                OnChangePage();
                shadowLeft.SetActive(true);
                btn_changeCover.SetActive(false);
                break;
            case 5:
                GlobalData.indexModule = 5;
                ChangePage(66);
                OnChangePage();
                shadowLeft.SetActive(true);
                btn_changeCover.SetActive(false);
                break;
        }
    }

    public void ViewBook()
    {
        panelAnimation.SetActive(false);
        if (!DatabaseManager.Instance.isTest){
            //loginGroup.SetActive(true);
            DatabaseManager.Instance.userId = "prueba1";
            GlobalData.Password = "Prueba123";

            StartCoroutine(DatabaseManager.Instance.SetLogin());
        }
            
        //book.gameObject.GetComponent<AutoFlip2>().FlipRightPage();
    }

    public void ViewShadowLeft()
    {
        if (book.CurrentPaper > 0)
        {
            shadowLeft.SetActive(true);
        }
    }

    public void ViewShadowRight()
    {
        if (book.CurrentPaper < book.papers.Length)
        {
            shadowRight.SetActive(true);
        }
    }

    public void PlayVideo()
    {
        panelVideo.SetActive(true);

        Application.ExternalCall("PlayVideo", video_url);
    }

    public void StopVideo()
    {
        panelVideo.SetActive(false);
    }
    
    public void ChangePage(int page)
    {
        book.CurrentPaper = page;

       /* PageFlipTime = refPageFlipTime;
        AutoFlip2 flipper = book.GetComponent<AutoFlip2>();
        int pageNum = page;
        if (pageNum < 0) pageNum = 0;
        if (pageNum > flipper.ControledBook.papers.Length * 2) pageNum = flipper.ControledBook.papers.Length * 2 - 1;
        book.GetComponent<AutoFlip2>().enabled = true;
        flipper.ControledBook.interactable = true;
        flipper.PageFlipTime = PageFlipTime;
        flipper.TimeBetweenPages = 0;
        flipper.StartFlipping((pageNum + 1) / 2);*/
    }

    public void ChangeCover()
    {
        if (currentCover < sprite_cover.Length - 1)
            currentCover++;
        else
            currentCover = 0;

        coverBook.sprite = sprite_cover[currentCover];
        coverBook2.sprite = sprite_cover2[currentCover];

        txt_user[0].gameObject.SetActive(false);
        txt_user[1].gameObject.SetActive(false);
        txt_user[2].gameObject.SetActive(false);

        txt_user[currentCover].gameObject.SetActive(true);
    }

    public void OnStartChangePage()
    {
        OnFlipPage(true, book.CurrentPaper);

        if (book.CurrentPaper == 1)
        {
            shadowLeft.SetActive(false);
            btn_changeCover.SetActive(false);
        }

        if (book.CurrentPaper >= book.papers.Length)
        {
            shadowRight.SetActive(false);
            topBar.SetActive(false);
        }
        else
        {
            shadowRight.SetActive(true);
        }
    }
    
    public void OpenURL(string url)
    {
        Application.ExternalCall("OpenUrl", url);
    }
    #endregion
}
