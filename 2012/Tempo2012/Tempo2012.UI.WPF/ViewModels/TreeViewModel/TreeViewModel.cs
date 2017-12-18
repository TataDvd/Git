using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tempo2012.EntityFramework.Models;
using System.Collections.ObjectModel;

namespace Tempo2012.UI.WPF.ViewModels.treeviewmodel
{
    public class TreeViewModel
    {
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                if (value != _isExpanded)
                {
                    _isExpanded = value;
                    
                }

                
            }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value != _isSelected)
                {
                    _isSelected = value;
                }
            }
        }
        private ObservableCollection<TreeViewModel> _SubAcc;
        public ObservableCollection<TreeViewModel> SubAccs
        {
            get
            {
                if (_SubAcc == null)
                {
                    _SubAcc = new ObservableCollection<TreeViewModel>();
                }
                return _SubAcc;
            }
            set
            {
                _SubAcc = value;
            }
        }
        private AccountsModel _currAcc;
        private bool _isExpanded;
        public AccountsModel CurrAcc
        {
            get
            {
                return _currAcc;
            }
            set
            {
                _currAcc=value;
                
            }

        }






        public bool _isSelected { get; set; }

        
    }
}
