using DDmod.Content.Dusts;
using DDmod.Content.Items.Melee.Sword;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace DDmod.Content.Projectiles.Ranged.Ammo
{
    public class 蘑菇子弹Proj : ModProjectile
    {
        public override string Texture => "DDmod/Image/Nullpng";
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Blood Bullet");
            //DisplayName.AddTranslation(7, "血猩弹");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 15;
        }
        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = true;
            Projectile.penetrate = 5;
            Projectile.extraUpdates = 2;
            Projectile.timeLeft = 600;
            Projectile.aiStyle = 1;
            AIType = 14;
            Projectile.GetGlobalProjectile<RangedProjectile>().BulletProj = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.alpha = 255;
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
        }
        List<NPC> N = new List<NPC>();
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            N.Add(target);
            Player player = Main.player[Projectile.owner];
            NPC npc = NPCdirection.FindClosest2(Projectile.Center,500,false, N);
            if(npc==null)
            {
                npc = NPCdirection.FindClosest(Projectile.Center, 500, false, target);
            }
            if (npc != null)
            {
                Projectile.Track(500, 0, Projectile.velocity.Length(), 0, false, npc.whoAmI);
                Projectile.localNPCImmunity[npc.whoAmI] = 0;
            }
            else
            {
                Projectile.velocity = Projectile.velocity.RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
            }
            Projectile.damage = (int)(Projectile.damage*0.8F);
            Projectile.netUpdate = true;
        }
        public override void OnKill(int timeLeft)
        {
            Projectile.position = Projectile.oldPosition;
            float A = Projectile.oldVelocity.Length() * 0.75F * (Projectile.extraUpdates + 1);
            if (A > 45)
            {
                A = 45;
            }
            for (int a = 0; a < 5; a++)
            {
                int dust = NewDust(Projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<Dusts.拉长粒子>(), 0, 0, 0, new Color(66, 255, 251, 200), 1);
                Main.dust[dust].velocity = -Projectile.oldVelocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)) * Main.rand.NextFloat(0.1F, 0.5f) * A;
                Main.dust[dust].noGravity = true;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = DDTextures.WhitePng.Value;
            //Main.spriteBatch.Draw(texture, Projectile.position - Main.screenPosition, null, Color.White, 0, Vector2.Zero, Projectile.Size / 2 * new Vector2(1, 1), 0, 0f);
            /*
            if (!Projectile.DProj().Bool[0])
            {
                //子弹
                float A = Projectile.ai[2];
                if (A > 90)
                {
                    A = 90;
                }
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + Projectile.velocity.PerfectNormalize() * Projectile.height / 2, null, Projectile.GetAlpha(new Color(99, 74, 187, 200)), Projectile.rotation, new Vector2(texture.Width / 2, 0), new Vector2(Projectile.scale, A), 0, 0f);
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + Projectile.velocity.PerfectNormalize() * Projectile.height / 2, null, Projectile.GetAlpha(new Color(99, 74, 187, 0) * 0.5F), Projectile.rotation, new Vector2(texture.Width / 2, 0), new Vector2(Projectile.scale, A / 8), 0, 0f);
            }*/
            for(int A = 0; A < Projectile.oldPos.Length;A++)
            {
                if (Projectile.oldPos[A]==Vector2.Zero)
                {
                    continue;
                }
                Vector2 vector = Projectile.oldPos[A] + Projectile.Size / 2;
                Vector2 oldP = new (1,0);
                float RO = Projectile.rotation;
                if(A>0)
                {
                    oldP = vector-(Projectile.oldPos[A-1] + Projectile.Size / 2);
                    RO = oldP.ToRotation() + MathHelper.PiOver2;
                }
                Main.spriteBatch.Draw(texture, vector - Main.screenPosition + Projectile.velocity.PerfectNormalize() * Projectile.height / 2, null, Projectile.GetAlpha(new Color(66,255, 251, 100)), RO, new Vector2(texture.Width / 2, 0), new Vector2(Projectile.scale, oldP.Length()/2), 0, 0f);
                if (A < 5)
                    Main.spriteBatch.Draw(texture, vector - Main.screenPosition + Projectile.velocity.PerfectNormalize() * Projectile.height / 2, null, Projectile.GetAlpha(new Color(100, 100, 100, 0)), RO, new Vector2(texture.Width / 2, 0), new Vector2(Projectile.scale, oldP.Length() / 2), 0, 0f);

            }
            return false;
        }
    }
}