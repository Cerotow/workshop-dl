using DDmod.Content.Projectiles.Pet.MasterPet;
using DDmod.Content.Projectiles.Pet.MasterPet.MasterPetBuff;

namespace DDmod.Content.Items.Pet
{
    public class LavaCrown : ModItem
    {
        public override void SetStaticDefaults()
        {

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 114514;
        }

        public override void SetDefaults()
        {
            Item.DefaultToVanitypet(ModContent.ProjectileType<Devil_Slime>(), ModContent.BuffType<Devil_SlimeBuff>());

            Item.width = 28;
            Item.height = 20;
            Item.rare = ItemRarityID.Master;
            Item.value = Item.sellPrice(0, 5);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.AddBuff(Item.buffType, 2);
            return false;
        }
        public override void UpdateInventory(Player player)
        {
        }
    }
}
