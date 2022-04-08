using System;
using System.Text.Json;

namespace tpmodul7_1302204058
{
    class program
    {
        static void Main(string[] args)
        {
            UIconfig obj = new UIconfig();
            Console.WriteLine("Berapa suhu badan anda saat ini? Dalam nilai " + obj.config.satuan_suhu);
            string suhuStr = Console.ReadLine();
            double suhu = double.Parse(suhuStr);

            Console.WriteLine("Berapa hari yang lalu (perkiraan) anda terakhir memiliki gejala deman?");
            string perkiraanStr = Console.ReadLine();
            int perkiraan = Int16.Parse(perkiraanStr);

            if (obj.config.satuan_suhu == "celcius" && suhu >= 36.5 && suhu <= 37.5 && perkiraan <= obj.config.batas_hari_deman)
            {
                Console.WriteLine(obj.config.pesan_diterima);
            }
            else if (obj.config.satuan_suhu == "fahrenheit" && suhu >= 97.7 && suhu <= 99.5 && perkiraan <= obj.config.batas_hari_deman)
            {
                Console.WriteLine(obj.config.pesan_diterima);
            }
            else
            {
                Console.WriteLine(obj.config.pesan_diterima);
            }

            CovidConfig cov = new CovidConfig(obj.config.satuan_suhu, perkiraan, obj.config.pesan_diterima, obj.config.pesan_diterima);
            cov.UbahSatuan();
            Console.WriteLine(cov.satuan_suhu);
        }
    }

    public class UIconfig
    {
        public CovidConfig config;
        public string filePath = Directory.GetCurrentDirectory() + "/covid_config.json";
        public UIconfig()
        {
            try
            {
                ReadConfigFile();
            }
            catch (Exception)
            {
                SetDefault();
                WriteNewConfigFile();
            }
        }
        private void SetDefault()
        {
            config = new CovidConfig("celcius", 14, "Anda tidak diperbolehkan masuk ke dalam gedung ini", "Anda dipersilahkan untuk masuk ke dalam gedung ini");
        }
        private CovidConfig ReadConfigFile()
        {
            String configJsonData = File.ReadAllText(filePath);
            config = JsonSerializer.Deserialize<CovidConfig>(configJsonData);
            return config;
        }
        private void WriteNewConfigFile()
        {
            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                WriteIndented = true
            };
            String jsonString = JsonSerializer.Serialize(config, options);
            File.WriteAllText(filePath, jsonString);
        }
    }

    public class CovidConfig
    {
        public string satuan_suhu { get; set; }
        public int batas_hari_deman { get; set; }
        public string pesan_ditolak { get; set; }
        public string pesan_diterima { get; set; }
        public CovidConfig(string stnSuhu, int btsHrDm, string psnDtlk, string psnDtrm)
        {
            this.satuan_suhu = stnSuhu;
            this.batas_hari_deman = btsHrDm;
            this.pesan_ditolak = psnDtlk;
            this.pesan_diterima = psnDtrm;
        }
        public void UbahSatuan()
        {
            if (this.satuan_suhu == "celcius")
            {
                this.satuan_suhu = "fahrenheit";
            }
            else if (this.satuan_suhu == "fahrenheit")
            {
                this.satuan_suhu = "celcius";
            }
        }
    }
}
