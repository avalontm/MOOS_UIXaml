using System;
using System.Collections.Generic;


namespace System.Windows.Input
{
    public class ICommand
    {
        public Action Execute { set; get; }

        public ICommand(Action action)
        {
            Execute = action;
        }
    }
}
