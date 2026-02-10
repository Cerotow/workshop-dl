using System;
using DDmod.Content.Dusts;
using DDmod.Content.Items.Series.Acorn;
using DDmod.Content.Projectiles.Ranged;
using DDmod.NoContent.Config;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static AssGen.Assets;

namespace DDmod.Content.Items.Ranged.Make
{
    public class 泰拉之锐 : ModItem
    {
        public override void Load()
        {
        }
        public override void SetDefaults()
        {
            Item.damage = 37;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 22;
            Item.height = 38;
            Item.useTime = 21;
            Item.useAnimation = 21;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 1;
            Item.value = Item.buyPrice(0, 30, 0, 0);
            Item.rare = 8;
            Item.shoot = ProjectileID.IchorArrow;
            Item.shootSpeed = 9f;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item5;
            Item.useAmmo = 40;
            Item.GetGlobalItem<RangedGlobalItem>().Bow = true;
            Item.GetGlobalItem<AdventureGearGlobalItem>().稀有度 = AdventureGearGlobalItem.泰拉级;
            ModifyBow.Load(Type, new Modify(), true);
            Item.GetGlobalItem<RangedGlobalItem>().SetBow(0, true, 4);
            Item.GetGlobalItem<RangedGlobalItem>().SetString(33, 30, 29, 6, 0, true, new Color(178, 226, 91));
        }

        public override void SetStaticDefaults()
        {
            if (Main.netMode != 2)
            {
                DDTextures.Bow[Type] = ModContent.Request<Texture2D>(Texture + "_NoStrings");
                DDTextures.BowGlow[Type] = ModContent.Request<Texture2D>(Texture + "_Glow");
            }
            RecipesSystem.Add(ModContent.ItemType<英雄断弓>(), 4, new Item(ModContent.ItemType<真圣弓>(), 1), new Item(ModContent.ItemType<真永夜弓>(), 1), new Item(ModContent.ItemType<SoulOfNature>(), 50), new Item(Type, 1));
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
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            spriteBatch.Draw(TextureAssets.Item[Item.type].Value, position, frame, drawColor, 0, origin, scale * 1.3F, 0, 0);
            return false;
        }
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Texture2D texture = TextureAssets.Item[Item.type].Value;
            spriteBatch.Draw(texture, Item.position + new Vector2(Item.width / 2, Item.height) - Main.screenPosition, null, alphaColor, rotation, new Vector2(texture.Width / 2, texture.Height - 10), scale, 0, 0);

            return false;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }
        public override void AddRecipes()
        {
            Condition text = new Condition(Language.GetTextValue("Mods.DDmod.Recipes.强化台合成"), () => !ModContent.GetInstance<DDConfigServer>().HunterSlime);
            CreateRecipe(1).AddIngredient(ModContent.ItemType<英雄断弓>()).AddIngredient(ModContent.ItemType<真圣弓>()).AddIngredient(ModContent.ItemType<真永夜弓>()).AddIngredient(ModContent.ItemType<SoulOfNature>(), 50).AddCondition(text).Register();
        }
        public class Modify : ModifyBow
        {
            float SP = 0;
            public override void PreModifyArrow(Item item, Projectile Projectile, RangedGlobalItem rangedItem, RangedProjectile ranged, ref float speed, ref int damage, ref float knockBack, ref int usedAmmoItemId, ref int projToShoot)
            {
                if (projToShoot == 1)
                {
                    projToShoot = ModContent.ProjectileType<泰拉箭幻影>();
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
                    {
                        Dust dust = Main.dust[NewDust(player.MountedCenter + new Vector2(Main.rand.NextFloat(30, 50)).RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)), 1, 1, ModContent.DustType<光球粒子>(), 0, 0, 0, newColor: new Color(71, 233, 60, 0), 1f)];
                        dust.noGravity = true;
                        dust.scale = 0.6F;
                        GlobalDust.DustPlayerOwner[dust.dustIndex] = player.whoAmI;
                        dust.velocity = (player.Center - dust.position) / Main.rand.NextFloat(20, 21);
                        dust.customData = -0.8F;
                        dust.velocity *= -(float)dust.customData;
                    }
                    {
                        Dust dust = Main.dust[NewDust(player.MountedCenter + new Vector2(Main.rand.NextFloat(30, 50)).RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)), 1, 1, ModContent.DustType<星光粒子>(), 0, 0, 0, newColor: new Color(141, 233, 130, 0), 1f)];
                        dust.noGravity = true;
                        dust.scale = 1.2F;
                        GlobalDust.DustPlayerOwner[dust.dustIndex] = player.whoAmI;
                        dust.velocity = (player.Center - dust.position) / Main.rand.NextFloat(10, 11);
                    }
                }
            }
            public override void NoChargeUpdate(Item item, Projectile Projectile, RangedGlobalItem rangedItem, RangedProjectile ranged)
            {
                Player player = Projectile.Player();
                if (SP < 3)
                {
                    SP += 0.05F;
                }
                ranged.BowTime += player.GetTotalAttackSpeed(DamageClass.Ranged) * SP / 2;
            }
            public override void Skill(Item item, Projectile Projectile, RangedGlobalItem rangedItem, RangedProjectile ranged)
            {
                Player player = Projectile.Player();
                if (ranged.BowTime > player.itemAnimationMax * 1.6f && ranged.Ammo[0] != ModContent.ProjectileType<泰拉箭>())
                {
                    for (int i = 0; i < 20; i++)
                    {
                        Dust dust = Main.dust[NewDust(Projectile.Center, 1, 1, ModContent.DustType<光球粒子>(), newColor: new Color(71, 233, 60, 0))];
                        dust.velocity = Vector2.One.RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)) * Main.rand.NextFloat(0.8F, 2.2F);
                        dust.noGravity = true;
                        dust.alpha = 100;
                        dust.scale = 1.8f;
                        dust.customData = -2;
                        GlobalDust.DustPlayerOwner[dust.dustIndex] = player.whoAmI;
                    }
                    for (int i = 0; i < 20; i++)
                    {
                        Dust dust = Main.dust[NewDust(Projectile.Center, 1, 1, ModContent.DustType<星光粒子>(), newColor: new Color(141, 233, 130, 0))];
                        dust.velocity = Vector2.One.RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)) * Main.rand.NextFloat(1.8F, 3.2F);
                        dust.noGravity = true;
                        dust.scale = 1.3f;
                        GlobalDust.DustPlayerOwner[dust.dustIndex] = player.whoAmI;
                    }
                    ranged.Ammo[0] = ModContent.ProjectileType<泰拉箭>();
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

                //  Vector2 Center = Projectile.Center - Projectile.velocity.PerfectNormalize() * (-10 + la + 8);
                if (Projectile.owner == Main.myPlayer)
                {
                    if (!ranged.Charge)
                    {
                        for (int a = 0; a < SP - 1; a++)
                        {
                            float R = Main.rand.NextFloat(150, 250);
                            Vector2 Center = Projectile.Center - Projectile.velocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-MathHelper.PiOver2, MathHelper.PiOver2)) * R;
                            Vector2 vector = (player.Dplayer().MouseWorld - Center).PerfectNormalize() * ranged.BowSpeed[0] * Charge;
                            int Proj2 = NewProjectileChange(Source, player.Center, -vector / 2, ModContent.ProjectileType<泰拉箭幻影>(), (int)(ranged.BowDamage[0] * Charge) / 2, ranged.BowKnockBack[0] * Charge, Projectile.owner, 2, 0, Magnification);
                        }
                    }
                    //发射
                    int Proj = NewProjectileChange(Source, Projectile.Center - Projectile.velocity.PerfectNormalize() * (-10 + la + 8), Projectile.velocity.PerfectNormalize() * ranged.BowSpeed[0] * Charge, ranged.Ammo[0], (int)(ranged.BowDamage[0] * Charge), ranged.BowKnockBack[0] * Charge, Projectile.owner, 0, 0, Magnification);
                    Main.projectile[Proj].scale = Projectile.scale;
                    Main.projectile[Proj].Resize((int)(Main.projectile[Proj].OriginalWidth() * (Main.projectile[Proj].scale)), (int)(Main.projectile[Proj].OriginalHeight() * (Main.projectile[Proj].scale)));
                }
            }
            /// <summary>
            /// 在最后更新
            /// </summary>  
            public override void PostUpdate(Item item, Projectile Projectile, RangedGlobalItem rangedItem, RangedProjectile ranged)
            {
                if (!Projectile.Player().controlUseItem)
                {
                    SP = 0;
                }
            }
        }
    }
}