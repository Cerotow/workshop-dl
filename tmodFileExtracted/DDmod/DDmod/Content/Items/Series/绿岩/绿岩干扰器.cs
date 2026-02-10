using DDmod.Content.Dusts;
using DDmod.Content.Items.Boss.绿岩之视;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.Items.Tiles.绿岩;
using DDmod.Content.Projectiles.GeneralProj;
using DDmod.Content.Projectiles.Magic;
using DDmod.Content.Projectiles.Ranged;
using DDmod.Content.Projectiles.Ranged.Gun;
using DDmod.Worlds;
using Terraria;
using Terraria.Localization;
using static AssGen.Assets;

namespace DDmod.Content.Items.Series.绿岩
{
    public class 绿岩干扰器 : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = 5;
            Item.noMelee = true;
            Item.knockBack = 2;
            Item.value = Item.buyPrice(0, 0, 10, 0);
            Item.rare = 2;
            SoundStyle sound = SoundID.Item157;
            sound.Pitch = -0.5F;
            Item.UseSound = sound;
            Item.DItem().DrawRanged = true;
            Item.DItem().DrawDistance -= 20;
        }
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 8));
        }
        public override bool? UseItem(Player player)
        {
            NPCDowned.绿岩刷怪 = !NPCDowned.绿岩刷怪;
            if (!NPCDowned.绿岩刷怪)
            {
                Main.NewText(Language.GetTextValue("Mods.DDmod.ItemTips.绿岩生成"), 100, 255, 100);
            }
            else
                Main.NewText(Language.GetTextValue("Mods.DDmod.ItemTips.绿岩生成2"), 100, 255, 100);
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.WorldData, -1, -1, null, 0, 0f, 0f, 0f, 0, 0, 0);
            }
            return true;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {
                if (line.Mod == "Terraria")
                {
                    if (line.Name == "Tooltip0")
                    {
                        if (!NPCDowned.绿岩刷怪)
                        {
                            Color color = new Color(255, 100, 100);
                            string t = string.Format("[c/{0:X2}{1:X2}{2:X2}:{3}]", new object[]
                {
                color.R,
                color.G,
                color.B,
                Language.GetText("Mods.DDmod.Items.绿岩干扰器.Tooltip2"),
                });
                            line.Text += "\n"+ t;
                        }
                        else
                        {
                            Color color = new Color(100, 255, 100);
                            string t = string.Format("[c/{0:X2}{1:X2}{2:X2}:{3}]", new object[]
                {
                color.R,
                color.G,
                color.B,
                Language.GetText("Mods.DDmod.Items.绿岩干扰器.Tooltip3"),
                });
                            line.Text += "\n" + t;
                        }
                    }
                }
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<绿岩砖>(), 40).AddIngredient(ModContent.ItemType<绿岩锭>(), 6).AddIngredient(ModContent.ItemType<绿岩电池>(), 1).AddTile(16).AddCondition(new Condition(Language.GetTextValue("Mods.DDmod.Recipes.绿岩合成"), () => Main.LocalPlayer.Dplayer().GreenstoneRecipe)).Register();
        }
    }
}