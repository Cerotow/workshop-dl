using DDmod.Content.Dusts;
using DDmod.Content.Items.Melee.Sword;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace DDmod.Content.Projectiles.Ranged.Ammo
{
    public class 魔金弹Proj : ModProjectile
    {
        public override string Texture => "DDmod/Image/Nullpng";
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Blood Bullet");
            //DisplayName.AddTranslation(7, "血猩弹");
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
        }
        public override void AI()
        {
            if (Projectile.ai[2] < Projectile.velocity.Length() * (Projectile.extraUpdates + 1))
            {
                Projectile.ai[2] += Projectile.velocity.Length() / 10 * (Projectile.extraUpdates + 1);
            }
            else
            {
                Projectile.ai[2] = Projectile.velocity.Length() * (Projectile.extraUpdates + 1);
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            if (Projectile.ai[1] == 1)
            {
                Projectile.extraUpdates = 20;

                if (!Projectile.DProj().Bool[1])
                {
                    for (int a = 0; a < 25; a++)
                    {
                        int dust = NewDust(Projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<Dusts.光球粒子>(), 0, 0, 0, new Color(99, 74, 187, 200), 0.8F);
                        Main.dust[dust].velocity = new Vector2(2, 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
                        Main.dust[dust].noGravity = true;
                        Main.dust[dust].customData = 1 + Main.dust[dust].DustAI(1);
                    }
                    Projectile.DProj().Bool[1] = true;
                }
                for (int a = 0; a < 1; a++)
                {
                    int dust = NewDust(Projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<Dusts.光球粒子>(), 0, 0, 0, new Color(99, 74, 187, 200), 0.4F);
                    Main.dust[dust].velocity = Vector2.Zero;
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].customData = 3;
                }
                Projectile.tileCollide = false;
                NPC npc = Main.npc[(int)Projectile.DProj().Times[1]];
                if (npc.CanBeChasedBy())
                {
                    Projectile.Chase(npc, 2, 100);
                    //Projectile.position += (npc.position - npc.oldPosition) / (Projectile.extraUpdates + 1);
                }
            }
            if (Projectile.ai[1] == 2)
            {
                Projectile.extraUpdates = 15;

                Projectile.localAI[2] += Projectile.velocity.Length();
                if (Projectile.localAI[2] > 50)
                {
                    for (int a = 0; a < 1; a++)
                    {
                        int dust = NewDust(Projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<Dusts.光球粒子>(), 0, 0, 0, new Color(99, 74, 187, 200), 0.4F);
                        Main.dust[dust].velocity = Vector2.Zero;
                        Main.dust[dust].noGravity = true;
                        Main.dust[dust].customData = 2F;
                    }
                }
                Projectile.Track(500,60,2,120);
            }
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];

            if (target.type != NPCID.TargetDummy && target.life > 2 && Projectile.ai[1]==0)
            {
                Vector2 vector = target.Center - new Vector2(0, Main.rand.Next(380, 640)).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
                int R = NewProjectile(Projectile.GetSource_FromThis(), vector, (target.Center - vector).PerfectNormalize() * 2, Projectile.type, Projectile.damage / 3, 0f, player.whoAmI, player.whoAmI,1);
                Main.projectile[R].DProj().Magnification /= 3;
                Main.projectile[R].DProj().Times[1] = target.whoAmI;
            }
        }
        public override void OnKill(int timeLeft)
        {
            Projectile.position = Projectile.oldPosition;
            if (Projectile.ai[1]==0)
            {
                float A = Projectile.oldVelocity.Length() * 0.75F * (Projectile.extraUpdates + 1);
                if (A > 45)
                {
                    A = 45;
                }
                for (int a = 0; a < 5; a++)
                {
                    int dust = NewDust(Projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<Dusts.拉长粒子>(), 0, 0, 0, new Color(99, 74, 187, 200), 1);
                    Main.dust[dust].velocity = -Projectile.oldVelocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)) * Main.rand.NextFloat(0.1F, 0.5f) * A;
                    Main.dust[dust].noGravity = true;
                }
            }
            else
            {
                float A = Projectile.oldVelocity.Length() * 0.5F * (Projectile.extraUpdates + 1);
                if (A > 45)
                {
                    A = 45;
                }
                for (int a = 0; a < 15; a++)
                {
                    int dust = NewDust(Projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<Dusts.光球粒子>(), 0, 0, 0, new Color(99, 74, 187, 200), 0.8f);
                    Main.dust[dust].velocity = -Projectile.oldVelocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)) * Main.rand.NextFloat(0.1F, 0.5f) * A;
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].customData = 2 + Main.dust[dust].DustAI(1);
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = DDTextures.WhitePng.Value;
            //Main.spriteBatch.Draw(texture, Projectile.position - Main.screenPosition, null, Color.White, 0, Vector2.Zero, Projectile.Size / 2 * new Vector2(1, 1), 0, 0f);
            if (Projectile.ai[1]==0)
            {
                //子弹
                float A = Projectile.ai[2];
                if (A > 90)
                {
                    A = 90;
                }
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + Projectile.velocity.PerfectNormalize() * Projectile.height / 2, null, Projectile.GetAlpha(new Color(99, 74, 187, 200)), Projectile.rotation, new Vector2(texture.Width / 2, 0), new Vector2(Projectile.scale, A), 0, 0f);
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + Projectile.velocity.PerfectNormalize() * Projectile.height / 2, null, Projectile.GetAlpha(new Color(99, 74, 187, 0) * 0.5F), Projectile.rotation, new Vector2(texture.Width / 2, 0), new Vector2(Projectile.scale, A / 8), 0, 0f);

            }
            return false;
        }
    }
}