using DDmod.Content.Projectiles.Boss;
using Terraria;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.NPCs.Boss.MeteorAnnihilator
{
    public class 歼灭者导弹2 : ModNPC
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.ProjectileNPC[NPC.type] = true;
            NPCID.Sets.CantTakeLunchMoney[Type] = true;
        }
        public override void SetDefaults()
        {
            NPCID.Sets.TrailCacheLength[Type] = 5;
            NPCID.Sets.TrailingMode[Type] = 0;
            NPC.damage = 50;
            NPC.width = 18;
            NPC.height = 18;
            NPC.defense = 4;
            NPC.lifeMax = 50;
            NPC.scale = 1f;
            NPC.aiStyle = -1;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.canGhostHeal = false;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.Item14;
            NPC.netAlways = true;
            NPC.dontCountMe = true;
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
            NPC.Dnpc().Properties.Iron = true;
            NPC.Dnpc().Properties.BossLife = 1.15F;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            database.Entries.Remove(bestiaryEntry);
        }
        public int moveSpeed;
        public int moveSpeedY;
        public override void AI()
        {
            Player player = Main.player[NPC.target];
            NPC.TargetClosest();
            Vector2 direction = player.Center - NPC.Center;
            direction.DirectPerfectNormalize();
            NPC.rotation = NPC.velocity.ToRotation()+MathHelper.PiOver2;
            NPC.localAI[1]++;
            if (NPC.ai[0] != 0 || NPC.ai[1] != 0)
            {
                direction = new Vector2(NPC.ai[0], NPC.ai[1]) - NPC.Center;
                float d = direction.Length()/10;
                direction.DirectPerfectNormalize();
                if(d>10)
                {
                    d = 10;
                }
                if(d<2)
                {
                    d = 2;
                }
                NPC.velocity = (NPC.velocity * 20 + direction * d) / 21;
                if(d<=2F)
                {
                    if (Main.netMode != 2)
                    {
                        NewProjectile(NPC.GetSource_Death(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<歼灭者导弹爆炸>(), NPC.damage / 6, 1, Main.myPlayer, 1);
                    }
                    PlaySound(SoundID.Item14, NPC.Center);
                    NPC.Kill();
                }
            }
            else
            {
                if (!Main.player[NPC.target].dead && NPC.localAI[1] < 40)
                {
                    NPC.velocity = (NPC.velocity * 20 + direction * 16) / 21;
                }
                if (NPC.localAI[1] > 100)
                {
                    if (Main.netMode != 1)
                    {
                        NewProjectile(NPC.GetSource_Death(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<歼灭者导弹爆炸>(), NPC.damage / 6, 1, Main.myPlayer, 1);
                    }
                    PlaySound(SoundID.Item14, NPC.Center);
                    NPC.Kill();
                }
            }
        }
        public override bool PreKill()
        {
            return false;
        }
        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
            if (Main.netMode != 2)
            {
                NewProjectile(NPC.GetSource_Death(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<歼灭者导弹爆炸>(), NPC.damage / 6, 1, Main.myPlayer, 1);
            }
            PlaySound(SoundID.Item14, NPC.Center);
            NPC.Kill();
        }
        public override bool? CanFallThroughPlatforms()
        {
            return true;
        }
        internal Color ColorFunction(float completionRatio)
        {
            float colorFade = 1f - Utils.GetLerpValue(0f, 1f, completionRatio, true);
            return Color.Lerp(playerHelper.MulticolorLerp((float)Math.Pow((double)completionRatio, 0.5), new Color[]
            {
                new Color(1, 255,7 ),
                new Color(1, 210,7 ),
                new Color(1, 190,7 ),
            }) * MathHelper.Lerp(0f, 1.4f, colorFade), new Color(1, 160, 7), (float)Math.Pow((double)completionRatio, 1.0));
        }
        internal float WidthFunction(float completionRatio)
        {
            float widthRatio = Utils.GetLerpValue(0f, 1f, completionRatio, false);
            return MathHelper.Lerp(playerHelper.FMulticolorLerp((float)Math.Pow((double)completionRatio, 0.5), new float[]
            {
                10,
                20,
                30,
                20,
            }) * NPC.scale, 10 * NPC.scale, (float)Math.Pow((double)completionRatio, 1.0));
        }
        internal static Trailing TrailDrawer;
        public override void HitEffect(HitInfo hit)
        {
            if (NPC.life <=0 && Main.netMode != 1)
            {
                NewProjectile(NPC.GetSource_Death(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<歼灭者导弹爆炸>(), NPC.damage / 6, 1, Main.myPlayer, 1);
            }
        }
        public override void FindFrame(int frameHeight)
        {
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[NPC.type].Value;
            if (TrailDrawer == null)
            {
                TrailDrawer = new Trailing(new Trailing.VertexWidthFunction(WidthFunction), new Trailing.VertexColorFunction(ColorFunction), null, GameShaders.Misc["贴图拖尾"]);
            }
            GameShaders.Misc["贴图拖尾"].SetShaderTexture(DDTextures.GlowTrail2);
            GameShaders.Misc["贴图拖尾"].Shader.Parameters["uSpeed"].SetValue(1.2f);
            TrailDrawer.Draw(NPC.oldPos, NPC.Size * 0.5f - Main.screenPosition, 104, null);
            Main.spriteBatch.Draw(texture, NPC.Center - Main.screenPosition, null, Color.White, NPC.rotation, new Vector2(texture.Width) / 2, 1, 0, 0f);
            return false;
        }
    }
}