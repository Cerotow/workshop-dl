using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Items;
using DDmod.Content.Items.Melee.Sword;
using DDmod.NoContent.Config;
using System.Reflection;
using Terraria;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Melee.Sword
{
    public class DeathSickle : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 700;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ignoreWater = true;
            //projectile.light = 0.50f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.ownerHitCheck = true;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30;
            Projectile.scale = 0.1f;
            Projectile.MeleeProj().SwordHitbox = true;
            Projectile.MeleeProj().oldVels = new Vector2[Projectile.oldPos.Length];
            Projectile.extraUpdates = 6;
            Projectile.localAI[2] = 1;
            Projectile.stopsDealingDamageAfterPenetrateHits = true;
        }
        public override bool? CanDamage()
        {
            return null;
        }
        int proj = 0;
        public bool SpecialAttack;
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(proj);
            writer.Write(R);
            writer.Write(D);
            writer.Write(SpecialAttack);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            proj = reader.ReadInt32();
            R = reader.ReadInt32();
            D = reader.ReadInt32();
            SpecialAttack = reader.ReadBoolean();
        }
        public int D;
        public int R;
        public int NPC = -1;
        public override bool PreAI()
        {
            Item item = Projectile.Player().ActiveItem();
            Projectile.MeleeProj().oldVels2 = TextureAssets.Item[item.type].Size().Length() / 2 * 1.5f - 2;
            Projectile.MeleeProj().oldVels2 += 6;
            //大小加成
            Player player = Main.player[Projectile.owner];
            if (!player.HasBuff(ModContent.BuffType<SpecialAttackCD>()) && player.controlUseTile && Projectile.DProj().vector[0] == Vector2.Zero)
            {
                if (Collision.CanHitLine(player.Dplayer().MouseWorld, 1, 1, player.position, player.width, player.height))
                {
                    NPC npc = NPCdirection.FindClosest(player.Dplayer().MouseWorld, 300, true);
                    if (npc != null)
                    {
                        NPC = npc.whoAmI;
                        SpecialAttack = true;
                        Projectile.alpha = 255;
                        player.AddBuff(ModContent.BuffType<SpecialAttackCD>(), 600);
                        Projectile.damage = (int)(Projectile.damage * 2.25f);
                        for (int A = 0; A < 100; A++)
                        {
                            int Type = 27;
                            Dust dust = Main.dust[NewDust(player.Center, player.width, player.height, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                            dust.noGravity = true;
                            dust.velocity = new Vector2(Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-1, 1)) * Main.rand.NextFloat(2, 6);
                            dust.scale = 3f;
                        }
                    }
                }
            }
            if (Projectile.scale < player.GetAdjustedItemScale(player.ActiveItem()))
            {
                Projectile.scale = player.GetAdjustedItemScale(player.ActiveItem());
            }
            Projectile.Resize((int)(TextureAssets.Item[item.type].Width()/10 * Projectile.scale), (int)(TextureAssets.Item[item.type].Size().Length() / 2 * Projectile.scale * 1.2f));

            if (!SpecialAttack)
            {
                Projectile.HoldProj(player, TextureAssets.Item[item.type].Size().Length() / 2 * Projectile.scale + 4, Projectile.ai[0], Projectile.DProj().vector[0], MathHelper.PiOver4, 0, true, 0.05f * Projectile.DProj().Times[0], false, false);
                Projectile.HoldSword2(player, -player.HeldItem.useAnimation * (Projectile.extraUpdates+1), -1, 2.5f, false);
            }
            else
            {
                if (Projectile.scale < player.GetAdjustedItemScale(player.ActiveItem())*2)
                {
                    Projectile.scale = player.GetAdjustedItemScale(player.ActiveItem())*2;
                }
                NPC npc = Main.npc[NPC];
                if (R == 0)
                {
                    if (npc != null)
                    {
                        if (npc.Center.X - player.Center.X < 0)
                        {
                            D = 1;
                        }
                        else
                        {
                            D = -1;
                        }
                        player.direction = D;
                    }
                }
                if (D != 0)
                {
                    Projectile.DProj().vector[0] = new Vector2(D, 0).RotatedBy(D);
                    if (D > 0)
                    {
                        Projectile.DProj().vector[1] = npc.position - new Vector2(56, 56);
                    }
                    else
                    {
                        Projectile.DProj().vector[1] = npc.position + new Vector2(npc.width + 56, -56);
                    }
                    if (R == 0)
                    {
                        for (int A = 0; A < (player.Center - Projectile.DProj().vector[1]).Length(); A+=10)
                        {
                            int Type = 27;
                            Dust dust = Main.dust[NewDust(player.Center+ (Projectile.DProj().vector[1]-player.Center).PerfectNormalize()*A, 1, 1, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                            dust.noGravity = true;
                            dust.velocity = new Vector2(Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-1, 1)) * Main.rand.NextFloat(1, 3);
                            dust.scale = 1.5f;
                        }
                    }
                    for (int A = 0; A < 1; A++)
                    {
                        int Type = 27;
                        Dust dust = Main.dust[NewDust(player.position, player.width, player.height, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                        dust.noGravity = true;
                        dust.velocity = new Vector2(Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-1, 1)) * Main.rand.NextFloat(2, 6);
                        dust.scale = 1.25f;
                    }
                }
                if (Projectile.DProj().vector[1]!=Vector2.Zero)
                {
                    player.Center = Projectile.DProj().vector[1];
                }
                player.mount.Dismount(player);
                for (int a = 0; a < 1000; a++)
                {
                    if (Main.projectile[a].active && Main.projectile[a].aiStyle == 7 && Main.projectile[a].owner == Projectile.owner)
                    {
                        Main.projectile[a].Kill();
                    }
                }
                player.immune = true;
                player.immuneTime = 5;
                player.immuneAlpha = Projectile.alpha;
                R++;
                if (R < 255)
                {
                    if(Projectile.alpha>0&&R>204)
                    {
                        Projectile.alpha -= 5;
                    }
                    if(Projectile.localAI[2]<2.5F)
                    {
                        Projectile.localAI[2] += (2.5F - Projectile.localAI[2]) / 100;
                        Projectile.localAI[0] = Projectile.localAI[2] * player.direction;
                        //Projectile.localAI[1] = AttackSpeed;
                        float A = Projectile.localAI[0] + (Projectile.localAI[1] / (-player.HeldItem.useAnimation * (Projectile.extraUpdates + 1) * 3));
                        Projectile.localAI[1]++;
                        if (Projectile.localAI[1] > 0)
                        {
                            Projectile.localAI[1] = 0;
                        }
                        Projectile.ai[0] = -A;
                        Projectile.netUpdate = true;
                    }
                    
                    Projectile.HoldProj(player, TextureAssets.Item[item.type].Size().Length() / 2 * Projectile.scale + 4, Projectile.ai[0], Projectile.DProj().vector[0], MathHelper.PiOver4, 0, true,0, false, false);
                    
                    Projectile.HoldSword2(player, -player.HeldItem.useAnimation * (Projectile.extraUpdates+1), -1, Projectile.localAI[2], false);
                    Projectile.localAI[1] = 0;
                }
                else
                {
                    Projectile.HoldProj(player, TextureAssets.Item[item.type].Size().Length() / 2 * Projectile.scale + 4, Projectile.ai[0], Projectile.DProj().vector[0], MathHelper.PiOver4, 0, true, 0.05f * Projectile.DProj().Times[0], false, false);
                    Projectile.HoldSword2(player, -player.HeldItem.useAnimation * (Projectile.extraUpdates+1), -1, Projectile.localAI[2], false);
                }
            }
            int useTime = (int)(player.HeldItem.useTime / player.GetTotalAttackSpeed(DamageClass.Melee));
            if (Projectile.localAI[1] >= 0 && Projectile.ai[1] == 2 && Projectile.DProj().Times[2] < useTime)
            {
                Projectile.ai[1] = 3;
                PlaySound(SoundID.Item71, Projectile.position);
            }
            if (Projectile.localAI[1] <= 0 || Projectile.MeleeProj().DelayedKill > 0)
            {
                proj = 0;
            }
            else
            {
                if (proj == 0 && !player.ActiveItem().GetGlobalItem<MeleeGlobalItem>().TrueMelee&& !SpecialAttack)
                {
                    proj++;
                    Type T = player.GetType();
                    var m = T.GetMethod("ItemCheck_Shoot", BindingFlags.NonPublic | BindingFlags.Instance);
                    object[] o = new object[3];
                    o[0] = player.whoAmI;
                    o[1] = player.ActiveItem();
                    o[2] = Projectile.damage;
                    m.Invoke(player, o);
                    Projectile.netUpdate = true;
                }
            }
            Projectile.spriteDirection = Projectile.DProj().Times[0] > 0 ? 0 : 1;
            return false;
        }
        public override void ModifyHitNPC(NPC target, ref HitModifiers modifiers)
        {

            Player player = Projectile.Player();

            Item item = Projectile.Player().ActiveItem();
            modifiers.ArmorPenetration += player.GetWeaponArmorPenetration(item);

            ItemLoader.ModifyHitNPC(item, Projectile.Player(), target, ref modifiers);
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Player player = Projectile.Player();
            if (SpecialAttack && Projectile.DProj().vector[1] != Vector2.Zero)
            {
                Projectile.DProj().vector[1] = Vector2.Zero;
                player.velocity = new Vector2(-D * 5F, -10);
                D = 0;
            }
            Item item = Projectile.Player().ActiveItem();

            int num6 = Main.DamageVar(damageDone, player.luck);

            Type T = player.GetType();
            var m = T.GetMethod("ApplyNPCOnHitEffects", BindingFlags.NonPublic | BindingFlags.Instance);
            object[] o = new object[7];
            o[0] = player.ActiveItem();
            o[1] = new Rectangle((int)Projectile.Center.X, (int)Projectile.Center.Y, Projectile.width, Projectile.height);
            o[2] = Projectile.damage;
            o[3] = Projectile.knockBack;
            o[4] = target.whoAmI;
            o[5] = num6;
            o[6] = damageDone;
            m.Invoke(player, o);

            Projectile.netUpdate = true;
            Projectile.Player().StatusToNPC(item.type, target.whoAmI);
            if (target.life > 5)
                Projectile.Player().OnHit(target.Center.X, target.Center.Y, target);
            ItemLoader.OnHitNPC(item, Projectile.Player(), target, hit, damageDone);
            if (ModContent.GetInstance<DDConfigClient>().SwordHit)
            {
                SoundStyle sound = new SoundStyle("DDmod/NoContent/Sounds/Items/Sword");
                sound.Pitch = 0;
                PlaySound(sound, target.position);
            }

            Vector2 vector = Main.rand.NextVector2Unit() * 60;
            int A = NewProjectile(Projectile.GetSource_FromThis(), target.Center, -vector / 10, ModContent.ProjectileType<GlobalSlash>(), 0, 0, Projectile.owner, target.whoAmI, 1);
            Main.projectile[A].DProj().color = new Color(170, 10, 255, 0) * 0.5f;
            Main.projectile[A].scale = 1f;
            if (SpecialAttack)
            {
                for (int a = 0; a < 5; a++)
                {
                    vector = Main.rand.NextVector2Unit() * 60;
                    int B = NewProjectile(Projectile.GetSource_FromThis(), target.Center, -vector / 2, ModContent.ProjectileType<GlobalSlash>(), Projectile.damage, 0, Projectile.owner, target.whoAmI, 1);
                    Main.projectile[B].DProj().color = new Color(170, 10, 255, 0) * 0.5f;
                    Main.projectile[B].scale = Main.rand.NextFloat(1,4);
                }
            }
        }

        Color color = new Color(170, 10, 255, 0) * 0.5f;
        public float TWidth()
        {
            return 8 * Projectile.scale;
        }
        public override void OnKill(int timeLeft)
        {
        }
        int Time;
        public override bool PreDraw(ref Color lightColor)
        {
            Item item = Projectile.Player().ActiveItem();
            if (Projectile.ai[1] < 2)
            {
                return false;
            }
            if (TrailDrawer == null)
            {
                TrailDrawer = new Trailing(new Trailing.VertexWidthFunction(TrailWidth), new Trailing.VertexColorFunction(TrailColor), null, GameShaders.Misc["刀光"]);
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
            if (Projectile.MeleeProj().DelayedKill <= 0)
            {
                Texture2D texture = TextureAssets.Item[item.type].Value;
                Vector2 Center = Projectile.Center - Main.screenPosition;
                if (Projectile.spriteDirection == 0)
                {
                    Main.spriteBatch.Draw(texture, Center, null, Lighting.GetColor((int)(Projectile.Player().Center.X / 16), (int)(Projectile.Player().Center.Y / 16), Color.White), Projectile.rotation, texture.Size() / 2, Projectile.scale, (SpriteEffects)Projectile.spriteDirection, 0f);
                }
                else
                {
                    Main.spriteBatch.Draw(texture, Center, null, Lighting.GetColor((int)(Projectile.Player().Center.X / 16), (int)(Projectile.Player().Center.Y / 16), Color.White), Projectile.rotation + MathHelper.PiOver2, texture.Size() / 2, Projectile.scale, (SpriteEffects)Projectile.spriteDirection, 0f);
                }
                if (SpecialAttack)
                {
                    for (int a = 0; a < Projectile.oldPos.Length; a+=3)
                    {
                        Center = Projectile.oldPos[a] +Projectile.Size/ 2 - Main.screenPosition;
                        if (Projectile.spriteDirection == 0)
                        {
                            Main.spriteBatch.Draw(texture, Center, null, new Color(170, 10, 255, 0)*(1- (float)a/Projectile.oldPos.Length), Projectile.oldRot[a], texture.Size() / 2, Projectile.scale, (SpriteEffects)Projectile.spriteDirection, 0f);
                            Main.spriteBatch.Draw(texture, Center, null, new Color(170, 10, 255, 0)*(1- (float)a/Projectile.oldPos.Length), Projectile.oldRot[a], texture.Size() / 2, Projectile.scale, (SpriteEffects)Projectile.spriteDirection, 0f);
                            Main.spriteBatch.Draw(texture, Center, null, new Color(170, 10, 255, 0)*(1- (float)a/Projectile.oldPos.Length), Projectile.oldRot[a], texture.Size() / 2, Projectile.scale, (SpriteEffects)Projectile.spriteDirection, 0f);
                        }
                        else
                        {
                            Main.spriteBatch.Draw(texture, Center, null, new Color(170, 10, 255, 0) * (1 - (float)a / Projectile.oldPos.Length), Projectile.oldRot[a] + MathHelper.PiOver2, texture.Size() / 2, Projectile.scale, (SpriteEffects)Projectile.spriteDirection, 0f);
                            Main.spriteBatch.Draw(texture, Center, null, new Color(170, 10, 255, 0) * (1 - (float)a / Projectile.oldPos.Length), Projectile.oldRot[a] + MathHelper.PiOver2, texture.Size() / 2, Projectile.scale, (SpriteEffects)Projectile.spriteDirection, 0f);
                            Main.spriteBatch.Draw(texture, Center, null, new Color(170, 10, 255, 0) * (1 - (float)a / Projectile.oldPos.Length), Projectile.oldRot[a] + MathHelper.PiOver2, texture.Size() / 2, Projectile.scale, (SpriteEffects)Projectile.spriteDirection, 0f);
                        }
                    }
                }
            }
            else
            {
                Projectile.alpha += 10;
            }
            return false;
        }
    }
}