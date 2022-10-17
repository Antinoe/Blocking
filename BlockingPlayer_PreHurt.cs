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
using Terraria.ModLoader.IO;
using Terraria.Audio;

namespace Blocking
{
    public partial class BlockingPlayer : ModPlayer
    {
		//Right before receiving damage.
		public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource, ref int cooldownCounter)
        {
			if (Player.immuneTime > 0)
			{
				return false;
			}
			else
			{
				if (parryTimer >= 1) //Parrying.
				{
					playSound = false;
					if (BlockingConfigClient.Instance.enableSoundsParrying)
					{
						SoundEngine.PlaySound(Parry, Player.position);
					}
					if (BlockingConfigClient.Instance.enableScreenshakeParrying)
					{
						screenShakeTimerParrying += BlockingConfig.Instance.parryTimer;
					}
					Player.immune = true;
					Player.immuneTime = BlockingConfig.Instance.parryImmuneTime; //Parry Immune Time is set to the configured value.
					parryCooldown = BlockingConfig.Instance.parryCooldown;
					if (parryCounterCooldown == 0 && BlockingConfig.Instance.enableParryCounters && parryCounter)
					{
						Player.AddBuff(BuffID.ParryDamageBuff, BlockingConfig.Instance.parryCounterTime); //Add the Striking Moment buff.
						parryCounterCooldown = BlockingConfig.Instance.parryCounterCooldown;
					}
					return false; //The reason we put the return value here, at the end of the Parry Counter code, rather than at the end of the Parry code, is because nothing after a Return statement is run. Therefore, if we had done the aforementioned, the Parry Counter code wouldn't run at all, rendering the ability useless.
				}
				
				if (guardTimer >= 1 && !hasShield && parryTimer == 0) //Guarding, but not Parrying and not Shield Guarding.
				{
					playSound = false;
					//The 2 strings below are what cause the Player to be launched back upon Shield Guarding.
					Player.velocity.X = 5f * hitDirection;
					Player.velocity.Y = -3f;
					if (BlockingConfigClient.Instance.enableSoundsGuardingBlock)
					{
						SoundEngine.PlaySound(Block, Player.position);
					}
					guardingCooldown = BlockingConfig.Instance.guardingCooldown;
					if (potentGuarding && canPotentGuard && !BlockingConfig.Instance.potentGuardingRequiresShield && Player.statMana >= damage - Player.statDefense)
					{
						Player.statMana -= damage - Player.statDefense;
						Player.immune = true;
						Player.immuneTime = BlockingConfig.Instance.blockingImmuneTime;
						return false;
					}
					else //No Mana to consume.
					{
						return true;
					}
				}
				
				if (guardTimer >= 1 && hasShield && parryTimer == 0) //Shield Guarding, but not Parrying.
				{
					playSound = false;
					//The 2 strings below are what cause the Player to be launched back upon Shield Guarding.
					Player.velocity.X = 5f * hitDirection;
					Player.velocity.Y = -3f;
					if (potentGuarding && canPotentGuard && Player.statMana >= damage - Player.statDefense)
					{
						Player.statMana -= damage - Player.statDefense;
					if (BlockingConfigClient.Instance.enableSoundsGuardingBlockShield)
					{
						SoundEngine.PlaySound(BlockShield, Player.position);
					}
						Player.immune = true;
						Player.immuneTime = BlockingConfig.Instance.blockingImmuneTime;
						potentGuardingCooldown = BlockingConfig.Instance.potentGuardingCooldown;
						return false;
					}
					else //No Mana to consume.
					{
						if (BlockingConfigClient.Instance.enableSoundsGuardingBlock)
						{
							SoundEngine.PlaySound(Block, Player.position);
						}
						if (BlockingConfigClient.Instance.enableSoundsGuardingBlockShieldBroken)
						{
							SoundEngine.PlaySound(BlockShieldBroken, Player.position);
						}
						guardingCooldown = BlockingConfig.Instance.guardingCooldown;
						return true;
					}
				}
				return true; //Keep this here. It must be at the end of the PreHurt Method so that the Player takes damage by default.
			}
		}
	}
}