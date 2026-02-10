using DDmod.Content.Projectiles.Talisman;
using Terraria;
using Terraria.ID;

namespace DDmod.Content.NPCs
{
    public class EnchantedVoodooDollNPC : ModNPC
    {
        public override void SetStaticDefaults()
        {
           //DisplayName.SetDefault("Enchanted Voodoo doll");
           //DisplayName.AddTranslation(7, "着魔的巫毒娃娃");
            Main.npcFrameCount[NPC.type] = 1;
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 11111;
            NPC.damage = 0;
            NPC.defense = 0;
            NPC.knockBackResist = 0;
            NPC.width = 22;
            NPC.height = 40;
            NPC.alpha = 0;
            NPC.npcSlots = 0f;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.netAlways = true;
            NPC.Dnpc().LifeUP = false;
            NPC.scale = 1;
            NPC.GetGlobalNPC<DGlobalNPCExp>().Exp = 0;
            NPC.Dnpc().Properties.Control = false;
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

                new FlavorTextBestiaryInfoElement(DDSystem.English?"Those whom he appoints will suffer from him":"被他指定之人,将会承受来自他的痛苦")
            });
        }
        public override void FindFrame(int frameHeight)
        {
        }
        public override void AI()
        {
            NPC.GetGlobalNPC<DGlobalNPCExp>().Exp = 0;
            if (NPC.ai[0] >= 0)
            {
                Projectile Projectile;
                bool T = false;
                for (int R = 0; R < 1000; R++)
                {
                    Projectile = Main.projectile[R];
                    if (Projectile.active && Projectile.type == ModContent.ProjectileType<EnchantedVoodooDoll>() && Projectile.owner == NPC.ai[0])
                    {
                        T = true;
                        NPC.Center = Projectile.Center;
                        NPC.life = Projectile.Tproj().TalismanLife;
                        NPC.lifeMax = Projectile.Tproj().MaxTalismanLife;
                        NPC.defense = Main.npc[(int)Projectile.ai[0]].defense;
                        NPC.dontTakeDamage = Main.npc[(int)Projectile.ai[0]].dontTakeDamage;
                        NPC.netUpdate = true;
                        for (int a = 0; a < BuffLoader.BuffCount; a++)
                        {
                            if (NPC.HasBuff(a) && !Main.npc[(int)Projectile.ai[0]].HasBuff(a))
                            {
                                Main.npc[(int)Projectile.ai[0]].AddBuff(a, NPC.buffTime[NPC.FindBuffIndex(a)]);
                            }
                        }
                        NPC.ai[1] = R;
                        break;
                    }
                }

                if (!T)
                {
                    NPC.Kill();
                }
            }
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
        }
        public override void HitEffect(HitInfo hit)
        {
        }
        public override void OnHitByItem(Player player, Item item, NPC.HitInfo hit, int damageDone)
        {
            Projectile Projectile = Main.projectile[(int)NPC.ai[1]];
            Projectile.Tproj().TalismanLife -= damageDone;
            if (hit.Crit)
            {
                damageDone = (damageDone + NPC.defense / 2) / 2;
            }
            Main.npc[(int)Projectile.ai[0]].SimpleStrikeNPC(damageDone + NPC.defense / 2, hit.HitDirection, hit.Crit, hit.HitDirection, hit.DamageType);
            Main.npc[(int)Projectile.ai[0]].netUpdate = true;
            Projectile.DProj().Times[3] = 1;
            Projectile.netUpdate = true;
        }
        public override void OnHitByProjectile(Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            Projectile Projectile = Main.projectile[(int)NPC.ai[1]];
            Projectile.Tproj().TalismanLife -= damageDone;
            if(hit.Crit)
            {
                damageDone = (damageDone + NPC.defense / 2) / 2;
            }
            Main.npc[(int)Projectile.ai[0]].SimpleStrikeNPC(damageDone + NPC.defense / 2, hit.HitDirection, hit.Crit, hit.HitDirection, hit.DamageType);
            Main.npc[(int)Projectile.ai[0]].netUpdate = true;
            Projectile.DProj().Times[3] = 1;
            Projectile.netUpdate = true;
        }
        public override bool CheckDead()
        {
            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            spriteBatch.Draw(TextureAssets.Npc[Type].Value, NPC.Center - screenPos, NPC.frame, Color.White,0,TextureAssets.Npc[Type].Size()/2,NPC.scale,0,0);
        }
    }
}