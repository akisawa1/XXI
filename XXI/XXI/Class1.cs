using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Data.Entity;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace XXI
{
    public class Rate
    {
        public int Id { get; set; }
        public DateTime date { get; set; }
        public int NumCode { get; set; }
        public string CharCode { get; set; }
        public int Nominal { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
    }

    class RateContext : DbContext
    {
        public RateContext()
            : base("ExchangeRates")
        { }

        public DbSet<Rate> Rates { get; set; }
    }
    public class ExchageRates
    {
        public decimal GetRate(int valute, DateTime date)
        {
            decimal rate;
            using (RateContext db = new RateContext())
            {
                System.Data.SqlClient.SqlParameter pDate = new System.Data.SqlClient.SqlParameter("@date", date);
                System.Data.SqlClient.SqlParameter pValute = new System.Data.SqlClient.SqlParameter("@valute", valute);
                var list = db.Rates.SqlQuery("SELECT * FROM Rates WHERE Rates.date=@date AND NumCode=@valute", pDate, pValute);
                rate = list.FirstOrDefault().Value;
             }

            return rate;
        }
        public void Download()
        {
            WebClient wc = new WebClient();
            string adress = "http://www.cbr.ru/scripts/XML_daily.asp?date_req=" +
                DateTime.Now.Date.ToString();
            wc.DownloadFile(adress, "temp.xml");

            XDocument xdoc = XDocument.Load("temp.xml");
            DateTime today = DateTime.Now.Date;
                /*DateTime.Parse(
                xdoc.Element("ValCurs").Attribute("Date").Value);
                */
            foreach (XElement v in xdoc.Element("ValCurs").Elements("Valute"))
            {
                using (RateContext db=new RateContext())
                {
                    System.Data.SqlClient.SqlParameter pDate = new System.Data.SqlClient.SqlParameter("@date", 
                        DateTime.Now.Date);
                    var list = db.Rates.SqlQuery("SELECT * FROM Rates WHERE Rates.date=@date", pDate);
                    if (list.Count() > 0) return;

                    Rate rate = new Rate
                    {
                        date = today,
                        NumCode = int.Parse(v.Element("NumCode").Value),
                        CharCode = v.Element("CharCode").Value,
                        Nominal = int.Parse(v.Element("Nominal").Value),
                        Name = v.Element("Name").Value,
                        Value = decimal.Parse(v.Element("Value").Value)
                    };
                    db.Rates.Add(rate);
                    db.SaveChanges();
                }
            }
        }
        static TimeSpan aTime = new TimeSpan(6, 0, 0);
        private static System.Timers.Timer aTimer;

        private static void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            ((ExchageRates)source).Download();

            int delay = (aTime - DateTime.Now.TimeOfDay).Add(new TimeSpan(24, 0, 0)).Milliseconds;
            aTimer.Interval = delay;
        }
        public void StartDownloadTimer(DateTime time)
        {
            aTime = time.TimeOfDay;
            aTimer = new System.Timers.Timer();
            OnTimedEvent(this, null);
        }
    }
}
