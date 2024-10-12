using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets
{
    public interface IFoxState
    {
        // 입력
        void OnInput();
        // 움직임
        void OnMove();
    }
}
