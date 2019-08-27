using System;

namespace Split_Tag
{
    internal static partial class Program
    {
        private static string[] _tag2List, _tag3List;

        public static void Main()
        {
            //Test String
            const string str = "91HBK01-10CP001";
            try
            {
                var finalInstrumentTagList = SplitTag(str, 1);
                //Display the Final Instrument Tag list
                finalInstrumentTagList.ForEach(Console.WriteLine);
                Console.ReadLine();
            }
            catch (InvalidProcessIdException e)
            {
                Console.WriteLine("Invalid Process ID Error");
                Console.ReadKey();
            }
            catch (Tag2InvalidDelimiterException e)
            {
                Console.WriteLine("Invalid Delimiter found in Tag 2 Section");
                Console.ReadKey();
            }
            catch (Tag3InvalidDelimiterException e)
            {
                Console.WriteLine("Invalid Delimiter found in Tag 3 Section");
                Console.ReadKey();
            }
            catch (QuantityMismatchException e)
            {
                Console.WriteLine("Entered Quantity is not matching with the Tag nos");
                Console.ReadKey();
            }
            catch (InvalidTag2Exception e)
            {
                Console.WriteLine("Tag 2 Section Contains Invalid Entries");
                Console.ReadKey();
            }
            catch (InvalidTag3Exception e)
            {
                Console.WriteLine("Tag 3 Section Contains Invalid Entries");
                Console.ReadKey();
            }
        }
    }
}