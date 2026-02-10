using DDmod.Content.Dusts;
using DDmod.Content.Projectiles.Ranged;

namespace DDmod.Content.Items.Boss.特殊
{
    public class 石魔弓 : ModItem
    {
        public override void SetStaticDefaults()
        {

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            if (Main.netMode != 2)
                DDTextures.Bow[Type] = ModContent.Request<Texture2D>(Texture + "_NoStrings");
        }

        public override void SetDefaults()
        {
            Item.damage = 60;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 6;
            Item.crit = 5;
            Item.value = Item.buyPrice(0, 20, 20, 0);
            Item.rare = ItemRarityID.Orange;
            Item.useAmmo = AmmoID.Arrow;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<石魔箭>();
            ModifyBow.Load(Type, new Modify(), true);
            Item.shootSpeed = 10f;
            Item.GetGlobalItem<RangedGlobalItem>().Bow = true;
            Item.GetGlobalItem<RangedGlobalItem>().SetString(14, 14, 5, 0, 6, true, new Color(255, 255, 56));
            Item.GetGlobalItem<AdventureGearGlobalItem>().稀有度 = 2;
        }
        public override bool CanShoot(Player player)
        {
            return base.CanShoot(player);
        }
        public class Modify : ModifyBow
        {
            int R = 0;
            public override void ChargeUpdate(Item item, Projectile Projectile, RangedGlobalItem rangedItem, RangedProjectile ranged)
            {
                Player player = Projectile.Player();
                ranged.BowTime -= player.GetTotalAttackSpeed(DamageClass.Ranged) * 0.5f;
                if (ranged.BowTime / player.itemAnimationMax < 1.6F)
                {
                    for (int a = 0; a < 2; a++)
                    {
                        Dust dust = Main.dust[NewDust(player.MountedCenter + new Vector2(Main.rand.NextFloat(30, 50)).RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)), 1, 1, ModContent.DustType<光球粒子>(), 0, 0, 0, new Color(255, 255, 56, 0), 1f)];
                        dust.noGravity = true;
                        dust.scale = 0.8F;
                        GlobalDust.DustPlayerOwner[dust.dustIndex] = player.whoAmI;
                        dust.velocity = (player.Center - dust.position) / Main.rand.NextFloat(20, 21);
                        dust.customData = 0.8F;
                        dust.velocity *= (float)dust.customData;

                    }
                }
            }
            public override void Skill(Item item, Projectile Projectile, RangedGlobalItem rangedItem, RangedProjectile ranged)
            {
                Player player = Projectile.Player();
                if (ranged.BowTime > player.itemAnimationMax * 1.6f && ranged.Ammo[0] != ModContent.ProjectileType<石魔箭>())
                {
                    for (int i = 0; i < 50; i++)
                    {
                        Dust dust = Main.dust[NewDust(Projectile.Center, 1, 1, ModContent.DustType<光球粒子>(), newColor: new Color(255, 255, 56, 0))];
                        dust.velocity = Vector2.One.RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)) * Main.rand.NextFloat(0.8F, 2.2F);
                        dust.noGravity = true;
                        dust.alpha = 100;
                        dust.scale = 1.8f;
                        dust.customData = 2;
                        GlobalDust.DustPlayerOwner[dust.dustIndex] = player.whoAmI;
                    }
                    ranged.Ammo[0] = ModContent.ProjectileType<石魔箭>();
                }
            }
            public override void PostUpdate(Item item, Projectile Projectile, RangedGlobalItem rangedItem, RangedProjectile ranged)
            {
                R++;
                bool Bool = Projectile.owner == Main.myPlayer;
                if (ranged.ShootShop || ranged.BowAnimationTime > 0)
                {
                    if (Main.rand.NextBool(20) && Bool && R > 20)
                    {
                        R = 0;
                        NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center - new Vector2(0, 24).RotatedBy(Projectile.rotation), Projectile.velocity * 10, ModContent.ProjectileType<石像弹>(), (int)(Projectile.damage * 1.5f), 0);
                        NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center + new Vector2(0, 24).RotatedBy(Projectile.rotation), Projectile.velocity * 10, ModContent.ProjectileType<石像弹>(), (int)(Projectile.damage * 1.5f), 0);
                    }
                }
            }
        }
    }
}