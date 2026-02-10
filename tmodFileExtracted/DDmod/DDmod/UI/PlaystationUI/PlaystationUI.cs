using DDmod.Content.Items.Sundries;
using DDmod.Content.NPCs.Boss.MeteorAnnihilator;
using DDmod.Modkey;
using DDmod.ModLinkage.BossChecklist;
using DDmod.Players;
using DDmod.UI.HunterQuests;
using DDmod.UI.ItemUI;
using DDmod.UI.ItemUI.背包;
using DDmod.UI.PlaystationUI.Game;
using DDmod.UI.PlaystationUI.Game.MeteorsPlane;
using MonoMod.Utils;
using System.Collections;
using Terraria.Chat;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.UI;
using static DDmod.Players.DDPlayer;

namespace DDmod.UI.PlaystationUI
{
    public static class PlaystationUI
    {
        
        public static void DrawUI(SpriteBatch spriteBatch)
        {
            if(Main.gameMenu)
            {
                return;
            }
            if (Main.LocalPlayer.GetModPlayer<PlaystationPlayer>().Start<0)
            {
                return;
            }
            //盖帽
            Vector2 vector = new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);
            Vector2 ScreenSize = new Vector2(218, 36);
            Texture2D Cover;
            Texture2D Screen;
            if (Main.LocalPlayer.GetModPlayer<PlaystationPlayer>().UIStyle == 1)
            {
                Cover = ModContent.Request<Texture2D>("DDmod/UI/PlaystationUI/PlaystationUI"+ (Main.LocalPlayer.GetModPlayer<PlaystationPlayer>().UIStyle+1)).Value;
                Screen = ModContent.Request<Texture2D>("DDmod/UI/PlaystationUI/PlaystationUI" + (Main.LocalPlayer.GetModPlayer<PlaystationPlayer>().UIStyle + 1) + "_Screen").Value;
            }
            else
            {
                Cover = ModContent.Request<Texture2D>("DDmod/UI/PlaystationUI/PlaystationUI").Value;
                Screen = ModContent.Request<Texture2D>("DDmod/UI/PlaystationUI/PlaystationUI2_Screen").Value;
            }
            spriteBatch.Draw(Screen, vector, null, Color.White, 0, Cover.Size() / 2, 1, 0, 0);
            MiniGameMian meteors = PlaystationSystem.Playstation[Main.LocalPlayer.GetModPlayer<PlaystationPlayer>().Start];
            if(meteors==null)
            {
                return;
            }

            spriteBatch.DrawrectBegin(new Rectangle((int)vector.X- Cover.Width/2+218, (int)vector.Y- Cover.Height/2+36, 364, 402), BlendState.AlphaBlend, Main.UIScaleMatrix, out SamplerState anisotropicClamp, out RasterizerState rasterizerState, out Rectangle scissorRectangle);
            meteors.Draw(spriteBatch, vector - Cover.Size() / 2 + ScreenSize);

            spriteBatch.DrawrectEnd(BlendState.AlphaBlend, Main.UIScaleMatrix, anisotropicClamp, rasterizerState, scissorRectangle);

            spriteBatch.Draw(Cover, vector, null, Color.White, 0, Cover.Size() / 2, 1, 0, 0);


        }
    }
    public class PlaystationSystem : ModSystem
    {
        public static Vector2 ScreenSize = new Vector2(364, 402);
        public static List<MiniGameMian> Playstation;
        public const int SelectGame = 0;
        public const int MeteorPlane = 1;
        public override void Load()
        {
            Playstation =
            [
                new MiniGameMian(),
                //刘醒飞机小游戏
                new MeteorsPlaneMian(MeteorPlane),
            ];
        }
        public override void Unload()
        {
            Playstation = null;
        }
        public override void PreUpdateTime()
        {
            for (int a = 0; a < Playstation.Count; a++)
            {
                Playstation[a].CheckActive();
                if (Playstation[a].Active)
                {
                    Playstation[a].Update();
                    Playstation[a].UpdateProj();
                    Playstation[a].UpdateNPC();
                    Playstation[a].UpdateDust();
                    Playstation[a].UpdateGore();
                } 
            }
        }
    }
    public class PlaystationPlayer : ModPlayer
    {
        public NPC npc = new NPC();
        /// <summary>
        /// 正在玩的游戏
        /// </summary>
        public int Start= -1;
        /// <summary>
        /// 
        /// </summary>
        public const int Greenstone = 1;
        /// <summary>
        /// UI样式
        /// </summary>
        public int UIStyle = -1;
        public int StartCDTime = 0;
        public Point16 point = Point16.Zero;
        public void Sync()
        {
            Mod mod = DDmod.Instance;
            ModPacket packet = mod.GetPacket(256);
            packet.Write((byte)DDType.PlayersGame);
            packet.Write((byte)Player.whoAmI);
            packet.Write(point.X);
            packet.Write(point.Y);
            packet.Send(-1, Player.whoAmI);
        }
        public override void PostUpdateMiscEffects()
        {
            if (StartCDTime > 0)
            {
                StartCDTime--;
                Mini_game game = Player.Dplayer().Mini_game_shortcuts;
                game.ResetControls();
                Main.playerInventory = false;
            }
            if (Main.netMode != 2)
            {
                if (Start >= 0)
                {
                    Main.playerInventory = false;
                    Main.LocalPlayer.Dplayer().Bossperspective(Player.Center + new Vector2(250 * Main.UIScale, 0), 2, false, 0.1F);
                    if (StartCDTime <= 0)
                    {
                        PlaystationSystem.Playstation[Start].ControlPlayer(Player);
                    }
                }
            }
        }
        public override void PreUpdate()
        {
        }
    }
}