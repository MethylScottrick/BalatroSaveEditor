#Region "Imports"
Imports LsonLib
Imports System.IO
Imports System.IO.Compression
Imports System.Text
Imports System.Text.RegularExpressions
#End Region
Namespace Lua
	Public Class ProfileFile
		Inherits LuaFile
#Region "Properties"
		Public Overrides ReadOnly Property FileType As LuaFileTypes
			Get
				Return LuaFileTypes.Profile
			End Get
		End Property
#End Region
#Region "Lua Properties"
		Public Property ValName As String
			Get
				Return ValString("name")
			End Get
			Set(value As String)
				ValString("name", True) = value
			End Set
		End Property
		'Public Property Val As Long
		'	Get
		'		Try
		'			Return DataDict("").GetLongLenient()
		'		Catch ex As Exception
		'			Return 0
		'		End Try
		'	End Get
		'	Set(value As Long)
		'		Try
		'			DataDict("") = value
		'		Catch ex As Exception
		'		End Try
		'	End Set
		'End Property
#End Region
#Region "Constructors/Destructors"
		Public Sub New(ByVal FilePath As String)
			MyBase.New(FilePath)
		End Sub
		Protected Overrides Sub Finalize()
			MyBase.Finalize()
		End Sub
#End Region
#Region "Methods"
#End Region
	End Class
End Namespace
