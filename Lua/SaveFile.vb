#Region "Imports"
Imports LsonLib
Imports System.IO
Imports System.IO.Compression
Imports System.Text
Imports System.Text.RegularExpressions
#End Region
Namespace Lua
	Public Class SaveFile
		Inherits LuaFile
#Region "Properties"
		Public Overrides ReadOnly Property FileType As LuaFileTypes
			Get
				Return LuaFileTypes.Save
			End Get
		End Property
#End Region
#Region "Lua Properties"
		Public Property ValState As Long?
			Get
				Return ValLong("STATE")
			End Get
			Set(value As Long?)
				ValLong("STATE", True) = value
			End Set
		End Property
		Public Property ValVersion As String
			Get
				Return ValString("VERSION")
			End Get
			Set(value As String)
				ValString("VERSION", True) = value
			End Set
		End Property
		'Public Property ValTags As Long
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
#Region "Lua Properties (BLIND)"
		'Public Property Val As Object
		'	Get
		'		Try
		'			Return DataDict("BLIND")("").Get
		'		Catch ex As Exception
		'			Return 0
		'		End Try
		'	End Get
		'	Set(value As Object)
		'		Try
		'			DataDict("BLIND")("") = value
		'		Catch ex As Exception
		'		End Try
		'	End Set
		'End Property
#End Region
#Region "Lua Properties (BACK)"
		'Public Property Val As Object
		'	Get
		'		Try
		'			Return DataDict("BACK")("").Get
		'		Catch ex As Exception
		'			Return 0
		'		End Try
		'	End Get
		'	Set(value As Object)
		'		Try
		'			DataDict("BACK")("") = value
		'		Catch ex As Exception
		'		End Try
		'	End Set
		'End Property
#End Region
#Region "Lua Properties (cardAreas)"
		Public Property ValJokerLimit As Long?
			Get
				Return ValLong("cardAreas\jokers\config\card_limit")
				'Try
				'	Return DataDict("cardAreas")("jokers")("config")("card_limit").GetLongLenient()
				'Catch ex As Exception
				'	Return 0
				'End Try
			End Get
			Set(value As Long?)
				Try
					ValLong("cardAreas\jokers\config\card_limit") = value
					ValLong("cardAreas\jokers\config\temp_limit") = value
					'DataDict("cardAreas")("jokers")("config")("card_limit") = value
					'DataDict("cardAreas")("jokers")("config")("temp_limit") = value
				Catch ex As Exception
				End Try
			End Set
		End Property
		Public Property ValConsumableLimit As Long?
			Get
				Return ValLong("cardAreas\consumeables\config\card_limit")
				'Try
				'	Return DataDict("cardAreas")("consumeables")("config")("card_limit").GetLongLenient()
				'Catch ex As Exception
				'	Return 0
				'End Try
			End Get
			Set(value As Long?)
				Try
					ValLong("cardAreas\consumeables\config\card_limit") = value
					ValLong("cardAreas\consumeables\config\temp_limit") = value
					'DataDict("cardAreas")("consumeables")("config")("card_limit") = value
					'DataDict("cardAreas")("consumeables")("config")("temp_limit") = value
				Catch ex As Exception
				End Try
			End Set
		End Property
		Public Property ValHandSize As Long?
			Get
				Return ValLong("cardAreas\hand\config\card_limit")
				'Try
				'	'Return DataDict("cardAreas")("hand")("config")("card_limit").GetLongLenient()
				'Catch ex As Exception
				'	Return 0
				'End Try
			End Get
			Set(value As Long?)
				Try
					ValLong("cardAreas\hand\config\card_limit") = value
					ValLong("cardAreas\hand\config\temp_limit") = value
					'DataDict("cardAreas")("hand")("config")("card_limit") = value
					'DataDict("cardAreas")("hand")("config")("temp_limit") = value
				Catch ex As Exception
				End Try
			End Set
		End Property
		'Public Property Val As Object
		'	Get
		'		Try
		'			Return DataDict("cardAreas")("").Get
		'		Catch ex As Exception
		'			Return 0
		'		End Try
		'	End Get
		'	Set(value As Object)
		'		Try
		'			DataDict("cardAreas")("") = value
		'		Catch ex As Exception
		'		End Try
		'	End Set
		'End Property
#End Region
#Region "Lua Properties (GAME)"
		'Public ReadOnly Property GameDict As LsonDict
		'	Get
		'		If DataDict Is Nothing Then
		'			Return Nothing
		'		Else
		'			Return DataDict("GAME")
		'		End If
		'	End Get
		'End Property
		Public Property ValDollars As Long?
			Get
				Try
					Return DataDict("GAME")("dollars").GetLongLenient()
				Catch ex As Exception
					Return 0
				End Try
			End Get
			Set(value As Long?)
				Try
					DataDict("GAME")("dollars") = value
				Catch ex As Exception
				End Try
			End Set
		End Property
		Public Property ValTarotRate As Long?
			Get
				Try
					Return DataDict("GAME")("tarot_rate").GetLongLenient()
				Catch ex As Exception
					Return 0
				End Try
			End Get
			Set(value As Long?)
				Try
					DataDict("GAME")("tarot_rate") = value
				Catch ex As Exception
				End Try
			End Set
		End Property
		Public Property ValPackSize As Long?
			Get
				Try
					Return DataDict("GAME")("pack_size").GetLongLenient()
				Catch ex As Exception
					Return 0
				End Try
			End Get
			Set(value As Long?)
				Try
					DataDict("GAME")("pack_size") = value
				Catch ex As Exception
				End Try
			End Set
		End Property
		Public Property ValWinAnte As Long?
			Get
				Try
					Return DataDict("GAME")("win_ante").GetLongLenient()
				Catch ex As Exception
					Return 0
				End Try
			End Get
			Set(value As Long?)
				Try
					DataDict("GAME")("win_ante") = value
				Catch ex As Exception
				End Try
			End Set
		End Property
		Public Property ValInflation As Long?
			Get
				Try
					Return DataDict("GAME")("inflation").GetLongLenient()
				Catch ex As Exception
					Return 0
				End Try
			End Get
			Set(value As Long?)
				Try
					DataDict("GAME")("inflation") = value
				Catch ex As Exception
				End Try
			End Set
		End Property
		Public Property ValSpectralRate As Long?
			Get
				Try
					Return DataDict("GAME")("spectral_rate").GetLongLenient()
				Catch ex As Exception
					Return 0
				End Try
			End Get
			Set(value As Long?)
				Try
					DataDict("GAME")("spectral_rate") = value
				Catch ex As Exception
				End Try
			End Set
		End Property
		Public Property ValRerollCost As Long?
			Get
				Try
					'Return DataDict("GAME")("base_reroll_cost").GetLongLenient()
					Return DataDict("GAME")("round_resets")("reroll_cost").GetLongLenient()
				Catch ex As Exception
					Return 0
				End Try
			End Get
			Set(value As Long?)
				Try
					'DataDict("GAME")("base_reroll_cost") = value
					DataDict("GAME")("round_resets")("reroll_cost") = value
				Catch ex As Exception
				End Try
			End Set
		End Property
		Public Property ValHands As Long?
			Get
				Try
					Return DataDict("GAME")("round_resets")("hands").GetLongLenient()
				Catch ex As Exception
					Return 0
				End Try
			End Get
			Set(value As Long?)
				Try
					DataDict("GAME")("round_resets")("hands") = value
				Catch ex As Exception
				End Try
			End Set
		End Property
		Public Property ValDiscards As Long?
			Get
				Try
					Return DataDict("GAME")("round_resets")("discards").GetLongLenient()
				Catch ex As Exception
					Return 0
				End Try
			End Get
			Set(value As Long?)
				Try
					DataDict("GAME")("round_resets")("discards") = value
				Catch ex As Exception
				End Try
			End Set
		End Property
		'Public Property ValAnte As Long?
		'	Get
		'		Try
		'			Return DataDict("GAME")("").GetLongLenient()
		'		Catch ex As Exception
		'			Return 0
		'		End Try
		'	End Get
		'	Set(value As Long?)
		'		Try
		'			DataDict("GAME")("") = value
		'		Catch ex As Exception
		'		End Try
		'	End Set
		'End Property
		Public Property ValPlayingCardRate As Long?
			Get
				Try
					Return DataDict("GAME")("playing_card_rate").GetLongLenient()
				Catch ex As Exception
					Return 0
				End Try
			End Get
			Set(value As Long?)
				Try
					DataDict("GAME")("playing_card_rate") = value
				Catch ex As Exception
				End Try
			End Set
		End Property
		Public Property ValEditionRate As Long?
			Get
				Try
					Return DataDict("GAME")("edition_rate").GetLongLenient()
				Catch ex As Exception
					Return 0
				End Try
			End Get
			Set(value As Long?)
				Try
					DataDict("GAME")("edition_rate") = value
				Catch ex As Exception
				End Try
			End Set
		End Property
		Public Property ValRound As Long?
			Get
				Try
					Return DataDict("GAME")("round").GetLongLenient()
				Catch ex As Exception
					Return 0
				End Try
			End Get
			Set(value As Long?)
				Try
					DataDict("GAME")("round") = value
				Catch ex As Exception
				End Try
			End Set
		End Property
		Public Property ValChips As Long?
			Get
				Try
					Return DataDict("GAME")("chips").GetLongLenient()
				Catch ex As Exception
					Return 0
				End Try
			End Get
			Set(value As Long?)
				Try
					DataDict("GAME")("chips") = value
				Catch ex As Exception
				End Try
			End Set
		End Property
		Public Property ValPlanetRate As Long?
			Get
				Try
					Return DataDict("GAME")("planet_rate").GetLongLenient()
				Catch ex As Exception
					Return 0
				End Try
			End Get
			Set(value As Long?)
				Try
					DataDict("GAME")("planet_rate") = value
				Catch ex As Exception
				End Try
			End Set
		End Property
		Public Property ValInterestCap As Long?
			Get
				Try
					Return DataDict("GAME")("interest_cap").GetLongLenient()
				Catch ex As Exception
					Return 0
				End Try
			End Get
			Set(value As Long?)
				Try
					DataDict("GAME")("interest_cap") = value
				Catch ex As Exception
				End Try
			End Set
		End Property
		Public Property ValInterestAmount As Long?
			Get
				Try
					Return DataDict("GAME")("interest_amount").GetLongLenient()
				Catch ex As Exception
					Return 0
				End Try
			End Get
			Set(value As Long?)
				Try
					DataDict("GAME")("interest_amount") = value
				Catch ex As Exception
				End Try
			End Set
		End Property
		Public Property ValDiscountPct As Long?
			Get
				Try
					Return DataDict("GAME")("discount_percent").GetLongLenient()
				Catch ex As Exception
					Return 0
				End Try
			End Get
			Set(value As Long?)
				Try
					DataDict("GAME")("discount_percent") = value
				Catch ex As Exception
				End Try
			End Set
		End Property
		Public Property ValJokerRate As Long?
			Get
				Try
					Return DataDict("GAME")("joker_rate").GetLongLenient()
				Catch ex As Exception
					Return 0
				End Try
			End Get
			Set(value As Long?)
				Try
					DataDict("GAME")("joker_rate") = value
				Catch ex As Exception
				End Try
			End Set
		End Property
		Public Property ValShopJokers As Long?
			Get
				Try
					Return DataDict("GAME")("shop")("joker_max").GetLongLenient()
				Catch ex As Exception
					Return 0
				End Try
			End Get
			Set(value As Long?)
				Try
					DataDict("GAME")("shop")("joker_max") = value
				Catch ex As Exception
				End Try
			End Set
		End Property
		'Public Property Val As Object
		'	Get
		'		Try
		'			Return DataDict("GAME")("").Get
		'		Catch ex As Exception
		'			Return 0
		'		End Try
		'	End Get
		'	Set(value As Object)
		'		Try
		'			DataDict("GAME")("") = value
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
