using System;
using System.Windows.Input;

namespace TeaRecallManager.ViewModels
{
  // 単純な非同期コマンド（UIテストを容易にするため最小実装）
  public class RelayCommand : ICommand
  {
    private readonly Func<Task> _executeAsync;
    private readonly Func<bool>? _canExecute;

    public RelayCommand(Func<Task> executeAsync, Func<bool>? canExecute = null)
    {
      _executeAsync = executeAsync ?? throw new ArgumentNullException(nameof(executeAsync));
      _canExecute = canExecute;
    }

    public bool CanExecute(object? parameter) => _canExecute?.Invoke() ?? true;

    public async void Execute(object? parameter) => await _executeAsync();

    public event EventHandler? CanExecuteChanged;
    public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
  }
}

