using DDmod.Content.Achievements;
using DDmod.Content.Dusts;

namespace DDmod.Content.Projectiles.Talisman
{
    public class 宣花葫芦Proj : Talismans
    {
        public override void SetStaticDefaults()
        {
        }
        public override void PreUse()
        {
            Player player = Main.player[Projectile.owner];
            if ((int)Projectile.ai[2] < 0 || (int)Projectile.ai[2] > 200 || !Main.npc[(int)Projectile.ai[2]].active || !Main.npc[(int)Projectile.ai[2]].Dnpc().Battlepet)
            {
                player.TPlayer().TalismanTimes = 0;
                return;
            }
            Vector2 vector = Main.npc[(int)Projectile.ai[2]].Center - Projectile.Center;
            Projectile.rotation = vector.ToRotation() + MathHelper.PiOver2;
            Projectile.DProj().Times[0]++;
            if (Projectile.DProj().Times[0] > 30)
            {
                float WL = (Projectile.DProj().Times[0]) / 30;
                if (WL > 20)
                {
                    WL = 20;
                }
                Main.npc[(int)Projectile.ai[2]].position -= vector.PerfectNormalize() * WL;
                if (vector.Length() < 30)
                {
                    CombatText.NewText(new Rectangle((int)Projectile.Center.X, (int)Projectile.Center.Y, 1, 1), new Color(4, 203, 180), Language.GetTextValue("Mods.DDmod.BattlePetText.捕捉成功"), false, false);
                    if (Projectile.owner == Main.myPlayer)
                    {
                        for (int a = 0; a < player.Dplayer().Bpets.Length; a++)
                        {
                            if (player.Dplayer().Bpets[a].Type == 0)
                            {
                                player.Dplayer().Bpets[a] = new Players.BattlePets(Main.npc[(int)Projectile.ai[2]].Dnpc().BattlepetType);
                                break;
                            }
                        }
                        HitInfo hit = new HitInfo();
                        hit.Damage = 9999;
                        hit.InstantKill = true;
                        Main.npc[(int)Projectile.ai[2]].StrikeNPC(hit);
                        if (Main.netMode != NetmodeID.SinglePlayer)
                            NetMessage.SendStrikeNPC(Main.npc[(int)Projectile.ai[2]], hit);

                        ModContent.GetInstance<宣花葫芦>().Condition.Complete();

                    }
                }
                if (Main.rand.NextBool(1))
                {
                    if (Main.rand.NextBool(4))
                    {

                        Vector2 po = vector.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-1, 1)) * Main.rand.NextFloat(2, 18);
                        Dust dust = Main.dust[NewDust(Projectile.PreviousCenter() - new Vector2(4) - new Vector2(0, 12).RotatedBy(Projectile.rotation) + po * 60, 1, 1, ModContent.DustType<速度粒子>(), 0, 0, 0, new Color(4, 203, 180, 0))];
                        dust.noGravity = true;
                        dust.scale = 4F;
                        dust.velocity = -po * 1.5F * (1.5F + (WL / 10 / 2))*2;
                        dust.rotation = dust.velocity.ToRotation();
                        dust.customData = (1.5F + (WL / 10 / 2))*2;
                        GlobalDust.DustProjectileOwner[dust.dustIndex] = Projectile.whoAmI;
                    }
                    else
                    {
                        Vector2 po = vector.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-1, 1)) * Main.rand.NextFloat(2, 18);
                        Dust dust = Main.dust[NewDust(Projectile.PreviousCenter() - new Vector2(4) - new Vector2(0, 12).RotatedBy(Projectile.rotation) + po * 60, 1, 1, ModContent.DustType<光球粒子>(), 0, 0, 0, new Color(4, 203, 180, 0))];
                        dust.noGravity = true;
                        dust.scale = 1.2F;
                        dust.velocity = -po * 1.5F * (1.5F + (WL / 10 / 2));
                        dust.customData = dust.DustAI(0) + (1.5F + (WL / 10 / 2));
                        GlobalDust.DustProjectileOwner[dust.dustIndex] = Projectile.whoAmI;

                    }
                }
                Projectile.velocity = vector.PerfectNormalize() * 0.2f;
            }
            if (!Projectile.DProj().Bool[1])
            {
                NewDustChange4(60, Projectile.Center, Vector2.Zero, ModContent.DustType<光球粒子>(), 0.1F, 5,true,1.2F,1.8F,0,0, new Color(4, 203, 180, 0),3);
                Projectile.Center = player.Dplayer().MouseWorld - vector.PerfectNormalize().RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)) * 400;
                NewDustChange4(60, Projectile.Center, Vector2.Zero, ModContent.DustType<光球粒子>(), 0.1F, 5, true, 1.2F, 1.8F, 0, 0, new Color(4, 203, 180, 0), 3);
                Projectile.DProj().Bool[1] = true;
                Projectile.netUpdate = true;
            }
            Projectile.velocity *= 0.98f;
        }
        public override void End()
        {
            Projectile.DProj().Bool[1] = false;
            Projectile.DProj().Times[0] = 0;
            NewDustChange4(60, Projectile.Center, Vector2.Zero, ModContent.DustType<光球粒子>(), 0.1F, 5, true, 1.2F, 1.8F, 0, 0, new Color(4, 203, 180, 0), 3);
        }
        public override void ExtraUse()
        {
            Player player = Main.player[Projectile.owner];

            float num = 500;
            int result = -1;
            for (int i = 0; i < 200; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active&&npc.Dnpc().Battlepet)
                {
                    float num2 = (player.Dplayer().MouseWorld - npc.Center).Length();
                    if (!(num <= num2))
                    {
                        if (Collision.CanHitLine(player.Dplayer().MouseWorld, 1, 1, npc.position, npc.width, npc.height))
                        {
                            num = num2;
                            result = npc.whoAmI;
                        }
                    }
                }
            }
            if (result != -1)
            {
                Projectile.ai[2] = result;
                Projectile.DProj().Bool[1] = false;
                Projectile.DProj().Times[0] = 0;
                Projectile.velocity = Vector2.Zero;
            }
        }
        public override bool MobileAI()
        {
            Dust dust;

            Projectile.RotationSpeed(Projectile.velocity.X * 0.05F, 0.05F);
            if (Math.Abs(Projectile.velocity.X) < 0.2F)
            {
                Projectile.rotation = 0;
            }
            Player player = Main.player[Projectile.owner];
            Projectile.spriteDirection = -player.direction;
            if (player.TPlayer().TalismanCD >= player.TPlayer().MaxTalismanCD)
            {
                dust = Main.dust[NewDust(Projectile.Center - new Vector2(4) - new Vector2(0, 16).RotatedBy(Projectile.rotation), 6, 1, ModContent.DustType<光球粒子>(), 0, 0, 0, new Color(4, 203, 180, 0))];
                dust.noGravity = true;
                dust.scale = 0.4F;
                dust.velocity = new Vector2(0, -1).RotatedBy(Projectile.rotation) * Main.rand.NextFloat(2, 6);
            }
            return base.MobileAI();
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.DProj().Times[2] += 0.1F;
            Player player = Main.player[Projectile.owner];
            SpriteEffects spriteEffects = 0;
            if (Projectile.spriteDirection == 1)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }


            Vector2 vector = Projectile.Size / 2;

            Color color = new Color(4, 203, 180, 255);
            Texture2D VoidStar = DDTextures.VoidStar.Value;
            Texture2D texture = DDTextures.Circle[9].Value;
            Main.spriteBatch.Draw(VoidStar, Projectile.Center + new Vector2(0, 18).RotatedBy(Projectile.rotation) - Main.screenPosition, null, color, Projectile.rotation, VoidStar.Size() / 2, new Vector2(0.5F, 0.1F), 0, 0f);
            Main.spriteBatch.Draw(VoidStar, Projectile.Center + new Vector2(0, 18).RotatedBy(Projectile.rotation) - Main.screenPosition, null, color, Projectile.rotation, VoidStar.Size() / 2, new Vector2(0.5F, 0.1F), 0, 0f);
            DDHelper.Compression(texture, color, Projectile.rotation, Projectile.Opacity, new Vector2(1, 15), 1, Projectile.DProj().Times[2], BlendState.Additive);

            Main.spriteBatch.Draw(texture, Projectile.Center + new Vector2(0, 18).RotatedBy(Projectile.rotation) - Main.screenPosition, null, color, 0f, Utils.Size(texture) / 2, 0.1F, 0, 0);
            Main.spriteBatch.Draw(texture, Projectile.Center + new Vector2(0, 18).RotatedBy(Projectile.rotation) - Main.screenPosition, null, color, 0f, Utils.Size(texture) / 2, 0.1F, 0, 0);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            texture = TextureAssets.Projectile[Projectile.type].Value;
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, texture.Size() / 2, 1, spriteEffects, 0f);
            if (Projectile.DProj().Bool[0])
            {
                //Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, new Color(0, 255, 0, 0), Projectile.rotation, texture.Size() / 2, 1, spriteEffects, 0f);
                //Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, new Color(0, 255, 0, 0), Projectile.rotation, texture.Size() / 2, 1, spriteEffects, 0f);
            }
            else
            {
            }
            return false;
        }
    }
}