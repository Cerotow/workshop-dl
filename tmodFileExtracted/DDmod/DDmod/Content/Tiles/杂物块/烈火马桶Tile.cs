using DDmod.Content.Dusts;
using DDmod.Content.Items.Tiles;
using DDmod.Content.Items.Tiles.晶凝;
using DDmod.Content.Items.Tiles.绿岩;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace DDmod.Content.Tiles.杂物块
{
    public class 烈火马桶Tile : DDToilet
    {
        public override int Icon => ModContent.ItemType<烈火马桶>();
        public override int Dust => 6;
        public override Color Color => new Color(255, 244, 118);
        public override Vector3 LightColor => new Color(255, 244, 118).ToVector3()*2;
        public override void SetDefaults()
        {
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
            TileObjectData.newTile.Width = 2;
            TileObjectData.newTile.Origin = new (0,2);
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16 };
        }
        public override void ModifySittingTargetInfo(int i, int j, ref TileRestingInfo info)
        {
            Tile tile = Framing.GetTileSafely(i, j);
            info.ExtraInfo.IsAToilet = true;
            info.TargetDirection = -1;
            info.VisualOffset.X = -4;

            if (tile.TileFrameX >= 36)
            {
                info.TargetDirection = 1;
            }

            info.AnchorTilePosition.X = i;
            if (tile.TileFrameX >= 18 && info.TargetDirection == -1)
            {
                info.AnchorTilePosition.X--;
            }
            if (tile.TileFrameX < 54 && info.TargetDirection == 1)
            {
                info.AnchorTilePosition.X++;
            }
            info.AnchorTilePosition.Y = j;
            if (tile.TileFrameY % 54 == 0)
            {
                info.AnchorTilePosition.Y += 2;
            }
            if (tile.TileFrameY % 54 == 18)
            {
                info.AnchorTilePosition.Y ++;
            }
            info.VisualOffset.Y = -4; 
            if (info.RestingEntity is Player player)
            {
                if (player.HasBuff(BuffID.Stinky))
                {
                    info.VisualOffset = Main.rand.NextVector2Circular(2, 2);
                }
                player.AddBuff(24, 60);
            }
            if (info.RestingEntity is NPC npc)
            {
                npc.Dnpc().SittingPoint = new Point(i, j);
                if (npc.velocity == Vector2.Zero)
                {
                    if(npc.gfxOffY<0)
                    npc.AddBuff(24, 60);
                    npc.gfxOffY = -10;
                }
            }
        }
        public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
        {
            drawData.glowColor=Color.White;
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
            int left = i - (t.TileFrameX % (2 * 18) / 18);
            int top = j - (t.TileFrameY % (3 * 18) / 18);

            int uniqueAnimationFrame = Main.tileFrame[Type] + left + top;
            uniqueAnimationFrame = uniqueAnimationFrame % 3;

            frameYOffset = uniqueAnimationFrame * 54;
        }
    }
}

