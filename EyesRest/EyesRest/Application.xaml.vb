Imports System.IO
Imports System.Threading
Imports System.Windows.Forms
Imports System.Windows.Threading

Class Application

   ' Application-level events, such as Startup, Exit, and DispatcherUnhandledException
   ' can be handled in this file.

   Private backgroundInterval As TimeSpan = TimeSpan.FromSeconds(15)
   Private onTopInterval As TimeSpan = TimeSpan.FromSeconds(30)
   Private ticksCount As Integer
   Private frozenMouseTicksCounts As Integer
   Private mousePosition As System.Drawing.Point

   Private mvWindows As New List(Of MainWindow)()

   Protected Overrides Sub OnStartup(e As StartupEventArgs)
#If DEBUG Then
      backgroundInterval = TimeSpan.FromMilliseconds(100)
      onTopInterval = TimeSpan.FromSeconds(3)
#End If

      'MyBase.OnStartup(e)
      Dim lTimer As New DispatcherTimer() With {.Interval = backgroundInterval}
      lTimer.Start()
      AddHandler lTimer.Tick, AddressOf Timer_Tick     
   End Sub

   Private Sub Timer_Tick(sender As Object, e As EventArgs)
      Dim lTimer = CType(sender, DispatcherTimer)
      lTimer.Stop()
      If Not mvWindows.Any() Then
         ticksCount += 1
         If mousePosition = Cursor.Position Then
            frozenMouseTicksCounts += 1
            If frozenMouseTicksCounts > 8 Then
               ticksCount = 0
            End If
         Else
            mousePosition = Cursor.Position
            frozenMouseTicksCounts = 0
         End If

         If ticksCount >= 15 * 4 Then
            ticksCount = 0
            Dim screenCount As Integer = Screen.AllScreens.Length
            Dim taskArray(screenCount - 1) As Task
            For i = 0 To screenCount - 1
               Dim s = Screen.AllScreens(i)
               Dim scheduler = TaskScheduler.FromCurrentSynchronizationContext()
               taskArray(i) = Task.Factory.StartNew(Sub()
                                                       Dim lWindow = New MainWindow()
                                                       lWindow.Left = s.Bounds.Left
                                                       lWindow.Top = s.Bounds.Top
                                                       lWindow.Width = 0
                                                       lWindow.Height = 0
                                                       lWindow.WindowStyle = WindowStyle.None
                                                       lWindow.Show()
                                                       mvWindows.Add(lWindow)
                                                    End Sub, Nothing, Nothing, scheduler)
            Next
            Task.WaitAll(taskArray)
            lTimer.Interval = onTopInterval
         End If
      Else
         For i = mvWindows.Count - 1 To 0 Step -1
            mvWindows(i).Close()            
         Next
         mvWindows.Clear()
         lTimer.Interval = backgroundInterval
      End If
      lTimer.Start()
   End Sub

End Class
