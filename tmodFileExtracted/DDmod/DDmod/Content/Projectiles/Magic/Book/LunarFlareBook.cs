using DDmod.Content.Dusts;

namespace DDmod.Content.Projectiles.Magic.Book
{
    public class LunarFlareBook : ModProjectile
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
            //Projectile.light = 0.50f;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 2;
            Projectile.coldDamage = true;
            Main.projFrames[Projectile.type] = 12;
        }
        public override bool PreAI()
        {
            Player player = Projectile.Player();
            Projectile.HoldProj(player, Projectile.DProj().Times[1], 0, new Vector2(player.direction, 0), 0, 0, true, Projectile.DProj().Times[4] >= 1f ? 1 : 0, false);
            bool channeling = player.channel && !player.noItems && !player.CCed && !player.dead && player.Dplayer().ForbiddenToAttack == 0;
            //控制书的距离
            if (Projectile.DProj().Times[1] < Projectile.SolidTileDistanceDetection(50, 15) && channeling)
            {
                Projectile.DProj().Times[1]++;
            }
            else
            {
                Projectile.DProj().Times[1]--;
            }
            //如果翻转
            if (Projectile.DProj().Times[2] != player.direction)
            {
                Projectile.DProj().Times[1] *= -1;
                Projectile.DProj().Times[2] = player.direction;
            }
            //上下浮动
            DDHelper.BackAndForth(3, -3, 0.1F,ref Projectile.DProj().Times[3],ref Projectile.DProj().Bool[0]);
            Projectile.position.Y -= Projectile.DProj().Times[3];
            //如果左键一直按着
            if (channeling)
            {
                if (player.statMana >= player.ItemMana())
                {
                    //如果书出来了
                    if (Projectile.DProj().Times[4] <= 1F)
                    {
                        Projectile.DProj().Times[4] += 0.03f;
                    }
                    else
                    {
                        //消耗魔力时的帧图
                        Projectile.frameCounter++;
                        if (Projectile.frameCounter >= player.HeldItem.useAnimation / 5)
                        {
                            Projectile.frameCounter = 0;
                            Projectile.frame++;
                        }
                        if (Projectile.frame > 9)
                        {
                            Projectile.frame = 2;
                        }
                        if (Projectile.frame >= 2 && Projectile.frame <= 9&& Main.myPlayer == Projectile.owner)
                        {
                            Projectile.DProj().Bool[1] = true;
                            Projectile.netUpdate = true;
                        }
                    }
                }
                else
                {
                    //没蓝时候的帧图
                    Projectile.DProj().Bool[1] = false;
                    Projectile.netUpdate = true;
                    Projectile.frameCounter++;
                    if (Projectile.frameCounter <= 5)
                    {
                        Projectile.frame = 11;
                    }
                    else if (Projectile.frameCounter <= 10)
                    {
                        Projectile.frame = 0;
                    }
                }
            }
            else
            {
                //收回时的帧图
                Projectile.DProj().Bool[1] = false;
                Projectile.frameCounter++;
                Projectile.netUpdate = true;
                if (Projectile.frameCounter <= 5)
                {
                    Projectile.frame = 11;
                }
                else if (Projectile.frameCounter <= 10)
                {
                    Projectile.frame = 0;
                }
                if (Projectile.DProj().Times[1] <= 0)
                {
                    player.itemTime = 0;
                    player.itemAnimation = 0;
                    
                    Projectile.Kill();
                }
                if (Projectile.DProj().Times[4] > 0.2F)
                {
                    Projectile.DProj().Times[4] -= 0.03f;
                }
            }
            //伤害
            int damageWithChargeAndStats = player.GetWeaponDamage(player.HeldItem);
            Projectile.damage = damageWithChargeAndStats;

            if (Projectile.ai[0] >= player.HeldItem.useAnimation && Projectile.DProj().Bool[1])
            {
                if (Main.myPlayer == Projectile.owner)
                {
                    //消耗魔力
                    int A = player.ItemMana();
                    player.statMana -= A;

                    Vector2 vector13 = Main.MouseWorld - Projectile.Center;
                    //如果玩家需要翻转
                    if (vector13.X > 0)
                    {
                        player.ChangeDir(1);
                    }
                    else
                    {
                        player.ChangeDir(-1);
                    }
                    Projectile.netUpdate = true;
                }
                int Type = ModContent.DustType<光球粒子>();
                //int Type = 229;
                for (float A = 0; A < Projectile.scale; A += 0.01f)
                {
                    Type = ModContent.DustType<光球粒子>();
                    if (Main.rand.NextBool(10))
                    {
                        Type = ModContent.DustType<速度粒子>();
                    }
                    Dust dust = Main.dust[NewDust(Projectile.Center, 1, 1, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, new Color(20, 233, 201, 0))];
                    dust.noGravity = true;
                    dust.scale = 1;
                    if(Type == ModContent.DustType<速度粒子>()) dust.scale = 3.1F;
                    dust.customData=2;
                    GlobalDust.DustPlayerOwner[dust.dustIndex] = player.whoAmI;
                    dust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(2.5f, 3.5f);
                    if (Type == ModContent.DustType<速度粒子>()) dust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(4.5f, 7.5f)*3;
                    dust.rotation = dust.velocity.ToRotation();
                }
                if (Projectile.owner == Main.myPlayer)
                {
                    Projectile proj = Main.projectile[NewProjectileChange(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(0, -10), ModContent.ProjectileType<Moonlight>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0, 0, 1)];
                }
                PlaySound(SoundID.Item88, Projectile.position);
                Projectile.ai[0] -= player.HeldItem.useAnimation;
            }
            Projectile.netUpdate = true;
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
            Texture2D VoidStar = DDTextures.VoidStar.Value;
            Projectile.DProj().Times[0] += 0.1f;
            Texture2D texture = DDTextures.Circle[4].Value;
            for (int A = 0; A < 2; A++)
            {
                Color color = new Color(20, 233, 201, 0);
                Vector2 vector2 = Projectile.Center - Main.screenPosition + new Vector2(-2, 7);
                Main.spriteBatch.Draw(VoidStar, vector2, null, color * 0.05f, 0, VoidStar.Size() / 2, new Vector2(0.5f, 0.2f) * Projectile.DProj().Times[4], 0, 0f);
                DDHelper.Compression(texture, color, 0, Projectile.Opacity, new Vector2(2, 6), 1, Projectile.DProj().Times[0], BlendState.Additive);

                Main.spriteBatch.Draw(texture, vector2, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), color, 0f, Utils.Size(texture) / 2, Projectile.DProj().Times[4], 0, 0);
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            }
            texture = TextureAssets.Projectile[Projectile.type].Value;
            SpriteEffects sprite = (SpriteEffects)((Main.player[Projectile.owner].direction == 1) ? 0 : 1);
            Rectangle? rectangle = new Rectangle?(new Rectangle(0, texture.Height / Main.projFrames[Projectile.type] * Projectile.frame, texture.Width, texture.Height / Main.projFrames[Projectile.type]));
            Vector2 vector = new Vector2(texture.Width / 2, texture.Height / Main.projFrames[Projectile.type] / 2);
            if (Main.player[Projectile.owner].direction == 1)
            {
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, rectangle, Color.White, Projectile.rotation, vector, new Vector2(Projectile.scale, Projectile.scale / 1.5f) / 1.3f, sprite, 0f);
            }
            else
            {
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, rectangle, Color.White, Projectile.rotation + MathHelper.Pi, vector, new Vector2(Projectile.scale, Projectile.scale / 1.5f) / 1.3f, sprite, 0f);
            }
            return false;
        }
    }
}