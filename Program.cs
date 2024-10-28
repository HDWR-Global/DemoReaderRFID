using SReaderAPI;

namespace ReadTag
{
    class Program
    {
        static void Main(string[] args)
        {
            SReader reader = null;
            try
            {

                reader = SReader.Create("tcp://192.168.1.136"); //tcp connect

                reader.Connect();

                var info = reader.GetReaderInfo();
                if (info.getCurrentRegion() != (int)SReader.Region.EU)
                {
                    reader.SetRegion(SReader.Region.EU);
                }
                if (info.Power != 26)
                    reader.SetReaderPower(26);

                reader.SetBuzzer(1);

                reader.TagRead += TagRead;

                Gen2.InventryValue value = new Gen2.InventryValue(4, 1);
                while (true)
                {
                    reader.Inventry(value, null);
                }
            }
            catch (ReaderException ex)
            {
                Console.WriteLine("Error:" + ex.Message);
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error:" + e.Message);
                Console.ReadLine();
            }
            finally
            {
                reader.ShutDown();
            }
        }

        static public void TagRead(Object sender, TagReadDataEventArgs e)
        {
            string epc = e.TagData.EpcString;
            int ant = e.TagData.Ant;
            int rssi = e.TagData.Rssi;
            Console.WriteLine("epc:" + epc + " ant:" + ant + " rssi:" + rssi);
        }
    }
}
