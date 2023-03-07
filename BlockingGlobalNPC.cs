using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.IO;
using Terraria.Audio;
//	This is needed in order to use Blocking's player-specific fields.
using static Blocking.BlockingPlayer;

namespace Blocking
{
    public class BlockingGlobalNPC : GlobalNPC
    {
		public int npcGuardTimer = 0;
		public override bool InstancePerEntity => true;
		
		public override bool StrikeNPC(NPC NPC, ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
		{
			//Parrying
			if (BlockingConfig.Instance.NPCParryWhitelist.Contains(new NPCDefinition(NPC.type)))
			{
				if (Main.rand.Next(2) == 0)
				{
					if (BlockingConfigClient.Instance.enableSoundsParrying)
					{
						SoundEngine.PlaySound(Parry, NPC.position);
					}
					damage = 0;
					NPC.immuneTime = 200;
					return false;
				}
			}
			//Guarding
			if (BlockingConfig.Instance.NPCGuardWhitelist.Contains(new NPCDefinition(NPC.type)))
			{
				if (Main.rand.Next(2) == 0)
				{
					if (BlockingConfigClient.Instance.enableSoundsGuardingBlock)
					{
						SoundEngine.PlaySound(Block, NPC.position);
					}
					damage = damage / 2;
					return false;
				}
			}
			//Shield Guarding
			if (BlockingConfig.Instance.NPCShieldGuardWhitelist.Contains(new NPCDefinition(NPC.type)))
			{
				if (Main.rand.Next(BlockingConfig.Instance.npcShieldGuardChance) == 0)
				{
					if (BlockingConfigClient.Instance.enableSoundsGuardingBlockShield)
					{
						SoundEngine.PlaySound(BlockShield, NPC.position);
					}
					damage = damage / 4;
					return false;
				}
			}
			return true;
		}
		//	Won't be using this. Maybe another time..
		/*public override void ModifyHitByItem(NPC npc, Player player, Item item, ref int damage, ref float knockback, ref bool crit)
		{
			Main.NewText("ModifyHitByItem is being ran.");
			BlockingPlayer bp = player.GetModPlayer<BlockingPlayer>();
			if (bp.counterDamage > 0)
			{
				damage += bp.counterDamage;
				bp.counterDamage -= damage;
				Main.NewText("Counter Damage spent.");
				SoundEngine.PlaySound(BlockShield with {Pitch = +0.75f, Volume = 1f}, npc.position);
			}
		}*/
	}
}