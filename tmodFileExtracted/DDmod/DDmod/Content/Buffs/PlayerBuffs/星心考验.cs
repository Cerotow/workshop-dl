using Terraria;

namespace DDmod.Content.Buffs.PlayerBuffs
{
    public class 星心考验 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.Dplayer().ManaKao++;
            player.Dplayer().LifeKao++;

            player.statLifeMax2 -= player.Dplayer().LifeKao / 100;
            player.statManaMax2 -= player.Dplayer().ManaKao / 100;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
        }
        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            tip = Language.GetTextValue("Mods.DDmod.Buffs.星心",new string[] { ""+Main.LocalPlayer.Dplayer().LifeKao / 100, ""+Main.LocalPlayer.Dplayer().ManaKao / 100 });
        }
    }
}
