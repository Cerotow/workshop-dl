using DDmod.Content.Dusts;
using DDmod.Content.Projectiles.Ranged;
using DDmod.Content.Projectiles.Ranged.Gun;

namespace DDmod.Content.Items.Ranged.NPCLoot
{
    public class 手持大炮 : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 10;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 65;
            Item.useAnimation = 65;
            Item.useStyle = 5;
            Item.noMelee = true;
            Item.knockBack = 5;
            Item.value = Item.buyPrice(0, 30, 0, 0);
            Item.rare = 6;
            SoundStyle sound = SoundID.Item14;
            sound.Pitch = -0.8F;
            Item.UseSound = sound;
            Item.autoReuse = true;
            Item.shoot = 10;
            Item.shootSpeed = 20f;
            Item.useAmmo = 929;
        }
        public override void SetStaticDefaults()
        {

        }
        public bool A;
        public int useTime;
        public int useAnimation;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
               velocity = velocity.RotatedBy(Item.GetGlobalItem<RangedGlobalItem>().Rota * (-player.direction));
            position -=  new Vector2(0, 10).RotatedBy(player.itemRotation);
            NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            player.velocity += -velocity.RotatedBy(Item.GetGlobalItem<RangedGlobalItem>().Rota * -player.direction) / 10;
            if (Item.GetGlobalItem<RangedGlobalItem>().Rota < 1.6F)
            {
                Item.GetGlobalItem<RangedGlobalItem>().Rota += 0.8F;
            }
            return false;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-0, -0);
        }
    }
}