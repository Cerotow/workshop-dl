using System;
using DDmod.Content.Projectiles.Ranged;
using DDmod.NoContent.Config;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DDmod.Content.Items.Ranged.Make
{
	public class 真永夜弓 : ModItem
	{
		public override void Load()
		{
		}
		public override void SetDefaults()
		{
			Item.damage = 36;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 22;
			Item.height = 38;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 1;
			Item.value = Item.buyPrice(0, 15, 0, 0);
			Item.rare = 6;
			Item.shoot = ProjectileID.IchorArrow;
			Item.shootSpeed = 15f;
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item5;
			Item.useAmmo = 40;
			Item.GetGlobalItem<RangedGlobalItem>().Bow = true;
            ModifyBow.Load(Type, new Modify(), true);
            Item.GetGlobalItem<RangedGlobalItem>().BowstringGlow = true;
			Item.GetGlobalItem<RangedGlobalItem>().StringColor = new Color(10, 186, 255, 0);
			Item.GetGlobalItem<RangedGlobalItem>().StringOffset = 13;
			Item.GetGlobalItem<RangedGlobalItem>().YOffset = 6;
			Item.GetGlobalItem<RangedGlobalItem>().ResidentGlow = true;
			Item.GetGlobalItem<RangedGlobalItem>().StringUP = 12;
			Item.GetGlobalItem<RangedGlobalItem>().StringDown = 12;
			Item.GetGlobalItem<RangedGlobalItem>().ScaleGlow = 4;
            Item.GetGlobalItem<RangedGlobalItem>().Offset = 4;
        }

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("True Nights bow");
		//DisplayName.AddTranslation(7, "真永夜弓");
			if (Main.netMode != 2)
			{
				DDTextures.Bow[Type] = ModContent.Request<Texture2D>(Texture + "_NoStrings");
				DDTextures.BowGlow[Type] = ModContent.Request<Texture2D>(Texture + "_Glow");
            }
            RecipesSystem.Add(ModContent.ItemType<永夜弓>(), 2, new Item(547, 20), new Item(548, 20), new Item(549, 20), new Item(Type, 1));
        }
        public override void Update(ref float gravity, ref float maxFallSpeed)
		{
		}
        public override void UpdateInventory(Player player)
		{
        }
        public override void AddRecipes()
        {
            Condition text = new Condition(Language.GetTextValue("Mods.DDmod.Recipes.强化台合成"), () => !ModContent.GetInstance<DDConfigServer>().HunterSlime);
            CreateRecipe(1).AddIngredient(ModContent.ItemType<永夜弓>()).AddIngredient(547, 20).AddIngredient(548, 20).AddIngredient(549, 20).AddCondition(text).Register();
        }
        public override void HoldItem(Player player)
        {
            if (player.Aplayer().TrueNightEnergy)
            {
                Item.GetGlobalItem<RangedGlobalItem>().PostConvertArrows = ModContent.ProjectileType<真永夜箭>();
                Item.GetGlobalItem<RangedGlobalItem>().BowLight = new Color(87, 208, 146).ToVector3() * 0.75f;

            }
            else
            {
                Item.GetGlobalItem<RangedGlobalItem>().PostConvertArrows = 0;
                Item.GetGlobalItem<RangedGlobalItem>().BowLight = new Vector3(0);

            }
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }
        public class Modify : ModifyBow
        {
            public override void ChargeUpdate(Item item, Projectile Projectile, RangedGlobalItem rangedItem, RangedProjectile ranged)
            {
                Player player = Projectile.Player();
                ranged.BowTime -= player.GetTotalAttackSpeed(DamageClass.Ranged) * 0.25f;
            }
            public override void Shoot(Item item, Projectile Projectile, RangedGlobalItem rangedItem, RangedProjectile ranged)
            {
                Player player = Projectile.Player();
                IEntitySource Source = player.GetSource_ItemUse_WithPotentialAmmo(player.HeldItem, ranged.BowUsedAmmoItemId[0]);
                float la = ranged.BowTime / player.itemAnimationMax * 6;
                float Charge = ranged.BowTime / player.itemAnimationMax;
                if (Charge > 1 && rangedItem.Skill == 0) Charge *= 1.5f;
                float Magnification = Charge;
                if (player.Aplayer().TrueNightEnergy)
                {
                    if (ranged.BowTime / player.itemAnimationMax > 1.8F)
                    {
                        for (int a = 0; a < 6; a++)
                        {
                            int Ty = Main.rand.Next(new int[] { -1, 1 });
                            float R = Main.rand.NextFloat(150, 250);
                            Vector2 Center = Projectile.Center - Projectile.velocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-MathHelper.PiOver2, MathHelper.PiOver2)) * R;
                            Vector2 vector = (player.Dplayer().MouseWorld - Center).PerfectNormalize() * ranged.BowSpeed[0] * Charge;
                            if (Projectile.owner == Main.myPlayer)
                            {
                                int Proj = NewProjectileChange(Source, Center, vector / 2, ranged.Ammo[0], (int)(ranged.BowDamage[0] * Charge) / 5, ranged.BowKnockBack[0] * Charge, Projectile.owner, Ty, 0, Magnification/5);
                                Main.projectile[Proj].DProj().Times[0] = (player.Dplayer().MouseWorld - Center).Length();
                                Main.projectile[Proj].extraUpdates = 3;
                            }
                        }
                        if (Projectile.owner == Main.myPlayer)
                        {
                            //发射
                            int Proj1 = NewProjectileChange(Source, Projectile.Center - Projectile.velocity.PerfectNormalize() * (-10 + la + 8), Projectile.velocity.PerfectNormalize() * ranged.BowSpeed[0] * Charge, ranged.Ammo[0], (int)(ranged.BowDamage[0] * Charge), ranged.BowKnockBack[0] * Charge, Projectile.owner, 0, 0, Magnification);
                            Main.projectile[Proj1].scale = Projectile.scale;
                            Main.projectile[Proj1].Resize((int)(Main.projectile[Proj1].OriginalWidth() * (Main.projectile[Proj1].scale)), (int)(Main.projectile[Proj1].OriginalHeight() * (Main.projectile[Proj1].scale)));
                        }
                    }
                    else if (ranged.BowTime / player.itemAnimationMax >= 1F)
                    {
                        for (int a = 0; a < 4; a++)
                        {
                            int Ty = Main.rand.Next(new int[] { -2, 2 });
                            float R = Main.rand.NextFloat(150, 250);
                            Vector2 Center = Projectile.Center - Projectile.velocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-MathHelper.PiOver2, MathHelper.PiOver2)) * R;
                            Vector2 vector = (player.Dplayer().MouseWorld - Center).PerfectNormalize() * ranged.BowSpeed[0] * Charge;

                            if (Projectile.owner == Main.myPlayer)
                            {
                                int Proj = NewProjectileChange(Source, Center, vector / 2, ranged.Ammo[0], (int)(ranged.BowDamage[0] * Charge) / 4, ranged.BowKnockBack[0] * Charge, Projectile.owner, Ty, 0, Magnification/4);
                                Main.projectile[Proj].DProj().Times[0] = (player.Dplayer().MouseWorld - Center).Length();
                            }
                        }
                        if (Projectile.owner == Main.myPlayer)
                        {
                            //发射
                            int Proj1 = NewProjectileChange(Source, Projectile.Center - Projectile.velocity.PerfectNormalize() * (-10 + la + 8), Projectile.velocity.PerfectNormalize() * ranged.BowSpeed[0] * Charge, ranged.Ammo[0], (int)(ranged.BowDamage[0] * Charge), ranged.BowKnockBack[0] * Charge, Projectile.owner, 0, 0, Magnification);
                            Main.projectile[Proj1].scale = Projectile.scale;
                            Main.projectile[Proj1].Resize((int)(Main.projectile[Proj1].OriginalWidth() * (Main.projectile[Proj1].scale)), (int)(Main.projectile[Proj1].OriginalHeight() * (Main.projectile[Proj1].scale)));
                        }
                    }
                }
                else
                {
                    if (Projectile.owner == Main.myPlayer)
                    {
                        //发射
                        int Proj1 = NewProjectileChange(Source, Projectile.Center - Projectile.velocity.PerfectNormalize() * (-10 + la + 8), Projectile.velocity.PerfectNormalize() * ranged.BowSpeed[0] * Charge, ranged.Ammo[0], (int)(ranged.BowDamage[0] * Charge), ranged.BowKnockBack[0] * Charge, Projectile.owner, 0, 0, Magnification);
                        Main.projectile[Proj1].scale = Projectile.scale;
                        Main.projectile[Proj1].Resize((int)(Main.projectile[Proj1].OriginalWidth() * (Main.projectile[Proj1].scale)), (int)(Main.projectile[Proj1].OriginalHeight() * (Main.projectile[Proj1].scale)));
                    }
                }
            }
        }
    }
}