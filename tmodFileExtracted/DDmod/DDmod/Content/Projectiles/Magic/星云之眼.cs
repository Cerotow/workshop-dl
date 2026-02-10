using DDmod.Content.Dusts;
using DDmod.Content.Particles;
using DDmod.Content.Projectiles.Magic.Staff;
using DDmod.Sync;
using Terraria.GameContent.Drawing;

namespace DDmod.Content.Projectiles.Magic
{
    public class 星云之眼 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
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
            Main.projFrames[Type] = 1;
            Projectile.hide = true;
        }
        int A;
        public override bool? CanDamage()
        {
            return false;
        }
        public override void AI()
        {
            Player player = Projectile.Player();
            Projectile.position += player.Dplayer().PrePosition / 2;
            for (int a = 1; a < Projectile.oldPos.Length; a++)
            {
                if (Projectile.oldPos[a] != Vector2.Zero)
                    Projectile.oldPos[a] += player.velocity / 2;
            }
            bool AC = false;
            for (int a = 0; a < 1000; a++)
            {
                Projectile Proj = Main.projectile[a];
                if (Proj.active && Projectile.owner == Proj.owner)
                {
                    if (Proj.type == ModContent.ProjectileType<星云幻梦Proj>())
                    {
                        AC = true;
                        break;
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
            //po(ProjNum);
            Projectile.ai[0]++;
            float rotationAngle = Projectile.ai[0] * 0.025f;
            rotationAngle %= MathHelper.TwoPi;

            Projectile.ai[1] += 0.003f;
            Projectile.ai[1] %= MathHelper.TwoPi;

            Vector2 ellipseOffset = new Vector2(
                (float)Math.Cos(rotationAngle) * 60f,
                (float)Math.Sin(rotationAngle) * 15f
            ).RotatedBy(Projectile.ai[1]);

            Vector2 targetPos = player.Center + ellipseOffset;

            Vector2 ellipseMajorAxis = new Vector2(60f, 0f).RotatedBy(Projectile.ai[1]);
            Vector2 playerForward = player.velocity.SafeNormalize(Vector2.UnitX);

            Vector2 toProjectile = (Projectile.Center - player.Center).SafeNormalize(Vector2.UnitX);
            float dot = Vector2.Dot(toProjectile, ellipseMajorAxis.SafeNormalize(Vector2.UnitX));

            bool isOnMajorAxisFront = dot > 0;
            float targetScale = isOnMajorAxisFront ? 1.25f : 0.75f;

            Projectile.scale = MathHelper.Lerp(Projectile.scale, targetScale, 0.02f);

            float distance = (targetPos - Projectile.Center).Length();
            float speed = MathHelper.Clamp(distance / 20f, 0.5f, 10f);
            Projectile.velocity = (targetPos - Projectile.Center).SafeNormalize(Vector2.Zero) * speed;

            NPC npc = NPCdirection.FindClosest(Projectile.Center, 1000, false);
            if (npc != null)
            {
                Projectile.DProj().vector[0] = (npc.Center - Projectile.Center) / 10;
                if (Projectile.DProj().vector[0].Length() >= 4 * Projectile.scale)
                {
                    Projectile.DProj().vector[0] = Projectile.DProj().vector[0].PerfectNormalize() * 4 * Projectile.scale;
                }
                Projectile.localAI[1]++;
                if(Projectile.localAI[1]>=60+Main.rand.Next(120))
                {
                    Projectile.NewProjectileChange(Projectile.Center + Projectile.DProj().vector[0], Projectile.DProj().vector[0].PerfectNormalize() *8, ModContent.ProjectileType<星云弹>(), Projectile.damage, 5);
                    Projectile.localAI[1] = 0;
                }
                Projectile.localAI[0] = 0;
            }
            else
            {
                Projectile.localAI[0] = -100;
            }
        }
        public void po(int ProjNum)
        {
            Player player = Projectile.Player();

            Vector2 vector = new Vector2(0, 60).RotatedBy(Projectile.ai[1] + Projectile.ai[0]* 0.025F);
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
                Dust.NewDust(Projectile.Center, 1, 1, 255);
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
            Color color = new Color(0, 100, 255, 0);
            Rectangle rectangle = new Rectangle(0, 0, texture.Width/2, texture.Height);
            Main.spriteBatch.Draw(texture, Projectile.Center- Main.screenPosition, rectangle, Color.White, Projectile.rotation, rectangle.Size()/2, Projectile.scale, spriteEffects, 0f);
            if (Projectile.localAI[0] == -100)
            {
                rectangle.X = texture.Width / 2;
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, rectangle, Color.White, Projectile.rotation, rectangle.Size() / 2, Projectile.scale, 0, 0f);

            }
            else
            {
                rectangle.X = texture.Width / 2;
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + Projectile.DProj().vector[0], rectangle, Color.White, Projectile.rotation, rectangle.Size() / 2, Projectile.scale, 0, 0f);


            }
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