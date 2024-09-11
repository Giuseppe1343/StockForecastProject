namespace StockForecasting.Modals
{
    public class MLData
    {
        private readonly List<MLInput> _data;
        private int _splitIndex;
        public int TrainCount => _splitIndex;
        public int TestCount => _data.Count - _splitIndex;

        public MLData(List<MLInput> data, float splitPercentage = 0.8f)
        {
            _data = data;
            _splitIndex = (int)(_data.Count * splitPercentage);
        }
        public void SetSplitPercentage(float splitPercentage)
        {
            _splitIndex = (int)(_data.Count * splitPercentage);
        }
        public IEnumerable<MLInput> Train => GetData(0, _splitIndex);
        public IEnumerable<MLInput> Test => GetData(_splitIndex, _data.Count);
        private IEnumerable<MLInput> GetData(int start, int end)
        {
            for (int i = start; i < end; i++)
                yield return _data[i];
        }
    }
}
