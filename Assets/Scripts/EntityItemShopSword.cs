using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityItemShopSword : EntityItemShop {

    public float damage = 3f;
    public GameObject attackAnimation;

    public override string Name {
        get {
            return "Sword (" + damage.ToString("0.0") + " atk)";
        }
    }

    protected override void OnPurchase(EntityPlayer player) {
        Destroy(player.GetComponent<PlayerAttack>());
        PlayerAttackMelee pam = player.gameObject.AddComponent<PlayerAttackMelee>();
        pam.damage = this.damage;
        pam.attackAnimation = this.attackAnimation;
    }
}