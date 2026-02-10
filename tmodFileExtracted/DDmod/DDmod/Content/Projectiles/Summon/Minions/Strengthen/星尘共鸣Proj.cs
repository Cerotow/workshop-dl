using DDmod.Content.Projectiles.Magic.Staff;
using DDmod.Content.Projectiles.Magic;
using DDmod.Content.Projectiles.Melee;
using DDmod.Content.Projectiles.Melee.Sword;
using Terraria;
using Terraria.Graphics.Shaders;
using DDmod.Content.Dusts;
using DDmod.Sync;

namespace DDmod.Content.Projectiles.Summon.Minions.Strengthen
{
    public class 星尘共鸣Proj : ModProjectile
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
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.hide = true;
            Projectile.DProj().Times[2] = 0;
        }
        //public static Asset<Texture2D> Glow;
        public override void Load()
        {
            //Glow = ModContent.Request<Texture2D>(Texture + "_Glow");
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
        }
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.CritChance = player.GetWeaponCrit(player.ActiveItem());
            Projectile.damage = player.GetWeaponDamage(player.ActiveItem());
            Vector2 vector = player.RotatedRelativePoint(player.ArmCenter(), reverseRotation: false, addGfxOffY: false);

            if (player.controlUseItem&& player.statMana >= player.ItemMana()/60)
            {
                player.Dplayer().Realm = 10;
                int p = -1;
                for (int a = 0; a < 1000; a++)
                {
                    Projectile projectile = Main.projectile[a];
                    if (projectile.type > 0 && projectile.active && projectile.owner == Projectile.owner && projectile.type == ModContent.ProjectileType<星尘核心Proj>())
                    {
                        p = a;
                        break;
                    }
                }
                if (p == -1)
                {
                    if (Projectile.ai[1] >= 1 && Projectile.DProj().Times[2] >= 20)
                    {
                        int proj = NewProjectile(Projectile.GetSource_FromThis(), player.Center, Vector2.Zero, ModContent.ProjectileType<星尘核心Proj>(), 0, 0, Projectile.owner);
                        Main.projectile[proj].netUpdate = true;
                    }
                    else
                    {
                        TT = 0;

                    }
                }
                if (TT2 > 1)
                {
                    TT2 -= 0.2F;
                }
                else
                {
                    TT2 += 0.2f;
                }

                if (player.Dplayer().MouseWorld.X - vector.X > 0)
                {
                    Projectile.DProj().vector[0] = new Vector2(1, 0);
                }
                else
                {
                    Projectile.DProj().vector[0] = new Vector2(-1, 0);
                }
                Projectile.velocity = Projectile.DProj().vector[0].PerfectNormalize();

                Projectile.DProj().Times[1] = Projectile.DProj().vector[0].ToRotation();
                if (player.Dplayer().MouseWorld.X - vector.X > 0)
                {
                    Projectile.HoldProj(player, 12, Projectile.DProj().Times[1], new Vector2(1, 0), -MathHelper.PiOver4 + player.fullRotation, 0.25F, true);
                    Projectile.Center -= new Vector2(Projectile.DProj().Times[2] / 5, Projectile.DProj().Times[2] / 2 + 6);
                }
                else
                {
                    Projectile.HoldProj(player, 12, Projectile.DProj().Times[1], new Vector2(1, 0), -MathHelper.PiOver4 + MathHelper.Pi + player.fullRotation, 0.25F, true);
                    Projectile.Center -= new Vector2(-Projectile.DProj().Times[2] / 5, Projectile.DProj().Times[2] / 2 + 6);
                }
                //if (Projectile.ai[1] < 0.5)
                {
                    float v = 0;
                    if (player.direction == -1) v = 3.14f;
                    player.itemRotation = Projectile.velocity.ToRotation() * player.gravDir + v - player.fullRotation - MathHelper.PiOver4 * (Projectile.DProj().Times[2] / 15) * player.direction;
                }
                if (Projectile.ai[1] < 1)
                {
                    Projectile.ai[1] += 0.05f;
                    Projectile.ai[0] = 0;
                }
                else
                if (Projectile.DProj().Times[2] >= 20)
                {
                    NPC npc = NPCdirection.FindClosest(player.Dplayer().MouseWorld, 500, true);
                    Projectile.ai[2]++;
                    if (player.controlUseTile && TT2 < 2 && npc != null)
                    {
                        TT2 = 3.5F;
                        SoundStyle sound = SoundID.Item20;
                        sound.Pitch = -1;
                        PlaySound(sound, Projectile.Center);
                        Main.player[Projectile.owner].MinionAttackTargetNPC = npc.whoAmI;
                        NewDustChangeRound2(20, npc.Center, 1, ModContent.DustType<星光粒子>(), 0, 3, true, 1.25F, 100, new Color(0, 185, 255, 0), 3);


                        Vector2 vec = npc.Center - Main.projectile[p].Center;
                        for (int A = 0; A < vec.Length(); A++)
                        {
                            int B;
                            if (Main.rand.NextBool(5))
                            {
                                B = Dust.NewDust(Main.projectile[p].Center + vec.PerfectNormalize() * A, 0, 0, ModContent.DustType<星光粒子>(), 0, 0, 100, new Color(40, 185, 255, 0), Main.rand.NextFloat(0.5F, 1.25F));
                                Main.dust[B].velocity = Main.rand.NextFloat(MathHelper.TwoPi).ToRotationVector2() * Main.rand.NextFloat(2);
                            }
                            B = Dust.NewDust(Main.projectile[p].Center + vec.PerfectNormalize() * A, 0, 0, ModContent.DustType<光球粒子>(), 0, 0, 100, new Color(40, 185, 255, 0), 0.5f);
                            Main.dust[B].velocity = Vector2.Zero;
                        }
                        Projectile.ai[2] = 0;
                    }
                    if (Main.player[Projectile.owner].MinionAttackTargetNPC != -1 && Projectile.ai[2] % 60 == 0)
                    {
                        Main.npc[Main.player[Projectile.owner].MinionAttackTargetNPC].AddBuff(ModContent.BuffType<星尘共鸣Buff>(), 60);
                    }
                }
                else
                {
                    Projectile.DProj().Times[3]++;
                    if (Projectile.DProj().Times[3] > 20)
                    {
                        Projectile.DProj().Times[2] += 4;
                    }

                }
            }
            else
            {
                Projectile.Kill();
                player.itemAnimation = 0;
                player.itemTime = 0;
            }
            return false;
        }
        public override void OnKill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {

        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            if (player.direction == 1)
            {
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, texture.Size() / 2, Projectile.scale, 0, 0f);
            }
            else
            {
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation + MathHelper.Pi, texture.Size() / 2, Projectile.scale, (SpriteEffects)(-1), 0f);
            }
            Texture2D Starlight = DDTextures.Starlight.Value;
            DDHelper.BackAndForth(0.4F, 0.8F, 0.04F, ref TT, ref Projectile.DProj().Bool[4]);
            Main.spriteBatch.Draw(Starlight, Projectile.position + new Vector2(Projectile.width / 2, -2).RotatedBy(player.fullRotation) - Main.screenPosition, null, new Color(80, 185, 255, 0), player.fullRotation, Starlight.Size() / 2, new Vector2(1, 0.5f) * 2 * Projectile.ai[1] * (TT * TT2), 0, 0f);
            Main.spriteBatch.Draw(Starlight, Projectile.position + new Vector2(Projectile.width / 2, -2).RotatedBy(player.fullRotation) - Main.screenPosition, null, new Color(80, 185, 255, 0), player.fullRotation, Starlight.Size() / 2, new Vector2(1, 0.5f) * Projectile.ai[1] * (TT * TT2), 0, 0f);

            return false;
        }
        float TT;
        float TT2 = 1;

    }
    public class 星尘核心Proj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.alpha = 0;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 1;
            Projectile.scale = 1F;
            Projectile.timeLeft = 600;
            Main.projFrames[Type] = 1;
            Projectile.hide = true;
        }
        int A;
        public override bool? CanDamage()
        {
            return false;
        }
        public override void AI()
        {
            Player player = Projectile.Player();
            Projectile.position += player.Dplayer().PrePosition / 2;
            for (int a = 1; a < Projectile.oldPos.Length; a++)
            {
                if (Projectile.oldPos[a] != Vector2.Zero)
                    Projectile.oldPos[a] += player.velocity / 2;
            }
            bool AC = false;
            for (int a = 0; a < 1000; a++)
            {
                Projectile Proj = Main.projectile[a];
                if (Proj.active && Projectile.owner == Proj.owner)
                {
                    if (Proj.type == ModContent.ProjectileType<星尘共鸣Proj>())
                    {
                        AC = true;
                        break;
                    }
                }
            }
            if (!AC)
            {
                Projectile.Kill();
            }
            Projectile.timeLeft = 5;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;

            Projectile.ai[0]++;
            float rotationAngle = Projectile.ai[0] * 0.025f;
            rotationAngle %= MathHelper.TwoPi;

            Projectile.ai[1] += 0.003f;
            Projectile.ai[1] %= MathHelper.TwoPi;

            Vector2 ellipseOffset = new Vector2(
                (float)Math.Cos(rotationAngle) * 60f,
                (float)Math.Sin(rotationAngle) * 15f
            ).RotatedBy(Projectile.ai[1]);

            Vector2 targetPos = player.Center + ellipseOffset;

            Vector2 ellipseMajorAxis = new Vector2(60f, 0f).RotatedBy(Projectile.ai[1]);

            Vector2 toProjectile = (Projectile.Center - player.Center).SafeNormalize(Vector2.UnitX);
            float dot = Vector2.Dot(toProjectile, ellipseMajorAxis.SafeNormalize(Vector2.UnitX));

            bool isOnMajorAxisFront = dot > 0;
            float targetScale = isOnMajorAxisFront ? 1.25f : 0.75f;

            Projectile.scale = MathHelper.Lerp(Projectile.scale, targetScale, 0.02f);

            float distance = (targetPos - Projectile.Center).Length();
            float speed = MathHelper.Clamp(distance / 20f, 0.5f, 10f);
            Projectile.velocity = (targetPos - Projectile.Center).SafeNormalize(Vector2.Zero) * speed;
            Projectile.localAI[2]--;
            Projectile.ai[2]++;
            if (Projectile.ai[2] >= player.ActiveItem().useAnimation)
            {
                if (player.statMana >= player.ItemMana() / 30)
                {
                    int A = player.ItemMana() / 30;
                    player.statMana -= A;
                    Projectile.ai[2] -= player.ActiveItem().useAnimation;
                    if (!player.HasBuff(ModContent.BuffType<星尘共鸣Buff>()))
                        player.AddBuff(ModContent.BuffType<星尘共鸣Buff>(), player.ActiveItem().useAnimation + 20);
                    Projectile.localAI[2] = 30;
                }
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(255, 255, 255, Projectile.alpha));
        }
        public override void OnKill(int timeLeft)
        {
            for (int a = 0; a < 6; a++)
            {
                int A= Dust.NewDust(Projectile.Center, 1, 1, ModContent.DustType<星光粒子>(),0,0,100,new Color(0,185,255,0));

            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects spriteEffects = (SpriteEffects)1;
            if (Projectile.direction == 1)
            {
                spriteEffects = 0;
            }
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, texture.Size() / 2, Projectile.scale, 0, 0f);
            if (Projectile.localAI[2] > 0)
            {
                float SC = 1- Projectile.localAI[2]/30;
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, new Color(0,185,255,0)* (1-SC), Projectile.rotation, texture.Size() / 2, Projectile.scale* (1+SC), 0, 0f);
            }
            return false;
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            if (Projectile.scale > 1f)
            {
                overPlayers.Add(index);
            }
            else if (Projectile.scale < 1f)
            {
                behindProjectiles.Add(index);
            }
        }
    }
}