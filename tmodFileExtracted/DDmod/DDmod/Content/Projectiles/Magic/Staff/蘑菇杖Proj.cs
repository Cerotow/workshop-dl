using DDmod.Content.Dusts;
using DDmod.Content.Items.Boss.蘑菇王;
using DDmod.Content.Projectiles.GeneralProj;
using DDmod.NoContent.Config;

namespace DDmod.Content.Projectiles.Magic.Staff
{
    public class 蘑菇杖Proj : ModProjectile
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
                SoundStyle sound = SoundID.NPCDeath1;
                sound.MaxInstances = 8;
                if (Projectile.ai[0] >= player.ActiveItem().useAnimation)
                {
                    int A = player.ItemMana();
                    player.statMana -= A;
                    Vector2 vector2 = Projectile.velocity.PerfectNormalize();
                    float Rand = Main.rand.NextFloat(0.8f, 5f);
                    if ( Main.myPlayer == Projectile.owner)
                    {
                        int ProjType = ModContent.ProjectileType<蘑菇>();
                        Projectile projectile = Main.projectile[NewProjectileChange(Projectile.GetSource_FromThis(), Projectile.Center + Projectile.velocity.PerfectNormalize() * Projectile.SolidTileDistanceDetection(80, 50), vector2 * 8,ProjType , Projectile.damage, Projectile.knockBack, Projectile.owner,2, 0, 1)];
                        projectile.scale = 1f;
                        projectile.DamageType= DamageClass.Magic;
                        projectile.netUpdate = true;
                    }
                    for (int r = 0; r < 30; r++)
                    {

                        Vector2 vector = Projectile.velocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-1, 1)) * Main.rand.NextFloat(4);
                        int  G =Dust.NewDust(Projectile.Center -new Vector2(4) + Projectile.velocity.PerfectNormalize() * 18, 1,1, ModContent.DustType<光球粒子>(), vector.X, vector.Y, 0, new Color(201, 105, 45, 0), Projectile.scale/2);
                        Main.dust[G].velocity = vector;
                        Main.dust[G].customData = 0.3F + Main.dust[G].DustAI(1);
                        if (r > 15)
                        {
                            vector = Projectile.velocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-1, 1)) * Main.rand.NextFloat(8);

                            G = Dust.NewDust(Projectile.Center - new Vector2(4) + Projectile.velocity.PerfectNormalize() * 18, 1, 1, ModContent.DustType<拉长粒子>(), vector.X, vector.Y, 0, new Color(201, 105, 45, 0), Projectile.scale*1.2F);
                            Main.dust[G].velocity = vector;
                        }
                    }
                    sound.Pitch = -0.4F;
                    sound.Volume = 0.1F;
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
            Color color = new Color(201, 105, 45, 0);
            Vector2 vector2 = Projectile.Center - Main.screenPosition + Projectile.velocity.PerfectNormalize() * 18;
            Texture2D texture3 = DDTextures.VoidStar.Value;
            Main.spriteBatch.Draw(texture3, vector2, null, color, Projectile.rotation - MathHelper.PiOver4, texture3.Size() / 2, new Vector2(T1 / 6, T1 / 4f)*2, 0, 0f);


            texture = DDTextures.Circle[8].Value;
            color = new Color(201, 105, 45, 0) * 0.3f;
            DDHelper.Compression(texture, color, Projectile.rotation - MathHelper.PiOver4, Projectile.Opacity, new Vector2(2 + T1, 1), Projectile.direction, Projectile.DProj().Times[0], BlendState.Additive);
            Main.spriteBatch.Draw(texture, vector2, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), color, 0f, Utils.Size(texture) / 2, T1 / 2, 0, 0);
            Main.spriteBatch.Draw(texture, vector2, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), color, 0f, Utils.Size(texture) / 2, T1 / 2, 0, 0);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
    }
}