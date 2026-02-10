using DDmod.Content.Projectiles.Summon.Minions.MinionBuff;

namespace DDmod.Content.Projectiles.Summon.Minions
{
    public class ShadowImp : Summons
    {
        public override void SetDefault()
        {
            Defaults(ModContent.BuffType<ShadowImpBuff>(), 12, 41, 800, ModContent.ProjectileType<ShadowFireBall>(), 15, 20, 300);
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.minionSlots = 1;
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            Main.projFrames[Projectile.type] = 6;
            /// <summary> 感觉有点针对召唤师了 </summary> ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }
        public override bool MinionContactDamage()
        {
            return false;
        }
        public override void Visual()
        {
            Projectile.rotation = Projectile.velocity.X * 0.03f;
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 6)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= 6)
            {
                Projectile.frame = 0;
            }
            //Lighting.AddLight(Projectile.Center, new Vector3(20, 255, 20) * (0.003F * Projectile.ai[0] / 50));
            Projectile.DProj().Times[1]++;
            if (Projectile.DProj().Times[1] % 5 == 0)
            {
                for (int A = 0; A < 1; A++)
                {
                    Dust dust = Main.dust[NewDust(Projectile.Center - new Vector2(Projectile.width / 2, -Projectile.height / 2), Projectile.width / 2, 1, 27)];
                    dust.scale = 0.8f;
                    dust.velocity = (Projectile.rotation + MathHelper.PiOver2).ToRotationVector2().RotatedBy(Main.rand.NextFloat(-1.5F, 1.5F)) * 2;
                }
            }
            if (!target)
            {
                if (Projectile.velocity.X > 0)
                {
                    Projectile.spriteDirection = 1;
                }
                else
                {
                    Projectile.spriteDirection = 0;
                }
            }
            else
            {
                Vector2 direction = npc.Center - Projectile.Center;
                if (direction.X > 0)
                {
                    Projectile.spriteDirection = 1;
                }
                else
                {
                    Projectile.spriteDirection = 0;
                }
            }
        }
        public override bool AttackAI()
        {
            
            if (!target)
            {
                if (Projectile.ai[0] < AttackSpeed)
                {
                    Projectile.ai[0]++;
                }
                if (Projectile.localAI[0] > 0F)
                {
                    Projectile.localAI[0] -= 0.01F;
                }
            }
            else
            {
                if (Projectile.localAI[0] < 0.25F)
                {
                    Projectile.localAI[0] += 0.01F;
                }
                if (Projectile.DProj().Times[1] % 5 == 0)
                {
                    for (int A = 0; A < 5; A++)
                    {
                        Dust dust = Main.dust[NewDust(Projectile.Center - new Vector2(0,40), 1, 1, 27)];
                        dust.scale = 1.2f;
                        dust.noGravity = true;
                        dust.velocity = (Projectile.rotation + MathHelper.PiOver2).ToRotationVector2().RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)) * 4;
                    }
                }
                Projectile.ai[0]++;
                Vector2 direction = npc.Center - Projectile.Center+ new Vector2(0, 40);
                DistanceNPC = direction.Length();
                direction = direction.PerfectNormalize();
                if (Projectile.ai[0] >= AttackSpeed)
                {
                    if (Projectile.owner == Main.myPlayer)
                    {
                        Main.projectile[NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center-new Vector2(0,40), direction * shootSpeed, shoot, Projectile.damage, Projectile.knockBack, Projectile.owner)].originalDamage = Projectile.originalDamage;
                    }
                    Projectile.netUpdate = true;
                    Projectile.ai[0] = 0;
                }
            }
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.localAI[1] += 0.1f;
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Texture2D texture2 = DDTextures.Circle[9].Value;
            Vector2 vector = Projectile.Size / 2;

            Rectangle? rectangle = new Rectangle?(new Rectangle(0, texture.Height / Main.projFrames[Projectile.type] * Projectile.frame, texture.Width / 2, texture.Height / Main.projFrames[Projectile.type]));
            if (target)
            {
                rectangle = new Rectangle?(new Rectangle(texture.Width / 2, texture.Height / Main.projFrames[Projectile.type] * Projectile.frame, texture.Width / 2, texture.Height / Main.projFrames[Projectile.type]));
            }
            Main.EntitySpriteDraw(texture2, Projectile.position - Main.screenPosition + vector, null, new Color(81, 6, 233,0), Projectile.localAI[1], texture2.Size()/2, Projectile.localAI[0], (SpriteEffects)Projectile.spriteDirection, 0);
            Main.EntitySpriteDraw(texture2, Projectile.position - Main.screenPosition + vector, null, new Color(81, 6, 233,0), Projectile.localAI[1], texture2.Size()/2, Projectile.localAI[0], (SpriteEffects)Projectile.spriteDirection, 0);
            Main.EntitySpriteDraw(texture, Projectile.position - Main.screenPosition + vector, rectangle, Color.White, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / Main.projFrames[Projectile.type]) / 2, Projectile.scale, (SpriteEffects)Projectile.spriteDirection, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
    }
}