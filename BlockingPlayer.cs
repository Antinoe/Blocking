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
		public int guardTimer = 0;
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
		public bool parryCounter = true;
		public int parryCounterCooldown = 0;
		public int screenShakeTimerGuarding = 0;
		public int screenShakeTimerParryingAttempt = 0;
		public int screenShakeTimerParrying = 0;
		
		//	Resetting the Booleans is important, as some of them may not reset by themselves.
		public override void ResetEffects()
		{
			//guardTimer = 0; //	Not resetting this anymore since the function stops working completely when doing so.
			hasShield = false;
			hasGlove = false;
			//hasGloveBenefits = false; //	Resetting this nullifies the effect completely. :moyai:
			hasBoot = false;
			//hasBootBenefits = false; //	Resetting this nullifies the effect completely. :moyai:
		}
		
		//	I'm not sure if I need this anymore, but I'll keep it for now.
		public static BlockingPlayer ModPlayer(Player player)
		{
			return player.GetModPlayer<BlockingPlayer>();
		}
		
		public override void PostUpdateMiscEffects()
		{
			//	Guarding
			if (guardTimer > 0)
			{
				Player.endurance += BlockingConfig.Instance.blockingPotency;
				Player.accRunSpeed *= BlockingConfig.Instance.guardingMoveSpeed;
				Player.maxRunSpeed *= BlockingConfig.Instance.guardingMoveSpeed;
				Player.velocity.X *= 0.95f; //	This is here to prevent usage of Speed items like the Hermes Boots or the Speedbooster from Metroid Mod.
				Player.delayUseItem = true;
			}
			if (guardingCooldown > 0)
			{
				guardingCooldown--;
			}
			
			//	Parrying
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
			
			//	Guard Bashing
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
			
			//	Shield Equipped
			if (guardTimer > 0 && hasShield)
			{
				Player.endurance += BlockingConfig.Instance.shieldBlockingPotency;
			}
			
			//	Glove Equipped
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
			
			//	Boot Equipped
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
			
			//	Screenshake
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
		
		public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource, ref int cooldownCounter)
        {
			//	Not sure what bug this fixed, but I'll leave it here anyway.
			if (Player.immuneTime > 0)
			{
				return false;
			}
			else
			{
				//	Guarding
				if (guardTimer > 0)
				{
					if (!hasShield && parryTimer == 0)
					{
						playSound = false;
						//	Potent Guarding
						if (potentGuarding && canPotentGuard && !BlockingConfig.Instance.potentGuardingRequiresShield && Player.statMana >= damage - Player.statDefense)
						{
							Player.statMana -= damage - Player.statDefense;
							Player.velocity.X = 5f * hitDirection;
							Player.velocity.Y = -3f;
							OnPotentBlock();
							return false;
						}
						//	No Mana left for Potent Guarding. Normal Guarding.
						else
						{
							Player.velocity.X = 5f * hitDirection;
							Player.velocity.Y = -3f;
							OnBlock();
							return true;
						}
					}
					
					//	Shield Guarding
					if (hasShield && parryTimer == 0)
					{
						playSound = false;
						//	Potent Shield Guarding.
						if (potentGuarding && canPotentGuard && Player.statMana >= damage - Player.statDefense)
						{
							Player.velocity.X = 5f * hitDirection;
							Player.velocity.Y = -3f;
							Player.statMana -= damage - Player.statDefense;
							OnPotentBlockShield();
							return false;
						}
						//	No Mana left to Potently Shield Guard.
						else
						{
							Player.velocity.X = 5f * hitDirection;
							Player.velocity.Y = -3f;
							OnBlockShield();
							return true;
						}
					}
				}
				//	Parrying.
				if (parryTimer > 0)
				{
					playSound = false;
					OnParry();
					return false; //	The reason we put the return value here, at the end of the Parry Counter code, rather than at the end of the Parry code, is because nothing after a Return statement is run. Therefore, if we had done the aforementioned, the Parry Counter code wouldn't run at all, rendering the ability useless.
					//	Remember: The placement of return values is very necessary. Always put it at the end of statements.
				}
				
				return true; //	Keep this here. It must be at the end of the PreHurt Method so that the Player takes damage by default.
			}
		}
		
		//	Immune Time and received damage is only after PreHurt, so the Immune Time scripts must be here, in the PostHurt Method. (Using the Hurt Method will not work.)
		public override void PostHurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit, int cooldownCounter)
		{
			//	Guarding
			if (guardTimer > 0 && !hasShield && parryTimer == 0)
			{
				Player.immune = true;
				Player.immuneTime = BlockingConfig.Instance.blockingImmuneTime;
				guardingCooldown = BlockingConfig.Instance.guardingCooldown;
			}
			//	Shield Guarding
			if (guardTimer > 0 && hasShield && parryTimer == 0)
			{
				Player.immune = true;
				Player.immuneTime = BlockingConfig.Instance.blockingImmuneTime;
				guardingCooldown = BlockingConfig.Instance.guardingCooldown;
			}
		}
		
		public void OnBlock()
		{
			if (BlockingConfigClient.Instance.enableSoundsGuardingBlock)
			{
				SoundEngine.PlaySound(Block, Player.position);
			}
			guardingCooldown = BlockingConfig.Instance.guardingCooldown;
		}
		public void OnPotentBlock()
		{
			Player.immune = true;
			Player.immuneTime = BlockingConfig.Instance.blockingImmuneTime;
			if (BlockingConfigClient.Instance.enableSoundsGuardingBlock)
			{
				SoundEngine.PlaySound(Block, Player.position);
			}
			guardingCooldown = BlockingConfig.Instance.guardingCooldown;
		}
		public void OnBlockShield()
		{
			if (BlockingConfigClient.Instance.enableSoundsGuardingBlockShieldBroken)
			{
				SoundEngine.PlaySound(BlockShield with {Pitch = +0.25f, Volume = 1f}, Player.position);
			}
			guardingCooldown = BlockingConfig.Instance.guardingCooldown;
		}
		public void OnPotentBlockShield()
		{
			if (BlockingConfigClient.Instance.enableSoundsGuardingBlockShield)
			{
				SoundEngine.PlaySound(BlockShield, Player.position);
			}
			Player.immune = true;
			Player.immuneTime = BlockingConfig.Instance.blockingImmuneTime;
			potentGuardingCooldown = BlockingConfig.Instance.potentGuardingCooldown;
		}
		public void OnParry()
		{
			if (BlockingConfigClient.Instance.enableSoundsParrying)
			{
				SoundEngine.PlaySound(Parry, Player.position);
			}
			if (BlockingConfigClient.Instance.enableScreenshakeParrying)
			{
				screenShakeTimerParrying += BlockingConfig.Instance.parryTimer;
			}
			Player.immune = true;
			Player.immuneTime = BlockingConfig.Instance.parryImmuneTime;
			parryCooldown = BlockingConfig.Instance.parryCooldown;
			if (parryCounterCooldown == 0 && BlockingConfig.Instance.enableParryCounters && parryCounter)
			{
				Player.AddBuff(BuffID.ParryDamageBuff, BlockingConfig.Instance.parryCounterTime);
				parryCounterCooldown = BlockingConfig.Instance.parryCounterCooldown;
			}
		}
		
		//	Animations
		public override void PostUpdate()
		{
			if (guardTimer >= 1)
			{
				Player.bodyFrame.Y = Player.bodyFrame.Height * 10;
			}
			if (guardTimer >= 10)
			{
				Player.bodyFrame.Y = Player.bodyFrame.Height * 11;
			}
			if (parryTimer >= 1)
			{
				Player.bodyFrame.Y = Player.bodyFrame.Height * 1;
			}
			if (parryTimer >= 4)
			{
				Player.bodyFrame.Y = Player.bodyFrame.Height * 2;
			}
			if (parryTimer >= 7)
			{
				Player.bodyFrame.Y = Player.bodyFrame.Height * 3;
			}
			if (parryTimer >= 10)
			{
				Player.bodyFrame.Y = Player.bodyFrame.Height * 4;
			}
		}
		
		public override void ModifyScreenPosition()
		{
			if (screenShakeTimerGuarding > 0)
			{
				Main.screenPosition.X += (float)Math.Round(Main.rand.Next((int)(0f - 1), (int)1) * BlockingConfigClient.Instance.screenShakeAmountGuarding);
				Main.screenPosition.Y += (float)Math.Round(Main.rand.Next((int)(0f - 1), (int)1) * BlockingConfigClient.Instance.screenShakeAmountGuarding);
			}
			if (screenShakeTimerParryingAttempt > 0)
			{
				Main.screenPosition.X += (float)Math.Round(Main.rand.Next((int)(0f - 1), (int)1) * BlockingConfigClient.Instance.screenShakeAmountParryingAttempt);
				Main.screenPosition.Y += (float)Math.Round(Main.rand.Next((int)(0f - 1), (int)1) * BlockingConfigClient.Instance.screenShakeAmountParryingAttempt);
			}
			if (screenShakeTimerParrying > 0)
			{
				Main.screenPosition.X += (float)Math.Round(Main.rand.Next((int)(0f - 1), (int)1) * BlockingConfigClient.Instance.screenShakeAmountParrying);
				Main.screenPosition.Y += (float)Math.Round(Main.rand.Next((int)(0f - 1), (int)1) * BlockingConfigClient.Instance.screenShakeAmountParrying);
			}
		}
		
		//	Sounds
		//	Question: Why declare the audio like this?
		//	Answer: A simple step in Code Optimization. If we were to declare this sound many times over in different places, it would get messy very quickly.
		public static readonly SoundStyle GuardRaise = new SoundStyle("Blocking/Sounds/GuardRaise");
		public static readonly SoundStyle GuardLower = new SoundStyle("Blocking/Sounds/GuardLower");
		public static readonly SoundStyle GuardBash = new SoundStyle("Blocking/Sounds/GuardBash");
		public static readonly SoundStyle ParryAttempt = new SoundStyle("Blocking/Sounds/ParryAttempt");
		public static readonly SoundStyle Parry = new SoundStyle("Blocking/Sounds/Parry");
		public static readonly SoundStyle Block = new SoundStyle("Blocking/Sounds/Block");
		public static readonly SoundStyle BlockShield = new SoundStyle("Blocking/Sounds/BlockShield");
	}
}
