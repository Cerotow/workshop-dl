using DDmod.Content.Dusts;
using DDmod.Content.Items.Tiles.绿岩;
using DDmod.UI.PlaystationUI;
using Humanizer;
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

namespace DDmod.Content.Tiles.绿岩
{
    public class 绿岩游戏机Tile : ModTile
    {
        public const int NextStyleHeight = 56;

        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileID.Sets.HasOutlines[Type] = true;
            TileID.Sets.CanBeSatOnForNPCs[Type] = true;
            TileID.Sets.CanBeSatOnForPlayers[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;

            DustType = ModContent.DustType<绿岩粒子>();
            AddMapEntry(new Color(100, 255, 100), CreateMapEntryName());

            TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 18 };
            TileObjectData.newTile.Width = 3;
            TileObjectData.newTile.Origin = new Point16(1, 2);
            TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
            TileObjectData.newTile.StyleWrapLimit = 2;
            TileObjectData.newTile.StyleMultiplier = 2;
            TileObjectData.newTile.StyleHorizontal = true;

            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
            TileObjectData.addAlternate(1);
            TileObjectData.addTile(Type);
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
        public override bool CreateDust(int i, int j, ref int type)
        {
            NewDust(new Vector2(i, j) * 16, 16, 16, ModContent.DustType<绿岩电光粒子>(), Main.rand.NextFloat(-4, 4), Main.rand.NextFloat(-4, 4), Scale: Main.rand.NextFloat(0.75F, 1.25F));
            return base.CreateDust(i, j, ref type);
        }
        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
        {
            return settings.player.IsWithinSnappngRangeToTile(i, j, PlayerSittingHelper.ChairSittingMaxDistance);
        }

        public override bool RightClick(int i, int j)
        {
            Player player = Main.LocalPlayer;
            Tile t = Main.tile[i, j];
            int left = i - t.TileFrameX % (18 * 3) / 18;
            int top = j - t.TileFrameY % (18 * 3) / 18;

            for (int A = 0; A < 255; A++)
            {
                if (player.active && player.GetModPlayer<PlaystationPlayer>().point == new Point16(left, top))
                {
                    return false;
                }

            }
            player.GetModPlayer<PlaystationPlayer>().Start = 0;
            player.GetModPlayer<PlaystationPlayer>().UIStyle = PlaystationPlayer.Greenstone;
            player.GetModPlayer<PlaystationPlayer>().point = new Point16(left, top);
            DDmod.SyncData(DDType.PlayersGame, player.whoAmI, -1, player.whoAmI);

            return true;
        }

        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;

            if (!player.IsWithinSnappngRangeToTile(i, j, PlayerSittingHelper.ChairSittingMaxDistance))
            {
                return;
            }

            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
            player.cursorItemIconID = ModContent.ItemType<绿岩游戏机>();

            if (Main.tile[i, j].TileFrameX / 35 < 1)
            {
                player.cursorItemIconReversed = true;
            }
        }
        public Asset<Texture2D> TileTexture;

        public string T => "DDmod/Content/Tiles/绿岩/绿岩游戏机Tile_Glow";

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
            int height = (tile.TileFrameY == 36|| tile.TileFrameY==92) ? 18 : 16;
            Main.spriteBatch.Draw(TileTexture.Value, new Vector2((float)(i * 16 - (int)Main.screenPosition.X), (float)(j * 16 - (int)Main.screenPosition.Y)) + zero, new Rectangle?(new Rectangle((int)tile.TileFrameX, (int)tile.TileFrameY, 16, height)), Color.White, 0f, Vector2.Zero, 1f, 0, 0f);

            int left = i - tile.TileFrameX % (18 * 3) / 18;
            int top = j - tile.TileFrameY % (18 * 3) / 18;
            for (int A = 0; A < 255; A++)
            {
                if (player.active && player.GetModPlayer<PlaystationPlayer>().point == new Point16(left, top))
                {
                    Main.spriteBatch.Draw(TileTexture.Value, new Vector2((float)(i * 16 - (int)Main.screenPosition.X), (float)(j * 16 - (int)Main.screenPosition.Y)) + zero, new Rectangle?(new Rectangle((int)tile.TileFrameX, (int)tile.TileFrameY+56, 16, height)), Color.White, 0f, Vector2.Zero, 1f, 0, 0f);
                }

            }
        }
    }
}