using DDmod.NoContent.Config;

namespace DDmod.Content.Projectiles.Magic.Staff
{
    public class 狱火杖Proj : ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
            //projectile.light = 0.50f;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 2;
            Projectile.coldDamage = true;
        }
        public float T1 = 0;
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.HoldProj(player, 24, 0, Vector2.Zero, MathHelper.PiOver4);
            if (player.statMana <= 0 && Main.myPlayer == Projectile.owner)
            {
                Projectile.Kill();
            }
            if (T1 <= 2.7F)
            {
                T1 += 0.033f;
                Projectile.ai[0] = 0;
            }
            else
            {
                if (Projectile.ai[0] >= player.ActiveItem().useAnimation)
                {
                    int A = player.ItemMana();
                    player.statMana -= A;
                    Vector2 vector2 = Projectile.velocity.PerfectNormalize();
                    float Rand = Main.rand.NextFloat(0.8f, 5f);
                    if ( Main.myPlayer == Projectile.owner)
                    {
                        Projectile projectile = Main.projectile[NewProjectileChange(Projectile.GetSource_FromThis(), Projectile.Center + Projectile.velocity.PerfectNormalize() * Projectile.SolidTileDistanceDetection(80, 50), vector2 * 8, ModContent.ProjectileType<魔法狱火小蛇>(), Projectile.damage, Projectile.knockBack, Projectile.owner,0, 0, 1)];
                        projectile.scale = 1f;
                        projectile.netUpdate = true;
                    }
                    PlaySound(SoundID.Item29, Projectile.position);

                    Projectile.ai[0] -= player.ActiveItem().useAnimation;
                }
            }
            int damageWithChargeAndStats = player.GetWeaponDamage(player.HeldItem);
            Projectile.damage = damageWithChargeAndStats;
            return false;
        }
        public override void OnKill(int timeLeft)
        {
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.DProj().Times[0] += 0.022f;
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), lightColor, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), Projectile.scale, 0, 0f);
            Player player = Main.player[Projectile.owner];
            Color color = new Color(253, 62, 3, 0);
            Vector2 vector2 = Projectile.Center - Main.screenPosition + Projectile.velocity.PerfectNormalize() * 26;
            Texture2D texture3 = DDTextures.VoidStar.Value;
            Main.spriteBatch.Draw(texture3, vector2, null, color*(1.7F-Lighting.GetColor((int)Projectile.Center.X / 16, (int)Projectile.Center.Y / 16).ToVector3().Length()), Projectile.rotation - MathHelper.PiOver4, texture3.Size() / 2, new Vector2(T1 / 6, T1 / 2.5f), 0, 0f);
            texture = DDTextures.Circle[9].Value;
            DDHelper.Compression(texture, color, Projectile.rotation - MathHelper.PiOver4, Projectile.Opacity, new Vector2(2 + T1, 1), Projectile.direction, Projectile.DProj().Times[0], BlendState.Additive);
            Main.spriteBatch.Draw(texture, vector2, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), color, 0f, Utils.Size(texture) / 2, T1 / 3/4, 0, 0);
            Main.spriteBatch.Draw(texture, vector2, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), color, 0f, Utils.Size(texture) / 2, T1 / 3/4, 0, 0);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
    }
}