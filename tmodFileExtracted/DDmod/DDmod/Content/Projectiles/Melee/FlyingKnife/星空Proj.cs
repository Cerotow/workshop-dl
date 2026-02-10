using DDmod.Content.Dusts;

namespace DDmod.Content.Projectiles.Melee.FlyingKnife
{
    public class 星空Proj : 飞刀Proj
    {
        public static Asset<Texture2D> Glow;
        public override void Load()
        {
            Glow = ModContent.Request<Texture2D>(Texture + "_Glow");
        }
        public override void Defaults()
        {
            AIStyle = 飞刀AI.AI3;
            Rotation = MathHelper.PiOver2;
            Rotation2 = MathHelper.Pi;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            Projectile.timeLeft = 60;
        }
        public override void PostAI()
        {
        }
        public override void OnKill(int timeLeft)
        {
            PlaySound(SoundID.Dig, Projectile.position);
            Player player = Main.player[Projectile.owner];
            NewDustChange2(50, Projectile.Center, Vector2.Zero, ModContent.DustType<天堂粒子>(), 0, 10,true,0.3F,1.1F,Alpha:100);
            NewDustChange4(20, Projectile.Center, Vector2.Zero, ModContent.DustType<星光粒子>(), 0, 8,true,0.6F,2F,100,0,new Color(255, 153, 183,0));
            if (Projectile.owner == Main.myPlayer)
            {
                Vector2 position = new Vector2(player.Center.X - Main.rand.Next(600) * player.direction, player.Center.Y - 800);
                float SP = 12;
                Vector2 velocity = (Projectile.Center - position).PerfectNormalize() * SP;
                NewProjectile(Projectile.GetSource_FromAI(), position, velocity*3, 9, Projectile.damage, 1, player.whoAmI, 0, Projectile.Center.Y,-1);
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
               Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            float RO = Projectile.rotation;
            SpriteEffects sprite = 0;
            Vector2 Origia = new Vector2(texture.Width * 0.5f, texture.Height *0.25F);
            if (Projectile.velocity.X < 0)
            {
                sprite = SpriteEffects.FlipVertically;
                RO -= Rotation2;
                Origia = new Vector2(texture.Width * 0.5f, texture.Height * 0.75F);
            }
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 Origia2 = new Vector2(Glow.Width() * 0.5f, Glow.Height() * 0.25F);
                float RO2 = Projectile.oldRot[i];
                if (Projectile.velocity.X < 0)
                {
                    RO2 -= Rotation2;
                    Origia2 = new Vector2(Glow.Width() * 0.5f, Glow.Height() * 0.75F);
                }
                Vector2 vector2 = Projectile.oldPos[i] + Projectile.Size/2 - Main.screenPosition;
                Color color = new Color(237, 63, 133, 0) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                Color color2 = new Color(237, 63, 133, 0).Opposite() * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                
                Main.spriteBatch.Draw(Glow.Value, vector2, null, color, RO2, Origia2, Projectile.scale / 4*0.75F, sprite, 0f);
                Main.spriteBatch.Draw(Glow.Value, vector2, null, color2, RO2, Origia2, Projectile.scale / 8 * 0.75F, sprite, 0f);
                Main.spriteBatch.Draw(Glow.Value, vector2, null, color, RO2, Origia2, Projectile.scale / 4*0.75F, sprite, 0f);
                Main.spriteBatch.Draw(Glow.Value, vector2, null, color2, RO2, Origia2, Projectile.scale / 8 * 0.75F, sprite, 0f);
            }
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, new Color(255, 255, 255, 160), RO, Origia, Projectile.scale, sprite, 0f);
            return false;
        }
    }
}