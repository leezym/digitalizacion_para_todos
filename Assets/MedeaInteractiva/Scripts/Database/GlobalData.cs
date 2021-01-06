using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalData
{
    public static string Password;
    public static string PasswordEncrypted;
    public static string Nombre;
    public static string TypeUser;
    public static string Pais;

    public static int CurrentModule = 1;
    public static int indexModule = 1;

    public static string CodeForgotPassword;

    #region MODULO 1
    public static string PAD_Identifica;
    public static string PAD_DefinePropositos;
    public static string PAD_IdentificaYSenala;
    public static string PAD_EstableceIndicadores;
    public static string Reflexion;
    public static string Objetivo_curso;
    public static string Estado = "Sin iniciar";
    #endregion

    #region MODULO 2
    public static string VUCA_Reflexion;
    public static string EstadoM2 = "Sin iniciar";
    public static bool LineaDeTiempoCMVCompletado;
    public static bool SignificadoDeTerminoCompletado;
    public static bool MatrizImpactoEsfuerzoCompletado;
    public static bool MindFulnessCompletado;
    public static bool EjercicioMatrizCompletado;
    #endregion

    #region MODULO 3
    public static string sesgosAreaExperiencia;
    public static string Mod3_Reflexion;
    public static string EstadoM3 = "Sin iniciar";
    public static bool PergaminoVisto;
    public static bool EncuestaM3Completada;
    public static bool SesgoCompletado;
    public static bool ReflexionM3Completada;
    public static bool EjercicioM3Completado;
    #endregion

    #region MODULO 4
    public static string Mod4_Reflexion;
    public static string EstadoM4 = "Sin iniciar";
    public static bool ReflexionM4Completada;
    public static bool EjercicioM4Completado;
    #endregion

    #region MODULO 5
    public static string Mod5_Reflexion;
    public static string EstadoM5 = "Sin iniciar";
    public static bool ReflexionM5Completada;
    public static bool EjercicioM5Completado;
    #endregion
}