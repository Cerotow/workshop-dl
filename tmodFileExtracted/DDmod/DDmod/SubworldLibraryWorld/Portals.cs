using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SubworldLibrary;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.WorldBuilding;
using Terraria.IO;
using StructureHelper;
using Terraria.ModLoader.IO;
using Terraria.Utilities;
using Terraria.ModLoader.Exceptions;
using Terraria.ModLoader.Default;
using DDmod.Worlds;
using DDmod.UI.HunterQuests;

namespace DDmod.SubworldLibraryWorld
{
    public class Portals : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 66;
            Projectile.height = 152;
            Projectile.scale = 1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 60;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
        }
        public override void AI()
        {
            Projectile.velocity = Vector2.Zero;
            Projectile.timeLeft = 114514;
            if (Main.LocalPlayer.getRect().Intersects(Projectile.getRect())&&Main.LocalPlayer.controlUp)
            {
                SubworldSystem.Exit();
            }
            DDHelper.BackAndForth(0.7F, 1F, 0.01F, ref Projectile.scale,ref Projectile.DProj().Bool[0]);
            if (Main.netMode != 2)
            {
                Texture2D texture = DDTextures.VoidStar.Value;
                int Type = 66;
                int dust = NewDust(Projectile.Center - new Vector2(Projectile.width / 2, 0), Projectile.width, 1, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].scale = Main.rand.NextFloat(0.8F, 1.1F);
                Main.dust[dust].velocity = new Vector2(0, -Main.rand.NextFloat(1, 10));
                Main.dust[dust].noLightEmittence = false;
                Main.dust[dust].color = new Color(144, 255, 200, 255);

            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects spriteEffects = (SpriteEffects)1;
            if (Projectile.direction == 1)
            {
                spriteEffects = 0;
            }
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 vector = Projectile.Size / 2;
            Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, new Color(144,255,200,0), Projectile.rotation, new Vector2(texture.Width, texture.Height+8) / 2, 1, spriteEffects, 0f);
            Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, new Color(144,255,200,0)*0.5F, Projectile.rotation, new Vector2(texture.Width, texture.Height+8) / 2, new Vector2(Projectile.scale,1), spriteEffects, 0f);

            Vector2 origin = ChatManager.GetStringSize(FontAssets.DeathText.Value, "W", Vector2.One, 0) / 2;
            for (int y = -1; y <=1; y++)
            {
                for (int x = -1; x <= 1; x++)
                {
                    DynamicSpriteFontExtensionMethods.DrawString(
                        Main.spriteBatch,
                        FontAssets.DeathText.Value,
                        "W",
                        Projectile.Center-new Vector2(0,texture.Height/2) + new Vector2(x, y) - Main.screenPosition,
                        new Color(0,0,0,255), 0f,
                        origin,
                        0.4F, SpriteEffects.None, 0f);
                }
            }
            DynamicSpriteFontExtensionMethods.DrawString(
                Main.spriteBatch,
                FontAssets.DeathText.Value,
                "W",
                Projectile.Center - new Vector2(0, texture.Height / 2) - Main.screenPosition,
                Color.White, 0f,
                origin,
                0.4F, SpriteEffects.None, 0f);
            return false;
        }
    }
}