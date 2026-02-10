using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Dusts;
using DDmod.Content.Items;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.Projectiles.Magic;
using DDmod.Players;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader.IO;
using Terraria.UI;
using static Terraria.Player;

namespace DDmod.Content.Projectiles
{
    public class DDGlobalProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public int track;
        /// <summary>减速频率</summary>
        public float Speed = 1;
        public float SpeedTime = 0;
        public float SpeedScope = 0;
        /// <summary>倍率</summary>
        public float Magnification = 1;
        /// <summary>
        /// 手持武器
        /// </summary>
        public Player HeldItem;

        /// <summary>穿透保护机制,一般用于Boss蠕虫</summary>
        public float PenetrationProtection = 1;
        /// <summary>控制弹幕时会影响屏幕位置使用</summary>
        public bool Detect;
        /// <summary>重点同步对象</summary>
        public bool Sync;
        /// <summary>正前负后</summary>
        public int Back = 1;
        /// <summary>双持另一把武器</summary>
        public int Other = -1;

        public Vector2[] vector = new Vector2[3];
        public bool[] Bool = new bool[5];

        public float[] Times = new float[5];
        public List<byte> NPCW;

        public Vector2 MouseWorld;

        public Color color;
        public Color Ecolor;
        public Color Ecolor2;
        public static Asset<Texture2D>[] Glow;
        public static Asset<Texture2D> 咒火弹;
        public static float[] ScaleGlow;
        public static Color[] GlowColor;
        public float ArmorReduction =1; 
        public Vector2 PrePosition;
        /// <summary>
        /// 秦始皇位置
        /// </summary>
        public Vector2 PreviousPosition;
        public static Trailing TrailDrawer;
        public static Color TrailColor(float completionRatio)
        {
            return Color.Transparent;
        }
        public static float TrailWidth(float completionRatio)
        {
            return 1;
        }
        public override void Load()
        {
            咒火弹 = ModContent.Request<Texture2D>("DDmod/Content/Projectiles/咒火弹");
        }
        public override void SetStaticDefaults()
        {
            if (Glow == null)
            {
                Glow = new Asset<Texture2D>[ProjectileLoader.ProjectileCount];
            }
            if (Main.netMode != 2)
            {
                Glow[275] = Glow[276] = ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Projectile_276_Glow");
                Glow[277] = ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Projectile_277_Glow");
            }
            if (ScaleGlow == null)
            {
                ScaleGlow = new float[ProjectileLoader.ProjectileCount];
            }
            if (GlowColor == null)
            {
                GlowColor = new Color[ProjectileLoader.ProjectileCount];
            }
        }
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            EntitySource_Parent entitySource_Parent = source as EntitySource_Parent;
            if (entitySource_Parent != null)
            {
                Projectile proj = entitySource_Parent.Entity as Projectile;
                if (proj != null)
                {
                    projectile.DProj().PenetrationProtection = proj.DProj().PenetrationProtection;
                }
            }
            if (entitySource_Parent != null)
            {
               NPC npc= entitySource_Parent.Entity as NPC;
                if (npc != null)
                {
                    projectile.DProj().ArmorReduction = npc.Dnpc().ArmorReduction;
                }
            }
        }

        public override void SetDefaults(Projectile projectile)
        {
            if (projectile.type==569|| projectile.type == 570|| projectile.type == 571)
            {
                projectile.usesLocalNPCImmunity = true;
                projectile.localNPCHitCooldown = 20;
            }
            if(projectile.type==221)
            {
                projectile.usesLocalNPCImmunity = true;
                projectile.localNPCHitCooldown = 30;
            }
            if(projectile.type==96)
            {
                ProjectileID.Sets.TrailingMode[projectile.type] = 0;
                ProjectileID.Sets.TrailCacheLength[projectile.type] = 8;
            }
            if(projectile.type==275|| projectile.type == 276|| projectile.type == 277)
            {
                ProjectileID.Sets.TrailingMode[projectile.type] = 2;
                ProjectileID.Sets.TrailCacheLength[projectile.type] = 8;
            }
            if(projectile.type==258)
            {
                ProjectileID.Sets.TrailingMode[projectile.type] = 2;
                ProjectileID.Sets.TrailCacheLength[projectile.type] = 12;
            }
        }
        public override void SendExtraAI(Projectile projectile, BitWriter bitWriter, BinaryWriter binaryWriter)
        {
            binaryWriter.Write(projectile.scale);
            // 1. 第一个字节：所有布尔+localAI标记+Times标记的一部分
            byte flags1 = 0;
            flags1 |= (byte)((Bool[0] ? 1 : 0) << 0);
            flags1 |= (byte)((Bool[1] ? 1 : 0) << 1);
            flags1 |= (byte)((Bool[2] ? 1 : 0) << 2);
            flags1 |= (byte)((Bool[3] ? 1 : 0) << 3);
            flags1 |= (byte)((Bool[4] ? 1 : 0) << 4);
            flags1 |= (byte)((projectile.localAI[0] != 0f ? 1 : 0) << 5);
            flags1 |= (byte)((projectile.localAI[1] != 0f ? 1 : 0) << 6);
            flags1 |= (byte)((Times[0] != 0f ? 1 : 0) << 7); // 借用第7位给Times[0]
            binaryWriter.Write(flags1);

            // 2. 按标记发送localAI
            if ((flags1 & 32) != 0) binaryWriter.Write(projectile.localAI[0]);
            if ((flags1 & 64) != 0) binaryWriter.Write(projectile.localAI[1]);

            // 3. 第二个字节：Times标记+Vector2标记
            byte flags2 = 0;
            flags2 |= (byte)((Times[1] != 0f ? 1 : 0) << 0);
            flags2 |= (byte)((Times[2] != 0f ? 1 : 0) << 1);
            flags2 |= (byte)((Times[3] != 0f ? 1 : 0) << 2);
            flags2 |= (byte)((Times[4] != 0f ? 1 : 0) << 3);
            flags2 |= (byte)((vector[0].X != 0f ? 1 : 0) << 4);
            flags2 |= (byte)((vector[0].Y != 0f ? 1 : 0) << 5);
            flags2 |= (byte)((vector[1].X != 0f ? 1 : 0) << 6);
            flags2 |= (byte)((vector[1].Y != 0f ? 1 : 0) << 7);
            binaryWriter.Write(flags2);

            // 4. 发送Times和Vector2[0-1]
            if ((flags1 & 128) != 0) binaryWriter.Write(Times[0]);
            if ((flags2 & 1) != 0) binaryWriter.Write(Times[1]);
            if ((flags2 & 2) != 0) binaryWriter.Write(Times[2]);
            if ((flags2 & 4) != 0) binaryWriter.Write(Times[3]);
            if ((flags2 & 8) != 0) binaryWriter.Write(Times[4]);
            if ((flags2 & 16) != 0) binaryWriter.Write(vector[0].X);
            if ((flags2 & 32) != 0) binaryWriter.Write(vector[0].Y);
            if ((flags2 & 64) != 0) binaryWriter.Write(vector[1].X);
            if ((flags2 & 128) != 0) binaryWriter.Write(vector[1].Y);

            // 5. 第三个字节：Vector2[2]+MouseWorld+整数大小标记
            byte flags3 = 0;
            flags3 |= (byte)((vector[2].X != 0f ? 1 : 0) << 0);
            flags3 |= (byte)((vector[2].Y != 0f ? 1 : 0) << 1);
            flags3 |= (byte)((MouseWorld.X != 0f ? 1 : 0) << 2);
            flags3 |= (byte)((MouseWorld.Y != 0f ? 1 : 0) << 3);
            flags3 |= (byte)((Back > 127 || Back < -128 ? 1 : 0) << 4);
            flags3 |= (byte)((Other > 127 || Other < -128 ? 1 : 0) << 5);
            flags3 |= (byte)((track > 32767 || track < -32768 ? 1 : 0) << 6);
            flags3 |= (byte)((projectile.localAI[2] != 0f ? 1 : 0) << 7); // localAI[2]放在这里
            binaryWriter.Write(flags3);

            // 6. 发送剩余数据
            if ((flags3 & 1) != 0) binaryWriter.Write(vector[2].X);
            if ((flags3 & 2) != 0) binaryWriter.Write(vector[2].Y);
            if ((flags3 & 4) != 0) binaryWriter.Write(MouseWorld.X);
            if ((flags3 & 8) != 0) binaryWriter.Write(MouseWorld.Y);
            if ((flags3 & 128) != 0) binaryWriter.Write(projectile.localAI[2]);

            // 7. 动态整数（直接写入，不额外标记）
            if ((flags3 & 16) != 0) binaryWriter.Write((short)Back); else binaryWriter.Write((sbyte)Back);
            if ((flags3 & 32) != 0) binaryWriter.Write((short)Other); else binaryWriter.Write((sbyte)Other);
            if ((flags3 & 64) != 0) binaryWriter.Write(track); else binaryWriter.Write((short)track);

            // 8. 颜色压缩（假设颜色经常为0）
            byte colorFlags = 0;
            colorFlags |= (byte)((color.R != 0 ? 1 : 0) << 0);
            colorFlags |= (byte)((color.G != 0 ? 1 : 0) << 1);
            colorFlags |= (byte)((color.B != 0 ? 1 : 0) << 2);
            colorFlags |= (byte)((color.A != 255 ? 1 : 0) << 3);
            binaryWriter.Write(colorFlags);

            if ((colorFlags & 1) != 0) binaryWriter.Write(color.R);
            if ((colorFlags & 2) != 0) binaryWriter.Write(color.G);
            if ((colorFlags & 4) != 0) binaryWriter.Write(color.B);
            if ((colorFlags & 8) != 0) binaryWriter.Write(color.A);

            // 9. NPCW极限压缩
            int npcwCount = NPCW?.Count ?? 0;
            if (npcwCount == 0)
            {
                binaryWriter.Write((byte)0); // 只有一个字节表示空列表
            }
            else
            {
                // 用第一个byte同时表示"有数据"和数量大小
                byte npcwHeader = (byte)(npcwCount > 255 ? 2 : 1);
                if (npcwCount > 65535) npcwHeader = 4;
                npcwHeader |= 0x80; // 最高位标记"有数据"
                binaryWriter.Write(npcwHeader);

                // 写入数量
                if (npcwCount <= 255) binaryWriter.Write((byte)npcwCount);
                else if (npcwCount <= 65535) binaryWriter.Write((ushort)npcwCount);
                else binaryWriter.Write(npcwCount);

                // 直接写入byte数组（已经很小了）
                foreach (byte b in NPCW)
                    binaryWriter.Write(b);
            }
        }

        public override void ReceiveExtraAI(Projectile projectile, BitReader bitReader, BinaryReader binaryReader)
        {
            projectile.scale = binaryReader.ReadSingle();
            // 1. 读取第一个标记
            byte flags1 = binaryReader.ReadByte();
            Bool[0] = (flags1 & 1) != 0;
            Bool[1] = (flags1 & 2) != 0;
            Bool[2] = (flags1 & 4) != 0;
            Bool[3] = (flags1 & 8) != 0;
            Bool[4] = (flags1 & 16) != 0;

            // 2. 读取localAI
            projectile.localAI[0] = (flags1 & 32) != 0 ? binaryReader.ReadSingle() : 0f;
            projectile.localAI[1] = (flags1 & 64) != 0 ? binaryReader.ReadSingle() : 0f;

            // 3. 读取第二个标记
            byte flags2 = binaryReader.ReadByte();

            // 4. 读取Times和Vector2[0-1]
            Times[0] = (flags1 & 128) != 0 ? binaryReader.ReadSingle() : 0f;
            Times[1] = (flags2 & 1) != 0 ? binaryReader.ReadSingle() : 0f;
            Times[2] = (flags2 & 2) != 0 ? binaryReader.ReadSingle() : 0f;
            Times[3] = (flags2 & 4) != 0 ? binaryReader.ReadSingle() : 0f;
            Times[4] = (flags2 & 8) != 0 ? binaryReader.ReadSingle() : 0f;
            vector[0].X = (flags2 & 16) != 0 ? binaryReader.ReadSingle() : 0f;
            vector[0].Y = (flags2 & 32) != 0 ? binaryReader.ReadSingle() : 0f;
            vector[1].X = (flags2 & 64) != 0 ? binaryReader.ReadSingle() : 0f;
            vector[1].Y = (flags2 & 128) != 0 ? binaryReader.ReadSingle() : 0f;

            // 5. 读取第三个标记
            byte flags3 = binaryReader.ReadByte();

            // 6. 读取剩余数据
            vector[2].X = (flags3 & 1) != 0 ? binaryReader.ReadSingle() : 0f;
            vector[2].Y = (flags3 & 2) != 0 ? binaryReader.ReadSingle() : 0f;
            MouseWorld.X = (flags3 & 4) != 0 ? binaryReader.ReadSingle() : 0f;
            MouseWorld.Y = (flags3 & 8) != 0 ? binaryReader.ReadSingle() : 0f;
            projectile.localAI[2] = (flags3 & 128) != 0 ? binaryReader.ReadSingle() : 0f;

            // 7. 读取动态整数
            Back = (flags3 & 16) != 0 ? binaryReader.ReadInt16() : binaryReader.ReadSByte();
            Other = (flags3 & 32) != 0 ? binaryReader.ReadInt16() : binaryReader.ReadSByte();
            track = (flags3 & 64) != 0 ? binaryReader.ReadInt32() : binaryReader.ReadInt16();

            // 8. 读取颜色
            byte colorFlags = binaryReader.ReadByte();
            color.R = (colorFlags & 1) != 0 ? binaryReader.ReadByte() : (byte)0;
            color.G = (colorFlags & 2) != 0 ? binaryReader.ReadByte() : (byte)0;
            color.B = (colorFlags & 4) != 0 ? binaryReader.ReadByte() : (byte)0;
            color.A = (colorFlags & 8) != 0 ? binaryReader.ReadByte() : (byte)255;

            // 9. 读取NPCW
            byte npcwHeader = binaryReader.ReadByte();
            if ((npcwHeader & 0x80) != 0)
            {
                int count = (npcwHeader & 3) switch
                {
                    1 => binaryReader.ReadByte(),
                    2 => binaryReader.ReadUInt16(),
                    _ => binaryReader.ReadInt32()
                };
                NPCW = new List<byte>(count);
                for (int i = 0; i < count; i++)
                    NPCW.Add(binaryReader.ReadByte());
            }
            else
            {
                NPCW = new List<byte>();
            }
        }
        public override void AI(Projectile projectile)
        {
        }
        
        public override bool PreAI(Projectile projectile)
        {
            if(track==0)
            {
                projectile.netUpdate = true;
            }
            track++;
            if (track % (projectile.extraUpdates+1) == 0)
            {
                if (PenetrationProtection < 1)
                {
                    PenetrationProtection += 0.001f;
                    if (projectile.minion)
                    {
                        PenetrationProtection += 0.02F;
                    }
                }
                else
                {
                    PenetrationProtection = 1;
                }
            }
            if (projectile.aiStyle == 7)
            {
                if (projectile.Player().ActiveItem().holdStyle == 2)
                    projectile.Player().SetCompositeArmBack(true, (CompositeArmStretchAmount)3, (projectile.Player().ArmCenter() - projectile.Center).ToRotation() + MathHelper.PiOver2 - projectile.Player().fullRotation);
                if(projectile.Player().PlayerAction().Jump)
                {
                    projectile.Kill();
                }
                if (projectile.Player().ActiveItem().type > 0 )
                {
                    if(projectile.Player().ActiveItem().GetGlobalItem<RangedGlobalItem>().Bow|| projectile.Player().ActiveItem().holdStyle > 0|| projectile.Player().ActiveItem().DItem().Twin)
                    return true;
                }
                projectile.Player().SetCompositeArmBack(true, (CompositeArmStretchAmount)3, (projectile.Player().ArmCenter() - projectile.Center).ToRotation() + MathHelper.PiOver2 - projectile.Player().fullRotation);

            }
            if (projectile.type == 96)
            {
                color = new Color(191, 255, 45, 0);
                for (int a = 0; a < 2; a++)
                {
                    int A = NewDust(projectile.position - new Vector2(5), projectile.width + 10, projectile.height + 10, 75, 0, 0, 100, new Color(191, 255, 45, 0), Main.rand.NextFloat(1F, 1.8F));
                    Main.dust[A].velocity = -projectile.velocity.PerfectNormalize() * 2;
                    Main.dust[A].rotation = projectile.velocity.ToRotation();
                    Main.dust[A].customData = 1F;
                    Main.dust[A].noGravity = true;
                }
                for (int a = 0; a < 2; a++)
                {
                    int A = NewDust(projectile.position-new Vector2(5), projectile.width+10, projectile.height+10, ModContent.DustType<速度粒子>(), 0, 0, 100, new Color(191, 255, 45, 0), Main.rand.NextFloat(0.8F, 1.3F));
                    Main.dust[A].velocity = -projectile.velocity.PerfectNormalize() * 2;
                    Main.dust[A].rotation = projectile.velocity.ToRotation();
                    Main.dust[A].customData = 1F;
                    Main.dust[A].noGravity = true;
                }
                projectile.rotation = projectile.velocity.ToRotation();
                return false;
            }
            if (projectile.type == 101)
            {
                if (projectile.timeLeft > 60)
                    projectile.timeLeft = 60;
                projectile.ai[1]+=projectile.velocity.Length();
                projectile.ai[2]++;
                //color = new Color(255, 97, 4, 0);
                color = new Color(191, 255, 45, 100);
                if (projectile.ai[1]>=72)
                {
                    if (projectile.ai[2] % 2 == 0)
                    {
                        for (int a = 0; a < 1; a++)
                        {
                            int A = NewDust(projectile.position - new Vector2(5), projectile.width + 10, projectile.height + 10, ModContent.DustType<光球粒子>(), 0, 0, 100, color, Main.rand.NextFloat(1.2F, 3F));
                            Main.dust[A].velocity = projectile.velocity.PerfectNormalize() * 2;
                            Main.dust[A].rotation = projectile.velocity.ToRotation();
                            Main.dust[A].customData = Main.dust[A].DustAI(2)+2;
                            Main.dust[A].noGravity = true;
                        }
                    }
                    if (Main.rand.NextBool(12))
                    {
                        for (int a = 0; a < 1; a++)
                        {
                            int A = NewDust(projectile.Center - new Vector2(4), 0, 0, ModContent.DustType<速度粒子>(), 0, 0, 100, color, Main.rand.NextFloat(1.2F, 3F));
                            Main.dust[A].velocity = projectile.velocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)) * Main.rand.NextFloat(3F, 12F);
                            Main.dust[A].rotation = Main.dust[A].velocity.ToRotation();
                            Main.dust[A].customData =2F;
                            Main.dust[A].noGravity = true;
                        }
                    }
                }
                projectile.rotation = projectile.velocity.ToRotation();
                return false;
            }
            if (projectile.type == 814)
            {
                if (projectile.localAI[0] == 0f)
                {
                    SoundEngine.PlaySound(SoundID.Item171, projectile.Center);
                    projectile.localAI[0] = 1f;
                    for (int num163 = 0; num163 < 8; num163++)
                    {
                        Dust obj13 = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, 5, projectile.velocity.X, projectile.velocity.Y, 100)];
                        obj13.velocity = (Main.rand.NextFloatDirection() * (float)Math.PI).ToRotationVector2() * 2f + projectile.velocity.SafeNormalize(Vector2.Zero) * 2f;
                        obj13.scale = 0.9f;
                        obj13.fadeIn = 1.1f;
                        obj13.position = projectile.Center;
                    }
                }

                projectile.alpha -= 20;
                if (projectile.alpha < 0)
                    projectile.alpha = 0;
                Dust obj14 = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, 5, projectile.velocity.X, projectile.velocity.Y, 100)];
                obj14.velocity = projectile.velocity / 2f;
                obj14.scale = 2f;
                obj14.position = projectile.Center + Main.rand.NextFloat() * projectile.velocity * 2f;
                obj14.noGravity = true;
                projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;
                if (projectile.velocity.Y < 10)
                {
                    projectile.velocity.Y += 0.05F;
                }
                projectile.velocity.X *= 0.995F;

                return false;
            }
            if (projectile.type == 100)
            {
                if (projectile.ai[2] < projectile.velocity.Length() * 4)
                    projectile.ai[2] += projectile.velocity.Length() / 4;
                else
                    projectile.ai[2] = projectile.velocity.Length() * 4;
            }
            if (projectile.type == 258)

            {
                projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;

                for (int a = 0; a < 2; a++)
                {
                    int A = NewDust(projectile.position - new Vector2(5), projectile.width + 10, projectile.height + 10, ModContent.DustType<速度粒子>(), 0, 0, 100, new Color(254, 121, 2,0), Main.rand.NextFloat(0.8F, 1.3F));
                    Main.dust[A].velocity = -projectile.velocity.PerfectNormalize() * 2;
                    Main.dust[A].rotation = projectile.velocity.ToRotation();
                    Main.dust[A].customData = 1F;
                    Main.dust[A].noGravity = true;
                }
                return false;
            }
                return base.PreAI(projectile);
        }
        public override void OnKill(Projectile projectile, int timeLeft)
        {
            if (projectile.type == 96)
            {
                for(int a = 0;a<30;a++)
                {
                    Dust dust = Main.dust[NewDust(projectile.Center-new Vector2(4), 1, 1, ModContent.DustType<速度粒子>(), projectile.oldVelocity.X, projectile.oldVelocity.Y, 100, color)];
                    dust.noGravity = true;
                    Vector2 vector = Vector2.One.RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi));
                    dust.velocity = vector * Main.rand.NextFloat(4, 7);
                    dust.rotation = dust.velocity.ToRotation();
                    dust.scale = Main.rand.NextFloat(2, 5);
                    dust.customData = 2F;
                }
                for(int a = 0;a<30;a++)
                {
                    Dust dust = Main.dust[NewDust(projectile.Center-new Vector2(4), 1, 1, 75, projectile.oldVelocity.X, projectile.oldVelocity.Y, 100)];
                    dust.noGravity = false;
                    Vector2 vector = Vector2.One.RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi));
                    dust.velocity = vector * Main.rand.NextFloat(1.3F);
                    dust.rotation = dust.velocity.ToRotation();
                    dust.scale = Main.rand.NextFloat(1, 2);
                    dust.customData = 1.2F;
                }
            }
            if (projectile.type == 258)
            {
                for(int a = 0;a<30;a++)
                {
                    Dust dust = Main.dust[NewDust(projectile.Center-new Vector2(4), 1, 1, ModContent.DustType<速度粒子>(), projectile.oldVelocity.X, projectile.oldVelocity.Y, 100,new Color(254, 121, 2,0))];
                    dust.noGravity = true;
                    Vector2 vector = Vector2.One.RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi));
                    dust.velocity = vector * Main.rand.NextFloat(2, 4.2F);
                    dust.rotation = dust.velocity.ToRotation();
                    dust.scale = Main.rand.NextFloat(1, 1.2F);
                    dust.customData = 0.3F;
                }
                for(int a = 0;a<30;a++)
                {
                    Dust dust = Main.dust[NewDust(projectile.Center-new Vector2(4), 1, 1, 6, projectile.oldVelocity.X, projectile.oldVelocity.Y, 100)];
                    dust.noGravity = false;
                    Vector2 vector = Vector2.One.RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi));
                    dust.velocity = vector * Main.rand.NextFloat(1.3F);
                    dust.rotation = dust.velocity.ToRotation();
                    dust.scale = Main.rand.NextFloat(0.4F, 0.8F);
                    dust.customData = 1.2F;
                }
            }
            if (projectile.type == 100)
            {
                for (int a = 0; a < 30; a++)
                {
                    Dust dust = Main.dust[NewDust(projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<光球粒子>(), projectile.oldVelocity.X, projectile.oldVelocity.Y, 100,new Color(255,50,50,0))];
                    dust.noGravity = true;
                    Vector2 vector = Vector2.One.RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi));
                    dust.velocity = vector * Main.rand.NextFloat(6F);
                    dust.rotation = dust.velocity.ToRotation();
                    dust.scale = Main.rand.NextFloat(1, 2);
                    dust.customData = -(dust.DustAI(8) +10F);
                }
            }
        }
        public override bool? CanDamage(Projectile projectile)
        {
            //落星
            if (projectile.type == ProjectileID.FallingStar&&DDSystem.BossSurvival)
            {
                return false;
            }

            return base.CanDamage(projectile);
        }
        public override void ModifyHitPlayer(Projectile projectile, Player target, ref HurtModifiers modifiers)
        {
            modifiers.ScalingArmorPenetration += 1- ArmorReduction;

        }
        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            Player player = Main.player[projectile.owner];
            if (target.Dnpc().PenetrationProtection > 0)
            {
                modifiers.SourceDamage *= PenetrationProtection;
            }

            if (PenetrationProtection > target.Dnpc().MaxPenetrationProtection)
            {
                PenetrationProtection -= target.Dnpc().PenetrationProtection;
                if (PenetrationProtection < target.Dnpc().MaxPenetrationProtection)
                {
                    PenetrationProtection = target.Dnpc().MaxPenetrationProtection;
                }
            }
        }
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[projectile.owner];
            if (projectile.type == 227)
            {
                if (damageDone >= 40)
                {
                    player.Heal(damageDone / 20);
                }
                else
                {
                    player.Heal(1);
                }
            }
        }
        public override bool ShouldUpdatePosition(Projectile projectile)
        {

            if (SpeedScope > 0)
            {
                //遍历弹幕
                for (int A = 0; A < 1000; A++)
                {
                    if(A<200)
                    {
                        NPC n = Main.npc[A];
                        if (n.active && n.CanBeChasedBy())
                        {
                            if (DDHelper.CircleInsertRectangle(n.Hitbox, projectile.Center, SpeedScope))
                            {
                                n.AddBuff(ModContent.BuffType<Frozen>(), 180);
                            }
                        }
                    }
                    Projectile proj = Main.projectile[A];
                    if (proj.active && proj.hostile && !proj.friendly && !proj.coldDamage && proj.velocity.Length() > 0)
                    {
                        if (DDHelper.CircleInsertRectangle(proj.Hitbox, projectile.Center, SpeedScope))
                        {
                            proj.DProj().SpeedTime = 3;
                            if (DDSystem.BossSurvival)
                            {
                                proj.DProj().Speed = 0.75F;
                            }
                            else
                            {
                                proj.DProj().Speed = 0.3F;
                            }
                        }
                    }
                }
            }
            if (SpeedTime > 0)
            {
                NewDustChange2(1, projectile.position,projectile.Size, ModContent.DustType<光球粒子>(), 0, 1, false, 0.4F, 0.8F, 0, new Color(0, 150, 255, 0));
                SpeedTime--;
                projectile.position -= projectile.velocity * (1F - Speed);
            }
            else
            {
                Speed = 1;
            }
            return base.ShouldUpdatePosition(projectile);
        }

        public override bool PreDraw(Projectile projectile, ref Color lightColor)
        {
            int A, V, B;

            Texture2D texture = DDTextures.VoidStar.Value;
            if (TrailDrawer == null)
            {
                TrailDrawer = new Trailing(new Trailing.VertexWidthFunction(TrailWidth), new Trailing.VertexColorFunction(TrailColor), null, GameShaders.Misc["刀光"]);
            }
            if (SpeedScope > 0)
            {
                Color color = new Color(0, 155, 255, 0);
                Vector2 vector = projectile.Size / 2;
                Texture2D Perlin = DDTextures.Perlin2.Value;
                Lighting.AddLight(projectile.Center, new Color(96, 255, 255).ToVector3());
                Main.spriteBatch.End();
                Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

                Vector2 origin = Perlin.Size() / 2;
                DDHelper.RotundityShaders(0.66F, color, track*0.01f, 1);
                Main.spriteBatch.Draw(Perlin, projectile.position + vector - Main.screenPosition, null, color, 0, origin, SpeedScope/128F, 0, 0);
                DDHelper.RotundityShaders(0.66F, color, track * 0.01f, 1);
                Main.spriteBatch.Draw(Perlin, projectile.position + vector - Main.screenPosition, null, color, -MathHelper.PiOver2, origin, SpeedScope / 128F, 0, 0);
                DDHelper.RotundityShaders(0.66F, color, track * 0.01f, 1);
                Main.spriteBatch.Draw(Perlin, projectile.position + vector - Main.screenPosition, null, color, MathHelper.PiOver2, origin, SpeedScope / 128F, 0, 0);
                DDHelper.RotundityShaders(0.66F, color, track * 0.01f, 1);
                Main.spriteBatch.Draw(Perlin, projectile.position + vector - Main.screenPosition, null, color, MathHelper.Pi, origin, SpeedScope / 128F, 0, 0);

                Main.spriteBatch.End();
                Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            }
            //探针激光
            if (projectile.type == 84)
            {
                lightColor = projectile.GetAlpha(Color.White);
            }
            //咒火弹
            if (projectile.type == 96)
            {
                texture = 咒火弹.Value;
                Color color = Lighting.GetColor((int)projectile.Center.X / 16, (int)projectile.Center.Y / 16, new Color(255, 255, 255, 0));
                color.A = 0;

                for (int i = 0; i < projectile.oldPos.Length; i++)
                {
                    Vector2 vector2 = projectile.oldPos[i] - Main.screenPosition+projectile.Size/2;
                    Color color2 = projectile.GetAlpha(Color.White) * ((projectile.oldPos.Length - i) / (float)projectile.oldPos.Length / 2f);
                    color2.A = 0;
                    float S = ((projectile.oldPos.Length - i) / (float)projectile.oldPos.Length)+0.1F;
                    if(S>1)
                    {
                        S = 1;
                    }
                    Main.spriteBatch.Draw(texture, vector2, null, color2, projectile.rotation, texture.Size() / 2, projectile.scale / 4 * S, SpriteEffects.None, 0);
                    Main.spriteBatch.Draw(texture, vector2, null, color2, projectile.rotation, texture.Size() / 2, projectile.scale / 4 * S, SpriteEffects.None, 0);
                }
                Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, null, color, projectile.velocity.ToRotation(), texture.Size() / 2, projectile.scale / 4, 0, 0f);
                return false;
            }
            if (projectile.type == 454)
            {
                texture = TextureAssets.Projectile[projectile.type].Value;
                Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, new Rectangle(0,0,texture.Width,texture.Height/2), projectile.GetAlpha(Color.White), 0, new Vector2(texture.Width, texture.Height/2) / 2, projectile.scale, 0, 0f);

                return false;
            }
            if (projectile.type == 100)
            {
                float SPEED = projectile.ai[2];
                texture = TextureAssets.Projectile[projectile.type].Value;
                Main.spriteBatch.Draw(texture, projectile.position + projectile.Size / 2 - projectile.velocity.PerfectNormalize() * 4 * projectile.scale - Main.screenPosition, new Rectangle(0, 8, texture.Width, 2), new Color(255,255,255,255), projectile.rotation, new Vector2(texture.Width, 0) / 2, projectile.scale * new Vector2(1, SPEED), 0, 0f);
                Main.spriteBatch.Draw(texture, projectile.position + projectile.Size / 2 - projectile.velocity.PerfectNormalize() * 4 * projectile.scale - Main.screenPosition, new Rectangle(0, 8, texture.Width, 2), new Color(255,0,0,0), projectile.rotation, new Vector2(texture.Width, 0) / 2, projectile.scale * new Vector2(1, SPEED), 0, 0f);
                Main.spriteBatch.Draw(texture, projectile.position + projectile.Size / 2 - Main.screenPosition+new Vector2(0,0), new Rectangle(0, 0, texture.Width, 6), new Color(255, 255, 255, 255), projectile.rotation, new Vector2(texture.Width, 0) / 2, projectile.scale, 0, 0f);
                Main.spriteBatch.Draw(texture, projectile.position + projectile.Size / 2 - Main.screenPosition+new Vector2(0,0), new Rectangle(0, 0, texture.Width, 6), new Color(255, 0, 0, 0), projectile.rotation, new Vector2(texture.Width, 0) / 2, projectile.scale, 0, 0f);

                Main.spriteBatch.Draw(texture, projectile.position + projectile.Size / 2-projectile.velocity.PerfectNormalize()* ((4+SPEED*2)*projectile.scale-0.5F) - new Vector2(0, 0) - Main.screenPosition, new Rectangle(0, texture.Height-4, texture.Width,4), new Color(255, 255, 255, 255), projectile.rotation, new Vector2(texture.Width, 0) / 2, projectile.scale, 0, 0f);
                Main.spriteBatch.Draw(texture, projectile.position + projectile.Size / 2-projectile.velocity.PerfectNormalize()* ((4+SPEED*2)*projectile.scale-0.5F) - new Vector2(0, 0) - Main.screenPosition, new Rectangle(0, texture.Height-4, texture.Width,4), new Color(255, 0, 0, 0), projectile.rotation, new Vector2(texture.Width, 0) / 2, projectile.scale, 0, 0f);
                //Main.spriteBatch.Draw(DDTextures.WhitePng.Value, projectile.position  - Main.screenPosition, null, projectile.GetAlpha(Color.White),0, Vector2.Zero, projectile.Size/2, 0, 0f);

                return false;
            }
            //花
            if (projectile.type == 275|| projectile.type == 276)
            {
                texture = Glow[275].Value;
                Texture2D texture2 = TextureAssets.Projectile[projectile.type].Value;
                Color color = new Color(225,128,206);
                if(projectile.type == 276) color = new Color(107, 182, 0);
                color.A = 0;

                for (int i = 0; i < projectile.oldPos.Length; i++)
                {
                    Vector2 vector2 = projectile.oldPos[i] - Main.screenPosition + projectile.Size / 2;
                    Color color2 = color * ((projectile.oldPos.Length - i) / (float)projectile.oldPos.Length);
                    float S = ((projectile.oldPos.Length - i) / (float)projectile.oldPos.Length);
                    Main.spriteBatch.Draw(texture, vector2, null, color2, projectile.oldRot[i], texture.Size() / 2, projectile.scale / 4 * S * 1.1f, SpriteEffects.None, 0);

                }
                Main.spriteBatch.Draw(texture2, projectile.Center - Main.screenPosition, new Rectangle(0, texture2.Height / 2 * projectile.frame, texture2.Width, texture2.Height / 2), Color.White, projectile.rotation, new Vector2(texture2.Width, texture2.Height / 2) / 2, projectile.scale, 0, 0f);

                return false;
            }
            if (projectile.type == 277)
            {
                texture = Glow[277].Value;
                Texture2D texture2 = TextureAssets.Projectile[projectile.type].Value;
                Color color = new Color(225,128,206);
                color.A = 0;
                for (int i = 0; i < projectile.oldPos.Length; i++)
                {
                    Vector2 vector2 = projectile.oldPos[i] - Main.screenPosition + projectile.Size / 2;
                    Color color2 = color * ((projectile.oldPos.Length - i) / (float)projectile.oldPos.Length);
                    float S = ((projectile.oldPos.Length - i) / (float)projectile.oldPos.Length);
                    Main.spriteBatch.Draw(texture, vector2, null, color2, projectile.oldRot[i], texture.Size() / 2, projectile.scale / 4 * S * 1.1f, SpriteEffects.None, 0);

                }
                Main.spriteBatch.Draw(texture2, projectile.Center - Main.screenPosition, new Rectangle(0, 0, texture2.Width, texture2.Height), Color.White, projectile.rotation, texture2.Size() / 2, projectile.scale, 0, 0f);

                return false;
            }
            //石巨人
            
            if (projectile.type == 258)
            {
                texture = DDTextures.MagicBall.Value;
                Texture2D texture2 = TextureAssets.Projectile[projectile.type].Value;
                Color color = new Color(254,121,2);
                color.A = 0;
                for (int i = 0; i < projectile.oldPos.Length; i++)
                {
                    Vector2 vector2 = projectile.oldPos[i] - Main.screenPosition + projectile.Size / 2;
                    Color color2 = color * ((projectile.oldPos.Length - i) / (float)projectile.oldPos.Length);
                    float S = ((projectile.oldPos.Length - i) / (float)projectile.oldPos.Length);
                    Main.spriteBatch.Draw(texture, vector2, null, color2, projectile.oldRot[i], texture.Size() / 2, projectile.scale / 4 * S, SpriteEffects.None, 0);

                }
                Main.spriteBatch.Draw(texture2, projectile.Center - Main.screenPosition, new Rectangle(0, 0, texture2.Width, texture2.Height), color, projectile.rotation, texture2.Size() / 2, projectile.scale/4, 0, 0f);
                Main.spriteBatch.Draw(texture2, projectile.Center - Main.screenPosition, new Rectangle(0, 0, texture2.Width, texture2.Height), color, projectile.rotation, texture2.Size() / 2, projectile.scale/4, 0, 0f);
                Main.spriteBatch.Draw(texture2, projectile.Center - Main.screenPosition, new Rectangle(0, 0, texture2.Width, texture2.Height), color, projectile.rotation, texture2.Size() / 2, projectile.scale/4, 0, 0f);
                Main.spriteBatch.Draw(texture2, projectile.Center - Main.screenPosition, new Rectangle(0, 0, texture2.Width, texture2.Height), color, projectile.rotation, texture2.Size() / 2, projectile.scale/4, 0, 0f);
                Main.spriteBatch.Draw(texture2, projectile.Center - Main.screenPosition, new Rectangle(0, 0, texture2.Width, texture2.Height), color, projectile.rotation, texture2.Size() / 2, projectile.scale/4, 0, 0f);

                return false;
            }

            return base.PreDraw(projectile, ref lightColor);
        }
    }
    public class PlayerItem : ModProjectile
    {
        public override void SetStaticDefaults()
        {
           //DisplayName.SetDefault("PlayerItem");
           //DisplayName.AddTranslation(7, "玩家物品");
        }
        public override string Texture => "DDmod/Image/Nullpng";
        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.scale = 1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 600;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.hide = true;
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindNPCsAndTiles.Add(index);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity = Vector2.Zero;
            return false;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            fallThrough = false;
            return true;
        }
        public override void AI()
        {
            if (Projectile.Player().PlayerAction().WeaponType == HandheldWeaponType.Ground)
            {
                Projectile.timeLeft = 10;
            }
            else
            {
                Projectile.Kill();
            }
            if (scale == 0)
            {
                scale = Projectile.Player().inventory[Projectile.Player().selectedItem].scale;
            }
            if (rotation == 0)
            {
                if (item != null)
                {
                    if (item.DamageType == DamageClass.Melee|| item.DamageType == DamageClass.MeleeNoSpeed)
                    {
                        rotation = Main.rand.NextFloat(1.5F, 3);
                        Projectile.rotation = rotation;
                        Projectile.width = (int)(texture.Width*0.6F*item.scale);
                        Projectile.height = (int)(texture.Height * 0.6F * item.scale);
                    }
                    else if (item.DamageType == DamageClass.Magic|| item.DamageType!=DamageClass.Summon)
                    {
                        rotation = Main.rand.NextFloat(-1.5F, 0.2F);
                        Projectile.rotation = rotation;
                        Projectile.width = (int)(texture.Width * 0.8F * item.scale);
                        Projectile.height = (int)(texture.Height * 0.8F * item.scale);
                    }
                    else
                    {
                        rotation = Main.rand.NextFloat(-1F, 1);
                        Projectile.rotation = rotation;
                    }
                }
            }
            if (Projectile.velocity != Vector2.Zero)
            {
                Projectile.rotation += 0.7F * Projectile.Player().direction;
            }
            else
            {
                Projectile.rotation = rotation;
            }
            Projectile.velocity.Y += 0.3F;
        }
        public Texture2D texture;
        public float rotation;
        public float scale;
        public Item item;
        public override bool PreDraw(ref Color lightColor)
        {
            if (texture == null)
            {
                texture = TextureAssets.Item[Projectile.Player().inventory[Projectile.Player().selectedItem].type].Value;
            }
            else
            {
                Player player = Projectile.Player();
                if (item == null)
                {
                    item = player.inventory[player.selectedItem];
                }
                Rectangle? sourceRect = new Rectangle?((Main.itemAnimations[item.type] == null) ? Utils.Frame(texture, 1, 1, 0, 0, 0, 0) : Main.itemAnimations[item.type].GetFrame(texture, -1));
                float frame = (Main.itemAnimations[item.type] == null) ? 1 : Main.itemAnimations[item.type].FrameCount;
                ItemSlot.GetItemLight(ref lightColor, ref scale, item, false);
                lightColor = player.GetImmuneAlpha(item.GetAlpha(lightColor) * player.stealth, 0f);
                Vector2 origin = new Vector2(texture.Width / 2, texture.Height / frame / 2);
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, sourceRect, lightColor, Projectile.rotation, origin, scale, SpriteEffects.None, 0);
            }
            return false;
        }
    }
}