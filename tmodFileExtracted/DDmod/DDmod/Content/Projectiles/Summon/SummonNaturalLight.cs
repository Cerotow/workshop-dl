using DDmod.Content.Dusts;
using DDmod.Content.NPCs;

namespace DDmod.Content.Projectiles.Summon
{
    public class SummonNaturalLight : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
        }
        public override void SetDefaults()
        {
            Projectile.width = 90;
            Projectile.height = 90;
            Projectile.scale /= 3;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 600;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.minion = true;
            Projectile.aiStyle = -1;
        }
        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, new Vector3(20, 255, 20) * 0.003F);
            if (Projectile.ai[0]>0)
            {
                NPC npc = Main.npc[(int)(Projectile.ai[0] - 1)];
                if (npc.CanBeChasedBy())
                {

                    Projectile.Track(1000, 20, 12, 50, true, (int)(Projectile.ai[0] - 1));
                }
                else
                {
                    Projectile.ai[0] = 0;
                }
            }
        }
        public override bool? CanHitNPC(NPC target)
        {
            if(Projectile.ai[0]>0)
            {
                return (Projectile.ai[0]-1) == target.whoAmI;
            }
            return base.CanHitNPC(target);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Vector2 vector = new Vector2(Projectile.width, Projectile.height) / 2;
            float Length = Projectile.velocity.Length() / (Projectile.height / 4);
            for (float l = 0; l < Length; l++)
            {
                for (int i = 0; i < Projectile.oldPos.Length; i++)
                {
                    Vector2 vector2 = Projectile.oldPos[i] - Main.screenPosition + vector;
                    Color color2 = Projectile.GetAlpha(new Color(0, 255, 0, 0)) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2f);
                    Main.spriteBatch.Draw(texture, vector2 - Projectile.velocity / Length * l, null, color2, Projectile.rotation, texture.Size() / 2, Projectile.scale * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), SpriteEffects.None, 0);
                    color2 = Projectile.GetAlpha(new Color(200, 0, 200, 0)) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2f);
                    Main.spriteBatch.Draw(texture, vector2 - Projectile.velocity / Length * l, null, color2, Projectile.rotation, texture.Size() / 2, Projectile.scale / 2 * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), SpriteEffects.None, 0);
                }
            }
            return false;
        }
        public override void OnKill(int timeLeft)
        {
            for (int A = 0; A < 30; A++)
            {
                int Type = ModContent.DustType<生命粒子>();
                Dust dust = Main.dust[NewDust(Projectile.Center , 1, 1, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                dust.noGravity = true;
                dust.scale = 1.4f;
                dust.velocity = new Vector2(0, -1).RotatedBy(Main.rand.NextFloat(0,MathHelper.TwoPi)) * Main.rand.NextFloat(2, 4);
            }
        }
    }
}