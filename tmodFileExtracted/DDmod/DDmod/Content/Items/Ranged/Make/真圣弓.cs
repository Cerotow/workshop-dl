using System;
using DDmod.Content.Dusts;
using DDmod.Content.Items.Series.杂物;
using DDmod.Content.Projectiles.Ranged;
using DDmod.NoContent.Config;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DDmod.Content.Items.Ranged.Make
{
    public class 真圣弓 : ModItem
    {
        public override void Load()
        {
        }
        public override void SetDefaults()
        {
            Item.damage = 38;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 22;
            Item.height = 38;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 1;
            Item.value = Item.buyPrice(0, 8, 0, 0);
            Item.rare = 6;
            Item.shoot = ProjectileID.IchorArrow;
            Item.shootSpeed = 9f;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item5;
            Item.useAmmo = 40;
            Item.GetGlobalItem<RangedGlobalItem>().Bow = true;
            ModifyBow.Load(Type, new Modify(), true);
            Item.GetGlobalItem<RangedGlobalItem>().BowstringGlow = true;
            Item.GetGlobalItem<RangedGlobalItem>().StringColor = new Color(199, 181, 127);
            Item.GetGlobalItem<RangedGlobalItem>().StringOffset = 3;
            Item.GetGlobalItem<RangedGlobalItem>().StringUP = 2;
            Item.GetGlobalItem<RangedGlobalItem>().StringDown = 2;
            Item.GetGlobalItem<RangedGlobalItem>().ResidentGlow = true;
            Item.GetGlobalItem<RangedGlobalItem>().Offset = 8;
            Item.GetGlobalItem<RangedGlobalItem>().ScaleGlow = 4;
        }

        public override void SetStaticDefaults()
        {
           //DisplayName.SetDefault("True Holy Bow");
           //DisplayName.AddTranslation(7, "真圣弓");
            if (Main.netMode != 2)
            {
                DDTextures.Bow[Type] = ModContent.Request<Texture2D>(Texture + "_NoStrings");
                DDTextures.BowGlow[Type] = ModContent.Request<Texture2D>(Texture + "_Glow");
            }
            RecipesSystem.Add(ModContent.ItemType<圣弓>(),3, new Item(520,20), new Item(521, 20), new Item(ModContent.ItemType<叶绿龟壳>()), new Item(Type));
        }
        public override void UpdateInventory(Player player)
        {
        }
        public override void HoldItem(Player player)
        {
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }
        public override void AddRecipes()
        {
            Condition text = new Condition(Language.GetTextValue("Mods.DDmod.Recipes.强化台合成"), () => !ModContent.GetInstance<DDConfigServer>().HunterSlime);
            CreateRecipe(1).AddIngredient(ModContent.ItemType<圣弓>()).AddIngredient(520, 20).AddIngredient(521, 20).AddIngredient(ModContent.ItemType<叶绿龟壳>()).AddCondition(text).Register();
        }
        public class Modify : ModifyBow
        {
            int R = 0;
            public override void PreModifyArrow(Item item, Projectile Projectile, RangedGlobalItem rangedItem, RangedProjectile ranged, ref float speed, ref int damage, ref float knockBack, ref int usedAmmoItemId, ref int projToShoot)
            {
                if (projToShoot == 1 && Projectile.Player().Aplayer().TrueHolyEnergy)
                {
                    R++;
                    int[] S = new int[] { ModContent.ProjectileType<神箭2>(), ModContent.ProjectileType<真神箭>(), ModContent.ProjectileType<真神箭2>() };
                    projToShoot = S[R % 3];
                }
            }
            public override void PostModifyArrow(Item item, Projectile Projectile, RangedGlobalItem rangedItem, RangedProjectile ranged, ref float speed, ref int damage, ref float knockBack, ref int usedAmmoItemId, ref int projToShoot, ref float KnockBack)
            {

            }
            public override void ChargeUpdate(Item item, Projectile Projectile, RangedGlobalItem rangedItem, RangedProjectile ranged)
            {
                Player player = Projectile.Player();
                ranged.BowTime -= player.GetTotalAttackSpeed(DamageClass.Ranged) * 0.5f;
                if (ranged.BowTime / player.itemAnimationMax < 1.6F)
                {
                    for (int a = 0; a < 2; a++)
                    {
                        Dust dust = Main.dust[NewDust(player.MountedCenter + new Vector2(Main.rand.NextFloat(30, 50)).RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)), 1, 1, ModContent.DustType<光球粒子>(), 0, 0, 0, Main.rand.Next(new Color[] { new Color(250, 221, 72,0), new Color(36, 127, 255,0), new Color(230, 77, 255,0) }), 1f)];
                        dust.noGravity = true;
                        dust.scale = 0.8F;
                        GlobalDust.DustPlayerOwner[dust.dustIndex] = player.whoAmI;
                        dust.velocity = (player.Center - dust.position) / Main.rand.NextFloat(20, 21);
                        dust.customData = 0.8F;
                        dust.velocity *= (float)dust.customData;

                    }
                }
            }
            public override void NoChargeUpdate(Item item, Projectile Projectile, RangedGlobalItem rangedItem, RangedProjectile ranged)
            {
                Player player = Projectile.Player();

            }
            public override void Skill(Item item, Projectile Projectile, RangedGlobalItem rangedItem, RangedProjectile ranged)
            {
                Player player = Projectile.Player();
                if (ranged.BowTime > player.itemAnimationMax * 1.6f && ranged.Ammo[0] != ModContent.ProjectileType<真神箭3>())
                {
                    for (int i = 0; i < 50; i++)
                    {
                        Dust dust = Main.dust[NewDust(Projectile.Center, 1, 1, ModContent.DustType<光球粒子>(), newColor: Main.rand.Next(new Color[] { new Color(250, 221, 72, 0), new Color(36, 127, 255, 0), new Color(230, 77, 255, 0) }))];
                        dust.velocity = Vector2.One.RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)) * Main.rand.NextFloat(0.8F, 2.2F);
                        dust.noGravity = true;
                        dust.alpha = 100;
                        dust.scale = 1.8f;
                        dust.customData = 2;
                        GlobalDust.DustPlayerOwner[dust.dustIndex] = player.whoAmI;
                    }
                    ranged.Ammo[0] = ModContent.ProjectileType<真神箭3>();
                }
            }
            public override void Shoot(Item item, Projectile Projectile, RangedGlobalItem rangedItem, RangedProjectile ranged)
            {
                Player player = Projectile.Player();
                IEntitySource Source = player.GetSource_ItemUse_WithPotentialAmmo(player.HeldItem, ranged.BowUsedAmmoItemId[0]);
                float la = ranged.BowTime / player.itemAnimationMax * 6;
                float Charge = ranged.BowTime / player.itemAnimationMax;
                if (Charge > 1 && rangedItem.Skill == 0) Charge = 1.5f;
                float Magnification = Charge;

                Vector2 Center = Projectile.Center - Projectile.velocity.PerfectNormalize() * (-10 + la + 8);
                if (Projectile.owner == Main.myPlayer)
                {
                    //发射
                    int Proj = NewProjectileChange(Source, Projectile.Center - Projectile.velocity.PerfectNormalize() * (-10 + la + 8), Projectile.velocity.PerfectNormalize() * ranged.BowSpeed[0] * Charge, ranged.Ammo[0], (int)(ranged.BowDamage[0] * Charge), ranged.BowKnockBack[0] * Charge, Projectile.owner, 0, 0, Magnification);
                    Main.projectile[Proj].scale = Projectile.scale;
                    Main.projectile[Proj].Resize((int)(Main.projectile[Proj].OriginalWidth() * (Main.projectile[Proj].scale)), (int)(Main.projectile[Proj].OriginalHeight() * (Main.projectile[Proj].scale)));
                    if (ranged.Ammo[0] == ModContent.ProjectileType<真神箭3>())
                    {
                        Main.projectile[Proj].ai[0] = 1;
                    }
                }
            }
            /// <summary>
            /// 在最后更新
            /// </summary>  
            public override void PostUpdate(Item item, Projectile Projectile, RangedGlobalItem rangedItem, RangedProjectile ranged)
            {
            }
        }
    }
}