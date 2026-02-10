using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.Projectiles.RemoteControl;
using Terraria;
using static AssGen.Assets;

namespace DDmod.Content.Items.Boss.MeteorDiggerItems
{
    public class MeteorExcavatorRemoteControl : ModItem
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.pick = 80;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = -1;
            Item.useAnimation = -1;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 6;
            Item.crit = 10;
            Item.value = 500000;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<MeteorExcavator>();
            Item.shootSpeed = 7;
            Item.noMelee = false;
            Item.channel = true;
        }
        public override bool CanPickup(Player player)
        {
            return base.CanPickup(player);
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (Main.LocalPlayer.Aplayer().破坏者核心装置)
            {
                foreach (TooltipLine line in tooltips)
                {
                    if (line.Mod == "Terraria")
                    {
                        if (line.Name == "PickPower")
                        {
                            line.Text = 205 + Language.GetTextValue("LegacyTooltip.26");
                        }
                    }
                }
            }
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
           
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<LargeMechanicalScrap>(), 15).AddIngredient(ItemID.MeteoriteBar, 30).AddTile(TileID.Anvils).AddCondition(new Condition(Language.GetTextValue("Mods.DDmod.Recipes.流星合成"), () => Main.LocalPlayer.Dplayer().MeteorRecipe)).Register();
        }
    }
}