# Testing checklist (v0.1.0, early beta)

Automated (every `./build.sh`):

- [x] Papyrus compiles.
- [x] Plugin validation: ESL range, Skyrim.esm only, every form link resolves, script properties (including the string and int arrays and the alias array) match their declarations, SEQ contents, byte-exact round trip.
- [x] FOMOD schema.
- [x] Cross-mod check (`tools/modkit/Modkit.Compat`) with all ten mods of the series. Report: `tools/modkit/compat-report.md`.

In game (not done yet; please help). Item and actor names to look for: the power "Sellsword Roster", the sellswords by name (`help "Stone-Arm" 4` and so on). Plugin FormIDs load as FE xxx 8xx: the power is `SSR_PowerRoster` (83B) and the quest is `SSR_RosterQuest` (800).

1. **Start**
   - [ ] On a new game, after leaving Helgen (or within 30 seconds on an existing save), you learn "Sellsword Roster" and see the intro note.
2. **Roster**
   - [ ] The power lists exactly 3 names plus Close, and they match `SSR_Avail01`-`08` being 1 (`show SSR_Avail01` ...). **Pressing a name opens that same person's details** (this checks that button indices work with hidden buttons).
   - [ ] Back returns to the roster. Close closes it.
3. **Hiring**
   - [ ] With Lydia or another follower: "You already have a follower..." and nothing is charged.
   - [ ] Without enough gold: the no-gold notification shows the wage.
   - [ ] Hire: the wage is taken, the sellsword appears behind you, follows you, and has the vanilla follower dialogue (wait, trade, part ways). Note their voice type if they have no follower lines.
   - [ ] The trait shows in effect (e.g. Bram can carry a lot; Hroki's armor in the trade menu).
4. **Wages**
   - [ ] Wait 24 hours: "<Name> collects N gold in wages."
   - [ ] Drop your gold and wait 24 hours: "wasn't paid (N gold owed)". Another 24 hours: "leaves your service", they stop following, and they're gone from the roster for 14 days.
5. **Dismissal and rehire**
   - [ ] "It's time to part ways" through dialogue: they stop following. Walk away, then use the power: they're back on the roster. Hire them again: same face and gear.
   - [ ] The power's "Release them" does the same.
6. **Death**
   - [ ] `kill` the sellsword: "has fallen" notification. They never appear on the roster again.
7. **Rotation**
   - [ ] Wait 7 days, open the roster: a new set (from those alive, not hired, not on cooldown).
8. **Blackshield Company** (optional)
   - [ ] With the Blackshield sellsword following, hiring from the roster is refused. Dismiss them, then hire: it works.

When reporting a bug, include the sellsword's name, what you did, and the Papyrus log lines containing `SSR_` (enable Papyrus logging in `Skyrim.ini`: `[Papyrus] bEnableLogging=1`, `bEnableTrace=1`).
