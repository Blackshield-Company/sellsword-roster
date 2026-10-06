Scriptname SSR_MercAliasScript extends ReferenceAlias
{Sellsword Roster: one alias per sellsword; keeps the reference persistent and reports a death.}

SSR_RosterScript Property Roster Auto
Int Property Index Auto

Event OnDeath(Actor akKiller)
	Roster.MercDied(Index)
EndEvent
