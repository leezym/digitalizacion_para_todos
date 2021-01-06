using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PhrasesPADDragged : MonoBehaviour
{
    public Transform[] groupSlots;

    public GameObject[] slots;
    public GameObject panelWriteActivity;

    public TMP_InputField[] txt_Activity;
    public TextMeshProUGUI[] drag_PAD_Activity;

    public GameObject panelPAD;
    public TextMeshProUGUI txt_PAD_Info;

    public GameObject panelMasInfo;
    public TextMeshProUGUI txt_info;

    string[] str_activity;

    private void Start()
    {
        str_activity = new string[txt_Activity.Length];
    }

    public void LoadData()
    {
        foreach (GameObject component in slots)
        {
            component.SetActive(false);
        }

        if (DatabaseManager.Instance.MatrixActivity != null)
        {
            for (int i = 0; i < txt_Activity.Length; i++)
            {
                if (DatabaseManager.Instance.MatrixActivity[i] != null)
                {
                    if (!string.IsNullOrEmpty(DatabaseManager.Instance.MatrixActivity[i].text))
                    {
                        DatabaseManager.Instance.MatrixActivity[i].text = DatabaseManager.Instance.MatrixActivity[i].text.Replace("\"", "");
                        DatabaseManager.Instance.MatrixActivity[i].text = DatabaseManager.Instance.MatrixActivity[i].text.Replace("\'", "");

                        str_activity[i] = DatabaseManager.Instance.MatrixActivity[i].text;
                        txt_Activity[i].text = str_activity[i];
                        drag_PAD_Activity[i].text = str_activity[i].Length > 20 ? string.Format("{0}...", str_activity[i].Substring(0, 18)) : str_activity[i];

                        if (!string.IsNullOrEmpty(str_activity[i]))
                        {
                            drag_PAD_Activity[i].transform.parent.gameObject.SetActive(true);

                            if (DatabaseManager.Instance.MatrixActivity[i].target == 0)
                            {
                                //drag_PAD_Activity[i].transform.parent.gameObject.transform.SetParent(groupSlots[0]);
                            }
                            else if (DatabaseManager.Instance.MatrixActivity[i].target == 1)
                            {
                                drag_PAD_Activity[i].transform.parent.gameObject.transform.SetParent(groupSlots[1]);
                            }
                            else if (DatabaseManager.Instance.MatrixActivity[i].target == 2)
                            {
                                drag_PAD_Activity[i].transform.parent.gameObject.transform.SetParent(groupSlots[2]);
                            }
                            else if (DatabaseManager.Instance.MatrixActivity[i].target == 3)
                            {
                                drag_PAD_Activity[i].transform.parent.gameObject.transform.SetParent(groupSlots[3]);
                            }
                            else if (DatabaseManager.Instance.MatrixActivity[i].target == 4)
                            {
                                drag_PAD_Activity[i].transform.parent.gameObject.transform.SetParent(groupSlots[4]);
                            }
                        }
                    }
                }
            }
        }
    }
    
    public void SaveData()
    {
        foreach (GameObject component in slots)
        {
            component.SetActive(false);
        }

        for (int i = 0; i < txt_Activity.Length; i++)
        {
            txt_Activity[i].text = txt_Activity[i].text.Replace("\"", "");
            txt_Activity[i].text = txt_Activity[i].text.Replace("\'", "");

            str_activity[i] = txt_Activity[i].text;
            drag_PAD_Activity[i].text = str_activity[i].Length > 20 ? string.Format("{0}...", str_activity[i].Substring(0, 18)) : str_activity[i];

            if (!string.IsNullOrEmpty(str_activity[i]))
            {
                drag_PAD_Activity[i].transform.parent.gameObject.SetActive(true);
            }

            DatabaseManager.Instance.MatrixActivity[i].text = str_activity[i];
        }

        SavePositionData();

        panelWriteActivity.SetActive(false);
    }

    public void SavePositionData()
    {
        for (int i = 0; i < txt_Activity.Length; i++)
        {
            if (slots[i].transform.parent.name == "Slots")
            {
                DatabaseManager.Instance.MatrixActivity[i].target = 0;
            }
            else if (slots[i].transform.parent.name == "Slot-I")
            {
                DatabaseManager.Instance.MatrixActivity[i].target = 1;
            }
            else if (slots[i].transform.parent.name == "Slot-II")
            {
                DatabaseManager.Instance.MatrixActivity[i].target = 2;
            }
            else if (slots[i].transform.parent.name == "Slot-III")
            {
                DatabaseManager.Instance.MatrixActivity[i].target = 3;
            }
            else if (slots[i].transform.parent.name == "Slot-IV")
            {
                DatabaseManager.Instance.MatrixActivity[i].target = 4;
            }
        }
    }

    public void LoadPad(int index)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if(slots[i].transform.parent.name == "Slots")
            {
                txt_Activity[i].interactable = true;
            }
            else
            {
                txt_Activity[i].interactable = false;
            }
        }

        txt_PAD_Info.text = GlobalData.PAD_IdentificaYSenala;

        panelWriteActivity.SetActive(true);
    }

    public void ShowInfo(int index)
    {
        panelMasInfo.SetActive(true);

        txt_info.text = str_activity[index];
    }
}
