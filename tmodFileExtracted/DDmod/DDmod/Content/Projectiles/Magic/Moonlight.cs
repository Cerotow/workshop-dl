using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace DDmod.Content.Projectiles.Magic
{
    public class Moonlight : ModProjectile
    {
        public override string Texture => "DDmod/Image/Nullpng";
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 1000;
            Projectile.extraUpdates = 2;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 40;
        }
        Player player => Main.player[Projectile.owner];
        public override bool PreAI()
        {
            if (Projectile.DProj().vector[1] == Vector2.Zero)
            {
                Projectile.DProj().vector[1] = Projectile.velocity;
            }
            if (Main.rand.NextBool(30))
            {
                Projectile.velocity = Projectile.DProj().vector[1].RotatedBy(Main.rand.NextFloat(-0.2f, 0.2f));
            }
            if (player.Center.Y - Projectile.Center.Y > 1200)
            {
                Projectile.Kill();
            }
            if (player.Center.Y - Projectile.Center.Y > 800)
            {
                if (Projectile.owner == Main.myPlayer)
                    Projectile.DProj().vector[0] = Main.MouseWorld;

                Projectile.netUpdate = true;
                if (!Projectile.DProj().Bool[0])
                {
                    for (int a = 0; a < 5; a++)
                    {
                        float C = Main.rand.NextFloat(500, 0) * player.direction;
                        Vector2 Center = new Vector2(Projectile.DProj().vector[0].X - C, Projectile.Center.Y);
                        Vector2 vector = (Projectile.DProj().vector[0] - Center).PerfectNormalize() * 8;
                        if (Projectile.owner == Main.myPlayer)
                        {
                            NewProjectile(Projectile.GetSource_FromThis(), Center, vector, ProjectileID.LunarFlare, Projectile.damage, Projectile.knockBack, Projectile.owner, 0, Projectile.DProj().vector[0].Y);
                        }
                    }
                    Projectile.DProj().Bool[0] = true;
                }
            }
            /*
            Dust dust = Main.dust[NewDust(Projectile.position, 1, 1, 229)];
            dust.noGravity = true;
            dust.velocity = Vector2.Zero;
            dust.scale = 1.8F;*/
            return false;
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public override void OnKill(int timeLeft)
        {
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = DDTextures.MiniVoidStar.Value;
            Vector2 vector = Projectile.Size / 2;
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                if (Projectile.oldPos[i] != Projectile.position)
                {
                    Vector2 vector2 = Projectile.oldPos[i] + vector - Main.screenPosition;
                    Main.spriteBatch.Draw(texture, vector2, null, new Color(20, 233, 201, 0), Projectile.rotation, texture.Size() / 2, Projectile.scale / 1.5f*((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), 0, 0f);
                    Main.spriteBatch.Draw(texture, vector2, null, new Color(20, 233, 201, 0).Opposite()*0.25F, Projectile.rotation, texture.Size() / 2, Projectile.scale / 3 * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), 0, 0f);
                    //Main.spriteBatch.Draw(texture, vector2, null, new Color(71, 233, 60, 0).Opposite() * 0.5F, Projectile.rotation, texture.Size() / 2, Projectile.scale / 10 + 0.025F + (float)i / 675 * 0.26f, spriteEffects, 0f);
                }
            }
            return base.PreDraw(ref lightColor);
        }
    }
}