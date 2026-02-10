using DDmod.Content.Dusts;
using DDmod.Content.Items.Boss.MeteorDiggerItems;
using DDmod.Content.Items.Boss.狱火蛇物品;
using DDmod.Content.Items.Boss.绿岩之视;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.NPCs.Boss.MeteorDigger;
using DDmod.Content.NPCs.Boss.恐惧缝合体;
using DDmod.Content.NPCs.Boss.狱火蛇;
using DDmod.Content.NPCs.Boss.绿岩之视;
using DDmod.Content.NPCs.Boss.鬼牙;
using DDmod.Worlds;
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.InteropServices;
using Terraria;
using Terraria.ModLoader;

namespace DDmod.ModLinkage.BossChecklist
{
    public class 绿岩之视Checklist : ModSystem
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
		public void DrawB(SpriteBatch sb, Rectangle rect, Color color)
        {
            DDHelper.BackAndForth(1.5F, 1.75F, 0.02F, ref Drift, ref DriftBool);

            Vector2 centered = new Vector2(rect.X, rect.Y);
            BossPosition = new Vector2(rect.Width / 2, rect.Height / 2f) + centered;
			if (EPosition == Vector2.Zero)
			{
				EPosition = new Vector2(rect.Width / 2, rect.Height / 2f) + centered;
				EPosition2 = new Vector2(Main.rand.NextFloat(100, 1000), Main.rand.NextFloat(-1000, 1000));

            }
			if(Main.rand.NextBool(100))
            EPosition2 = new Vector2(Main.rand.NextFloat(500, 1000), Main.rand.NextFloat(-500, 500));
            Vector2 vector2 = EPosition2 / 10;

            if (vector2.Length() > 30)
            {
                vector2 = vector2.PerfectNormalize() * 30;
            }
            EPosition += (vector2 - EPosition).PerfectNormalize() * (vector2 - EPosition).Length() / 10;

            float Rotation = 0.2f;

            Texture2D texture = DDTextures.VoidStar.Value;
            Rectangle rectangle = new Rectangle(0, texture.Height / 2, texture.Width, texture.Height / 2);

            sb.Draw(texture, BossPosition + new Vector2(-58, 0).RotatedBy(Rotation), rectangle, new Color(100, 255, 100, 0), Rotation, new Vector2(texture.Width / 2, 0), new Vector2(0.25F, Drift), 0, 0);
            sb.Draw(texture, BossPosition + new Vector2(-58, 0).RotatedBy(Rotation), rectangle, new Color(100, 255, 100, 0), Rotation, new Vector2(texture.Width / 2, 0), new Vector2(0.25F, Drift), 0, 0);
            texture = TextureAssets.Npc[ModContent.NPCType<绿岩炮>()].Value;

            SpriteEffects sprite = 0;
			rectangle = new Rectangle(0, 0, texture.Width / 2, texture.Height / 2);
			sb.Draw(texture, (BossPosition - new Vector2(-1, 64).RotatedBy(Rotation)), rectangle, color, 1.7f, rectangle.Size() / 2, 1, sprite, 0);


			frameCounter++;
			if (frameCounter >= 6)
			{
				frameCounter = 0;
				frame++;
			}
			if (frame >= 4)
			{
				frame = 0;
			}
			for (int a = 0; a < 6; a++)
			{
				DDHelper.BackAndForth(-20, 20, 0.5F, ref Time[a], ref Bool[a]);
			}
            sprite = SpriteEffects.FlipHorizontally;

            texture = TextureAssets.Npc[ModContent.NPCType<绿岩之视>()].Value;
            rectangle = new Rectangle(0, texture.Height / 4*frame, texture.Width/4, texture.Height / 4);
            sb.Draw(texture, BossPosition, rectangle, color, Rotation, new Vector2(texture.Width / 4, texture.Height/4) / 2, 1F, sprite, 0f);
			rectangle.X += texture.Width / 4;
            sb.Draw(texture, BossPosition, rectangle, color, Rotation, new Vector2(texture.Width / 4, texture.Height/4) / 2, 1F, sprite, 0f);
            rectangle.X += texture.Width / 4;
            sb.Draw(texture, BossPosition, rectangle, color, Rotation, new Vector2(texture.Width / 4, texture.Height/4) / 2, 1F, sprite, 0f);
            rectangle.X += texture.Width / 4;
            sb.Draw(texture, BossPosition, rectangle, color, Rotation, new Vector2(texture.Width / 4, texture.Height/4) / 2, 1F, sprite, 0f);

			
			
			texture = 绿岩之视.asset.Value;
			Vector2 vector = EPosition;
			rectangle = new Rectangle(0, 0, texture.Width/2, texture.Height / 2);
			sprite = SpriteEffects.FlipVertically;
			sb.Draw(texture, BossPosition + vector, rectangle, Color.White, vector.ToRotation(), rectangle.Size() / 2, new Vector2(1 - vector.Length() / 120, 1), sprite, 0);
			sb.Draw(texture, BossPosition + vector, rectangle, new Color(100, 255, 100, 0) * 0.4F, vector.ToRotation(), rectangle.Size() / 2, new Vector2(1 - vector.Length() / 120, 1), sprite, 0);


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

			int bossType = ModContent.NPCType<绿岩之视>();

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

                    if (Main.rand.NextBool(10))
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
                        int Gore = BGore.NewGore(new Vector2(rect.Width + 16, Main.rand.NextFloat(-300, rect.Height / 4)), new Vector2(-4 * A, 0), GoreType, A, Main.rand.Next(3), ChecklistHelper.绿岩之视);
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
                    Background -= 1;
                    if (Background <= -texture.Width / 2)
                    {
                        Background = 0;
                    }

                    sb.Draw(texture, rect.TopLeft() + new Vector2(Background, rect.Height - 200), null, Color.White, 0, new Vector2(0, texture.Height), 0.5F, 0, 0);
                    sb.Draw(texture, rect.TopLeft() + new Vector2(Background + texture.Width * 0.5F, rect.Height - 200), null, Color.White, 0, new Vector2(0, texture.Height), 0.5F, 0, 0);
                    
                    Background2 -= 3;
                    if (Background2 <= -texture.Width * 0.6F)
                    {
                        Background2 = 0;
                    }
                    for (int a = 0; a < ChecklistHelper.Gore.Length; a++)
                    {
                        if (ChecklistHelper.Gore[a] != null && ChecklistHelper.Gore[a].post==0)
                        {
                            ChecklistHelper.Gore[a].Dawn(sb, rect, ChecklistHelper.绿岩之视);
                        }
                    }
                    texture = ModContent.Request<Texture2D>("Terraria/Images/Background_116").Value;
                    sb.Draw(texture, rect.TopLeft() + new Vector2(Background2, rect.Height - 100), null, Color.White, 0, new Vector2(0, texture.Height), 0.6F, 0, 0);
                    sb.Draw(texture, rect.TopLeft() + new Vector2(Background2 + texture.Width * 0.6F, rect.Height - 100), null, Color.White, 0, new Vector2(0, texture.Height), 0.6F, 0, 0);

                    texture = ModContent.Request<Texture2D>("Terraria/Images/Background_94").Value;
                    Background3 -= 5;
                    if (Background3 <= -texture.Width)
                    {
                        Background3 = 0;
                    }
                    for (int a = 0; a < ChecklistHelper.Gore.Length; a++)
                    {
                        if (ChecklistHelper.Gore[a] != null && ChecklistHelper.Gore[a].post==1)
                        {
                            ChecklistHelper.Gore[a].Dawn(sb, rect, ChecklistHelper.绿岩之视);
                        }
                    }
                    sb.Draw(texture, rect.TopLeft() + new Vector2(Background3, rect.Height + 200), null, Color.White, 0, new Vector2(0, texture.Height), 1, 0, 0);
                    sb.Draw(texture, rect.TopLeft() + new Vector2(Background3 + texture.Width, rect.Height + 200), null, Color.White, 0, new Vector2(0, texture.Height), 1, 0, 0);

                    for (int a = 0; a < ChecklistHelper.Gore.Length; a++)
                    {
                        if (ChecklistHelper.Gore[a] != null && ChecklistHelper.Gore[a].post == 2)
                        {
                            ChecklistHelper.Gore[a].Dawn(sb, rect, ChecklistHelper.绿岩之视);
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
			int summonItem = ModContent.ItemType<绿岩信号增幅器>();
			bossChecklistMod.Call(
				"LogBoss",
				Mod,
                "绿岩之视",
				4.9f,
				() => NPCDowned.绿岩之视,
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