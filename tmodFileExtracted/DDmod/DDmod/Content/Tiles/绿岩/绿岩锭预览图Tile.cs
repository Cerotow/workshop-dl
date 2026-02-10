using DDmod.Content.Dusts;
using Terraria.Enums;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ObjectData;

namespace DDmod.Content.Tiles.绿岩
{
    public class 绿岩锭预览图Tile : ModTile
    {
        public const int NextStyleHeight = 38;

        public override void SetStaticDefaults()
        {
            Main.tileLavaDeath[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileLighted[(int)Type] = true;
            TileID.Sets.FramesOnKillWall[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;
            DDSystem.Instance.DDEquipGlow.TryGetValue("绿岩锭预览图", out int GG);
            Main.tileGlowMask[Type] = (short)GG;

            DustType = ModContent.DustType<绿岩粒子>();

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
            TileObjectData.newTile.Width = 8;
            TileObjectData.newTile.Height = 7;
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16, 16, 16, 16, 16 };
            TileObjectData.newTile.Origin = new Point16(3, 6);

            TileObjectData.addTile(Type);
            AddMapEntry(new Color(100, 255, 100), CreateMapEntryName());
        }
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = 3;
        }
        public override bool CreateDust(int i, int j, ref int type)
        {
            NewDust(new Vector2(i, j) * 16, 16, 16, ModContent.DustType<绿岩电光粒子>(), Main.rand.NextFloat(-4, 4), Main.rand.NextFloat(-4, 4), Scale: Main.rand.NextFloat(0.75F, 1.25F));
            return base.CreateDust(i, j, ref type);
        }
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0f;
            g = 0.15f;
            b = 0f;
        }
        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            frameCounter++;
            if (frameCounter % 10 == 0)
            {
                frame++;
            }
        }
        public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset)
        {
            int uniqueAnimationFrame = Main.tileFrame[Type];
            uniqueAnimationFrame = uniqueAnimationFrame % 16;

            frameYOffset = (uniqueAnimationFrame % 4) * 126;
            frameXOffset = (uniqueAnimationFrame / 4) * 144;
        }
    }
}
