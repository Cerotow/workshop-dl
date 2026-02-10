using DDmod.Content.Dusts;
using DDmod.Modkey;

namespace DDmod.Content.Projectiles.Talisman
{
    public class HunyuanPearlUmbrella : Talismans
    {
        public static Asset<Texture2D> Umbrella;
        public override void Load()
        {
            Umbrella = ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Talisman/Umbrella");
        }
        public override void SetStaticDefaults()
        {
        }
        public override bool PreEnd()
        {
            Player player = Main.player[Projectile.owner];
            if(Projectile.owner!=Main.myPlayer)
            {
                return false;
            }
            if (player.TPlayer().TalismanTimes <= 0 || Projectile.Player().TPlayer().Shield <= 0)
            {
                player.TPlayer().TalismanTimes = 0;
                Projectile.Player().TPlayer().Shield = 0;
                Projectile.Player().TPlayer().MaxShield = 0;
                Projectile.Center = player.Center;
                Projectile.DProj().Bool[0] = false;
            }
            return false;
        }
        public override bool Use()
        {
            Player player = Main.player[Projectile.owner];
            if (player.TPlayer().TalismanCD >= player.TPlayer().MaxTalismanCD && Projectile.owner == Main.myPlayer && ModkeySetup.TalismanKey.JustPressed  && !player.HasBuff(23))
            {
                bool Crit = Main.rand.Next(100) < Projectile.CritChance;
                Projectile.DProj().Bool[0] = true;
                player.TPlayer().Shield = Crit ? Projectile.damage * 2 : Projectile.damage;
                player.TPlayer().MaxShield = Crit ? Projectile.damage * 2 : Projectile.damage;
                player.TPlayer().TalismanTimes = player.TPlayer().MaxTalismanTimes;
                player.TPlayer().TalismanCD = 0;
                Projectile.velocity = Vector2.Zero;
                Projectile.netUpdate = true;
            }
            return false;
        }
        public override bool MobileAI()
        {
            Player player = Main.player[Projectile.owner];
            if (!Projectile.DProj().Bool[0])
            {
                player.TPlayer().Shield = 0;
                player.TPlayer().MaxShield = 0;
            }//跟随AI
            Projectile.rotation = MathHelper.PiOver4 + MathHelper.PiOver4 / 2 * Projectile.Player().direction;
            return true;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.DProj().Bool[0])
            {
                return false;
            }
            SpriteEffects spriteEffects = 0;
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 vector = Projectile.Size / 2;
            Vector2 Origin = new Vector2(texture.Width / 2, texture.Height / 4);
            Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, (int)(texture.Width), (int)(texture.Height / 2))), Color.White, Projectile.rotation, Origin, 1, spriteEffects, 0f);
            return false;
        }
    }
}