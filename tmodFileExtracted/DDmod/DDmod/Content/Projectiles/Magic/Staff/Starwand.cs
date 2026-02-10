
namespace DDmod.Content.Projectiles.Magic.Staff
{
    public class Starwand : ModProjectile
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
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];

            Vector2 vector = player.RotatedRelativePoint(player.ArmCenter(), reverseRotation: false, addGfxOffY: false);
            if (player.Dplayer().MouseWorld.X - vector.X > 0)
            {
                Projectile.DProj().vector[0] = new Vector2(0.01F, -1);
            }
            else
            {
                Projectile.DProj().vector[0] = new Vector2(-0.01F, -1);
            }
            Projectile.HoldProj(player, 32, 0, Projectile.DProj().vector[0], MathHelper.PiOver4,direction:true);

            if (player.statMana <= 0 && Main.myPlayer == Projectile.owner)
            {
                Projectile.Kill();
            }
            if (T1 <= 0.7F)
            {
                T1 += 0.033f;
            }
            else
            {
                if (Projectile.ai[0] >= player.ActiveItem().useAnimation)
                {
                    if (Projectile.owner == Main.myPlayer)
                    {
                        int A = player.ItemMana();
                        player.statMana -= A;
                        Vector2 vector2 = Projectile.velocity.PerfectNormalize();
                        float Next = Main.rand.NextFloat(-20, 20);
                        Projectile projectile = Main.projectile[NewProjectileChange(Projectile.GetSource_FromThis(), Projectile.Center+new Vector2(0,20) + Projectile.velocity.PerfectNormalize() * Projectile.SolidTileDistanceDetection(100, 50) + new Vector2(Next, 0), vector2 * 8 + new Vector2(Next / 15, 0), ModContent.ProjectileType<MagicStar>(), Projectile.damage, Projectile.knockBack, Projectile.owner, vector2.Y * 8, 0, 1)];
                        projectile.scale = 1;
                    }
                    SoundStyle sound = SoundID.Item29; sound.Volume = 0.5F; PlaySound(sound, Projectile.position);

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
        public float T1 = 0;
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.DProj().Times[0] += 0.22f;
            Color color = Projectile.GetAlpha(new Color(0, 100, 255, 155));

            Texture2D texture2 = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Main.spriteBatch.Draw(texture2, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, texture2.Width, texture2.Height)), lightColor, Projectile.rotation, new Vector2(texture2.Width / 2, texture2.Height / 2), Projectile.scale, 0, 0f);

            Texture2D texture = DDTextures.Circle[9].Value;

            Vector2 vector2 = Projectile.Center - Main.screenPosition + Projectile.velocity.PerfectNormalize() * 26;

            Texture2D texture3 = DDTextures.VoidStar.Value;
            Main.spriteBatch.Draw(texture3, vector2, null, color, Projectile.rotation - MathHelper.PiOver4, texture3.Size() / 2, new Vector2(T1 / 3, T1)*1.1f, 0, 0f);

            DDHelper.Compression(texture, color, Projectile.rotation - MathHelper.PiOver4, Projectile.Opacity, new Vector2(3 + T1, 1), Projectile.direction, Projectile.DProj().Times[0], BlendState.Additive);

            Main.spriteBatch.Draw(texture, vector2, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), color, 0f, Utils.Size(texture) / 2, T1/4, 0, 0);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            //Main.spriteBatch.Draw(texture3, vector2, null, color, Projectile.rotation - MathHelper.PiOver4, texture3.Size() / 2, new Vector2(T1 / 2, Projectile.scale) / 1.5f, 0, 0f);
            return false;
        }
    }
}