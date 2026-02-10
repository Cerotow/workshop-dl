
using DDmod.Content.Items.Tiles.晶凝;
using DDmod.Content.Items.Tiles.花岗岩基地;
using DDmod.UI.PlaystationUI;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using DDmod.Content.NPCs.IittleMonster;
using DDmod.Content.Projectiles.GeneralProj;
using DDmod.Content.Items.Tiles.流星;
using DDmod.Content.Items.Boss.流星破坏者;

namespace DDmod.Content.Tiles.流星
{
	public class 流星信号塔Tile : ModTile
	{
		public const int NextStyleHeight = 56;

		public override void SetStaticDefaults() {
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileID.Sets.HasOutlines[Type] = true;
			TileID.Sets.CanBeSatOnForNPCs[Type] = true;
			TileID.Sets.CanBeSatOnForPlayers[Type] = true;
			TileID.Sets.DisableSmartCursor[Type] = true;

			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsChair);

			DustType = 12;

			AddMapEntry(new Color(200, 20, 20), Language.GetText("MapObject.Chair"));

			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16 ,16};
            TileObjectData.newTile.CoordinatePaddingFix = new Point16(0, 2);
			TileObjectData.addTile(Type);
		}

		public override void NumDust(int i, int j, bool fail, ref int num) {
			num = fail ? 1 : 3;
		}

		public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings) {
			return settings.player.IsWithinSnappngRangeToTile(i, j, 100); // Avoid being able to trigger it from long range
		}

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
        }
        public override bool RightClick(int i, int j)
        {
            Player player = Main.LocalPlayer;
            if (!player.IsWithinSnappngRangeToTile(i, j, 100))
            {
                return false;
            }
            for (int A = 0; A < player.inventory.Length-1; A++)
            {
                if (player.inventory[A].type > 0 && player.inventory[A].type == ModContent.ItemType<高压流星电池>())
                {
                    player.inventory[A].stack--;
                    Tile tile = Main.tile[i, j];
                    int left = i - (int)tile.TileFrameX % (3 * 16) / 16;
                    int top = j - (int)tile.TileFrameY % (4 * 16) / 16;
                    NewProjectile(player.GetSource_FromAI(), new Vector2(left, top) * 16 + new Vector2(24.5F, 15), Vector2.Zero, ModContent.ProjectileType<流星信号>(), 0, 0);
                    break;
                }
            }
            return true;
        }

		public override void MouseOver(int i, int j) {
			Player player = Main.LocalPlayer;

            if (!player.IsWithinSnappngRangeToTile(i, j, 100))
            { 
				return;
			}

			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
			player.cursorItemIconID = ModContent.ItemType<高压流星电池>();

			if (Main.tile[i, j].TileFrameX / 35 < 1) {
				player.cursorItemIconReversed = true;
			}
        }
        public Asset<Texture2D> TileTexture;
        public override void NearbyEffects(int i, int j, bool closer)
        {
            Tile t = Main.tile[i, j];
        }
        public string T => "DDmod/Content/Tiles/流星/流星信号塔Tile_Glow";
        public override void Load()
        {
            if (!Main.dedServ)
            {
                TileTexture = ModContent.Request<Texture2D>(T);
            }
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Player player = Main.LocalPlayer;
            Tile tile = Main.tile[i, j];
            Vector2 zero = new Vector2((float)Main.offScreenRange, (float)Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            int height = (tile.TileFrameY == 36) ? 18 : 16;
            Main.spriteBatch.Draw(TileTexture.Value, new Vector2((float)(i * 16 - (int)Main.screenPosition.X), (float)(j * 16 - (int)Main.screenPosition.Y)) + zero, new Rectangle?(new Rectangle((int)tile.TileFrameX, (int)tile.TileFrameY, 16, height)), Color.White, 0f, Vector2.Zero, 1f, 0, 0f);

        }
    }
}
