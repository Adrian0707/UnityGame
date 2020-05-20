using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScriptEditor : MonoBehaviour
{
    public TMP_InputField inputField;
    public Scripts scriptsCurrent;
    public Scripts scriptsDefaut;
    public GameObject scriptsHolder;
    public GameObject content;
    public Button saveButton;
    public Button resetButton;
    public Button applyButton;
    public Button inUseButton;
    private FieldInfo currentSelected;
    void Start()
    {
       //Gintut.text= Scripts.viligerInteraction;
    }
    private void Awake()
    {
        saveButton.interactable = false;
        resetButton.interactable =false;
        applyButton.interactable =false;
        inUseButton.interactable = false;

        foreach (Transform item in content.transform)
        {
            Destroy(item.gameObject);
        }
        foreach (var item in scriptsCurrent.GetType().GetFields())
        {
            if (!(item.FieldType.FullName == "System.Boolean"))
            {
                GameObject empty = Instantiate(scriptsHolder);
                empty.transform.SetParent(content.transform, false);
                empty.GetComponentInChildren<TextMeshProUGUI>().text = item.Name;
                empty.GetComponent<Button>().onClick.AddListener(() => ReadScriptToEdit(item));
            
            }

        }
    }
    public void ReadScriptToEdit(FieldInfo field)
    {
        inputField.text = (string)field.GetValue(scriptsCurrent);
        currentSelected = field;
        saveButton.interactable = true;
        resetButton.interactable = true;
        applyButton.interactable = true;
        inUseButton.interactable = true;
        inUsebuttonRefresh();
    }
    public void inUseButtonClick()
    {
        bool notInUse = (bool)scriptsCurrent.GetType().GetField(currentSelected.Name + "NormalMode").GetValue(scriptsCurrent);
        scriptsCurrent.GetType().GetField(currentSelected.Name + "NormalMode").SetValue(scriptsCurrent, !notInUse);
        inUsebuttonRefresh();
    }
    public void inUsebuttonRefresh()
    {
        if (!(bool)scriptsCurrent.GetType().GetField(currentSelected.Name + "NormalMode").GetValue(scriptsCurrent))
        {
            inUseButton.GetComponentInChildren<Text>().text = "in use";
            inUseButton.GetComponentInChildren<Text>().color = Color.green;

        }
        else
        {
            inUseButton.GetComponentInChildren<Text>().text = "not in use";
            inUseButton.GetComponentInChildren<Text>().color = Color.red;
        }
    }
    public void SaveScript()
    {
        currentSelected.SetValue(scriptsCurrent, inputField.text);
        SaveSystem.SaveScripts(scriptsCurrent);
    }
    public void ApplyScript()
    {
        currentSelected.SetValue(scriptsCurrent, inputField.text);
    }
    public void ResetScript()
    {
        //currentSelected.SetValue(scriptsCurrent, );
        inputField.text = (string)scriptsDefaut.GetType().GetField(currentSelected.Name).GetValue(scriptsDefaut);
    }
    
}
