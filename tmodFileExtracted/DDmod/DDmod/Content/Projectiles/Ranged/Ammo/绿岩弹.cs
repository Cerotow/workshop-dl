using DDmod.Content.Items.Melee.Sword;
using Microsoft.Xna.Framework.Graphics;

namespace DDmod.Content.Projectiles.Ranged.Ammo
{
    public class 绿岩弹Proj : ModProjectile
    {
        public override string Texture => "DDmod/Image/Nullpng";
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = true;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 2;
            Projectile.timeLeft = 600;
            Projectile.aiStyle = 1;
            AIType = 14;
            Projectile.GetGlobalProjectile<RangedProjectile>().BulletProj = true;
            Projectile.alpha = 255;
        }
        public override void AI()
        {
            /*
            if (Projectile.ai[2] < Projectile.velocity.Length() * (Projectile.extraUpdates + 1))
            {
                Projectile.ai[2] += Projectile.velocity.Length() / 10 * (Projectile.extraUpdates + 1);
            }
            else
            {
                Projectile.ai[2] = Projectile.velocity.Length() * (Projectile.extraUpdates + 1);
            }*/
            Projectile.ai[2] += Projectile.velocity.Length();
            if (Projectile.ai[2] > 50)
            {
                for (int a = 0; a < 1; a++)
                {
                    int dust = NewDust(Projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<Dusts.速度粒子>(), 0, 0, 0, new Color(51, 255, 51, 155), 1.8F);
                    Main.dust[dust].velocity = Vector2.Zero;
                    Main.dust[dust].rotation = Projectile.velocity.ToRotation();
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].customData = 2;
                }
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            if (Projectile.ai[0] == 1)
            {
                Projectile.Track(500, 0, Projectile.velocity.Length(), 0, false, (int)Projectile.ai[1]);
                if (!Main.npc[(int)Projectile.ai[1]].CanBeChasedBy()|| !Collision.CanHitLine(Projectile.Center, 1, 1, Main.npc[(int)Projectile.ai[1]].position, Main.npc[(int)Projectile.ai[1]].width, Main.npc[(int)Projectile.ai[1]].height))
                {
                    Projectile.ai[0] = -1;
                }
            }
        }
        public override bool? CanHitNPC(NPC target)
        {
            if (Projectile.ai[0] == 1)
            {
                return Projectile.ai[1] == target.whoAmI ? null : false;
            }
            return base.CanHitNPC(target);
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            if (Projectile.ai[0] == 0)
            {
                float A = Projectile.oldVelocity.Length() * 0.75F * (Projectile.extraUpdates + 1);
                if (A > 45)
                {
                    A = 45;
                }
                for (int a = 0; a < 5; a++)
                {
                    int dust = NewDust(Projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<Dusts.速度粒子>(), 0, 0, 0, new Color(51, 255, 51, 155), 1.8F);
                    Main.dust[dust].velocity = -Projectile.oldVelocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)) * Main.rand.NextFloat(0F, 0.2f) * A;
                    Main.dust[dust].rotation = Main.dust[dust].velocity.ToRotation();
                    Main.dust[dust].noGravity = true;
                }
                List<NPC> N = [target];
                for (int A2 = 0; A2 < 5; A2++)
                {
                    NPC npc = NPCdirection.FindClosest2(Projectile.Center, 500, false, N);
                    if (npc != null)
                    {
                        N.Add(npc);
                        Vector2 vector = npc.Center - Projectile.Center;
                        NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, vector.PerfectNormalize() * Projectile.velocity.Length(), Projectile.type, Projectile.damage / 4, Projectile.knockBack / 4, -1, 1, npc.whoAmI);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else
            {
                float A = Projectile.oldVelocity.Length() * 0.75F * (Projectile.extraUpdates + 1);
                if (A > 45)
                {
                    A = 45;
                }
                for (int a = 0; a < 5; a++)
                {
                    int dust = NewDust(Projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<Dusts.速度粒子>(), 0, 0, 0, new Color(51, 255, 51, 155), 1.8F);
                    Main.dust[dust].velocity = -Projectile.oldVelocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)) * Main.rand.NextFloat(0.1F, 0.5f) * A;
                    Main.dust[dust].rotation = Main.dust[dust].velocity.ToRotation();
                    Main.dust[dust].noGravity = true;
                }
            }
            /*
            if (Projectile.ai[0] == 0)
            {
                float A = Projectile.oldVelocity.Length() * 0.75F * (Projectile.extraUpdates + 1);
                if (A > 45)
                {
                    A = 45;
                }
                for (int a = 0; a < 5; a++)
                {
                    int dust = NewDust(Projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<Dusts.拉长粒子>(), 0, 0, 0, new Color(11, 255, 11, 155), 1);
                    Main.dust[dust].velocity = -Projectile.oldVelocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)) * Main.rand.NextFloat(0F, 0.2f) * A;
                    Main.dust[dust].noGravity = true;
                }
                List<NPC> N = new List<NPC>();
                N.Add(target);
                for (int A2 = 0; A2 < 5; A2++)
                {
                    NPC npc = NPCdirection.FindClosest2(Projectile.Center, 500, false, N);
                    if (npc != null)
                    {
                        N.Add(npc);
                        Vector2 vector = npc.Center - Projectile.Center;
                        NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, vector.PerfectNormalize() * Projectile.velocity.Length(), Projectile.type, Projectile.damage / 4, Projectile.knockBack / 4, -1, 1,target.whoAmI);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else
            {
                float A = Projectile.oldVelocity.Length() * 0.75F * (Projectile.extraUpdates + 1);
                if (A > 45)
                {
                    A = 45;
                }
                for (int a = 0; a < 5; a++)
                {
                    int dust = NewDust(Projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<Dusts.拉长粒子>(), 0, 0, 0, new Color(11, 255, 11, 155), 1);
                    Main.dust[dust].velocity = -Projectile.oldVelocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)) * Main.rand.NextFloat(0.1F, 0.5f) * A;
                    Main.dust[dust].noGravity = true;
                }
            }*/
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.position = Projectile.oldPosition;
            return base.OnTileCollide(oldVelocity);
        }
        public override void OnKill(int timeLeft)
        {
            float A = Projectile.oldVelocity.Length() * 0.75F * (Projectile.extraUpdates + 1);
            if (A > 45)
            {
                A = 45;
            }
            for (int a = 0; a < 5; a++)
            {
                int dust = NewDust(Projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<Dusts.速度粒子>(), 0, 0, 0, new Color(51, 255, 51, 155), 1.8F);
                Main.dust[dust].velocity = -Projectile.oldVelocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)) * Main.rand.NextFloat(0.1F, 0.5f) * A;
                Main.dust[dust].rotation = Main.dust[dust].velocity.ToRotation();
                Main.dust[dust].noGravity = true;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
            //子弹
            float A = Projectile.ai[2];
            if (A > 90)
            {
                A = 90;
            }
            Texture2D texture = DDTextures.WhitePng.Value;
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + Projectile.velocity.PerfectNormalize() * Projectile.height / 2, null, Projectile.GetAlpha(new Color(101, 255, 101, 155)), Projectile.rotation, new Vector2(texture.Width / 2, 0), new Vector2(Projectile.scale, A), 0, 0f);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + Projectile.velocity.PerfectNormalize() * Projectile.height / 2, null, Projectile.GetAlpha(new Color(50, 50, 50, 155)), Projectile.rotation, new Vector2(texture.Width / 2, 0), new Vector2(Projectile.scale, A / 8), 0, 0f);

            return false;
        }
    }
}