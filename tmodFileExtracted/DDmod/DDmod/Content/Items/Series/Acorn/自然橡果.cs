using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria.ID;
using Terraria.ModLoader;

namespace DDmod.Content.Items.Series.Acorn
{
    public class 自然橡果 : ModItem
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 0, 6, 0);
            Item.rare = ItemRarityID.Orange;
            Item.accessory = true;
        }

        public override void PostDrawInWorld(SpriteBatch sb, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D tex = (Texture2D)ModContent.Request<Texture2D>("DDmod/Content/Items/Series/Acorn/自然橡果_Glow");
            float x = (float)(Item.width / 2f - tex.Width / 2f);
            float y = Item.height - tex.Height;
            lightColor = new Color(255, 255, 255);
            sb.Draw(tex, new Vector2(Item.position.X - Main.screenPosition.X + tex.Width / 2 + x, Item.position.Y - Main.screenPosition.Y + tex.Height / 2 + y + 2f), new Rectangle?(new Rectangle(0, 0, tex.Width, tex.Height)), lightColor, rotation, new Vector2(tex.Width / 2, tex.Height / 2), scale, SpriteEffects.None, 0f);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statManaMax2+=40;
            Lighting.AddLight(player.Center,new Color(0,255,0).ToVector3()*0.5F);
        }

    }
}
