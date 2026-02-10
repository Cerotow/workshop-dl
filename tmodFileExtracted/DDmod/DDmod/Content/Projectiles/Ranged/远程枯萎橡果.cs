using DDmod.Content.Dusts;

namespace DDmod.Content.Projectiles.Ranged
{
    public class 远程枯萎橡果 : ModProjectile
    {
        public override void SetDefaults()
        {

            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.aiStyle = 14;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 520;
            Main.projFrames[Projectile.type] = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 40;
            Projectile.DamageType = DamageClass.Ranged;
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;

        }
        public override void AI()
        {
            NewDustChange(2, Projectile.position, Projectile.Size, ModContent.DustType<枯萎粒子>(), 1F, 2F,true,0.6F);
            if((Projectile.velocity.Length())<0.1F)
            {
                Projectile.Kill();
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.velocity.X = -oldVelocity.X;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.velocity.Y = -oldVelocity.Y;
            }
            Projectile.velocity *= 0.5f;
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            NewDustChange(40, Projectile.position, Projectile.Size, ModContent.DustType<枯萎粒子>(), 1F, 8F);
            if (Main.myPlayer == Projectile.owner)
                NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center - new Vector2(0, 10), new Vector2(0, -10), ModContent.ProjectileType<远程枯萎光束>(), Projectile.damage / 3, 0, Projectile.owner);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Vector2 vector = new Vector2(texture.Width / 2, texture.Height / 2);
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 vector2 = Projectile.oldPos[i] + new Vector2(Projectile.width, Projectile.height) / 2 - Main.screenPosition;
                Color color = new Color(255, 0, 0, 50) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2f);
                Main.spriteBatch.Draw(texture, vector2, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale * 1.4f * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), 0, 0f);
            }
            Main.spriteBatch.Draw(texture, Projectile.position + new Vector2(Projectile.width, Projectile.height) / 2 - Main.screenPosition, null, lightColor, Projectile.rotation, vector, Projectile.scale, 0, 0f);
            return false;
        }
    }
    public class 远程枯萎光束 : ModProjectile
    {
        public override string Texture => "DDmod/Image/Nullpng";
        public override void SetDefaults()
        {

            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 120;
            Main.projFrames[Projectile.type] = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 40;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.extraUpdates = 0;
        }

        public override void SetStaticDefaults()
        {

            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 15;
        }
        public override bool? CanDamage()
        {
            return Projectile.ai[0]>15;
        }
        public override void AI()
        {
            Projectile.ai[0]++;
            DDHelper.BackAndForth(-1, 1, 0.1F, ref Projectile.ai[1], ref Projectile.DProj().Bool[0]);
            NPC npc = Projectile.FindTargetWithinRange(300, false);
            if (Projectile.DProj().vector[0] == Vector2.Zero)
            {
                Projectile.DProj().vector[0] = Projectile.velocity;
            }
            if (npc != null && npc.active && Projectile.GetGlobalProjectile<DDGlobalProjectile>().track > 15)
            {
                if (!Projectile.hostile && Projectile.friendly)
                {
                    Vector2 vector = (npc.Center - Projectile.Center).RotatedBy(Projectile.ai[1]).PerfectNormalize() * 14;
                    Projectile.velocity = (Projectile.velocity * 30 + vector) / (31);
                }
                Projectile.DProj().vector[0] = Projectile.velocity;
            }
            else
            {
                Projectile.velocity = Projectile.DProj().vector[0].RotatedBy(Projectile.ai[1]);
            }
        }

        public override void OnKill(int timeLeft)
        {
            NewDustChange(40, Projectile.position, Projectile.Size, ModContent.DustType<枯萎粒子>(), 1F, 3F);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = DDTextures.VoidStar.Value;
            Vector2 vector = new Vector2(Projectile.width, Projectile.height) / 2;
            float Length = Projectile.velocity.Length() / (Projectile.height / 4);
            for (float l = 0; l < Length; l++)
            {
                for (int i = 0; i < Projectile.oldPos.Length; i++)
                {
                    Vector2 vector2 = Projectile.oldPos[i] - Main.screenPosition + vector;
                    Color color2 = Projectile.GetAlpha(new Color(255, 0, 0, 0)) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2f);
                    Main.spriteBatch.Draw(texture, vector2 - Projectile.velocity / Length * l, null, color2, Projectile.rotation, texture.Size() / 2, Projectile.scale * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length)/4, SpriteEffects.None, 0);
                    Main.spriteBatch.Draw(texture, vector2 - Projectile.velocity / Length * l, null, color2, Projectile.rotation, texture.Size() / 2, Projectile.scale * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length)/4, SpriteEffects.None, 0);
                    color2 = Projectile.GetAlpha(new Color(0, 150, 150, 0)) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2f);
                    Main.spriteBatch.Draw(texture, vector2 - Projectile.velocity / Length * l, null, color2, Projectile.rotation, texture.Size() / 2, Projectile.scale / 2f * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length)/4, SpriteEffects.None, 0);
                }
            }
            return false;
        }
    }
}