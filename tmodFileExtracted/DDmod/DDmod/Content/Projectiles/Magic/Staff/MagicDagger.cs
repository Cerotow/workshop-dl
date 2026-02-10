namespace DDmod.Content.Projectiles.Magic.Staff
{
    public class MagicDagger : ModProjectile
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
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
        }
        int T = 0;
        int T2 = 0;
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            player.heldProj = Projectile.whoAmI;
            Vector2 vector2 = player.RotatedRelativePoint(player.ArmCenter(), true);
            if (T == 0)
            {
                T = player.direction;
                Projectile.ai[0] = 2 * player.direction;
                Projectile.localAI[1] = -12;
                Projectile.DProj().vector[0] = (Main.MouseWorld - vector2).PerfectNormalize();
            }
            Vector2 vector = Projectile.DProj().vector[0].PerfectNormalize();
            Projectile.HoldProj(player, 24, Projectile.ai[0], Projectile.DProj().vector[0], MathHelper.PiOver2, 0, false, 0.5f * T, false);
            if (Projectile.localAI[1] < 0)
            {
                Projectile.localAI[1]++;
                Projectile.ai[0] = -2 * T;
            }

            if (player.statMana < player.ItemMana())
            {
                if (Projectile.localAI[0] > 2F || Projectile.localAI[0] < -2F)
                {
                    Projectile.Kill();
                }
            }
            if (player.direction == 1)
            {
                if (Projectile.ai[0] > 2)
                {
                    T = -1;
                    Projectile.localAI[1] = -12;
                    if (Main.myPlayer == Projectile.owner)
                    {
                        Projectile.DProj().vector[0] = (Main.MouseWorld - vector2).PerfectNormalize();

                        player.ChangeDir(Projectile.DProj().vector[0].X > 0 ? 1 : -1);
                    }
                }
                if (Projectile.ai[0] < -2F)
                {
                    T = 1;
                    Projectile.localAI[1] = -12;
                    if (Main.myPlayer == Projectile.owner)
                    {
                        Projectile.DProj().vector[0] = (Main.MouseWorld - vector2).PerfectNormalize();
                        player.ChangeDir(Projectile.DProj().vector[0].X > 0 ? 1 : -1);
                    }
                }
                if (Projectile.localAI[1] == 0)
                {
                    if (T == 1)
                    {
                        if (Projectile.ai[0] > 0)
                        {
                            Projectile.localAI[1] = 1;
                            if (Main.myPlayer == Projectile.owner)
                            {
                                Projectile projectile = Main.projectile[NewProjectileChange(Projectile.GetSource_FromThis(), Projectile.Center, vector * 25, 93, Projectile.damage, Projectile.knockBack, Projectile.owner)];
                            }
                            player.statMana -= player.ItemMana();
                            for (int A = 0; A < 25; A++)
                            {
                                int num = NewDust(Projectile.position, Projectile.width, Projectile.height, 57, Projectile.velocity.X * 0.2f + Projectile.direction * 3, Projectile.velocity.Y * 0.2f, 100, default, 1f);
                                Main.dust[num].velocity.X *= 0.3f;
                                Main.dust[num].velocity.Y *= 0.3f;
                            }
                            Projectile.netUpdate = true;
                        }
                    }
                    else if (Projectile.ai[0] < 0)
                    {
                        Projectile.localAI[1] = 1;

                        if (Main.myPlayer == Projectile.owner)
                        {
                            Projectile projectile = Main.projectile[NewProjectileChange(Projectile.GetSource_FromThis(), Projectile.Center, vector * 25, 93, Projectile.damage, Projectile.knockBack, Projectile.owner)];
                        }
                        player.statMana -= player.ItemMana();
                        for (int A = 0; A < 25; A++)
                        {
                            int num = NewDust(Projectile.position, Projectile.width, Projectile.height, 57, Projectile.velocity.X * 0.2f + Projectile.direction * 3, Projectile.velocity.Y * 0.2f, 100, default, 1f);
                            Main.dust[num].velocity.X *= 0.3f;
                            Main.dust[num].velocity.Y *= 0.3f;
                        }
                        Projectile.netUpdate = true;
                    }
                }
            }
            else
            {
                if (Projectile.ai[0] < -2F)
                {
                    T = 1;
                    Projectile.localAI[1] = -12;
                    if (Main.myPlayer == Projectile.owner)
                    {
                        Projectile.DProj().vector[0] = (Main.MouseWorld - vector2).PerfectNormalize();
                        player.ChangeDir(Projectile.DProj().vector[0].X > 0 ? 1 : -1);
                    }
                }
                if (Projectile.ai[0] > 2F)
                {
                    T = -1;
                    Projectile.localAI[1] = -12;
                    if (Main.myPlayer == Projectile.owner)
                    {
                        Projectile.DProj().vector[0] = (Main.MouseWorld - vector2).PerfectNormalize();
                        player.ChangeDir(Projectile.DProj().vector[0].X > 0 ? 1 : -1);
                    }
                }
                if (Projectile.localAI[1] == 0)
                {
                    if (T == 1)
                    {
                        if (Projectile.ai[0] > 0)
                        {
                            Projectile.localAI[1] = 1;

                            if (Main.myPlayer == Projectile.owner)
                            {
                                Projectile projectile = Main.projectile[NewProjectileChange(Projectile.GetSource_FromThis(), Projectile.Center, vector * 25, 93, Projectile.damage, Projectile.knockBack, Projectile.owner)];

                                player.statMana -= player.ItemMana();
                            }
                            for (int A = 0; A < 25; A++)
                            {
                                int num = NewDust(Projectile.position, Projectile.width, Projectile.height, 57, Projectile.velocity.X * 0.2f + Projectile.direction * 3, Projectile.velocity.Y * 0.2f, 100, default, 1f);
                                Main.dust[num].velocity.X *= 0.3f;
                                Main.dust[num].velocity.Y *= 0.3f;
                            }
                            Projectile.netUpdate = true;
                        }
                    }
                    else if (Projectile.ai[0] < 0)
                    {
                        Projectile.localAI[1] = 1;
                        if (Main.myPlayer == Projectile.owner)
                        {
                            Projectile projectile = Main.projectile[NewProjectileChange(Projectile.GetSource_FromThis(), Projectile.Center, vector * 25, 93, Projectile.damage, Projectile.knockBack, Projectile.owner)];

                            player.statMana -= player.ItemMana();
                        }
                        for (int A = 0; A < 25; A++)
                        {
                            int num = NewDust(Projectile.position, Projectile.width, Projectile.height, 57, Projectile.velocity.X * 0.2f + Projectile.direction * 3, Projectile.velocity.Y * 0.2f, 100, default, 1f);
                            Main.dust[num].velocity.X *= 0.3f;
                            Main.dust[num].velocity.Y *= 0.3f;
                        }
                        Projectile.netUpdate = true;
                    }
                }
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
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Magic/Staff/MagicDagger2");
            Vector2 vector = new Vector2(Projectile.width, Projectile.height) / 2;
            Player player = Main.player[Projectile.owner];
            if (Projectile.localAI[1] == 0)
            {
                for (int i = 0; i < Projectile.oldPos.Length; i++)
                {
                    Vector2 vector2 = Projectile.oldPos[i] + vector - Main.screenPosition;
                    Color color = new Color(236, 236, 51, 0) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2f);
                    Main.spriteBatch.Draw(texture, vector2, null, color, Projectile.oldRot[i], texture.Size() / 2, Projectile.scale * 1.2F, 0, 0f);
                    Main.spriteBatch.Draw(texture, vector2, null, color, Projectile.oldRot[i], texture.Size() / 2, Projectile.scale * 0.8F, 0, 0f);

                    Projectile.oldPos[i] += player.velocity;
                }
                texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), Color.White, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), Projectile.scale, 0, 0f);
            }
            return false;
        }
    }
}