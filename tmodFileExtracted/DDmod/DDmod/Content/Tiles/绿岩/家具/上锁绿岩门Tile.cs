
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ObjectInteractions;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using DDmod.Content.Items.Tiles.晶凝;
using DDmod.Content.Dusts;
using DDmod.Content.Items.Tiles.绿岩;
using Humanizer;
using DDmod.Content.Items.Series.绿岩;
using DDmod.Worlds;

namespace DDmod.Content.Tiles.绿岩.家具
{
    public class 上锁绿岩门Tile : DDDoorOff
    {
        public override int Icon => ModContent.ItemType<绿岩钥匙>();
        public override int Dust => ModContent.DustType<绿岩粒子>();
        public override Color Color => new Color(11, 255, 11);
        public override int DoorID => -ModContent.TileType<绿岩门关Tile>();
        public override Vector3 LightColor => new Vector3(0, 0.15F, 0);
        public override bool CreateDust(int i, int j, ref int type)
        {
            NewDust(new Vector2(i, j) * 16, 16, 16, ModContent.DustType<绿岩电光粒子>(), Main.rand.NextFloat(-4, 4), Main.rand.NextFloat(-4, 4), Scale: Main.rand.NextFloat(0.75F, 1.25F));
            return base.CreateDust(i, j, ref type);
        }
        public override void SetDefaults()
        {
            DDGlobalTile.TopInvincible[Type] = true;
            TileID.Sets.PreventsTileReplaceIfOnTopOfIt[Type] = true;
            TileID.Sets.PreventsTileRemovalIfOnTopOfIt[Type] = true;
            TileID.Sets.PreventsTileHammeringIfOnTopOfIt[Type] = true;

            TileObjectData.newTile.AnchorTop = new AnchorData();
            TileObjectData.newTile.AnchorBottom = new AnchorData();
        }
        public override bool RightClick(int i, int j)
        {
            Tile tile = Main.tile[i, j];
            for (int A = 0; A < Main.LocalPlayer.inventory.Length; A++)
            {
                if (Main.LocalPlayer.inventory[A].type > 0 && Main.LocalPlayer.inventory[A].type == ModContent.ItemType<绿岩钥匙>())
                {
                    int left = i - (int)tile.TileFrameX % (1 * 16) / 16;
                    int top = j - (int)tile.TileFrameY % (3 * 16) / 16;
                    tile = Main.tile[left, top];
                    tile.TileType = (ushort)-DoorID;

                    tile = Main.tile[left, top + 1];
                    tile.TileType = (ushort)-DoorID;

                    tile = Main.tile[left, top + 2];
                    tile.TileType = (ushort)-DoorID;
                    PlaySound(SoundID.Unlock, new Vector2(i, j) * 16);
                    NPCDowned.绿岩刷怪 = true;
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        NetMessage.SendTileSquare(Main.LocalPlayer.whoAmI, left, top, 1, 3);
                        DDmod.SyncData(DDType.PlayersWorld, Main.LocalPlayer.whoAmI, -1, Main.LocalPlayer.whoAmI,(byte)0);
                    }
                    break;
                }
            }
            return true;
        }
        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            blockDamaged = false;
            return false;
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            fail = true;
            effectOnly = true;
        }
    }
}