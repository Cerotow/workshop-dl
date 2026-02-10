using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Dusts;
using DDmod.Content.Items;
using DDmod.Content.Items.Boss.Boss特殊;
using DDmod.Content.Items.Boss.天雷怒云;
using DDmod.Content.Items.Boss.流星破坏者;
using DDmod.Content.Items.Boss.特殊;
using DDmod.Content.Items.Boss.狱火蛇物品;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.Items.Melee.Sword.Make;
using DDmod.Content.Items.Melee.Sword.NPCLoot;
using DDmod.Content.Items.Series.Acorn;
using DDmod.NoContent.Config;
using DDmod.Players;
using Humanizer;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.UI;

namespace DDmod.Content.Projectiles.Melee.Sword
{
    public class GlobalSword : ModProjectile
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
            Projectile.extraUpdates = 8;
            Projectile.noEnchantments = true;
            ProjectileID.Sets.AllowsContactDamageFromJellyfish[Projectile.type] = true;
            Projectile.stopsDealingDamageAfterPenetrateHits = true;
        }
        public override bool? CanDamage()
        {
            return null;
        }
        public override bool? CanHitNPC(NPC target)
        {
            Item item = Projectile.Player().ActiveItem();
            if (item.type== 5129)
            {
                return true;
            }
            return base.CanHitNPC(target);
        }
        int proj = 0;
        float SprintChop = 0;
        bool Special;
        bool Special2;
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(proj);
            writer.Write(Special);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            proj = reader.ReadInt32();
            Special = reader.ReadBoolean();
        }
        public override bool PreAI()
        {
            Item item = Projectile.Player().ActiveItem();
            if(color==null|| color.Length<ItemLoader.ItemCount)
            {
                color = new Color[ItemLoader.ItemCount];
            }
            if (Main.netMode != 2&&color[item.type] == Color.Transparent)
            {
                Color[] colors = DDHelper.GetColors(TextureAssets.Item[item.type].Value);
                int a = 0;
                Vector4 vector4 = Vector4.Zero;
                for (int i = 0; i < colors.Length; i++)
                {
                    if (colors[i] != Color.Transparent)
                    {
                        a++;
                        vector4 += colors[i].ToVector4();
                    }
                }
                vector4 /= a;
                color[item.type] = new Color(vector4.X, vector4.Y, vector4.Z, (1 - (vector4.X + vector4.Y + vector4.Z) / 3) / 2);
                if (item.GetGlobalItem<MeleeGlobalItem>().Color != Color.Transparent)
                {
                    color[item.type] = item.GetGlobalItem<MeleeGlobalItem>().Color;
                }
            }
            float Length = item.GetGlobalItem<MeleeGlobalItem>().EffectLength;
            bool ZDY = Length > 0;
            Rectangle? sourceRect = new Rectangle?((Main.itemAnimations[item.type] == null) ? Utils.Frame(TextureAssets.Item[item.type], 1, 1, 0, 0, 0, 0) : Main.itemAnimations[item.type].GetFrame(TextureAssets.Item[item.type].Value, -1));

            if (ZDY)
            {
                Projectile.MeleeProj().oldVels2 = Length * 1.5f - 2;
            }
            else
            {
                Projectile.MeleeProj().oldVels2 = sourceRect.Value.Size().Length() / 2 * 1.5f - 2;
            }
            if (item.type == 1306)
            {
                Projectile.MeleeProj().oldVels2 += 4;
            }
            //大小加成
            Player player = Main.player[Projectile.owner];
            if (SprintChop == 0)
            {
                if (player.dashDelay < 0)
                {
                    if (Math.Abs(player.velocity.X) > 5)
                    {
                        SprintChop = Math.Abs(player.velocity.X) / 5;
                        //player.ChangeDir(player.Center.X - player.Dplayer().MouseWorld.X > 0 ? 1 : -1);
                    }
                }
                if (SprintChop == 0)
                {
                    SprintChop = -1;
                }
            }
            if (Projectile.scale < player.GetAdjustedItemScale(player.ActiveItem()))
            {
                Projectile.scale = player.GetAdjustedItemScale(player.ActiveItem());
            }
            if (ZDY)
            {
                Projectile.Resize((int)(sourceRect.Value.Width * Projectile.scale) / 10, (int)(Length * Projectile.scale * 1.2f));
                if (item.GetGlobalItem<MeleeGlobalItem>().SpecialAttack && !Special2 && player.controlUseTile && !Special && !Projectile.Player().HasBuff(ModContent.BuffType<SpecialAttackCD>()))
                {
                    Projectile.DProj().MouseWorld = player.Dplayer().MouseWorld;
                    Projectile.DProj().vector[2] = Projectile.DProj().MouseWorld - player.Center;
                    player.AddBuff(ModContent.BuffType<SpecialAttackCD>(), 180);
                    Projectile.damage = (int)(Projectile.damage * 2.25f);
                    Special = true;
                    if (item.type == ModContent.ItemType<C>())
                    {
                        Projectile.HoldProj(player, Length * Projectile.scale + 4, Projectile.ai[0], Projectile.DProj().vector[0], MathHelper.PiOver4, 0, true, 0, false, false);
                        Projectile.extraUpdates = 24;
                    }
                    Projectile.netUpdate = true;
                }
                else if (!Special)
                {
                    if (item.type == 1827 || item.type == 3013)
                    {
                        Projectile.extraUpdates = 24;
                    }
                    Projectile.HoldProj(player, Length * Projectile.scale + 4, Projectile.ai[0], Projectile.DProj().vector[0], MathHelper.PiOver4, 0, true, 0.05f * Projectile.DProj().Times[0], false, false);
                }
                else
                {
                    SpecialAttacks(sourceRect);
                }
            }
            else
            {
                int HE = sourceRect.Value.Height;
                if (sourceRect.Value.Width > HE)
                {
                    HE = sourceRect.Value.Width;
                }
                Projectile.Resize((int)(sourceRect.Value.Width * Projectile.scale) / 10, (int)(HE * Projectile.scale));
                if (item.GetGlobalItem<MeleeGlobalItem>().SpecialAttack && !Special2 && player.controlUseTile && !Special && !Projectile.Player().HasBuff(ModContent.BuffType<SpecialAttackCD>()))
                {
                    Projectile.DProj().MouseWorld = player.Dplayer().MouseWorld;
                    Projectile.DProj().vector[2] = Projectile.DProj().MouseWorld - player.Center;
                    player.AddBuff(ModContent.BuffType<SpecialAttackCD>(), 180);
                    Projectile.damage = (int)(Projectile.damage * 2.25f);
                    Special = true;
                    if (item.type == ModContent.ItemType<C>())
                    {
                        Projectile.HoldProj(player, Length * Projectile.scale + 4, Projectile.ai[0], Projectile.DProj().vector[0], MathHelper.PiOver4, 0, true, 0, false, false);
                        Projectile.extraUpdates = 24;
                    }
                    Projectile.netUpdate = true;
                }
                else if (!Special)
                {
                    if (item.type == 1827 || item.type == 3013)
                    {
                        Projectile.extraUpdates = 24;
                    }
                    else
                    if (item.type == 5284)
                    {
                        Projectile.extraUpdates = 4;
                    }
                    else
                    if (item.type == ModContent.ItemType<破坏者巨刃>())
                    {
                        Projectile.extraUpdates = 6;
                    }
                    Projectile.HoldProj(player, sourceRect.Value.Size().Length() / 2 * Projectile.scale + 4, Projectile.ai[0], Projectile.DProj().vector[0], MathHelper.PiOver4, 0, true, 0.05f * Projectile.DProj().Times[0], false, false);
                }
                else
                {
                    SpecialAttacks(sourceRect);
                }
            }
            Special2 = true;
            Projectile.HoldSword2(player, -player.HeldItem.useAnimation * (Projectile.extraUpdates + 1), -1, 2f, true);
            if (item.type == 3018)
            {
                if ((Projectile.DProj().Times[0] > 0 && player.direction > 0) || (Projectile.DProj().Times[0] < 0 && player.direction < 0))
                {
                    Projectile.rotation += 0.2F * player.direction;
                }
                else
                {
                    Projectile.rotation -= 0.2F * player.direction;
                }
                if (Projectile.localAI[1] < 0)
                    Projectile.MeleeProj().oldVels3 = 0.05F;
            }
            if (item.type == 5097)
            {
                if (Projectile.localAI[1] < 0)
                    Projectile.MeleeProj().oldVels3 = 0.05F;
            }
            if (item.type == 1909)
            {
                if ((Projectile.DProj().Times[0] > 0 && player.direction > 0) || (Projectile.DProj().Times[0] < 0 && player.direction < 0))
                {
                    Projectile.rotation += 0.2F * player.direction;
                }
                else
                {
                    Projectile.rotation -= 0.2F * player.direction;
                }
                if (Projectile.localAI[1] < 0)
                    Projectile.MeleeProj().oldVels3 = 0.05F;
            }
            if (item.type == ModContent.ItemType<寒冰巨剑>())
            {
                if (Projectile.localAI[1] < 0)
                    Projectile.MeleeProj().oldVels3 = 0.05F;
            }
            if (item.type == ModContent.ItemType<蠕虫毒牙>())
            {
                if (Projectile.localAI[1] < 0)
                    Projectile.MeleeProj().oldVels3 = -0.05F;
            }
            if (item.type == 3013 || item.type == 1827)
            {
                player.heldProj = -1;
            }
            //player.ChangeDir(player.Center.X - player.Dplayer().MouseWorld.X > 0 ? -1 : 1);
            int useTime = (int)(player.HeldItem.useTime / player.GetTotalAttackSpeed(DamageClass.Melee));
            if (Projectile.localAI[1] >= 0 && Projectile.ai[1] == 2 && Projectile.DProj().Times[2] < useTime)
            {
                Projectile.ai[1] = 3;

                if (item.type == 1327)
                {
                    PlaySound(SoundID.Item71, Projectile.position);
                }
                else
                {
                    PlaySound(SoundID.Item1, Projectile.position);
                }
            }
            if (Projectile.localAI[1] <= 0 || Projectile.MeleeProj().DelayedKill > 0)
            {
                proj = 0;
            }
            else
            {
                if (proj == 0 && !player.ActiveItem().GetGlobalItem<MeleeGlobalItem>().TrueMelee)
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
            if (!Special)
                DDSword.Useeffects(Projectile);
            Projectile.spriteDirection = Projectile.DProj().Times[0] > 0 ? 0 : 1;
            return false;
        }
        public void SpecialAttacks(Rectangle? sourceRect)
        {
            Item item = Projectile.Player().ActiveItem();
            proj++;
            Player player = Main.player[Projectile.owner];
            if (item.type == 1227)
            {
                if (proj % ((Projectile.extraUpdates + 1)) == 0)
                    DDPlayer.移动玩家(player, 30, true);
                if (proj % ((Projectile.extraUpdates + 1) / 2) == 0 && Main.myPlayer == Projectile.owner)
                {
                    NewProjectile(Projectile.GetSource_FromAI(), Projectile.Player().MountedCenter, Vector2.Zero, 228, Projectile.damage, 0);
                    //NewProjectile(Projectile.GetSource_FromAI(), Projectile.Player().MountedCenter+ Projectile.DProj().vector[2].PerfectNormalize()*2.5F * (Projectile.extraUpdates + 1), Vector2.Zero, 228, Projectile.damage, 0);

                }
                Projectile.HoldProj(player, sourceRect.Value.Size().Length() / 2 * Projectile.scale + 4, 0, Projectile.DProj().vector[0], MathHelper.PiOver4, 0, true, 0, false, false);
                player.position -= player.velocity;
                player.velocity.Y = -0.001f;
                player.velocity.X = 0;
                player.position += Collision.TileCollision(player.position, Projectile.DProj().vector[2].PerfectNormalize() * 5, player.width, player.height, true, true);
                int D = NewDust(player.MountedCenter - new Vector2(4) + Projectile.DProj().vector[2].PerfectNormalize() * 70 * Projectile.scale, 1, 1, ModContent.DustType<速度粒子>(), 0, 0, 100, new Color(100, 255, 100, 0), 3);
                Main.dust[D].velocity = Projectile.DProj().vector[2].PerfectNormalize() * 0.01F;
                Main.dust[D].rotation = Projectile.DProj().vector[2].ToRotation();
                Projectile.extraUpdates = 16;
                if (Collision.TileCollision(player.position, Projectile.DProj().vector[2].PerfectNormalize() * 5, player.width, player.height, true, true).Length() < 0.4F || proj > 150)
                {
                    Projectile.Kill();
                    player.itemAnimation = 0;
                    player.itemTime = 0;
                }
            }

        }
        
        public override void ModifyHitNPC(NPC target, ref HitModifiers modifiers)
        {
            
            Player player = Projectile.Player();
            Item item = Projectile.Player().ActiveItem();
            if (SprintChop > 0)
            {
                modifiers.FinalDamage += SprintChop;
                //damage += (int)(damage * SprintChop);
                if (target.knockBackResist != 0)
                    modifiers.Knockback += SprintChop;
            }
            float armorPenetrationPercent = 0f;

            if (item.type == 5129 && target.isLikeATownNPC)
            {
                armorPenetrationPercent = 1f;
                if (target.type == 18)
                    modifiers.TargetDamageMultiplier *= 2;
            }

            modifiers.ArmorPenetration += player.GetWeaponArmorPenetration(item);
            modifiers.ScalingArmorPenetration += armorPenetrationPercent;
            ItemLoader.ModifyHitNPC(item, Projectile.Player(), target, ref modifiers);
        }
        float DS = 0;
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            DS++;
            Player player = Projectile.Player();
            Item item = Projectile.Player().ActiveItem();
            if (item.type == 5284)
            {
                target.AddBuff(24, 120);
            }
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
            if (player.ActiveItem().type == ItemID.LightsBane)
            {
                return;
            }
            Vector2 vector = Main.rand.NextVector2Unit() * 60;
            if (item.type == ModContent.ItemType<阴影剑>())
            {

                int A = NewProjectile(Projectile.GetSource_FromThis(), target.Center, -vector / 10, ModContent.ProjectileType<GlobalSlash>(), 0, 0, Projectile.owner, target.whoAmI, 1);
                Main.projectile[A].DProj().Ecolor = new Color(255, 1, 1, 255);
                Main.projectile[A].localAI[0] = 0.6F;
                Main.projectile[A].localAI[1] = 1F;
                Main.projectile[A].localAI[2] = 0F;
                Main.projectile[A].scale = 0.5F;
                A = NewProjectile(Projectile.GetSource_FromThis(), target.Center, -vector / 10, ModContent.ProjectileType<GlobalSlash>(), 0, 0, Projectile.owner, target.whoAmI, 1);
                Main.projectile[A].DProj().Ecolor = new Color(1, 1, 1, 255);
                Main.projectile[A].localAI[0] = 0.3F;
                Main.projectile[A].localAI[1] = 0.5F;
                Main.projectile[A].localAI[2] = 0F;
                Main.projectile[A].scale = 0.25F;
                return;
            }

            if (item.type != 5284 && item.type != ModContent.ItemType<AcornSword>() && item.type != ModContent.ItemType<GelSword>())
            {
                int A = NewProjectile(Projectile.GetSource_FromThis(), target.Center, -vector / 10, ModContent.ProjectileType<GlobalSlash>(), 0, 0, Projectile.owner, target.whoAmI, 1);
                Main.projectile[A].DProj().color = color[item.type];
                //Main.NewText(color[item.type]);
                if (SprintChop > 0)
                {
                    Main.projectile[A].scale += SprintChop / 2;
                    if (target.knockBackResist != 0)
                    {
                        target.velocity.Y -= 3;
                    }
                    Main.LocalPlayer.Dplayer().PlayerShake(5, 10);
                    player.immune = true;
                    player.hurtCooldowns[1] = 30;
                    player.immuneTime = 30;
                    player.immuneNoBlink = true;
                    if (Projectile.Player().dashType != 2)
                    {
                        Projectile.Player().velocity.X = (target.Center - player.Center).PerfectNormalize().X * -SprintChop;
                        if (Projectile.Player().velocity.Y != 0)
                        {
                            Projectile.Player().velocity.Y = -SprintChop;
                        }
                    }
                    SprintChop = -1;
                    DDmod.SyncData(DDType.PlayerCenter, player.whoAmI, -1, player.whoAmI);
                }
                if (item.GetGlobalItem<MeleeGlobalItem>().Color2!=Color.Transparent)
                {
                    Main.projectile[A].DProj().Ecolor = item.GetGlobalItem<MeleeGlobalItem>().Color;
                    Main.projectile[A].DProj().Ecolor2 = item.GetGlobalItem<MeleeGlobalItem>().Color2;
                }
                if (item.type == 1327)
                {
                    Main.projectile[A].DProj().color = new Color(170, 10, 255, 0) * 0.5f;
                }
            }

        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            Item item = Projectile.Player().ActiveItem();
            if (item.type == 3013 || item.type == 1827)
            {
                overPlayers.Add(index);
            }
        }

        public static Color[] color;
        public float TWidth()
        {
            Item item = Projectile.Player().ActiveItem();
            if (item.type == 1306)
            {
                return 9;
            }
            float Length = item.GetGlobalItem<MeleeGlobalItem>().EffectLength;
            bool ZDY = Length > 0;
            if (ZDY)
            {
                return Length / 2 + 4;
            }
            else
            {
                Rectangle? sourceRect = new Rectangle?((Main.itemAnimations[item.type] == null) ? Utils.Frame(TextureAssets.Item[item.type], 1, 1, 0, 0, 0, 0) : Main.itemAnimations[item.type].GetFrame(TextureAssets.Item[item.type].Value, -1));

                return sourceRect.Value.Size().Length() / 4 + 4;
            }
        }
        public override void OnKill(int timeLeft)
        {
        }
        int Time;
        public override bool PreDraw(ref Color lightColor)
        {
            Item item = Projectile.Player().ActiveItem();
            Player player = Projectile.Player();
            Vector2 Velocity = Utils.RotatedBy(Vector2.Normalize(Projectile.velocity), 0, default);
            Vector2 cen = Vector2.Zero;

            ItemSlot.GetItemLight(ref lightColor, ref Projectile.scale, item, false);
            lightColor = player.GetImmuneAlpha(item.GetAlpha(lightColor) * player.stealth, 0f);
            Rectangle? sourceRect = new Rectangle?((Main.itemAnimations[item.type] == null) ? Utils.Frame(TextureAssets.Item[item.type], 1, 1, 0, 0, 0, 0) : Main.itemAnimations[item.type].GetFrame(TextureAssets.Item[item.type].Value, -1));

            if (player.mount.Active)
            {
                Projectile.Center = player.RotatedRelativePoint(player.ArmCenter() - cen, reverseRotation: false, addGfxOffY: false) + Velocity * (sourceRect.Value.Size().Length() / 2 * Projectile.scale + 4) + new Vector2(0, player.gfxOffY);

            }
            else
            {
                Projectile.Center = player.ArmCenter() - cen + Velocity * (sourceRect.Value.Size().Length() / 2 * Projectile.scale + 4) + new Vector2(0, player.gfxOffY);
            }
            if (Projectile.ai[1] < 2)
            {
                return false;
            }
            Vector2 vector = Projectile.Player().ArmCenter();
            if (Projectile.MeleeProj().oldPlayer != Vector2.Zero)
            {
                vector = Projectile.MeleeProj().oldPlayer;
            }
            //白蜡木剑
            if (item.type != 5284)
            {
                Color color2 = color[item.type];
                if (item.DItem().HandheldColor == Color.Transparent && item.GetGlobalItem<MeleeGlobalItem>().ColorL == null)
                {
                    color2 = Lighting.GetColor((int)(Projectile.Player().Center.X / 16), (int)(Projectile.Player().Center.Y / 16), color2);
                    color2.A = color[item.type].A;
                }
                DDHelper.BladeTrail(DDTextures.WhitePng, color2, 1F, Projectile.DProj().Times[0] > 0);
                TrailDrawer.Draw(Projectile.MeleeProj().oldVels, vector - Main.screenPosition, 88, null, Projectile.scale * TWidth());
                if (item.GetGlobalItem<MeleeGlobalItem>().Color2 != Color.Transparent)
                {
                    color2 = Lighting.GetColor((int)(Projectile.Player().Center.X / 16), (int)(Projectile.Player().Center.Y / 16), item.GetGlobalItem<MeleeGlobalItem>().Color2);
                    color2.A = item.GetGlobalItem<MeleeGlobalItem>().Color2.A;
                }
                if (item.type == ModContent.ItemType<雷霆剑>())
                {
                    DDHelper.BladeTrail(DDTextures.Wave, color2, 1F, Projectile.DProj().Times[0] > 0);
                }
                else
                {
                    DDHelper.BladeTrail(DDTextures.Wave, color2, 1F, Projectile.DProj().Times[0] > 0);
                    
                }
                TrailDrawer.Draw(Projectile.MeleeProj().oldVels, vector - Main.screenPosition, 88, null, Projectile.scale * TWidth());
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            if (Projectile.MeleeProj().DelayedKill <= 0)
            {

                Texture2D texture = TextureAssets.Item[item.type].Value;
                Vector2 Center = Projectile.Center - Main.screenPosition;
                if (item.DItem().HandheldColor != Color.Transparent)
                {
                    lightColor = item.DItem().HandheldColor;
                }
                if (Projectile.spriteDirection == 0)
                {
                    Main.spriteBatch.Draw(texture, Center, sourceRect.Value, lightColor, Projectile.rotation, sourceRect.Value.Size() / 2, Projectile.scale, (SpriteEffects)Projectile.spriteDirection, 0f);
                }
                else
                {
                    Main.spriteBatch.Draw(texture, Center, sourceRect.Value, lightColor, Projectile.rotation + MathHelper.PiOver2, sourceRect.Value.Size() / 2, Projectile.scale, (SpriteEffects)Projectile.spriteDirection, 0f);
                }
                if (item.type > 0 && item.glowMask != -1)
                {
                    lightColor = new Color(255, 255, 255, item.alpha);
                    texture = TextureAssets.GlowMask[(int)item.glowMask].Value;
                    if (Projectile.spriteDirection == 0)
                    {
                        Main.spriteBatch.Draw(texture, Center, sourceRect.Value, lightColor, Projectile.rotation, sourceRect.Value.Size() / 2, Projectile.scale, (SpriteEffects)Projectile.spriteDirection, 0f);
                    }
                    else
                    {
                        Main.spriteBatch.Draw(texture, Center, sourceRect.Value, lightColor, Projectile.rotation + MathHelper.PiOver2, sourceRect.Value.Size() / 2, Projectile.scale, (SpriteEffects)Projectile.spriteDirection, 0f);
                    }
                }
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            }
            else
            {
                Projectile.alpha += 10;
            }
            return false;
        }
    }
    public class GlobalSlash : ModProjectile
    {
        public override string Texture => "DDmod/Image/VoidStar";
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Slash");
            //DisplayName.AddTranslation(7, "斩");
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
            Projectile.scale = 1f;
            Projectile.ArmorPenetration = 100000;
            Projectile.localAI[0] = 1;
            Projectile.localAI[1] = 1;
            Projectile.localAI[2] = 0.02F;
        }
        public override void AI()
        {
            Projectile.scale = Projectile.ai[2];
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.Resize((int)(Projectile.OriginalWidth() * Projectile.scale), (int)(Projectile.OriginalHeight() * (0.2f + Main.npc[(int)Projectile.ai[0]].Size.Length() / 100)));
            if (Projectile.velocity.Length() > 0.1f)
                Projectile.velocity = Projectile.velocity.PerfectNormalize() * 0.09F;
            Projectile.timeLeft = 20;

            Projectile.localAI[0] -= 0.03f * Projectile.localAI[1] + Projectile.localAI[2];
            Projectile.Center = Main.npc[(int)Projectile.ai[0]].Center + Projectile.DProj().vector[0];
            Projectile.active = Main.npc[(int)Projectile.ai[0]].active;
            if (Projectile.localAI[0] < 0F)
            {
                Projectile.Kill();
            }
        }
        public override bool? CanHitNPC(NPC target)
        {
            if (target.whoAmI == Projectile.ai[0])
            {
                return null;
            }
            return false;
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
            Color color = Projectile.DProj().color*0.8F;
            Vector2 vector = new Vector2(Projectile.width, Projectile.height) / 2;

            if (Projectile.ai[1] >= 2)
            {
                return false;
            }
            if (Projectile.DProj().Ecolor == default)
            {
                for (int a = 0; a < 1; a++)
                {
                    
                    Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, color * 1f, Projectile.rotation, texture.Size() / 2, new Vector2(Projectile.localAI[0], (1f + Projectile.scale)) * (0.5F + (1 - Projectile.localAI[0]) * 2), spriteEffects, 0f);
                    
                    //Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, color.Opposite() * 1f, Projectile.rotation, texture.Size() / 2, new Vector2(Projectile.localAI[0], (1f + Projectile.scale)) * (0.5F + (1 - Projectile.localAI[0]) * 2), spriteEffects, 0f);
                    //Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, color * 4.4f, Projectile.rotation, texture.Size() / 2, new Vector2(0.6f, Projectile.scale / 16), spriteEffects, 0f);

                }
            }
            else
            {
                color = Projectile.DProj().Ecolor;
                for (int a = 0; a < 1; a++)
                {
                    Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, color, Projectile.rotation, texture.Size() / 2, new Vector2(Projectile.localAI[0], (1f + Projectile.scale)) * (0.5F + (1 - Projectile.localAI[0]) * 2), spriteEffects, 0f);
                    color = Projectile.DProj().Ecolor2;
                    Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, color, Projectile.rotation, texture.Size() / 2, new Vector2(Projectile.localAI[0], (1f + Projectile.scale)) * (0.5F + (1 - Projectile.localAI[0]) * 2), spriteEffects, 0f);
                    //Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, color * 4.4f, Projectile.rotation, texture.Size() / 2, new Vector2(0.6f, Projectile.scale / 16), spriteEffects, 0f);

                }

            }

            return false;
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            if (Projectile.ai[1] == 1 && Projectile.damage <= 0)
            {
                overPlayers.Add(index);
            }
        }
    }
    public static class DDSword
    {
        public static void Useeffects(Projectile Projectile)
        {
            Item item = Projectile.Player().ActiveItem();
            bool Bool = Projectile.owner == Main.myPlayer;
            if (item.type == ModContent.ItemType<海爵之咬>() && Bool)
            {
                for (int A = -Projectile.height / 2; A < Projectile.height / 2; A += (int)(4 * Projectile.scale))
                {
                    if (Main.myPlayer == Projectile.owner && Main.rand.NextBool(300) && Projectile.localAI[1] >= 1)
                    {
                        NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center + Projectile.velocity.PerfectNormalize() * (A + 10 * Projectile.scale) + new Vector2(Main.rand.Next(Projectile.height / 4), Main.rand.Next(Projectile.height / 4)), Projectile.oldVelocity, 405, Projectile.damage / 4, 0, -1, -20);
                    }
                }
            }
            if (item.type == ModContent.ItemType<破坏者巨刃>())
            {
                for (int A = -Projectile.height / 2; A < Projectile.height / 2; A += (int)(4 * Projectile.scale))
                {
                    if (Main.rand.NextBool(30) && Projectile.localAI[1] >= 1)
                    {
                        int Type = 6;
                        if(Main.rand.NextBool(10))
                        {
                            Type= ModContent.DustType<方块粒子>();
                        }
                        Dust dust = Main.dust[NewDust(Projectile.Center + Projectile.velocity.PerfectNormalize() * (A + 10 * Projectile.scale), Projectile.height / 4, Projectile.height / 4, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                        dust.noGravity = true;
                        dust.velocity = (Projectile.rotation - MathHelper.PiOver4).ToRotationVector2() * 2;
                        dust.scale = Main.rand.NextFloat(1F, 2F);
                        if (Type == ModContent.DustType<方块粒子>())
                        {
                            dust.scale /= 2;
                            dust.customData = 2F;
                            dust.color = new Color(253, 62, 3,0);
                        }
                    }
                }
            }
            if (item.type == ModContent.ItemType<火山长剑>())
            {
                for (int A = -Projectile.height / 2; A < Projectile.height / 2; A += (int)(4 * Projectile.scale))
                {
                    if (Main.rand.NextBool(30) && Projectile.localAI[1] >= 1)
                    {
                        int Type = 6;
                        Dust dust = Main.dust[NewDust(Projectile.Center + Projectile.velocity.PerfectNormalize() * (A + 10 * Projectile.scale), Projectile.height / 4, Projectile.height / 4, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                        dust.noGravity = true;
                        dust.velocity = (Projectile.rotation - MathHelper.PiOver4).ToRotationVector2() * 2;
                        dust.scale = Main.rand.NextFloat(1F, 2F);
                    }
                }
            }
            if (item.type == 1227)
            {
                if (Main.myPlayer == Projectile.owner && Projectile.localAI[1] >= 1 && Main.rand.NextBool(50))
                    NewProjectile(Projectile.GetSource_FromAI(), Projectile.Player().MountedCenter + Projectile.velocity.PerfectNormalize() * Main.rand.NextFloat(30F, 106F) * Projectile.scale, Projectile.velocity.PerfectNormalize() * Main.rand.NextFloat(5F, 12F), 228, Projectile.damage, 0);
            }
            for (int A = -Projectile.height / 2; A < Projectile.height / 2; A += (int)(4 * Projectile.scale))
            {
                if (Main.rand.NextBool(30) && Projectile.localAI[1] >= 1 && (Projectile.Player().magmaStone || item.type == 5284))
                {
                    int Type = 6;
                    Dust dust = Main.dust[NewDust(Projectile.Center + Projectile.velocity.PerfectNormalize() * (A + 10 * Projectile.scale), Projectile.height / 4, Projectile.height / 4, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                    dust.noGravity = true;
                    dust.velocity = (Projectile.rotation - MathHelper.PiOver4).ToRotationVector2() * 3;
                    dust.scale = Main.rand.NextFloat(1F, 2F);
                }
            }
        }
    }

}