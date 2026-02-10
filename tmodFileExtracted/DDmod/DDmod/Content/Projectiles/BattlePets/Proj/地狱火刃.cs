using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Dusts;
using DDmod.Content.NPCs.Boss.绿岩之视;
using DDmod.Content.Particles;
using DDmod.Content.Projectiles.Magic.Staff;
using DDmod.Content.Projectiles.Melee.Sword;
using DDmod.NoContent.Config;
using DDmod.Worlds;
using System.Transactions;
using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.BattlePets.Proj
{
    public class 地狱火刃 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
        }

        public override void SetDefaults()
        {
            Projectile.MeleeProj().oldVels = new Vector2[Projectile.oldPos.Length];
            Projectile.MeleeProj().oldVels2 = 56;

            Projectile.width = 14;
            Projectile.height = 60;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.alpha = 0;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 9;
            Projectile.scale = 1f;
            Projectile.timeLeft = 300;
            Projectile.hide = false;
            Projectile.DProj().Times[0] = 2;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.localAI[2] = -1;
        }
        public override void AI()
        {
            Player player = Projectile.Player();
            Projectile.velocity = (Projectile.rotation - MathHelper.PiOver4).ToRotationVector2().PerfectNormalize();

            Players.BattlePets battle = player.Dplayer().Bpets[player.Dplayer().FightPets];
            if (player.Dplayer().ownedFightPets < 0 || battle == null || battle.Type != Players.BattlePets.牢中灵骷 || battle.Variant != 4)
            {
                Projectile.spriteDirection = Projectile.DProj().Times[0] > 0 ? 0 : 1;
                Projectile.Kill();
                return;
            }

            Projectile proj = Main.projectile[player.Dplayer().ownedFightPets];
            bool isReturning = Projectile.ai[0] > 0 && Projectile.ai[0] <= 100;

            // 更新基础状态
            Projectile.damage = isReturning ? 0 : proj.damage;
            Projectile.timeLeft = 1200;

            // 初始化目标
            if (isReturning || Projectile.localAI[2] == -1)
                Projectile.localAI[2] = proj.DProj().Times[0];

            NPC target = Main.npc[(int)Projectile.localAI[2]];
            if (!target.CanBeChasedBy() && isReturning)
            {
                Projectile.localAI[2] = proj.DProj().Times[0];
                target = Main.npc[(int)Projectile.localAI[2]];
            }

            Vector2 toTarget = Projectile.Center - target.Center;

            if (target.CanBeChasedBy() && proj.DProj().Bool[3])
            {
                // 目标移动逻辑
                float innerRange = Projectile.height - 12;
                float middleRange = Projectile.height - 24;
                float outerRange = Projectile.height * 2;

                if (DDHelper.CircleInsertRectangle(target.Hitbox, Projectile.Center, innerRange))
                {
                    Projectile.DProj().vector[0] = DDHelper.CircleInsertRectangle(target.Hitbox, Projectile.Center, middleRange)
                        ? toTarget.PerfectNormalize()
                        : Projectile.DProj().vector[0] * 0.5f;
                }
                else if (!DDHelper.CircleInsertRectangle(target.Hitbox, Projectile.Center, outerRange))
                {
                    NewDustChange2(100, Projectile.Center, Vector2.Zero, 6, 0, 8, true, 1.2f, 2f, 0);
                    for (int i = 0; i < 1000; i++)
                    {
                        Projectile.position -= toTarget.PerfectNormalize();
                        if (DDHelper.CircleInsertRectangle(target.Hitbox, Projectile.Center, innerRange))
                        {
                            NewDustChange2(100, Projectile.Center, Vector2.Zero, 6, 0, 8, true, 1.2f, 2f, 0);
                            break;
                        }
                    }
                }
                else
                {
                    Projectile.DProj().vector[0] = -toTarget.PerfectNormalize();
                }

                // 攻击计时器
                if (DDHelper.CircleInsertRectangle(target.Hitbox, Projectile.Center, Projectile.height) || Projectile.ai[0] < 100)
                    Projectile.ai[0]--;

                Projectile.position += (target.position - target.oldPosition) / (Projectile.extraUpdates + 1);
            }
            else
            {
                // 无目标时的移动
                if (isReturning)
                {
                    Projectile.ai[0] = 100;
                    Vector2 toOwner = proj.Center + new Vector2(0, 40) - Projectile.Center;
                    if (toOwner.Length() > 3)
                        Projectile.DProj().vector[0] = (Projectile.DProj().vector[0] * 20 + toOwner.PerfectNormalize() * 2) / 21;
                    else
                        Projectile.DProj().vector[0] *= 0.92f;

                    float targetRotation = Projectile.DProj().Times[0] > 0 ? 6 : 5;
                    Projectile.RotationSpeed(targetRotation, 0.06f);
                }
                else
                {
                    Projectile.ai[0]--;
                    Projectile.DProj().vector[0] = Vector2.Zero;
                }
            }

            // 旋转方向
            Projectile.DProj().Times[0] = toTarget.X > 0 ? -Projectile.localAI[0] : Projectile.localAI[0];

            // 攻击状态
            if (Projectile.ai[0] == 0)
                Projectile.ClearInvincibleFrame();
            if (Projectile.ai[0] <= 0)
            {
                // 旋转攻击阶段
                Projectile.localAI[0] = 2;
                Projectile.rotation += 0.03f * Projectile.DProj().Times[0];
                if (++Projectile.ai[1] > 40)
                {
                    Projectile.ai[1] = 0;
                    Projectile.ai[0] = 300;
                }
            }
            else if (Projectile.ai[0] > 100)
            {
                // 快速旋转
                Projectile.rotation += 0.03f * Projectile.DProj().Times[0];
            }
            else
            {
                // 减速瞄准阶段
                if (Projectile.ai[0] == 100 && target.CanBeChasedBy() && proj.DProj().Bool[3])
                {
                    // 冲锋攻击
                    Projectile.position = target.Center + toTarget.PerfectNormalize().RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)) * 500;
                    for (int i = 0; i < 1000; i++)
                    {
                        Projectile.position -= toTarget.PerfectNormalize() * 4;
                        if(Projectile.position.Y<4|| Projectile.position.X<4)
                        {
                            break;
                        }
                        if (DDHelper.CircleInsertRectangle(target.Hitbox, Projectile.Center, Projectile.height - 12))
                        {
                            NewDustChange2(100, Projectile.Center, Vector2.Zero, 6, 0, 8, true, 1.2f, 2f, 0);
                            break;
                        }
                    }
                }

                // 减速旋转（关键逻辑保持不变）
                float baseAngle = (-toTarget).ToRotation() + MathHelper.PiOver4;
                float targetAngle = Projectile.DProj().Times[0] > 0 ? baseAngle - 2.4f : baseAngle + 2.4f;
                Projectile.RotationSpeed(targetAngle, Projectile.localAI[0] + 0.03f);
            }

            // 减速控制（关键逻辑保持不变）
            if (Projectile.ai[0] > 0)
            {
                Projectile.scale = 1;
                Projectile.localAI[0] *= 0.96f;
            }

            Projectile.spriteDirection = Projectile.DProj().Times[0] > 0 ? 0 : 1;
        }
        public override bool ShouldUpdatePosition()
        {
            Projectile.position += Projectile.DProj().vector[0];
            return false;
        }

        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            if (ModContent.GetInstance<DDConfigClient>().SwordHit)
            {
                SoundStyle sound = new SoundStyle("DDmod/NoContent/Sounds/Items/Sword");
                sound.Pitch = 0;
                PlaySound(sound, target.position);
            }
            Vector2 vector = Main.rand.NextVector2Unit() * 60;
            int A = NewProjectile(Projectile.GetSource_FromThis(), target.Center, -vector / 10, ModContent.ProjectileType<GlobalSlash>(), 0, 0, Projectile.owner, target.whoAmI, 1);
            Main.projectile[A].DProj().color = color;
            target.AddBuff(323, 180);
        }

        public override void OnKill(int timeLeft)
        {
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float laserLength = MathHelper.Lerp(0, Projectile.height, 1.25f);
            Vector2 direction = Projectile.velocity.PerfectNormalize();
            float collisionPoint = 0f;

            return Collision.CheckAABBvLineCollision(
                Utils.TopLeft(targetHitbox),
                Utils.Size(targetHitbox),
                Projectile.Center,
                Projectile.Center + direction * laserLength,
                projHitbox.Width,
                ref collisionPoint);
        }

        Color color = new Color(255, 103, 3, 0);

        public float TWidth()
        {
            return 24;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 center = Projectile.Center;
            bool flip = Projectile.DProj().Times[0] > 0;

            // 绘制刀光拖尾
            DDHelper.BladeTrail(DDTextures.WhitePng, color, 1f, flip);
            TrailDrawer.Draw(Projectile.MeleeProj().oldVels, center - Main.screenPosition, 88, null, Projectile.scale * TWidth());

            DDHelper.BladeTrail(DDTextures.Wave, color, 1f, flip);
            TrailDrawer.Draw(Projectile.MeleeProj().oldVels, center - Main.screenPosition, 88, null, Projectile.scale * TWidth());

            // 切换绘制模式
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState,
                DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            // 绘制 projectile 和拖尾
            DrawProjectileWithTrail(lightColor);

            return false;
        }

        private void DrawProjectileWithTrail(Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 center = Projectile.Center - Main.screenPosition;
            lightColor = Color.White;

            // 绘制拖尾
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                float opacity = (Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length;
                DrawProjectileFrame(texture, Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition,
                    Projectile.oldRot[i], color * opacity, Projectile.scale);
            }

            // 绘制本体
            DrawProjectileFrame(texture, center, Projectile.rotation, lightColor, Projectile.scale);
            Main.spriteBatch.Draw(DDTextures.VoidStar.Value, center, null, color,0,
               DDTextures.VoidStar.Size()/2, 0.25F, (SpriteEffects)Projectile.spriteDirection, 0f);
            Main.spriteBatch.Draw(DDTextures.VoidStar.Value, center, null, color,0,
               DDTextures.VoidStar.Size()/2, 0.25F, (SpriteEffects)Projectile.spriteDirection, 0f);
        }

        private void DrawProjectileFrame(Texture2D texture, Vector2 position, float rotation, Color drawColor, float scale)
        {
            if (Projectile.spriteDirection == 0)
            {
                Main.spriteBatch.Draw(texture, position, null, drawColor, rotation,
                    new Vector2(0, texture.Height), scale, SpriteEffects.None, 0f);
            }
            else
            {
                Main.spriteBatch.Draw(texture, position, null, drawColor, rotation + MathHelper.PiOver2,
                    new Vector2(texture.Width, texture.Height), scale, (SpriteEffects)Projectile.spriteDirection, 0f);
            }
        }
    }
    public class 地狱火刃2 : ModProjectile
    {
        public override string Texture => new 地狱火刃().Texture;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
        }

        public override void SetDefaults()
        {
            Projectile.MeleeProj().oldVels = new Vector2[Projectile.oldPos.Length];
            Projectile.MeleeProj().oldVels2 = 56;

            Projectile.width = 14;
            Projectile.height = 60;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.alpha = 0;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 9;
            Projectile.scale = 1f;
            Projectile.timeLeft = 6000;
            Projectile.hide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 90;
        }
        public override void AI()
        {

            NPC npc;
                npc = NPCdirection.FindClosest(Projectile.Center, 800, true, null);
            
            if (npc != null && npc.active && Projectile.GetGlobalProjectile<DDGlobalProjectile>().track > 30)
            {
                if (!Projectile.hostile && Projectile.friendly)
                {
                    Vector2 vector = (npc.Center - Projectile.Center).PerfectNormalize() * 2;
                    Projectile.DProj().vector[0] = (Projectile.DProj().vector[0] * 120 + vector) / (121);
                }
            }
            if(Projectile.DProj().vector[0].Length()<2)
            {
                Projectile.DProj().vector[0] *= 1.01F;
            }
            // 旋转方向
            if (Projectile.DProj().vector[0] == Vector2.Zero)
            {
                Projectile.DProj().vector[0] = Projectile.velocity;
            }
            Projectile.velocity = (Projectile.rotation - MathHelper.PiOver4).ToRotationVector2().PerfectNormalize();
            if (Projectile.DProj().Times[0]==0)
            Projectile.DProj().Times[0] = Projectile.DProj().vector[0].X > 0 ? 2 : -2;
            Projectile.rotation += 0.05f * Projectile.DProj().Times[0];
            Projectile.spriteDirection = Projectile.DProj().Times[0] > 0 ? 0 : 1;
            if (Main.rand.NextBool(300))
            {
                Vector2 vector = Projectile.DProj().vector[0].RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
                Projectile.NewProjectileChange(Projectile.Center + vector.PerfectNormalize() * Main.rand.Next(500, 1400) / 10, -vector, ModContent.ProjectileType<火刃>(), Projectile.damage, Projectile.knockBack, -1);
            }
        }
        public override bool ShouldUpdatePosition()
        {
            //Projectile.Player().Center = Projectile.Center;
            Projectile.position += Projectile.DProj().vector[0];
            for (int i = 1; i < Projectile.oldPos.Length; i++)
            {
                Projectile.oldPos[i] += Projectile.DProj().vector[0];
            }
                return false;
        }

        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            if (ModContent.GetInstance<DDConfigClient>().SwordHit)
            {
                SoundStyle sound = new SoundStyle("DDmod/NoContent/Sounds/Items/Sword");
                sound.Pitch = 0;
                PlaySound(sound, target.position);
            }
            Vector2 vector = Main.rand.NextVector2Unit() * 60;
            int A = NewProjectile(Projectile.GetSource_FromThis(), target.Center, -vector / 10, ModContent.ProjectileType<GlobalSlash>(), 0, 0, Projectile.owner, target.whoAmI, 1);
            Main.projectile[A].DProj().color = color;
            target.AddBuff(323, 180);
        }

        public override void OnKill(int timeLeft) { }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float laserLength = MathHelper.Lerp(0, Projectile.height, 1.25f);
            Vector2 direction = Projectile.velocity.PerfectNormalize();
            float collisionPoint = 0f;

            return Collision.CheckAABBvLineCollision(
                Utils.TopLeft(targetHitbox),
                Utils.Size(targetHitbox),
                Projectile.Center,
                Projectile.Center + direction * laserLength,
                projHitbox.Width,
                ref collisionPoint);
        }

        Color color = new Color(255, 103, 3, 0);

        public float TWidth()
        {
            return 24;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 center = Projectile.Center;
            bool flip = Projectile.DProj().Times[0] > 0;

            // 绘制刀光拖尾
            DDHelper.BladeTrail(DDTextures.WhitePng, color, 1f, flip);
            TrailDrawer.Draw(Projectile.MeleeProj().oldVels, center - Main.screenPosition, 88, null, Projectile.scale * TWidth());

            DDHelper.BladeTrail(DDTextures.Wave, color, 1f, flip);
            TrailDrawer.Draw(Projectile.MeleeProj().oldVels, center - Main.screenPosition, 88, null, Projectile.scale * TWidth());

            // 切换绘制模式
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState,
                DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            // 绘制 projectile 和拖尾
            DrawProjectileWithTrail(lightColor);

            return false;
        }

        private void DrawProjectileWithTrail(Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 center = Projectile.Center - Main.screenPosition;
            lightColor = Color.White;

            // 绘制拖尾
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                float opacity = (Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length;
                DrawProjectileFrame(texture, Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition,
                    Projectile.oldRot[i], color * opacity, Projectile.scale);
            }

            // 绘制本体
            DrawProjectileFrame(texture, center, Projectile.rotation, lightColor, Projectile.scale);
            Main.spriteBatch.Draw(DDTextures.VoidStar.Value, center, null, color,0,
               DDTextures.VoidStar.Size()/2, 0.25F, (SpriteEffects)Projectile.spriteDirection, 0f);
            Main.spriteBatch.Draw(DDTextures.VoidStar.Value, center, null, color,0,
               DDTextures.VoidStar.Size()/2, 0.25F, (SpriteEffects)Projectile.spriteDirection, 0f);
        }

        private void DrawProjectileFrame(Texture2D texture, Vector2 position, float rotation, Color drawColor, float scale)
        {
            if (Projectile.spriteDirection == 0)
            {
                Main.spriteBatch.Draw(texture, position, null, drawColor, rotation,
                    new Vector2(0, texture.Height), scale, SpriteEffects.None, 0f);
            }
            else
            {
                Main.spriteBatch.Draw(texture, position, null, drawColor, rotation + MathHelper.PiOver2,
                    new Vector2(texture.Width, texture.Height), scale, (SpriteEffects)Projectile.spriteDirection, 0f);
            }
        }
    }
    public class 火刃 : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 96;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.aiStyle = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }
        public override void AI()
        {
            Player player = Projectile.Player();
            if (!Projectile.DProj().Bool[0])
            {
                Projectile.velocity = Projectile.velocity.PerfectNormalize() * 0.01F;
                Projectile.rotation = Projectile.velocity.ToRotation();
                Projectile.DProj().Times[2]++;
                if (Projectile.DProj().Times[2] > 60)
                {
                    Projectile.DProj().Bool[0] = true;
                }
                if (Projectile.DProj().Times[2] > 10)
                {
                    Projectile.DProj().Times[3] += 0.1F;
                }
                int Type = 6;

                if (Projectile.DProj().Times[3] < 1)
                {
                    float R = (1 - Projectile.DProj().Times[3]);
                    if (R > 1) R = 1;
                    for (int A = 0; A < 10; A++)
                    {
                        Dust dust = Main.dust[NewDust(Projectile.Center - Projectile.velocity.PerfectNormalize() * 56 - new Vector2(4), 1, 1, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                        dust.noGravity = true;
                        dust.scale = 1.4f + 0.5F * R;
                        dust.velocity = (Main.rand.NextVector2Unit() * Main.rand.NextFloat(0f, 2f + 1 * R) * new Vector2(2f, 2F)).RotatedBy(Projectile.rotation);
                        dust.noLightEmittence = false;
                    }
                }
                else
                {
                    Projectile.DProj().Times[3] = 1;
                    for (int A = 0; A < 10; A++)
                    {
                        Dust dust = Main.dust[NewDust(Projectile.Center - Projectile.velocity.PerfectNormalize() * 56 - new Vector2(4), 1, 1, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                        dust.noGravity = true;
                        dust.scale = 1.4f;
                        dust.velocity = (Main.rand.NextVector2Unit() * Main.rand.NextFloat(0f, 2f) * new Vector2(2f, 2F)).RotatedBy(Projectile.rotation);
                        dust.noLightEmittence = false;
                    }
                }

                if (Projectile.DProj().Times[3] == 1 && Projectile.ai[1]==0)
                {
                    Projectile.ai[1] = 1;
                    SoundStyle sound = new SoundStyle("DDmod/NoContent/Sounds/Items/叉");
                    sound.Pitch = 1;
                    PlaySound(sound, Projectile.position);
                    for (int A = 0; A < 50; A++)
                    {
                        float a = Main.rand.NextFloat(-30f, 30f);
                        Dust dust = Main.dust[NewDust(Projectile.Center - Projectile.velocity.PerfectNormalize() * 60 - new Vector2(4), 1, 1, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                        dust.noGravity = true;
                        dust.scale = 2.3f;
                        dust.position += Projectile.velocity.PerfectNormalize().RotatedBy(MathHelper.PiOver2) * a;
                        dust.velocity = Projectile.velocity.PerfectNormalize() * Main.rand.NextFloat(10f, 30f) * (1 - Math.Abs(a) / 90);
                        dust.noLightEmittence = false;
                    }
                }
            }
            else
            {
                Projectile.scale -= 0.1f;
                if (Projectile.DProj().Bool[2])
                {
                    Projectile.scale -= 0.1f;
                }
                if (Projectile.scale < 0.01F)
                {
                    Projectile.Kill();
                }
            }
        }
        public override void OnKill(int timeLeft)
        {
            if (!Projectile.DProj().Bool[1])
            {
                for (int A = -50; A < 50; A++)
                {
                    Dust dust = Main.dust[NewDust(Projectile.position + Projectile.velocity.PerfectNormalize() * A, 68, 68, 6, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                    dust.noGravity = true;
                    dust.scale = 1.4f;
                    dust.velocity = (Main.rand.NextVector2Unit() * Main.rand.NextFloat(0f, 2f) * new Vector2(2f, 2F)).RotatedBy(Projectile.rotation);
                    dust.noLightEmittence = false;
                }
            }
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (ModContent.GetInstance<DDConfigClient>().SwordHit)
            {
                SoundStyle sound = new SoundStyle("DDmod/NoContent/Sounds/Items/Sword");
                sound.Pitch = 0;
                PlaySound(sound, target.position);
            }
            Vector2 vector = Main.rand.NextVector2Unit() * 60;
            int A = NewProjectile(Projectile.GetSource_FromThis(), target.Center, -vector / 10, ModContent.ProjectileType<GlobalSlash>(), 0, 0, Projectile.owner, target.whoAmI, 1);
            Main.projectile[A].DProj().color = new Color(255, 103, 3, 0);
            if (Projectile.DProj().Times[3] < 1)
            {
                modifiers.SourceDamage += 2;
                if (target.knockBackResist > 0)
                {
                     vector = Projectile.velocity.PerfectNormalize() * 2;
                    if (Projectile.DProj().Bool[4])
                    {
                        vector = Projectile.velocity.PerfectNormalize() * 10;
                    }
                    target.velocity = vector;
                    DDmod.SyncData(DDType.NPCCenter, target.whoAmI, -1, Projectile.owner);
                }
            }
            if (target.velocity.Y != 0)
            {
                modifiers.Knockback *= 0;
            }
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            if (Projectile.DProj().Bool[1])
            {
                if (target.knockBackResist > 0)
                {
                    Vector2 vector = (target.Center - Projectile.Center).PerfectNormalize() * 8;
                    target.velocity = vector;
                    DDmod.SyncData(DDType.NPCCenter, target.whoAmI, -1, Projectile.owner);
                }
            }
            target.AddBuff(323, 180);
        }
        public override bool? CanDamage()
        {
            if (Projectile.DProj().Bool[1])
            {
                Player player = Projectile.Player();
                return !player.HasBuff(BuffID.ManaSickness);

            }
            return true;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Projectile.Player();
            Projectile.DProj().Times[0] += 0.05F;
            Vector2 vector2 = Projectile.Center - Main.screenPosition;
            Color color = Projectile.GetAlpha(new Color(253, 62, 3, 0));
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            if (Projectile.DProj().Times[3] >= 1)
            {
                Main.EntitySpriteDraw(texture, vector2, null, new Color(150, 150, 150, 255), Projectile.rotation - MathHelper.PiOver4, Utils.Size(texture) * 0.5f, 0.25F, 0, 0);
                Main.EntitySpriteDraw(texture, vector2, null, color, Projectile.rotation - MathHelper.PiOver4, Utils.Size(texture) * 0.5f, 0.25F, 0, 0);
            }
            else if (Projectile.DProj().Times[3] > 0)
            {
                Rectangle rectangle = new Rectangle((int)(texture.Width *(1- Projectile.DProj().Times[3])), (int)(texture.Height*(1 - Projectile.DProj().Times[3])), (int)(texture.Width * (Projectile.DProj().Times[3])), (int)(texture.Height * ( Projectile.DProj().Times[3])));
                vector2 += Projectile.velocity.PerfectNormalize() * 40;
                Main.EntitySpriteDraw(texture, vector2, rectangle, new Color(150, 150, 150, 255), Projectile.rotation - MathHelper.PiOver4, Utils.Size(texture) * 0.5f, 0.25F, 0, 0);
                Main.EntitySpriteDraw(texture, vector2, rectangle, color, Projectile.rotation - MathHelper.PiOver4, Utils.Size(texture) * 0.5f, 0.25F, 0, 0);
            }

            return false;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float laserLength = MathHelper.Lerp(0, Projectile.height, 1.25f);
            Vector2 direction = Projectile.velocity.PerfectNormalize();
            float collisionPoint = 0f;

            return Collision.CheckAABBvLineCollision(
                Utils.TopLeft(targetHitbox),
                Utils.Size(targetHitbox),
                Projectile.Center,
                Projectile.Center + direction * laserLength,
                projHitbox.Width,
                ref collisionPoint);
        }
    }
}