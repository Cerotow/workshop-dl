using DDmod.Content.Dusts;
using DDmod.Content.NPCs.IittleMonster.绿岩;
using DDmod.Content.Particles;
using DDmod.Content.Tiles;
using DDmod.Content.Tiles.绿岩;
using DDmod.Worlds;
using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.ObjectData;
using Terraria.UI;

namespace DDmod.Content.Projectiles.Hostile
{
    public class 绿岩炮台 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Orange Laser");
            //DisplayName.AddTranslation(7, "橙激光");
        }
        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.alpha = 0;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
        }
        int A;
        public override void AI()
        {
            if (Projectile.DProj().vector[0] == Vector2.Zero)
            {
                Projectile.DProj().vector[0] = Projectile.position / 16;
                Main.tileFrame[ModContent.TileType<绿岩炮台Tile>()] = (int)Projectile.ai[2];
            }
            int i = (int)(Projectile.DProj().vector[0].X);
            int j = (int)(Projectile.DProj().vector[0].Y);
            Player player = Main.player[Player.FindClosest(Projectile.Center, 1, 1)];
            if (player.Center.Length() - Projectile.Center.Length() > 1500 || !Main.tile[i, j].HasTile || Main.tile[i, j].TileType != ModContent.TileType<绿岩炮台Tile>())
            {
                Projectile.Kill();
                return;
            }
            Projectile.timeLeft = 5;
            Projectile.ai[0] = Main.tile[i, j].TileFrameX;
            if (Projectile.ai[0] % 75 == 0)
            {
                NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, new Vector2(Projectile.ai[1] * 2, 0), ModContent.ProjectileType<绿岩激光>(), 20, 0);
                Projectile.netUpdate = true;
            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
        }
        public override void OnKill(int timeLeft)
        {
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 vector = new Vector2(Projectile.width, Projectile.height / 2);
            if (Projectile.ai[1] == -1)
            {
                Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, new Color(100, 255, 100, 0) * Projectile.ai[1], Projectile.rotation, new Vector2(texture.Width, texture.Height / 2), Projectile.scale, SpriteEffects.FlipHorizontally, 0f);

            }
            else
            {
                Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, new Color(100, 255, 100, 0) * Projectile.ai[1], Projectile.rotation, new Vector2(texture.Width, texture.Height / 2), Projectile.scale, 0, 0f);

            }
            return false;
        }
    }
}