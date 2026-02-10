using DDmod.Content.Items;

namespace DDmod.Content.Projectiles.Melee.FlyingKnife
{
   
    public enum 飞刀AI : byte
    {
        /// <summary>空AI  </summary>
        AI0,
        /// <summary>直线飞行飞刀  </summary>
        AI1,
        /// <summary>飞行一段时间开始旋转</summary>
        AI2,
        /// <summary>无限飞行飞刀 </summary>
        AI3,
    }
    public abstract class 飞刀Proj : ModProjectile
    {
        public virtual void Defaults()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            AIStyle = 飞刀AI.AI1;
            Defaults();
        }
        public 飞刀AI AIStyle;
        /// <summary>
        /// 旋转
        /// </summary>
        public float Rotation;
        /// <summary>
        /// 翻转旋转
        /// </summary>
        public float Rotation2;
        /// <summary>
        /// 重力计时器
        /// </summary>
        public float GravityTimer;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if(AIStyle == 飞刀AI.AI1)
            {
                GravityTimer++;
                Projectile.rotation = Projectile.velocity.ToRotation() + Rotation;
                if (Projectile.velocity.Y < 20 && GravityTimer > 20)
                {
                    Projectile.velocity.Y += 0.2f;
                }
            }
            else if (AIStyle == 飞刀AI.AI2)
            {
                GravityTimer++;
                if (GravityTimer > 30)
                {
                    Projectile.rotation += Projectile.velocity.X * 0.03f;
                }
                else
                {
                    Projectile.rotation = Projectile.velocity.ToRotation() + Rotation;
                }
                if (Projectile.velocity.X < 0)
                {
                    Projectile.spriteDirection = -1;
                }
                if (Projectile.velocity.Y < 20 && GravityTimer > 30)
                {
                    Projectile.velocity.Y += 0.2f;
                }
            }
            else if (AIStyle == 飞刀AI.AI3)
            {
                Projectile.rotation = Projectile.velocity.ToRotation() + Rotation;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            float RO = Projectile.rotation;
            SpriteEffects sprite = 0;
            Vector2 Origia = new Vector2(texture.Width/2,Projectile.height/2);
            if (Projectile.velocity.X < 0)
            {
                sprite = SpriteEffects.FlipHorizontally;
            }

            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, lightColor, RO, Origia, Projectile.scale, sprite, 0f);
            //Main.spriteBatch.Draw(DDTextures.WhitePng.Value, Projectile.position - Main.screenPosition, null, Color.White*0.5F, 0, Vector2.Zero, Projectile.Size/2, sprite, 0f);

            return false;
        }
    }
}