using DDmod.Content.Dusts;
using DDmod.Content.Items.Boss.MeteorDiggerItems;
using DDmod.Content.Items.Boss.流星破坏者;
using DDmod.Content.Items.Boss.狱火蛇物品;
using DDmod.Content.Items.Boss.绿岩之视;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.NPCs.Boss.MeteorDigger;
using DDmod.Content.NPCs.Boss.恐惧缝合体;
using DDmod.Content.NPCs.Boss.流星破坏者;
using DDmod.Content.NPCs.Boss.狱火蛇;
using DDmod.Content.NPCs.Boss.绿岩之视;
using DDmod.Content.NPCs.Boss.鬼牙;
using DDmod.Worlds;
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.InteropServices;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace DDmod.ModLinkage.BossChecklist
{
    public class 流星破坏者Checklist : ModSystem
	{

		public bool DriftBool;
		public float Drift;

        //帧
        public int frame;
        public float frameCounter;

        //Boss位置和boss移动
        public Vector2 EPosition;
		//眼睛到达位置
        public Vector2 EPosition2;
        public Vector2 BossPosition;
        public Vector2 BossVelocity;

        public float flame;
		public bool flameBool;

        public bool[] Bool = new bool[6];
        public float[] Time = new float[6];

        public float Background;
        public float Background2;
        public float Background3;

        public static Asset<Texture2D> BossChecklistBook;

		public override void Load()
		{
			BossChecklistBook = ModContent.Request<Texture2D>("DDmod/ModLinkage/BossChecklist/天空Checklist");
			for (int a = 0; a < ChecklistHelper.Gore.Length; a++)
			{
                ChecklistHelper.Gore[a] = new BGore();
			}
		}
		public override void Unload()
        {
			BossChecklistBook = null;
		}

		public override void UpdateUI(GameTime gameTime)
		{
        }
        internal static Trailing TrailDrawer;
        internal static Trailing TrailDrawer2;
        internal Color ColorFunction(float completionRatio)
        {
            float colorFade = 1f - Utils.GetLerpValue(0f, 1f, completionRatio, true);
            return Color.Lerp(playerHelper.MulticolorLerp((float)Math.Pow((double)completionRatio, 0.5), new Color[]
            {
                new Color(252, 160,28 ),
                new Color(252, 160,28 ),
                new Color(252, 160,28 ),
                new Color(248, 66,5 ),
            }) * MathHelper.Lerp(0f, 1.4f, colorFade), new Color(248, 66, 5), (float)Math.Pow((double)completionRatio, 1.0));
        }
        internal float WidthFunction(float completionRatio)
        {
            float widthRatio = Utils.GetLerpValue(0f, 1f, completionRatio, false);
            return MathHelper.Lerp(playerHelper.FMulticolorLerp((float)Math.Pow((double)completionRatio, 0.5), new float[]
            {
                50,
                60,
                70,
                40,
                30,
            }), 5, (float)Math.Pow((double)completionRatio, 1.0));
        }
        internal Color ArmColorFunction(float completionRatio)
        {
            float colorFade = 1f - Utils.GetLerpValue(0f, 1f, completionRatio, true);
            return new Color(252, 128, 48);
        }
        internal float ArmWidthFunction(float completionRatio)
        {
            return 30;
        }
        public void DrawB(SpriteBatch spriteBatch, Rectangle rect, Color color)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.UIScaleMatrix);
            float Scale = 0.75f;
            frameCounter++;
            frameCounter %= 64;
            frame = (int)(frameCounter / 8);
            Vector2 screenPos = new Vector2(rect.X, rect.Y);
            Vector2 Center = new Vector2(rect.Width, rect.Height * 0.8F) / 2;

            Vector2 LeftArmCen = Center - new Vector2(152, -18) * Scale;
            float LeftShoulder = -0.1F;

            Vector2 RightArmCen = Center - new Vector2(-152, -18) * Scale;
            float RightShoulder = 0.1F;

            Texture2D texture = TextureAssets.Npc[ModContent.NPCType<流星破坏者>()].Value;
            //尾巴
            Texture2D Extra = 流星破坏者.Extra.Value;
            Texture2D Extra2 = 流星破坏者.Extra2.Value;
            Texture2D Extra3 = 流星破坏者.Extra3.Value;
            //肩膀
            Texture2D Shoulder = 流星破坏者.Shoulder.Value;

            if (TrailDrawer == null)
            {
                TrailDrawer = new Trailing(new Trailing.VertexWidthFunction(WidthFunction), new Trailing.VertexColorFunction(ColorFunction), null, GameShaders.Misc["贴图拖尾"], 1);
            }
            if (TrailDrawer2 == null)
            {
                TrailDrawer2 = new Trailing(new Trailing.VertexWidthFunction(ArmWidthFunction), new Trailing.VertexColorFunction(ArmColorFunction), null, GameShaders.Misc["贴图拖尾"],1);
            }
            Vector2 vector;

            Vector2 Pos = Center + new Vector2(44, 34) * Scale;
            GameShaders.Misc["贴图拖尾"].SetShaderTexture(DDTextures.GlowTrail2);
            GameShaders.Misc["贴图拖尾"].Shader.Parameters["uSpeed"].SetValue(3f);
            Vector2[] vectors = [Pos, Pos + new Vector2(0.8f, 1) * (60) * Scale, Pos + new Vector2(0.8f, 1) * (120) * Scale];

            TrailDrawer.Draw(vectors, screenPos, 104, null, Scale * 1.3F, 1, spriteBatch);

            Pos = Center + new Vector2(-44, 34) * Scale;
            vectors = [Pos, Pos + new Vector2(-0.8f, 1) * (60) * Scale, Pos + new Vector2(-0.8f, 1) * (120) * Scale];

            TrailDrawer.Draw(vectors, screenPos, 104, null, Scale * 1.3F, 1, spriteBatch);

            vector = (Center + new Vector2(0, 120 * Scale)) - (Center + new Vector2(0, 90 * Scale));


            spriteBatch.Draw(Extra, (Center + new Vector2(0, 120 * Scale)) - vector * 0.33F + screenPos, new Rectangle(0, 0, Extra.Width, Extra.Height / 2), color, vector.ToRotation() - MathHelper.PiOver2, new Vector2(Extra.Width / 2, 0), Scale, 0, 0);

            spriteBatch.Draw(Extra2, (Center + new Vector2(0, 120 * Scale)) - vector * 0.66F + screenPos, new Rectangle(0, 0, Extra2.Width, Extra2.Height / 2), color, vector.ToRotation() - MathHelper.PiOver2, new Vector2(Extra2.Width / 2, 0), Scale, 0, 0);

            spriteBatch.Draw(Extra3, (Center + new Vector2(0, 120 * Scale)) + screenPos, new Rectangle(0, 0, Extra3.Width, Extra3.Height / 2), color, vector.ToRotation() - MathHelper.PiOver2, new Vector2(Extra3.Width / 2, 0), Scale, 0, 0);


            //左手
            Rectangle rectangle = new Rectangle(0, Shoulder.Height / 8 * frame, Shoulder.Width, Shoulder.Height / 8);
            Vector2 vector1 = new Vector2(46, 76);
            spriteBatch.Draw(Shoulder, Center - new Vector2(92, 38) * Scale + screenPos, rectangle, color, LeftShoulder, new Vector2(Shoulder.Width - 12, 28), Scale, SpriteEffects.FlipHorizontally, 0);

            //右手
            rectangle = new Rectangle(0, Shoulder.Height / 8 * frame, Shoulder.Width, Shoulder.Height / 8);
            vector1 = new Vector2(46, 76);
            spriteBatch.Draw(Shoulder, Center - new Vector2(-92, 38) * Scale + screenPos, rectangle, color, RightShoulder, new Vector2(12, 28), Scale, 0, 0);


            spriteBatch.Draw(texture, Center + screenPos, new Rectangle(0, texture.Height / 8 * frame, texture.Width / 2, texture.Height / 8), color, 0, new Vector2(texture.Width / 2, texture.Height / 8) / 2, Scale, 0, 0);



            RasterizerState state = new RasterizerState()
            {
                CullMode = CullMode.CullCounterClockwiseFace,
                ScissorTestEnable = true,
            };

            //左手
            texture = TextureAssets.Npc[ModContent.NPCType<流星大炮>()].Value;
            Texture2D Arm = 流星大炮.Arm.Value;
            Vector2 Center2 = LeftArmCen + new Vector2(-10, 180)*Scale;
            float ArmRot = (LeftArmCen - Center2).ToRotation() + 0.4f;
            rectangle = new Rectangle(0, Arm.Height / 8 * frame, Arm.Width, Arm.Height / 8);
            Main.spriteBatch.End();
            Vector2 ArmCen2 = Center - new Vector2(92, 38)*Scale;
            Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, state, null, Main.GameViewMatrix.TransformationMatrix);
            Pos = ArmCen2 + LeftShoulder.ToRotationVector2().PerfectNormalize().RotatedBy(-MathHelper.PiOver4 - 0.15F + MathHelper.Pi) * 64*Scale;

            GameShaders.Misc["贴图拖尾"].SetShaderTexture(DDTextures.GlowTrail2);
            GameShaders.Misc["贴图拖尾"].Shader.Parameters["uSpeed"].SetValue(3f);
            vector = LeftArmCen + (Center2 - LeftArmCen).RotatedBy(-0.1f).PerfectNormalize() * 28 * Scale;
            vectors = [Pos, LeftArmCen + (Pos - LeftArmCen) / 2, vector, vector, vector, vector];

            TrailDrawer2.Draw(vectors, screenPos, 104, null, Scale, 1, spriteBatch);

            spriteBatch.Draw(Arm, LeftArmCen + screenPos, rectangle, color, ArmRot, new Vector2(Arm.Width, 0), Scale, 0, 0);
            spriteBatch.Draw(DDTextures.VoidStar.Value, Pos + screenPos, null, new Color(252, 128, 48, 155), 0, DDTextures.VoidStar.Size() / 2, Scale / 2, 0, 0);
            spriteBatch.Draw(DDTextures.VoidStar.Value, Pos + screenPos, null, new Color(252, 128, 48, 155), 0, DDTextures.VoidStar.Size() / 2, Scale / 2, 0, 0);



            Vector2 Pos2 = LeftArmCen - (ArmRot - 0.5F).ToRotationVector2() * 104 * Scale;
            Vector2[] vectors2 = [Pos2, Center2 + (Pos2 - Center2) / 2, Center2, Center2, Center2, Center2];
            TrailDrawer2.Draw(vectors2, screenPos, 104, null, Scale, 1, spriteBatch);

            spriteBatch.Draw(DDTextures.VoidStar.Value, Pos2 = screenPos, null, new Color(252, 128, 48, 155), 0, DDTextures.VoidStar.Size() / 2, Scale / 2, 0, 0);
            spriteBatch.Draw(DDTextures.VoidStar.Value, Pos2 + screenPos, null, new Color(252, 128, 48, 155), 0, DDTextures.VoidStar.Size() / 2, Scale / 2, 0, 0);
            spriteBatch.Draw(DDTextures.VoidStar.Value, Center2 + screenPos, null, new Color(252, 128, 48, 155), 0, DDTextures.VoidStar.Size() / 2, Scale / 2, 0, 0);
            spriteBatch.Draw(DDTextures.VoidStar.Value, Center2 + screenPos, null, new Color(252, 128, 48, 155), 0, DDTextures.VoidStar.Size() / 2, Scale / 2, 0, 0);

            spriteBatch.Draw(texture, Center2 + screenPos, new Rectangle(0, texture.Height / 8 * frame, texture.Width / 2, texture.Height / 8), color, 0.6f, new Vector2(texture.Width / 2, texture.Height / 8) / 2, Scale, 0, 0);






            //右手
            texture = TextureAssets.Npc[ModContent.NPCType<流星激光枪>()].Value;
            Arm = 流星激光枪.Arm.Value;

            Center2 = RightArmCen + new Vector2(10, 180) * Scale;
            ArmRot = (Center2-RightArmCen).ToRotation() - 0.4f;
            rectangle = new Rectangle(0, Arm.Height / 8 * frame, Arm.Width, Arm.Height / 8);
            Main.spriteBatch.End();
            ArmCen2 = Center - new Vector2(-92, 38) * Scale;
            Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, state, null, Main.GameViewMatrix.TransformationMatrix);
            Pos = ArmCen2 + RightShoulder.ToRotationVector2().PerfectNormalize().RotatedBy(MathHelper.PiOver4 + 0.15F) * 64 * Scale;

            GameShaders.Misc["贴图拖尾"].SetShaderTexture(DDTextures.GlowTrail2);
            GameShaders.Misc["贴图拖尾"].Shader.Parameters["uSpeed"].SetValue(3f);
            vector = RightArmCen + (Center2 - RightArmCen).RotatedBy(0.1f).PerfectNormalize() * 28 * Scale;
            vectors = [Pos, RightArmCen + (Pos - RightArmCen) / 2, vector, vector, vector, vector];

            TrailDrawer2.Draw(vectors, screenPos, 104, null, Scale, 1, spriteBatch);

            spriteBatch.Draw(Arm, RightArmCen + screenPos, rectangle, color, ArmRot, new Vector2(0, 0), Scale, 0, 0);
            spriteBatch.Draw(DDTextures.VoidStar.Value, Pos + screenPos, null, new Color(252, 128, 48, 155), 0, DDTextures.VoidStar.Size() / 2, Scale / 2, 0, 0);
            spriteBatch.Draw(DDTextures.VoidStar.Value, Pos + screenPos, null, new Color(252, 128, 48, 155), 0, DDTextures.VoidStar.Size() / 2, Scale / 2, 0, 0);




            Pos2 = RightArmCen - (ArmRot + MathHelper.Pi + 0.5F).ToRotationVector2() * 104 * Scale;
            vectors2 = [Pos2, Center2 + (Pos2 - Center2) / 2, Center2, Center2, Center2, Center2];
            TrailDrawer2.Draw(vectors2, screenPos, 104, null, Scale, 1, spriteBatch);

            spriteBatch.Draw(DDTextures.VoidStar.Value, Pos2 = screenPos, null, new Color(252, 128, 48, 155), 0, DDTextures.VoidStar.Size() / 2, Scale / 2, 0, 0);
            spriteBatch.Draw(DDTextures.VoidStar.Value, Pos2 + screenPos, null, new Color(252, 128, 48, 155), 0, DDTextures.VoidStar.Size() / 2, Scale / 2, 0, 0);
            spriteBatch.Draw(DDTextures.VoidStar.Value, Center2 + screenPos, null, new Color(252, 128, 48, 155), 0, DDTextures.VoidStar.Size() / 2, Scale / 2, 0, 0);
            spriteBatch.Draw(DDTextures.VoidStar.Value, Center2 + screenPos, null, new Color(252, 128, 48, 155), 0, DDTextures.VoidStar.Size() / 2, Scale / 2, 0, 0);

            spriteBatch.Draw(texture, Center2 + screenPos, new Rectangle(0, texture.Height / 8 * frame, texture.Width / 2, texture.Height / 8), color, -0.6f, new Vector2(texture.Width / 2, texture.Height / 8) / 2, Scale,SpriteEffects.FlipHorizontally, 0);


        }
        int CAIDAN;
        public override void PostSetupContent()
		{
			if (!ModLoader.TryGetMod("BossChecklist", out Mod bossChecklistMod))
			{
				return;
			}
			if (bossChecklistMod.Version < new Version(1, 6))
			{
				return;
			}

			int bossType = ModContent.NPCType<流星破坏者>();

			List<int> collection = new List<int>()
			{
			};

			string despawnInfo = null;
			var customBossPortrait = (SpriteBatch sb, Rectangle rect, Color color) =>
			{
                var BossDraw = (SpriteBatch sb, Rectangle rect, Color color) =>
                {
                    if (CAIDAN > 0)
                    {
                        CAIDAN--;
                    }
                    if (Main.rand.NextBool(10000))
                    {
                        CAIDAN = 3000;
                    }

                    if (Main.rand.NextBool(100))
                    {
                        float A = Main.rand.NextFloat(1f, 1f);
                        int GoreType = Mod.Find<ModGore>("Cloud_" + Main.rand.Next(22)).Type;
                        if (Main.rand.NextBool(300))
                        {
                            GoreType = Mod.Find<ModGore>("Cloud_" + Main.rand.Next(22, 41)).Type;
                        }
                        if (CAIDAN > 0)
                        {
                            GoreType = Mod.Find<ModGore>("Cloud_" + Main.rand.Next(22, 41)).Type;
                        }
                        int Gore = BGore.NewGore(new Vector2(rect.Width + 16, Main.rand.NextFloat(-300, rect.Height / 4)), new Vector2(-0.5f * A, 0), GoreType, A, Main.rand.Next(3), ChecklistHelper.流星破坏者);
                        ChecklistHelper.Gore[Gore].gravity = false;
                        ChecklistHelper.Gore[Gore].color = new Color(255, 255, 255, 150) * Main.rand.NextFloat(0.8F, 1F);
                        if (ChecklistHelper.Gore[Gore].post==0)
                        {
                            ChecklistHelper.Gore[Gore].velocity *= 0.4F;
                            ChecklistHelper.Gore[Gore].color = new Color(255, 255, 255, 150) * Main.rand.NextFloat(0.4F, 0.6F);
                        }
                        if (ChecklistHelper.Gore[Gore].post==1)
                        {
                            ChecklistHelper.Gore[Gore].velocity *= 0.7F;
                            ChecklistHelper.Gore[Gore].color = new Color(255, 255, 255, 150) * Main.rand.NextFloat(0.1F, 0.2F);
                        }
                        ChecklistHelper.Gore[Gore].scale = Main.rand.NextFloat(0.2F, 1.4F);
                        if (CAIDAN > 0)
                        {
                            //ChecklistHelper.Gore[Gore].color = new Color(Main.rand.Next(256), Main.rand.Next(256), Main.rand.Next(256), Main.rand.Next(256));
                        }
                        ChecklistHelper.Gore[Gore].Norotating = true;
                    }
                    Texture2D texture = ModContent.Request<Texture2D>("Terraria/Images/Background_7").Value;
                    Background = 0;

                    sb.Draw(texture, rect.TopLeft() + new Vector2(Background, rect.Height - 200), null, color, 0, new Vector2(0, texture.Height), 0.5F, 0, 0);
                    sb.Draw(texture, rect.TopLeft() + new Vector2(Background + texture.Width * 0.5F, rect.Height - 200), null, color, 0, new Vector2(0, texture.Height), 0.5F, 0, 0);
                    
                    Background2 = 0;
                    for (int a = 0; a < ChecklistHelper.Gore.Length; a++)
                    {
                        if (ChecklistHelper.Gore[a] != null && ChecklistHelper.Gore[a].post==0)
                        {
                            ChecklistHelper.Gore[a].Dawn(sb, rect, ChecklistHelper.流星破坏者);
                        }
                    }
                    texture = ModContent.Request<Texture2D>("Terraria/Images/Background_116").Value;
                    sb.Draw(texture, rect.TopLeft() + new Vector2(Background2, rect.Height - 100), null, color, 0, new Vector2(0, texture.Height), 0.6F, 0, 0);
                    sb.Draw(texture, rect.TopLeft() + new Vector2(Background2 + texture.Width * 0.6F, rect.Height - 100), null, color, 0, new Vector2(0, texture.Height), 0.6F, 0, 0);

                    texture = ModContent.Request<Texture2D>("Terraria/Images/Background_94").Value;
                    Background3 = 0;
                    for (int a = 0; a < ChecklistHelper.Gore.Length; a++)
                    {
                        if (ChecklistHelper.Gore[a] != null && ChecklistHelper.Gore[a].post==1)
                        {
                            ChecklistHelper.Gore[a].Dawn(sb, rect, ChecklistHelper.流星破坏者);
                        }
                    }
                    sb.Draw(texture, rect.TopLeft() + new Vector2(Background3, rect.Height + 200), null, color, 0, new Vector2(0, texture.Height), 1, 0, 0);
                    sb.Draw(texture, rect.TopLeft() + new Vector2(Background3 + texture.Width, rect.Height + 200), null, color, 0, new Vector2(0, texture.Height), 1, 0, 0);

                    for (int a = 0; a < ChecklistHelper.Gore.Length; a++)
                    {
                        if (ChecklistHelper.Gore[a] != null && ChecklistHelper.Gore[a].post == 2)
                        {
                            ChecklistHelper.Gore[a].Dawn(sb, rect, ChecklistHelper.流星破坏者);
                        }
                    }
                    for (int a = 0; a < ChecklistHelper.Gore.Length; a++)
                    {
                        if (ChecklistHelper.Gore[a] != null)
                        {
                            if (ChecklistHelper.Gore[a].active)
                            {
                                if (ChecklistHelper.Gore[a].position.Y > (rect.Height * 2))
                                {
                                    ChecklistHelper.Gore[a].active = false;
                                }
                                else if (ChecklistHelper.Gore[a].position.Y < (-rect.Height * 2))
                                {
                                    ChecklistHelper.Gore[a].active = false;
                                }
                                else if (ChecklistHelper.Gore[a].position.X > (rect.Width * 2))
                                {
                                    ChecklistHelper.Gore[a].active = false;
                                }
                                else if (ChecklistHelper.Gore[a].position.X < (-rect.Width * 2))
                                {
                                    ChecklistHelper.Gore[a].active = false;
                                }
                                ChecklistHelper.Gore[a].UpdateGore(a);
                            }
                        }
                    }

                    DrawB(sb, rect, color);
                };
                sb.BossChecklistDraw(BossChecklistBook,rect,color, BossDraw);
			};
			int summonItem = ModContent.ItemType<高压流星电池>();
			bossChecklistMod.Call(
				"LogBoss",
				Mod,
                "流星破坏者",
				11.8f,
				() => NPCDowned.流星破坏者,
				bossType,
				new Dictionary<string, object>()
				{
					//["displayName"] = Language.GetText("Mods.DDmod.NPCs.LifeGuard.BossChecklistIntegration.EntryName2"),
					//召唤物
					["spawnItems"] = summonItem,
					//收藏品
					//["collectibles"] = collection,
					//绘制
					["customPortrait"] = customBossPortrait,
				}
			);

		}
	}
}