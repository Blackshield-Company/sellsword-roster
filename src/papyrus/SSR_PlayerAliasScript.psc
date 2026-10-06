Scriptname SSR_PlayerAliasScript extends ReferenceAlias
{Sellsword Roster: wakes the roster up after a save is loaded.}

SSR_RosterScript Property Roster Auto

Event OnPlayerLoadGame()
	Roster.GameLoaded()
EndEvent
