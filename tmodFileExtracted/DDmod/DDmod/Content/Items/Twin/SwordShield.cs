using System;
using DDmod.Content.Projectiles.Melee.TwinSwords;
using DDmod.Content.Projectiles.Ranged.Gun;
using DDmod.Players;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using DDmod.Content.Buffs.DeBuffs;

namespace DDmod.Content.Items
{
	public abstract class Swordshield : ModItem
	{
		public virtual void Set()
		{

		}
        /// <summary>
        /// 防御倍率
        /// </summary>
        public int Magnification = 2;
        /// <summary>
        /// 格挡时间
        /// </summary>
        public int BlockTime = 30;
        /// <summary>
        /// 防御CD
        /// </summary>
        public int BlockCD = 120;
        /// <summary>
        /// 减伤
        /// </summary>
        public float Endurance = 0;

        public override void SetDefaults()
		{
			Item.DamageType = DamageClass.Melee;
			Item.autoReuse = true;
			Item.useStyle = 13;
			Item.noMelee = true;
			Item.UseSound = null;
			Item.channel = true;
			Item.noUseGraphic = true;
			Item.DItem().Twin = true;
            Item.GetGlobalItem<MeleeGlobalItem>().SwordandShield = true;
            Set();
            Item.useAnimation = Item.useTime;

        }
        public override bool MeleePrefix()
        {
            return true;
        }
        public override void Update(ref float gravity, ref float maxFallSpeed)
		{
            
        }
        public override void HoldItem(Player player)
        {
            player.Dplayer().ShieldEndurance = Endurance;
            if (player.itemAnimation == 0 && player.controlUseTile && player.Dplayer().ShieldDefense == 0 && player.Dplayer().ShieldCD == 0)
            {
                player.immune = false;
                if (Main.myPlayer == player.whoAmI)
                {
                    int proj = NewProjectile(player.GetSource_FromAI(), player.Center, Vector2.Zero, Item.shoot, 0, 0, player.whoAmI, 0, 0);
                    Main.projectile[proj].DProj().Back = 1;
                    Main.projectile[proj].netUpdate = true;
                }
            }
        }
        public override void UpdateInventory(Player player)
		{
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.Dplayer().ShieldDefense == 0)
            {
                int proj2 = NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 0, 0);
                Main.projectile[proj2].DProj().Back = -1;
                if (player.Dplayer().Block > 0)
                {
                    Main.projectile[proj2].DProj().Bool[0] = true;
                    player.Dplayer().Block = 0;
                }
                Main.projectile[proj2].netUpdate = true;
            }
            return false;
		}
        public override bool CanUseItem(Player player)
        {
            return player.Dplayer().ShieldDefense == 0;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            MeleeGlobalItem.R += 1f;
            tooltips.Add(new TooltipLine(Mod, "特殊武器", Language.GetTextValue("Mods.DDmod.Tooltips.Rightdefense"))
            {
                OverrideColor = new Color(255, 255, 255)
            });

            foreach (TooltipLine line in tooltips)
            {
                if (line.Mod == "Terraria")
                {
                    if (line.Name == "Tooltip0")
                    {
                        line.Text = Language.GetTextValue("Mods.DDmod.Tooltips.SwordShield",new object[] { ""+Magnification*Item.defense , ((Endurance)*100).ToString("F0") });
                    }
                }
            }
        }
    }
}