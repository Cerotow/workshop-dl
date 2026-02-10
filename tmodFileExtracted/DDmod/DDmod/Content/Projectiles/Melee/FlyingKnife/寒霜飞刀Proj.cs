using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Dusts;

namespace DDmod.Content.Projectiles.Melee.FlyingKnife
{
    public class 寒霜飞刀Proj : 飞刀Proj
    {
        public static Asset<Texture2D> Glow;
        public override void Load()
        {
            Glow = ModContent.Request<Texture2D>(Texture + "_Glow");
        }
        public override void Defaults()
        {
            AIStyle = 飞刀AI.AI1;
            Rotation = MathHelper.PiOver2;
            Rotation2 = MathHelper.Pi;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            Projectile.timeLeft = 330;
        }
        public override void PostAI()
        {
            if (Main.rand.NextBool(2))
            {
                NewDustChange2(2, Projectile.Center - new Vector2(4), Vector2.Zero, ModContent.DustType<冰雾>(), 0, 1, true, 0.2F,2,Main.rand.Next(-500,-120), new Color(92, 208, 252,155));
            }
        }
        public override void OnKill(int timeLeft)
        {
            PlaySound(SoundID.Dig, Projectile.position);
            Player player = Main.player[Projectile.owner];
            NewDustChange2(30, Projectile.Center - new Vector2(4), Vector2.Zero, ModContent.DustType<冰雾>(), 0, 2,true, 0.2F, 2, Main.rand.Next(-500, -120), new Color(92, 208, 252, 155));
            if (Projectile.owner == Main.myPlayer)
            {
                Vector2 position = new Vector2(player.Center.X - Main.rand.Next(600) * player.direction, player.Center.Y - 800);
                float SP = 12;
                Vector2 velocity = (Projectile.Center - position).PerfectNormalize() * SP;
                //NewProjectile(Projectile.GetSource_FromAI(), position, velocity, 9, Projectile.damage*2, 1, player.whoAmI, 0, Projectile.Center.Y);
            }
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            if (Main.rand.NextBool(2))
            {
                target.AddBuff(BuffID.Frostburn, 180);
            }
            if (Main.rand.NextBool(20))
            {
                target.AddBuff(ModContent.BuffType<Freeze>(), 180);
            }
            target.AddBuff(ModContent.BuffType<Frozen>(), 180);
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
                Vector2 vector2 = Projectile.oldPos[i] + Projectile.Size/2 - Main.screenPosition+Projectile.velocity.PerfectNormalize()*10;
                Color color = new Color(93, 168, 202, 0) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                
                Main.spriteBatch.Draw(Glow.Value, vector2, null, color, RO2, Origia2, Projectile.scale / 4*0.75F, sprite, 0f);
            }

            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, RO, Origia, Projectile.scale, sprite, 0f);

            return false;
        }
    }
}