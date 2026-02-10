using DDmod.Content.Dusts;
using DDmod.Content.Particles;
using DDmod.Content.Projectiles.Magic.Staff;
using DDmod.Sync;
using Terraria.GameContent.Drawing;

namespace DDmod.Content.Projectiles.Magic
{
    public class SmallMeteors : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
        }
        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.alpha = 0;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 1;
            Projectile.scale = 1F;
            Projectile.timeLeft = 600;
            CooldownSlot = 1;
            Main.projFrames[Type] = 4;
            Projectile.frame = Main.rand.Next(4);
            Projectile.hide = true;
        }
        int A;
        public override void AI()
        {
            Player player = Projectile.Player();
            Projectile.position += player.Dplayer().PrePosition / 2;
            for (int a = 1; a < Projectile.oldPos.Length; a++)
            {
                if (Projectile.oldPos[a] != Vector2.Zero)
                    Projectile.oldPos[a] += player.velocity / 2;
            }
            int ProjNum = 0;
            bool AC = false;
            bool A = false;
            for (int a = 0; a < 1000; a++)
            {
                Projectile Proj = Main.projectile[a];
                if (Proj.active && Projectile.owner == Proj.owner)
                {
                    if (Proj.type == ModContent.ProjectileType<MeteorStaff>())
                    {
                        AC = true;
                        break;
                    }
                }
            }
            for (int a = 0; a < Projectile.whoAmI; a++)
            {
                Projectile Proj = Main.projectile[a];
                if (Proj.active && Projectile.owner == Proj.owner)
                {
                    if (Proj.type == Projectile.type)
                    {
                        ProjNum++;
                    }
                }
            }
            if (!AC)
            {
                Projectile.Kill();
            }
            Projectile.timeLeft = 5;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            po(ProjNum);

            Vector2 vector = new Vector2(0, 60).RotatedBy(MathHelper.TwoPi / 6 * ProjNum + player.Dplayer().PlayerTimes * player.direction * 0.025F);

            if (Projectile.ai[1]<0)
            {
                vector = new Vector2(0, -60).RotatedBy(MathHelper.TwoPi / 6 * ProjNum + player.Dplayer().PlayerTimes * player.direction * 0.025F);
            }
            vector.X *= 1f;
            vector.Y *= 0.1f;
            //vector = vector.RotatedBy(player.Dplayer().PlayerTimes*0.01);

            if (Projectile.oldVelocity.X > 0)
            {
                if (vector.X > 0)
                {
                    Projectile.scale = 1.25F - ((vector.X / 60F)/4);
                }
                else
                {
                    Projectile.scale = 1F + ((60-(-vector.X)) / 60F) / 4;
                }
            }
            else
            {
                if (vector.X > 0)
                {
                    Projectile.scale = 1.25F - ((120-vector.X) / 60F) / 4;
                }
                else
                {
                    Projectile.scale = 0.75F + ((-vector.X) / 60F) / 4;
                }
            }

            if (Projectile.ai[1] < 0)
            {
                vector = vector.RotatedBy(player.Dplayer().PlayerTimes / 200F+MathHelper.PiOver2);
            }
            else
            {
                vector = vector.RotatedBy(player.Dplayer().PlayerTimes / 200F);
            }
            float F = (player.Center + vector - Projectile.Center).Length() / 40;
            if (F < 0.5)
            {
                //F = 0.5F;
            }

            Projectile.velocity = (player.Center + vector - Projectile.Center).PerfectNormalize() * F;

            //Projectile.velocity = Projectile.velocity.RotatedBy((-MathHelper.PiOver4 + 0.1F) * player.direction);
        }
        public void po(int ProjNum)
        {
            Player player = Projectile.Player();

            Vector2 vector = new Vector2(0, 60).RotatedBy(MathHelper.TwoPi / 6 * ProjNum + player.Dplayer().PlayerTimes * player.direction * 0.025F);

            if (Projectile.ai[1] < 0)
            {
                vector = new Vector2(0, -60).RotatedBy(MathHelper.TwoPi / 6 * ProjNum + player.Dplayer().PlayerTimes * player.direction * 0.025F);
            }
            vector.X *= 1f;
            vector.Y *= 0.1f;

            if (Projectile.DProj().vector[0].X > 0)
            {
                if (vector.X > 0)
                {
                    Projectile.scale = 1.25F - ((vector.X / 60F) / 4);
                }
                else
                {
                    Projectile.scale = 1F + ((60 - (-vector.X)) / 60F) / 4;
                }
            }
            else
            {
                if (vector.X > 0)
                {
                    Projectile.scale = 1.25F - ((120 - vector.X) / 60F) / 4;
                }
                else
                {
                    Projectile.scale = 0.75F + ((-vector.X) / 60F) / 4;
                }
            }
            float F = (player.Center + vector - Projectile.Center).Length() / 40;

            Projectile.DProj().vector[0] = (player.Center + vector - Projectile.Center).PerfectNormalize() * F;

        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(255, 255, 255, Projectile.alpha));
        }
        public override void OnKill(int timeLeft)
        {
            for (int a = 0; a < 6; a++)
            {
                Dust.NewDust(Projectile.Center, 1, 1, 6);
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects spriteEffects = (SpriteEffects)1;
            if (Projectile.direction == 1)
            {
                spriteEffects = 0;
            }
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Vector2 vector = new Vector2(texture.Width / 2, texture.Height / 8);
            Color color = new Color(0, 100, 255, 0);
            Main.spriteBatch.Draw(texture, Projectile.position + new Vector2(Projectile.width, Projectile.height) / 2 - Main.screenPosition, new Rectangle?(new Rectangle(0,texture.Height/4*Projectile.frame,texture.Width,texture.Height/4)), Color.White, Projectile.rotation, vector, Projectile.scale, spriteEffects, 0f);
            return false;
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            if(Projectile.scale>1f)
            {
                overPlayers.Add(index);
            }
            else if(Projectile.scale<1f)
            {
                behindProjectiles.Add(index);
            }
        }
    }
}