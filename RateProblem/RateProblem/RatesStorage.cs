using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RateProblem
{
    internal class RatesStorage
    {
        private ReaderWriterLock _lock = new ReaderWriterLock();
        private int _readerTimeout = 20;
        private int _writerTimeout = 200;
        private Dictionary<string, Rate> rates = new();

        // этим методом мы копируем котировки из нативного класса в базовый класс нашей программы, обновляем котировки много и часто, будем считать, что у нас 20+  вызовов этого метода в секунду
        public void UpdateRate(NativeRate newRate)
        {
            _lock.AcquireWriterLock(_writerTimeout);
            try
            {
                if (rates.ContainsKey(newRate.Symbol) == false)
                {
                    rates.Add(newRate.Symbol, new Rate());
                }

                var oldRate = rates[newRate.Symbol];
                oldRate.Time = newRate.Time;
                oldRate.Bid = newRate.Bid;
                oldRate.Ask = newRate.Ask;
            }
            finally 
            {
                _lock.ReleaseWriterLock();
            }
        }

        // это тоже высоконагруженный метод, вызываем 20+  раз в секунду
        public Rate GetRate(string symbol)
        {
            _lock.AcquireReaderLock(_readerTimeout);
            try
            {
                if (rates.ContainsKey(symbol) == false)
                    return null;
                return rates[symbol];
            }
            finally
            {
                _lock.ReleaseReaderLock();
            }
        }

    }
}
