namespace KR_Lib.Structures
{
    public class InconsistentStructure : Structure
    {
        public InconsistentStructure() : base(int.MinValue)
        {
        }

        public override Structure ToModel()
        {
            return this as InconsistentStructure;
        }
    }
}
