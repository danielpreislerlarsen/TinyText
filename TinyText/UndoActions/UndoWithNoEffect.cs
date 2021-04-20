using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyText.UndoActions
{
    public class UndoWithNoEffect : IUndoAction
    {
        public void Execute()
        {
        }
    }
}
