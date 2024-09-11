using System;
using System.Collections.Generic;
using System.Linq;
namespace StockForecasting.Modals
{
    public class MLData
    {
        private readonly List<MLInput> _data;
        private readonly int _splitIndex;
        public int TrainCount => _splitIndex;

        public MLData(List<MLInput> data, float splitPercentage = 0.8f)
        {
            _data = data;
            _splitIndex = (int)(_data.Count * splitPercentage);
        }

        private IEnumerable<MLInput> GetData(int start, int end)
        {
            for (int i = start; i < end; i++)
                yield return _data[i];
        }

        public IEnumerable<MLInput> Train => GetData(0, _splitIndex);
        public IEnumerable<MLInput> Test => GetData(_splitIndex, _data.Count);

        public float SuccessPercentage { get; set; }
        public float ActualCurrent => _data[^1].Value;
        public (float Lower, float Upper) PredictionCurrent { get; set; }

    }
}
