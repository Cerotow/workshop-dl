namespace DDmod.Content.Projectiles.Magic.Book
{
    public class CursedFlames : ModProjectile
    {
        public static Asset<Texture2D> Glow;
        public override void Load()
        {
            Glow = ModContent.Request<Texture2D>(Texture + "_Glow");
        }
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
            Main.projFrames[Projectile.type] = 8;
        }
        //火焰计时器
        public int HY;
        public override bool PreAI()
        {
            Player player = Projectile.Player();
            Projectile.HoldProj(player, Projectile.DProj().Times[1], 0, new Vector2(player.direction, 0), 0, 0, true, Projectile.DProj().Times[4] >= 1f ? 1 : 0, false);

            Projectile.HoldBook(player.ItemMana(), 50, 15, 0.1F, 3);

            int Type = 75;
            if (Projectile.DProj().Bool[1])
            {
                Dust dust2 = Main.dust[NewDust(Projectile.position + new Vector2(4, 10), 30, 1, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                dust2.noGravity = true;
                dust2.scale = 1;
                dust2.velocity = new Vector2(0, -Main.rand.NextFloat(2, 6));
            }
            if (Projectile.DProj().Bool[1])
            {
                if (Projectile.ai[0] >= player.HeldItem.useAnimation)
                {
                    Vector2 vector2 = Projectile.velocity.PerfectNormalize();

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
                        vector2 = vector13.PerfectNormalize();
                    }
                    for (float A = 0; A < Projectile.scale; A += 0.01f)
                    {
                        Dust dust = Main.dust[NewDust(Projectile.Center, 1, 1, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                        dust.noGravity = true;
                        dust.scale = 1.3f;
                        dust.velocity = Main.rand.NextVector2Unit() * -Main.rand.NextFloat(3, 3.5f);
                    }
                    if (Projectile.owner == Main.myPlayer)
                    {
                        if (vector2.Y > 0)
                        {
                            Projectile proj = Main.projectile[NewProjectileChange(Projectile.GetSource_FromThis(), Projectile.Center, vector2 * 10, 95, Projectile.damage, Projectile.knockBack, Projectile.owner, 0, 0, 1)];
                            proj.localAI[0] = 1;
                        }
                        else
                        {
                            Projectile proj = Main.projectile[NewProjectileChange(Projectile.GetSource_FromThis(), Projectile.Center, vector2 * 10, 95, Projectile.damage, Projectile.knockBack, Projectile.owner, 0, 0, 1)];
                        }
                    }
                    SoundStyle sound = SoundID.Item8;
                    PlaySound(sound, Projectile.position);
                    Projectile.ai[0] -= player.HeldItem.useAnimation;
                    Projectile.netUpdate = true;
                }
                NPC npc = Projectile.FindTargetWithinRange(400, true);
                if (npc != null && npc.CanBeChasedBy(Projectile, false) && HY < 0)
                {
                    Vector2 vector = Vector2.Subtract(npc.Center, Projectile.Center);
                    if (Projectile.owner == Main.myPlayer)
                    {
                        Projectile proj = Main.projectile[NewProjectileChange(Projectile.GetSource_FromThis(), Projectile.Center - vector.PerfectNormalize() * 50, vector.PerfectNormalize() * 7, 101, Projectile.damage, Projectile.knockBack, Projectile.owner, 0, 0, 1)];
                        proj.hostile = false;
                        proj.friendly = true;
                    }
                    HY = 20;
                }
                HY--;


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
            Texture2D VoidStar = DDTextures.VoidStar.Value;
            Projectile.DProj().Times[0] += 0.1f;
            Texture2D texture = DDTextures.Circle[4].Value;
            Texture2D texture2 = Glow.Value;
            Color color = new Color(96, 248, 2, 0);
            for (int A = 0; A < 2; A++)
            {
                Vector2 vector2 = Projectile.Center - Main.screenPosition + new Vector2(-2, 7);
                Main.spriteBatch.Draw(VoidStar, vector2, null, color * 0.05f, 0, VoidStar.Size() / 2, new Vector2(0.5f, 0.2f) * Projectile.DProj().Times[4], 0, 0f);
                DDHelper.Compression(texture, color, 0, Projectile.Opacity, new Vector2(2, 6), 1, Projectile.DProj().Times[0], BlendState.Additive);

                Main.spriteBatch.Draw(texture, vector2, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), color, 0f, Utils.Size(texture) / 2, Projectile.DProj().Times[4], 0, 0);
                Main.spriteBatch.Draw(texture, vector2, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), color, 0f, Utils.Size(texture) / 2, Projectile.DProj().Times[4], 0, 0);
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            }
            texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            SpriteEffects sprite = (SpriteEffects)((Main.player[Projectile.owner].direction == 1) ? 0 : 1);
            Rectangle? rectangle = new Rectangle?(new Rectangle(0, texture.Height / Main.projFrames[Projectile.type] * Projectile.frame, texture.Width, texture.Height / Main.projFrames[Projectile.type]));
            Vector2 vector = new Vector2(texture.Width / 2, texture.Height / Main.projFrames[Projectile.type] / 2);
            if (Main.player[Projectile.owner].direction == 1)
            {
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, rectangle, lightColor, Projectile.rotation, vector, new Vector2(Projectile.scale, Projectile.scale / 1.5f) / 1.3f, sprite, 0f);

                Main.spriteBatch.Draw(texture2, Projectile.Center - Main.screenPosition, rectangle, Color.White, Projectile.rotation, vector, new Vector2(Projectile.scale, Projectile.scale / 1.5f) / 1.3f, sprite, 0f);
            }
            else
            {
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, rectangle, lightColor, Projectile.rotation + MathHelper.Pi, vector, new Vector2(Projectile.scale, Projectile.scale / 1.5f) / 1.3f, sprite, 0f);

                Main.spriteBatch.Draw(texture2, Projectile.Center - Main.screenPosition, rectangle, Color.White, Projectile.rotation + MathHelper.Pi, vector, new Vector2(Projectile.scale, Projectile.scale / 1.5f) / 1.3f, sprite, 0f);
            }
            return false;
        }
    }
}