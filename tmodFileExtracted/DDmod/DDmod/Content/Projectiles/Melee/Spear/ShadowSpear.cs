namespace DDmod.Content.Projectiles.Melee.Spear
{
    public class ShadowSpear : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 1;
            Projectile.timeLeft = 300;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.ownerHitCheck = true;
        }
        public override bool? CanDamage()
        {
            return true;
        }
        public int MaxAnimation = 0;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if(MaxAnimation==0)
            {
                MaxAnimation = player.ActiveItem().useAnimation;
            }
            //额外更新
            if ((int)player.GetTotalAttackSpeed(Projectile.DamageType) > 1)
            {
                Projectile.extraUpdates = (int)player.GetTotalAttackSpeed(Projectile.DamageType)-1;
            }
            else
            {
                Projectile.extraUpdates =0;
            }
            //方向盘
            Projectile.rotation = Projectile.velocity.ToRotation() + (float)Math.PI / 2f;
            //大小加成
            Projectile.ProjScale();
            //记录速度
            if (Projectile.DProj().vector[0] == Vector2.Zero)
            {
                Projectile.DProj().vector[0] = Projectile.velocity;
            }
            //方向变动
            Vector2 velocity = Utils.RotatedBy(Projectile.DProj().vector[0].PerfectNormalize(), Projectile.ai[1]);
            Projectile.velocity = velocity * 2;

            if (Projectile.localAI[1] == 0)
            {
                //前进
                Projectile.ai[0] += player.GetTotalAttackSpeed(Projectile.DamageType) / (Projectile.extraUpdates+1)*3;
                //召唤额外弹幕
                if (Projectile.ai[0] >= MaxAnimation / 2)
                {
                    if (!Projectile.DProj().Bool[0]&& (Projectile.DProj().Times[1]==0)||(Projectile.DProj().Times[1]==1 && Projectile.ai[0] >= MaxAnimation / 2 + MaxAnimation/6) ||(Projectile.DProj().Times[1]==2 && Projectile.ai[0] >= MaxAnimation / 2 + MaxAnimation / 3))
                    {
                        int proj = NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.6F, 0.6F)), Projectile.type, Projectile.damage / 2, 0, player.whoAmI);
                        Main.projectile[proj].DProj().Bool[0] = true;
                        Main.projectile[proj].alpha = 120;
                        Projectile.DProj().Times[1] ++;
                    }
                }
                //戳完后
                if (Projectile.ai[0] > MaxAnimation)
                {
                    Projectile.localAI[1]++;
                }
            }
            else
            {
                //收回
                Projectile.ai[0] -= player.GetTotalAttackSpeed(Projectile.DamageType) / (Projectile.extraUpdates + 1) * 3;
                if (Projectile.ai[0] <= MaxAnimation / 4)
                {
                    Projectile.localAI[1]++;
                }

            }
            //收回后消失
            if (Projectile.localAI[1] >= 2)
            {
                if (!Projectile.DProj().Bool[0])
                {
                    player.itemTime = 0;
                    player.itemAnimation = 0;
                }
                Projectile.Kill();
                return;
            }
            //贴图旋转
            float a = 0;
            if (player.direction == -1) a = 3.14f;
            //速度方向
            float Rotation = Projectile.velocity.ToRotation()-player.fullRotation;
            //影矛本体
            if (!Projectile.DProj().Bool[0])
            {
                player.itemTime = 5;
                player.itemAnimation = 5;
                player.heldProj = Projectile.whoAmI;
                player.itemRotation = Rotation + a;
            }
            //前进
            float A = (Projectile.ai[0] / MaxAnimation) * 50 * Projectile.scale;
            //位置
            Projectile.Center = player.RotatedRelativePoint(player.ArmCenter(), reverseRotation: false, addGfxOffY: false) + Projectile.velocity * A;
            //贴图水平翻转
            Projectile.spriteDirection = ((!(Vector2.Dot(Projectile.velocity, Vector2.UnitX) < 0f)) ? 1 : (-1));
            //同步
            Projectile.netUpdate = true;
        }
        public override void OnKill(int timeLeft)
        {
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
        }
        public override bool PreDraw(ref Color lightColor)
        {
            //方向盘
            if (Projectile.rotation == 0)
            {
                Projectile.rotation = Projectile.velocity.ToRotation() + (float)Math.PI / 2f;
            }
            Player player = Main.player[Projectile.owner];
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            SpriteEffects spriteEffects = 0;
            if (player.direction == -1)
            {
                spriteEffects = (SpriteEffects)1;
            }
            float ro = Projectile.rotation + MathHelper.PiOver4;
            if (player.direction == -1)
            {
                ro = Projectile.rotation - MathHelper.PiOver4;
            }
            if(Projectile.DProj().Bool[0])
            {
                lightColor.R = 200;
                lightColor.G = 0;
                lightColor.B = 200;
            }
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition - Projectile.velocity.PerfectNormalize() * 38, null, Projectile.GetAlpha(lightColor), ro, new Vector2(texture.Width, texture.Height) / 2, Projectile.scale, spriteEffects, 0f);
            return false;
        }
    }
}