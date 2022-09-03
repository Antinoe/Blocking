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
		public override void PostUpdateMiscEffects() //Core.
		{
			//Guarding
			if (guardTimer > 0)
			{
				Player.endurance += BlockingConfig.Instance.blockingPotency;
				Player.accRunSpeed *= BlockingConfig.Instance.guardingMoveSpeed;
				Player.maxRunSpeed *= BlockingConfig.Instance.guardingMoveSpeed;
				Player.velocity.X *= 0.95f; //This is here to prevent usage of Speed items like the Hermes Boots or the Speedbooster from Metroid Mod.
				Player.delayUseItem = true;
			}
			if (guardingCooldown > 0)
			{
				guardingCooldown--;
			}
			
			//Parrying
			if (parryTimer > 0)
			{
				parryTimer--;
			}
			if (parryCooldown > 0)
			{
				parryCooldown--;
			}
			if (parryCounterCooldown > 0)
			{
				parryCounterCooldown--;
			}
			
			//Guard Bashing
			if (guardBashing)
			{
				Player.thorns = BlockingConfig.Instance.guardBashingPotency;
			}
			if (guardBashTimer > 0)
			{
				guardBashTimer--;
				guardBashing = true;
			}
			else
			{
				guardBashing = false;
			}
			if (guardBashCooldown > 0)
			{
				guardBashCooldown--;
			}
			
			//Shield Equipped
			if (guardTimer > 0 && hasShield)
			{
				Player.endurance += BlockingConfig.Instance.shieldBlockingPotency;
			}
			
			//Glove Equipped
			if (hasGlove && BlockingConfig.Instance.enableGloveBenefits)
			{
				hasGloveBenefits = true;
			}
			else
			{
				hasGloveBenefits = false;
			}
			if (guardTimer > 0 && BlockingConfig.Instance.enableGloveBenefits && BlockingConfig.Instance.gloveBenefitsBlockingPotency > 0)
			{
				Player.endurance += BlockingConfig.Instance.gloveBenefitsBlockingPotency;
			}
			
			//Boot Equipped
			if (hasBoot && BlockingConfig.Instance.enableBootBenefits)
			{
				hasBootBenefits = true;
			}
			else
			{
				hasBootBenefits = false;
			}
			if (hasBootBenefits && guardTimer > 0)
			{
				Player.moveSpeed += BlockingConfig.Instance.bootBenefitsMoveSpeed;
			}
			
			//Screenshake
			if (screenShakeTimerGuarding > 0)
			{
				screenShakeTimerGuarding--;
			}
			if (screenShakeTimerParryingAttempt > 0)
			{
				screenShakeTimerParryingAttempt--;
			}
			if (screenShakeTimerParrying > 0)
			{
				screenShakeTimerParrying--;
			}
		}
	}
}