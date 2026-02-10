namespace DDmod.Content.Buffs.PlayerBuffs
{
    public class 力量之魂 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = false;  
           //DisplayName.SetDefault("Soul of Power");
           //DisplayName.AddTranslation(7, "力量之魂");
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage(DamageClass.Generic) += (float)Main.LocalPlayer.Aplayer().精金力量 / 100;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, int buffIndex, ref BuffDrawParams drawParams)
        {
            spriteBatch.Draw(drawParams.Texture, drawParams.Position, null, drawParams.DrawColor, 0, Vector2.Zero, 1, 0, 0);
            Texture2D texture = ModContent.Request<Texture2D>("DDmod/Content/Buffs/PlayerBuffs/数字").Value;
            int A = Main.LocalPlayer.Aplayer().精金力量 / 10;
            if (A > 0)
            {
                spriteBatch.Draw(texture, drawParams.Position+new Vector2(20,10)-new Vector2(texture.Width+4,0), new Rectangle(0, texture.Height / 10* A, texture.Width, texture.Height / 10), new Color(255,255,255,0), 0, Vector2.Zero, 1, 0, 0);
                spriteBatch.Draw(texture, drawParams.Position+new Vector2(20, 10)-new Vector2(texture.Width+4,0), new Rectangle(0, texture.Height / 10* A, texture.Width, texture.Height / 10), new Color(255,255,255,0), 0, Vector2.Zero, 1, 0, 0);
                spriteBatch.Draw(texture, drawParams.Position+new Vector2(20, 10)-new Vector2(texture.Width+4,0), new Rectangle(0, texture.Height / 10* A, texture.Width, texture.Height / 10), new Color(255,255,255,0), 0, Vector2.Zero, 1, 0, 0);
                spriteBatch.Draw(texture, drawParams.Position+new Vector2(20, 10)-new Vector2(texture.Width+4,0), new Rectangle(0, texture.Height / 10* A, texture.Width, texture.Height / 10), new Color(255,255,255,0), 0, Vector2.Zero, 1, 0, 0);
            }
            spriteBatch.Draw(texture, drawParams.Position + new Vector2(20, 10), new Rectangle(0, texture.Height / 10* (Main.LocalPlayer.Aplayer().精金力量%10), texture.Width, texture.Height / 10), new Color(255, 255, 255, 0), 0, Vector2.Zero, 1, 0, 0);
            spriteBatch.Draw(texture, drawParams.Position + new Vector2(20, 10), new Rectangle(0, texture.Height / 10* (Main.LocalPlayer.Aplayer().精金力量%10), texture.Width, texture.Height / 10), new Color(255, 255, 255, 0), 0, Vector2.Zero, 1, 0, 0);
            spriteBatch.Draw(texture, drawParams.Position + new Vector2(20, 10), new Rectangle(0, texture.Height / 10* (Main.LocalPlayer.Aplayer().精金力量%10), texture.Width, texture.Height / 10), new Color(255, 255, 255, 0), 0, Vector2.Zero, 1, 0, 0);
            spriteBatch.Draw(texture, drawParams.Position + new Vector2(20, 10), new Rectangle(0, texture.Height / 10* (Main.LocalPlayer.Aplayer().精金力量%10), texture.Width, texture.Height / 10), new Color(255, 255, 255, 0), 0, Vector2.Zero, 1, 0, 0);
            return false;
        }
        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            tip = Language.GetTextValue("Mods.DDmod.Buffs.力量之魂.Description", Main.LocalPlayer.Aplayer().精金力量);
        }
    }
}
