namespace DDmod.Content.Buffs.DeBuffs
{
    public class Freeze : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            Lighting.AddLight(npc.Center, new Color(37, 137, 187).ToVector3());

            if (npc.Dnpc().BossPhysique && npc.buffTime[buffIndex]>1)
            {
                npc.buffTime[buffIndex] = 1;
            }
            else if (!npc.Dnpc().BossPhysique)
            {
                npc.Dnpc().NoMove = true;
            }
            if (npc.Dnpc().Properties.Fire)
            {
                npc.buffTime[buffIndex] -= 3;
            }
            if (npc.buffTime[buffIndex] == 1 && Main.netMode != 2&& !npc.boss&&!npc.Dnpc().BossPhysique)
            {
                npc.AddBuff(ModContent.BuffType<Frozen>(), 300);
                float S = (float)npc.width / DDTextures.冻结.Width() + 0.4F;
                PlaySound(SoundID.Item27, npc.Center);
                for (int a = 0; a < 5*S; a++)
                {
                    Dust dust = Main.dust[NewDust(npc.position + new Vector2(npc.width / 2, npc.height) - new Vector2(DDTextures.冻结.Width() * S / 2, DDTextures.冻结.Height() * S), (int)(DDTextures.冻结.Width()*S), (int)(DDTextures.冻结.Height() * S), 185)];
                    dust.velocity = new Vector2(0,-2*S).RotatedBy(Main.rand.NextFloat(-1,1));
                    dust.noGravity = false;
                    dust.alpha = 100;
                    dust.scale = S+0.8f;
                }
            }
        }
    }
}