using DDmod.Content.Dusts;
using DDmod.Content.Particles;
using Terraria;
using Terraria.GameContent.Drawing;

namespace DDmod.Content.Projectiles.GeneralProj
{
    public class 蘑菇 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.alpha = 0;
            Projectile.penetrate = 5;
            Projectile.scale = 1;
            Projectile.timeLeft = 600;
        }
        int A;
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.ai[0] != 2)
            {
                if (Projectile.velocity.X != oldVelocity.X)
                {
                    Projectile.velocity.X = -oldVelocity.X * 0.3F;
                }
                if (Projectile.velocity.Y != oldVelocity.Y)
                {
                    Projectile.velocity.Y = -oldVelocity.Y * 0.3F;
                }
            }
            else
            {
                Projectile.localAI[1]++;
            }
            return false;
        }
        public override void AI()
        {
            if (Projectile.ai[0] == 0)
            {
                Projectile.rotation += Projectile.velocity.X * 0.06F;
                Projectile.velocity.X *= 0.98F;
                if (Projectile.velocity.Length() < 0.1F)
                {
                    Projectile.Kill();
                }
                if (Projectile.DProj().track > 20 && Projectile.velocity.Y < 10)
                {
                    Projectile.velocity.Y += 0.2F;
                }
                Projectile.ai[2] = 1F;
            }
            else if (Projectile.ai[0] == 1)
            {
                Projectile.penetrate = 1;
                if (Projectile.ai[1] >= 0)
                {
                    if (!Main.npc[(int)Projectile.ai[1]].active)
                    {
                        Projectile.Kill();
                    }
                    if (!Projectile.getRect().Intersects(Main.npc[(int)Projectile.ai[1]].getRect()))
                    {
                        Projectile.position += (Main.npc[(int)Projectile.ai[1]].Center - Projectile.Center).PerfectNormalize();
                    }
                    Projectile.rotation = (Main.npc[(int)Projectile.ai[1]].Center - Projectile.Center).ToRotation() + MathHelper.PiOver2;
                    Projectile.position += Main.npc[(int)Projectile.ai[1]].position - Main.npc[(int)Projectile.ai[1]].oldPosition;
                }
                if (Projectile.ai[2] < 1F)
                {
                    Projectile.ai[2] += 0.01F;
                }
            }
            else if (Projectile.ai[0] == 2)
            {
                Projectile.penetrate = -1;
                Projectile.rotation += Projectile.velocity.X * 0.06F;
                Projectile.velocity.X *= 0.98F;
                if (Projectile.DProj().track > 20 && Projectile.velocity.Y < 10)
                {
                    Projectile.velocity.Y += 0.2F;
                }

                if (Projectile.localAI[1] >= 2 && Projectile.localAI[0] == 0)
                {
                    Projectile.localAI[0]++;
                }

                Projectile.ai[2] = 0F;
                if (Projectile.localAI[0] == 0)
                {
                    Projectile.ai[2] = 1F;
                }
                if (Projectile.localAI[0] == 1)
                {
                    Projectile.scale *= 4f;
                    Projectile.ProjScaleChange();
                    for (int A = 0; A < 50; A++)
                    {
                        if (A < 15)
                            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height - 4, ModContent.DustType<蘑菇粒子>(), Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-2, 2), 0, default(Color), Projectile.scale / 4);
                        Dust.NewDust(Projectile.position, Projectile.width, Projectile.height - 4, ModContent.DustType<光球粒子>(), Main.rand.NextFloat(-6, 6), Main.rand.NextFloat(-6, 6), 0, new Color(201, 105, 45, 0), Projectile.scale / 8);
                        if (A > 15)
                        {

                            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height - 4, ModContent.DustType<拉长粒子>(), Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-2, 2), 0, new Color(201, 105, 45, 0), Projectile.scale / 4);
                        }
                    }
                    SoundStyle sound = SoundID.Item14;
                    sound.Pitch = -1;
                    PlaySound(sound, Projectile.Center);
                    Projectile.timeLeft = 3;
                    Projectile.localAI[0]++;
                }
                Projectile.ProjScaleChange();
            }
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            if (Projectile.localAI[0] == 0)
                Projectile.localAI[0]=1;
        }
        public override bool? CanHitNPC(NPC target)
        {
            if (Projectile.ai[0] == 2)
            {
                return null;
            }
            if (Projectile.ai[2] >= 1F)
            {
                if (Projectile.ai[0] != 1|| target.whoAmI == Projectile.ai[1])
                {
                    return null;
                }
            }
            return false;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(255, 255, 255, Projectile.alpha));
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
        }
        public override void OnKill(int timeLeft)
        {
            if (Projectile.ai[0] == 2)
            {
            }
            else
            {

                for (int A = 0; A < 30; A++)
                {
                    Dust.NewDust(Projectile.position, Projectile.width, Projectile.height - 4, ModContent.DustType<蘑菇粒子>(), Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-2, 2), 0, default(Color), Projectile.scale);
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 v = Projectile.Center - Main.screenPosition;
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];

            texture.DrawCentre(Projectile, null, lightColor* Projectile.ai[2], Projectile.scale);
            if (Projectile.ai[0] == 2)
            {
                texture.DrawCentre(Projectile, null, new Color(201, 45, 45, 0) * Projectile.ai[2], Projectile.scale + 0.1f);
                texture.DrawCentre(Projectile, null, new Color(201, 45, 45, 0) * Projectile.ai[2], Projectile.scale + 0.1f);
                texture.DrawCentre(Projectile, null, new Color(201, 45, 45, 0) * Projectile.ai[2], Projectile.scale + 0.1f);
            }
            return false;
            if (Projectile.ai[0] == 0)
            {
            }
            else
            {
                
                Main.spriteBatch.Draw(texture, Projectile.position - Main.screenPosition+new Vector2(Projectile.width/2,Projectile.height), null, lightColor, Projectile.rotation, new Vector2(texture.Width/2,texture.Height), Projectile.scale, 0, 0f);
            }
            return false;
        }
    }
}