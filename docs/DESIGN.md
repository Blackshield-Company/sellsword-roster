# Design notes (v0.1, early beta)

## Records (60, FormIDs 0x800-0x83B, ESL-flagged, Skyrim.esm only, no overrides)

| EditorID | Type | Purpose |
|---|---|---|
| `SSR_RosterQuest` | QUST (Start Game Enabled, Run Once) | Controller (`SSR_RosterScript`). Alias 0 = player (`SSR_PlayerAliasScript`, wakes the controller on load). Aliases 1-8 = one optional, empty alias per sellsword (`SSR_MercAliasScript` with `Index` 0-7). Filling an alias makes the sellsword's reference persistent and reports a death. In `SEQ/SellswordRoster.seq`. |
| `SSR_Merc<Name>` (8) | NPC_ | Template: a vanilla leveled bandit character (traits, stats, model/animation, inventory, attack data, keywords). Own name, short name, factions (`SSR_SellswordFaction`, `PotentialFollowerFaction`), AI data (unaggressive, brave, helps friends and allies, no crime), spell list (the trait), no packages. Unique. |
| `SSR_Trait<Name>` (8) | SPEL (ability, constant effect) | The trait. |
| `SSR_Eff<Name>[2]` (9) | MGEF (value modifier, constant, recover) | DamageResist 50, AttackDamageMult 0.25, Stamina 50 + StaminaRateMult 50, HealRateMult 100, ResistMagic 25, Archery 25, CarryWeight 150, SpeedMult 15. |
| `SSR_PowerRoster`, `SSR_PowerEffect` | SPEL (lesser power) + MGEF (script) | "Sellsword Roster". |
| `SSR_MsgRoster` | MESG (10 buttons max: 8 names + Close) | Each name button has the condition `GetGlobalValue SSR_AvailNN == 1`, so only this week's sellswords show. The script gets the button's index in the full list back. |
| `SSR_MsgDetail<Name>` (8) | MESG | Background, trait, wage (`%.0f`), Hire / Back. |
| `SSR_MsgIntro`, `SSR_MsgHiredMenu`, `SSR_MsgEmpty`, `SSR_MsgHaveFollower`, `SSR_MsgNoGold` | MESG | |
| `SSR_SellswordFaction` | FACT | Hidden, no relations. |
| `SSR_*` settings, `SSR_Avail01-08` | GLOB (17) | |

Vanilla records used (none edited): `DialogueFollower` (0750BA, `DialogueFollowerScript.SetFollower` / `DismissFollower`), `PlayerFollowerCount` (0BCC98), `PotentialFollowerFaction` (05C84D), `Gold001`, `MQ101`, and the eight `LCharBandit*` templates.

## Names without SKSE

`Form.GetName()` needs SKSE, so the script has a `String[] Names` property (VMAD string array, same order as the roster) and builds notifications with `Debug.Notification`. Wages are an `Int[]` property. Modkit 10 adds `Vmad.IntList` / `StrList` / `Str` and validates array kinds.

## Hiring

1. Refuse if a sellsword is already hired or `PlayerFollowerCount >= 1` (Lydia, a hireling, the Blackshield sellsword, etc.). `SetFollower` would otherwise overwrite the vanilla follower alias and strand the old follower as a teammate.
2. Take the first day's wage.
3. First hire: `PlaceActorAtMe` and `ForceRefTo` the sellsword's alias. After that the same reference is moved, enabled and reused, so the face and gear (rolled from the leveled template the first time) stay the same.
4. `AddToFaction(PotentialFollowerFaction)`, `SetRelationshipRank(player, 3)`, `DialogueFollowerScript.SetFollower`.

## While hired (polled every `SSR_PollSeconds`)

- Dead: the roster name is retired (also caught by the alias `OnDeath`).
- No longer a player teammate: dismissed through vanilla dialogue, or the vanilla "waited three days" timeout. They return to this week's roster.
- Payday (`GetCurrentGameTime() >= nextPay`; skipped days are processed one by one, up to 60): pay wage plus debt, or add to the debt. If `missed > SSR_GraceDays`, call `DismissFollower(0, 0)`, put them on cooldown for `SSR_LeaveCooldownDays`, and take them off the roster.
- On release, `RemoveFromFaction(PotentialFollowerFaction)` and relationship rank 0, so the vanilla "Follow me" line can't re-hire them for free. Once they're out of 3D or more than 3000 units away, they're disabled (not deleted).

## Rotation

When the power is used and `nextRotation` has passed, `SSR_RosterSize` sellswords are picked at random from those who are alive, not hired and not on cooldown, and their `SSR_AvailNN` globals are set to 1.

## Compatibility notes

- Blackshield Company's sellsword (`BSC_Sellsword`) also uses `DialogueFollower.SetFollower` and also checks `PlayerFollowerCount`. Whichever is hired first takes the slot. The two mods share no records.
- No vanilla follower records are edited, so follower frameworks that hook `DialogueFollower` should see the roster sellswords as ordinary followers. This hasn't been tested.
