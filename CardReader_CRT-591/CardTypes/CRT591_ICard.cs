using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardReader_CRT_591
{
    public interface CRT591_ICard
    {
        CRT591_CardTypes CardBaseType { get; }
    }
}
