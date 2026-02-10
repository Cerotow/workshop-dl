using System;
using DDmod.Content.Dusts;
using DDmod.Worlds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace DDmod.Content.Tiles.绿岩
{
	public class 绿岩电刺Tile : DDSimplifyBlocks
    {
        public override Vector3 LightColor => new Color(100,255,100).ToVector3()*1F;
        public override Color Color => new Color(100,255,100);
        //public override int Glow => ModContent.TileType<绿岩能量管道Tile_Glow>();
        public override int Sound => 0;
        public override int Dust => ModContent.DustType<绿岩电光粒子>();
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = 3;
        }
        public override void SetDefaults()
        {
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
            Main.tileSolid[Type] = false;
            MinPick = 100;
            MineResist = 3;
        }
        public override bool Slope(int i, int j)=>false;
        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            return true;
        }
        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if ( !NPCDowned.绿岩之视)
            {
                fail = true;
            }
        }
        public override bool CanReplace(int i, int j, int tileTypeBeingPlaced)
        {
            return !(!NPCDowned.绿岩之视);
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
            uniqueAnimationFrame = uniqueAnimationFrame % 4;

            frameYOffset = uniqueAnimationFrame * 54;
        }
        public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
        {
            //drawData.glowColor = Color.White;
        }

        public override bool CanExplode(int i, int j) => false;
        public override bool KillSound(int i, int j, bool fail)
        {
            return false;
        }
        public override void NearbyEffects(int i, int j, bool closer)
		{
            if(Main.LocalPlayer.getRect().Intersects(new Rectangle(i*16,j*16,16,16)))
            {
                Main.LocalPlayer.AddBuff(ModContent.BuffType<绿岩电流>(),30);

            }
        }
	}
}
