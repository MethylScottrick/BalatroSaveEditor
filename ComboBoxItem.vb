#Region "Imports"
Imports System
Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports System.Runtime
Imports System.Runtime.CompilerServices
Imports System.Runtime.Hosting
Imports System.Runtime.InteropServices
Imports System.Runtime.Remoting
Imports System.Runtime.Serialization
Imports System.Windows
Imports System.Windows.Forms
#End Region
<DefaultProperty("Text")>
Public Class ComboBoxItem
	Public Property Text As String
	Public Property Tag As Object
	Public Sub New()
	End Sub
	Public Sub New(ByVal Text As String)
		Me.New()
		Me.Text = Text
	End Sub
	Public Sub New(ByVal Text As String, ByVal Tag As Object)
		Me.New(Text)
		Me.Tag = Tag
	End Sub
	Public Overrides Function ToString() As String
		Return Text
	End Function
	Public Overrides Function Equals(obj As Object) As Boolean
		Return MyBase.Equals(obj)
	End Function
	Public Shared Operator =(ByVal obj1 As ComboBoxItem, ByVal obj2 As ComboBoxItem) As Boolean
		If obj1 Is Nothing AndAlso obj2 Is Nothing Then Return True
		If obj1 IsNot Nothing AndAlso obj2 IsNot Nothing AndAlso obj1.Text = obj2.Text AndAlso obj1.Tag = obj2.Tag Then
			Return True
		Else
			Return False
		End If
	End Operator
	Public Shared Operator =(ByVal obj1 As ComboBoxItem, ByVal obj2 As String) As Boolean
		If obj1 Is Nothing AndAlso obj2 Is Nothing Then Return True
		If obj1 IsNot Nothing AndAlso obj2 IsNot Nothing AndAlso obj1.Text = obj2 Then
			Return True
		Else
			Return False
		End If
	End Operator
	Public Shared Operator <>(ByVal obj1 As ComboBoxItem, ByVal obj2 As ComboBoxItem) As Boolean
		Return Not (obj1 = obj2)
	End Operator
	Public Shared Operator <>(ByVal obj1 As ComboBoxItem, ByVal obj2 As String) As Boolean
		Return Not (obj1 = obj2)
	End Operator
End Class
