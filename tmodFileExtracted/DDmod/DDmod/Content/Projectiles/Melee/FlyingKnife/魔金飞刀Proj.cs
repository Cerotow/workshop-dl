namespace DDmod.Content.Projectiles.Melee.FlyingKnife
{
    public class 魔金飞刀Proj : 飞刀Proj
    {

        public override void Defaults()
        {
            AIStyle = 飞刀AI.AI3;
            Rotation = MathHelper.PiOver2;
            Rotation2 = MathHelper.Pi;
            Projectile.timeLeft = 180;
        }
        public override void PostAI()
        {
            Projectile.ai[0]++;
            if (Projectile.ai[1] < 5 && !Projectile.DProj().Bool[0]&& Projectile.ai[0]>1)
            {
                if (Projectile.owner == Main.myPlayer)
                {
                    NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center- Projectile.velocity, Projectile.velocity, Type, (int)(Projectile.damage* (1 - Projectile.ai[1] / 5)), 0, Projectile.owner,0,Projectile.ai[1]+1);
                }
                Projectile.DProj().Bool[0] = true;
            }
            if(Projectile.damage<1)
            {
                Projectile.damage = 1;
            }
        }
        public override void OnKill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];
            if (Projectile.ai[1] == 0)
            {
                PlaySound(SoundID.Dig, Projectile.position);
                for (int i = 0; i < 20; i++)
                {
                    Vector2 projDirection = Main.rand.NextVector2Unit(Projectile.velocity.ToRotation(), Main.rand.NextFloat(-MathHelper.PiOver4, MathHelper.PiOver4)) * (Main.rand.NextFloat(2.8f, 3f) * (Projectile.velocity.Length() / 10));
                    NewDust(Projectile.position, 1, 1, 14, projDirection.X, projDirection.Y, 0, default, 1f);
                }
            }

        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            float RO = Projectile.rotation;
            SpriteEffects sprite = 0;
            Vector2 Origia = new Vector2(texture.Width * 0.5f, Projectile.height / 2);
            if (Projectile.velocity.X < 0)
            {
                sprite = SpriteEffects.FlipVertically;
                RO -= Rotation2;
                Origia = new Vector2(texture.Width * 0.5f, texture.Height - Projectile.height / 2);
            }

            if (Projectile.ai[1] == 0)
            {
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, lightColor, RO, Origia, Projectile.scale, sprite, 0f);
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, new Color(114, 0, 255, 50) * (1 - Projectile.ai[1] / 5), RO, Origia, Projectile.scale, sprite, 0f);
            }
            else
            {
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, new Color(114,0,255,50)*(1-Projectile.ai[1]/5), RO, Origia, Projectile.scale, sprite, 0f);
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, new Color(114,0,255, 50) * (1 - Projectile.ai[1] / 5), RO, Origia, Projectile.scale, sprite, 0f);
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, new Color(114,0,255, 50) * (1 - Projectile.ai[1] / 5), RO, Origia, Projectile.scale, sprite, 0f);
            }
            return false;
        }
    }
}