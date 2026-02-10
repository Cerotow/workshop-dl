using DDmod.Content.Buffs.DeBuffs;
using DDmod.NoContent.Config;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Boss
{
    public class Boss闪电 : ModProjectile
    {
        public override string Texture => "DDmod/Image/Nullpng";
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 10000;
            int Length = 375;
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.aiStyle = -1;
            Projectile.tileCollide = true;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.timeLeft = Length;
            Projectile.extraUpdates = Length / 20;
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            if(V2==null)
            {
                V2 = [];
            }
            V2.Clear();
            int count = reader.ReadInt32();
            for (int A = 0; A < count; A++)
            {
                V2.Add(reader.ReadVector2());
            }
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(V2.Count);
            for (int A = 0; A < V2.Count; A++)
            {
                writer.WriteVector2(V2[A]);

            }
        }
        Vector2[] Vector;
        List<Vector2> V2 = new List<Vector2>();
        public override void AI()
        {
            if (V2 == null||V2.Count==0)
            {
                V2 = [Projectile.position];
            }
            if (Projectile.ai[2] != 0)
            {
                Projectile.timeLeft = (int)Projectile.ai[2];
                Projectile.localAI[2] = Projectile.ai[2];
                Projectile.ai[2] = 0;
            }
            if (Projectile.DProj().vector[0] == Vector2.Zero)
            {
                Projectile.DProj().vector[0] = Projectile.velocity.PerfectNormalize() * 3;
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
                V2.Add(Projectile.position);
                Projectile.timeLeft = 10000;
            }
            else
            if (Projectile.timeLeft > 1000)
            {
                Projectile.extraUpdates = 15;
                Projectile.damage = 0;
                Projectile.timeLeft = 10000;
                Projectile.velocity = Vector2.Zero;
                Projectile.scale -= 0.004F;
                if (Projectile.scale <= 0)
                {
                    Projectile.Kill();
                }
            }
            else
            {
                if(Projectile.ai[0]>0)
                Projectile.scale = Projectile.ai[0];
                Projectile.DProj().Times[0]++;
                if (Projectile.DProj().Times[0] > 20 && Main.rand.NextBool(10) && Projectile.DProj().track > 3)
                {
                    V2.Add(Projectile.position);
                    Projectile.DProj().Times[0] = 0;
                    Vector2 vector1 = Utils.RotatedBy(Projectile.DProj().vector[0].PerfectNormalize() * Projectile.velocity.Length(), Main.rand.NextFloat(-0.6F, 0.6F), default);
                    Projectile.velocity = vector1;
                    Projectile.netUpdate = true;
                }
            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<Charged2>(), 30);
            for (int i = 0; i < 40; i++)
            {
                Dust dust = Main.dust[NewDust(Projectile.Center, 1, 1, 226)];
                dust.velocity = new Vector2(0, -1).RotatedBy(Main.rand.NextFloat(-1, 1)) * Main.rand.NextFloat(1, 4);
                dust.noGravity = false;
                dust.alpha = 100;
                dust.scale = 1.3f;
            }
            Main.LocalPlayer.Dplayer().PlayerShake(3, 10);
            SoundStyle sound = new SoundStyle("DDmod/NoContent/Sounds/Items/闪电");
            sound.Pitch = -0.1f;
            sound.MaxInstances = 20;
            sound.Volume = .1f;
            PlaySound(sound, Projectile.position);
            Projectile.netUpdate = true;
            target.AddBuff(144, 180);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Main.LocalPlayer.Dplayer().PlayerShake(3, 10);
            SoundStyle sound = new SoundStyle("DDmod/NoContent/Sounds/Items/闪电");
            sound.Pitch = -0.1f;
            sound.MaxInstances = 20;
            sound.Volume = .1f;
            PlaySound(sound, Projectile.position);
            Projectile.timeLeft = 2;
            Projectile.tileCollide = false;
            Projectile.netUpdate = true;
            for (int i = 0; i < 40; i++)
            {
                Dust dust = Main.dust[NewDust(Projectile.Center, 1, 1, 226)];
                dust.velocity = new Vector2(0, -1).RotatedBy(Main.rand.NextFloat(-1, 1)) * Main.rand.NextFloat(1, 4);
                dust.noGravity = false;
                dust.alpha = 100;
                dust.scale = 1.3f;
            }
            return false;
        }
        public override void OnKill(int timeLeft)
        {
        }
        internal Color ColorFunction(float completionRatio)
        {
            return new Color(0, 186, 242);
        }
        internal float WidthFunction(float completionRatio)
        {
            return 30;
        }
        internal static Trailing TrailDrawer;
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects spriteEffects = 0;
            Texture2D texture = DDTextures.MiniVoidStar.Value;
            Color color = new Color(0, 186, 242, 0);
            Vector2 vector = Projectile.Size / 2;
            /*
            if (TrailDrawer == null)
            {
                TrailDrawer = new Trailing(new Trailing.VertexWidthFunction(WidthFunction), new Trailing.VertexColorFunction(ColorFunction), null, GameShaders.Misc["贴图拖尾"]);
            }
            GameShaders.Misc["贴图拖尾"].SetShaderTexture(DDTextures.VoidStar);
            GameShaders.Misc["贴图拖尾"].Shader.Parameters["uSpeed"].SetValue(Projectile.velocity.Length() / 20);

            for (int i = 0; i < V2.Count-1; i++)
            {
                TrailDrawer.Draw([V2[i],V2[i+1]], Projectile.Size * 0.5f - Main.screenPosition, 204, null);
            }*/
            if (V2 == null)
            {
                for (int i = 0; i < Projectile.oldPos.Length; i++)
                {
                    if (Projectile.oldPos[i] != Projectile.position)
                    {
                        Vector2 vector2 = Projectile.oldPos[i] + vector - Main.screenPosition;
                        Main.spriteBatch.Draw(texture, vector2, null, new Color(0, 186, 242, 0), Projectile.rotation, texture.Size() / 2, Projectile.scale / 6 + 0.15f + (float)i / 375 / 4, spriteEffects, 0f);
                        Main.spriteBatch.Draw(texture, vector2, null, new Color(255, 70, 15, 0), Projectile.rotation, texture.Size() / 2, Projectile.scale / 10 + 0.025F + (float)i / 375 / 4 * 0.6f, spriteEffects, 0f);
                        
                    }
                }
            }
            else
            {
                float ro = 0;
                int B = 0;
                for (int i = 0; i < V2.Count; i++)
                {

                    Vector2 Sp = Vector2.Zero;
                    float Count = 0;
                    if (i > 0)
                    {
                        Sp = (V2[i] - V2[i - 1]).PerfectNormalize() * 3;
                        Count = (V2[i] - V2[i - 1]).Length() / 3+1;
                        float r2 = (V2[i] - V2[i - 1]).ToRotation();
                        if (ro != r2)
                        {
                            ro = r2;
                        }
                    }
                    if (V2[i] != Projectile.position)
                    {
                        Vector2 vector2;
                        float sc = Projectile.scale * (1F - B / Projectile.localAI[2]);
                        for (int A = (int)Count-1; A >=0 ; A--)
                        {
                            
                             vector2 = V2[i] - (Sp * A) + vector - Main.screenPosition;
                            Main.spriteBatch.Draw(texture, vector2, null, new Color(0, 186, 242, 0), ro, texture.Size() / 2, new Vector2(sc >= 0.2F ? sc : 0.2F, Projectile.scale * (1F - B / Projectile.localAI[2])) / 2, spriteEffects, 0f);
                            Main.spriteBatch.Draw(texture, vector2, null, new Color(255, 70, 15, 0), ro, texture.Size() / 2, new Vector2(sc >= 0.2F ? sc : 0.2F, Projectile.scale * (1F - B / Projectile.localAI[2]) / 2) / 2, spriteEffects, 0f);

                            B++;
                        }
                    }
                }
                if (Projectile.ai[1] == 1)
                {
                    Vector2 vector2 = V2[0] + vector - Main.screenPosition;
                    Main.spriteBatch.Draw(texture, vector2, null, new Color(0, 186, 242, 0), ro, texture.Size() / 2, Projectile.scale*1.2F , spriteEffects, 0f);
                    Main.spriteBatch.Draw(texture, vector2, null, new Color(255, 70, 15, 0), ro, texture.Size() / 2, Projectile.scale*1.2F, spriteEffects, 0f);
                }
            }
            return false;
        }
    }
}