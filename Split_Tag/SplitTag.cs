using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Text;

/*                  90--HBK--10--CP--103
 *                unit--[tag1]--[tag2]--[process_id]--[tag3] 
 */
namespace Split_Tag
{
    internal static partial class Program
    {
        private static List<string> SplitTag(string str, int qty)
        {
            //Final target list of this program
            var finalInstrumentTagList = new List<string>();
            if ((str.Length >= 10) & (str.Length <= 12))
            {
                finalInstrumentTagList.Add(str);
                return finalInstrumentTagList;
            }

            //Variable to hold Process ID of the test string
            var processId = "";

            //Tag2 Extrapolated Values. Eg. 01-05 => 01,02,03,04,05
            var tag2ListExtrapolated = new List<string>();
            //Tag3 Extrapolated Values. Eg. 101-105 => 101,102,103,104,105
            var tag3ListExtrapolated = new List<string>();
            //List of Possible Process IDs the program can encounter
            string[] fn = {"CP", "CF", "TE", "CL", "CT", "CQ", "CS"};
            int tag2Start, tag2End; // Tag 2 Start Part & End part
            int tag3Start, tag3End; // Tag 3 Start Part & End part
            str = str.Replace(@" ", string.Empty); //Remove whitespace
            //Find the process ID of the given instrument tag no
            foreach (var s in fn)
                if (str.Contains(s))
                {
                    processId = s;
                    break;
                }

            //If the program couldn't find the process ID, display error
            if (processId.Equals(string.Empty)) throw new InvalidProcessIdException();

            //Split the Instrument tag using process ID as reference
            var tag = Regex.Split(str, processId);
            //Properly assign extracted strings in proper variable names for easy reference
            //Extract the unit number from the tag 0
            var unitStr = new StringBuilder();
            for (var i = 0; i <= 1; i++)
                if (char.IsDigit(tag[0], i))
                    unitStr.Append(tag[0][i]);
            var unit = string.Empty;
            //Trim the Unit number from the instrument tag
            if (unitStr.Length != 0)
            {
                unit = unitStr.ToString();
                tag[0] = tag[0].Replace(unit, "");
            }

            var tag1 = tag[0].Substring(0, 3);
            var tag2 = tag[0].Remove(0, 3);
            var tag3 = tag[1];

            //Process Tag3 for Multiple Values
            //Check for "-" character in tag no
            //Tag3 was processed initially before tag 2 in order to determine the actual quantity from the requested qty
            if (tag3.Contains("-"))
            {
                _tag3List = Regex.Split(tag3, "-");
                int.TryParse(_tag3List[0], out tag3Start);
                int.TryParse(_tag3List[1], out tag3End);
                if (tag3Start > tag3End) throw new Tag3InvalidDelimiterException();
                for (var i = tag3Start; i <= tag3End; i++) tag3ListExtrapolated.Add(i.ToString("000"));
            }
            else if (tag3.Contains("&"))
            {
                _tag3List = Regex.Split(tag3, "&");
                int.TryParse(_tag3List[0], out tag3Start);
                int.TryParse(_tag3List[1], out tag3End);
                if (tag3Start > tag3End) throw new Tag3InvalidDelimiterException();
                tag3ListExtrapolated.Add(tag3Start.ToString("000"));
                tag3ListExtrapolated.Add(tag3End.ToString("000"));
            }
            else if (Regex.IsMatch(tag3, @"\d"))
            {
                tag3ListExtrapolated.Add(tag3);
            }
            else
            {
                throw new Tag3InvalidDelimiterException();
            }

            //Process Tag2 for Multiple Values
            //Check for "-" character in tag no
            if (tag2.Contains("-"))
            {
                _tag2List = Regex.Split(tag2, "-");
                int.TryParse(_tag2List[0], out tag2Start);
                int.TryParse(_tag2List[1], out tag2End);
                if (tag2Start > tag2End) throw new Tag2InvalidDelimiterException();
                //Extrapolate the strings
                for (var i = tag2Start; i <= tag2End; i++) tag2ListExtrapolated.Add(i.ToString("00"));
                if (tag2ListExtrapolated.Count > qty / tag3ListExtrapolated.Count)
                {
                    if (qty / tag3ListExtrapolated.Count == 1) throw new QuantityMismatchException();
                    tag2ListExtrapolated.Clear();
                    var incFactor = (tag2End - tag2Start) / (qty / tag3ListExtrapolated.Count - 1);
                    for (var i = tag2Start; i <= tag2End; i += incFactor) tag2ListExtrapolated.Add(i.ToString("00"));
                    if (qty != tag2ListExtrapolated.Count * tag3ListExtrapolated.Count)
                        throw new QuantityMismatchException();
                }
            }
            else if (tag2.Contains("&"))
            {
                _tag2List = Regex.Split(tag2, "&");
                int.TryParse(_tag2List[0], out tag2Start);
                int.TryParse(_tag2List[1], out tag2End);
                if (tag2Start > tag2End) throw new Tag2InvalidDelimiterException();
                //This list contains only two nos of tags only. Hence included directly
                tag2ListExtrapolated.Add(tag2Start.ToString("00"));
                tag2ListExtrapolated.Add(tag2End.ToString("00"));
            }
            else if (Regex.IsMatch(tag2, @"\d"))
            {
                tag2ListExtrapolated.Add(tag2);
            }
            else
            {
                throw new Tag3InvalidDelimiterException();
            }

            //Build Instrument tag strings
            foreach (var s1 in tag2ListExtrapolated)
            foreach (var s2 in tag3ListExtrapolated)
                finalInstrumentTagList.Add(unit + tag1 + s1 + processId + s2);
            return finalInstrumentTagList;
        }
    }

    internal class InvalidProcessIdException : Exception
    {
    }

    internal class Tag2InvalidDelimiterException : Exception
    {
    }

    internal class Tag3InvalidDelimiterException : Exception
    {
    }

    internal class QuantityMismatchException : Exception
    {
    }

    internal class InvalidTag2Exception : Exception
    {
    }

    internal class InvalidTag3Exception : Exception
    {
    }
}