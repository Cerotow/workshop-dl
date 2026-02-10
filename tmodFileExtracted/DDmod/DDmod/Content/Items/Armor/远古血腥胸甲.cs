using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DDmod.Content.Items.Armor
{
	[AutoloadEquip(EquipType.Body)]
	public class 远古血腥胸甲 : ModItem
	{
		public override void SetStaticDefaults()
		{
		}
		public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.defense = 7;
            Item.value = 50000;
            Item.rare = 1;
        }
        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Generic) += 0.03F;
        }
	}
}