using System;
using System.Linq;
using System.IO;
using Quandl.NET;
using static Tools.GFile;
using static Tools.G;
namespace Tools
{
    // https://github.com/lppkarl/Quandl.NET

    public class ZQuandl
    {
        public static string ApiKey { get { return "gCbpWopzuxctHw6y-qq5"; } }

        private DateTime m_defaultDt1 = new DateTime(2017, 1, 1);
        private DateTime m_defaultDt2 = DateTime.Now;

        QuandlClient m_client = new QuandlClient(ApiKey);

        private static ZQuandl m_instance;
        public static ZQuandl Q
        {
            get
            {
                if (m_instance == null)
                    m_instance = new ZQuandl();
                return m_instance;
            }
        }

        private ZQuandl()
        {
        }


        // Given a directory path
        // Return a string array containing the Quandl dataset codes (*-datasets-codes.csv) files in that directory
        public static string[] GetQuandlDatasetCodes(string path)
        {
            return GetFilesEndingWith(path, "-datasets-codes.csv");
        }

        public Quandl.NET.Model.Response.TimeseriesDataResponse GetHistorical(string code, DateTime? _dt1 = null, DateTime? _dt2 = null)
        {
         
            DateTime dt1, dt2;
            if (_dt1 == null || _dt2 == null)
            {
                dt1 = m_defaultDt1;
                dt2 = m_defaultDt2;
            }
            else
            {
                dt1 = _dt1.Value;
                dt2 = _dt2.Value;
            }

            Console.Write("Retrieving Quandl data {0} to {1} ... ", dt1.ToShortDateString(), dt2.ToShortDateString());
            var qc = new MyQuandlCode(code);
            var data = m_client.Timeseries.GetDataAsync(qc.DatabaseCode, qc.DatasetCode, startDate: dt1, endDate: dt2);
            data.Wait();
            Console.WriteLine("Done.");

            // EXAMPLE WITH FILTERING
            /*var data = await client.Timeseries.GetDataAsync("WIKI", "FB",
                columnIndex: 4,
                startDate: new DateTime(2014, 1, 1),
                endDate: new DateTime(2014, 12, 31),
                collapse: Collapse.Monthly,
                transform: Transform.Rdiff);*/

            return data.Result;
        }

        public void GetHistoricalRAW(string code, string outputFilename)
        {
            if (!outputFilename.EndsWith(".csv")) outputFilename += ".csv";     // ensure our filename ends with ".csv"

            // The call returns a stream based on the returnFormat
            // (i.e. json stream if returnFormat = ReturnFormat.Json,
            //       xml stream if returnFormat = ReturnFormat.Xml,
            //       csv stream if returnFormat = ReturnFormat.Csv)
            // Let use Get time-series data api as an example
            Console.Write("Retrieving Quandl data ... ");
            var qc = new MyQuandlCode(code);
            var csvStream = m_client.Timeseries.GetDataAsync(qc.DatabaseCode, qc.DatasetCode, ReturnFormat.Csv);
            csvStream.Wait();
            Console.WriteLine("Done.");
            try
            {
                using (var fs = File.Create(outputFilename))
                {
                    csvStream.Result.CopyTo(fs);
                }
                Console.WriteLine("Data written to file: '{0}'", outputFilename);
            }
            finally
            {
                csvStream.Dispose();
                csvStream = null;
            }
        }

        public void GetCurrenciesCountriesStates()
        {
            // The call
            var iso = Quandl.NET.Quandl.GetISOCurrencyCodesAsync();
            iso.Wait();

            // Output: "Code: AFN, Country: AFGHANISTAN, Currency: Afghani; Code: ALL, Country: ALBANIA, Currency: Lek; Code: DZD, Country: ALGERIA, Currency: Algerian Dinar"
            Console.WriteLine(string.Join("; ", iso.Result.Select(c => $"Code: {c.AlphabeticCode}, Country: {c.Country}, Currency: {c.Currency}").Take(3)));
        }

        public Quandl.NET.Model.Response.DatabaseMetadataResponse GetMetaDataTimeSeriesDatabase(string databaseCode)
        {
            // The call
            var meta = m_client.Timeseries.GetDatabaseMetadataAsync(databaseCode);
            meta.Wait();

            // Output: "Name: Wiki EOD Stock Prices; Premium: False; DatasetsCount: 3187"
            Console.WriteLine($"Name: {meta.Result.Database.Name}; Premium: {meta.Result.Database.Premium}; DatasetsCount: {meta.Result.Database.DatasetsCount}");
            return meta.Result;
        }

        public string GetEntireTimeSeriesDatabase(string databaseCode, string outputZipFilename)
        {
            if (!outputZipFilename.EndsWith(".zip")) outputZipFilename += ".zip";       // ensure filename ends with ".zip"

            // Get an entire time-series database Reference
            // The call returns a zip stream with csv inside, you should use the csv after the zip is decompressed
            var stream = m_client.Timeseries.GetEntireDatabaseAsync(databaseCode, DownloadType.Full);
            stream.Wait();

            try
            {
                using (var fs = File.Create(string.Format(outputZipFilename)))
                {
                    stream.Result.CopyTo(fs);
                }
                Console.WriteLine("Entire Database {0} output to file: '{1}'", databaseCode, outputZipFilename);
            }
            finally
            {
                stream.Dispose();
                stream = null;
            }
            return outputZipFilename;
        }

        /*
        Get an entire time-series database Reference
        // The call returns a zip stream with csv inside, you should use the csv after the zip is decompressed
        using (var stream = await client.Timeseries.GetEntireDatabaseAsync("SCF", Model.Enum.DownloadType.Full))
        using (var fs = File.Create("someFileName.zip"))
        {
            stream.CopyTo(fs);
        }

        Get time-series data and metadata Reference
        // The call
        var data = await client.Timeseries.GetDataAndMetadataAsync("WIKI", "FB",
            columnIndex: 4,
            startDate: new DateTime(2014, 1, 1),
            endDate: new DateTime(2014, 12, 31),
            collapse: Collapse.Monthly,
            transform: Transform.Rdiff);
        // Output should be similar to Get time-series data api


        Tables Api

        Get table with filters Reference
        // Create row filter & column filter for the call, use ampersand(&) to separate each criteria for row Filter
        var rowFilter = "ticker=SPY,IWM,GLD&date>2014-01-07";
        var columnFilter = "ticker,date,shares_outstanding";
        // The call
        var result = await client.Tables.GetAsync("ETFG/FUND", rowFilter, columnFilter);
        // Output: "ticker; date; shares_outstanding"
        Console.WriteLine(string.Join("; ", result.Datatable.Columns.Select(c => c.Name)));
        // Output: "GLD; 2014-01-02; 264800000"
        Console.WriteLine(string.Join("; ", result.Datatable.Data.First()));
        
        Get table metadata Reference
        // The call
        var result = await client.Tables.GetMetadataAsync("AR/MWCS");
        // Output: "Name: MarketWorks Futures Settlement CME; Filters: code, date; Premium: True"
        Console.WriteLine($"Name: {result.Datatable.Name}; Filters: {string.Join(", ", result.Datatable.Filters)}; Premium: {result.Datatable.Premium}");

        Download an entire table Reference
        // The call returns a zip stream with csv inside, you should use the csv after the zip is decompressed
        using (var stream = await client.Tables.DownloadAsync("WIKI/PRICES"))
        using (var fs = File.Create("someFileName.zip"))
        {
            stream.CopyTo(fs);
        }


        Useful Data And Lists for Quandl

        Get List Of Index Constituents And The Corresponding Quandl Code
        // The call returns a list of S&P500 index constituents.
        // There are also calls for other indexes, including Dow Jones, NASDAQ Composite, NASDAQ 100, NYSE Composite, FTSE 100
        var result = await Quandl.GetSP500IndexConstituentsAsync();
        // Output: "MMM; ABT; ABBV; ACN; ACE; ATVI; ADBE; ADT; AAP; AES"
        Console.WriteLine(string.Join("; ", result.Take(10).Select((c => c.Ticker))));
        
        Get List Of Futures And The Corresponding Quandl Code
        // The call
        var result = await Quandl.GetFuturesMetadataAsync();
        // Output: "0D; 6T; 6Z; 7Q; 8I; 8Z; AD; AD; AFR; AG"
        Console.WriteLine(string.Join("; ", result.Take(10).Select(md => md.Symbol)));
        
        Get List Of Commodities And The Corresponding Quandl Code
        // The call
        var result = await Quandl.GetCommoditiesAsync();
        // Output: "Name: Milk, Non-Fat Dry, Chicago, Sector: Farms and Fisheries; Name: CME Milk Futures, Sector: Farms and Fisheries; Name: Cheddar Cheese, Barrels, Chicago, Sector: Farms and Fisheries"
        Console.WriteLine(string.Join("; ", result.Select(r => $"Name: {r.Name}, Sector: {r.Sector}").Take(10)));

        Get List Of Currencies, Countries and States
        // The call
        var result = await Quandl.GetISOCurrencyCodesAsync();
        // Output: "Code: AFN, Country: AFGHANISTAN, Currency: Afghani; Code: ALL, Country: ALBANIA, Currency: Lek; Code: DZD, Country: ALGERIA, Currency: Algerian Dinar"
        Console.WriteLine(string.Join("; ", result.Select(c => $"Code: {c.AlphabeticCode}, Country: {c.Country}, Currency: {c.Currency}").Take(3)));

        */

        public static void Print(Quandl.NET.Model.Response.TimeseriesDataResponse response)
        {
            Console.WriteLine(string.Join("; ", response.DatasetData.ColumnNames));
            Console.WriteLine(string.Join("; ", response.DatasetData.Data.First()));
            Console.WriteLine(string.Format("{0} rows total", response.DatasetData.Data.Count));
        }

    } // end of class MyQuandl



    public class MyQuandlCode
    {
        public string DatabaseCode { get; private set; }
        public string DatasetCode { get; private set; }

        public MyQuandlCode(string code)
        {
            string[] split = code.Split('/');
            this.DatabaseCode = split[0];
            this.DatasetCode = split[1]; 
        }
    } // end of class MyQuandlCode




} // end of namespace
