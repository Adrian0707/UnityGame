using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Experimental.Rendering.LWRP;

public class CreateLoot : MonoBehaviour
{

    private GameObjectsSystem gameObjectsSystem;
    private EnemiesSystem enemiesSystem;
    [Header("Prefabs")]
    public GameObject emptyItemName;
    public GameObject dropSlider;
    public GameObject colorSlider;
    public GameObject lootOption;
    public GameObject nameSaved;
    [Header("UI")]

    public TextMeshProUGUI infoText;
    public TextMeshProUGUI profitText;
    public TextMeshProUGUI powerText;
    public GameObject itemContent;
    public GameObject dropContent;
    public GameObject savedContent;
    public TMP_InputField inputField;
    public Button save;
    public CreateEnemy createEnemy;
    [HideInInspector] public int power;
    [HideInInspector] public int profit;

    public LootTable currentLootTable;
    private void OnEnable()
    {
        powerText.text = "Power " + power;
        enemiesSystem = GameObject.FindGameObjectWithTag("EnemiesSystem").GetComponent<EnemiesSystem>();
        gameObjectsSystem = GameObject.FindGameObjectWithTag("GameObjectsSystem").GetComponent<GameObjectsSystem>();


        inputField.onValueChanged.AddListener((value) => { CanBeSaved(); });
        currentLootTable = new LootTable(new List<Loot>());

        foreach (Transform child in itemContent.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in dropContent.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in savedContent.transform)
        {
            Destroy(child.gameObject);
        }

        GameObject empty;
        foreach (GameObject up in gameObjectsSystem.DropItems)
        {
            empty = Instantiate(emptyItemName);
            empty.transform.SetParent(itemContent.transform, false);
            empty.GetComponent<Image>().color = Color.Lerp(Color.red, Color.white, 0.8f);
            empty.transform.Find("Text").GetComponent<Text>().text = up.name;
            empty.transform.Find("Image").GetComponent<Image>().sprite = up.GetComponent<SpriteRenderer>().sprite;
            empty.GetComponent<Button>().onClick.AddListener(() =>
            {
                desctiptionSetAndButton(up.GetComponent<PowerUp>(), up.GetComponent<SpriteRenderer>().sprite);
            });
        }
    }
    public void desctiptionSetAndButton(PowerUp up, Sprite sprite)
    {

        Loot loot = new Loot();
        loot.thisLoot = up;
        currentLootTable.loots.Add(loot);
        CanBeSaved();
        Loot param = loot;
        GameObject empty;
        empty = Instantiate(dropSlider);
        empty.transform.SetParent(dropContent.transform, false);
        empty.GetComponent<Image>().color = Color.Lerp(Color.blue, Color.white, 0.8f);
        empty.transform.Find("Text").GetComponent<Text>().text = param.thisLoot.name;
        empty.transform.Find("Image").GetComponent<Image>().sprite = sprite;

        SetValue(empty, param);
        empty.transform.Find("SliderChance").GetComponent<Slider>().onValueChanged.AddListener((value) => { SetValue(empty, param); });
        empty.transform.Find("SliderAmount").GetComponent<Slider>().onValueChanged.AddListener((value) => { SetValue(empty, param); });
        empty.transform.Find("Delete").GetComponent<Button>().onClick.AddListener(() => DeleteLootObject(empty, param));
    }
    public void SetValue(GameObject gameObject, Loot loot)
    {
        loot.lootChance = (int)gameObject.transform.Find("SliderChance").GetComponent<Slider>().value;
        gameObject.transform.Find("ValueChance").GetComponent<Text>().text = "Chance " + gameObject.transform.Find("SliderChance").GetComponent<Slider>().value.ToString() + " %";
        loot.amount = (int)gameObject.transform.Find("SliderAmount").GetComponent<Slider>().value;
        gameObject.transform.Find("ValueAmount").GetComponent<Text>().text = "Amount " + gameObject.transform.Find("SliderAmount").GetComponent<Slider>().value.ToString();
        CanBeSaved();
    }
    public void CloseWindow()
    {
        this.gameObject.SetActive(false);
    }
    public void OpenWindow()
    {
        this.gameObject.SetActive(true);
    }
    public void DeleteLootObject(GameObject gameObject, Loot loot)
    {
        Destroy(gameObject);
        currentLootTable.loots.Remove(loot);
        CanBeSaved();
    }
    public void CanBeSaved()
    {
        GetProfit();

        if (string.IsNullOrEmpty(inputField.text) || currentLootTable.loots.Count < 1 || !ChanceSumIsCorrect() || profit > power)
        {
            if (string.IsNullOrEmpty(inputField.text))
            {
                infoText.text = "Name is empty";
            }
            save.interactable = false;
        }
        else
        {
            infoText.text = "";
            save.interactable = true;
        }
    }
    public void SaveLootTable()
    {
        currentLootTable.name = inputField.text;
        createEnemy.ChangeLootTable(currentLootTable, profit);
        CloseWindow();
    }
    private void GetProfit()
    {
        profit = 0;
        foreach (var item in currentLootTable.loots)
        {

            profit += item.amount * item.thisLoot.price * item.lootChance;
        }
        profitText.text = "Profit: " + profit;
        if (profit > power)
        {
            infoText.text = "Profit " + profit + " cant be >" + power + " power";
        }
    }
    private bool ChanceSumIsCorrect()
    {
        int chanceSum = 0;
        foreach (var item in currentLootTable.loots)
        {
            chanceSum += item.lootChance;
        }
        if (chanceSum <= 100)
        {
            return true;
        }
        else
        {
            infoText.text = "Chance sum is " + chanceSum + " must be < 100";
            return false;
        }
    }
}
