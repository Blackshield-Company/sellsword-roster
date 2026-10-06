# Compatibility report

10 plugins loaded together with Mutagen 0.54.4.0.

## Load order (simulated)

| # | Plugin | ESL | Masters | New records | Overrides | Runtime FormIDs |
|---|---|---|---|---|---|---|
| 1 | BlackshieldCompany.esp | yes | Skyrim.esm | 53 | 0 | FE000800-FE000834 |
| 2 | BountiesOfOblivion.esp | yes | Skyrim.esm | 81 | 0 | FE001800-FE001850 |
| 3 | ContractHunter.esp | yes | Skyrim.esm | 86 | 0 | FE002800-FE002855 |
| 4 | SteelOath.esp | yes | Skyrim.esm | 56 | 0 | FE003800-FE003837 |
| 5 | EmberSmithing.esp | yes | Skyrim.esm | 537 | 0 | FE004800-FE004A18 |
| 6 | GrayManeDebt.esp | yes | Skyrim.esm | 53 | 0 | FE005800-FE005834 |
| 7 | BlackshieldCodex.esp | yes | Skyrim.esm, BlackshieldCompany.esp, BountiesOfOblivion.esp, ContractHunter.esp, SteelOath.esp, EmberSmithing.esp | 45 | 0 | FE006800-FE00682C |
| 8 | WarBanners.esp | yes | Skyrim.esm | 32 | 0 | FE007800-FE00781F |
| 9 | Roadwarden.esp | yes | Skyrim.esm | 52 | 0 | FE008800-FE008833 |
| 10 | SellswordRoster.esp | yes | Skyrim.esm | 60 | 0 | FE009800-FE00983B |

1055 new records; every simulated runtime FormID is unique.

## Overrides of existing records

None. No plugin in the set overrides a vanilla record or another plugin's record.

## EditorIDs and prefixes

| Plugin | Prefix | EditorIDs | Off-prefix |
|---|---|---|---|
| BlackshieldCompany.esp | `BSC_` | 53 | 0 |
| BountiesOfOblivion.esp | `BOO_` | 81 | 0 |
| ContractHunter.esp | `CHT_` | 86 | 0 |
| SteelOath.esp | `SOT_` | 56 | 0 |
| EmberSmithing.esp | `EMB_` | 537 | 0 |
| GrayManeDebt.esp | `GMD_` | 53 | 0 |
| BlackshieldCodex.esp | `BCX_` | 45 | 0 |
| WarBanners.esp | `WBN_` | 32 | 0 |
| Roadwarden.esp | `RWD_` | 52 | 0 |
| SellswordRoster.esp | `SSR_` | 60 | 0 |

1055 EditorIDs across the set; collisions: 0.

## Globals

| Plugin | Globals |
|---|---|
| BlackshieldCompany.esp | 9 |
| BountiesOfOblivion.esp | 16 |
| ContractHunter.esp | 21 |
| SteelOath.esp | 20 |
| EmberSmithing.esp | 0 |
| GrayManeDebt.esp | 5 |
| BlackshieldCodex.esp | 9 |
| WarBanners.esp | 9 |
| Roadwarden.esp | 26 |
| SellswordRoster.esp | 17 |

132 globals in total, no two with the same EditorID.

## Papyrus scripts

| Plugin | Scripts (attached + shipped .pex) |
|---|---|
| BlackshieldCompany.esp | 5 |
| BountiesOfOblivion.esp | 5 |
| ContractHunter.esp | 5 |
| SteelOath.esp | 3 |
| EmberSmithing.esp | 0 |
| GrayManeDebt.esp | 13 |
| BlackshieldCodex.esp | 2 |
| WarBanners.esp | 4 |
| Roadwarden.esp | 2 |
| SellswordRoster.esp | 4 |

43 distinct script names; duplicates: 0.

## Links between plugins in the set

- BlackshieldCodex.esp -> EmberSmithing.esp: MagicEffect EMB_EmberEngravingEffect
- BlackshieldCodex.esp -> EmberSmithing.esp: MagicEffect EMB_HearthEngravingEffect
- BlackshieldCodex.esp -> EmberSmithing.esp: MagicEffect EMB_BellowsEngravingEffect
- BlackshieldCodex.esp -> BlackshieldCompany.esp: ObjectEffect BSC_EnchBladeEmber
- BlackshieldCodex.esp -> BlackshieldCompany.esp: Weapon BSC_BlackshieldBlade
- BlackshieldCodex.esp -> BlackshieldCompany.esp: Armor BSC_BlackshieldShield
- BlackshieldCodex.esp -> BlackshieldCompany.esp: Armor BSC_BlackshieldPlate
- BlackshieldCodex.esp -> BlackshieldCompany.esp: Quest BSC_CompanyQuest
- BlackshieldCodex.esp -> BlackshieldCompany.esp: GlobalFloat BSC_ContractActive
- BlackshieldCodex.esp -> BlackshieldCompany.esp: GlobalFloat BSC_ContractsDone
- BlackshieldCodex.esp -> SteelOath.esp: GlobalFloat SOT_ActiveSteel
- BlackshieldCodex.esp -> SteelOath.esp: GlobalFloat SOT_ActiveShield
- BlackshieldCodex.esp -> SteelOath.esp: GlobalFloat SOT_ActiveIron
- BlackshieldCodex.esp -> SteelOath.esp: GlobalFloat SOT_ActiveEndurance
- BlackshieldCodex.esp -> SteelOath.esp: GlobalFloat SOT_ActiveClose
- BlackshieldCodex.esp -> ContractHunter.esp: Quest CHT_HunterQuest
- BlackshieldCodex.esp -> ContractHunter.esp: GlobalFloat CHT_BountiesClaimed
- BlackshieldCodex.esp -> ContractHunter.esp: GlobalFloat CHT_PayMult
- BlackshieldCodex.esp -> ContractHunter.esp: GlobalFloat CHT_OpenNotice
- BlackshieldCodex.esp -> ContractHunter.esp: GlobalFloat CHT_OpenWanted
- BlackshieldCodex.esp -> ContractHunter.esp: GlobalFloat CHT_OpenDread
- BlackshieldCodex.esp -> BountiesOfOblivion.esp: GlobalFloat BOO_FavorDagon
- BlackshieldCodex.esp -> BountiesOfOblivion.esp: GlobalFloat BOO_FavorBoethiah
- BlackshieldCodex.esp -> BountiesOfOblivion.esp: GlobalFloat BOO_FavorNamira
- BlackshieldCodex.esp -> BountiesOfOblivion.esp: GlobalFloat BOO_FavorMolagBal
- BlackshieldCodex.esp -> BountiesOfOblivion.esp: GlobalFloat BOO_FavorClavicus
- BlackshieldCodex.esp -> BountiesOfOblivion.esp: GlobalFloat BOO_FavorHircine
- BlackshieldCodex.esp -> BountiesOfOblivion.esp: GlobalFloat BOO_FavorSanguine
- BlackshieldCodex.esp -> BountiesOfOblivion.esp: GlobalFloat BOO_FavorVaermina

## Installed files

85 distinct installed paths across 10 archives; collisions: 0.

## Result

**PASS**: no errors.

## Archives checked

Produced by `tools/modkit/compat-series.sh --local dist/SellswordRoster-0.1.0.zip --report tools/modkit/compat-report.md` on 2026-10-06. The first nine archives were downloaded from their GitHub releases (v0.1.0). The tenth is this repository's v0.1.0 build.

| Archive | sha256 |
|---|---|
| BlackshieldCompany-0.1.0.zip | 820b57753c71347a45bbddf6ca4d1d22457dccfbb44a8fcb0aa821a6803a3b8c |
| BountiesOfOblivion-0.1.0.zip | d99bb3455ec41ebf7cc6722262a752a83b5dcc00ab22cb9182efbb5de316caeb |
| ContractHunter-0.1.0.zip | e96fde2641e2fa27ee9a767b687033ac346475a19568b2941af9704971655ffe |
| SteelOath-0.1.0.zip | 6764aefc8beb8aaa0a14378f50bf092908b8e6e0f8ea16c51153b50379208ef8 |
| EmberSmithing-0.1.0.zip | d3fb31257e796478c2bada653df83df2e90ff81476025c3a34b0ecc2fe7f9e96 |
| GrayManeDebt-0.1.0.zip | 69ea29dbfd1dc9dae8093684768663b1d17dba1a47a8f83162015a8eefa5e72a |
| BlackshieldCodex-0.1.0.zip | e79ec7aae4cfea450bc22aae69e75acb836bf7d8d9cff3353ae805783b2a55f7 |
| WarBanners-0.1.0.zip | 5151dab8dc2bf8a8bae48c3b23a2e133a7effd2b2679b24e5cb0eba01a640660 |
| Roadwarden-0.1.0.zip | 7e8435325467fa18b85188858bb8d7bb3eb43aba8577ddb9485b0c78639d25a1 |
| SellswordRoster-0.1.0.zip | see the v0.1.0 release (the record check above doesn't depend on archive timestamps) |

## Runtime interactions (not visible to a record check; reviewed by hand)

| Shared thing | Mods | How it's handled |
|---|---|---|
| Vanilla follower slot (`DialogueFollower`, `PlayerFollowerCount`) | Blackshield Company (company sellsword), Sellsword Roster | Both check `PlayerFollowerCount` before `SetFollower`, so only one follower at a time, as in vanilla. Neither edits the follower quest. |
| Outdoor hostile spawns | Contract Hunter (avengers), Roadwarden (encounters) | Roadwarden waits 2 minutes after any combat and spawns 10-50 m behind the player. Contract Hunter holds avengers while the player is in combat. They can still overlap if an avenger tick lands while you walk toward a non-hostile Roadwarden encounter. |
| Player teammates | War Banners (rally aliases), Blackshield Company, Sellsword Roster | War Banners only reads teammates (Find Matching Reference, `GetPlayerTeammate`) and casts a short buff, so roster and company sellswords are rallied like any follower. |
| Gold | Blackshield Company (treasury), Contract Hunter (pay), Roadwarden (rewards, tolls), Sellsword Roster (wages) | Plain `AddItem` / `RemoveItem` of `Gold001`. No shared state. |
| Lesser powers | War Banners, Sellsword Roster, Contract Hunter (ledger), and others | Separate spells; no shared records. |
| Cross-plugin records | Blackshield Codex -> five first-wave plugins | The only plugin with masters beyond Skyrim.esm. All 29 links resolve (listed above). |
