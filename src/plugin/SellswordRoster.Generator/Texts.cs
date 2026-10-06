namespace SellswordRoster.Generator;

internal static class Texts
{
    public sealed record Merc(string Key, string Name, string Template, string Bio, string Trait, string TraitText, int Wage);

    public static readonly Merc[] Mercs =
    {
        new("Hroki", "Hroki Stone-Arm", "NordM",
            "A grey-bearded Nord who has held shield walls for jarls, merchants and, he admits, one very rich goat farmer. He doesn't run.",
            "Shield-Brother", "+50 armor", 40),
        new("Vala", "Vala the Red", "NordF",
            "She earned the name in the Reach and won't say how. Hits first, asks never.",
            "Bloodhound", "weapon attacks do 25% more damage", 50),
        new("Septimus", "Septimus Varo", "EvenTonedM",
            "A former Legion quartermaster. Marches all day, complains all night, never drops behind.",
            "Iron Lungs", "+50 stamina, stamina regenerates 50% faster", 35),
        new("Ilsa", "Ilsa Brightwater", "EvenTonedF",
            "Trained with the priests of Kynareth until she found the pay was better with a sword. Patches herself up between swings.",
            "Field Medic", "health regenerates twice as fast", 45),
        new("Durak", "Durak gro-Bagol", "OrcM",
            "Once a chieftain's champion, now between strongholds. Spells slide off him. So do most arguments.",
            "Thick Hide", "25% magic resistance", 60),
        new("AshEye", "Ash-Eye", "Archer",
            "Nobody knows their real name. Rumor says they once shot a hawk out of a snowstorm.",
            "Deadeye", "+25 Archery", 45),
        new("Bram", "Bram the Mule", "CommonerM",
            "Carries more than a pack horse and eats about as much. Swings a fair axe when he puts the bags down.",
            "Pack Mule", "+150 carry weight", 30),
        new("Liv", "Liv Quickstep", "CommonerF",
            "A Riften runner gone legitimate, mostly. First to the fight, first to the loot.",
            "Fleet-Footed", "moves 15% faster", 35),
    };

    public const string Intro = "SELLSWORD ROSTER\n\nWord has gone around that you pay in coin. Sellswords looking for work now send word of where to find them.\n\nUse the lesser power Sellsword Roster to see who is available this week. A hired sellsword follows you like any follower and takes a daily wage every 24 hours. Miss two paydays in a row and they walk.";
    public const string Roster = "SELLSWORD ROSTER\n\nLooking for work this week (the roster changes in %.0f days):";
    public const string HiredMenu = "SELLSWORD ROSTER\n\nYou have a sellsword in your service. Their next wage is due in %.0f hours.";
    public const string Empty = "No sellswords are looking for work this week. Try again in %.0f days.";
    public const string HaveFollower = "You already have a follower. Part ways with them first: only one follower at a time.";
    public const string NoGold = "You can't pay the first day's wage (%.0f gold).";
}
