using DDmod.Content.Dusts;
using DDmod.Content.Items.Series.绿岩;
using DDmod.Content.Items.Tiles.绿岩;
using DDmod.UI.PlaystationUI;
using DDmod.Worlds;
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
    public class 绿岩干扰器Tile : ModTile
    {
        public const int NextStyleHeight = 72;

        public override void SetStaticDefaults()
        {
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileID.Sets.CanBeSatOnForNPCs[Type] = true;
            TileID.Sets.CanBeSatOnForPlayers[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;
            TileID.Sets.FramesOnKillWall[Type] = true;
            if (DDSystem.Instance.DDEquipGlow.TryGetValue("绿岩干扰器", out int GG))
                Main.tileGlowMask[Type] = (short)GG;

            DustType = ModContent.DustType<绿岩粒子>();
            RegisterItemDrop(ModContent.ItemType<绿岩干扰器>(), 0);

            AddMapEntry(new Color(100, 255, 100), CreateMapEntryName());

            TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
            TileObjectData.newTile.Height = 4;
            TileObjectData.newTile.CoordinateHeights = [16, 16, 16, 16];
            TileObjectData.newTile.AnchorWall = true;
            TileObjectData.newTile.AnchorBottom = new AnchorData();
            TileObjectData.newTile.Origin = new Point16(0, 0);
            TileObjectData.addTile(Type);
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
        public Vector3 LightColor => new Vector3(0, 0.15F, 0);
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = LightColor.X;
            g = LightColor.Y;
            b = LightColor.Z;
        }
        public override bool CreateDust(int i, int j, ref int type)
        {
            NewDust(new Vector2(i, j) * 16, 16, 16, ModContent.DustType<绿岩电光粒子>(), Main.rand.NextFloat(-4, 4), Main.rand.NextFloat(-4, 4), Scale: Main.rand.NextFloat(0.75F, 1.25F));
            return base.CreateDust(i, j, ref type);
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
            uniqueAnimationFrame = uniqueAnimationFrame % 8;

            frameYOffset = uniqueAnimationFrame * 72;
        }
        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
        {
            return settings.player.IsWithinSnappngRangeToTile(i, j, PlayerSittingHelper.ChairSittingMaxDistance);
        }
        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            return true;
        }
        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
        }
        public override bool CanReplace(int i, int j, int tileTypeBeingPlaced)
        {
            return false;
        }


        public override void Load()
        {
            if (!Main.dedServ)
            {
            }
        }
    }
}