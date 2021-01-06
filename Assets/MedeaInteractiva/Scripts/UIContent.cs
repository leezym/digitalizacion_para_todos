using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BookCurlPro;

public class UIContent : MonoBehaviour
{
    #region LOGIN
    public GameObject loginGroup;

    public TextMeshProUGUI[] txt_name;
    public GameObject feedBackConnection;
    public GameObject feedBackIcon_Good;
    public GameObject feedBackIcon_Wrong;
    public TextMeshProUGUI feedbackName;
    public TextMeshProUGUI infoFeedbackText;


    #endregion LOGIN

    #region BOOK
    [Header("Book")]
    public BookPro book;
    public GameObject topBar;
    public GameObject btn_changeCover;
    public GameObject shadowLeft;
    public GameObject shadowRight;
    public Button[] btn_topbar;
    public Button btn_next;
    public Button btn_prev;
    public Image leftHotSpot;
    public Image rightHotSpot;
    public Image coverBook;
    public Image coverBook2;
    public Sprite[] sprite_cover;
    public Sprite[] sprite_cover2;
    public TextMeshProUGUI[] txt_user;
    #endregion BOOK

    #region MODULO 1
    [Header("MODULO 1")]
    public GameObject popupReflexion;
    public GameObject contentInputReflexion;
    public GameObject background_popup;
    public GameObject hover_reflexion;
    public GameObject tutorial1;
    public GameObject tutorial2;
    public TMP_InputField input_PAD_Identifica;
    public TMP_InputField input_PAD_DefinePropositos;
    public TMP_InputField input_PAD_IdentificaYSenala;
    public TMP_InputField input_PAD_EstableceIndicadores;
    public TMP_InputField input_Reflexion;
    public TMP_InputField input_Objetivo_curso;

    public Button btn_pad_open;
    public Sprite spr_pad_saved;
    public Button btn_pad_save;
    public Button btn_Reflexion;
    public Sprite spr_reflexion_saved;
    public Button btn_Objetivo_curso;
    public Button btn_popup_reflexion;
    public GameObject txt_contenido_habito_ok;
    public GameObject panelVideo;
    public GameObject panelAnimation;
    public GameObject btn_lider;
    public Animator animator_reflexion;
    public AudioSource audioMeditation;
    public Image audioOff1;

    public ScrollSnapRect scrollSnapPAD;
    #endregion MODULO 1

    #region MODULO 2
    [Header("MODULO 2")]
    public TMP_InputField input_VUCA_Oportunidades_A;
    public TMP_InputField input_VUCA_Oportunidades_B;
    public TMP_InputField input_VUCA_Tecnica_A,     input_VUCA_Tecnica_B;
    public TMP_InputField input_VUCA_Sintomas_A,    input_VUCA_Sintomas_B;
    public TMP_InputField input_VUCA_Iniciativa_A,  input_VUCA_Iniciativa_B;
    public TMP_InputField input_VUCA_Reflexion;
    public GameObject popupReflexionVUCA;
    public Button btn_vuca_Reflexion;
    public Button btn_vuca_open;
    public Button btn_vuca_save;
    public Sprite spr_vuca_saved;
    public Image audioOff2;

    public ScrollSnapRect scrollSnapVUCA;
    public InventoryPieces inventoryPiecesExerciseVUCA;
    public PhrasesPADDragged phrasesPAD;
    #endregion MODULO 2

    #region MODULO 3
    [Header("MODULO 3")]
    public TMP_InputField inputFieldEncuesta;
    public TMP_InputField input_SesgosAreaExperiencia;
    public TMP_InputField input_Mod3_DeclaracionLiderazgo_A,    input_Mod3_DeclaracionLiderazgo_B;
    public TMP_InputField input_Mod3_SesgosInconscientes_A,     input_Mod3_SesgosInconscientes_B;
    public TMP_InputField input_Mod3_PersonasIdentificadas_A,   input_Mod3_PersonasIdentificadas_B;
    public TMP_InputField input_Mod3_Decisiones_A,              input_Mod3_Decisiones_B;
    public TMP_InputField input_Mod3_Reflexion;

    public GameObject encuesta_M3;
    public GameObject pergamino_M3;
    public GameObject pergaminoAnim;
    public GameObject ejercicio_M3;
    public GameObject popupReflexionMod3;

    public GameObject btn_encuesta_M3;
    public GameObject btn_reflexion_M3;
    public GameObject btn_ejercicio_Open_M3;
    public GameObject btn_ejercicio_Save_M3;
    public GameObject btn_Sesgo_Save_Mod3;

    public ScrollSnapRect scrollSnapMod3;
    public Image audioOff3;
    #endregion MODULO 3

    #region MODULO 4
    [Header("MODULO 4")]
    public TMP_InputField input_Mod4_Pregunta1_A;
    public TMP_InputField input_Mod4_Pregunta1_B;
    public TMP_InputField input_Mod4_Pregunta2_A, input_Mod4_Pregunta2_B;
    public TMP_InputField input_Mod4_Pregunta3_A, input_Mod4_Pregunta3_B;
    public TMP_InputField input_Mod4_Pregunta4_A, input_Mod4_Pregunta4_B;
    public TMP_InputField input_Mod4_Pregunta5_A, input_Mod4_Pregunta5_B;
    public TMP_InputField input_Mod4_Pregunta6_A, input_Mod4_Pregunta6_B;
    public TMP_InputField input_Mod4_Reflexion;

    public GameObject ejercicio_M4;
    public GameObject popupReflexionMod4;

    public GameObject btn_reflexion_M4;
    public GameObject btn_ejercicio_Open_M4;
    public GameObject btn_ejercicio_Save_M4;

    public ScrollSnapRect scrollSnapMod4;
    public Image audioOff4;
    #endregion MODULO 4

    #region MODULO 5
    [Header("MODULO 5")]
    public TMP_InputField input_Mod5_Pregunta1_A;
    public TMP_InputField input_Mod5_Pregunta1_B;
    public TMP_InputField input_Mod5_Pregunta2_A, input_Mod5_Pregunta2_B;
    public TMP_InputField input_Mod5_Pregunta3_A, input_Mod5_Pregunta3_B;
    public TMP_InputField input_Mod5_Reflexion;

    public GameObject ejercicio_M5;
    public GameObject popupReflexionMod5;

    public GameObject btn_reflexion_M5;
    public GameObject btn_ejercicio_Open_M5;
    public GameObject btn_ejercicio_Save_M5;

    public ScrollSnapRect scrollSnapMod5;
    public Image audioOff5;
    #endregion MODULO 5
}
