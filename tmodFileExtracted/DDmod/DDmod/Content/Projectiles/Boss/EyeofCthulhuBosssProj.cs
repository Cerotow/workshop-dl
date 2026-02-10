namespace DDmod.Content.Projectiles.Boss
{
    public class EyeofCthulhuBosssProj : ModProjectile
    {
        public float TelegraphDelay
        {
            get
            {
                return Projectile.ai[0];
            }
            set
            {
                Projectile.ai[0] = value;
            }
        }
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 3;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            Projectile.width = 100;
            Projectile.height = 110;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.alpha = 0;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 1;
            Projectile.scale = 1F;
            Projectile.timeLeft = 600;
            CooldownSlot = 1;
        }
        int A;
        public override void AI()
        {
            Projectile.timeLeft = 2;
            if (Main.npc[(int)Projectile.ai[0]].type == NPCID.EyeofCthulhu && Main.npc[(int)Projectile.ai[0]].active && Main.npc[(int)Projectile.ai[0]].ai[0] == 3)
            {
                Projectile.Center = Main.npc[(int)Projectile.ai[0]].Center;
                Projectile.width = (int)(100 * Projectile.scale);
                Projectile.height = (int)(110 * Projectile.scale);
                if (Main.npc[(int)Projectile.ai[0]].ai[1] == 3)
                {
                    Projectile.scale += 0.005F;
                }
                else if (Main.npc[(int)Projectile.ai[0]].ai[1] < 3 || Main.npc[(int)Projectile.ai[0]].ai[1] > 4)
                {
                    Projectile.scale -= 0.02F;
                }
                if (Projectile.scale <= 1)
                {
                    Projectile.active = false;
                }
            }
            else
            {
                Projectile.active = false;
            }
            Projectile.rotation = Main.npc[(int)Projectile.ai[0]].rotation;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = (Texture2D)TextureAssets.Npc[4];
            Vector2 vector = new Vector2(Projectile.width / 2, Projectile.height / 2) + (Projectile.rotation - 1.57f).ToRotationVector2().PerfectNormalize() * 24;

            for (int i = 0; i < Projectile.oldRot.Length; i++)
            {
                Vector2 vector2 = Projectile.oldPos[i] + vector - Main.screenPosition;
                Color color = Projectile.GetAlpha(new Color(200, 0, 0)) * 0.8f * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 4f);
                Main.spriteBatch.Draw(texture, vector2, new Rectangle?(Main.npc[(int)Projectile.ai[0]].frame), color, Projectile.rotation, new Vector2(texture.Width, texture.Height / 6) / 2, Projectile.scale * 1.1F, (SpriteEffects)(-Main.npc[(int)Projectile.ai[0]].spriteDirection), 0f);
            }
            return false;
        }
    }
}