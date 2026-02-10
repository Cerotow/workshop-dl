using DDmod.Content.Dusts;
using Terraria.Enums;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ObjectData;

namespace DDmod.Content.Tiles.绿岩
{
    public class 绿岩电弧炉Tile : ModTile
    {
        public const int NextStyleHeight = 38;

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.1f;
            g = 0.7f;
            b = 0.1f;
        }
        public override void SetStaticDefaults()
        {
            Main.tileLighted[Type] = true;
            Main.tileTable[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            Main.tileFrameImportant[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;
            TileID.Sets.IgnoredByNpcStepUp[Type] = true;

            DustType = ModContent.DustType<绿岩粒子>();
            DDSystem.Instance.DDEquipGlow.TryGetValue("绿岩电弧炉", out int GG);
            Main.tileGlowMask[Type] = (short)GG;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
            TileObjectData.newTile.Width = 8;
            TileObjectData.newTile.Height = 6;
            TileObjectData.newTile.DrawYOffset=2;
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16, 16 , 16 , 16};
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.Origin = new Point16(4, 5);

            TileObjectData.addTile(Type);

            AddMapEntry(new Color(100, 255, 100), CreateMapEntryName());
        }
        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            frameCounter++;
            if (frameCounter % 5 == 0)
            {
                frame++;
            }
        }
        public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset)
        {
            Tile t = Main.tile[i, j];

            int uniqueAnimationFrame = Main.tileFrame[Type];
            uniqueAnimationFrame = uniqueAnimationFrame % 22;

            frameXOffset = (uniqueAnimationFrame / 6) * 144;
            frameYOffset = (uniqueAnimationFrame % 6) * 108;
        }
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = 3;
        }
        public override void PlaceInWorld(int i, int j, Item item)
        {

            Tile t = Main.tile[i, j];
            //t.TileFrameX = Main.rand.Next(new short[] { 0, 36, 72 });
        }
        public override bool CreateDust(int i, int j, ref int type)
        {
            NewDust(new Vector2(i, j) * 16, 16, 16, ModContent.DustType<绿岩电光粒子>(), Main.rand.NextFloat(-4, 4), Main.rand.NextFloat(-4, 4), Scale: Main.rand.NextFloat(0.75F, 1.25F));
            return base.CreateDust(i, j, ref type);
        }

    }
}
