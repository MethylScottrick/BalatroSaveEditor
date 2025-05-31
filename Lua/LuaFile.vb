#Region "Imports"
Imports LsonLib
Imports System.IO
Imports System.IO.Compression
Imports System.Text
Imports System.Text.RegularExpressions
#End Region
Namespace Lua
	Public Class LuaFile
#Region "Properties"
		Protected Property FilePath As String
		Protected Property TextEncoding As Encoding
		Public Property DecompressedData As Byte()
		Public Property DecompressedText As String
		Public Property DataDict As LsonDict
		Public Overridable ReadOnly Property FileType As LuaFileTypes
			Get
				Return LuaFileTypes.Unknown
			End Get
		End Property
#End Region
#Region "Lua Helper Properties"
		Protected Function GetLsonValueFromKeyPath(ByVal Key As String) As LsonValue
			Dim Tokens As String() = Key.Split("\")
			Dim CurrentDict As LsonDict = DataDict
			For i As Integer = 0 To Tokens.Length - 2
				If CurrentDict.ContainsKey(Tokens(i)) AndAlso CurrentDict(Tokens(i)).GetType() Is GetType(LsonDict) Then ' test this
					CurrentDict = CurrentDict(Tokens(i))
				Else
					Return Nothing
				End If
			Next
			If CurrentDict.ContainsKey(Tokens(Tokens.Length - 1)) Then
				Return CurrentDict(Tokens(Tokens.Length - 1))
			Else
				Return Nothing
			End If
		End Function
		Protected Sub SetLsonValueFromKeyPath(ByVal Key As String, ByVal Value As Object, ByVal ValueType As LuaPropertyTypes, Optional ByVal Mandatory As Boolean = False)
			Dim ValueIsNothing As Boolean
			Dim NewLsonValue As LsonValue = Nothing
			If Value Is Nothing Then
				ValueIsNothing = True
			Else
				Select Case ValueType
					Case LuaPropertyTypes.Boolean
						Dim TypedValue As Boolean? = Value
						If TypedValue.HasValue Then
							ValueIsNothing = False
							NewLsonValue = New LsonBool(TypedValue.Value)
						Else
							ValueIsNothing = True
						End If
					Case LuaPropertyTypes.Dict
						Throw New NotImplementedException()
					Case LuaPropertyTypes.Double
						Dim TypedValue As Double? = Value
						If TypedValue.HasValue Then
							ValueIsNothing = False
							NewLsonValue = New LsonNumber(TypedValue.Value)
						Else
							ValueIsNothing = True
						End If
					Case LuaPropertyTypes.Long
						Dim TypedValue As Long? = Value
						If TypedValue.HasValue Then
							ValueIsNothing = False
							NewLsonValue = New LsonNumber(TypedValue.Value)
						Else
							ValueIsNothing = True
						End If
					Case LuaPropertyTypes.String
						ValueIsNothing = False
						NewLsonValue = New LsonString(Value)
					Case Else
						ValueIsNothing = True
				End Select
			End If
			Dim Tokens As String() = Key.Split("\")
			Dim CurrentDict As LsonDict = DataDict
			For i As Integer = 0 To Tokens.Length - 2
				If CurrentDict.ContainsKey(Tokens(i)) AndAlso CurrentDict(Tokens(i)).GetType() Is GetType(LsonDict) Then ' test this
					CurrentDict = CurrentDict(Tokens(i))
				Else
					If ValueIsNothing AndAlso (Not Mandatory) Then
						Return
					Else
						Dim NewDict As New LsonDict()
						CurrentDict.Add(Tokens(i), NewDict)
						CurrentDict = NewDict
					End If
				End If
			Next
			If CurrentDict.ContainsKey(Tokens(Tokens.Length - 1)) Then
				If ValueIsNothing Then
					If Mandatory Then
						CurrentDict(Tokens(Tokens.Length - 1)) = LsonNoValue.Instance
					Else
						CurrentDict.Remove(Tokens(Tokens.Length - 1))
					End If
				Else
					CurrentDict(Tokens(Tokens.Length - 1)) = NewLsonValue
				End If
			Else
				If ValueIsNothing Then
					If Mandatory Then CurrentDict.Add(Tokens(Tokens.Length - 1), LsonNoValue.Instance)
				Else
					CurrentDict.Add(Tokens(Tokens.Length - 1), NewLsonValue)
				End If
			End If
		End Sub
		Protected Property ValLong(ByVal Key As String, Optional ByVal Mandatory As Boolean = False) As Long?
			Get
				Dim lv As LsonValue = GetLsonValueFromKeyPath(Key)
				If lv Is Nothing Then Return Nothing
				If lv.GetType() IsNot GetType(LsonNumber) Then Return Nothing
				Return lv.GetLongLenient()
			End Get
			Set(value As Long?)
				SetLsonValueFromKeyPath(Key, value, LuaPropertyTypes.Long, Mandatory)
			End Set
		End Property
		Protected Property ValBoolean(ByVal Key As String, Optional ByVal Mandatory As Boolean = False) As Boolean?
			Get
				Dim lv As LsonValue = GetLsonValueFromKeyPath(Key)
				If lv Is Nothing Then Return Nothing
				If lv.GetType() IsNot GetType(LsonBool) Then Return Nothing
				Return lv.GetBoolLenient()
			End Get
			Set(value As Boolean?)
				SetLsonValueFromKeyPath(Key, value, LuaPropertyTypes.Boolean, Mandatory)
			End Set
		End Property
		Protected Property ValDouble(ByVal Key As String, Optional ByVal Mandatory As Boolean = False) As Double?
			Get
				Dim lv As LsonValue = GetLsonValueFromKeyPath(Key)
				If lv Is Nothing Then Return Nothing
				If lv.GetType() IsNot GetType(LsonNumber) Then Return Nothing
				Return lv.GetDoubleLenient()
			End Get
			Set(value As Double?)
				SetLsonValueFromKeyPath(Key, value, LuaPropertyTypes.Double, Mandatory)
			End Set
		End Property
		Protected Property ValString(ByVal Key As String, Optional ByVal Mandatory As Boolean = False) As String
			Get
				Dim lv As LsonValue = GetLsonValueFromKeyPath(Key)
				If lv Is Nothing Then Return Nothing
				If lv.GetType() IsNot GetType(LsonString) Then Return Nothing
				Return lv.GetStringLenient()
			End Get
			Set(value As String)
				SetLsonValueFromKeyPath(Key, value, LuaPropertyTypes.String, Mandatory)
			End Set
		End Property
#End Region
#Region "Constructors/Destructors"
		Public Sub New()
		End Sub
		Public Sub New(ByVal FilePath As String)
			Me.FilePath = FilePath
			OpenFile()
		End Sub
		Protected Overrides Sub Finalize()
			MyBase.Finalize()
		End Sub
#End Region
#Region "Methods"
		Protected Sub OpenFile()
			DecompressedData = Nothing
			Using fsIn As New FileStream(Me.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
				Using msOut As New MemoryStream()
					Using dsInflater As New DeflateStream(fsIn, CompressionMode.Decompress)
						dsInflater.CopyTo(msOut)
						DecompressedData = msOut.ToArray()
						If DebugWriteDecompressedFile Then File.WriteAllBytes(Path.ChangeExtension(Path.GetFileName(Me.FilePath), "lua"), DecompressedData)
					End Using
				End Using
			End Using
			DecompressedText = String.Empty
			Using ms As New MemoryStream(DecompressedData)
				Using sr As New StreamReader(ms)
					DecompressedText = sr.ReadToEnd()
					Me.TextEncoding = sr.CurrentEncoding
				End Using
			End Using
			Dim LuaString As String = Regex.Replace(DecompressedText.Trim(), "return (.*)", "$1")
			Me.DataDict = LsonDict.Parse(LuaString)
		End Sub
		Public Sub SaveFile()
			Dim DecompressedString As String = Me.DataDict.ToLuaCode()
			'File.WriteAllText("debugout.lua", DecompressedString) ' DEBUG
			Dim DecompressedBytes As Byte() = Me.TextEncoding.GetBytes(DecompressedString)
			Using msIn As New MemoryStream(DecompressedBytes)
				Using msOut As New MemoryStream()
					Using dsDeflater As New DeflateStream(msOut, CompressionMode.Compress)
						msIn.CopyTo(dsDeflater)
					End Using
					'Dim CompressedBytes As Byte() = msOut.ToArray()
					'Dim a = 1
					File.WriteAllBytes(Me.FilePath, msOut.ToArray()) ' This must be call OUTSIDE the using for the deflate stream, so it gets flushed first
				End Using
			End Using
		End Sub
		Public Sub BackupFile()
			File.Copy(Me.FilePath, Me.FilePath & ".bak", True)
		End Sub
#End Region
	End Class
End Namespace
