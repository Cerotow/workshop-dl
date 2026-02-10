using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.ID;
using DDmod.Worlds;
using Terraria.WorldBuilding;
using Terraria.ModLoader.IO;
using DDmod.Content.Items.Series.Heart;
using DDmod.Content.Items.Tiles.晶凝;
using DDmod.Content.Dusts;
using DDmod.Content.Tiles.农场;
using DDmod.Content.NPCs.IittleMonster;
using DDmod.Content.Tiles.流星;
using Microsoft.Xna.Framework.Graphics;
using FullSerializer.Internal;
using Terraria.DataStructures;

namespace DDmod.Content.Tiles.绿岩
{
    public class 绿岩格网块Tile_Glow : DDBlocksCopy
    {
    }

    public class 绿岩格网块Tile : DDBlocks
    {
        public override int Sound => 0;
        public override int Dust => ModContent.DustType<绿岩电光粒子>();
        public override Color Color => new Color(100, 155, 100);
        public override void SetDefaults()
        {
            Main.tileMerge[ModContent.TileType<绿岩砖Tile>()][Type] = true;
            Main.tileMerge[ModContent.TileType<绿岩Tile>()][Type] = true;
            DDGlobalTile.ForbidSpawn[Type] = true;
        }
        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (!NPCDowned.绿岩之视 && j > DDWorld.GreenRockLab.Y + 60)
            {
                fail = true;
            }
        }
        public override bool CanReplace(int i, int j, int tileTypeBeingPlaced)
        {
            return !(!NPCDowned.绿岩之视 && j > DDWorld.GreenRockLab.Y + 60);
        }
        public override bool CanPlace(int i, int j)
        {
            return !(!NPCDowned.绿岩之视 && j > DDWorld.GreenRockLab.Y + 60);
        }
        public override bool Slope(int i, int j)
        {
            return !( !NPCDowned.绿岩之视 && j > DDWorld.GreenRockLab.Y + 60);
        }
        public Vector2 vector;
        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            Speed = 50;
        }
        int Speed = 100;
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Speed = 50;
            Texture2D TileTexture = TextureAssets.Tile[ModContent.TileType<绿岩格网块Tile_Glow>()].Value;
            Tile tile = Main.tile[i, j];
            Vector2 zero = new Vector2((float)Main.offScreenRange, (float)Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            Color color = new Color(20, 20, 20, 0);
            Player player = Main.LocalPlayer;
            int Glow = (int)(player.Dplayer().PlayerTimes / 2) % Speed;
            for (int P = 0; P < 5; P++)
            {
                if (Glow == i % Speed)
                {
                    color = Color.White;
                }
                for (int a = 1; a < 9; a++)
                {
                    int T = (int)(Glow - a);
                    if (T < 0)
                    {
                        T += Speed;
                    }

                    if (T == i % Speed)
                    {
                        color = Color.White * (1F - a * 0.1F);
                    }
                    T = (int)(Glow + a);
                    if (T >= Speed)
                    {
                        T -= Speed;
                    }
                    if (T == i % Speed)
                    {
                        color = Color.White * (1F - a * 0.1F);
                    }
                }
            }
            color.A = 0;
            if (tile.TileColor > 0)
            {
                Texture2D texture2 = Main.instance.TilePaintSystem.TryGetTileAndRequestIfNotReady(ModContent.TileType<绿岩格网块Tile_Glow>(), 0, tile.TileColor);
                if (texture2 != null)
                {
                    TileTexture = texture2;
                }
            }
            Vector2 Po = new Vector2((float)(i * 16 - (int)Main.screenPosition.X), (float)(j * 16 - (int)Main.screenPosition.Y)) + zero;
            tile.DrawSlope(TileTexture,spriteBatch,Po,color);
        }
    }
}