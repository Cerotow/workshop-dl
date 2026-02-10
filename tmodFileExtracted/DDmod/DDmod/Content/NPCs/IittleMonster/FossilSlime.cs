using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Items.Series.Acorn;
using DDmod.Content.NPCs.EliteMonster;
using DDmod.Content.NPCs.IittleMonster.旗子;
using DDmod.Content.Projectiles.Summon;
using System.Linq;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.NPCs.IittleMonster
{
    public class FossilSlime : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 2;
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = 1;
            AIType = 1;
            AnimationType = 1;
            NPC.lifeMax = 60;
            NPC.damage = 14;
            NPC.defense = 8;
            NPC.knockBackResist = 0.8f;
            NPC.width = 20;
            NPC.height = 20;
            NPC.value = Item.buyPrice(0, 0, 2, 0);
            NPC.lavaImmune = true;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.HitSound = SoundID.Item1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.netAlways = true;
            NPC.scale = 1.3f;
            NPC.Dnpc().Properties.Iron = true;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<化石史莱姆旗>();
        }
        public override bool? CanFallThroughPlatforms()
        {
            return false;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundDesert
            });
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(3380, 1, 2, 5));
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (NPCdirection.Incident(spawnInfo))
            {
                return 0;
            }
            if (spawnInfo.Player.ZoneUndergroundDesert)
            {
                return 0.05f;
            }
            return 0;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            return true;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            /*
            RasterizerState state = new RasterizerState()
            {
                CullMode = CullMode.CullCounterClockwiseFace,
                ScissorTestEnable = true,
            };

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, state, null, Main.GameViewMatrix.TransformationMatrix);

            GameShaders.Misc["渲染滤镜"].UseOpacity(1);
            GameShaders.Misc["渲染滤镜"].SetShaderTexture(ModContent.Request<Texture2D>("DDmod/Image/化石"));
            GameShaders.Misc["渲染滤镜"].Shader.Parameters["uImageSize1"].SetValue(ModContent.Request<Texture2D>("DDmod/Image/化石").Size());
            GameShaders.Misc["渲染滤镜"].UseColor(drawColor);
            GameShaders.Misc["渲染滤镜"].Shader.Parameters["uColor2"].SetValue(drawColor.ToVector3() * 0.75f);
            GameShaders.Misc["渲染滤镜"].Shader.Parameters["renderTargetArea"].SetValue(new Vector2(ModContent.Request<Texture2D>("DDmod/Image/化石").Width(), ModContent.Request<Texture2D>("DDmod/Image/化石").Height()));
            GameShaders.Misc["渲染滤镜"].Shader.Parameters["uWorldPosition"].SetValue(Vector2.Zero);
            GameShaders.Misc["渲染滤镜"].Shader.Parameters["position"].SetValue(Vector2.Zero);
            GameShaders.Misc["渲染滤镜"].Shader.Parameters["ImageSize"].SetValue(new Vector2(ModContent.Request<Texture2D>("DDmod/Image/化石").Width() / 2, ModContent.Request<Texture2D>("DDmod/Image/化石").Height() / Main.npcFrameCount[NPC.type] / 2)*1.05f);
            GameShaders.Misc["渲染滤镜"].Shader.Parameters["upscaleFactor"].SetValue(new Vector2(0F));
            GameShaders.Misc["渲染滤镜"].Apply();
            Texture2D texture = TextureAssets.Npc[NPC.type].Value;
            spriteBatch.Draw(texture, NPC.Center - screenPos+new Vector2(0,NPC.height/2+4), new Rectangle?(NPC.frame), drawColor, NPC.rotation, new Vector2(texture.Width, texture.Height) / 2, NPC.scale * new Vector2(1 - NPC.Dnpc().Times[4], 1 + NPC.Dnpc().Times[4]), 0, 0f);
            spriteBatch.End();
            spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, state, null, Main.GameViewMatrix.TransformationMatrix);*/
        }
    }
}