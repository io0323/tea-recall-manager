using System;
using System.Threading.Tasks;
using System.Windows;
using TeaRecallManager.Data;

namespace TeaRecallManager
{
  /* アプリケーション開始処理とDB初期化を行う */
  public partial class App : Application
  {
    private async void OnStartup(object sender, StartupEventArgs e)
    {
      // 非同期でDBを初期化してシードを行う
      await DbInitializer.InitializeAsync();

      var main = new Views.MainWindow();
      main.Show();
    }
  }
}

