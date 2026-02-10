namespace DDmod.Content.Projectiles.Melee
{
	public class Sheet222 : ModProjectile
	{

        public override void SetDefaults()
        {
            Projectile.width = 46;
            Projectile.height = 46;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            //projectile.light = 0.50f;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 10;
            Projectile.coldDamage = true;
            Main.projFrames[Projectile.type] = 7;
        }
        float A;
        float B;
        bool V;
        public override bool PreAI()
        {
            Projectile.frameCounter++;
            if (Projectile.frame == 1|| Projectile.frame == 4)
            {
                if (Projectile.frameCounter > 1)
                {
                    Projectile.frame++;
                    Projectile.frameCounter = 0;
                }
            }
            if (Projectile.frameCounter > 3)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= 7)
            {
                Projectile.frame = 0;
            }
            if(Projectile.frame == 0||Projectile.frame == 5)
            {
                A = -1f;
            }
            else if (Projectile.frame == 1 || Projectile.frame == 4)
            {
                A = 0;
            }
            else if (Projectile.frame == 2 || Projectile.frame == 3)
            {
                A = 1;
            }
            Player player = Main.player[Projectile.owner];
            Vector2 vector = player.RotatedRelativePoint(player.MountedCenter, true);
            bool channeling = player.channel && !player.noItems && !player.CCed && !player.dead;
            if (channeling)
            {
                if (Main.myPlayer == Projectile.owner)
                {
                    float scaleFactor6 = 1f;
                    if (player.inventory[player.selectedItem].shoot == Projectile.type)
                    {
                        scaleFactor6 = player.inventory[player.selectedItem].shootSpeed * Projectile.scale;
                    }
                    Vector2 vector13 = Main.MouseWorld - vector;
                    vector13.Normalize();
                    vector13 *= scaleFactor6;
                    if (vector13.X != Projectile.velocity.X || vector13.Y != Projectile.velocity.Y)
                    {
                        Projectile.netUpdate = true;
                    }
                    Projectile.velocity = vector13;
                }
            }
            else
            {
                Projectile.Kill();
            }
            Projectile.ai[0]++;
            {
                float num2 = MathHelper.WrapAngle(Projectile.rotation) + 3.1415927f;
                float oldRotationAdjusted = MathHelper.WrapAngle(Projectile.oldRot[1]) + 3.1415927f;
                float deltaAngle = Math.Abs(num2 - oldRotationAdjusted);

                AngularDamageFactor = MathHelper.Lerp(this.AngularDamageFactor, deltaAngle, 0.2f);
                int damageWithChargeAndStats = player.GetWeaponDamage(player.HeldItem);
                Projectile.damage = damageWithChargeAndStats;
                Vector2 projDirection = Utils.RotatedBy(Projectile.velocity, A, default(Vector2));

                Projectile.position = player.RotatedRelativePoint(player.MountedCenter, true) - Projectile.Size / 2f + Vector2.Normalize(Projectile.velocity) * 20;
                Projectile.rotation = Projectile.velocity.ToRotation();
                //projectile.spriteDirection = projectile.direction;
                Projectile.timeLeft = 5;
                player.ChangeDir(Projectile.direction);
                player.heldProj = Projectile.whoAmI;
                player.itemTime = 5;
                player.itemAnimation = 5;
                player.itemRotation = (float)Math.Atan2(projDirection.Y * Projectile.direction, projDirection.X * Projectile.direction);

                Lighting.AddLight(Projectile.Center, new Vector3(Main.DiscoColor.R, Main.DiscoColor.G, Main.DiscoColor.B) * 0.002f);
                return false;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects spriteEffects = 0;
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 vector = Projectile.Size / 2;
            Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / Main.projFrames[Projectile.type] * Projectile.frame, texture.Width, texture.Height / Main.projFrames[Projectile.type])), Color.White, Projectile.rotation,new Vector2(texture.Width,texture.Height/7)/2, Projectile.scale*4, spriteEffects, 0f);
            return false;
        }
        public float AngularDamageFactor
        {
            get
            {
                return Projectile.ai[1];
            }
            set
            {
                Projectile.ai[1] = value;
            }
        }
    }
}