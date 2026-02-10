using DDmod.Content.Dusts;
using DDmod.Modkey;

namespace DDmod.Content.Projectiles.Talisman
{
    public abstract class Talismans : ModProjectile
    {
        public virtual void Set()
        {

        }
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.timeLeft = 1200;
            Projectile.penetrate = -1;
            Projectile.Tproj().Talisman = true;
            Set();

        }
        public bool NewNPC;
        public bool NewGore;
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            NewNPC = reader.ReadBoolean();
            NewGore = reader.ReadBoolean();
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(NewNPC);
            writer.Write(NewGore);
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public virtual void PreUse()
        {

        }
        public virtual void PostUse()
        {

        }
        /// <summary>
        /// 法宝使用完以后
        /// </summary>
        /// <returns></returns>
        public virtual bool PreEnd()
        {
            Player player = Main.player[Projectile.owner];
            return player.TPlayer().TalismanTimes <= 0;
        }
        public virtual void End()
        {
        }
        public virtual bool Use()
        {
            if (Projectile.owner == Main.myPlayer)
            {
                Player player = Main.player[Projectile.owner];
                return player.TPlayer().TalismanCD >= player.TPlayer().MaxTalismanCD && ModkeySetup.TalismanKey.JustPressed && !player.HasBuff(23);
            }
            return false;
        }
        public virtual void ExtraUse()
        {
        }
        /// <summary>
        /// 跟随AI
        /// </summary>
        /// <returns></returns>
        public virtual bool MobileAI()
        {
            return true;
        }
        //玩家没有装备这个法宝
        public virtual bool PreKill()
        {
            if (Projectile.Player().TPlayer().type != Projectile.Tproj().TalismanType)
            {
                if (Main.myPlayer == Projectile.owner)
                {
                    Projectile.Kill();
                    Projectile.Player().TPlayer().TalismanCD = 0;
                    Projectile.Player().TPlayer().TalismanCD2 = 0;
                }
            }
            return true;
        }
        public float Float = 0;
        public bool FloatBool;
        int CritChance = 0;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (player.dead)
            {
                Projectile.DProj().Bool[0] = false;
            }
            Projectile.damage = (int)player.GetTotalDamage(TalismanDamage.Instance).ApplyTo(Projectile.originalDamage);
            if (CritChance == 0)
            {
                CritChance=Projectile.CritChance;
            }
            else
            {
                Projectile.CritChance = CritChance+(int)player.GetTotalCritChance(TalismanDamage.Instance);
            }
            Projectile.timeLeft = 2;
            player.TPlayer().UseTalisman = Projectile.DProj().Bool[0];
            //使用中
            if (Projectile.DProj().Bool[0])
            {
                PreUse();
                //使用时间
                if (PreEnd()&& Projectile.owner == Main.myPlayer)
                {
                    player.TPlayer().TalismanTimes = 0;
                    Projectile.DProj().Bool[0] = false;
                    End();
                }
                Projectile.netUpdate = true;
                PostUse();
            }
            //可以使用
            else
            {
                //使用法宝
                if (Use())
                {
                    Projectile.DProj().Bool[0] = true;
                    player.TPlayer().TalismanTimes = player.TPlayer().MaxTalismanTimes;
                    player.TPlayer().TalismanCD = 0;
                    ExtraUse();
                    if (player.whoAmI == Main.myPlayer && Main.netMode == 1)
                    {
                        DDmod.SyncData(DDType.Talisman, player.whoAmI, -1, player.whoAmI);
                    }
                    Projectile.netUpdate = true;
                }
                if (MobileAI())
                {
                    //跟随AI
                    Vector2 vector = Projectile.Player().MountedCenter;
                    vector.X -= 40 * Projectile.Player().direction;
                    vector.Y += Float - 20;
                    vector = vector - Projectile.Center;
                    DDHelper.BackAndForth(-10, 10, 0.2f, ref Float, ref FloatBool);
                    float A = vector.Length();
                    DDHelper.MaxandMinF(ref A, 10, 0);
                    if (!player.dead)
                    {
                        if (vector.Length() > 1200)
                        {
                            Projectile.velocity = (Projectile.velocity * 8 + vector * player.velocity.Length()/40) / 9;
                        }
                        if (vector.Length() > 50)
                        {
                            vector.DirectPerfectNormalize();
                            Projectile.velocity = (Projectile.velocity * 8 + vector * A) / 9;

                        }
                        else
                        {
                            vector.DirectPerfectNormalize();
                            Projectile.velocity = (Projectile.velocity * 60 + vector * A) / 61;

                        }
                    }
                    else
                    {
                        if (vector.Length() > 1200)
                        {
                            Projectile.Kill();
                        }
                        vector.DirectPerfectNormalize();
                        Projectile.velocity = (Projectile.velocity * 8 + -vector * A) / 9;
                    }
                }
                PreKill();
                Projectile.tileCollide = false;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(255, 255, 255, Projectile.alpha));
        }
        int Time;
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects spriteEffects = 0;
            if (Projectile.spriteDirection == 1)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, texture.Size() / 2, 1, spriteEffects, 0f);
            return false;
        }
    }
}