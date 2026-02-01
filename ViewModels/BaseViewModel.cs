using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TeaRecallManager.ViewModels
{
  /* INotifyPropertyChanged を提供するベースVM */
  public class BaseViewModel : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void RaisePropertyChanged([CallerMemberName] string? name = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
  }
}

