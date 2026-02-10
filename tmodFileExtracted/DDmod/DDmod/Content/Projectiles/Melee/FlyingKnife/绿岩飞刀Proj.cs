using DDmod.Content.Dusts;
using DDmod.Content.Projectiles.GeneralProj;
using Terraria;

namespace DDmod.Content.Projectiles.Melee.FlyingKnife
{
    public class 绿岩飞刀Proj : 飞刀Proj
    {

        public override void Defaults()
        {
            AIStyle = 飞刀AI.AI1;
            Rotation = MathHelper.PiOver2;
            Rotation2 = MathHelper.Pi;
            Projectile.width = 10;
            Projectile.height = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
        }
        public override void PostAI()
        {
        }
        int r = 0;
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            List<NPC> list = new List<NPC>();
            if (r < 1)
            {
                r++;
                NPC npc = NPCdirection.FindClosest(target.Center, 300, false, target);
                if (npc != null)
                {
                    list.Add(npc);
                    int T = NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<绿岩能量>(), hit.SourceDamage, 0, -1, npc.whoAmI,2);
                    Main.projectile[T].DamageType = DamageClass.Melee;
                    Main.projectile[T].DProj().NPCW = new List<byte>() { (byte)target.whoAmI };
                }
            }
        }
        public override void OnKill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];
            NewDustChange2(30, Projectile.Center - new Vector2(4), Vector2.Zero, ModContent.DustType<速度粒子>(), 2, 4, true, 1 , 3 , 100, new Color(119, 237, 130, 0));


        }
        public override void PostDraw(Color lightColor)
        {
            DDSystem.Instance.DDEquipGlow.TryGetValue("绿岩飞刀", out int GG);

            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            float RO = Projectile.rotation;
            SpriteEffects sprite = 0;
            Vector2 Origia = new Vector2(texture.Width/2,Projectile.height/2);
            if (Projectile.velocity.X < 0)
            {
                sprite = SpriteEffects.FlipHorizontally;
            }
            
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                float RO2 = Projectile.oldRot[i];

                Vector2 vector2 = Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition;
                Color color = new Color(100, 255, 100, 0) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                Main.spriteBatch.Draw(texture, vector2, null, color, RO2, Origia, Projectile.scale, sprite, 0f);
            }
            texture = TextureAssets.GlowMask[GG].Value;
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, RO, Origia, Projectile.scale, sprite, 0f);
        }
    }
}