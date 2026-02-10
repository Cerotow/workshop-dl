using Terraria.GameContent.UI.BigProgressBar;

namespace DDmod.BossHealthBar
{
    public class DDmodBossBar : ModBossBarStyle
    {
        public override bool PreventDraw => true;
        public static Asset<Texture2D> Damage;
        public static Asset<Texture2D> Defense;
        public override void Load()
        {
            Damage = ModContent.Request<Texture2D>("DDmod/UI/血条UI/Damage");
            Defense = ModContent.Request<Texture2D>("DDmod/UI/血条UI/Defense");
        }
        public override void Draw(SpriteBatch spriteBatch, IBigProgressBar currentBar, BigProgressBarInfo info)
        {
            if (!Main.gameInactive)
            {
                NPCTracker.UpdateNPCTracker();
            }
            NPCTracker.DrawHealthBars(spriteBatch);
        }
    }
}
