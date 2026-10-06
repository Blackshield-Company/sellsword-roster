using Mutagen.Bethesda.Plugins;

namespace SellswordRoster.Generator;

/// <summary>
/// Skyrim.esm records this mod links to (FormIDs looked up with Mutagen.Bethesda.FormKeys.SkyrimSE).
/// Nothing here is overridden; the mod only references these.
/// </summary>
internal static class Vanilla
{
    public static readonly ModKey Skyrim = ModKey.FromFileName("Skyrim.esm");
    private static FormKey K(uint id) => new(Skyrim, id);

    public static readonly FormKey PlayerRef = K(0x000014);
    public static readonly FormKey MQ101 = K(0x03372B);
    public static readonly FormKey Gold001 = K(0x00000F);
    public static readonly FormKey EquipVoice = K(0x025BEE);

    // Vanilla follower system
    public static readonly FormKey DialogueFollower = K(0x0750BA);
    public static readonly FormKey PlayerFollowerCount = K(0x0BCC98);
    public static readonly FormKey PotentialFollowerFaction = K(0x05C84D);

    // Appearance / voice / stats / gear templates (leveled bandit characters, one race/sex each)
    public static readonly FormKey LCharBanditMeleeNordM = K(0x01A320);
    public static readonly FormKey LCharBanditMeleeNordF = K(0x01A321);
    public static readonly FormKey LCharBanditMeleeEvenTonedM = K(0x01A323);
    public static readonly FormKey LCharBanditMeleeEvenTonedF = K(0x01A322);
    public static readonly FormKey LCharBanditMeleeCommonerM = K(0x01A319);
    public static readonly FormKey LCharBanditMeleeCommonerF = K(0x01A31E);
    public static readonly FormKey LCharBanditBossOrcM = K(0x01B0EA);
    public static readonly FormKey LCharBanditMissile = K(0x01E770);

    public static readonly FormKey NordRace = K(0x013746);
    public static readonly FormKey ClassWarrior1H = K(0x013176);
    public static readonly FormKey CsHumanMelee = K(0x068849);
}
