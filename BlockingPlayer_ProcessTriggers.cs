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
		public override void ProcessTriggers(TriggersSet triggersSet)
		{
			if (Blocking.Guard.Current && BlockingConfig.Instance.enableGuarding && guardingCooldown == 0)
			{
				if (guardTimer < 40)
				{
					guardTimer += 1;
					//Some test I did..
					/*if (guardTimer == 20)
					{
						Main.NewText("Test for tick 20.");
					}
					if (guardTimer == 40)
					{
						Main.NewText("Test for tick 40.");
					}*/
				}
			}
			if (Blocking.Guard.JustPressed && BlockingConfig.Instance.enableGuarding && guardingCooldown == 0)
			{
				if (BlockingConfigClient.Instance.enableSoundsGuardingRaise)
				{
					SoundEngine.PlaySound(GuardRaise, Player.position);
				}
				if (BlockingConfigClient.Instance.enableScreenshakeGuarding)
				{
					screenShakeTimerGuarding += 5;
				}
			}
			if (Blocking.Guard.JustReleased && BlockingConfig.Instance.enableGuarding && guardingCooldown == 0)
			{
				guardTimer = 0;
				if (BlockingConfigClient.Instance.enableSoundsGuardingLower)
				{
					SoundEngine.PlaySound(GuardLower, Player.position);
				}
				if (BlockingConfigClient.Instance.enableScreenshakeGuarding)
				{
					screenShakeTimerGuarding += 5;
				}
				guardBashTimer = 0;
			}
			if (Blocking.Guard.Current && Blocking.GuardBash.JustPressed && guardBashCooldown < 1 && BlockingConfig.Instance.enableGuarding && BlockingConfig.Instance.enableGuardBashing)
			{
				if (BlockingConfigClient.Instance.enableSoundsGuardingBash)
				{
					SoundEngine.PlaySound(GuardBash, Player.position);
				}
				if (BlockingConfigClient.Instance.enableScreenshakeGuardingGuardBash)
				{
					screenShakeTimerGuarding += BlockingConfig.Instance.guardBashTimer;
				}
					guardBashTimer = BlockingConfig.Instance.guardBashTimer;
					guardBashCooldown = BlockingConfig.Instance.guardBashCooldown;
			}
			if (Blocking.TogglePotentGuard.JustPressed)
			{
				if (!potentGuarding)
				{
					potentGuarding = true;
					if (BlockingConfigClient.Instance.enableSoundsGuardingBlockShield)
					{
						SoundEngine.PlaySound(BlockShield, Player.position);
					}
				}
				else
				{
					potentGuarding = false;
					if (BlockingConfigClient.Instance.enableSoundsGuardingBlock)
					{
						SoundEngine.PlaySound(Block, Player.position);
					}
				}
			}
			if (BlockingConfig.Instance.enablePotentGuarding)
			{
				canPotentGuard = true;
			}
			else
			{
				canPotentGuard = false;
			}
			if (Blocking.Parry.JustPressed && parryCooldown == 0 && BlockingConfig.Instance.enableParrying)
			{
				if (BlockingConfigClient.Instance.enableSoundsParryingAttempt)
				{
					SoundEngine.PlaySound(ParryAttempt, Player.position);
				}
				if (BlockingConfigClient.Instance.enableScreenshakeParrying)
				{
					screenShakeTimerParryingAttempt += BlockingConfig.Instance.parryTimer;
				}
				if (parryTimer <= 0) //If less than or equal to 0.
				{
					parryTimer = BlockingConfig.Instance.parryTimer; //Set the Parry Timer to the configured value.
					parryCooldown = BlockingConfig.Instance.parryCooldown;
					if (hasGloveBenefits && BlockingConfig.Instance.enableGloveBenefits) //If wearing Gloves, then set the Parry Timer to the configured value.
					{
						parryTimer += BlockingConfig.Instance.gloveBenefitsParryTimer;
					}
				}
			}
		}
	}
}