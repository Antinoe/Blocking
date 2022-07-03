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
    public class BlockingPlayer : ModPlayer
    {
		public bool guarding = false;
		public bool parrying = false;
		public int parryTimer = 0;
		public bool hasShield = false;
		public bool hasGlove = false;
		public bool hasGloveBenefits = false;
		public bool hasBoot = false;
		public bool hasBootBenefits = false;
		public bool guardBashing = false;
		public int guardBashTimer = 0;
		public int guardBashCooldown = 0;
		public bool potentGuarding = true;
		public bool canPotentGuard;
		public int guardingCooldown = 0;
		public int potentGuardingCooldown = 0;
		public int parryCooldown = 0;
		public int parryCounterCooldown = 0;
		
		//Resetting the Booleans is important, as some of them may not reset by themselves.
		public override void ResetEffects()
		{
			guarding = false;
			hasShield = false;
			hasGlove = false;
			//hasGloveBenefits = false; //Resetting this nullifies the effect completely. :moyai:
			hasBoot = false;
			//hasBootBenefits = false; //Resetting this nullifies the effect completely. :moyai:
		}
		
		public static BlockingPlayer ModPlayer(Player Player)
		{
			return Player.GetModPlayer<BlockingPlayer>();
		}
		
		//Sounds.
		//Question: Why declare the audio like this?
		//Answer: A simple step in Code Optimization. If we were to declare this sound many times over in different places, it would get messy very quickly.
		public static readonly SoundStyle GuardRaise = new SoundStyle("Blocking/Sounds/GuardRaise");
		public static readonly SoundStyle GuardLower = new SoundStyle("Blocking/Sounds/GuardLower");
		public static readonly SoundStyle GuardBash = new SoundStyle("Blocking/Sounds/GuardBash");
		public static readonly SoundStyle Parry = new SoundStyle("Blocking/Sounds/Parry");
		public static readonly SoundStyle Block = new SoundStyle("Blocking/Sounds/Block");
		public static readonly SoundStyle BlockShield = new SoundStyle("Blocking/Sounds/BlockShield");
		public static readonly SoundStyle BlockShieldBroken = new SoundStyle("Blocking/Sounds/BlockShieldBroken");
		
		public override void PostUpdateMiscEffects() //Core.
		{
			//Controls
			if (Blocking.Guard.Current && BlockingConfig.Instance.enableGuarding && guardingCooldown == 0)
			{
				guarding = true;
			}
			if (Blocking.Guard.JustPressed && BlockingConfig.Instance.enableGuarding && guardingCooldown == 0)
			{
				SoundEngine.PlaySound(GuardRaise, Player.position);
				if (parryTimer <= 0) //If less than or equal to 0.
				{
					if (BlockingConfig.Instance.enableParrying)
					{
						parryTimer = BlockingConfig.Instance.parryTimer; //Set the Parry Timer to the configured value.
						if (hasGloveBenefits && BlockingConfig.Instance.enableGloveBenefits) //If wearing Gloves, then set the Parry Timer to the configured value.
						{
							parryTimer += BlockingConfig.Instance.gloveBenefitsParryTimer;
						}
					}
				}
			}
			if (Blocking.Guard.JustReleased && BlockingConfig.Instance.enableGuarding && guardingCooldown == 0)
			{
				SoundEngine.PlaySound(GuardLower, Player.position);
				parryTimer = 0;
				guardBashTimer = 0;
				guarding = false;
			}
			if (Blocking.Guard.Current && Blocking.GuardBash.JustPressed && guardBashCooldown < 1 && BlockingConfig.Instance.enableGuarding && BlockingConfig.Instance.enableGuardBashing)
			{
					SoundEngine.PlaySound(GuardBash, Player.position);
					guardBashTimer = BlockingConfig.Instance.guardBashTimer;
					guardBashCooldown = BlockingConfig.Instance.guardBashCooldown;
			}
			if (Blocking.TogglePotentGuard.JustPressed)
			{
				if (!potentGuarding)
				{
					potentGuarding = true;
					SoundEngine.PlaySound(BlockShield, Player.position);
				}
				else
				{
					potentGuarding = false;
					SoundEngine.PlaySound(Block, Player.position);
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
			
			//Guarding
			if (guarding)
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
				parrying = true;
			}
			else
			{
				parrying = false;
			}
			if (parryCooldown > 0)
			{
				parryCooldown--;
			}
			if (parryCounterCooldown > 0)
			{
				parryCounterCooldown--;
			}
			
			//Shield Equipped
			if (guarding && hasShield)
			{
				Player.endurance += BlockingConfig.Instance.shieldBlockingPotency;
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
			
			//Glove Equipped
			if (hasGlove && BlockingConfig.Instance.enableGloveBenefits)
			{
				hasGloveBenefits = true;
			}
			else
			{
				hasGloveBenefits = false;
			}
			if (guarding && BlockingConfig.Instance.enableGloveBenefits && BlockingConfig.Instance.gloveBenefitsBlockingPotency > 0)
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
			if (hasBootBenefits && guarding)
			{
				Player.moveSpeed += BlockingConfig.Instance.bootBenefitsMoveSpeed;
			}
		}
		
		public override void PostUpdate() //Animations
		{
			if (guarding && parrying) //If Guarding and Parrying, then have arms spread out.
			{
				Player.bodyFrame.Y = Player.bodyFrame.Height * 10;
			}
			else if (guarding) //Otherwise, have arms in a more "Guarding" position.
			{
				Player.bodyFrame.Y = Player.bodyFrame.Height * 6;
			}
		}

		//Right before receiving damage.
		public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref Terraria.DataStructures.PlayerDeathReason damageSource)
        {
			if (parrying && parryCooldown == 0) //Parrying.
			{
				SoundEngine.PlaySound(Parry, Player.position);
				Player.immune = true;
				Player.immuneTime = BlockingConfig.Instance.parryImmuneTime; //Parry Immune Time is set to the configured value.
				parryCooldown = BlockingConfig.Instance.parryCooldown;
				if (parryCounterCooldown == 0)
				{
					Player.AddBuff(BuffID.ParryDamageBuff, BlockingConfig.Instance.parryCounterTime); //Add the Striking Moment buff.
					parryCounterCooldown = BlockingConfig.Instance.parryCounterCooldown;
				}
				return false; //The reason we put the return value here, at the end of the Parry Counter code, rather than at the end of the Parry code, is because nothing after a Return statement is run. Therefore, if we had done the aforementioned, the Parry Counter code wouldn't run at all, rendering the ability useless.
			}
			
			if (guarding && !hasShield && !parrying) //Guarding, but not Parrying and not Shield Guarding.
			{
				//The 2 strings below are what cause the Player to be launched back upon Shield Guarding.
				Player.velocity.X = 5f * hitDirection;
				Player.velocity.Y = -3f;
				SoundEngine.PlaySound(Block, Player.position);
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
			
			if (guarding && hasShield && !parrying) //Shield Guarding, but not Parrying.
			{
				//The 2 strings below are what cause the Player to be launched back upon Shield Guarding.
				Player.velocity.X = 5f * hitDirection;
				Player.velocity.Y = -3f;
				if (potentGuarding && canPotentGuard && Player.statMana >= damage - Player.statDefense)
				{
					Player.statMana -= damage - Player.statDefense;
					SoundEngine.PlaySound(BlockShield, Player.position);
					Player.immune = true;
					Player.immuneTime = BlockingConfig.Instance.blockingImmuneTime;
					potentGuardingCooldown = BlockingConfig.Instance.potentGuardingCooldown;
					return false;
				}
				else //No Mana to consume.
				{
					SoundEngine.PlaySound(Block, Player.position);
					SoundEngine.PlaySound(BlockShieldBroken, Player.position);
					guardingCooldown = BlockingConfig.Instance.guardingCooldown;
					return true;
				}
			}
			return true; //Keep this here. It must be at the end of the PreHurt Method so that the Player takes damage by default.
		}
		
		//Immune Time and received damage is only after PreHurt, so the Immune Time scripts must be here, in the PostHurt Method. (Using the Hurt Method will not work.)
		public override void PostHurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit)
		{
			if (guarding && !hasShield && !parrying) //Guarding, but not Parrying and not Shield Guarding.
			{
				Player.immune = true;
				Player.immuneTime = BlockingConfig.Instance.blockingImmuneTime;
				guardingCooldown = BlockingConfig.Instance.guardingCooldown;
			}
			if (guarding && hasShield && !parrying) //Shield Guarding, but not Parrying.
			{
				Player.immune = true;
				Player.immuneTime = BlockingConfig.Instance.blockingImmuneTime;
				guardingCooldown = BlockingConfig.Instance.guardingCooldown;
			}
		}
	}
}