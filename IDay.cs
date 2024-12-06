namespace AdventOfCode2024
{
    public interface IDay
    {
        public string GetName();
        public Task<long> One();
        public Task<long> Two();
    }
}
