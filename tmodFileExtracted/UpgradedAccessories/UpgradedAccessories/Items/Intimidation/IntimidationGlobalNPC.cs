using Terraria;
using Terraria.ModLoader;

namespace UpgradedAccessories.Items.Intimidation {
    public class IntimidationGlobalNPC : GlobalNPC {

        public bool seared;
        public bool rotten;
        public bool webbed;
        public bool poisoned;
        public bool intimidated;

        public override bool InstancePerEntity => true;

        public override void ResetEffects(NPC npc) {
            seared = false;
            rotten = false;
            webbed = false;
            poisoned = false;
            intimidated = false;
        }

        public override void AI(NPC npc) {
            if(webbed) {
                if(intimidated) {
                    npc.position -= npc.velocity * 0.15f;
                } else {
                    npc.position -= npc.velocity * 0.05f;
                }
            }
        }

        public override void UpdateLifeRegen(NPC npc, ref int damage) {
            if(poisoned) {
                if(npc.lifeRegen > 0) npc.lifeRegen = 0;
                if(intimidated) {
                    npc.lifeRegen -= 30;
                    if(damage < 5) damage = 5;
                } else {
                    npc.lifeRegen -= 10;
                    if(damage < 2) damage = 2;
                }
            }
        }

        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref int damage, ref float knockback, ref bool crit) {
            if(rotten) {
                if(intimidated) {
                    damage = damage * 6 / 5;
                } else {
                    damage = damage * 21 / 20;
                }
            }
        }

        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) {
            if(rotten) {
                if(intimidated) {
                    damage = damage * 6 / 5;
                } else {
                    damage = damage * 21 / 20;
                }
            }
        }

        public override void ModifyHitNPC(NPC npc, NPC target, ref int damage, ref float knockback, ref bool crit) {
            if(seared) {
                if(intimidated) {
                    damage = damage * 4 / 5;
                } else {
                    damage = damage * 19 / 20;
                }
            }
        }

    }
}