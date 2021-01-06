using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tools : MonoBehaviour
{
    public GameObject[] modules;

    [Header("Módulo 1")]
    public TextMeshProUGUI input_PAD_Identifica;
    public TextMeshProUGUI input_PAD_DefinePropositos;
    public TextMeshProUGUI input_PAD_IdentificaYSenala;
    public TextMeshProUGUI input_PAD_EstableceIndicadores;
    public TextMeshProUGUI input_Reflexion;
    public TextMeshProUGUI input_Objetivo_curso;

    [Header("Módulo 2")]
    public TextMeshProUGUI input_VUCA_Oportunidades;
    public TextMeshProUGUI input_VUCA_Tecnica;
    public TextMeshProUGUI input_VUCA_Sintomas;
    public TextMeshProUGUI input_VUCA_Iniciativa;
    public TextMeshProUGUI input_VUCA_Reflexion;

    [Header("Módulo 3")]
    public TextMeshProUGUI Encuesta_pregunta1;
    public TextMeshProUGUI Encuesta_pregunta2;
    public TextMeshProUGUI Encuesta_pregunta3;
    public TextMeshProUGUI inputFieldEncuesta;
    public TextMeshProUGUI input_SesgosAreaExperiencia;
    public TextMeshProUGUI input_Mod3_DeclaracionLiderazgo;
    public TextMeshProUGUI input_Mod3_SesgosInconscientes;
    public TextMeshProUGUI input_Mod3_PersonasIdentificadas;
    public TextMeshProUGUI input_Mod3_Decisiones;
    public TextMeshProUGUI input_Mod3_Reflexion;

    [Header("Módulo 4")]
    public TextMeshProUGUI input_Mod4_Pregunta1;
    public TextMeshProUGUI input_Mod4_Pregunta2;
    public TextMeshProUGUI input_Mod4_Pregunta3;
    public TextMeshProUGUI input_Mod4_Pregunta4;
    public TextMeshProUGUI input_Mod4_Pregunta5;
    public TextMeshProUGUI input_Mod4_Pregunta6;
    public TextMeshProUGUI input_Mod4_Reflexion;

    [Header("Módulo 5")]
    public TextMeshProUGUI input_Mod5_Pregunta1;
    public TextMeshProUGUI input_Mod5_Pregunta2;
    public TextMeshProUGUI input_Mod5_Pregunta3;
    public TextMeshProUGUI input_Mod5_Reflexion;


    private void OnEnable()
    {
        #region Modulo 1

        input_PAD_Identifica.text = GlobalData.PAD_Identifica;
        input_PAD_DefinePropositos.text = GlobalData.PAD_DefinePropositos;
        input_PAD_IdentificaYSenala.text = GlobalData.PAD_IdentificaYSenala;
        input_PAD_EstableceIndicadores.text = GlobalData.PAD_EstableceIndicadores;
        input_Reflexion.text = GlobalData.Reflexion;
        input_Objetivo_curso.text = GlobalData.Objetivo_curso;
        #endregion

        #region Modulo 2

        string oportA = "";
        string oportB = "";
        string tecnA = "";
        string tecnB = "";
        string sintA = "";
        string sintB = "";
        string inicA = "";
        string inicB = "";

        if (DatabaseManager.Instance.VUCA_Oportunidades[0] != null)
            oportA = DatabaseManager.Instance.VUCA_Oportunidades[0].text;

        if (DatabaseManager.Instance.VUCA_Oportunidades[1] != null)
            oportB = DatabaseManager.Instance.VUCA_Oportunidades[1].text;

        if (DatabaseManager.Instance.VUCA_Tecnica[0] != null)
            tecnA = DatabaseManager.Instance.VUCA_Tecnica[0].text;

        if (DatabaseManager.Instance.VUCA_Tecnica[1] != null)
            tecnB = DatabaseManager.Instance.VUCA_Tecnica[1].text;

        if (DatabaseManager.Instance.VUCA_Sintomas[0] != null)
            sintA = DatabaseManager.Instance.VUCA_Sintomas[0].text;

        if (DatabaseManager.Instance.VUCA_Sintomas[1] != null)
            sintB = DatabaseManager.Instance.VUCA_Sintomas[1].text;

        if (DatabaseManager.Instance.VUCA_Iniciativa[0] != null)
            inicA = DatabaseManager.Instance.VUCA_Iniciativa[0].text;

        if (DatabaseManager.Instance.VUCA_Iniciativa[1] != null)
            inicB = DatabaseManager.Instance.VUCA_Iniciativa[1].text;

        if (!string.IsNullOrEmpty(oportA))
            input_VUCA_Oportunidades.text = string.Format("{0}<br><br>{1}", oportA, oportB);
        else
            input_VUCA_Oportunidades.text = oportB;

        if (!string.IsNullOrEmpty(tecnA))
            input_VUCA_Tecnica.text = string.Format("{0}<br><br>{1}", tecnA, tecnB);
        else
            input_VUCA_Tecnica.text = tecnB;

        if (!string.IsNullOrEmpty(sintA))
            input_VUCA_Sintomas.text = string.Format("{0}<br><br>{1}", sintA, sintB);
        else
            input_VUCA_Sintomas.text = sintB;

        if (!string.IsNullOrEmpty(inicA))
            input_VUCA_Iniciativa.text = string.Format("{0}<br><br>{1}", inicA, inicB);
        else
            input_VUCA_Iniciativa.text = inicB;

        input_VUCA_Reflexion.text = GlobalData.VUCA_Reflexion;
        #endregion

        #region Modulo 3

        Encuesta_pregunta1.text = DatabaseManager.Instance.encuestaM3.answer[0];
        Encuesta_pregunta2.text = DatabaseManager.Instance.encuestaM3.answer[1];
        Encuesta_pregunta3.text = DatabaseManager.Instance.encuestaM3.answer[2];
        inputFieldEncuesta.text = DatabaseManager.Instance.encuestaM3.inputText;
        input_SesgosAreaExperiencia.text = GlobalData.sesgosAreaExperiencia;

        string mod3oportA = "";
        string mod3oportB = "";
        string mod3tecnA = "";
        string mod3tecnB = "";
        string mod3sintA = "";
        string mod3sintB = "";
        string mod3inicA = "";
        string mod3inicB = "";

        if (DatabaseManager.Instance.Mod3_DeclaracionLiderazgo[0] != null)
            mod3oportA = DatabaseManager.Instance.Mod3_DeclaracionLiderazgo[0].text;

        if (DatabaseManager.Instance.Mod3_DeclaracionLiderazgo[1] != null)
            mod3oportB = DatabaseManager.Instance.Mod3_DeclaracionLiderazgo[1].text;

        if (DatabaseManager.Instance.Mod3_SesgosInconscientes[0] != null)
            mod3tecnA = DatabaseManager.Instance.Mod3_SesgosInconscientes[0].text;

        if (DatabaseManager.Instance.Mod3_SesgosInconscientes[1] != null)
            mod3tecnB = DatabaseManager.Instance.Mod3_SesgosInconscientes[1].text;

        if (DatabaseManager.Instance.Mod3_PersonasIdentificadas[0] != null)
            mod3sintA = DatabaseManager.Instance.Mod3_PersonasIdentificadas[0].text;

        if (DatabaseManager.Instance.Mod3_PersonasIdentificadas[1] != null)
            mod3sintB = DatabaseManager.Instance.Mod3_PersonasIdentificadas[1].text;

        if (DatabaseManager.Instance.Mod3_Decisiones[0] != null)
            mod3inicA = DatabaseManager.Instance.Mod3_Decisiones[0].text;

        if (DatabaseManager.Instance.Mod3_Decisiones[1] != null)
            mod3inicB = DatabaseManager.Instance.Mod3_Decisiones[1].text;

        if (!string.IsNullOrEmpty(mod3oportA))
            input_Mod3_DeclaracionLiderazgo.text = string.Format("{0}<br><br>{1}", mod3oportA, mod3oportB);
        else
            input_Mod3_DeclaracionLiderazgo.text = mod3oportB;

        if (!string.IsNullOrEmpty(mod3tecnA))
            input_Mod3_SesgosInconscientes.text = string.Format("{0}<br><br>{1}", mod3tecnA, mod3tecnB);
        else
            input_Mod3_SesgosInconscientes.text = mod3tecnB;

        if (!string.IsNullOrEmpty(mod3sintA))
            input_Mod3_PersonasIdentificadas.text = string.Format("{0}<br><br>{1}", mod3sintA, mod3sintB);
        else
            input_Mod3_PersonasIdentificadas.text = mod3sintB;

        if (!string.IsNullOrEmpty(mod3inicA))   
            input_Mod3_Decisiones.text = string.Format("{0}<br><br>{1}", mod3inicA, mod3inicB);
        else
            input_Mod3_Decisiones.text = mod3inicB;

        input_Mod3_Reflexion.text = GlobalData.Mod3_Reflexion;
        #endregion

        #region Modulo 4

        string mod4pre1A= "";
        string mod4pre1B= "";
        string mod4pre2A= "";
        string mod4pre2B= "";
        string mod4pre3A= "";
        string mod4pre3B= "";
        string mod4pre4A= "";
        string mod4pre4B= "";
        string mod4pre5A= "";
        string mod4pre5B= "";
        string mod4pre6A= "";
        string mod4pre6B= "";

        if (DatabaseManager.Instance.Mod4_Pregunta1[0] != null)
            mod4pre1A = DatabaseManager.Instance.Mod4_Pregunta1[0].text;
        if (DatabaseManager.Instance.Mod4_Pregunta1[1] != null)
            mod4pre1B = DatabaseManager.Instance.Mod4_Pregunta1[1].text;

        if (DatabaseManager.Instance.Mod4_Pregunta2[0] != null)
            mod4pre2A = DatabaseManager.Instance.Mod4_Pregunta2[0].text;
        if (DatabaseManager.Instance.Mod4_Pregunta2[1] != null)
            mod4pre2B = DatabaseManager.Instance.Mod4_Pregunta2[1].text;

        if (DatabaseManager.Instance.Mod4_Pregunta3[0] != null)
            mod4pre3A = DatabaseManager.Instance.Mod4_Pregunta3[0].text;
        if (DatabaseManager.Instance.Mod4_Pregunta3[1] != null)
            mod4pre3B = DatabaseManager.Instance.Mod4_Pregunta3[1].text;

        if (DatabaseManager.Instance.Mod4_Pregunta4[0] != null)
            mod4pre4A = DatabaseManager.Instance.Mod4_Pregunta4[0].text;
        if (DatabaseManager.Instance.Mod4_Pregunta4[1] != null)
            mod4pre4B = DatabaseManager.Instance.Mod4_Pregunta4[1].text;

        if (DatabaseManager.Instance.Mod4_Pregunta5[0] != null)
            mod4pre5A = DatabaseManager.Instance.Mod4_Pregunta5[0].text;
        if (DatabaseManager.Instance.Mod4_Pregunta5[1] != null)
            mod4pre5B = DatabaseManager.Instance.Mod4_Pregunta5[1].text;

        if (DatabaseManager.Instance.Mod4_Pregunta6[0] != null)
            mod4pre6A = DatabaseManager.Instance.Mod4_Pregunta6[0].text;
        if (DatabaseManager.Instance.Mod4_Pregunta6[1] != null)
            mod4pre6B = DatabaseManager.Instance.Mod4_Pregunta6[1].text;

        if (!string.IsNullOrEmpty(mod4pre1A))
            input_Mod4_Pregunta1.text = string.Format("{0}<br><br>{1}", mod4pre1A, mod4pre1B);
        else
            input_Mod4_Pregunta1.text = mod4pre1B;

        if (!string.IsNullOrEmpty(mod4pre2A))
            input_Mod4_Pregunta2.text = string.Format("{0}<br><br>{1}", mod4pre2A, mod4pre2B);
        else
            input_Mod4_Pregunta2.text = mod4pre2B;

        if (!string.IsNullOrEmpty(mod4pre3A))
            input_Mod4_Pregunta3.text = string.Format("{0}<br><br>{1}", mod4pre3A, mod4pre3B);
        else
            input_Mod4_Pregunta3.text = mod4pre3B;

        if (!string.IsNullOrEmpty(mod4pre4A))
            input_Mod4_Pregunta4.text = string.Format("{0}<br><br>{1}", mod4pre4A, mod4pre4B);
        else
            input_Mod4_Pregunta4.text = mod4pre4B;

        if (!string.IsNullOrEmpty(mod4pre5A))
            input_Mod4_Pregunta5.text = string.Format("{0}<br><br>{1}", mod4pre5A, mod4pre5B);
        else
            input_Mod4_Pregunta5.text = mod4pre5B;

        if (!string.IsNullOrEmpty(mod4pre6A))
            input_Mod4_Pregunta6.text = string.Format("{0}<br><br>{1}", mod4pre6A, mod4pre6B);
        else
            input_Mod4_Pregunta6.text = mod4pre6B;

        input_Mod4_Reflexion.text = GlobalData.Mod4_Reflexion;
        #endregion

        #region Modulo 5

        string mod5pre1A = "";
        string mod5pre1B = "";
        string mod5pre2A = "";
        string mod5pre2B = "";
        string mod5pre3A = "";
        string mod5pre3B = "";

        if (DatabaseManager.Instance.Mod5_Pregunta1[0] != null)
            mod5pre1A = DatabaseManager.Instance.Mod5_Pregunta1[0].text;
        if (DatabaseManager.Instance.Mod5_Pregunta1[1] != null)
            mod5pre1B = DatabaseManager.Instance.Mod5_Pregunta1[1].text;

        if (DatabaseManager.Instance.Mod5_Pregunta2[0] != null)
            mod5pre2A = DatabaseManager.Instance.Mod5_Pregunta2[0].text;
        if (DatabaseManager.Instance.Mod5_Pregunta2[1] != null)
            mod5pre2B = DatabaseManager.Instance.Mod5_Pregunta2[1].text;

        if (DatabaseManager.Instance.Mod5_Pregunta3[0] != null)
            mod5pre3A = DatabaseManager.Instance.Mod5_Pregunta3[0].text;
        if (DatabaseManager.Instance.Mod5_Pregunta3[1] != null)
            mod5pre3B = DatabaseManager.Instance.Mod5_Pregunta3[1].text;


        if (!string.IsNullOrEmpty(mod5pre1A))
            input_Mod5_Pregunta1.text = string.Format("{0}<br><br>{1}", mod5pre1A, mod5pre1B);
        else
            input_Mod5_Pregunta1.text = mod5pre1B;

        if (!string.IsNullOrEmpty(mod5pre2A))
            input_Mod5_Pregunta2.text = string.Format("{0}<br><br>{1}", mod5pre2A, mod5pre2B);
        else
            input_Mod5_Pregunta2.text = mod5pre2B;

        if (!string.IsNullOrEmpty(mod5pre3A))
            input_Mod5_Pregunta3.text = string.Format("{0}<br><br>{1}", mod5pre3A, mod5pre3B);
        else
            input_Mod5_Pregunta3.text = mod5pre3B;

        input_Mod5_Reflexion.text = GlobalData.Mod5_Reflexion;
        #endregion

        LoadModule(GlobalData.CurrentModule - 1);
    }

    public void LoadModule(int index)
    {
        BookManager.Instance.feedBackConnection.SetActive(false);

        foreach(GameObject component in modules)
        {
            component.SetActive(false);
        }

        modules[index].SetActive(true);
    }
}
