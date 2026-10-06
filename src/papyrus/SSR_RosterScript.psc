Scriptname SSR_RosterScript extends Quest
{Sellsword Roster: eight named sellswords, a few of them looking for work each
week. Hire one through the lesser power; they become a follower through the
vanilla follower system (DialogueFollower), so the one-follower limit applies.
Each hired sellsword takes a daily wage every 24 game hours. Miss more than
SSR_GraceDays paydays in a row and they leave, and they won't work for you
again for SSR_LeaveCooldownDays.

The menu shows only the sellswords whose SSR_AvailNN global is 1 (button
conditions), so no SKSE is needed for a dynamic list.}

; ---------------------------------------------------------------------------
; Settings and records
; ---------------------------------------------------------------------------
GlobalVariable Property SSR_PowerGranted Auto
GlobalVariable Property SSR_RotationDays Auto
GlobalVariable Property SSR_RosterSize Auto
GlobalVariable Property SSR_WageMult Auto
{Percent applied to every wage.}
GlobalVariable Property SSR_GraceDays Auto
GlobalVariable Property SSR_LeaveCooldownDays Auto
GlobalVariable Property SSR_PollSeconds Auto
GlobalVariable Property SSR_HiredIndex Auto
{-1 or the roster index (0-7) of the sellsword in your service.}
GlobalVariable Property SSR_WagesPaid Auto
GlobalVariable[] Property Avail Auto

ActorBase[] Property Mercs Auto
String[] Property Names Auto
Int[] Property Wages Auto
Message[] Property Details Auto
ReferenceAlias[] Property Slots Auto

Spell Property SSR_PowerRoster Auto
Message Property SSR_MsgIntro Auto
Message Property SSR_MsgRoster Auto
Message Property SSR_MsgHiredMenu Auto
Message Property SSR_MsgEmpty Auto
Message Property SSR_MsgHaveFollower Auto
Message Property SSR_MsgNoGold Auto

Quest Property DialogueFollower Auto
GlobalVariable Property PlayerFollowerCount Auto
Faction Property PotentialFollowerFaction Auto
MiscObject Property Gold001 Auto
Quest Property MQ101 Auto

Int Property MERC_COUNT = 8 AutoReadOnly
Int Property BTN_CLOSE = 8 AutoReadOnly

; ---------------------------------------------------------------------------
; State
; ---------------------------------------------------------------------------
Int hired = -1
Float nextPay = 0.0
Int missed = 0
Int debt = 0
Float nextRotation = 0.0
Float[] cooldownUntil
Bool[] dead
Int leaving = -1

Event OnInit()
	cooldownUntil = new Float[8]
	dead = new Bool[8]
	SSR_HiredIndex.SetValue(-1.0)
	RegisterForSingleUpdate(10.0)
EndEvent

Function GameLoaded()
	RegisterForSingleUpdate(5.0)
EndFunction

Event OnUpdate()
	If SSR_PowerGranted.GetValueInt() == 0
		If CanGrant()
			GrantPower()
		Else
			RegisterForSingleUpdate(30.0)
			Return
		EndIf
	EndIf
	If hired >= 0
		CheckHired()
	EndIf
	If leaving >= 0
		CheckLeaving()
	EndIf
	If hired >= 0 || leaving >= 0
		Float poll = SSR_PollSeconds.GetValue()
		If poll < 1.0
			poll = 1.0
		EndIf
		RegisterForSingleUpdate(poll)
	EndIf
EndEvent

Bool Function CanGrant()
	If MQ101 && MQ101.IsRunning() && !MQ101.IsCompleted()
		Return False
	EndIf
	Actor player = Game.GetPlayer()
	Return !player.IsInCombat() && Game.IsMovementControlsEnabled() && Game.IsFightingControlsEnabled()
EndFunction

Function GrantPower()
	SSR_PowerGranted.SetValueInt(1)
	Game.GetPlayer().AddSpell(SSR_PowerRoster)
	Rotate(Utility.GetCurrentGameTime())
	SSR_MsgIntro.Show()
EndFunction

; ---------------------------------------------------------------------------
; The roster
; ---------------------------------------------------------------------------

Int Function WageOf(Int i)
	Int w = (Wages[i] * SSR_WageMult.GetValue() / 100.0) as Int
	If w < 1
		w = 1
	EndIf
	Return w
EndFunction

; Picks SSR_RosterSize sellswords who are alive, not hired and not sulking.
Function Rotate(Float afNow)
	Float days = SSR_RotationDays.GetValue()
	If days < 1.0
		days = 1.0
	EndIf
	nextRotation = afNow + days
	Int[] pool = new Int[8]
	Int n = 0
	Int i = 0
	While i < MERC_COUNT
		Avail[i].SetValueInt(0)
		If !dead[i] && i != hired && afNow >= cooldownUntil[i]
			pool[n] = i
			n += 1
		EndIf
		i += 1
	EndWhile
	Int want = SSR_RosterSize.GetValueInt()
	Int picked = 0
	While picked < want && n > 0
		Int k = Utility.RandomInt(0, n - 1)
		Avail[pool[k]].SetValueInt(1)
		pool[k] = pool[n - 1]
		n -= 1
		picked += 1
	EndWhile
EndFunction

Int Function AvailableCount()
	Int c = 0
	Int i = 0
	While i < MERC_COUNT
		If Avail[i].GetValueInt() != 0
			c += 1
		EndIf
		i += 1
	EndWhile
	Return c
EndFunction

; Called by the lesser power.
Function OpenRoster()
	Float now = Utility.GetCurrentGameTime()
	If now >= nextRotation
		Rotate(now)
	EndIf
	If hired >= 0
		Float hours = (nextPay - now) * 24.0
		If hours < 0.0
			hours = 0.0
		EndIf
		Int c = SSR_MsgHiredMenu.Show(hours)
		If c == 1
			Release(hired)
			Return
		ElseIf c != 0
			Return
		EndIf
	EndIf
	Float daysLeft = nextRotation - now
	If AvailableCount() == 0
		SSR_MsgEmpty.Show(daysLeft)
		Return
	EndIf
	Bool browsing = True
	While browsing
		Int choice = SSR_MsgRoster.Show(daysLeft)
		If choice < 0 || choice >= MERC_COUNT
			browsing = False
		ElseIf Details[choice].Show(WageOf(choice) as Float) == 0
			Hire(choice)
			browsing = False
		EndIf
	EndWhile
EndFunction

Function Hire(Int i)
	Actor player = Game.GetPlayer()
	If hired >= 0 || PlayerFollowerCount.GetValue() >= 1.0
		SSR_MsgHaveFollower.Show()
		Return
	EndIf
	Int wage = WageOf(i)
	If player.GetItemCount(Gold001) < wage
		SSR_MsgNoGold.Show(wage as Float)
		Return
	EndIf
	player.RemoveItem(Gold001, wage)
	SSR_WagesPaid.SetValue(SSR_WagesPaid.GetValue() + wage)

	Actor merc = Slots[i].GetActorReference()
	If merc && merc.IsDead()
		Return
	EndIf
	If !merc
		merc = player.PlaceActorAtMe(Mercs[i])
		Slots[i].ForceRefTo(merc)
	EndIf
	If leaving == i
		leaving = -1
	EndIf
	; A step behind the player.
	Float a = player.GetAngleZ()
	merc.MoveTo(player, -100.0 * Math.Sin(a), -100.0 * Math.Cos(a), 0.0)
	merc.Enable()
	merc.AddToFaction(PotentialFollowerFaction)
	merc.SetRelationshipRank(player, 3)
	(DialogueFollower as DialogueFollowerScript).SetFollower(merc)

	hired = i
	SSR_HiredIndex.SetValueInt(i)
	Avail[i].SetValueInt(0)
	nextPay = Utility.GetCurrentGameTime() + 1.0
	missed = 0
	debt = 0
	Debug.Notification(Names[i] + " signs on: " + wage + " gold a day.")
	Debug.Trace("[SellswordRoster] SSR_ hired " + i + " (" + Names[i] + ") wage " + wage)
	RegisterForSingleUpdate(2.0)
EndFunction

; ---------------------------------------------------------------------------
; While hired
; ---------------------------------------------------------------------------

Function CheckHired()
	Actor merc = Slots[hired].GetActorReference()
	If !merc || merc.IsDead()
		MercDied(hired)
		Return
	EndIf
	If !merc.IsPlayerTeammate()
		; Dismissed through dialogue, or left after waiting too long.
		Released(hired, False)
		Return
	EndIf
	Float now = Utility.GetCurrentGameTime()
	Int guard = 0
	While hired >= 0 && now >= nextPay && guard < 60
		PayDay()
		nextPay += 1.0
		guard += 1
	EndWhile
EndFunction

Function PayDay()
	Actor player = Game.GetPlayer()
	Int owed = WageOf(hired) + debt
	If player.GetItemCount(Gold001) >= owed
		player.RemoveItem(Gold001, owed, True)
		SSR_WagesPaid.SetValue(SSR_WagesPaid.GetValue() + owed)
		missed = 0
		debt = 0
		Debug.Notification(Names[hired] + " collects " + owed + " gold in wages.")
		Return
	EndIf
	missed += 1
	debt = owed
	If missed > SSR_GraceDays.GetValueInt()
		Debug.Notification(Names[hired] + " leaves your service. No pay, no sword.")
		Debug.Trace("[SellswordRoster] SSR_ " + Names[hired] + " left unpaid, owed " + owed)
		Int i = hired
		(DialogueFollower as DialogueFollowerScript).DismissFollower(0, 0)
		Released(i, True)
	Else
		Debug.Notification(Names[hired] + " wasn't paid (" + owed + " gold owed). Pay up by tomorrow or lose them.")
	EndIf
EndFunction

; From the hired menu: let them go cleanly.
Function Release(Int i)
	Actor merc = Slots[i].GetActorReference()
	If merc && merc.IsPlayerTeammate()
		(DialogueFollower as DialogueFollowerScript).DismissFollower(0, 1)
	EndIf
	Released(i, False)
EndFunction

Function Released(Int i, Bool abUnpaid)
	Actor merc = Slots[i].GetActorReference()
	If merc
		merc.RemoveFromFaction(PotentialFollowerFaction)
		merc.SetRelationshipRank(Game.GetPlayer(), 0)
	EndIf
	Float now = Utility.GetCurrentGameTime()
	If abUnpaid
		cooldownUntil[i] = now + SSR_LeaveCooldownDays.GetValue()
		Avail[i].SetValueInt(0)
	ElseIf !dead[i]
		Avail[i].SetValueInt(1) ; back on this week's roster
	EndIf
	hired = -1
	SSR_HiredIndex.SetValue(-1.0)
	missed = 0
	debt = 0
	leaving = i
EndFunction

; A released sellsword is disabled (kept, not deleted, so they look the same
; next time) once the player is out of sight.
Function CheckLeaving()
	Actor merc = Slots[leaving].GetActorReference()
	If !merc || merc.IsDead()
		leaving = -1
		Return
	EndIf
	If merc.IsPlayerTeammate()
		Return
	EndIf
	If !merc.Is3DLoaded() || merc.GetDistance(Game.GetPlayer()) > 3000.0
		merc.Disable()
		leaving = -1
	EndIf
EndFunction

Function MercDied(Int i)
	If dead[i]
		Return
	EndIf
	dead[i] = True
	Avail[i].SetValueInt(0)
	If hired == i
		hired = -1
		SSR_HiredIndex.SetValue(-1.0)
	EndIf
	If leaving == i
		leaving = -1
	EndIf
	Debug.Notification(Names[i] + " has fallen. The roster is one name shorter.")
	Debug.Trace("[SellswordRoster] SSR_ " + Names[i] + " died")
EndFunction
