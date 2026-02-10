
using DDmod.Content.Dusts;
using DDmod.Helper;
using DDmod.Worlds;
using System;
using Terraria;

namespace DDmod.Content.Buffs
{
    public class DDGlobalBuff : GlobalBuff
    {
        public override bool ReApply(int type, Player player, int time, int buffIndex)
        {
            return false;
        }
        public override bool ReApply(int type, NPC npc, int time, int buffIndex)
        {
            return false;
        }
        public override void Update(int type, Player player, ref int buffIndex)
        {
            if(type ==13)
            {
                if(player.HasBuff(ModContent.BuffType<战争药水Buff>())|| player.HasBuff(ModContent.BuffType<纷争药水Buff>()))
                {
                    if(player.buffTime[buffIndex]>2)
                    player.buffTime[buffIndex] = 2;
                }
            }
            //重力
            if (type == BuffID.Gravitation)
            {
                player.gravControl = false;
                player.Aplayer().Gravitation = 5;
            }
            //魔力病
            if (type == BuffID.ManaSickness)
            {
                player.GetAttackSpeed(DamageClass.Magic) -= (float)player.buffTime[buffIndex] / 600 / 2;
            }
            //毒液
            if (type == 70)
            {
                player.moveSpeed *= 0.8f;
                player.maxRunSpeed *= 0.8f;
            }
            //暗影焰
            if (type == 153)
            {
                BuffDust(player, 27, 1f);
                player.lifeRegen -= 30;
            }
        }
        public void BuffDust(Player npc, int type, float Scale)
        {
            if (Main.rand.NextBool(2))
            {
                for (int A = 0; A < npc.Size.Length() / 80; A++)
                {
                    Dust dust = Main.dust[NewDust(npc.position, npc.width, npc.height, type, 0, Main.rand.NextFloat(-3, -1))];
                    dust.noGravity = false;
                    dust.scale = Main.rand.NextFloat(0.5F, 1.25F) * Scale;
                }
            }
        }
        public void BuffDust(NPC npc,int type,float Scale)
        {
            if (Main.rand.NextBool(2))
            {
                for (int A = 0; A < npc.Size.Length() / 80; A++)
                {
                    Dust dust = Main.dust[NewDust(npc.position, npc.width, npc.height, type, 0, Main.rand.NextFloat(-3, -1))];
                    dust.noGravity = false;
                    dust.scale = Main.rand.NextFloat(0.5F, 1.25F) * Scale;
                }
            }
        }
        public override void Update(int type, NPC npc, ref int buffIndex)
        {
            //中毒
            if (type == 20)
            {
                if (npc.Dnpc().BossPhysique)
                {
                    npc.Dnpc().MoveSpeed *= 0.95f;
                }
                else
                {
                    npc.Dnpc().MoveSpeed *= 0.3f;
                }
            }
            //毒液
            if (type == 70)
            {
                if (npc.Dnpc().BossPhysique)
                {
                    npc.Dnpc().MoveSpeed *= 0.92f;
                }
                else
                {
                    npc.Dnpc().MoveSpeed *= 0.4f;
                }
                npc.Dnpc().Penetrate = 10;
            }
            //霜火
            if (type == 44)
            {
                npc.onFrostBurn = false;

                npc.lifeRegen -= 30;
                if (npc.IceCombustion())
                {
                    npc.lifeRegen += 15;
                }
                if (npc.NoIceCombustion())
                {
                    npc.lifeRegen -= 30;
                }
                BuffDust(npc, 135, 1);
            }
            //冻伤
            if (type == 324)
            {
                npc.onFrostBurn2 = false;

                BuffDust(npc, 135, 1.25f);
                npc.lifeRegen -= 50;
                if (npc.IceCombustion())
                {
                    npc.lifeRegen += 25;
                }
                if (npc.NoIceCombustion())
                {
                    npc.lifeRegen -= 50;
                }

            }
            //着火了
            if (type == 24)
            {
                npc.onFire = false;

                BuffDust(npc, 6, 1);
                npc.lifeRegen -= 8;
                if (npc.Combustion()) npc.lifeRegen -= 8;
            }
            //咒火
            if (type == 39)
            {
                npc.onFire2 = false;

                BuffDust(npc, 75, 1.25f);
                npc.lifeRegen -= 48;
                if (npc.Combustion()||npc.Dnpc().Properties.Light)
                {
                    npc.lifeRegen -= 48;
                    if(npc.Dnpc().Properties.Light)
                    {
                        npc.lifeRegen -= 48;
                    }
                }
            }
            //地狱火
            if (type == 323)
            {
                npc.onFire3 = false;
                BuffDust(npc, 6, 1.25f);
                npc.lifeRegen -= 30;
                if (npc.Combustion()) npc.lifeRegen -= 30;
            }
            //暗影焰
            if (type == 153)
            {
                npc.shadowFlame = false;

                BuffDust(npc, 27, 1f);
                npc.lifeRegen -= 30;
                if (npc.Combustion() || npc.Dnpc().Properties.Light)
                {
                    npc.lifeRegen -= 30;
                    if (npc.Dnpc().Properties.Light)
                    {
                        npc.lifeRegen -= 30;
                    }
                }
                if (DDWorld.诅咒之火)
                {
                    npc.lifeRegen -= 30;
                    if (npc.Combustion() || npc.Dnpc().Properties.Light)
                    {
                        npc.lifeRegen -= 30;
                        if (npc.Dnpc().Properties.Light)
                        {
                            npc.lifeRegen -= 30;
                        }
                    }

                    if (Main.rand.NextBool(2))
                    {
                        for (int A = 0; A < npc.Size.Length() / 80; A++)
                        {
                            Dust dust = Main.dust[NewDust(npc.position, npc.width, npc.height, ModContent.DustType<光球粒子>(), 0, 0, 0, new Color(81, 6, 233, 0))];
                            dust.noGravity = false;
                            dust.velocity.X = Main.rand.NextFloat(-1, 1);
                            dust.velocity.Y = -Main.rand.NextFloat(2, 10);
                            dust.scale = Main.rand.NextFloat(0.5F, 1.25F);
                            dust.customData = 1;
                        }
                    }
                }
            }
            if (type == 337)
            {
                npc.tentacleSpiked = false;
                int num4 = 0;
                for (int j = 0; j < 1000; j++)
                {
                    if (Main.projectile[j].active && Main.projectile[j].type == 971 && Main.projectile[j].ai[0] == 1f && Main.projectile[j].ai[1] == (float)npc.whoAmI)
                        num4++;
                }

                npc.lifeRegen -= num4 * 20;
            }
        }
        public override void ModifyBuffText(int type, ref string buffName, ref string tip, ref int rare)
        {
            if (type == BuffID.ManaSickness)
            {
                tip = Language.GetTextValue("Mods.DDmod.Buffs.ManaSickness", ((int)(((float)Main.LocalPlayer.buffTime[Main.LocalPlayer.FindBuffIndex(type)]) / 600 * 50)));
            }
        }
    }
}