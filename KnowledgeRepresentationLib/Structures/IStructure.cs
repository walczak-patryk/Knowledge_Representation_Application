using KR_Lib.Formulas;
using KR_Lib.Structures;

namespace KR_Lib.Structures
{
    public interface IStructure
    {
        Structure ToModel();
        int H(Formula formula, int time);
    }
}
