using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.Projectiles.Melee;
using DDmod.Content.Projectiles.Melee.Spear.Proj;
using Terraria;
using Terraria.ID;

namespace DDmod.Content.Items.Boss.天雷怒云
{
    public class 雷霆剑 : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 48;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 14;
            Item.useAnimation = 14;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 3;
            Item.value = Item.buyPrice(0, 12, 0, 0);
            Item.rare = 5;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.scale += 0.2f;
            Item.alpha = 255;
            Item.GetGlobalItem<MeleeGlobalItem>().Color = new Color(66, 155, 255, 50);
            Item.GetGlobalItem<MeleeGlobalItem>().ColorL = true;
            DDSystem.Instance.DDEquipGlow.TryGetValue("雷霆剑", out int GG);
            Item.glowMask = (short)GG;
        }
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(4, 4, true));
        }
        int a = 0;
        public override void OnHitNPC(Player player, NPC target, HitInfo hit, int damageDone)
        {
            if (a<3)
            {
                target.AddBuff(ModContent.BuffType<引雷>(), 240);
            }
            target.AddBuff(144, 180);
            a++;
        }
        public override bool? UseItem(Player player)
        {
            a = 0;
            if (Main.myPlayer == player.whoAmI)
            {
                for (int A = 0; A < 200; A++)
                {
                    if (Main.npc[A].CanBeChasedBy() && Main.npc[A].HasBuff(ModContent.BuffType<引雷>()))
                    {
                        int C = NewProjectile(player.GetSource_FromAI(), Main.npc[A].Center - new Vector2(Main.rand.Next(-3000, 3000) / 30, 800), new Vector2(0, 10), ModContent.ProjectileType<闪电>(), player.GetWeaponDamage(Item), player.GetWeaponKnockback(Item),-1,0,1,400);
                        Main.projectile[C].DamageType = DamageClass.Melee;
                    }
                }
                int D = NewProjectile(player.GetSource_FromAI(), new Vector2(Main.MouseWorld.X, player.Center.Y) - new Vector2(Main.rand.Next(-3000, 3000) / 30, 800), new Vector2(0, 10), ModContent.ProjectileType<闪电>(), player.GetWeaponDamage(Item), player.GetWeaponKnockback(Item), -1, 0, 1, 400);
                Main.projectile[D].DamageType = DamageClass.Melee;
            }

            return true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }
        public override Color? GetAlpha(Color color)
        {
            return color;
        }
        public override void ModifyHitNPC(Player player, NPC target, ref NPC.HitModifiers modifiers)
        {
        }
        public override bool MeleePrefix()
        {
            return true;
        }
    }
}