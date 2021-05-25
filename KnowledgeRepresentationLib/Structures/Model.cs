using KR_Lib.Structures;

namespace KR_Lib.Queries
{
    public class Model : Structure
    {
        public Model(int endTime) : base(endTime)
        {

        }

        public Model(Structure structure) : base(structure)
        { }

        public new Structure ToModel()
        {
            return this as Model;
        }
    }
}