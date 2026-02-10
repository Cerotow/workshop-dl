using DDmod.Content.Projectiles.GeneralProj;
using DDmod.NoContent.Config;

namespace DDmod.Content.Projectiles.Magic.Staff
{
    public class 夜光蘑菇杖Proj : ModProjectile
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
            if (T1 <= 1.4F)
            {
                T1 += 0.1f;
                Projectile.ai[0] = 0;
            }
            else
            {
                SoundStyle sound = SoundID.Item29;
                sound.MaxInstances = 8;
                if (Projectile.soundDelay == 0)
                {
                    Projectile.soundDelay = 20;
                    sound.Volume = (Projectile.ai[0]/ player.ActiveItem().useAnimation)/3;
                    PlaySound(sound, Projectile.position);
                }
                if (Projectile.ai[0] >= player.ActiveItem().useAnimation)
                {
                    int A = player.ItemMana();
                    player.statMana -= A;
                    Vector2 vector2 = Projectile.velocity.PerfectNormalize();
                    float Rand = Main.rand.NextFloat(0.8f, 5f);
                    if ( Main.myPlayer == Projectile.owner)
                    {
                        int ProjType = ModContent.ProjectileType<蘑菇弹>();
                        Projectile projectile = Main.projectile[NewProjectileChange(Projectile.GetSource_FromThis(), Projectile.Center + Projectile.velocity.PerfectNormalize() * Projectile.SolidTileDistanceDetection(80, 50), vector2 * 8,ProjType , Projectile.damage, Projectile.knockBack, Projectile.owner,0, 0, 1)];
                        projectile.scale = 1f;
                        projectile.DamageType= DamageClass.Magic;
                        projectile.netUpdate = true;
                    }

                    sound.Pitch = 1;
                    sound.Volume = 1;
                    PlaySound(sound, Projectile.position);
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
            Color color = new Color(74, 189, 226);
            Vector2 vector2 = Projectile.Center - Main.screenPosition + Projectile.velocity.PerfectNormalize() * 18;
            Texture2D texture3 = DDTextures.VoidStar.Value;
            Main.spriteBatch.Draw(texture3, vector2, null, color, Projectile.rotation - MathHelper.PiOver4, texture3.Size() / 2, new Vector2(T1 / 6, T1 / 2.5f)*2, 0, 0f);

            Main.spriteBatch.Draw(texture3, Projectile.Center - Main.screenPosition + Projectile.velocity.PerfectNormalize() * Projectile.SolidTileDistanceDetection(80, 50), null, color*0.8F* (Projectile.ai[0] / player.ActiveItem().useAnimation), Projectile.rotation - MathHelper.PiOver4, texture3.Size() / 2, 0.5F, 0, 0f);
            Main.spriteBatch.Draw(texture3, Projectile.Center - Main.screenPosition + Projectile.velocity.PerfectNormalize() * Projectile.SolidTileDistanceDetection(80, 50), null, color*0.6F* (Projectile.ai[0] / player.ActiveItem().useAnimation), Projectile.rotation - MathHelper.PiOver4, texture3.Size() / 2, 0.5F, 0, 0f);

            texture = DDTextures.Circle[8].Value;
            color = new Color(74, 189, 226);
            DDHelper.Compression(texture, color, Projectile.rotation - MathHelper.PiOver4, Projectile.Opacity, new Vector2(2 + T1, 1), Projectile.direction, Projectile.DProj().Times[0], BlendState.AlphaBlend);
            Main.spriteBatch.Draw(texture, vector2, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), color, 0f, Utils.Size(texture) / 2, T1 / 2, 0, 0);
            Main.spriteBatch.Draw(texture, vector2, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), color, 0f, Utils.Size(texture) / 2, T1 / 2, 0, 0);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
    }
}