'***************************************************************************

' Project  : AvionicsInstrumentControlDemo                                  
' File     : AirSpeedIndicatorInstrumentControl.cs                          
' Version  : 1                                                              
' Language : C#                                                             
' Summary  : The air speed indicator instrument control                     
' Creation : 19/06/2008                                                     
' Autor    : Guillaume CHOUTEAU                                             
' History  :                                                                

'***************************************************************************


Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Collections
Imports System.Drawing
Imports System.Text
Imports System.Data

Namespace AvionicsInstrumentControlDemo
    Class AirSpeedIndicatorInstrumentControl
        Inherits InstrumentControl
#Region "Fields"

        ' Parameters
        'Private airSpeed As Single
        'Private groundSpeed As Single

        Private groundSpeedTargetValue As Single
        Private groundSpeedMoveIndex As Integer
        Private groundSpeedStartingValue As Single

        Private airSpeedTargetValue As Single
        Private airSpeedMoveIndex As Integer
        Private airSpeedStartingValue As Single

        ' Images
        Private bmpCadran As New Bitmap(HK_GCS_Lite.My.Resources.AvionicsInstrumentsControlsRessources.speed_glowing)
        'Private bmpCadran As New Bitmap(HK_GCS.My.Resources.AvionicsInstrumentsControlsRessources.AirSpeedIndicator_Background)
        'Private bmpGroundNeedle As New Bitmap(HK_GCS.My.Resources.AvionicsInstrumentsControlsRessources.GroundSpeedNeedle)
        Private bmpGroundNeedle As New Bitmap(HK_GCS_Lite.My.Resources.AvionicsInstrumentsControlsRessources.needle_06)
        Private bmpAirNeedle As New Bitmap(HK_GCS_Lite.My.Resources.AvionicsInstrumentsControlsRessources.needle_07)
        Private bmpScroll As New Bitmap(HK_GCS_Lite.My.Resources.AvionicsInstrumentsControlsRessources.Number_Scroll)
        Private bmpGlass As New Bitmap(HK_GCS_Lite.My.Resources.AvionicsInstrumentsControlsRessources.glass_layer_3)

        Private nMaxSpeed As Single = 80
        Private sUnitLabel As String = "MPH"
        Private sInstrumentLabel As String = "Speed"

#End Region

#Region "Contructor"

        ''' <summary>
        ''' Required designer variable.
        ''' </summary>
        Private components As System.ComponentModel.Container = Nothing

        Public Sub New()
            ' Double bufferisation
            SetStyle(ControlStyles.DoubleBuffer Or ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint, True)
        End Sub

#End Region

#Region "Component Designer generated code"
        ''' <summary>
        ''' Required method for Designer support - do not modify 
        ''' the contents of this method with the code editor.
        ''' </summary>
        Private Sub InitializeComponent()
            components = New System.ComponentModel.Container()
        End Sub
#End Region

#Region "Paint"

        Protected Overrides Sub OnPaint(ByVal pe As PaintEventArgs)
            ' Calling the base class OnPaint
            MyBase.OnPaint(pe)

            ' Pre Display computings
            Dim ptCounter As New Point(165, 135)
            Dim ptRotation As New Point(150, 149)
            'Dim ptimgNeedle As New Point(136, 39)
            Dim ptimgNeedle As New Point(135, 36)

            bmpCadran.MakeTransparent(Color.Yellow)
            bmpGroundNeedle.MakeTransparent(Color.Yellow)
            bmpAirNeedle.MakeTransparent(Color.Yellow)
            bmpGlass.MakeTransparent(Color.Yellow)

            Me.BackColor = GetSystemColor("F5F4F1")

            Dim alphaNeedle As Double

            Dim scale As Single = CSng(Me.Width) / bmpCadran.Width

            ScrollCounter(pe, bmpScroll, 3, (GetCurrentEaseInstrument(groundSpeedStartingValue, groundSpeedTargetValue, groundSpeedMoveIndex)), ptCounter, scale)

            ' diplay mask
            Dim maskPen As New Pen(Me.BackColor, 30 * scale)
            pe.Graphics.DrawRectangle(maskPen, 0, 0, bmpCadran.Width * scale, bmpCadran.Height * scale)

            ' display border
            pe.Graphics.DrawImage(bmpBorder, 0, 0, CSng(bmpCadran.Width * scale), CSng(bmpCadran.Height * scale))

            ' display cadran
            pe.Graphics.DrawImage(bmpCadran, 0, 0, CSng(bmpCadran.Width * scale), CSng(bmpCadran.Height * scale))

            Dim fontSize As Single
            fontSize = Me.Width / 180 * 9
            Using string_format As New StringFormat()
                string_format.Alignment = StringAlignment.Center
                string_format.LineAlignment = StringAlignment.Near
                Using the_font As New Font("Arial", fontSize, FontStyle.Bold)
                    pe.Graphics.DrawString((nMaxSpeed / 8).ToString("0"), the_font, Brushes.Azure, bmpCadran.Height * 0.34 * scale, bmpCadran.Width * 0.7 * scale, string_format)
                    pe.Graphics.DrawString(((nMaxSpeed / 8) * 2).ToString("0"), the_font, Brushes.Azure, bmpCadran.Height * 0.22 * scale, bmpCadran.Width * 0.56 * scale, string_format)
                    pe.Graphics.DrawString(((nMaxSpeed / 8) * 3).ToString("0"), the_font, Brushes.Azure, bmpCadran.Height * 0.22 * scale, bmpCadran.Width * 0.36 * scale, string_format)
                    pe.Graphics.DrawString(((nMaxSpeed / 8) * 4).ToString("0"), the_font, Brushes.Azure, bmpCadran.Height * 0.34 * scale, bmpCadran.Width * 0.21 * scale, string_format)
                    pe.Graphics.DrawString(((nMaxSpeed / 8) * 5).ToString("0"), the_font, Brushes.Azure, bmpCadran.Height * 0.5 * scale, bmpCadran.Width * 0.16 * scale, string_format)
                    pe.Graphics.DrawString(((nMaxSpeed / 8) * 6).ToString("0"), the_font, Brushes.Azure, bmpCadran.Height * 0.66 * scale, bmpCadran.Width * 0.21 * scale, string_format)
                    pe.Graphics.DrawString(((nMaxSpeed / 8) * 7).ToString("0"), the_font, Brushes.Azure, bmpCadran.Height * 0.79 * scale, bmpCadran.Width * 0.36 * scale, string_format)
                    pe.Graphics.DrawString((nMaxSpeed).ToString("0"), the_font, Brushes.Azure, bmpCadran.Height * 0.77 * scale, bmpCadran.Width * 0.56 * scale, string_format)

                End Using
            End Using

            fontSize = Me.Width / 180 * 9
            Using the_font As New Font("Arial", fontSize, FontStyle.Bold)
                pe.Graphics.DrawString(sInstrumentLabel, the_font, Brushes.Azure, bmpCadran.Height * 0.54 * scale, bmpCadran.Width * 0.66 * scale)
            End Using
            fontSize = Me.Width / 180 * 8
            Using the_font As New Font("Arial", fontSize, FontStyle.Bold)
                pe.Graphics.DrawString(sUnitLabel, the_font, Brushes.Azure, bmpCadran.Height * 0.54 * scale, bmpCadran.Width * 0.75 * scale)
            End Using

            If airSpeedTargetValue > 0 Then
                alphaNeedle = InterpolPhyToAngle(GetCurrentEaseInstrument(airSpeedStartingValue, airSpeedTargetValue, airSpeedMoveIndex), 0, nMaxSpeed, 180.0, 468.0)
                RotateImage(pe, bmpAirNeedle, alphaNeedle, ptimgNeedle, ptRotation, scale)
            End If

            alphaNeedle = InterpolPhyToAngle(GetCurrentEaseInstrument(groundSpeedStartingValue, groundSpeedTargetValue, groundSpeedMoveIndex), 0.0, nMaxSpeed, 180.0, 468.0)
            'alphaNeedle = InterpolPhyToAngle(groundSpeed, 0.0, nMaxSpeed, 180.0, 468.0)
            ' display small needle
            RotateImage(pe, bmpGroundNeedle, alphaNeedle, ptimgNeedle, ptRotation, scale)
            'Debug.Print("alphaNeedle=" & alphaNeedle)


            pe.Graphics.DrawImage(bmpGlass, 0, 0, CSng(bmpGlass.Width * scale), CSng(bmpGlass.Height * scale))


        End Sub

#End Region

#Region "Methods"


        ''' Define the physical value to be displayed on the indicator
        ''' The aircraft air speed in kts
        Public Sub SetAirSpeedIndicatorParameters(ByVal aircraftGroundSpeed As Single, ByVal maxValue As Integer, ByVal instrumentName As String, ByVal unitString As String, Optional ByVal aircraftAirSpeed As Single = -1)
            Dim bFoundOne As Boolean = False

            If aircraftGroundSpeed <> groundSpeedTargetValue Then
                bFoundOne = True
                groundSpeedStartingValue = GetCurrentEaseInstrument(groundSpeedStartingValue, groundSpeedTargetValue, groundSpeedMoveIndex)
                groundSpeedMoveIndex = 0
                groundSpeedTargetValue = aircraftGroundSpeed
            End If

            If aircraftAirSpeed <> airSpeedTargetValue Then
                bFoundOne = True
                airSpeedStartingValue = GetCurrentEaseInstrument(airSpeedStartingValue, airSpeedTargetValue, airSpeedMoveIndex)
                airSpeedMoveIndex = 0
                airSpeedTargetValue = aircraftAirSpeed
            End If

            If bFoundOne = True Then
                'groundSpeed = aircraftGroundSpeed
                nMaxSpeed = maxValue
                sUnitLabel = unitString
                sInstrumentLabel = instrumentName
                'airSpeed = aircraftAirSpeed

                Me.Refresh()
            End If
        End Sub

        Public Sub TickEase()
            Dim bFoundOne As Boolean = False
            If groundSpeedMoveIndex <= g_EaseSteps - 2 Then
                bFoundOne = True
                groundSpeedMoveIndex = groundSpeedMoveIndex + 1
            End If

            If airSpeedMoveIndex <= g_EaseSteps - 2 Then
                bFoundOne = True
                airSpeedMoveIndex = airSpeedMoveIndex + 1
            End If

            If bFoundOne = True Then
                Me.Refresh()
            End If

        End Sub
#End Region

#Region "IDE"




#End Region

    End Class
End Namespace
