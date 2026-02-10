using DDmod.Content.Projectiles.GeneralProj;
using DDmod.Content.Projectiles.Melee;
using Terraria;
using Terraria.ID;
using Terraria.WorldBuilding;

namespace DDmod.Content.Items.Melee.Sword.Make
{
    public class 开膛手 : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 33;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 3;
            Item.value = Item.buyPrice(0, 4, 0, 0);
            Item.rare = 6;
            Item.UseSound = SoundID.Item1;
            Item.scale  = 1.3F;
            Item.shoot = ModContent.ProjectileType<血>();
            Item.shootSpeed = 8;
            Item.autoReuse = true;
            Item.GetGlobalItem<MeleeGlobalItem>().Color = new Color(100, 0, 0, 0);
        }
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        bool R;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            R = false;
            for (int a = 0; a < Main.rand.Next(5)+1; a++)
            {
               int P =  NewProjectile(source, position, velocity.RotatedBy(Main.rand.NextFloat(-0.1F, 0.1F))* Main.rand.NextFloat(0.6f, 1F), type, damage/4, knockback/4);
                Main.projectile[P].DamageType = DamageClass.Melee;
            }     return false;
        }
        public override void ModifyHitNPC(Player player, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (target.HasBuff(30))
            {
                modifiers.SourceDamage += 0.2F;
            }
        }
        public override void OnHitNPC(Player player, NPC target, HitInfo hit, int damageDone)
        {
            if (target.HasBuff(30)&&!R)
            {
                R = true;
                player.Heal((int)(damageDone * 0.1F));
            }
            target.AddBuff(30, 180);
        }
        public override bool MeleePrefix()
        {
            return true;
        }
        public override void AddRecipes()
        {
            //CreateRecipe(1).AddIngredient(724).AddIngredient(676).AddIngredient(520,25).AddIngredient(521,25).AddTile(TileID.MythrilAnvil).Register();
        }
    }
}