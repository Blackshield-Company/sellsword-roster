Scriptname SSR_PowerEffectScript extends ActiveMagicEffect
{Sellsword Roster: the lesser power opens the roster.}

SSR_RosterScript Property Roster Auto

Event OnEffectStart(Actor akTarget, Actor akCaster)
	Roster.OpenRoster()
EndEvent
