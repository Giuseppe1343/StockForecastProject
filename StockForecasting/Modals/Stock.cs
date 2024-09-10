namespace StockForecasting.Modals
{
    public class Stock
    {
        //Model Bound
        public int Id { get; set; }
        public string Name { get; set; }
        public List<TransactionRow> Transactions { get; set; }
        
        //Generated
        public IEnumerable<TrainRow> TrainDaily 
        {
            get
            {
                ActualDaily = (float)Transactions[Transactions.Count - 1].TAmount;
                return Transactions.SkipLast(1).Select(x => new TrainRow((float)x.TAmount));
            }
        }
        public IEnumerable<TrainRow> TrainWeekly
        {
            get
            {
                var nextWeek = Transactions[0].TDate.Date.AddDays(7);
                double sum = 0;

                var it = Transactions.GetEnumerator();
                bool hasRemainingItems = false;
                do
                {
                    hasRemainingItems = it.MoveNext();
                    if (!hasRemainingItems)
                    {
                        ActualWeekly = (float)(sum + it.Current.TAmount);
                        break;
                    }
                    if (it.Current.TDate.Date >= nextWeek)
                    {
                        yield return new TrainRow((float)sum);
                        nextWeek = it.Current.TDate.Date.AddDays(7);
                        sum = 0;
                    }
                    sum += it.Current.TAmount;
                }
                while (hasRemainingItems);
                //foreach (var item in Transactions)
                //{
                //    if (item.TDate.Date >= nextWeek)
                //    {
                //        yield return new TrainRow((float)sum);
                //        nextWeek = item.TDate.Date.AddDays(7);
                //        sum = 0;
                //    }
                //    sum += item.TAmount;
                //}
            }
        }
        public IEnumerable<TrainRow> TrainMonthly
        {
            get
            {
                var nextMonth = Transactions[0].TDate.Date.AddMonths(1);
                double sum = 0;

                var it = Transactions.GetEnumerator();
                bool hasRemainingItems = false;
                do
                {
                    hasRemainingItems = it.MoveNext();
                    if (!hasRemainingItems)
                    {
                        ActualMonthly = (float)(sum + it.Current.TAmount);
                        break;
                    }
                    if (it.Current.TDate.Date >= nextMonth)
                    {
                        yield return new TrainRow((float)sum);
                        nextMonth = it.Current.TDate.Date.AddMonths(1);
                        sum = 0;
                    }
                    sum += it.Current.TAmount;
                }while (hasRemainingItems);
                //var nextMonth = Transactions[0].TDate.Date.AddMonths(1);
                //double sum = 0;
                //foreach (var item in Transactions)
                //{
                //    if (item.TDate.Date >= nextMonth)
                //    {
                //        yield return new TrainRow((float)sum);
                //        nextMonth = item.TDate.Date.AddMonths(1);
                //        sum = 0;
                //    }
                //    sum += item.TAmount;
                //}
            }
        }

        //Calculated
        public (float Lower,float Upper) PredictionDaily { get; set; }
        public (float Lower, float Upper) PredictionWeekly { get; set; }
        public (float Lower, float Upper) PredictionMonthly { get; set; }
        public float ActualDaily { get; private set; }
        public float ActualWeekly { get; private set; }
        public float ActualMonthly { get; private set; }
    }
}
