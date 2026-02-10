using DDmod.Content.Dusts;
using DDmod.Content.Items.Boss.MeteorDiggerItems;
using DDmod.Content.Items.Boss.流星破坏者;
using DDmod.Content.Items.Boss.狱火蛇物品;
using DDmod.Content.Items.Boss.绿岩之视;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.NPCs.Boss.MeteorDigger;
using DDmod.Content.NPCs.Boss.天雷怒云;
using DDmod.Content.NPCs.Boss.恐惧缝合体;
using DDmod.Content.NPCs.Boss.流星破坏者;
using DDmod.Content.NPCs.Boss.狱火蛇;
using DDmod.Content.NPCs.Boss.绿岩之视;
using DDmod.Content.NPCs.Boss.鬼牙;
using DDmod.Worlds;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection;
using System.Runtime.InteropServices;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace DDmod.ModLinkage.BossChecklist
{
    public class 天雷怒云Checklist : ModSystem
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
			BossChecklistBook = ModContent.Request<Texture2D>("DDmod/ModLinkage/BossChecklist/天空乌云Checklist");
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
        public void DrawB(SpriteBatch spriteBatch, Rectangle rect, Color color)
        {
            float Scale = 1.3f;
            frameCounter++;
            frameCounter %= 36;
            frame = (int)(frameCounter / 6);
            Vector2 screenPos = new Vector2(rect.X, rect.Y);
            Vector2 Center = new Vector2(rect.Width, rect.Height) / 2;

            Texture2D texture = TextureAssets.Npc[ModContent.NPCType<天雷怒云>()].Value;

            Rectangle rectangle = new Rectangle(0, texture.Height / 6 * frame, texture.Width/2, texture.Height / 6);
            spriteBatch.Draw(texture, Center + screenPos, rectangle, new Color(100,100,100,255), 0, rectangle.Size() / 2, Scale,0, 0f);
            spriteBatch.Draw(天雷怒云.Glow.Value, Center + screenPos, rectangle, Color.White, 0, rectangle.Size() / 2, Scale,0, 0f);


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

			int bossType = ModContent.NPCType<天雷怒云>();

			List<int> collection = new List<int>()
			{
			};

			string despawnInfo = null;
			var customBossPortrait = (SpriteBatch sb, Rectangle rect, Color color) =>
			{
                var BossDraw = (SpriteBatch sb, Rectangle rect, Color color) =>
                {
                    color *= 0.15F;
                    color.A = 255;
                    if (CAIDAN > 0)
                    {
                        CAIDAN--;
                    }
                    if (Main.rand.NextBool(10000))
                    {
                        CAIDAN = 3000;
                    }

                    if (Main.rand.NextBool(20))
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
                        int Gore = BGore.NewGore(new Vector2(rect.Width + 16, Main.rand.NextFloat(-300, rect.Height / 4)), new Vector2(-2f * A, 0), GoreType, A, Main.rand.Next(3), ChecklistHelper.天雷怒云);
                        ChecklistHelper.Gore[Gore].gravity = false;
                        Color Rcolor = new Color(25,25, 25, 200);
                        ChecklistHelper.Gore[Gore].color = Rcolor * Main.rand.NextFloat(0.9F, 1F);
                        if (ChecklistHelper.Gore[Gore].post==0)
                        {
                            ChecklistHelper.Gore[Gore].velocity *= 0.4F;
                            ChecklistHelper.Gore[Gore].color = Rcolor * Main.rand.NextFloat(0.6F, 0.8F);
                        }
                        if (ChecklistHelper.Gore[Gore].post==1)
                        {
                            ChecklistHelper.Gore[Gore].velocity *= 0.7F;
                            ChecklistHelper.Gore[Gore].color = Rcolor * Main.rand.NextFloat(0.4F, 0.5F);
                        }
                        ChecklistHelper.Gore[Gore].scale = Main.rand.NextFloat(0.75F, 2F);
                        if (CAIDAN > 0)
                        {
                            //ChecklistHelper.Gore[Gore].color = new Color(Main.rand.Next(256), Main.rand.Next(256), Main.rand.Next(256), Main.rand.Next(256));
                        }
                        ChecklistHelper.Gore[Gore].Norotating = true;
                    }
                    if (Main.rand.NextBool(1))
                    {
                        int GoreType = Mod.Find<ModGore>("雨").Type;

                        int Gore = BGore.NewGore(new Vector2(Main.rand.Next(-200, rect.Width+100), -200), new Vector2(3, 9)*3, GoreType, Main.rand.NextFloat(1f, 1.5f), Main.rand.Next(3), ChecklistHelper.天雷怒云);
                        ChecklistHelper.Gore[Gore].gravity = false;
                        Color Rcolor = new Color(45, 45, 45, 50);
                        ChecklistHelper.Gore[Gore].color = Rcolor * Main.rand.NextFloat(0.9F, 1F);
                        if (ChecklistHelper.Gore[Gore].post == 0)
                        {
                            ChecklistHelper.Gore[Gore].velocity *= 0.4F;
                            ChecklistHelper.Gore[Gore].color = Rcolor * Main.rand.NextFloat(0.6F, 0.8F);
                            ChecklistHelper.Gore[Gore].scale *= 0.75F;
                        }
                        if (ChecklistHelper.Gore[Gore].post == 1)
                        {
                            ChecklistHelper.Gore[Gore].velocity *= 0.7F;
                            ChecklistHelper.Gore[Gore].color = Rcolor * Main.rand.NextFloat(0.4F, 0.5F);
                            ChecklistHelper.Gore[Gore].scale *= 0.5F;
                        }
                        ChecklistHelper.Gore[Gore].rotating = new Vector2(3, 9).ToRotation()+MathHelper.PiOver2;
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
                            ChecklistHelper.Gore[a].Dawn(sb, rect, ChecklistHelper.天雷怒云);
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
                            ChecklistHelper.Gore[a].Dawn(sb, rect, ChecklistHelper.天雷怒云);
                        }
                    }
                    sb.Draw(texture, rect.TopLeft() + new Vector2(Background3, rect.Height + 200), null, color, 0, new Vector2(0, texture.Height), 1, 0, 0);
                    sb.Draw(texture, rect.TopLeft() + new Vector2(Background3 + texture.Width, rect.Height + 200), null, color, 0, new Vector2(0, texture.Height), 1, 0, 0);

                    DrawB(sb, rect, color);
                    for (int a = 0; a < ChecklistHelper.Gore.Length; a++)
                    {
                        if (ChecklistHelper.Gore[a] != null && ChecklistHelper.Gore[a].post == 2)
                        {
                            ChecklistHelper.Gore[a].Dawn(sb, rect, ChecklistHelper.天雷怒云);
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

                };
                sb.BossChecklistDraw(BossChecklistBook,rect,color, BossDraw);
			};
			int summonItem = ModContent.ItemType<Content.Items.Boss.天雷怒云.引雷针>();
			bossChecklistMod.Call(
				"LogBoss",
				Mod,
                "天雷怒云",
				8.5f,
				() => NPCDowned.天雷怒云,
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