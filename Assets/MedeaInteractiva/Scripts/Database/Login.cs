using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Globalization;
using System.Text.RegularExpressions;

public class Login : MonoBehaviour
{
    private static Login instance;

    public GameObject login;
    public GameObject contentSearchUser;
    public GameObject register;
    public GameObject contentForgotPassword;
    public GameObject forgotPassword;
    public GameObject verifyCode;
    public GameObject changePassword;

    public TMP_InputField input_Register_User;
    public TMP_InputField input_Register_Password;
    public TMP_InputField input_Verify_Register_Password;

    public TMP_InputField input_nickname;
    public TMP_InputField input_password;
    public TMP_InputField input_forgotEmail;
    public TMP_InputField input_verifyCode;
    public TMP_InputField input_changePassword;
    public TMP_InputField input_changePassword2;

    public Button btn_login;
    public Button btn_search_user;
    public Button btn_register;
    public Button btn_login_mini;
    public Button btn_login_mini2;
    public Button btn_login_mini3;
    public Button btn_register_mini;
    public Button btn_forgotPassword;
    public Button btn_forgotPassword_mini;
    public Button btn_verifyCode_mini;
    public Button btn_verifyCode;
    public Button btn_back_verifiCode;
    public Button btn_changePassword;
    public Button btn_back_changePassword;

    public GameObject popupCorrectRegisterPassword;
    public GameObject forgetPasswordOk;
    public GameObject passwordChange;

    public GameObject canvasLogin;

    public TextMeshProUGUI text_info;
    public TextMeshProUGUI text_info2;

    [SerializeField]
    private bool isValidatedPassword;
    [SerializeField]
    private bool isValidatedUser;
    [SerializeField]
    private bool isValidatedEmail;

    private bool isPassword;

    private string GetRandomCharacter(int stringLength = 6)
    {
        int _stringLength = stringLength - 1;
        string randomString = "";
        string[] characters = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
        for (int i = 0; i <= _stringLength; i++)
        {
            randomString += characters[Random.Range(0, characters.Length)];
        }
        return randomString;
    }

    public static Login Instance { get => instance; set => instance = value; }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        if (PlayerPrefs.HasKey("codeForgotPassword"))
        {
            if (!string.IsNullOrEmpty(PlayerPrefs.GetString("codeForgotPassword")))
            {
                GlobalData.CodeForgotPassword = PlayerPrefs.GetString("codeForgotPassword");
            }
        }

        btn_search_user.onClick.AddListener(OnButtonSearchPressed);
        btn_register.onClick.AddListener(OnButtonRegisterPressed);
        btn_register_mini.onClick.AddListener(OpenRegisterGroup);
        btn_login.onClick.AddListener(OnButtonLoginPressed);
        btn_login_mini.onClick.AddListener(OpenLoginGroup);
        btn_forgotPassword.onClick.AddListener(OnButtonForgotPasswordPressed);
        btn_login_mini2.onClick.AddListener(OpenLoginGroup);
        btn_login_mini3.onClick.AddListener(OpenLoginGroup);
        btn_forgotPassword_mini.onClick.AddListener(OpenForgotPassword);
        btn_verifyCode.onClick.AddListener(OnButtonVerifyCodePressed);
        btn_verifyCode_mini.onClick.AddListener(OpenVerifyCode);
        btn_back_verifiCode.onClick.AddListener(OpenForgotPassword);
        btn_changePassword.onClick.AddListener(OnButtonChangePasswordPressed);
        btn_back_changePassword.onClick.AddListener(OpenForgotPassword);
    }

    public void OpenLoginGroup()
    {
        OnError("");
        login.SetActive(true);
        contentSearchUser.SetActive(false);
        register.SetActive(false);
        forgotPassword.SetActive(false);
        btn_login_mini.gameObject.SetActive(false);
        btn_login.gameObject.SetActive(true);
        btn_forgotPassword_mini.gameObject.SetActive(true);
        contentForgotPassword.SetActive(false);
        changePassword.SetActive(false);
        verifyCode.SetActive(false);
    }

    public void OpenRegisterGroup()
    {
        OnError("");
        login.SetActive(false);
        contentSearchUser.SetActive(true);
        register.SetActive(false);
        btn_forgotPassword_mini.gameObject.SetActive(false);
        contentForgotPassword.SetActive(false);
    }

    public void OpenForgotPassword()
    {
        btn_forgotPassword.interactable = true;
        login.SetActive(false);
        contentForgotPassword.SetActive(true);
        verifyCode.SetActive(false);
        forgotPassword.SetActive(true);
        changePassword.SetActive(false);
        OnError("");
    }

    public void OpenVerifyCode()
    {
        OnError("");
        if (string.IsNullOrEmpty(GlobalData.CodeForgotPassword))
        {
            OnError("!Debe completar el campo Correo y presionar el botón ENVIAR para recibir un nuevo código!");
        }
        forgotPassword.SetActive(false);
        verifyCode.SetActive(true);
    }

    public void OnCorrectUser()
    {
        OnError("");
        contentSearchUser.SetActive(false);
        register.SetActive(true);
    }

    public void OnRegisterPassword()
    {
        OnError("");
        OpenLoginGroup();
        StartCoroutine(PopupRegister());
    }

    public void OpenChangePassword()
    {
        OnError("");
        verifyCode.SetActive(false);
        changePassword.SetActive(true);
    }

    public void OnButtonSearchPressed()
    {
        OnError("");

        if (string.IsNullOrEmpty(input_Register_User.text))
        {
            OnError("Debe completar el campo.");
            return;
        }

        DatabaseManager.Instance.userId = input_Register_User.text;

        StartCoroutine(DatabaseManager.Instance.CheckUser());
    }

    public void OnButtonRegisterPressed()
    {
        OnError("");

        if (string.IsNullOrEmpty(input_Register_Password.text) || string.IsNullOrEmpty(input_Verify_Register_Password.text))
        {
            OnError("Debe completar todos los campos.");
            return;
        }

        if (input_Register_Password.text != input_Verify_Register_Password.text)
        {
            OnError("Las contraseñas no coinciden.");
            return;
        }

        if (!isValidatedPassword)
        {
            OnError("La contraseña no cumple con los requisitos.");
            return;
        }

        GlobalData.Password = input_Register_Password.text;

        StartCoroutine(DatabaseManager.Instance.Register());
    }

    void OnButtonLoginPressed()
    {
        OnError("");

        /*if (string.IsNullOrEmpty(input_nickname.text) || string.IsNullOrEmpty(input_password.text))
        {
            return;
        }*/

        DatabaseManager.Instance.userId = input_nickname.text;
        GlobalData.Password = input_password.text;

        StartCoroutine(DatabaseManager.Instance.SetLogin());
    }

    void OnButtonForgotPasswordPressed()
    {
        OnError("");

        if (string.IsNullOrEmpty(input_forgotEmail.text))
        {
            OnError("Debe completar el correo.");
            return;
        }

        if (!isValidatedEmail)
        {
            OnError("El correo no cumple con los requisitos.");
            return;
        }

        btn_forgotPassword.interactable = false;

        DatabaseManager.Instance.userId = input_forgotEmail.text;
        GlobalData.CodeForgotPassword = GetRandomCharacter().ToUpperInvariant();

        StartCoroutine(DatabaseManager.Instance.SendCode());
    }


    void OnButtonVerifyCodePressed()
    {
        OnError("");

        if (string.IsNullOrEmpty(input_verifyCode.text))
        {
            OnError("Por favor ingrese el código que se le envió al correo.");
            return;
        }

        if (input_verifyCode.text.ToUpperInvariant() != GlobalData.CodeForgotPassword.ToUpperInvariant())
        {
            OnError("Código incorrecto.");
            return;
        }

        OpenChangePassword();
    }

    void OnButtonChangePasswordPressed()
    {
        OnError("");

        if (string.IsNullOrEmpty(input_changePassword.text) || string.IsNullOrEmpty(input_changePassword2.text))
        {
            OnError("Debe completar todos los campos.");
            return;
        }

        if (input_changePassword.text != input_changePassword2.text)
        {
            OnError("Las contraseñas no coinciden.");
            return;
        }

        if (!isValidatedPassword)
        {
            OnError("La contraseña no cumple con los requisitos.");
            return;
        }

        GlobalData.Password = input_changePassword.text;

        StartCoroutine(DatabaseManager.Instance.ChangePassword());
    }

    IEnumerator PopupRegister()
    {
        popupCorrectRegisterPassword.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        popupCorrectRegisterPassword.SetActive(false);
    }

    IEnumerator PopupForgetPassword()
    {
        forgetPasswordOk.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        forgetPasswordOk.SetActive(false);
    }

    IEnumerator PopupChangePassword()
    {
        passwordChange.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        passwordChange.SetActive(false);
    }

    public void OnLoginUser()
    {
        OnError("");

        PlayerPrefs.SetString("codeForgotPassword", "");

        canvasLogin.SetActive(false);
    }

    public void OnVerifyCode()
    {
        OnError("");
        OpenVerifyCode();
        StartCoroutine(PopupForgetPassword());
    }

    public void OnChangePassword()
    {
        OnError("");
        login.SetActive(true);
        forgotPassword.SetActive(false);
        verifyCode.SetActive(false);
        contentForgotPassword.SetActive(false);
        StartCoroutine(PopupChangePassword());
    }

    public void OnError(string text)
    {
        text_info.text = text;
        text_info2.text = text;
    }

    public void ValidateUser(TMP_InputField input)
    {
        isValidatedUser = IsValidUsername(input.text);

        if (!isValidatedUser)
        {
            OnError("");
        }
        else
        {
            OnError("");
        }
    }

    public void ValidateEmail(TMP_InputField input)
    {
        isValidatedEmail = IsvalidEmailId(input.text);

        if (!isValidatedEmail)
        {
            OnError("");
        }
        else
        {
            OnError("");
        }
    }

    public void ValidatePassword(TMP_InputField input)
    {
        isValidatedPassword = IsValidPassword(input.text);
    }

    public static bool IsValidPassword(string inputPassword)
    {
        //string pattern = @"([a-zA-Z0-9._-]{1,40})[@]([a-zA-Z0-9._-]{2,20})[.]([a-zA-Z0-9._-]{2,20})";
        Regex pattern = new Regex(@"^(?=\w*\d)(?=\w*[A-Z])(?=\w*[a-z])\S{8,16}$");
        Match match = pattern.Match(inputPassword);
        return match.Success;
    }

    public static bool IsvalidEmailId(string inputEmail)
    {
        Regex pattern = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,5})+)$");
        Match match = pattern.Match(inputEmail);
        return match.Success;
    }
    public static bool IsValidUsername(string inputUsername)
    {
        Regex pattern = new Regex("^[a-zA-Z]{8,12}$");
        Match match = pattern.Match(inputUsername);
        return match.Success;
    }

    public void ToggleTerms(Toggle toggle)
    {
        //btn_register.gameObject.SetActive(toggle.isOn);
    }

    public void Terminos()
    {
        Application.OpenURL("https://info.planinternational.org.pe/hubfs/Documentos%20-%20Plan%20International/Terminos%20y%20Condiciones_Plan_International.pdf");
    }
}
