using DDmod.Content.Buffs.DeBuffs;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;

namespace DDmod.Content.Projectiles.Melee.Sword
{
    public class ZombieArm : ModProjectile
    {
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 25;
            Projectile.width = 20;
            Projectile.height = 34;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ignoreWater = true;
            //projectile.light = 0.50f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.ownerHitCheck = true;
            Projectile.MeleeProj().oldVels = new Vector2[Projectile.oldPos.Length];
            Projectile.MeleeProj().oldVels2 = 40;
            Projectile.MeleeProj().SwordHitbox = true;
            Projectile.extraUpdates = 6;
            Projectile.stopsDealingDamageAfterPenetrateHits = true;
        }
        public int proj = 0;
        //特殊攻击
        public bool Special;
        public int Da;
        public int 妈妈;
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(proj);
            writer.Write(Da);
            writer.Write(NPC);
            writer.Write(NPC2);
            writer.Write(妈妈);
            writer.Write(Special);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            proj = reader.ReadInt32();
            Da = reader.ReadInt32();
            NPC = reader.ReadInt32();
            NPC2 = reader.ReadInt32();
            妈妈 = reader.ReadInt32();
            Special = reader.ReadBoolean();
        }
        public override bool? CanDamage()
        {
            if(Special)
            {
                return true;
            }
            return base.CanDamage();
        }

        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            if (!Special&& Projectile.DProj().MouseWorld == Vector2.Zero)
            {
                Projectile.DProj().MouseWorld = player.Dplayer().MouseWorld;
                Projectile.netUpdate = true;
            }

            if (player.controlUseTile && Projectile.localAI[1] == 0 && Da < 310 * Projectile.extraUpdates)
            {
                if (NPC >= 0 && Main.npc[NPC].active)
                {
                    Projectile.DProj().MouseWorld = player.Dplayer().MouseWorld;
                    Projectile.DProj().vector[0] = Projectile.DProj().MouseWorld - player.Center;
                    Projectile.netUpdate = true;
                    proj = 0;
                }
                Projectile.MeleeProj().SwordHitbox = false;
                Special = true;
                Projectile.netUpdate = true;
            }
            if (Projectile.scale < player.GetAdjustedItemScale(player.ActiveItem()))
            {
                Projectile.scale = player.GetAdjustedItemScale(player.ActiveItem());
            }
            Projectile.Resize((int)(Projectile.OriginalWidth() * Projectile.scale), (int)(Projectile.OriginalHeight() * Projectile.scale));
            if (!Special)
            {
                Projectile.HoldProj(player, 26 * Projectile.scale, Projectile.ai[0], Projectile.DProj().vector[0], MathHelper.PiOver4, 0, true, 0.05f * Projectile.DProj().Times[0], false, false);
                Projectile.HoldSword2(player, -player.HeldItem.useAnimation * (Projectile.extraUpdates+1), -1, 2.1f, false);

            }
            else
            {
                Projectile.HoldProj(player, 26 * Projectile.scale, Projectile.ai[0], Projectile.DProj().vector[0], MathHelper.PiOver4, 0, true, 0.05f * Projectile.DProj().Times[0], true, false);
                //Projectile.localAI[1] = 1;
            }
            if ((Projectile.localAI[1] <= 0 || Projectile.MeleeProj().DelayedKill > 0) && !Special)
            {
                proj = 0;
            }
            else
            {
                if (proj == 0)
                {
                    if (!player.controlUseTile)
                    {
                        PlaySound(SoundID.Item1, Projectile.position);
                    }
                    Projectile.netUpdate = true;
                }
                proj++;
                if (proj > 60)
                {
                    if (NPC >= 0)
                    {
                        Special = false;
                        Projectile.MeleeProj().SwordHitbox = true;
                        proj = 0;
                    }
                    else if (proj > 120)
                    {
                        player.itemAnimation = 0;
                        player.itemTime=0;
                        Projectile.Kill();
                    }
                }
            }
            if (NPC >= 0)
            {
                Main.npc[NPC].Center = player.Center + Projectile.velocity.PerfectNormalize() * 40 * Projectile.scale;
                Main.npc[NPC].Dnpc().Control = 120000;
                Main.npc[NPC].velocity = Projectile.velocity.PerfectNormalize();
                Main.npc[NPC].rotation = (Main.npc[NPC].Center-player.Center).ToRotation();
                if(Main.npc[NPC].Center.X - player.Center.X>0)
                {
                    Main.npc[NPC].spriteDirection= 0;
                }
                else
                {
                    Main.npc[NPC].spriteDirection = 1;
                    Main.npc[NPC].rotation += MathHelper.Pi;
                }
                Da++;
                if (Da % 180 == 179)
                {
                    //Main.npc[NPC].StrikeNPCNoInteraction(Projectile.damage / 2, 1, player.direction, Main.rand.Next(100) < Projectile.CritChance);
                }
                if ( Projectile.owner == Main.myPlayer)
                {
                    DDmod.SyncData(DDType.NPCCenter, NPC, -1, Projectile.owner);
                }
                Projectile.netUpdate = true;
                Main.npc[NPC].netUpdate = true;
            }
            if (!Special)
            {
                NPC = -1;
                Projectile.netUpdate = true;
            }
            if (NPC2 >= 0 && 妈妈 <= 30 && NPC == -1)
            {
            }
            Projectile.spriteDirection = Projectile.DProj().Times[0] > 0 ? 0 : 1;
            return false;
        }
        public override void OnKill(int timeLeft)
        {
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (Special)
            {
                modifiers.SourceDamage /= 2;
            }
            if (target.Dnpc().Control > 0)
            {
                modifiers.Knockback *= 0;
            }
        }
        public int NPC = -1;
        public int NPC2 = -1;
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            if (NPC == -1 && Special)
            {
                if (!target.Dnpc().BossPhysique && target.Size.Length() / 2 < 100 && target.realLife < 0 && target.knockBackResist != 0)
                {
                    NPC = target.whoAmI;

                    for (int A = 0; A < 200; A++)
                    {
                        Projectile.localNPCImmunity[A] = -1;
                    }
                    proj = 0;
                    Projectile.netUpdate = true;
                }
            }
            if (!Special && NPC2 == -1 && target.Dnpc().Control > 0)
            {
                NPC2 = target.whoAmI;
                target.Dnpc().Player = player.whoAmI;
                Projectile.netUpdate = true;
            }
            if (!Special)
            {
                Vector2 vector = Main.rand.NextVector2Unit() * 60;
                int A = NewProjectile(Projectile.GetSource_FromThis(), target.Center, -vector / 10, ModContent.ProjectileType<ZombieSlash>(), 0, 0, Projectile.owner, target.whoAmI, 0);
                Main.projectile[A].DProj().color = color;
            }
            if (target.Dnpc().Control > 0)
            {
                target.velocity = (Projectile.DProj().MouseWorld - player.Center).PerfectNormalize() * 12;
                target.Dnpc().Control = 120;
                Mod mod = DDmod.Instance;
                Projectile.netUpdate = true;
                if (Main.netMode == 0)
                {
                    return;
                }
                DDmod.SyncData(DDType.NPCCenter, target.whoAmI, -1, Projectile.owner);
            }
        }
        Color color = new Color(209, 214, 138, 0);
        public float TWidth()
        {
            return 5 * Projectile.scale;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.ai[1] < 2)
            {
                Projectile.ai[1]++;
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

            Texture2D texture = TextureAssets.Item[Projectile.Player().ActiveItem().type].Value;

            Vector2 Center = Projectile.Center - Main.screenPosition;

            if (Projectile.MeleeProj().DelayedKill <= 0)
            {
                if (Projectile.spriteDirection == 0)
                {
                    Main.spriteBatch.Draw(texture, Center, null, Color.White, Projectile.rotation, texture.Size() / 2, Projectile.scale, (SpriteEffects)Projectile.spriteDirection, 0f);
                }
                else
                {
                    Main.spriteBatch.Draw(texture, Center, null, Color.White, Projectile.rotation + MathHelper.PiOver2, texture.Size() / 2, Projectile.scale, (SpriteEffects)Projectile.spriteDirection, 0f);
                }
            }
            else
            {
                Projectile.alpha += 10;
            }
            return false;
        }
    }
    public class ZombieSlash : ModProjectile
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
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.Resize((int)(Projectile.OriginalWidth() * Projectile.scale), (int)(Projectile.OriginalHeight() * (0.2f + Main.npc[(int)Projectile.ai[0]].Size.Length() / 100)));
            if (Projectile.velocity.Length() > 0.1f)
                Projectile.velocity = Projectile.velocity.PerfectNormalize() * 0.09F;
            Projectile.timeLeft = 20;

            Projectile.scale -= 0.05f;
            Projectile.Center = Main.npc[(int)Projectile.ai[0]].Center;
            Projectile.active = Main.npc[(int)Projectile.ai[0]].active;
            if (Projectile.scale < 0.05F)
            {
                Projectile.Kill();
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(255, 255, 255, Projectile.alpha));
        }
        int Time;
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects spriteEffects = 0;
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Color color = Projectile.DProj().color;
            color.A = 0;
            Vector2 vector = new Vector2(Projectile.width, Projectile.height) / 2;

            if (Projectile.ai[1] >= 2)
            {
                return false;
            }
            for (int a = 0; a < 3; a++)
            {
                Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, color * 1.4f, Projectile.rotation, texture.Size() / 2, new Vector2(0.3f + Main.npc[(int)Projectile.ai[0]].Size.Length() / 100, Projectile.scale / 16) * 1.5f, spriteEffects, 0f);
                //Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, color * 4.4f, Projectile.rotation, texture.Size() / 2, new Vector2(0.6f, Projectile.scale / 16), spriteEffects, 0f);

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
}
