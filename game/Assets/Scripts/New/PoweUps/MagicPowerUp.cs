using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicPowerUp : PowerUp
{
    //public Inventory playerInventory;
   // public GenericMana genericMana;
    public MagicPowerUp(string type, int price) : base(type, price)
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerHealth")&& collision.isTrigger)
        {
            collision.transform.parent.GetComponentInChildren<GenericMana>().AddMana(1);


            Destroy(this.gameObject);
        }
    }
}
