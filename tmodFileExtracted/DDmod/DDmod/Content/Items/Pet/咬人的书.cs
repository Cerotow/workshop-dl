using DDmod.Content.Buffs.PlayerBuffs;
using DDmod.Content.Projectiles.Pet;
using DDmod.Content.Projectiles.Pet.MasterPet;
using DDmod.Content.Projectiles.Pet.MasterPet.MasterPetBuff;
using DDmod.Content.Projectiles.Pet.PetBuff;

namespace DDmod.Content.Items.Pet
{
    public class 咬人的书 : ModItem
    {
        public override void SetStaticDefaults()
        {

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 114514;
        }

        public override void SetDefaults()
        {
            Item.DefaultToVanitypet(ModContent.ProjectileType<薄绿>(), ModContent.BuffType<薄绿Buff>());

            Item.width = 28;
            Item.height = 20;
            Item.rare = ItemRarityID.Master;
            Item.value = Item.sellPrice(0, 0,10);
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
