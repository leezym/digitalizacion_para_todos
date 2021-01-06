[System.Serializable]
public class ContentData
{
    public int id;
    public string userId;
    public string password;
    public string nombre;
    public string typeUser;
    public string cargo;
    public string nombreMentor;
    public string correoMentor;
    public string pais;

    public string pad_Identifica;
    public string pad_DefinePropositos;
    public string pad_IdentificaYSenala;
    public string pad_EstableceIndicadores;
    public string reflexion;
    public string objetivo_curso;
    public string estado;

    public string[] vuca_Oportunidades = new string[2];
    public string[] vuca_Tecnica = new string[2];
    public string[] vuca_Sintomas = new string[2];
    public string[] vuca_Iniciativa = new string[2];
    public string vuca_Reflexion;
    public string vuca_check;
    public MatrixActivity[] matrixActivities;
    public string estado_M2;
}

[System.Serializable]
public class MatrixActivity
{
    public int position;
    public string activity;
}