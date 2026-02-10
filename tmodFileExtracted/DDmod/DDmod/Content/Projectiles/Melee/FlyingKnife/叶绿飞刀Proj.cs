using Terraria;
namespace DDmod.Content.Projectiles.Melee.FlyingKnife
{
    public class 叶绿飞刀Proj : 飞刀Proj
    {

        public override void Defaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 17;
            AIStyle = 飞刀AI.AI3;
            Rotation = MathHelper.PiOver2;
            Rotation2 = MathHelper.Pi;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 1200;
            Projectile.alpha = 2;
        }
        public override bool PreAI()
        {
            if (Projectile.ai[2] == 0)
            {
                Projectile.alpha--;
                Projectile.Track(300, 20, 12);
                if (Projectile.extraUpdates != 3)
                {
                    Projectile.velocity /= 4;
                    Projectile.extraUpdates = 3;
                }
                Projectile.ai[0]++;
                if (Projectile.ai[0] == 20)
                {
                    NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.velocity.RotatedBy(-0.2F) * 4, Projectile.type, Projectile.damage, Projectile.knockBack, -1, 40);
                    NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.velocity.RotatedBy(0.2F) * 4, Projectile.type, Projectile.damage, Projectile.knockBack, -1, 40);
                }
            }
            else if (Projectile.ai[2] == 2 || Projectile.ai[2] <= -3)
            {
                if (Projectile.timeLeft >120)
                {
                    Projectile.timeLeft = 120;
                }
                Projectile.extraUpdates = 0;
                Projectile.position = Projectile.oldPosition;
            }
            return true;
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            if (Projectile.ai[2]<=0)
                Projectile.ai[2]--;
            Projectile.netUpdate = true;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.ai[2] = 2; 
            Projectile.velocity = oldVelocity;
            Projectile.tileCollide = false;
            Projectile.netUpdate = true;
            return false;
        }
        public override bool? CanHitNPC(NPC target)
        {
            if(!(Projectile.ai[2] == 2 || Projectile.ai[2] <= -3))
            {
                return base.CanHitNPC(target);
            }
            return false;
        }
        public override void OnKill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];

        }
        public override bool? CanDamage()
        {
            return (Projectile.ai[2] == 2 || Projectile.ai[2] <= -3)==false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if(Projectile.alpha>0)
            {
                return false;
            }
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 vector = Projectile.Size/2;
            Player player = Main.player[Projectile.owner];
            float r = 1;
            if (Projectile.timeLeft <= 120)
            {
                r = (float)Projectile.timeLeft / 120;
            }
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 vector2 = Projectile.oldPos[i] + vector - Main.screenPosition;
                Color color = new Color(105, 221, 0, 0) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length)*0.4f;
                Main.spriteBatch.Draw(texture, vector2, null, color* r, Projectile.oldRot[i], new Vector2(texture.Width/2, texture.Height / 4), Projectile.scale * 1.2F, 0, 0f);
                Main.spriteBatch.Draw(texture, vector2, null, color* r, Projectile.oldRot[i], new Vector2(texture.Width / 2, texture.Height / 4), Projectile.scale * 0.8F, 0, 0f);
            }
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition,null, lightColor* r, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 4), Projectile.scale, 0, 0f);

            return false;
        }
    }
}