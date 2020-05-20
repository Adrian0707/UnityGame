using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CoinTextManager : MonoBehaviour
{
    public Inventory playerInventory;
    public TextMeshProUGUI coidDisplay;
    private void Start()
    {
        coidDisplay.text = "" + playerInventory.coins;
    }
    public void UpdateCoinCount()
    {
        coidDisplay.text = "" + playerInventory.coins;
    }
}
