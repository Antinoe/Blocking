using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace Blocking
{
    [Label("Server Config")]
    public class BlockingConfig : ModConfig
    {
        //This is here for the Config to work at all.
        public override ConfigScope Mode => ConfigScope.ServerSide;
		
        public static BlockingConfig Instance;
		
	[Header("General")]
		
        [Label("Enable Guarding")]
        [Tooltip("If false, Players cannot Guard, Parry or Guard Bash.\n[Default: On]")]
        [DefaultValue(true)]
        public bool enableGuarding {get; set;}
		
        [Label("Enable Potent Guarding")]
        [Tooltip("If false, Guarding cannot absorb damage at the cost of Mana.\n[Default: On]")]
        [DefaultValue(true)]
        public bool enablePotentGuarding {get; set;}
		
        [Label("Blocking Potency")]
        [Tooltip("The percentage of damage Guarding reduces.\n[Default: 0.6]")]
        [Slider]
        [DefaultValue(0.60f)]
        [Range(0f, 1f)]
        [Increment(.05f)]
        public float blockingPotency {get; set;}
		
        [Label("Blocking Immune Time")]
        [Tooltip("How long Players are immune after Blocking.\n[Default: 40]")]
        [Slider]
        [DefaultValue(40)]
        [Range(0, 120)]
        [Increment(5)]
        public int blockingImmuneTime {get; set;}
		
        [Label("Guarding Cooldown")]
        [Tooltip("How long until Guarding may be performed again.\n[Default: 0]")]
        [Slider]
        [DefaultValue(0)]
        [Range(0, 360)]
        [Increment(10)]
        public int guardingCooldown {get; set;}
		
        [Label("Guarding Movement Speed")]
        [Tooltip("How slowly Players move when Guarding.\n[Default: 0.3]")]
        [Slider]
        [DefaultValue(0.3f)]
        [Range(0.1f, 1f)]
        [Increment(.1f)]
        public float guardingMoveSpeed {get; set;}
		
        [Label("Enable Guard Bashing")]
        [Tooltip("If false, Players can not perform Guard Bashes.\n[Default: Off]")]
        [DefaultValue(false)]
        public bool enableGuardBashing {get; set;}
		
        [Label("Guard Bash Potency")]
        [Tooltip("How strong Guard Bashing is.\n[Default: 0.5]")]
        [Slider]
        [DefaultValue(0.5f)]
        [Range(0f, 10f)]
        [Increment(0.5f)]
        public float guardBashingPotency {get; set;}
		
        [Label("Guard Bash Timer")]
        [Tooltip("How long Guard Bashing is active.\n[Default: 10]")]
        [Slider]
        [DefaultValue(10)]
        [Range(0, 100)]
        [Increment(5)]
        public int guardBashTimer {get; set;}
		
        [Label("Guard Bash Cooldown")]
        [Tooltip("How long until Players may perform a Guard Bash again.\n[Default: 40]")]
        [Slider]
        [DefaultValue(40)]
        [Range(0, 100)]
        [Increment(5)]
        public int guardBashCooldown {get; set;}
		
	[Header("Shields")]
		
        [Label("Potent Guarding requires Shield")]
        [Tooltip("If false, Potent Guarding can be performed without a Shield.\n[Default: Om]")]
        [DefaultValue(true)]
        public bool potentGuardingRequiresShield {get; set;}
		
        [Label("Shield Blocking Potency")]
        [Tooltip("Additional percentage of damage Shield Guarding reduces when Potent Guarding is inactive.\n[Default: 0.1]")]
        [Slider]
        [DefaultValue(0.1f)]
        [Range(0f, 1f)]
        [Increment(.05f)]
        public float shieldBlockingPotency {get; set;}
		
        [Label("Potent Guarding Cooldown")]
        [Tooltip("How long until Guarding may be performed again.\n[Default: 0]")]
        [Slider]
        [DefaultValue(0)]
        [Range(0, 360)]
        [Increment(10)]
        public int potentGuardingCooldown {get; set;}
		
        [Label("Shield Life")]
        [Tooltip("How much extra Max Life shields grant (per 1 Defense).\n[Default: 25]")]
        [Slider]
        [DefaultValue(25)]
        [Range(0, 100)]
        [Increment(5)]
        public int shieldLife {get; set;}
		
	[Header("Parrying")]
		
        [Label("Enable Parrying")]
        [Tooltip("If false, Players cannot perform Parries.\n[Default: On]")]
        [DefaultValue(true)]
        public bool enableParrying {get; set;}
		
        [Label("Parry Timer")]
        [Tooltip("Time left to initiate a Parry.\n[Default: 10]")]
        [Slider]
        [DefaultValue(10)]
        [Range(0, 50)]
        [Increment(5)]
        public int parryTimer {get; set;}
		
        [Label("Parry Immune Time")]
        [Tooltip("How long Players are immune after Parrying.\n[Default: 60]")]
        [Slider]
        [DefaultValue(60)]
        [Range(0, 100)]
        [Increment(5)]
        public int parryImmuneTime {get; set;}
		
        [Label("Parry Cooldown")]
        [Tooltip("How long until Parries may be performed again.\n[Default: 0]")]
        [Slider]
        [DefaultValue(0)]
        [Range(0, 360)]
        [Increment(10)]
        public int parryCooldown {get; set;}
		
        [Label("Parry Counter Time")]
        [Tooltip("Time left to perform a Parry Counter.\n[Default: 40]")]
        [Slider]
        [DefaultValue(40)]
        [Range(0, 120)]
        [Increment(5)]
        public int parryCounterTime {get; set;}
		
        [Label("Parry Counter Cooldown")]
        [Tooltip("How long until Parry Counters may be performed again.\n[Default: 0]")]
        [Slider]
        [DefaultValue(0)]
        [Range(0, 360)]
        [Increment(10)]
        public int parryCounterCooldown {get; set;}
		
	[Header("Miscellaneous")]
		
        [Label("Enable Glove Benefits")]
        [Tooltip("If false, Players will not gain benefits from wearing Gloves.\n[Default: On]")]
        [DefaultValue(true)]
        public bool enableGloveBenefits {get; set;}
		
        [Label("Glove Benefits Blocking Potency")]
        [Tooltip("Additional percentage of damage Guarding reduces with Glove Benefits active.\n[Default: 0.05]")]
        [Slider]
        [DefaultValue(0.05f)]
        [Range(0f, 1f)]
        [Increment(.05f)]
        public float gloveBenefitsBlockingPotency {get; set;}
		
        [Label("Glove Benefits Parry Timer")]
        [Tooltip("Additional Parry Time granted from Gloves.\n[Default: 5]")]
        [Slider]
        [DefaultValue(5)]
        [Range(0, 50)]
        [Increment(5)]
        public int gloveBenefitsParryTimer {get; set;}
		
        [Label("Enable Boot Benefits")]
        [Tooltip("If false, Players will not gain benefits from wearing Boots.\n[Default: On]")]
        [DefaultValue(true)]
        public bool enableBootBenefits {get; set;}
		
        [Label("Boot Benefits Movement Speed")]
        [Tooltip("How much Movement Speed is increased when Guarding with Boot Benefits active.\n[Default: 0.75f]")]
        [Slider]
        [DefaultValue(0.35f)]
        [Range(0f, 2f)]
        [Increment(0.05f)]
        public float bootBenefitsMoveSpeed {get; set;}
    }
}