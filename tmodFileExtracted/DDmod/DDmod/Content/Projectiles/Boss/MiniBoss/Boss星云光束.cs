using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Dusts;
using DDmod.Content.Projectiles.Melee.Spear.Proj;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Boss.MiniBoss
{
    public class Boss星云光束 : 闪电
    {
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 10000;
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 0;
            Projectile.timeLeft = 280;
            Projectile.penetrate = -1;
            Projectile.alpha += 255;
            Projectile.scale = 0.75f;
        }
        public override void AI()
        {
            if (V2 == null)
            {
                V2 = new List<Vector2>();
            }
            if (Projectile.ai[2] > 0)
            {
                Projectile.ai[2]--;
                Projectile.timeLeft++;
                return;
            }
            if (Projectile.localAI[2] == 0)
            {

                Projectile.localAI[2] = Projectile.timeLeft;
            }
            if(Projectile.ai[0]!=0)
            {
                Projectile.scale = Projectile.ai[0];
                Projectile.ai[0] = 0;
            }
            Projectile.ai[1]++;
            if (Projectile.ai[1]== 60)
            {
                SoundStyle sound = SoundID.Item158;
                sound.Volume = 1;
                sound.Pitch = -1;
                PlaySound(sound, Projectile.position);
                Projectile.ProjScaleChange(1F*Projectile.scale);
            }
            if (Projectile.ai[1] > 60)
            {
                Projectile.extraUpdates = 20;
                if (Projectile.DProj().vector[0] == Vector2.Zero)
                {
                    Projectile.DProj().vector[0] = Projectile.velocity.PerfectNormalize() * 10;
                    Projectile.velocity = Projectile.DProj().vector[0];
                }
                if (Projectile.timeLeft < 2)
                {
                    if (Vector == null)
                    {
                        Vector = new Vector2[Projectile.oldPos.Length];
                        for (int i = 0; i < Projectile.oldPos.Length; i++)
                        {
                            Vector[i] = Projectile.oldPos[i];
                        }
                    }
                    Projectile.timeLeft = 10000;
                }
                else
                if (Projectile.timeLeft > 1000)
                {
                    Projectile.extraUpdates = 5;
                    Projectile.damage = 0;
                    Projectile.timeLeft = 10000;
                    Projectile.velocity = Vector2.Zero;
                    Projectile.scale *= 0.98F;
                    Projectile.scale -= 0.01F;
                    if (Projectile.scale <= 0)
                    {
                        Projectile.Kill();
                    }
                }
                else
                {

                    if (Main.rand.NextBool(4))
                    {
                        Dust dust3 = Main.dust[NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<速度粒子>(), Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 255, new Color(255, 31, 174, 45))];
                        dust3.noGravity = true;
                        dust3.scale = Projectile.scale * 5;
                        dust3.velocity = Projectile.velocity/4;
                        dust3.customData = 2F * Projectile.scale;
                        dust3.rotation = dust3.velocity.ToRotation();
                    }
                    V2.Add(Projectile.position);
                    Projectile.DProj().Times[0]++;
                    if (Projectile.DProj().Times[0] > 20 && Projectile.DProj().track > 3)
                    {
                        Projectile.DProj().Times[0] = 0;

                        Projectile.netUpdate = true;
                    }
                }
            }
        }
        public override bool ShouldUpdatePosition()
        {
            return Projectile.ai[1] >= 60;
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.ai[2] > 0)
            {
                return false;
            }
            if (Projectile.ai[1] < 60)
            {
                Main.spriteBatch.Draw(DDTextures.Wire.Value, Projectile.Center - Main.screenPosition, null, new Color(255, 31, 174, 45)*0.5f, Projectile.velocity.ToRotation(), new Vector2(0, 1), new Vector2(5, 10), 0, 0f);

                return false;
            }
            SpriteEffects spriteEffects = 0;
            Texture2D texture = DDTextures.MiniVoidStar.Value;
            Color color = new Color(255, 31, 174, 45);
            Vector2 vector = Projectile.Size / 2;
            if (V2 == null)
            {
                for (int i = 0; i < Projectile.oldPos.Length; i++)
                {
                    if (Projectile.oldPos[i] != Projectile.position)
                    {
                        Vector2 vector2 = Projectile.oldPos[i] + vector - Main.screenPosition;
                        Main.spriteBatch.Draw(texture, vector2, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale / 6 + 0.15f + (float)i / 375 / 4, spriteEffects, 0f);
                        Main.spriteBatch.Draw(texture, vector2, null, color.Opposite(), Projectile.rotation, texture.Size() / 2, Projectile.scale / 10 + 0.025F + (float)i / 375 / 4 * 0.6f, spriteEffects, 0f);
                    }
                }
            }
            else
            {
                float ro = 0;
                for (int i = 0; i < V2.Count; i += (int)(Projectile.scale+1))
                {
                    float sc = Projectile.scale * (1F - i / Projectile.localAI[2]);
                    if (i > 0)
                    {
                        float r2 = (V2[i] - V2[i - 1]).ToRotation();
                        if (ro != r2)
                        {
                            ro = r2;
                        }
                    }
                    if (V2[i] != Projectile.position)
                    {
                        Vector2 vector2 = V2[i] + vector - Main.screenPosition;
                        Main.spriteBatch.Draw(texture, vector2, null, color, ro, texture.Size() / 2, Projectile.scale * new Vector2(3,1), spriteEffects, 0f);
                        Main.spriteBatch.Draw(texture, vector2, null, color.Opposite(), ro, texture.Size() / 2, Projectile.scale * new Vector2(3,1 )/2, spriteEffects, 0f);
                        if (i == 0)
                        {
                            vector2 = V2[i] + vector - Main.screenPosition;
                            Main.spriteBatch.Draw(texture, vector2, null, color, ro, texture.Size() / 2, Projectile.scale, spriteEffects, 0f);
                            Main.spriteBatch.Draw(texture, vector2, null, color.Opposite(), ro, texture.Size() / 2, Projectile.scale / 2, spriteEffects, 0f);
                        }
                        //Main.spriteBatch.Draw(DDTextures.WhitePng.Value, V2[i] - Main.screenPosition, null, Color.White, 0, Vector2.Zero, Projectile.Size / 2, spriteEffects, 0f);
                    }
                }
            }
            return false;
        }
    }
}