using DDmod.Content.Items.Boss.MeteorAnnihilatorItems;
using DDmod.Content.Items.Boss.StarGuardItems;
using DDmod.Content.NPCs.Boss.MeteorAnnihilator;
using DDmod.Content.NPCs.Boss.StarGuardBulan;
using DDmod.Content.NPCs.EliteMonster;
using DDmod.Content.Projectiles.Boss;
using DDmod.Worlds;

namespace DDmod.ModLinkage.BossChecklist
{
	public static class ChecklistHelper
    {
        public static BProj[] Proj = new BProj[1000];
        public static BNPC[] NPC = new BNPC[200];
        public static BGore[] Gore = new BGore[1000];
        public static BDust[] Dust = new BDust[3000];

        public const int 矿洞幽魂 = 1;
        public const int 流星歼灭者 = 2;
        public const int 鬼牙 = 3;
        public const int 恐惧缝合体 = 4;
        public const int 绿岩之视 = 5;
        public const int 夜光蘑菇王 = 6;
        public const int 炼狱头颅 = 7;
        public const int 流星破坏者 = 8;
        public const int 天雷怒云 = 9;
        public const int 克苏鲁心脏 = 10;
        public static void BossChecklistDraw(this SpriteBatch sb,Asset<Texture2D> Book, Rectangle rect,Color color, Action<SpriteBatch, Rectangle, Color> customDrawing)
        {
            Texture2D texture = DDTextures.Nav_Prev.Value;
            Vector2 centered = new Vector2(rect.X, rect.Y);
            sb.Draw(Book.Value, centered - new Vector2(20, 12), null, Color.White, 0, Vector2.Zero, 1, 0, 0f);
            rect.X -= 6;
            rect.Y -= 2;
            rect.Height -= 32;
            rect.Width += 8;
            sb.DrawrectBegin(rect, BlendState.AlphaBlend, Main.UIScaleMatrix, out SamplerState anisotropicClamp, out RasterizerState rasterizerState, out Rectangle scissorRectangle);

            customDrawing(sb, rect, color);


            sb.DrawrectEnd(BlendState.AlphaBlend, Main.UIScaleMatrix, anisotropicClamp, rasterizerState, scissorRectangle);
            rect.X += 6;
            rect.Y += 2;
            rect.Height += 32;
            rect.Width -= 8;
            float R = 0.5f;
            Rectangle rectangle = new Rectangle(rect.X + 8, rect.Y + 414, 18, 20);
            if (rectangle.Intersects(new Rectangle(Main.mouseX, Main.mouseY, 1, 1)))
            {
                R = 1;
            }
            sb.Draw(texture, centered + new Vector2(8, 414), null, color * R, 0, Vector2.Zero, 1f, 0, 0f);
        }
        public static void BossChecklistDraw2(this SpriteBatch sb,Asset<Texture2D> Book,Asset<Texture2D> Book2, Rectangle rect,Color color, Action<SpriteBatch, Rectangle, Color> customDrawing)
        {
            Texture2D texture = DDTextures.Nav_Prev.Value;
            Vector2 centered = new Vector2(rect.X, rect.Y);
            sb.Draw(Book.Value, centered - new Vector2(20, 12), null, Color.White, 0, Vector2.Zero, 1, 0, 0f);
            rect.X -= 6;
            rect.Y -= 2;
            rect.Height -= 32;
            rect.Width += 8;
            sb.DrawrectBegin(rect, BlendState.AlphaBlend, Main.UIScaleMatrix, out SamplerState anisotropicClamp, out RasterizerState rasterizerState, out Rectangle scissorRectangle);

            customDrawing(sb, rect, color);


            sb.DrawrectEnd(BlendState.AlphaBlend, Main.UIScaleMatrix, anisotropicClamp, rasterizerState, scissorRectangle);
            sb.Draw(Book2.Value, centered - new Vector2(20, 12), null, Color.White*0.9f, 0, Vector2.Zero, 1, 0, 0f);
            rect.X += 6;
            rect.Y += 2;
            rect.Height += 32;
            rect.Width -= 8;
            float R = 0.5f;
            Rectangle rectangle = new Rectangle(rect.X + 8, rect.Y + 414, 18, 20);
            if (rectangle.Intersects(new Rectangle(Main.mouseX, Main.mouseY, 1, 1)))
            {
                R = 1;
            }
            sb.Draw(texture, centered + new Vector2(8, 414), null, color * R, 0, Vector2.Zero, 1f, 0, 0f);
        }
	}
}