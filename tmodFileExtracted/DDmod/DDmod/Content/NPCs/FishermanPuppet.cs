namespace DDmod.Content.NPCs
{
    public class FishermanPuppet : ModNPC
    {
        public override void SetStaticDefaults()
        {
           //DisplayName.SetDefault("Fisherman Puppet");
           //DisplayName.AddTranslation(7, "渔夫傀儡");
            Main.npcFrameCount[NPC.type] = 11;
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = 0;
            NPC.lifeMax = 1145141919;
            NPC.damage = 0;
            NPC.defense = 10;
            NPC.knockBackResist = 0;
            NPC.width = 40;
            NPC.height = 40;
            NPC.alpha = 0;
            NPC.npcSlots = 0f;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.Item1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.netAlways = true;
            NPC.Dnpc().LifeUP = false;
            NPC.scale = 1.3f;
        }
        public override bool? CanFallThroughPlatforms()
        {
            return false;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,

                new FlavorTextBestiaryInfoElement(DDSystem.English?"Hit him":"打他！")
            });
        }
        public override void FindFrame(int frameHeight)
        {
            if (NPC.frame.Y != 0)
            {
                NPC.frameCounter++;
            }
            if (NPC.frameCounter >= 4)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y += 54;
            }
            if (NPC.frame.Y >= 594)
            {
                NPC.frame.Y = 0;
            }
        }
        public override void AI()
        {
            NPC.life = NPC.lifeMax;
            NPC.localAI[0]++;
            if(NPC.localAI[0]>1800)
            {
                Damage[2] = Damage[1];
                Damage[1] = 0;
                NPC.localAI[0] = 0;
            }
           // NPC.immune[0] = 100;
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
        }
        long[] Damage = new long[3];
        public override void HitEffect(HitInfo hit)
        {
            NPC.frame.Y = 54 * 5;
            NPC.life += (int)hit.Damage;
            Damage[0] += (long)hit.Damage;
            Damage[1] += (long)hit.Damage;
            //Main.NewText(NPC.whoAmI);
        }
        public override bool CheckDead()
        {
            NPC.life = 1145141919;
            return false;
        }
        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            return null;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Main.spriteBatch.Draw(DDTextures.WhitePng.Value, NPC.position - Main.screenPosition, null, Color.White * 0.5F, 0, Vector2.Zero, NPC.Size / 2, 0, 0f);
            DynamicSpriteFontExtensionMethods.DrawString(
                spriteBatch,
                FontAssets.MouseText.Value,
                "无敌帧: " + NPC.immune[0],
                NPC.Center - Main.screenPosition - new Vector2(0, 48),
                Color.Crimson, 0f,
                ChatManager.GetStringSize(FontAssets.MouseText.Value, "无敌帧: " + NPC.immune[0], Vector2.One) / 2,
                0.75F, SpriteEffects.None, 0f);

            return;
            DynamicSpriteFontExtensionMethods.DrawString(
                spriteBatch,
                FontAssets.MouseText.Value,
                "总伤害: " + Damage[0] + "\n每30秒定点伤害:" + Damage[2],
                NPC.Center - Main.screenPosition - new Vector2(0, 48),
                Color.Crimson, 0f,
                ChatManager.GetStringSize(FontAssets.MouseText.Value, "总伤害: "+Damage[0]+"\n每30秒定点伤害:" + Damage[2], Vector2.One) / 2,
                0.75F, SpriteEffects.None, 0f);

            DynamicSpriteFontExtensionMethods.DrawString(
                spriteBatch,
                FontAssets.MouseText.Value,
                ""+(int)(NPC.localAI[0]/60),
                NPC.Center - Main.screenPosition + new Vector2(0, 38),
                Color.Crimson, 0f,
                ChatManager.GetStringSize(FontAssets.MouseText.Value, "" + (int)(NPC.localAI[0] / 60), Vector2.One) / 2,
                0.75F, SpriteEffects.None, 0f);
        }
    }
}