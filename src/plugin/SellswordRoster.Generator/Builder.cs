using Modkit.Mutagen;
using Mutagen.Bethesda;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Records;
using Mutagen.Bethesda.Skyrim;
using Noggog;
using static SellswordRoster.Generator.Vanilla;

namespace SellswordRoster.Generator;

internal sealed class Builder
{
    private readonly SkyrimMod _mod;
    private readonly FormIdRegistry _ids;
    private static readonly SkyrimRelease R = SkyrimRelease.SkyrimSE;
    private const byte VanillaQuestFormVersion = 65;
    public const int AliasPlayer = 0;
    public const int FirstMercAlias = 1;

    public Builder(SkyrimMod mod, FormIdRegistry ids)
    {
        _mod = mod;
        _ids = ids;
    }

    private FormKey Id(string editorId) => _ids.Get(editorId);
    private static FormLink<T> L<T>(FormKey k) where T : class, IMajorRecordGetter => new(k);

    private static FormKey Template(string key) => key switch
    {
        "NordM" => LCharBanditMeleeNordM,
        "NordF" => LCharBanditMeleeNordF,
        "EvenTonedM" => LCharBanditMeleeEvenTonedM,
        "EvenTonedF" => LCharBanditMeleeEvenTonedF,
        "CommonerM" => LCharBanditMeleeCommonerM,
        "CommonerF" => LCharBanditMeleeCommonerF,
        "OrcM" => LCharBanditBossOrcM,
        "Archer" => LCharBanditMissile,
        _ => throw new ArgumentException(key),
    };

    // Trait effects: (actor value, magnitude) pairs, applied as a constant ability on the sellsword.
    private static (ActorValue Av, float Mag)[] TraitEffects(string key) => key switch
    {
        "Hroki" => new[] { (ActorValue.DamageResist, 50f) },
        "Vala" => new[] { (ActorValue.AttackDamageMult, 0.25f) },
        "Septimus" => new[] { (ActorValue.Stamina, 50f), (ActorValue.StaminaRateMult, 50f) },
        "Ilsa" => new[] { (ActorValue.HealRateMult, 100f) },
        "Durak" => new[] { (ActorValue.ResistMagic, 25f) },
        "AshEye" => new[] { (ActorValue.Archery, 25f) },
        "Bram" => new[] { (ActorValue.CarryWeight, 150f) },
        "Liv" => new[] { (ActorValue.SpeedMult, 15f) },
        _ => throw new ArgumentException(key),
    };

    public void Build()
    {
        var qRoster = Id("SSR_RosterQuest");
        var mercs = Texts.Mercs;

        // --- Settings ------------------------------------------------------------------------------
        var gGranted = Global("SSR_PowerGranted", 0);
        var gRotation = Global("SSR_RotationDays", 7);
        var gSize = Global("SSR_RosterSize", 3);
        var gWageMult = Global("SSR_WageMult", 100);
        var gGrace = Global("SSR_GraceDays", 1);
        var gCooldown = Global("SSR_LeaveCooldownDays", 14);
        var gPoll = Global("SSR_PollSeconds", 5);
        var gHired = Global("SSR_HiredIndex", -1);
        var gPaid = Global("SSR_WagesPaid", 0);
        var avail = mercs.Select((m, i) => Global($"SSR_Avail{i + 1:00}", 0)).ToArray();

        var faction = Add(new Faction(Id("SSR_SellswordFaction"), R)
        {
            EditorID = "SSR_SellswordFaction",
            Name = "Roster Sellswords",
            Flags = Faction.FactionFlag.HiddenFromPC,
        });

        // --- Sellswords ------------------------------------------------------------------------------
        // Looks, voice, stats, gear and combat data from a vanilla leveled bandit template; own name,
        // factions (no bandit faction), AI data, spells (the trait) and no packages.
        const NpcConfiguration.TemplateFlag looksAndGear = NpcConfiguration.TemplateFlag.Traits | NpcConfiguration.TemplateFlag.Stats
            | NpcConfiguration.TemplateFlag.ModelAnimation | NpcConfiguration.TemplateFlag.Inventory
            | NpcConfiguration.TemplateFlag.AttackData | NpcConfiguration.TemplateFlag.Keywords;
        var npcs = new List<FormKey>();
        var details = new List<FormKey>();
        foreach (var m in mercs)
        {
            var effects = TraitEffects(m.Key).Select((e, j) => Effect($"SSR_Eff{m.Key}{(j == 0 ? "" : (j + 1).ToString())}",
                $"{m.Trait}", e.Av, $"{m.Trait}: {m.TraitText}.")).ToArray();
            var ability = Add(new Spell(Id($"SSR_Trait{m.Key}"), R)
            {
                EditorID = $"SSR_Trait{m.Key}",
                Name = m.Trait,
                Description = $"{m.Trait}: {m.TraitText}.",
                Type = SpellType.Ability,
                CastType = CastType.ConstantEffect,
                TargetType = TargetType.Self,
                Flags = SpellDataFlag.ManualCostCalc | SpellDataFlag.NoAbsorbOrReflect,
                Effects = TraitEffects(m.Key).Select((e, j) => Fx(effects[j], e.Mag, 0)).ToExtendedList(),
            });

            var n = new Npc(Id($"SSR_Merc{m.Key}"), R)
            {
                EditorID = $"SSR_Merc{m.Key}",
                Name = m.Name,
                ShortName = m.Name.Split(' ')[0],
                ObjectBounds = new ObjectBounds { First = new P3Int16(-22, -14, 0), Second = new P3Int16(22, 14, 128) },
                Template = new FormLinkNullable<INpcSpawnGetter>(Template(m.Template)),
                Race = L<IRaceGetter>(NordRace),
                Class = L<IClassGetter>(ClassWarrior1H),
                CombatStyle = L<ICombatStyleGetter>(CsHumanMelee).AsNullable(),
                Configuration = new NpcConfiguration
                {
                    Flags = NpcConfiguration.Flag.AutoCalcStats | NpcConfiguration.Flag.Unique,
                    TemplateFlags = looksAndGear,
                    Level = new PcLevelMult { LevelMult = 1 },
                    CalcMinLevel = 1,
                    CalcMaxLevel = 0,
                    SpeedMultiplier = 100,
                    DispositionBase = 35,
                },
                AIData = new AIData
                {
                    Aggression = Aggression.Unaggressive, Confidence = Confidence.Brave, EnergyLevel = 50,
                    Responsibility = Responsibility.NoCrime, Mood = Mood.Neutral, Assistance = Assistance.HelpsFriendsAndAllies,
                },
                PlayerSkills = new PlayerSkills(),
                Height = 1,
                Weight = 50,
                ActorEffect = new() { L<ISpellRecordGetter>(ability.FormKey) },
            };
            n.Factions.Add(new RankPlacement { Faction = L<IFactionGetter>(faction.FormKey), Rank = 0 });
            n.Factions.Add(new RankPlacement { Faction = L<IFactionGetter>(PotentialFollowerFaction), Rank = 0 });
            Add(n);
            npcs.Add(n.FormKey);

            details.Add(Msg($"SSR_MsgDetail{m.Key}",
                $"{m.Name.ToUpperInvariant()}\n\n{m.Bio}\n\nTrait: {m.Trait} ({m.TraitText}).\nWage: %.0f gold a day, the first day paid up front.",
                Btn("Hire"), Btn("Back")));
        }

        // --- Messages --------------------------------------------------------------------------------
        var rosterButtons = mercs.Select((m, i) =>
        {
            var b = Btn($"{m.Name} ({m.Trait})");
            b.Conditions.Add(Cond.Global(avail[i], CompareOperator.EqualTo, 1));
            return b;
        }).Append(Btn("Close")).ToArray();
        var msgRoster = Msg("SSR_MsgRoster", Texts.Roster, rosterButtons);
        var msgIntro = Msg("SSR_MsgIntro", Texts.Intro, Btn("Ok"));
        var msgHired = Msg("SSR_MsgHiredMenu", Texts.HiredMenu, Btn("Show the roster"), Btn("Release them"), Btn("Close"));
        var msgEmpty = Notice("SSR_MsgEmpty", Texts.Empty);
        var msgHave = Notice("SSR_MsgHaveFollower", Texts.HaveFollower);
        var msgNoGold = Notice("SSR_MsgNoGold", Texts.NoGold);

        // --- Power -----------------------------------------------------------------------------------
        var mgefPower = Add(new MagicEffect(Id("SSR_PowerEffect"), R)
        {
            EditorID = "SSR_PowerEffect",
            Name = "Sellsword Roster",
            Flags = MagicEffect.Flag.NoDuration | MagicEffect.Flag.NoMagnitude | MagicEffect.Flag.NoArea
                    | MagicEffect.Flag.Painless | MagicEffect.Flag.NoHitEvent | MagicEffect.Flag.HideInUI,
            MagicSkill = ActorValue.None, ResistValue = ActorValue.None, SecondActorValue = ActorValue.None,
            Archetype = new MagicEffectArchetype { Type = MagicEffectArchetype.TypeEnum.Script, ActorValue = ActorValue.None },
            CastType = CastType.FireAndForget,
            TargetType = TargetType.Self,
            CastingSoundLevel = SoundLevel.Normal,
            Description = "See which sellswords are looking for work.",
            VirtualMachineAdapter = Vmad.Adapter(Vmad.Script("SSR_PowerEffectScript", Vmad.Obj("Roster", qRoster))),
        });
        var power = Add(new Spell(Id("SSR_PowerRoster"), R)
        {
            EditorID = "SSR_PowerRoster",
            Name = "Sellsword Roster",
            Description = "See which sellswords are looking for work this week, and hire one.",
            Type = SpellType.LesserPower,
            CastType = CastType.FireAndForget,
            TargetType = TargetType.Self,
            Flags = SpellDataFlag.ManualCostCalc | SpellDataFlag.NoAbsorbOrReflect,
            EquipmentType = L<IEquipTypeGetter>(EquipVoice).AsNullable(),
            Effects = { Fx(mgefPower.FormKey, 0, 0) },
        });

        // --- Controller quest: player alias + one alias per sellsword (keeps them persistent) ---------
        var quest = Add(new Quest(qRoster, R)
        {
            EditorID = "SSR_RosterQuest",
            Name = "Sellsword Roster",
            Flags = Quest.Flag.StartGameEnabled | Quest.Flag.RunOnce,
            Priority = 30,
            Type = Quest.TypeEnum.None,
            QuestFormVersion = VanillaQuestFormVersion,
        });
        quest.Aliases.Add(new QuestAlias
        {
            ID = AliasPlayer, Type = QuestAlias.TypeEnum.Reference, Name = "Player",
            ForcedReference = new FormLinkNullable<IPlacedGetter>(PlayerRef),
        });
        for (int i = 0; i < mercs.Length; i++)
        {
            quest.Aliases.Add(new QuestAlias
            {
                ID = (uint)(FirstMercAlias + i), Type = QuestAlias.TypeEnum.Reference, Name = $"Merc{mercs[i].Key}",
                Flags = QuestAlias.Flag.Optional,
            });
        }
        quest.NextAliasID = (uint)(FirstMercAlias + mercs.Length);

        var adapter = Vmad.QuestAdapter(Vmad.Script("SSR_RosterScript",
                Vmad.Obj("SSR_PowerGranted", gGranted), Vmad.Obj("SSR_RotationDays", gRotation), Vmad.Obj("SSR_RosterSize", gSize),
                Vmad.Obj("SSR_WageMult", gWageMult), Vmad.Obj("SSR_GraceDays", gGrace), Vmad.Obj("SSR_LeaveCooldownDays", gCooldown),
                Vmad.Obj("SSR_PollSeconds", gPoll), Vmad.Obj("SSR_HiredIndex", gHired), Vmad.Obj("SSR_WagesPaid", gPaid),
                Vmad.ObjList("Avail", avail),
                Vmad.ObjList("Mercs", npcs.ToArray()),
                Vmad.StrList("Names", mercs.Select(m => m.Name).ToArray()),
                Vmad.IntList("Wages", mercs.Select(m => m.Wage).ToArray()),
                Vmad.ObjList("Details", details.ToArray()),
                Vmad.AliasList("Slots", qRoster, Enumerable.Range(FirstMercAlias, mercs.Length).ToArray()),
                Vmad.Obj("SSR_PowerRoster", power.FormKey),
                Vmad.Obj("SSR_MsgIntro", msgIntro), Vmad.Obj("SSR_MsgRoster", msgRoster), Vmad.Obj("SSR_MsgHiredMenu", msgHired),
                Vmad.Obj("SSR_MsgEmpty", msgEmpty), Vmad.Obj("SSR_MsgHaveFollower", msgHave), Vmad.Obj("SSR_MsgNoGold", msgNoGold),
                Vmad.Obj("DialogueFollower", DialogueFollower), Vmad.Obj("PlayerFollowerCount", PlayerFollowerCount),
                Vmad.Obj("PotentialFollowerFaction", PotentialFollowerFaction), Vmad.Obj("Gold001", Gold001), Vmad.Obj("MQ101", MQ101)))
            .WithAliasScripts(qRoster, AliasPlayer, Vmad.Script("SSR_PlayerAliasScript", Vmad.Obj("Roster", qRoster)));
        for (int i = 0; i < mercs.Length; i++)
            adapter = adapter.WithAliasScripts(qRoster, FirstMercAlias + i,
                Vmad.Script("SSR_MercAliasScript", Vmad.Obj("Roster", qRoster), Vmad.Int("Index", i)));
        quest.VirtualMachineAdapter = adapter;
    }

    // ----------------------------------------------------------------------------------------
    private T Add<T>(T record) where T : SkyrimMajorRecord
    {
        _mod.GetTopLevelGroup<T>().Add(record);
        return record;
    }

    private FormKey Global(string edid, float value)
    {
        var g = new GlobalFloat(Id(edid), R) { EditorID = edid, Data = value };
        _mod.Globals.Add(g);
        return g.FormKey;
    }

    private FormKey Effect(string edid, string name, ActorValue av, string description)
    {
        var m = new MagicEffect(Id(edid), R)
        {
            EditorID = edid,
            Name = name,
            Flags = MagicEffect.Flag.Recover | MagicEffect.Flag.NoDuration | MagicEffect.Flag.NoArea | MagicEffect.Flag.NoHitEvent,
            MagicSkill = ActorValue.None, ResistValue = ActorValue.None, SecondActorValue = ActorValue.None,
            Archetype = new MagicEffectArchetype { Type = MagicEffectArchetype.TypeEnum.ValueModifier, ActorValue = av },
            CastType = CastType.ConstantEffect,
            TargetType = TargetType.Self,
            CastingSoundLevel = SoundLevel.Normal,
            Description = description,
        };
        _mod.MagicEffects.Add(m);
        return m.FormKey;
    }

    private static Effect Fx(FormKey mgef, float magnitude, int duration) => new()
    {
        BaseEffect = new FormLinkNullable<IMagicEffectGetter>(mgef),
        Data = new EffectData { Magnitude = magnitude, Area = 0, Duration = duration },
    };

    private static MessageButton Btn(string text) => new() { Text = text };

    private FormKey Msg(string edid, string text, params MessageButton[] buttons)
    {
        var m = new Message(Id(edid), R) { EditorID = edid, Description = text, Flags = Message.Flag.MessageBox, INAM = new byte[4] };
        m.MenuButtons.AddRange(buttons);
        _mod.Messages.Add(m);
        return m.FormKey;
    }

    private FormKey Notice(string edid, string text)
    {
        var m = new Message(Id(edid), R) { EditorID = edid, Description = text, Flags = 0, DisplayTime = 2, INAM = new byte[4] };
        _mod.Messages.Add(m);
        return m.FormKey;
    }
}
