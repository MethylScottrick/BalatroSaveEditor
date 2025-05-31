#Region "Imports"
Imports LsonLib
Imports System
Imports System.IO
Imports System.IO.Compression
Imports System.Runtime
Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Json
Imports System.Text
Imports System.Text.RegularExpressions
#End Region
Public Module Globals
#Region "Constants"
	Public Const SettingsFilename As String = "settings.jkr"
	Public Const ProfileFilename As String = "profile.jkr"
	Public Const SaveFilename As String = "save.jkr"
	Public Const MetaFilename As String = "meta.jkr"
	'Public Const Filename As String = ".jkr"
	'Public Const TestFilename As String = "settings.jkr"
	'Public Const TestFilePath2 As String = "C:\Users\Methy\Documents\Visual Studio 2010\Projects\FactorioHelper\Assets\testdata.lua"
	'Public Const TestFilePath3 As String = "C:\Users\Methy\Documents\Visual Studio 2010\Projects\BalatroSaveEditor\testdata2.lua"
	Public Const TestFilename As String = "save.jkr"
	Public Const TestFilename2 As String = "meta.jkr"
	Public Const TestFilename3 As String = "profile.jkr"
	Public Const CommaOnLastMember As Boolean = True
	Public Const WriteArrayKeys As Boolean = True
	Public Const DebugWriteDecompressedFile As Boolean = True
#End Region
#Region "Properties"
	Public ReadOnly Property BalatroDataDir As String
		Get
			Dim ExpectedDir As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData, Environment.SpecialFolderOption.None), "Balatro")
			If Directory.Exists(ExpectedDir) Then
				Return ExpectedDir
			Else
				Throw New DirectoryNotFoundException()
			End If
		End Get
	End Property
	Public ReadOnly Property BalatroProfileDir(ByVal ProfileIndex As Integer) As String
		Get
			Dim ExpectedDir As String = Path.Combine(BalatroDataDir, (ProfileIndex + 1).ToString())
			If Directory.Exists(ExpectedDir) Then
				Return ExpectedDir
			Else
				Throw New DirectoryNotFoundException()
			End If
		End Get
	End Property
	Public ReadOnly Property SettingsPath As String
		Get
			Try
				Dim ExpectedPath As String = Path.Combine(BalatroDataDir, SettingsFilename)
				If File.Exists(ExpectedPath) Then
					Return ExpectedPath
				Else
					Throw New FileNotFoundException()
				End If
			Catch ex As Exception
				Return Nothing
			End Try
		End Get
	End Property
	Public ReadOnly Property ProfilePath(ByVal ProfileIndex As Integer) As String
		Get
			Try
				Dim ExpectedPath As String = Path.Combine(BalatroProfileDir(ProfileIndex), ProfileFilename)
				If File.Exists(ExpectedPath) Then
					Return ExpectedPath
				Else
					Throw New FileNotFoundException()
				End If
			Catch ex As Exception
				Return Nothing
			End Try
		End Get
	End Property
	Public ReadOnly Property SavePath(ByVal ProfileIndex As Integer) As String
		Get
			Try
				Dim ExpectedPath As String = Path.Combine(BalatroProfileDir(ProfileIndex), SaveFilename)
				If File.Exists(ExpectedPath) Then
					Return ExpectedPath
				Else
					Throw New FileNotFoundException()
				End If
			Catch ex As Exception
				Return Nothing
			End Try
		End Get
	End Property
	Public ReadOnly Property MetaPath(ByVal ProfileIndex As Integer) As String
		Get
			Try
				Dim ExpectedPath As String = Path.Combine(BalatroProfileDir(ProfileIndex), MetaFilename)
				If File.Exists(ExpectedPath) Then
					Return ExpectedPath
				Else
					Throw New FileNotFoundException()
				End If
			Catch ex As Exception
				Return Nothing
			End Try
		End Get
	End Property
	Public ReadOnly Property TestFilePath As String
		Get
			Dim ExpectedPath As String = Path.Combine(BalatroDataDir, TestFilename)
			If File.Exists(ExpectedPath) Then
				Return ExpectedPath
			Else
				Throw New FileNotFoundException()
			End If
		End Get
	End Property
	Public ReadOnly Property TestFilePath(ByVal ProfileIndex As Integer) As String
		Get
			Dim ExpectedPath As String = Path.Combine(BalatroProfileDir(ProfileIndex), TestFilename)
			If File.Exists(ExpectedPath) Then
				Return ExpectedPath
			Else
				Throw New FileNotFoundException()
			End If
		End Get
	End Property
	Public ReadOnly Property TestFilePath2(ByVal ProfileIndex As Integer) As String
		Get
			Dim ExpectedPath As String = Path.Combine(BalatroProfileDir(ProfileIndex), TestFilename2)
			If File.Exists(ExpectedPath) Then
				Return ExpectedPath
			Else
				Throw New FileNotFoundException()
			End If
		End Get
	End Property
	Public ReadOnly Property TestFilePath3(ByVal ProfileIndex As Integer) As String
		Get
			Dim ExpectedPath As String = Path.Combine(BalatroProfileDir(ProfileIndex), TestFilename3)
			If File.Exists(ExpectedPath) Then
				Return ExpectedPath
			Else
				Throw New FileNotFoundException()
			End If
		End Get
	End Property
#End Region
#Region "Methods"
	<Extension()> Public Function ToLuaCode(ByVal Value As LsonValue) As String
		Return "return " & LsonValueToLuaCodeText(Value)
	End Function
	Private Function LsonValueToLuaCodeText(ByVal Value As LsonValue) As String
		If Value Is Nothing Then Return "nil"
		Select Case Value.GetType()
			Case GetType(LsonBool)
				If Value.GetBoolLenient() Then
					Return "true"
				Else
					Return "false"
				End If
			Case GetType(LsonDict)
				If IsLsonDictAnArray(Value) Then
					Return LsonArrayToLuaCodeText(Value)
				Else
					Return LsonDictToLuaCodeText(Value)
				End If
				Return LsonDictToLuaCodeText(Value)
			Case GetType(LsonNoValue)
				' not sure what to do here yet, so break
				Throw New Exception()
			Case GetType(LsonNumber)
				If IsLsonNumberFloating(Value) Then
					Return Value.GetDoubleLenient().ToString().ToLower() ' lower so exponent is "e", not "E"
				Else
					Try
						Return Value.GetLongLenient().ToString()
					Catch ex As Exception
						Return Value.GetDoubleLenient().ToString().ToLower() ' lower so exponent is "e", not "E"
					End Try
				End If
			Case GetType(LsonString)
				Return """" & Value.GetStringLenient().Replace("""", "\""") & """"
			Case GetType(LsonValue)
				' not sure what to do here yet, so break
				Throw New Exception()
			Case Else
				Return String.Empty
		End Select
	End Function
	Private Function LsonDictToLuaCodeText(ByVal Dict As LsonDict) As String
		Dim sb As New StringBuilder()
		sb.Append("{")
		For i As Integer = 0 To Dict.Keys.Count - 1
			Dim k As LsonValue = Dict.Keys(i)
			If k.GetType() Is GetType(LsonDict) Then Throw New Exception("Lua dictionary keys cannot be dictionaries(?)")
			sb.Append("[")
			sb.Append(LsonValueToLuaCodeText(k))
			sb.Append("]=")
			sb.Append(LsonValueToLuaCodeText(Dict.Values(i)))
			If CommaOnLastMember OrElse (i < Dict.Keys.Count - 1) Then sb.Append(",")
		Next
		sb.Append("}")
		Return sb.ToString()
	End Function
	Private Function LsonArrayToLuaCodeText(ByVal LsonArray As LsonDict) As String
		Dim sb As New StringBuilder()
		sb.Append("{")
		For i As Integer = 0 To LsonArray.Keys.Count - 1
			If WriteArrayKeys Then
				sb.Append("[")
				sb.Append((i + 1).ToString())
				sb.Append("]=")
			End If
			sb.Append(LsonValueToLuaCodeText(LsonArray.Values(i)))
			If CommaOnLastMember OrElse (i < LsonArray.Keys.Count - 1) Then sb.Append(",")
		Next
		sb.Append("}")
		Return sb.ToString()
	End Function
	Private Function IsLsonDictAnArray(ByVal Dict As LsonDict) As Boolean
		If Dict Is Nothing Then Return False
		For i As Integer = 0 To Dict.Keys.Count - 1
			Dim lv As LsonValue = Dict.Keys(i)
			If lv.GetType() IsNot GetType(LsonNumber) Then Return False
			If lv.GetLongLenient() <> i + 1 Then Return False
		Next
		Return True
	End Function
	Private Function IsLsonNumberFloating(ByVal Value As LsonNumber) As Boolean	' TODO: test this
		If Value Is Nothing Then Return False
		If Value.GetType() IsNot GetType(LsonNumber) Then Return False
		Dim TempDouble As Double = Value.GetDoubleLenient()
		If TempDouble Mod CDbl(1) = CDbl(0) Then
			Return False
		Else
			Return True
		End If
	End Function
	'Public Function GetTextEncoding(ByVal Bytes As Byte()) As Encoding
	'	Using ms As New MemoryStream(Bytes)
	'		Using sr As New StreamReader(ms)
	'			sr.Read() ' is this necessary?
	'			Return sr.CurrentEncoding
	'		End Using
	'	End Using
	'End Function
	''' <summary>Converts indented, multiline string to compact, single-line string</summary>
	Public Function CompactText(ByVal Text As String) As String
		Dim sb As New StringBuilder()
		Using sr As New StringReader(Text)
			Do
				Dim NextLine As String = sr.ReadLine()
				If NextLine = Nothing Then
					Exit Do
				Else
					sb.Append(NextLine.Trim())
				End If
			Loop
		End Using
		Return sb.ToString()
	End Function
#End Region
#Region "Enums"
	Public Enum LuaPropertyTypes As Integer
		Invalid = 0
		Null
		[Long]
		[Double]
		[Boolean]
		[String]
		Dict
	End Enum
#End Region
End Module
