#Region "Imports"
Imports L = BalatroSaveEditor.Lua
Imports System
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Text
Imports System.Windows.Forms
#End Region
Public Class frmBalatroSaveEditor
#Region "Properties"
	Private Property SaveFile As L.SaveFile
	Private Property LoadedProfile As Integer = -1
#End Region
#Region "Constructors/Destructors"
	Private Sub frmBalatroSaveEditor_Load(sender As Object, e As EventArgs) Handles MyBase.Load
		'OpenDefaultSaveFile()
		Init()
	End Sub
	Private Sub frmBalatroSaveEditor_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
	End Sub
	Private Sub frmBalatroSaveEditor_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
	End Sub
#End Region
#Region "Methods"
	Private Sub Init()
		SetNoActiveSave()
		cboProfile.Items.Clear()
		cboProfile.SelectedItem = Nothing
		Dim SelectedProfile As Integer = -1
		Try
			Dim SettingsFile As New L.SettingsFile(SettingsPath)
			If SettingsFile.ValProfile.HasValue Then SelectedProfile = SettingsFile.ValProfile.Value
		Catch ex As Exception
		End Try
		Dim NumProfileSaves As Integer = 0
		Dim ItemToSelect As ComboBoxItem = Nothing
		For i As Integer = 0 To 2
			Try
				Dim ProfileFile As New L.ProfileFile(ProfilePath(i))
				If Not File.Exists(SavePath(i)) Then Continue For
				Dim NewCbi As New ComboBoxItem((i + 1).ToString() & ": " & ProfileFile.ValName, i)
				cboProfile.Items.Add(NewCbi)
				NumProfileSaves += 1
				If SelectedProfile = (i + 1) Then ItemToSelect = NewCbi
			Catch ex As Exception
			End Try
		Next
		Application.DoEvents()
		If ItemToSelect IsNot Nothing Then cboProfile.SelectedItem = ItemToSelect
	End Sub
	Public Sub OpenProfileSave(ByVal ProfileIndex As Integer)
		Try
			Me.SaveFile = New L.SaveFile(TestFilePath(ProfileIndex))
			ReadValuesToControls()
			LoadedProfile = ProfileIndex
		Catch ex As Exception
			SetNoActiveSave()
		End Try
	End Sub
	Public Sub SaveProfileSave()
		Try
			Me.SaveFile.BackupFile()
			WriteControlsToValues()
			Me.SaveFile.SaveFile()
		Catch ex As Exception
			SetNoActiveSave()
		End Try
	End Sub
	'Public Sub OpenDefaultSaveFile()
	'	Me.SaveFile = New L.SaveFile(TestFilePath(2))
	'	ReadValuesToControls()
	'End Sub
	'Public Sub SaveDefaultSaveFile()
	'	Me.SaveFile.BackupFile()
	'	WriteControlsToValues()
	'	Me.SaveFile.SaveFile()
	'End Sub
	Public Sub ReadValuesToControls()
		With Me.SaveFile
			nudAnte.Value = .ValWinAnte
			nudChips.Value = .ValChips
			nudConsumableLimit.Value = .ValConsumableLimit
			nudDiscards.Value = .ValDiscards
			nudDiscountPct.Value = .ValDiscountPct
			nudDollars.Value = .ValDollars
			nudEditionRate.Value = .ValEditionRate
			nudHands.Value = .ValHands
			nudHandSize.Value = .ValHandSize
			nudInflation.Value = .ValInflation
			nudInterestAmount.Value = .ValInterestAmount
			nudInterestCap.Value = .ValInterestCap
			nudJokerLimit.Value = .ValJokerLimit
			nudJokerRate.Value = .ValJokerRate
			nudPackSize.Value = .ValPackSize
			nudPlanetRate.Value = .ValPlanetRate
			nudPlayingCardRate.Value = .ValPlayingCardRate
			nudRerollCost.Value = .ValRerollCost
			nudRound.Value = .ValRound
			nudShopJokers.Value = .ValShopJokers
			nudSpectralRate.Value = .ValSpectralRate
			nudTarotRate.Value = .ValTarotRate
			nudWinAnte.Value = .ValWinAnte
		End With
	End Sub
	Public Sub WriteControlsToValues()
		With Me.SaveFile
			.ValWinAnte = nudAnte.Value
			.ValChips = nudChips.Value
			.ValConsumableLimit = nudConsumableLimit.Value
			.ValDiscards = nudDiscards.Value
			.ValDiscountPct = nudDiscountPct.Value
			.ValDollars = nudDollars.Value
			.ValEditionRate = nudEditionRate.Value
			.ValHands = nudHands.Value
			.ValHandSize = nudHandSize.Value
			.ValInflation = nudInflation.Value
			.ValInterestAmount = nudInterestAmount.Value
			.ValInterestCap = nudInterestCap.Value
			.ValJokerLimit = nudJokerLimit.Value
			.ValJokerRate = nudJokerRate.Value
			.ValPackSize = nudPackSize.Value
			.ValPlanetRate = nudPlanetRate.Value
			.ValPlayingCardRate = nudPlayingCardRate.Value
			.ValRerollCost = nudRerollCost.Value
			.ValRound = nudRound.Value
			.ValShopJokers = nudShopJokers.Value
			.ValSpectralRate = nudSpectralRate.Value
			.ValTarotRate = nudTarotRate.Value
			.ValWinAnte = nudWinAnte.Value
		End With
	End Sub
	''' <summary></summary>
	Public Sub SetNoActiveSave()
		LoadedProfile = -1
		Me.SaveFile = Nothing
	End Sub
#End Region
#Region "Event Handlers"
	Private Sub btnSave_Click(sender As System.Object, e As System.EventArgs) Handles btnSave.Click
		'SaveDefaultSaveFile()
		SaveProfileSave()
	End Sub
	Private Sub btnReload_Click(sender As System.Object, e As System.EventArgs) Handles btnReload.Click
		If LoadedProfile <> -1 Then OpenProfileSave(LoadedProfile)
	End Sub
	Private Sub btnGetSettingsLua_Click(sender As System.Object, e As System.EventArgs) Handles btnGetSettingsLua.Click
		Try
			Dim SettingsFile As New L.SettingsFile(SettingsPath)
			Clipboard.SetText(SettingsFile.DecompressedText)
			System.Media.SystemSounds.Asterisk.Play()
		Catch ex As Exception
			MsgBox(ex.Message)
		End Try
	End Sub
	Private Sub btnGetProfileLua_Click(sender As System.Object, e As System.EventArgs) Handles btnGetProfileLua.Click
		Try
			Dim ProfileFile As New L.ProfileFile(ProfilePath(2))
			Clipboard.SetText(ProfileFile.DecompressedText)
			System.Media.SystemSounds.Asterisk.Play()
		Catch ex As Exception
			MsgBox(ex.Message)
		End Try
	End Sub
	Private Sub btnGetMetaLua_Click(sender As System.Object, e As System.EventArgs) Handles btnGetMetaLua.Click
		Try
			Dim MetaFile As New L.MetaFile(MetaPath(2))
			Clipboard.SetText(MetaFile.DecompressedText)
			System.Media.SystemSounds.Asterisk.Play()
		Catch ex As Exception
			MsgBox(ex.Message)
		End Try
	End Sub
	Private Sub btnGetSaveLua_Click(sender As System.Object, e As System.EventArgs) Handles btnGetSaveLua.Click
		Try
			Dim SaveFile As New L.SaveFile(SavePath(2))
			Clipboard.SetText(SaveFile.DecompressedText)
			System.Media.SystemSounds.Asterisk.Play()
		Catch ex As Exception
			MsgBox(ex.Message)
		End Try
	End Sub
	Private Sub cboProfile_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cboProfile.SelectedIndexChanged
		Try
			Dim cbitem As ComboBoxItem = cboProfile.SelectedItem
			If cbitem IsNot Nothing Then OpenProfileSave(cbitem.Tag)
		Catch ex As Exception
			SetNoActiveSave()
		End Try
	End Sub
#End Region
End Class
