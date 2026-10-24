# Nexus Mods page: Sellsword Roster

Early beta: v0.1.0 passes every automated build check but nobody has playtested it in game yet. The page says this at the top. The exact upload-form values are in [NEXUS_FIELDS.md](NEXUS_FIELDS.md).

## Page settings

- **Game:** Skyrim Special Edition
- **Mod name:** Sellsword Roster
- **Version:** 0.1.0 (early beta)
- **Category:** Followers & Companions
- **Suggested tags:** Followers, Companions, Gameplay, Economy, Lore-Friendly, ESL, No SKSE, Scripted
- **Requirements:** none.
- **Permissions (suggested):** the source is on GitHub under MIT, so allow modification and conversions with credit, but not re-uploads of the archive itself.
- **File to upload:** `SellswordRoster-0.1.0.zip` (FOMOD) as the main file, named "Sellsword Roster".

## Description (paste into the BBCode editor)

```bbcode
[center][size=6][b]SELLSWORD ROSTER[/b][/size]
[size=3][i]Steel for hire, by name. Coin up front.[/i][/size]

[size=3][color=#e0a030][b]EARLY BETA (v0.1.0)[/b]: this version passes every automated build check but hasn't been playtested in game yet. Please report bugs in the Bugs tab or on GitHub, ideally with your Papyrus log.[/color][/size][/center]

[line]

[size=4][b]Summary[/b][/size]
Eight named sellswords, each with a background, a trait and a daily wage. Every week a few of them are looking for work. Hire one with a lesser power and they follow you like any vanilla follower. Keep paying them, or they walk.

[size=4][b]The roster[/b][/size]
[list]
[*][b]Hroki Stone-Arm[/b]: Shield-Brother (+50 armor). 40 gold a day.
[*][b]Vala the Red[/b]: Bloodhound (+25% weapon damage). 50.
[*][b]Septimus Varo[/b]: Iron Lungs (+50 stamina, faster stamina regeneration). 35.
[*][b]Ilsa Brightwater[/b]: Field Medic (health regenerates twice as fast). 45.
[*][b]Durak gro-Bagol[/b]: Thick Hide (25% magic resistance). 60.
[*][b]Ash-Eye[/b]: Deadeye (+25 Archery). 45.
[*][b]Bram the Mule[/b]: Pack Mule (+150 carry weight). 30.
[*][b]Liv Quickstep[/b]: Fleet-Footed (moves 15% faster). 35.
[/list]
Looks, voices, levels and gear come from vanilla templates, so they fit right in.

[size=4][b]How it works[/b][/size]
[list]
[*]After Helgen you learn the lesser power [b]Sellsword Roster[/b]. It shows who's looking for work this week (3 of 8, changing every 7 days).
[*]Read their background and wage, then [b]Hire[/b]: pay the first day and they appear at your side.
[*]They use the [b]vanilla follower system[/b]: follow, wait, trade, favors, part ways.
[*][b]Wages[/b] are taken every 24 game hours. If you miss one, the debt carries over. Miss two in a row and they leave and refuse work for 14 days.
[*]Dismissed sellswords go back on the roster and look the same next time. [b]Death is permanent.[/b]
[/list]

[size=4][b]Requirements[/b][/size]
[list]
[*]Skyrim Special Edition or Anniversary Edition (1.5.97 or 1.6.x). Works on Steam under Proton on Linux.
[*]Nothing else. No SKSE, no SkyUI, no DLC, no Creation Club content.
[/list]

[size=4][b]Installation[/b][/size]
Install with [b]Mod Organizer 2[/b] or [b]Vortex[/b] (FOMOD installer) and enable [b]SellswordRoster.esp[/b]. It's ESL-flagged, so it doesn't take a full load-order slot. Safe to add mid-playthrough.

[size=4][b]Compatibility[/b][/size]
[list]
[*][b]No edits to vanilla records and nothing placed in the world.[/b] Skyrim.esm is the only master.
[*][b]One follower at a time[/b], as in vanilla: a roster sellsword takes the same slot as Lydia, a hireling or [b]Blackshield Company's sellsword[/b]. Part ways with your current follower first. (The mods share no records.)
[*]Multiple-follower mods: not tested yet.
[*]Compatible with the rest of my mods (Blackshield Company, Bounties of Oblivion, Contract Hunter, Steel Oath, Ember Smithing, The Gray-Mane Debt, Blackshield Codex, War Banners, Roadwarden), checked by loading all ten plugins together.
[/list]

[size=4][b]Settings (console)[/b][/size]
[code]set SSR_WageMult to 100           (percent applied to every wage)
set SSR_RosterSize to 3 / set SSR_RotationDays to 7
set SSR_GraceDays to 1 / set SSR_LeaveCooldownDays to 14[/code]

[size=4][b]Uninstalling[/b][/size]
Part ways with your sellsword first. As with any scripted mod, prefer not to remove it mid-playthrough.

[size=4][b]Known issues / early beta limits[/b][/size]
[list]
[*]No new dialogue: they use vanilla follower lines for their voice type. If one has no follower lines, please tell me which.
[*]Wages are counted from the hour you hired them.
[*]A dismissed sellsword disappears once you've walked away, rather than going to an inn.
[/list]

[size=4][b]Source[/b][/size]
Open source (MIT): [url=https://github.com/Blackshield-Company/sellsword-roster]github.com/Blackshield-Company/sellsword-roster[/url]

[size=4][b]Credits[/b][/size]
[list]
[*][url=https://github.com/Mutagen-Modding/Mutagen]Mutagen[/url] by Noggog and contributors, which generates the plugin from code.
[*][url=https://github.com/russo-2025/papyrus-compiler]papyrus-compiler[/url] by russo-2025, the open-source Papyrus compiler.
[*][url=https://github.com/GandaG/fomod-schema]FOMOD schema[/url] by GandaG.
[*]Bethesda Game Studios for Skyrim.
[/list]
```
