using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerStat
{
    Health,
    Speed,
    Attack,
    stoneGetting,
    stoneDamage,
    woodGetting,
    woodDamage,
    mana
}
[System.Serializable]
public class PlayerUpgrde
{
    public int amount;
    public StatModType type;
    public PlayerStat playerStat;
}
[CreateAssetMenu(menuName = "Upgrades/Player")]
public class UpPlayer : Upgrade
{
    public PlayerStatistics playerStatistics;
    public PlayerUpgrde[] playerUpgrdes;
    public Signal2 manaUpdate;

    public override void Equip()
    {
        if (activated == false)
        {
            activated = true;
            foreach (var up in playerUpgrdes)
            {
                StatModifier statModifier = new StatModifier(up.amount, up.type, this);
                statModifiers.Add(statModifier);
                switch (up.playerStat)
                {
                    case PlayerStat.Health:
                        playerStatistics.health.AddModifier(statModifier);
                        break;
                    case PlayerStat.Speed:
                        playerStatistics.speed.AddModifier(statModifier);
                        break;
                    case PlayerStat.Attack:
                        playerStatistics.attack.AddModifier(statModifier);
                        break;
                    case PlayerStat.stoneGetting:
                        playerStatistics.stoneGetting.AddModifier(statModifier);
                        break;
                    case PlayerStat.stoneDamage:
                        playerStatistics.stoneDamage.AddModifier(statModifier);
                        break;
                    case PlayerStat.woodGetting:
                        playerStatistics.woodGetting.AddModifier(statModifier);
                        break;
                    case PlayerStat.woodDamage:
                        playerStatistics.woodDamage.AddModifier(statModifier);
                        break;
                    case PlayerStat.mana:
                        playerStatistics.mana.AddModifier(statModifier);
                        manaUpdate.Raise();
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public override void Unequip()
    {
        if (activated == true)
        {
            activated = false;

            playerStatistics.health.RemoveAllModifiersFromSorce(this);
            playerStatistics.attack.RemoveAllModifiersFromSorce(this);
            playerStatistics.speed.RemoveAllModifiersFromSorce(this);
            playerStatistics.stoneDamage.RemoveAllModifiersFromSorce(this);
            playerStatistics.stoneGetting.RemoveAllModifiersFromSorce(this);
            playerStatistics.woodDamage.RemoveAllModifiersFromSorce(this);
            playerStatistics.woodGetting.RemoveAllModifiersFromSorce(this);
            playerStatistics.mana.RemoveAllModifiersFromSorce(this);
            manaUpdate.Raise();

        }
    }
}