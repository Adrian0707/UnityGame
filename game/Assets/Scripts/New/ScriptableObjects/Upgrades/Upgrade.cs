using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : ScriptableObject
{

    public string description;
    public Sprite sprite;
    public Sprite sprite2;
    public bool activated = false;
    public bool bought = false;
    public int cost;
    public Signal2 noResources;
    protected List<StatModifier> statModifiers = new List<StatModifier>();


    private void OnEnable()
    {
        if (activated)
        {
            this.Unequip();
            this.Equip();
        }
        else
        {
            this.Unequip();
        }
    }

    public void Buy()
    {
        Gui gui = GameObject.FindGameObjectWithTag("GUI").GetComponent<Gui>();
       // Debug.LogError("GUI" + gui.name);
        if (gui.coin >= cost)
        {
            gui.ModifyCoin(-cost);
            bought = true;
        }
        else
        {
            noResources.Raise();
        }
    }

    public virtual void Equip()
    {
        activated = true;

    }

    public virtual void Unequip()
    {
        activated = false;
    }
    public UpgradeSerializable ToSerializable()
    {
        return new UpgradeSerializable(this);
    }
}
