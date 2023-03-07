using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hook;
public class HookFunction
{
    public HookFunction(Function origin, Function ripped)
    {
        Origin = origin;
        Ripped = ripped;
    }

    public Function Origin;
    public Function Ripped;

    public bool Modified { get; private set; }

    public void SetOrigin()
    {

        Modified = false;
    }

}