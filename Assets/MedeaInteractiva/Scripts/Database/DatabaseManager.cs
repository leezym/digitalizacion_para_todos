using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;
using CryptSharp;
using System.Text.RegularExpressions;

//Mail
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

using UE.Email;

public class DatabaseManager : Database
{
    private static DatabaseManager instance;

    public static DatabaseManager Instance { get => instance; set => instance = value; }

    private JsonData jsonData;

    #region Mod2
    public CustomizedString[] VUCA_Oportunidades;
    public CustomizedString[] VUCA_Tecnica;
    public CustomizedString[] VUCA_Sintomas;
    public CustomizedString[] VUCA_Iniciativa;
    public CustomizedString[] MatrixActivity;
    #endregion

    #region Mod3
    public CustomizedString[] Mod3_DeclaracionLiderazgo;
    public CustomizedString[] Mod3_SesgosInconscientes;
    public CustomizedString[] Mod3_PersonasIdentificadas;
    public CustomizedString[] Mod3_Decisiones;

    public EncuestaM3 encuestaM3;
    #endregion

    #region Mod4
    public CustomizedString[] Mod4_Pregunta1;
    public CustomizedString[] Mod4_Pregunta2;
    public CustomizedString[] Mod4_Pregunta3;
    public CustomizedString[] Mod4_Pregunta4;
    public CustomizedString[] Mod4_Pregunta5;
    public CustomizedString[] Mod4_Pregunta6;
    #endregion

    #region Mod5
    public CustomizedString[] Mod5_Pregunta1;
    public CustomizedString[] Mod5_Pregunta2;
    public CustomizedString[] Mod5_Pregunta3;
    #endregion

    void Awake()
    {
        if (instance == null)
        {
            instance = this; DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        jsonData = new JsonData();
    }

    #region Encriptar
    //https://www.the-art-of-web.com/php/blowfish-crypt/
    //Referencia PHP
    //<?PHP
    //  $new_password = "<user input or randomly generated>";
    //  $password_hash = better_crypt($new_password); // or password_hash($new_password, PASSWORD_DEFAULT);
    //  // => store in database
    //  ?>

    //  ...

    //  <?PHP
    //  $password_entered = "<user input at login>";
    //  $password_hash = "<retrieved from database based on username>";
    //  if(password_verify($password_entered, $password_hash)) {
    //    // password is correct
    //  }
    //?>
    ////Fin referencia PHP

    //Referencia c#
    //string salt = BlowfishCrypter.Blowfish.GenerateSalt(
    //new CrypterOptions()
    //{
    //    {CrypterOption.Variant,BlowfishCrypterVariant.Corrected },
    //    {CrypterOption.Rounds,10 }
    //});
    //Debug.Log("Salt: " + salt);
    //    string result = BlowfishCrypter.Blowfish.Crypt(password, salt);
    //Debug.Log("Crypt: " + result);

    //    bool isRight = BlowfishCrypter.CheckPassword(password, result);
    //Debug.Log("Check: " + isRight);
    //Fin referencia c#

    public string CrypterPassword(string password)
    {
        string salt = BlowfishCrypter.Blowfish.GenerateSalt(
        new CrypterOptions()
        {
        {CrypterOption.Variant,BlowfishCrypterVariant.Corrected },
        {CrypterOption.Rounds,10 }
        });

        string result = BlowfishCrypter.Blowfish.Crypt(password, salt);

        return result;
    }

    public bool VerifyPassword(string password, string blowfishCrypter)
    {
        bool isRight = BlowfishCrypter.CheckPassword(password, blowfishCrypter);
        return isRight;
    }

    #endregion

    public IEnumerator CheckUser()
    {
        if (connecting)
        {
            //Debug.Log("¡Hay una conexión en curso!");
            yield break;
        }

        Login.Instance.OnError("");

        connecting = true;
        onConnection = false;

        string hash = Md5Sum(userId + secretKey);

        string parameters =
            "&userId=" + userId +
            "&hash=" + hash;

        using (UnityWebRequest webRequest = WebRequestGET(url[0], parameters))
        {
            if (webRequest == null)
                yield break;

            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                //Debug.Log("Conexión fallida: " + webRequest.downloadHandler.text);
                Login.Instance.OnError("¡Error de conexión!");
                onConnection = false;
                connecting = false;
            }
            else
            {
                if (webRequest.downloadHandler.text == "Usuario encontrado.")
                {
                    yield return new WaitForSeconds(0.01f);

                    Login.Instance.OnCorrectUser();
                }
                else if (webRequest.downloadHandler.text == "Usuario no encontrado.")
                {
                    Login.Instance.OnError("¡El usuario no existe!");
                }
                else
                {
                    Login.Instance.OnError("¡El usuario ya tiene contraseña!");
                }

                onConnection = true;
                connecting = false;
            }
        }
    }

    public IEnumerator Register()
    {
        if (connecting)
        {
            //Debug.Log("¡Hay una conexión en curso!");
            yield break;
        }

        Login.Instance.OnError("");

        connecting = true;
        onConnection = false;

        string hash = Md5Sum(userId + secretKey);

        string blowfishCrypter = CrypterPassword(GlobalData.Password);

        string parameters =
            "&userId=" + userId +
            "&password=" + blowfishCrypter +
            "&hash=" + hash;

        using (UnityWebRequest webRequest = WebRequestGET(url[1], parameters))
        {
            if (webRequest == null)
                yield break;

            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                //Debug.Log("Conexión fallida: " + webRequest.downloadHandler.text);
                Login.Instance.OnError("¡Error de conexión!");
                onConnection = false;
                connecting = false;
            }
            else
            {
                if (webRequest.downloadHandler.text == "Contraseña registrada.")
                {
                    yield return new WaitForSeconds(0.01f);

                    Login.Instance.OnRegisterPassword();
                }
                else
                {
                    Login.Instance.OnError("¡El usuario ya tiene contraseña!");
                }

                onConnection = true;
                connecting = false;
            }
        }
    }

    public IEnumerator SetLogin()
    {
        if (connecting)
        {
            //Debug.Log("¡Hay una conexión en curso!");
            yield break;
        }

        Login.Instance.OnError("");

        connecting = true;
        onConnection = false;

        string hash = Md5Sum(userId + secretKey);

        //WWWForm form = new WWWForm();
        //form.AddField("userId", userId);
        //form.AddField("password", GlobalData.Password);
        //form.AddField("hash", hash);

        string form = "&userId=" + userId + "&password=" + GlobalData.Password + "&hash=" + hash;

        using (UnityWebRequest webRequest = WebRequestGET(url[2], form))
        {
            if (webRequest == null)
                yield break;

            yield return webRequest.SendWebRequest();
            //Debug.Log("Conexión: " + webRequest.downloadHandler.text);
            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                //Debug.Log("Conexión fallida: " + webRequest.downloadHandler.text);
                Login.Instance.OnError("¡Error de conexión!");
                onConnection = false;
                connecting = false;
            }
            else
            {
                if (webRequest.downloadHandler.text == "Usuario no encontrado.")
                {
                    Login.Instance.OnError("Usuario no encontrado.");
                }
                else if (webRequest.downloadHandler.text == "Contraseña incorrecta")
                {
                    Login.Instance.OnError("Contraseña incorrecta.");
                }
                else
                {
                    string[] _result = webRequest.downloadHandler.text.Split('#');
                    if (VerifyPassword(GlobalData.Password, _result[0]))
                    {
                        /////////MODULO 1////////////////
                        GlobalData.PasswordEncrypted = _result[0];
                        GlobalData.Pais = _result[1];
                        GlobalData.PAD_Identifica = _result[2];
                        GlobalData.PAD_DefinePropositos = _result[3];
                        GlobalData.PAD_IdentificaYSenala = _result[4];
                        GlobalData.PAD_EstableceIndicadores = _result[5];
                        GlobalData.Reflexion = _result[6];
                        GlobalData.Objetivo_curso = _result[7];
                        GlobalData.Estado = _result[8];
                        GlobalData.TypeUser = _result[9];
                        GlobalData.Nombre = _result[10];
                        /////////FIN MODULO 1////////////////
                        /////////MODULO 2////////////////
                        if (!string.IsNullOrEmpty(_result[11]))
                        {
                            VUCA_Oportunidades = jsonData.LoadCustomizedString(_result[11]); //JsonHelper.getJsonArray<CustomizedString>(_result[10]); 
                        }
                        if (!string.IsNullOrEmpty(_result[12]))
                        {
                            VUCA_Tecnica = jsonData.LoadCustomizedString(_result[12]);
                        }
                        if (!string.IsNullOrEmpty(_result[13]))
                        {
                            VUCA_Sintomas = jsonData.LoadCustomizedString(_result[13]);
                        }
                        if (!string.IsNullOrEmpty(_result[14]))
                        {
                            VUCA_Iniciativa = jsonData.LoadCustomizedString(_result[14]);
                        }
                        GlobalData.VUCA_Reflexion = _result[15];
                        GlobalData.EstadoM2 = _result[16];

                        string[] vuca_check = _result[17].Split('|');

                        GlobalData.LineaDeTiempoCMVCompletado = true ? vuca_check[0] == "1" : false;
                        GlobalData.SignificadoDeTerminoCompletado = true ? vuca_check[1] == "1" : false;
                        GlobalData.MatrizImpactoEsfuerzoCompletado = true ? vuca_check[2] == "1" : false;
                        GlobalData.MindFulnessCompletado = true ? vuca_check[3] == "1" : false;
                        GlobalData.EjercicioMatrizCompletado = true ? vuca_check[4] == "1" : false;

                        if (!string.IsNullOrEmpty(_result[18]))
                            MatrixActivity = jsonData.LoadCustomizedString(_result[18]);
                        /////////FIN MODULO 2////////////////
                        ////////////MODULO 3////////////////
                        GlobalData.sesgosAreaExperiencia = _result[19];

                        if (!string.IsNullOrEmpty(_result[20]))
                        {
                            Mod3_DeclaracionLiderazgo = jsonData.LoadCustomizedString(_result[20]);
                        }
                        if (!string.IsNullOrEmpty(_result[21]))
                        {
                            Mod3_SesgosInconscientes = jsonData.LoadCustomizedString(_result[21]);
                        }
                        if (!string.IsNullOrEmpty(_result[22]))
                        {
                            Mod3_PersonasIdentificadas = jsonData.LoadCustomizedString(_result[22]);
                        }
                        if (!string.IsNullOrEmpty(_result[23]))
                        {
                            Mod3_Decisiones = jsonData.LoadCustomizedString(_result[23]);
                        }

                        GlobalData.Mod3_Reflexion = _result[24];

                        if (!string.IsNullOrEmpty(_result[25]))
                        {
                            encuestaM3 = jsonData.LoadSurveyM3(_result[25]);
                        }

                        string[] mod3_check = _result[26].Split('|');

                        GlobalData.EstadoM3 = _result[27];

                        GlobalData.EncuestaM3Completada = true ? mod3_check[0] == "1" : false;
                        GlobalData.PergaminoVisto = true ? mod3_check[1] == "1" : false;
                        GlobalData.SesgoCompletado = true ? mod3_check[2] == "1" : false;
                        GlobalData.EjercicioM3Completado = true ? mod3_check[3] == "1" : false;
                        GlobalData.ReflexionM3Completada = true ? mod3_check[4] == "1" : false;

                        /////////FIN MODULO 3////////////////
                        ////////////MODULO 4////////////////

                        if (!string.IsNullOrEmpty(_result[28]))
                        {
                            Mod4_Pregunta1 = jsonData.LoadCustomizedString(_result[28]);
                        }
                        if (!string.IsNullOrEmpty(_result[29]))
                        {
                            Mod4_Pregunta2 = jsonData.LoadCustomizedString(_result[29]);
                        }
                        if (!string.IsNullOrEmpty(_result[30]))
                        {
                            Mod4_Pregunta3 = jsonData.LoadCustomizedString(_result[30]);
                        }
                        if (!string.IsNullOrEmpty(_result[31]))
                        {
                            Mod4_Pregunta4 = jsonData.LoadCustomizedString(_result[31]);
                        }
                        if (!string.IsNullOrEmpty(_result[32]))
                        {
                            Mod4_Pregunta5 = jsonData.LoadCustomizedString(_result[32]);
                        }
                        if (!string.IsNullOrEmpty(_result[33]))
                        {
                            Mod4_Pregunta6 = jsonData.LoadCustomizedString(_result[33]);
                        }

                        GlobalData.Mod4_Reflexion = _result[34];

                        string[] mod4_check = _result[35].Split('|');

                        GlobalData.EstadoM4 = _result[36];

                        GlobalData.EjercicioM4Completado = true ? mod4_check[0] == "1" : false;
                        GlobalData.ReflexionM4Completada = true ? mod4_check[1] == "1" : false;

                        /////////FIN MODULO 4///////////////
                        ////////////MODULO 5////////////////

                        if (!string.IsNullOrEmpty(_result[37]))
                        {
                            Mod5_Pregunta1 = jsonData.LoadCustomizedString(_result[37]);
                        }
                        if (!string.IsNullOrEmpty(_result[38]))
                        {
                            Mod5_Pregunta2 = jsonData.LoadCustomizedString(_result[38]);
                        }
                        if (!string.IsNullOrEmpty(_result[39]))
                        {
                            Mod5_Pregunta3 = jsonData.LoadCustomizedString(_result[39]);
                        }

                        GlobalData.Mod5_Reflexion = _result[40];

                        string[] mod5_check = _result[41].Split('|');

                        GlobalData.EstadoM5 = _result[42];

                        GlobalData.EjercicioM5Completado = true ? mod5_check[0] == "1" : false;
                        GlobalData.ReflexionM5Completada = true ? mod5_check[1] == "1" : false;

                        /////////FIN MODULO 5///////////////

                        yield return new WaitForSeconds(0.01f);

                        connecting = false;
                        BookManager.Instance.LoadData();
                    }
                    else
                    {
                        Login.Instance.OnError("Contraseña incorrecta.");
                    }
                }

                onConnection = true;
                connecting = false;
            }
        }
    }

    //public IEnumerator UpdateData()
    //{
    //    if (connecting)
    //    {
    //        Debug.Log("¡Hay una conexión en curso!");
    //        yield break;
    //    }

    //    BookManager.Instance.OnError("");

    //    connecting = true;
    //    onConnection = false;

    //     string hash = Md5Sum(userId + secretKey);

    //     string[] vuca_check = new string[5];
    //     if(GlobalData.LineaDeTiempoCMVCompletado)
    //         vuca_check[0] = "1";
    //     else
    //         vuca_check[0] = "0";
    //     if (GlobalData.SignificadoDeTerminoCompletado)
    //         vuca_check[1] = "1";
    //     else
    //         vuca_check[1] = "0";
    //     if (GlobalData.MatrizImpactoEsfuerzoCompletado)
    //         vuca_check[2] = "1";
    //     else
    //         vuca_check[2] = "0";
    //     if (GlobalData.MindFulnessCompletado)
    //         vuca_check[3] = "1";
    //     else
    //         vuca_check[3] = "0";
    //     if (GlobalData.EjercicioMatrizCompletado)
    //         vuca_check[4] = "1";
    //     else
    //         vuca_check[4] = "0";
    //     string save_check = string.Join("|", vuca_check);

    //    ///////////////////////////M3//////////////////////////////
    //    string[] mod3_check = new string[5];
    //    if (GlobalData.EncuestaM3Completada)
    //        mod3_check[0] = "1";
    //    else
    //        mod3_check[0] = "0";
    //    if (GlobalData.PergaminoVisto)
    //        mod3_check[1] = "1";
    //    else
    //        mod3_check[1] = "0";
    //    if (GlobalData.SesgoCompletado)
    //        mod3_check[2] = "1";
    //    else
    //        mod3_check[2] = "0";
    //    if (GlobalData.EjercicioM3Completado)
    //        mod3_check[3] = "1";
    //    else
    //        mod3_check[3] = "0";
    //    if (GlobalData.ReflexionM3Completada)
    //        mod3_check[4] = "1";
    //    else
    //        mod3_check[4] = "0";
    //    string save_check_m3 = string.Join("|", mod3_check);

    //    ///////////////////////////M3//////////////////////////////

    //    string VUCA_Oportunidades = jsonData.GetCustomizedString(this.VUCA_Oportunidades);
    //    string VUCA_Tecnica = jsonData.GetCustomizedString(this.VUCA_Tecnica);
    //    string VUCA_Sintomas = jsonData.GetCustomizedString(this.VUCA_Sintomas);
    //    string VUCA_Iniciativa = jsonData.GetCustomizedString(this.VUCA_Iniciativa);
    //    string matrixActivity = jsonData.GetCustomizedString(this.MatrixActivity);

    //    var Unescape_VUCA_Oportunidades = Regex.Unescape(VUCA_Oportunidades);
    //    var Unescape_VUCA_Tecnica = Regex.Unescape(VUCA_Tecnica);
    //    var Unescape_VUCA_Sintomas = Regex.Unescape(VUCA_Sintomas);
    //    var Unescape_VUCA_Iniciativa = Regex.Unescape(VUCA_Iniciativa);
    //    var Unescape_matrixActivity = Regex.Unescape(matrixActivity);

    //    string encuestaM3 = jsonData.GetSurveyM3(this.encuestaM3);
    //    string Unescape_encuestaM3 = Regex.Unescape(encuestaM3);

    //    string Mod3_DeclaracionLiderazgo = jsonData.GetCustomizedString(this.Mod3_DeclaracionLiderazgo);
    //    string Mod3_SesgosInconscientes = jsonData.GetCustomizedString(this.Mod3_SesgosInconscientes);
    //    string Mod3_PersonasIdentificadas = jsonData.GetCustomizedString(this.Mod3_PersonasIdentificadas);
    //    string Mod3_Decisiones = jsonData.GetCustomizedString(this.Mod3_Decisiones);

    //    var Unescape_Mod3_DeclaracionLiderazgo = Regex.Unescape(Mod3_DeclaracionLiderazgo);
    //    var Unescape_Mod3_SesgosInconscientes = Regex.Unescape(Mod3_SesgosInconscientes);
    //    var Unescape_Mod3_PersonasIdentificadas = Regex.Unescape(Mod3_PersonasIdentificadas);
    //    var Unescape_Mod3_Decisiones = Regex.Unescape(Mod3_Decisiones);

    //    WWWForm form = new WWWForm();
    //    form.AddField("userId", userId);
    //    form.AddField("password", GlobalData.PasswordEncrypted);
    //    form.AddField("pad_Identifica", GlobalData.PAD_Identifica);
    //    form.AddField("pad_DefinePropositos", GlobalData.PAD_DefinePropositos);
    //    form.AddField("pad_IdentificaYSenala", GlobalData.PAD_IdentificaYSenala);
    //    form.AddField("pad_EstableceIndicadores", GlobalData.PAD_EstableceIndicadores);
    //    form.AddField("reflexion", GlobalData.Reflexion);
    //    form.AddField("objetivo_curso", GlobalData.Objetivo_curso);
    //    form.AddField("estado", GlobalData.Estado);
    //    form.AddField("vuca_Oportunidades", Unescape_VUCA_Oportunidades);
    //    form.AddField("vuca_Tecnica", Unescape_VUCA_Tecnica);
    //    form.AddField("vuca_Sintomas", Unescape_VUCA_Sintomas);
    //    form.AddField("vuca_Iniciativa", Unescape_VUCA_Iniciativa);
    //    form.AddField("vuca_Reflexion", GlobalData.VUCA_Reflexion);
    //    form.AddField("vuca_check", save_check);
    //    form.AddField("estado_M2", GlobalData.EstadoM2);
    //    form.AddField("matrixActivities", Unescape_matrixActivity);
    //    form.AddField("mod3_sesgosAreaExperiencia", GlobalData.sesgosAreaExperiencia);
    //    form.AddField("mod3_DeclaracionLiderazgo", Unescape_Mod3_DeclaracionLiderazgo);
    //    form.AddField("mod3_SesgosInconscientes", Unescape_Mod3_SesgosInconscientes);
    //    form.AddField("mod3_PersonasIdentificadas", Unescape_Mod3_PersonasIdentificadas);
    //    form.AddField("mod3_Decisiones", Unescape_Mod3_Decisiones);
    //    form.AddField("mod3_Reflexion", GlobalData.Mod3_Reflexion);
    //    form.AddField("mod3_Encuesta", Unescape_encuestaM3);
    //    form.AddField("mod3_check", save_check_m3);
    //    form.AddField("estado_M3", GlobalData.EstadoM3);
    //    form.AddField("hash", hash);

    //    using (UnityWebRequest webRequest = WebRequestPOST(url[3], form))
    //     {
    //         if (webRequest == null)
    //             yield break;

    //         yield return webRequest.SendWebRequest();
    //       // Debug.Log("Conexión: " + webRequest.downloadHandler.text);
    //        if (webRequest.isNetworkError || webRequest.isHttpError)
    //         {
    //             //Debug.Log("Conexión fallida: " + webRequest.downloadHandler.text);
    //             BookManager.Instance.OnError("¡Error de conexión!");
    //             onConnection = false;
    //             connecting = false;
    //         }
    //         else
    //         {
    //             //Debug.Log("Conexión establecida: " + webRequest.downloadHandler.text);
    //             onConnection = true;
    //             connecting = false;
    //         }
    //     }
    //}

    public IEnumerator UpdateData(bool check = false)
    {
        if (GlobalData.indexModule == 1)
        {
            StartCoroutine(UpdateM1(check));
        }
        if (GlobalData.indexModule == 2)
        {
            StartCoroutine(UpdateM2(check));
        }
        if (GlobalData.indexModule == 3)
        {
            StartCoroutine(UpdateM3(check));
        }
        if (GlobalData.indexModule == 4)
        {
            StartCoroutine(UpdateM4(check));
        }
        if (GlobalData.indexModule == 5)
        {
            StartCoroutine(UpdateM5(check));
        }

        yield return null;
    }

    public IEnumerator UpdateM1(bool check)
    {
        connectionError = false;

        if (connecting)
        {
            Debug.Log("¡Hay una conexión en curso!");
            yield break;
        }

        Login.Instance.OnError("");

        connecting = true;
        onConnection = false;

        string hash = Md5Sum(userId + secretKey);

        WWWForm form = new WWWForm();
        form.AddField("userId", userId);
        form.AddField("password", GlobalData.PasswordEncrypted);
        form.AddField("indexModule", GlobalData.indexModule);
        form.AddField("pad_Identifica", GlobalData.PAD_Identifica);
        form.AddField("pad_DefinePropositos", GlobalData.PAD_DefinePropositos);
        form.AddField("pad_IdentificaYSenala", GlobalData.PAD_IdentificaYSenala);
        form.AddField("pad_EstableceIndicadores", GlobalData.PAD_EstableceIndicadores);
        form.AddField("reflexion", GlobalData.Reflexion);
        form.AddField("objetivo_curso", GlobalData.Objetivo_curso);
        form.AddField("estado", GlobalData.Estado);
        form.AddField("hash", hash);

        using (UnityWebRequest webRequest = WebRequestPOST(url[3], form))
        {
            if (webRequest == null)
                yield break;

            yield return webRequest.SendWebRequest();
            // Debug.Log("Conexión: " + webRequest.downloadHandler.text);
            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                //Debug.Log("Conexión fallida: " + webRequest.downloadHandler.text);
                Login.Instance.OnError("¡Error de conexión!");
                connectionError = true;
                onConnection = false;
                connecting = false;
            }
            else
            {
                //Debug.Log("Conexión establecida: " + webRequest.downloadHandler.text);
                onConnection = true;
                connecting = false;

                if (check)
                {
                    StartCoroutine(CheckStatus(1));
                }
            }
        }
    }

    public IEnumerator UpdateM2(bool check)
    {
        connectionError = false;

        if (connecting)
        {
            Debug.Log("¡Hay una conexión en curso!");
            yield break;
        }

        Login.Instance.OnError("");

        connecting = true;
        onConnection = false;

        string hash = Md5Sum(userId + secretKey);

        string[] vuca_check = new string[5];
        if (GlobalData.LineaDeTiempoCMVCompletado)
            vuca_check[0] = "1";
        else
            vuca_check[0] = "0";
        if (GlobalData.SignificadoDeTerminoCompletado)
            vuca_check[1] = "1";
        else
            vuca_check[1] = "0";
        if (GlobalData.MatrizImpactoEsfuerzoCompletado)
            vuca_check[2] = "1";
        else
            vuca_check[2] = "0";
        if (GlobalData.MindFulnessCompletado)
            vuca_check[3] = "1";
        else
            vuca_check[3] = "0";
        if (GlobalData.EjercicioMatrizCompletado)
            vuca_check[4] = "1";
        else
            vuca_check[4] = "0";
        string save_check = string.Join("|", vuca_check);

        string VUCA_Oportunidades = jsonData.GetCustomizedString(this.VUCA_Oportunidades);
        string VUCA_Tecnica = jsonData.GetCustomizedString(this.VUCA_Tecnica);
        string VUCA_Sintomas = jsonData.GetCustomizedString(this.VUCA_Sintomas);
        string VUCA_Iniciativa = jsonData.GetCustomizedString(this.VUCA_Iniciativa);
        string matrixActivity = jsonData.GetCustomizedString(this.MatrixActivity);

        var Unescape_VUCA_Oportunidades = Regex.Unescape(VUCA_Oportunidades);
        var Unescape_VUCA_Tecnica = Regex.Unescape(VUCA_Tecnica);
        var Unescape_VUCA_Sintomas = Regex.Unescape(VUCA_Sintomas);
        var Unescape_VUCA_Iniciativa = Regex.Unescape(VUCA_Iniciativa);
        var Unescape_matrixActivity = Regex.Unescape(matrixActivity);

        WWWForm form = new WWWForm();
        form.AddField("userId", userId);
        form.AddField("password", GlobalData.PasswordEncrypted);
        form.AddField("indexModule", GlobalData.indexModule);
        form.AddField("vuca_Oportunidades", Unescape_VUCA_Oportunidades);
        form.AddField("vuca_Tecnica", Unescape_VUCA_Tecnica);
        form.AddField("vuca_Sintomas", Unescape_VUCA_Sintomas);
        form.AddField("vuca_Iniciativa", Unescape_VUCA_Iniciativa);
        form.AddField("vuca_Reflexion", GlobalData.VUCA_Reflexion);
        form.AddField("vuca_check", save_check);
        form.AddField("estado_M2", GlobalData.EstadoM2);
        form.AddField("matrixActivities", Unescape_matrixActivity);
        form.AddField("hash", hash);

        using (UnityWebRequest webRequest = WebRequestPOST(url[3], form))
        {
            if (webRequest == null)
                yield break;

            yield return webRequest.SendWebRequest();
            // Debug.Log("Conexión: " + webRequest.downloadHandler.text);
            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                //Debug.Log("Conexión fallida: " + webRequest.downloadHandler.text);
                Login.Instance.OnError("¡Error de conexión!");
                connectionError = true;
                onConnection = false;
                connecting = false;
            }
            else
            {
                //Debug.Log("Conexión establecida: " + webRequest.downloadHandler.text);
                onConnection = true;
                connecting = false;

                if (check)
                {
                    StartCoroutine(CheckStatus(2));
                }
            }
        }
    }

    public IEnumerator UpdateM3(bool check)
    {
        connectionError = false;

        if (connecting)
        {
            Debug.Log("¡Hay una conexión en curso!");
            yield break;
        }

        Login.Instance.OnError("");

        connecting = true;
        onConnection = false;

        string hash = Md5Sum(userId + secretKey);

        ///////////////////////////M3//////////////////////////////
        string[] mod3_check = new string[5];
        if (GlobalData.EncuestaM3Completada)
            mod3_check[0] = "1";
        else
            mod3_check[0] = "0";
        if (GlobalData.PergaminoVisto)
            mod3_check[1] = "1";
        else
            mod3_check[1] = "0";
        if (GlobalData.SesgoCompletado)
            mod3_check[2] = "1";
        else
            mod3_check[2] = "0";
        if (GlobalData.EjercicioM3Completado)
            mod3_check[3] = "1";
        else
            mod3_check[3] = "0";
        if (GlobalData.ReflexionM3Completada)
            mod3_check[4] = "1";
        else
            mod3_check[4] = "0";
        string save_check_m3 = string.Join("|", mod3_check);

        ///////////////////////////M3//////////////////////////////

        string encuestaM3 = jsonData.GetSurveyM3(this.encuestaM3);
        string Unescape_encuestaM3 = Regex.Unescape(encuestaM3);

        string Mod3_DeclaracionLiderazgo = jsonData.GetCustomizedString(this.Mod3_DeclaracionLiderazgo);
        string Mod3_SesgosInconscientes = jsonData.GetCustomizedString(this.Mod3_SesgosInconscientes);
        string Mod3_PersonasIdentificadas = jsonData.GetCustomizedString(this.Mod3_PersonasIdentificadas);
        string Mod3_Decisiones = jsonData.GetCustomizedString(this.Mod3_Decisiones);

        var Unescape_Mod3_DeclaracionLiderazgo = Regex.Unescape(Mod3_DeclaracionLiderazgo);
        var Unescape_Mod3_SesgosInconscientes = Regex.Unescape(Mod3_SesgosInconscientes);
        var Unescape_Mod3_PersonasIdentificadas = Regex.Unescape(Mod3_PersonasIdentificadas);
        var Unescape_Mod3_Decisiones = Regex.Unescape(Mod3_Decisiones);

        WWWForm form = new WWWForm();
        form.AddField("userId", userId);
        form.AddField("password", GlobalData.PasswordEncrypted);
        form.AddField("indexModule", GlobalData.indexModule);
        form.AddField("mod3_sesgosAreaExperiencia", GlobalData.sesgosAreaExperiencia);
        form.AddField("mod3_DeclaracionLiderazgo", Unescape_Mod3_DeclaracionLiderazgo);
        form.AddField("mod3_SesgosInconscientes", Unescape_Mod3_SesgosInconscientes);
        form.AddField("mod3_PersonasIdentificadas", Unescape_Mod3_PersonasIdentificadas);
        form.AddField("mod3_Decisiones", Unescape_Mod3_Decisiones);
        form.AddField("mod3_Reflexion", GlobalData.Mod3_Reflexion);
        form.AddField("mod3_Encuesta", Unescape_encuestaM3);
        form.AddField("mod3_check", save_check_m3);
        form.AddField("estado_M3", GlobalData.EstadoM3);
        form.AddField("hash", hash);

        using (UnityWebRequest webRequest = WebRequestPOST(url[3], form))
        {
            if (webRequest == null)
                yield break;

            yield return webRequest.SendWebRequest();
            // Debug.Log("Conexión: " + webRequest.downloadHandler.text);
            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                //Debug.Log("Conexión fallida: " + webRequest.downloadHandler.text);
                Login.Instance.OnError("¡Error de conexión!");
                connectionError = true;
                onConnection = false;
                connecting = false;
            }
            else
            {
                //Debug.Log("Conexión establecida: " + webRequest.downloadHandler.text);
                onConnection = true;
                connecting = false;

                if (check)
                {
                    StartCoroutine(CheckStatus(3));
                }
            }
        }
    }

    public IEnumerator UpdateM4(bool check)
    {
        connectionError = false;

        if (connecting)
        {
            Debug.Log("¡Hay una conexión en curso!");
            yield break;
        }

        Login.Instance.OnError("");

        connecting = true;
        onConnection = false;

        string hash = Md5Sum(userId + secretKey);

        ///////////////////////////M4//////////////////////////////
        string[] mod4_check = new string[2];

        if (GlobalData.EjercicioM4Completado)
            mod4_check[0] = "1";
        else
            mod4_check[0] = "0";
        if (GlobalData.ReflexionM4Completada)
            mod4_check[1] = "1";
        else
            mod4_check[1] = "0";

        string save_check_m4 = string.Join("|", mod4_check);

        ///////////////////////////M4//////////////////////////////

        string Mod4_Pregunta1 = jsonData.GetCustomizedString(this.Mod4_Pregunta1);
        string Mod4_Pregunta2 = jsonData.GetCustomizedString(this.Mod4_Pregunta2);
        string Mod4_Pregunta3 = jsonData.GetCustomizedString(this.Mod4_Pregunta3);
        string Mod4_Pregunta4 = jsonData.GetCustomizedString(this.Mod4_Pregunta4);
        string Mod4_Pregunta5 = jsonData.GetCustomizedString(this.Mod4_Pregunta5);
        string Mod4_Pregunta6 = jsonData.GetCustomizedString(this.Mod4_Pregunta6);


        var Unescape_Mod4_Pregunta1 = Regex.Unescape(Mod4_Pregunta1);
        var Unescape_Mod4_Pregunta2 = Regex.Unescape(Mod4_Pregunta2);
        var Unescape_Mod4_Pregunta3 = Regex.Unescape(Mod4_Pregunta3);
        var Unescape_Mod4_Pregunta4 = Regex.Unescape(Mod4_Pregunta4);
        var Unescape_Mod4_Pregunta5 = Regex.Unescape(Mod4_Pregunta5);
        var Unescape_Mod4_Pregunta6 = Regex.Unescape(Mod4_Pregunta6);


        WWWForm form = new WWWForm();
        form.AddField("userId", userId);
        form.AddField("password", GlobalData.PasswordEncrypted);
        form.AddField("indexModule", GlobalData.indexModule);
        form.AddField("mod4_Pregunta1", Unescape_Mod4_Pregunta1);
        form.AddField("mod4_Pregunta2", Unescape_Mod4_Pregunta2);
        form.AddField("mod4_Pregunta3", Unescape_Mod4_Pregunta3);
        form.AddField("mod4_Pregunta4", Unescape_Mod4_Pregunta4);
        form.AddField("mod4_Pregunta5", Unescape_Mod4_Pregunta5);
        form.AddField("mod4_Pregunta6", Unescape_Mod4_Pregunta6);
        form.AddField("mod4_Reflexion", GlobalData.Mod4_Reflexion);
        form.AddField("mod4_check", save_check_m4);
        form.AddField("estado_M4", GlobalData.EstadoM4);
        form.AddField("hash", hash);

        using (UnityWebRequest webRequest = WebRequestPOST(url[3], form))
        {
            if (webRequest == null)
                yield break;

            yield return webRequest.SendWebRequest();
            // Debug.Log("Conexión: " + webRequest.downloadHandler.text);
            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                //Debug.Log("Conexión fallida: " + webRequest.downloadHandler.text);
                Login.Instance.OnError("¡Error de conexión!");
                connectionError = true;
                onConnection = false;
                connecting = false;
            }
            else
            {
                //Debug.Log("Conexión establecida: " + webRequest.downloadHandler.text);
                onConnection = true;
                connecting = false;

                if (check)
                {
                    StartCoroutine(CheckStatus(4));
                }
            }
        }
    }

    public IEnumerator UpdateM5(bool check)
    {
        connectionError = false;

        if (connecting)
        {
            Debug.Log("¡Hay una conexión en curso!");
            yield break;
        }

        Login.Instance.OnError("");

        connecting = true;
        onConnection = false;

        string hash = Md5Sum(userId + secretKey);

        ///////////////////////////M5//////////////////////////////
        string[] mod5_check = new string[2];

        if (GlobalData.EjercicioM5Completado)
            mod5_check[0] = "1";
        else
            mod5_check[0] = "0";
        if (GlobalData.ReflexionM5Completada)
            mod5_check[1] = "1";
        else
            mod5_check[1] = "0";

        string save_check_m5 = string.Join("|", mod5_check);

        ///////////////////////////M5//////////////////////////////

        string Mod5_Pregunta1 = jsonData.GetCustomizedString(this.Mod5_Pregunta1);
        string Mod5_Pregunta2 = jsonData.GetCustomizedString(this.Mod5_Pregunta2);
        string Mod5_Pregunta3 = jsonData.GetCustomizedString(this.Mod5_Pregunta3);

        var Unescape_Mod5_Pregunta1 = Regex.Unescape(Mod5_Pregunta1);
        var Unescape_Mod5_Pregunta2 = Regex.Unescape(Mod5_Pregunta2);
        var Unescape_Mod5_Pregunta3 = Regex.Unescape(Mod5_Pregunta3);


        WWWForm form = new WWWForm();
        form.AddField("userId", userId);
        form.AddField("password", GlobalData.PasswordEncrypted);
        form.AddField("indexModule", GlobalData.indexModule);
        form.AddField("mod5_Pregunta1", Unescape_Mod5_Pregunta1);
        form.AddField("mod5_Pregunta2", Unescape_Mod5_Pregunta2);
        form.AddField("mod5_Pregunta3", Unescape_Mod5_Pregunta3);
        form.AddField("mod5_Reflexion", GlobalData.Mod5_Reflexion);
        form.AddField("mod5_check", save_check_m5);
        form.AddField("estado_M5", GlobalData.EstadoM5);
        form.AddField("hash", hash);

        using (UnityWebRequest webRequest = WebRequestPOST(url[3], form))
        {
            if (webRequest == null)
                yield break;

            yield return webRequest.SendWebRequest();
            // Debug.Log("Conexión: " + webRequest.downloadHandler.text);
            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                //BookManager.Instance.LoadFeedBackConnection(, false);
                //Debug.Log("Conexión fallida: " + webRequest.downloadHandler.text);
                Login.Instance.OnError("¡Error de conexión!");
                connectionError = true;
                onConnection = false;
                connecting = false;
            }
            else
            {
                //BookManager.Instance.LoadFeedBackConnection(, true);
                //Debug.Log("Conexión establecida: " + webRequest.downloadHandler.text);
                onConnection = true;
                connecting = false;

                if (check)
                {
                    StartCoroutine(CheckStatus(5));
                }
            }
        }
    }

    public IEnumerator CheckStatus(int index)
    {
        if (connecting)
        {
            Debug.Log("¡Hay una conexión en curso!");
            if (connectionError)
            {
                BookManager.Instance.LoadFeedBackConnection(false);
            }
            yield break;
        }

        Login.Instance.OnError("");

        connecting = true;
        onConnection = false;

        string hash = Md5Sum(userId + secretKey);

        WWWForm form = new WWWForm();
        form.AddField("userId", userId);
        form.AddField("indexModule", index);
        form.AddField("hash", hash);

        using (UnityWebRequest webRequest = WebRequestPOST(url[6], form))
        {
            if (webRequest == null)
                yield break;

            yield return webRequest.SendWebRequest();
            Debug.Log("Conexión: " + webRequest.downloadHandler.text);
            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                BookManager.Instance.LoadFeedBackConnection(false);
                //Debug.Log("Conexión fallida: " + webRequest.downloadHandler.text);
                Login.Instance.OnError("¡Error de conexión!");
                connectionError = true;
                onConnection = false;
                connecting = false;
            }
            else
            {
                bool estado = bool.Parse(webRequest.downloadHandler.text);
                BookManager.Instance.LoadFeedBackConnection(estado);
                connectionError = false;
                //Debug.Log("Conexión establecida: " + webRequest.downloadHandler.text);
                onConnection = true;
                connecting = false;
            }
        }
    }


    public IEnumerator ChangePassword()
    {
        if (connecting)
        {
            //Debug.Log("¡Hay una conexión en curso!");
            yield break;
        }

        Login.Instance.OnError("");

        connecting = true;
        onConnection = false;

        string hash = Md5Sum(userId + secretKey);

        string blowfishCrypter = CrypterPassword(GlobalData.Password);

        WWWForm form = new WWWForm();
        form.AddField("userId", userId);
        form.AddField("password", blowfishCrypter);
        form.AddField("hash", hash);

        using (UnityWebRequest webRequest = WebRequestPOST(url[4], form))
        {
            if (webRequest == null)
                yield break;

            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                //Debug.Log("Conexión fallida: " + webRequest.downloadHandler.text);
                Login.Instance.OnError("¡Error de conexión!");
                onConnection = false;
                connecting = false;
            }
            else
            {
                if (webRequest.downloadHandler.text == "Contraseña cambiada.")
                {
                    yield return new WaitForSeconds(0.01f);

                    Login.Instance.OnChangePassword();
                }
                else if (webRequest.downloadHandler.text == "El usuario no existe.")
                {
                    Login.Instance.OnError("¡El usuario no existe!");
                }
                else
                {
                    Login.Instance.OnError("¡El usuario no ha creado la contraseña!");
                }

                onConnection = true;
                connecting = false;
            }
        }
    }

    public IEnumerator SendCode()
    {
        if (connecting)
        {
            //Debug.Log("¡Hay una conexión en curso!");
            yield break;
        }

        Login.Instance.OnError("");

        connecting = true;
        onConnection = false;

        PlayerPrefs.SetString("codeForgotPassword", GlobalData.CodeForgotPassword);

        WWWForm form = new WWWForm();
        form.AddField("userId", userId);
        form.AddField("code", GlobalData.CodeForgotPassword);

        using (UnityWebRequest webRequest = WebRequestPOST(url[5], form))
        {
            if (webRequest == null)
                yield break;

            yield return webRequest.SendWebRequest();
            //Debug.Log("Conexión: " + webRequest.downloadHandler.text);
            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                Login.Instance.OnError("¡Error de conexión!");
                onConnection = false;
                connecting = false;
            }
            else
            {
                if (webRequest.downloadHandler.text == "Su correo no se encuentra en nuestra base de datos")
                {
                    Login.Instance.OnError("¡Su correo no se encuentra en nuestra base de datos!");
                }
                else if (webRequest.downloadHandler.text == "Problema con el envío de correo")
                {
                    Login.Instance.OnError("¡Problema con el envío de correo!");
                }
                else/* if(webRequest.downloadHandler.text == "Ok")*/
                {
                    Login.Instance.OnVerifyCode();

                    //SendEmail(userId, GlobalData.CodeForgotPassword);
                }

                Login.Instance.btn_forgotPassword.interactable = true;

                onConnection = true;
                connecting = false;
            }
        }
    }

    void SendEmail(string userId, string code)
    {
        string info = string.Format("{0}<br /><br />{1}{2}{3}<br /><br />{4}<br />{5}", "Hola,", "Ingresa el código de verificación ", code, " en el curso para restablecer la contraseña.", "Coordialmente,", "El equipo de Sura Lider Mentor.");

        /*MailMessage mail = new MailMessage();

        mail.IsBodyHtml = true;

        mail.From = new MailAddress("apps.medea@gmail.com");

        mail.To.Add(userId);

        mail.Subject = "Recuperación de contraseña";

        mail.Body = info;

        SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");

        smtpServer.Port = 587;

        smtpServer.Credentials = new System.Net.NetworkCredential("apps.medea@gmail.com", "Medea19780") as ICredentialsByHost;

        smtpServer.EnableSsl = true;

        ServicePointManager.ServerCertificateValidationCallback =

        delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)

        { return true; };

        smtpServer.Send(mail);

        Debug.Log("success");*/




        /*string email = "apps.medea@gmail.com";

        string subject = MyEscapeURL("jmedeagame@gmail.com");

        string body = MyEscapeURL(info + "\r\nFull of non-escaped chars");


        Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);*/


        //Email.SendEmail("apps.medea@gmail.com", userId, "Recuperación de contraseña", "Hola", "smtp.gmail.com", "apps.medea@gmail.com", "Medea19780");

        //Email.SendEmailToken("apps.medea@gmail.com", userId, "Recuperación de contraseña", info, "5d611839-256e-419a-abb2-bbecf2c01a20");

    }

    string MyEscapeURL(string url)
    {
        return WWW.EscapeURL(url).Replace("+", "%20");
    }
}
