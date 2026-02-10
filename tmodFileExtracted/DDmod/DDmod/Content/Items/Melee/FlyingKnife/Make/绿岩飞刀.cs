using DDmod.Content.Projectiles.Melee.FlyingKnife;
using Terraria.ID;

namespace DDmod.Content.Items.Melee.FlyingKnife.Make
{
    public class 绿岩飞刀 : 飞刀
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void Defaults()
        {
            属性(14, 10, 18, 15, 1, 3, 12, ModContent.ProjectileType<绿岩飞刀Proj>());
            Value(0, 1, 60, 0);
            DDSystem.Instance.DDEquipGlow.TryGetValue("绿岩飞刀", out int GG);
            Item.glowMask = (short)GG;
            Item.alpha = 255;
        }
        public override Color? GetAlpha(Color color)
        {
            return color;
        }
        public override void PostDrawInWorld(SpriteBatch sb, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            /*
            Texture2D tex = (Texture2D)ModContent.Request<Texture2D>(Texture+"_Glow");
            float x = (float)(Item.width / 2f - tex.Width / 2f);
            float y = Item.height - tex.Height;
            sb.Draw(tex, new Vector2(Item.position.X - Main.screenPosition.X + tex.Width / 2 + x, Item.position.Y - Main.screenPosition.Y + tex.Height / 2 + y + 2f), null, new Color(255, 255, 255), rotation, new Vector2(tex.Width / 2, tex.Height / 2), scale, SpriteEffects.None, 0f);
        */
        }
        public override void AddRecipes()
        {}
    }
}