using Terraria;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Melee.Sword
{
    public class Muramasa : ModProjectile
    {
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 15;
            Projectile.width = 20;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ignoreWater = true;
            //projectile.light = 0.50f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.ownerHitCheck = true;
            Projectile.MeleeProj().SwordHitbox = true;
            Projectile.MeleeProj().oldVels = new Vector2[Projectile.oldPos.Length];
            Projectile.MeleeProj().oldVels2 = 44;
            Projectile.extraUpdates = 9;
            Projectile.MeleeProj().oldVels3 = 0.1F;
            Projectile.stopsDealingDamageAfterPenetrateHits = true;
        }
        int proj = 0;
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(proj);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            proj = reader.ReadInt32();
        }
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];

            if (Projectile.scale < player.GetAdjustedItemScale(player.ActiveItem()))
            {
                Projectile.scale = player.GetAdjustedItemScale(player.ActiveItem());
            }
            Projectile.Resize((int)(Projectile.OriginalWidth() * Projectile.scale), (int)(Projectile.OriginalHeight() * Projectile.scale));

            Projectile.HoldProj(player, 40 * Projectile.scale, Projectile.ai[0], Projectile.DProj().vector[0], MathHelper.PiOver4, 0, true, 0.05f * Projectile.DProj().Times[0], false, false);

            Projectile.HoldSword2(player, -player.HeldItem.useAnimation * (Projectile.extraUpdates+1), -1, 2.4f, false);

            if (Projectile.localAI[1] <= 0 || Projectile.MeleeProj().DelayedKill > 0)
            {
                proj = 0;
            }
            else
            {
                if (proj == 0)
                {
                    PlaySound(SoundID.Item1, Projectile.position);
                    proj++;
                    if (Projectile.owner == Main.myPlayer)
                    {
                        Projectile.DProj().MouseWorld = Main.MouseWorld;
                        Projectile.netUpdate = true;

                        Vector2 vector = Main.rand.NextVector2Unit() * 60;
                        NewProjectile(Projectile.GetSource_FromThis(), Projectile.DProj().MouseWorld + vector, -vector / 10, ModContent.ProjectileType<MuramasaSlash>(), Projectile.damage / 2, Projectile.knockBack / 2, Projectile.owner);
                    }
                }
            }
            if (Projectile.localAI[1] >= 0)
            {
                for (int A = -Projectile.height; A < Projectile.height / 2; A += (int)(10 * Projectile.scale))
                {
                    if (Main.rand.NextBool(20))
                    {
                        int Type = 172;
                        Dust dust = Main.dust[NewDust(Projectile.Center + Projectile.velocity.PerfectNormalize() * (A + 10 * Projectile.scale), Projectile.height / 4, Projectile.height / 4, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                        dust.noGravity = true;
                        dust.velocity =Projectile.velocity.PerfectNormalize()*Main.rand.NextFloat(0.5F,3);
                        dust.scale = 1.3f;
                    }
                }
            }
            Projectile.spriteDirection = Projectile.DProj().Times[0] > 0 ? 0 : 1;
            return false;
        }
        public override void OnKill(int timeLeft)
        {
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            for (int A = 0; A < 30; A++)
            {
                int Type = 172;
                Dust dust = Main.dust[NewDust(target.Center, 1, 1, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                dust.noGravity = true;
                dust.scale = 1.5F;
                dust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(2, 5);
            }
            Vector2 vector = Main.rand.NextVector2Unit() * 60;
            NewProjectile(Projectile.GetSource_FromThis(), target.Center + vector, -vector / 10, ModContent.ProjectileType<MuramasaSlash>(), Projectile.damage / 2, Projectile.knockBack / 2, Projectile.owner, target.whoAmI, 1);
        }
        Color color = new Color(0, 50, 255, 255)*0.5F;
        Color color2 = new Color(56, 78, 210, 0);
        public float TWidth()
        {
            return 28*Projectile.scale;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.ai[1] < 2)
            {
                Projectile.ai[1]++;
                return false;
            }
            Vector2 vector = Projectile.Player().ArmCenter();
            if (Projectile.MeleeProj().oldPlayer != Vector2.Zero)
            {
                vector = Projectile.MeleeProj().oldPlayer;
            }
            DDHelper.BladeTrail(DDTextures.WhitePng, color, 1F, Projectile.DProj().Times[0] > 0);
            TrailDrawer.Draw(Projectile.MeleeProj().oldVels, vector - Main.screenPosition, 88, null, TWidth());
            DDHelper.BladeTrail(DDTextures.Wave, color, 1F, Projectile.DProj().Times[0] > 0);
            TrailDrawer.Draw(Projectile.MeleeProj().oldVels, vector - Main.screenPosition, 88, null, TWidth());
            Main.spriteBatch.End();
            Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            Texture2D texture = TextureAssets.Item[155].Value;

            Vector2 Center = Projectile.Center - Main.screenPosition;

            if (Projectile.MeleeProj().DelayedKill <= 0)
            {
                if (Projectile.spriteDirection == 0)
                {
                    Main.spriteBatch.Draw(texture, Center, null, Color.White, Projectile.rotation, texture.Size() / 2, Projectile.scale, (SpriteEffects)Projectile.spriteDirection, 0f);
                }
                else
                {
                    Main.spriteBatch.Draw(texture, Center, null, Color.White, Projectile.rotation + MathHelper.PiOver2, texture.Size() / 2, Projectile.scale, (SpriteEffects)Projectile.spriteDirection, 0f);
                }
            }
            else
            {
                Projectile.alpha += 10;
            }
            return false;
        }
    }
    public class MuramasaSlash : ModProjectile
    {
        public override string Texture => "DDmod/Image/VoidStar";
        public override void SetStaticDefaults()
        {
           //DisplayName.SetDefault("Muramasa Slash");
           //DisplayName.AddTranslation(7, "村正斩");
        }
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            Projectile.width = 8;
            Projectile.height = 90;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 1;
            Projectile.timeLeft = 20;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.scale = 1.4f;
            Projectile.ArmorPenetration = 100000;
        }
        public override bool? CanDamage()
        {
            return true;
        }
        public override bool? CanHitNPC(NPC target)
        {
            if (Projectile.ai[1] != 0)
            {
                return target.whoAmI == (int)Projectile.ai[0];
            }
            return null;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();

            
            for (int A = -Projectile.height / 2; A < Projectile.height / 2; A += (int)(10 * Projectile.scale))
            {
                if (Main.rand.NextBool(10))
                {
                    int Type = 172;
                    Dust dust = Main.dust[NewDust(Projectile.Center- new Vector2(Projectile.height) / 16 + Projectile.velocity.PerfectNormalize() * (A + 10 * Projectile.scale), Projectile.height/8, Projectile.height / 8, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                    dust.noGravity = true;
                    dust.velocity = (Projectile.rotation - MathHelper.PiOver4).ToRotationVector2();
                    dust.scale = 1f;
                }
            }
        }
        public override void OnKill(int timeLeft)
        {

        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {

        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(255, 255, 255, Projectile.alpha));
        }
        int Time;
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects spriteEffects = 0;
            Texture2D texture = DDTextures.Starlight3.Value;
            Color color = new Color(0, 55, 255, 255);
            Vector2 vector = new Vector2(Projectile.width, Projectile.height) / 2;

            for (int a = 0; a < 3; a++)
            {
                color.A = 255;
                Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, color, Projectile.rotation + MathHelper.PiOver2, texture.Size() / 2, new Vector2(Projectile.scale/8, Projectile.scale*2) * 1.5f, spriteEffects, 0f);

                color.A = 0;
                Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, color.Opposite()*0.5F, Projectile.rotation+MathHelper.PiOver2, texture.Size() / 2, new Vector2(Projectile.scale/8, Projectile.scale*2), spriteEffects, 0f);
            }
            return false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float num2 = 0.75f;
            float LaserLength = MathHelper.Lerp(0, Projectile.height, num2);
            Vector2 Pvelocity = Utils.RotatedBy(Projectile.velocity.PerfectNormalize(), 0, default);
            float num = 0f;
            bool T = false;
            if(Collision.CheckAABBvLineCollision(Utils.TopLeft(targetHitbox), Utils.Size(targetHitbox), Projectile.Center, Projectile.Center + Pvelocity * LaserLength, projHitbox.Width, ref num))
            {
                T = true;
            }
            if(Collision.CheckAABBvLineCollision(Utils.TopLeft(targetHitbox), Utils.Size(targetHitbox), Projectile.Center, Projectile.Center - Pvelocity * LaserLength, projHitbox.Width, ref num))
            {
                T = true;
            }
            return new bool?(T);

        }
    }
}
