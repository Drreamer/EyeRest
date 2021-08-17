Imports System.Windows.Threading

Class MainWindow


   Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
      'Dispatcher.BeginInvoke(Sub()
                                WindowStyle = WindowStyle.None
                                WindowState = WindowState.Maximized
                                Topmost = True

                             'End Sub,  DispatcherPriority.ApplicationIdle)
        
   End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        Me.Hide()
    End Sub
End Class
