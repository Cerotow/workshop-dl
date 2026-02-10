
using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Projectiles.Melee;
using DDmod.Content.Projectiles.Ranged.Ammo;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Ranged
{
    public class HellBats : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.aiStyle = -1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 1800;
            Projectile.tileCollide = true;
            Projectile.penetrate = -1;
            Projectile.ArmorPenetration = 15;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;

        }

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 5;
        }
        public override void AI()
        {
            Player player = Projectile.Player();
            if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = 5;
                Dust dust = Main.dust[NewDust(Projectile.position, Projectile.width, Projectile.height, 6)];
                dust.noGravity = true;
                dust.alpha = 100;
                dust.scale = 1.3f;
            }
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (Projectile.ai[0] == 0)
            {
                Projectile.rotation = (Projectile.velocity + player.velocity).ToRotation();
                Vector2 vector = player.Center - Projectile.Center;
                if (vector.Length() > 80)
                {
                    Projectile.velocity = (Projectile.velocity * 20 + vector.PerfectNormalize() * 5) / (21);
                }
                Projectile.position += player.velocity;
                if (Projectile.DProj().Bool[0])
                {
                    if (Projectile.scale < 1.1F)
                    {
                        Projectile.scale += 0.02F;
                    }
                    if (vector.Length() > 80)
                    {
                        Projectile.DProj().Bool[0] = false;
                    }
                }
                else
                {
                    if (Projectile.scale > 0.9F)
                    {
                        Projectile.scale -= 0.02F;
                    }
                    if (vector.Length() > 80)
                    {
                        Projectile.DProj().Bool[0] = true;
                    }
                }
            }
            else
            {
                if (!Projectile.DProj().Bool[1])
                {
                    Projectile.velocity = (player.Dplayer().MouseWorld-player.Center).PerfectNormalize()*8;
                    Projectile.localNPCHitCooldown = -1;
                    Projectile.DProj().Bool[1] = true;
                }
            }
            Projectile.ProjScaleChange();
            Projectile.netUpdate = true;
            Projectile.frameCounter++;
            if (Projectile.frameCounter%4==0)
            {
                Projectile.frame++;
                Projectile.frame%=5;
            }
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 30+30 * Projectile.DProj().Times[0]; i++)
            {
                Vector2 projDirection = Utils.RotatedBy(Vector2.One, Main.rand.NextFloat(0, MathHelper.TwoPi), default) * Main.rand.NextFloat(0, 4.2F);
                Dust dust = Main.dust[NewDust(Projectile.position, 1, 1, 6)];
                dust.velocity = projDirection;
                dust.noGravity = true;
                dust.alpha = 100;
                dust.scale = 1.8f;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.velocity.Y = -oldVelocity.Y;
            }
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.velocity.X = -oldVelocity.X;
            }
            return Projectile.ai[0]!=0;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            lightColor = Color.White;
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Vector2 vector = new Vector2(Projectile.width, Projectile.height) / 2;
            Color color = Color.White;
            color.A = 255;

            if (Projectile.ai[0] == 0)
            {
                if (Projectile.velocity.X + Projectile.Player().velocity.X > 0)
                {
                    Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / Main.projFrames[Projectile.type] * Projectile.frame, texture.Width, texture.Height / Main.projFrames[Projectile.type])), color, Projectile.rotation, new Vector2(texture.Width, texture.Height / Main.projFrames[Projectile.type]) / 2, Projectile.scale, 0, 0f);
                }
                else
                {
                    Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / Main.projFrames[Projectile.type] * Projectile.frame, texture.Width, texture.Height / Main.projFrames[Projectile.type])), color, Projectile.rotation + MathHelper.Pi, new Vector2(texture.Width, texture.Height / Main.projFrames[Projectile.type]) / 2, Projectile.scale, (SpriteEffects)1, 0f);
                }
            }
            else
            {
                if (Projectile.velocity.X > 0)
                {
                    Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / Main.projFrames[Projectile.type] * Projectile.frame, texture.Width, texture.Height / Main.projFrames[Projectile.type])), color, Projectile.rotation, new Vector2(texture.Width, texture.Height / Main.projFrames[Projectile.type]) / 2, Projectile.scale, 0, 0f);
                }
                else
                {
                    Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / Main.projFrames[Projectile.type] * Projectile.frame, texture.Width, texture.Height / Main.projFrames[Projectile.type])), color, Projectile.rotation + MathHelper.Pi, new Vector2(texture.Width, texture.Height / Main.projFrames[Projectile.type]) / 2, Projectile.scale, (SpriteEffects)1, 0f);
                }
            }
            return false;
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            if(Projectile.DProj().Bool[0])
            {
                overPlayers.Add(index);
            }
            else
            {
                //overPlayers.Clear();
            }
        }
    }
}
