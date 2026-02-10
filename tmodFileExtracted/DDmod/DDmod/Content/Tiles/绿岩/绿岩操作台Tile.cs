using DDmod.Content.Dusts;
using DDmod.Content.Items.Series.绿岩;
using DDmod.Content.Items.Tiles.绿岩;
using DDmod.Content.Items.农场.食物;
using DDmod.Content.Items.农场.种子;
using DDmod.Content.Tiles.农场;
using DDmod.Worlds;
using Terraria;
using Terraria.Enums;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ObjectData;

namespace DDmod.Content.Tiles.绿岩
{
    public class 绿岩操作台Tile : ModTile
    {
        public const int NextStyleHeight = 38;

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            if (Main.tile[i, j].TileFrameY < 162)
            {
                r = 0.1f;
                g = 0.7f;
                b = 0.1f;
            }
        }
        public override void SetStaticDefaults()
        {
            Main.tileLighted[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            Main.tileFrameImportant[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;
            TileID.Sets.IgnoredByNpcStepUp[Type] = true;
            TileID.Sets.HasOutlines[Type] = true;
            MinPick = 100;

            DustType = ModContent.DustType<绿岩粒子>();
            DDSystem.Instance.DDEquipGlow.TryGetValue("绿岩操作台", out int GG);
            Main.tileGlowMask[Type] = (short)GG;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
            TileObjectData.newTile.Width = 4;
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.DrawYOffset = 2;
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16 };
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceLeft;
            TileObjectData.newTile.Origin = new Point16(3, 2);
            TileObjectData.newTile.StyleHorizontal = true;

            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
            TileObjectData.newAlternate.Origin = new Point16(0, 2);
            TileObjectData.addAlternate(1);
            RegisterItemDrop(-1, 0);

            TileObjectData.addTile(Type);

            AddMapEntry(new Color(100, 255, 100), CreateMapEntryName());
        }
        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            frameCounter++;
            if (frameCounter % 10 == 0)
            {
                frame++;
            }
        }
        public override IEnumerable<Item> GetItemDrops(int i, int j)
        {
            yield return null;

        }
        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (!NPCDowned.绿岩之视 && (Main.tile[i, j].WallType == ModContent.WallType<绿岩砖墙Tile>() || Main.tile[i, j].WallType == ModContent.WallType<绿岩格网墙Tile>()))
            {
                fail = true;
            }
            if (!fail)
            {
                if (Main.rand.NextBool(2))
                {
                    Item.NewItem(new EntitySource_TileBreak(i, j), new Vector2(i * 16 + 8, j * 16 + 8), ModContent.ItemType<绿岩砖>(), Main.rand.Next(4, 8));
                }
                else
                {
                    Item.NewItem(new EntitySource_TileBreak(i, j), new Vector2(i * 16 + 8, j * 16 + 8), ModContent.ItemType<绿岩格网块>(), Main.rand.Next(4, 8));

                }
            }
        }
        public override bool CanReplace(int i, int j, int tileTypeBeingPlaced)
        {
            return !(!Main.hardMode && !NPCDowned.绿岩之视 && (Main.tile[i, j].WallType == ModContent.WallType<绿岩砖墙Tile>() || Main.tile[i, j].WallType == ModContent.WallType<绿岩格网墙Tile>()));
        }
        public override bool CanPlace(int i, int j)
        {
            return true;
        }
        public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset)
        {
            Tile t = Main.tile[i, j];
            if (t.TileFrameY < 162)
            {
                int uniqueAnimationFrame = Main.tileFrame[Type];
                uniqueAnimationFrame = uniqueAnimationFrame % 3;

                frameYOffset = uniqueAnimationFrame * 54;
            }
            else
            {
                frameYOffset = 0;
            }
        }
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = 3;
        }
        public override void PlaceInWorld(int i, int j, Item item)
        {
            Tile t = Main.tile[i, j];
        }
        public override bool CreateDust(int i, int j, ref int type)
        {
            NewDust(new Vector2(i, j) * 16, 16, 16, ModContent.DustType<绿岩电光粒子>(), Main.rand.NextFloat(-4, 4), Main.rand.NextFloat(-4, 4), Scale: Main.rand.NextFloat(0.75F, 1.25F));
            return base.CreateDust(i, j, ref type);
        }
        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
        {
            return settings.player.IsWithinSnappngRangeToTile(i, j, PlayerSittingHelper.ChairSittingMaxDistance*2);
        }
        public override bool RightClick(int i, int j)
        {
            Tile tile = Main.tile[i, j];
            int left = i - (int)tile.TileFrameX % (4 * 18) / 18;
            int top = j - (int)tile.TileFrameY % (3 * 18) / 18;
            
            if (Main.LocalPlayer.IsWithinSnappngRangeToTile(i, j, PlayerSittingHelper.ChairSittingMaxDistance*2))
            {
                Wiring.HitSwitch(left, top);
                NetMessage.SendData(59, -1, -1, null, left, top);
            }
            return true;
        }

        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
            player.cursorItemIconID = ModContent.ItemType<绿岩操作台>();
        }
    }
}
