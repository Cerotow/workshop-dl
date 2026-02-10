using DDmod.Content.Dusts;

namespace DDmod.Content.Projectiles.Magic.Gun
{
    public class 星辰炮Proj : ModProjectile
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

        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            float Value = 0;
            if (player.statMana >= player.ItemMana() && Main.myPlayer == Projectile.owner)
            {
                Projectile.ai[2]++;
                Value = 1;
            }
            Vector2 vector = player.Dplayer().MouseWorld;
            if ((vector - Projectile.Center).Length() < 100)
            {
                vector += (vector - player.Center).PerfectNormalize() * 100;
            }
            float targetRotation = (vector - player.Center).ToRotation();

            player.ChangeDir((vector.X - player.Center.X) >= 0 ? 1 : -1);

            Projectile.rotation = targetRotation;

            Vector2 muzzleOffset = new Vector2(0, -10 * player.direction).RotatedBy(Projectile.rotation);
            Projectile.DProj().Times[3] = (vector - Projectile.Center).SafeNormalize(Vector2.UnitX).ToRotation();
            Projectile.HoldProj(player, 30, 0, Projectile.localAI[2] == 0 ? Projectile.DProj().Times[3].ToRotationVector2() : Projectile.velocity, 0, 0, Projectile.localAI[2] < 15 && Value == 1, Value, false,false);
            player.ChangeDir((vector.X - player.Center.X) >= 0 ? 1 : -1);
            Projectile.position += muzzleOffset;

            bool channeling = player.channel && !player.noItems && !player.CCed && !player.dead && player.Dplayer().ForbiddenToAttack == 0;
            if (channeling || Projectile.ai[0] / player.IteUseAnimation() < (1F/ player.ActiveItem().MagicItem().charging))
            {
                if (Projectile.ai[1] >= player.ActiveItem().MagicItem().charging)
                {
                    if (Projectile.soundDelay <= 10)
                    {
                        Projectile.soundDelay = 1145141919;
                        SoundStyle sound = SoundID.Item29;
                        sound.Pitch = -0.3f;
                        PlaySound(sound, Projectile.position);
                        Projectile.localAI[0]++;
                    }
                }
                else
                {
                    if (Projectile.soundDelay == 0 && Value == 1)
                    {
                        Projectile.soundDelay = 5;
                        SoundStyle sound = SoundID.Item29;
                        sound.MaxInstances = 15;
                        sound.Volume = Projectile.ai[1]*2;
                        PlaySound(sound, Projectile.position);
                    }
                    if (Projectile.ai[2] >= player.IteUseAnimation() / player.ActiveItem().MagicItem().ExtraMana)
                    {
                        int A = player.ItemMana();
                        player.statMana -= A;
                        Projectile.ai[2] -= player.IteUseAnimation() / player.ActiveItem().MagicItem().ExtraMana;
                    }
                    if (Value == 1)
                    {
                        for (int A = 0; A < Projectile.ai[1]+1; A++)
                        {
                            vector = Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.25F, 0.25F) * Projectile.ai[1]) * 6;
                            int DU = NewDust(Projectile.PreviousCenter() - player.velocity - new Vector2(4) + vector * (12 + (3 * Projectile.ai[1])), 0, 0, ModContent.DustType<速度粒子>(), 0, 0, 255, new Color(0, 100, 255, 0), 1.2F);
                            Main.dust[DU].customData = 1F;
                            Main.dust[DU].noGravity = false;
                            Main.dust[DU].velocity = -vector * 0.6F;
                            Main.dust[DU].rotation = Main.dust[DU].velocity.ToRotation();
                            GlobalDust.DustProjectileOwner[DU] = Projectile.whoAmI;
                        }
                    }
                }

                DDHelper.MaxandMinF(ref Projectile.ai[0], player.IteUseAnimation(), 0);
                Projectile.ai[1] = Projectile.ai[0] / player.IteUseAnimation() * player.ActiveItem().MagicItem().charging;

                Projectile.DProj().Times[2] = Projectile.velocity.ToRotation();
                Projectile.DProj().vector[1] = Projectile.Center + Projectile.DProj().Times[2].ToRotationVector2() * (35 + (10 * Projectile.ai[1]));

            }
            else
            {
                Projectile.damage = (int)(player.GetWeaponDamage(player.HeldItem) * (Projectile.ai[1]));
                if (Projectile.localAI[2] == 0)
                {
                    Projectile.localAI[1] = Projectile.ai[1];
                    if (Main.myPlayer == Projectile.owner)
                    {
                        NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.velocity * 8, ModContent.ProjectileType<魔力巨星>(), Projectile.damage, Projectile.knockBack, -1, Projectile.ai[1]);
                    }
                    SoundStyle sound = SoundID.Item14;
                    sound.Volume = Projectile.ai[1];
                    sound.Pitch = -0.8F;
                    PlaySound(sound, Projectile.position);
                    Projectile.ai[1] *= 3F;
                    player.velocity -= Projectile.velocity * Projectile.ai[1];

                    float A = Projectile.ai[1];
                    for (int a = 0; a < 35; a++)
                    {
                        int dust = NewDust(Projectile.Center + Projectile.DProj().Times[2].ToRotationVector2() * (55) - new Vector2(4), 1, 1, ModContent.DustType<Dusts.速度粒子>(), 0, 0, 0, new Color(0, 100, 255, 0), A);
                        Main.dust[dust].velocity = Projectile.oldVelocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)) * Main.rand.NextFloat(1, 4) * A;
                        Main.dust[dust].rotation = Main.dust[dust].velocity.ToRotation();
                        Main.dust[dust].noGravity = true;
                        Main.dust[dust].customData = Projectile.ai[1]/2;
                    }
                }
                if (Projectile.ai[1] > 0)
                {
                    Projectile.ai[1] *= 0.85F;
                }
                Projectile.localAI[2]++;
                Projectile.velocity = Projectile.velocity.RotatedBy(Projectile.ai[1] / 30*-player.direction);
            }
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
            if(Projectile.localAI[2]>15)
            {
                return false;

            }
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            if (Main.player[Projectile.owner].direction == 1)
            {
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, texture.Size() / 2, Projectile.scale, 0, 0f);
            }
            else
            {
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation + MathHelper.Pi, texture.Size() / 2, Projectile.scale, (SpriteEffects)1, 0f);
            }
            float ro = 0;
            Vector2 vector = Projectile.DProj().vector[1] - Main.screenPosition;
            Projectile.DProj().Times[0] += 0.1f;
            Texture2D texture2 = DDTextures.Circle[9].Value;
            Color color2 = Projectile.GetAlpha(new Color(0, 100, 255, 0)) * 0.66F;
            texture = DDTextures.VoidStar.Value;
            Color color = Projectile.GetAlpha(new Color(0, 100, 255, 0));
            float RO = Projectile.DProj().Times[2];
            Main.spriteBatch.Draw(texture, vector, null, color, RO, texture.Size() / 2, new Vector2(Projectile.scale / 4, Projectile.scale) * Projectile.ai[1]/2, 0, 0f);

            DDHelper.Compression(texture2, color, RO, Projectile.Opacity, new Vector2(6, 1), Projectile.direction, Projectile.DProj().Times[0], BlendState.Additive);

            Main.EntitySpriteDraw(texture2, vector, new Rectangle?(new Rectangle(0, 0, texture2.Width, texture2.Height)), color2, 0f, Utils.Size(texture2) * 0.5f, Projectile.ai[1] / 8, 0, 0);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }
    }
    
    public class 魔力巨星 : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 58;
            Projectile.height = 58;
            Projectile.friendly = true;
            Projectile.aiStyle = -1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = true;
            Projectile.penetrate = -1;
            Projectile.alpha = 255;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.extraUpdates = 5;

        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = height = 58;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }
        public override void AI()
        {
            if (Projectile.ai[1]++ > 8)
            {
                Dust dust = Main.dust[NewDust(Projectile.Center - new Vector2(4)-Projectile.velocity* Projectile.ai[0]*2, 0, 0, ModContent.DustType<速度粒子>(), 0, 0, 0, new Color(0, 100, 255, 0), Projectile.ai[0]*4)];
                dust.velocity = Vector2.Zero;
                dust.rotation = Projectile.velocity.ToRotation();
                dust.customData = Projectile.ai[0]*3;
            }
            if(Projectile.velocity.X>0)
            {
                Projectile.rotation += 0.1F;
            }
            else
            {

                Projectile.rotation -= 0.1F;
            }
            Projectile.DProj().Magnification = Projectile.ai[0];
            Projectile.scale = Projectile.ai[0];
            Projectile.ProjScaleChange();
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Projectile.damage = (int)(Projectile.damage * 0.8F);
            float A = Projectile.ai[0] * 3;
            for (int a = 0; a < 35; a++)
            {
                int dust = NewDust(Projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<Dusts.速度粒子>(), 0, 0, 0, new Color(0, 100, 255, 0), A);
                Main.dust[dust].velocity = Projectile.oldVelocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)) * Main.rand.NextFloat(2, 4) * A;
                Main.dust[dust].rotation = Main.dust[dust].velocity.ToRotation();
                Main.dust[dust].noGravity = true;
                Main.dust[dust].customData = Projectile.ai[0] * 1;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            float ro = Projectile.rotation;
            Vector2 vector = Projectile.Center - Main.screenPosition;

            Main.spriteBatch.Draw(texture, vector, null, new Color(180, 180, 180, 255), ro, texture.Size() / 2,Projectile.ai[0]/4, 0, 0);
            Main.spriteBatch.Draw(texture, vector, null, new Color(0, 100, 255, 0), ro, texture.Size() / 2,Projectile.ai[0]/4, 0, 0);

           // Main.spriteBatch.Draw(DDTextures.WhitePng.Value, Projectile.position - Main.screenPosition, null, Color.White, 0,Vector2.Zero,Projectile.Size/2, 0, 0);

            return false;
        }
        public override void OnKill(int timeLeft)
        {
            NewDustChange4((int)(20 * Projectile.scale+5), Projectile.Center - new Vector2(4), Vector2.Zero, ModContent.DustType<速度粒子>(), 3 * Projectile.scale, 10 * Projectile.scale, true, 3, 6, 0,1000, new Color(0, 100, 255, 0),3);
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                NewDustChange2(2, Projectile.oldPos[i] + Projectile.Size / 2 - new Vector2(4), Vector2.Zero, ModContent.DustType<光球粒子>(), 0, 1, true, Projectile.scale / 2, Projectile.scale, 0, new Color(0, 100, 255, 0) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length));

            }
        }
    }
}