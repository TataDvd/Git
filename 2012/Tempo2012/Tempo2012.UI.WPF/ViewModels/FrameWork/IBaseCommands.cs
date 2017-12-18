using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Tempo2012.UI.WPF.ViewModels
{
    public interface IBaseCommands
    {
        ICommand AddFirmCommand { get;}
        ICommand UpdateFirmCommand { get; }
        ICommand DeleteFirmCommand { get;}
        ICommand MoveNextFirmCommand { get; }
        ICommand MovePreviusCommand { get; }
        ICommand MoveLastCommand { get;}
        ICommand MoveFirstCommand { get; }
        ICommand SearchCommand { get; }
        ICommand ReportCommand { get; }
        ICommand ViewCommand { get;  }
        ICommand CancelCommand { get;}
    }
}
