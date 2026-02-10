using DDmod.Content.Particles;
using DDmod.SubworldLibraryWorld;
using SubworldLibrary;
using Terraria.GameContent.Drawing;

namespace DDmod.Content.NPCs
{
#pragma warning disable
    public class BlackHole : ModNPC
    {
        public override void SetStaticDefaults()
        {
           //DisplayName.SetDefault("Black hole");
           //DisplayName.AddTranslation(7, "黑洞");
            Main.npcFrameCount[NPC.type] = 2;
        }
        public override void SetDefaults()
        {
            NPC.width = 200;
            NPC.height = 200;
            NPC.aiStyle = -1;
            NPC.damage = 0;
            NPC.defense = 0;
            NPC.dontTakeDamage = true;
            NPC.lifeMax = 114514;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.4f;
            NPC.noGravity = true;
            NPC.scale = 1f;
        }
        float GG;
        float GG2;
        public override bool PreAI()
        {
            NPC.TargetClosest();
            Player player = Main.player[NPC.target];
            if(new Rectangle((int)Main.MouseWorld.X,(int)Main.MouseWorld.Y,1,1).Intersects(NPC.getRect()))
            {
                if(Main.LocalPlayer.controlUseTile)
                {
                    Main.LocalPlayer.SetTalkNPC(NPC.whoAmI);

                    Main.npcChatText = GetChat();
                }
            }
            /*
            if (player.Distance(NPC.Center) < 200)
            {
                //NPC.townNPC = true;
            }
            else
            {
                NPC.townNPC = false;
            }
            NPC.townNPC = false;
            if (!SubworldLibrary.Subworld.IsActive<FleshRealm>())
            {
                if (player.Distance(NPC.Center) >3000)
                {
                    NPC.active = false;
                }
            }*/
            if (NPC.ai[0] ==0)
            {
                NPC.scale = 0.1F;
            }
            if (NPC.ai[0] < 20)
            {
                NPC.ai[0]++;
                NPC.scale += 0.05f;
            }
            GG -= 0.1f;
            GG2 += 0.1f;
            NPC.rotation = GG;
            NPC.timeLeft = 60;
            Lighting.AddLight(NPC.Center, 2.55f * 0.3f, 0, 0);
            if (NPC.ai[1] == 1)
            {
                NPC.scale -= 0.03f;
            }
            if (NPC.scale < 0.1)
            {
                NPC.active = false;
            }
            if (NPC.ai[1] != 1)
            {
                Vector2 Vector = Utils.RotatedBy(new Vector2(1, 0), Main.rand.NextFloat(0F, MathHelper.TwoPi), default);
                DDParticle.RequestParticleSpawn(ParticleType.blackHole, new ParticleOrchestraSettings
                {
                    PositionInWorld = (NPC.Center + Vector * 250),
                    MovementVector = Vector * -5
                });
            }
            return true;
        }
        public override string GetChat()
        {
            //if (!SubworldSystem.IsActive<FleshRealm>())
            if(true)
            {
                return "这是一个黑洞,看来克苏鲁之眼是从这里面逃跑了";
            }
            else
            {
                return "通过这里,好像能回到以前的世界";
            }
        }
        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = "进入黑洞";
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = (Texture2D)TextureAssets.Npc[NPC.type];
            spriteBatch.Draw(DDTextures.VoidStar.Value, NPC.Center - Main.screenPosition, null, new Color(255, 0, 0, 0), NPC.rotation, DDTextures.VoidStar.Size() / 2, NPC.scale * 2F, SpriteEffects.FlipHorizontally, 0);
            spriteBatch.Draw(DDTextures.VoidStar.Value, NPC.Center - Main.screenPosition, null, new Color(255, 0, 0, 0), NPC.rotation, DDTextures.VoidStar.Size() / 2, NPC.scale * 2F, SpriteEffects.FlipHorizontally, 0);
            DDHelper.Compression(texture, new Color(255, 0, 0) * 0.9f, 0, 255, new Vector2(1, 1), 0, GG, BlendState.Additive);
            spriteBatch.Draw(ModContent.Request<Texture2D>("DDmod/Image/旋转特效").Value, NPC.Center - Main.screenPosition, null, new Color(255, 0, 0) * 0.9f, NPC.rotation, ModContent.Request<Texture2D>("DDmod/Image/旋转特效").Size() / 2, NPC.scale *0.3F, SpriteEffects.None, 0);
            DDHelper.Compression(texture, new Color(255, 0, 0) * 0.9f, 0, 255, new Vector2(1, 1), 0, GG2, BlendState.Additive);
            spriteBatch.Draw(ModContent.Request<Texture2D>("DDmod/Image/旋转特效").Value, NPC.Center - Main.screenPosition, null, new Color(255, 0, 0) * 0.9f, NPC.rotation, ModContent.Request<Texture2D>("DDmod/Image/旋转特效").Size() / 2, NPC.scale *0.31F, SpriteEffects.FlipHorizontally, 0);

            DDHelper.Compression(texture, new Color(255, 255, 255) * 0.9f, 0, 255, new Vector2(1, 1), 0, 0, BlendState.AlphaBlend);
            spriteBatch.Draw(texture, NPC.Center - Main.screenPosition, null, Color.White, NPC.rotation, texture.Size() / 2, NPC.scale * 1.3F, SpriteEffects.None, 0);
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
        public override void OnChatButtonClicked(bool firstButton, ref string shopName)
        {
            if (firstButton)
            {
                if (!SubworldSystem.IsActive<FleshRealm>())
                {
                    SubworldSystem.Enter<FleshRealm>();
                }
                else
                {
                    SubworldSystem.Exit();
                }
            }
        }
    }
}