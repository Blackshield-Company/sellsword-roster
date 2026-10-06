# Changelog

All notable changes to this project are documented here. Versions follow [Semantic Versioning](https://semver.org/).

## [0.1.0] - 2026-10-06

Early beta. First public release. It passes every automated check in the build (Papyrus compile, plugin validation, round trip, SEQ check, FOMOD schema, cross-mod compatibility check) but has not been playtested in game yet. Please report problems on the Nexus page or on GitHub.

### Added
- Eight named sellswords, each with a background, a trait (a constant ability) and a daily wage of 30 to 60 gold. Their looks, voices, stats and gear come from vanilla leveled bandit templates.
- The lesser power "Sellsword Roster", granted after the opening. It lists who is looking for work this week (3 of the 8, changing every 7 days) and hires one.
- Hired sellswords use the vanilla follower system: follow, wait, trade, favors, and dismissal through dialogue. The vanilla one-follower limit applies.
- Wages: the first day is paid when you hire. After that the wage is taken every 24 game hours. If you miss a payday, the debt carries over. Miss two in a row and the sellsword leaves and refuses work for 14 days.
- Dismissed sellswords go back on this week's roster and keep their looks next time. A sellsword who dies is gone for good.
- Settings as console globals.
- `SEQ/SellswordRoster.seq` for the Start Game Enabled controller quest.
- FOMOD installer with optional Papyrus sources.
