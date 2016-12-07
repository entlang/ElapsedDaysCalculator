using System;

namespace ElapsedDaysCalculator
{
    internal class Program
    {
        private static readonly int[] daysPerMonth = {31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31};

        private static void Main(string[] args)
        {
            var validStartDate = false;
            var startDay = 0;
            var startMonth = 0;
            var startYear = 0;

            var validEndDate = false;
            var endtDay = 0;
            var endMonth = 0;
            var endYear = 0;

            while (!validStartDate)
            {
                var startDate = PromptForStartDate();

                if (startDate != null)
                {
                    validStartDate = ParseDate(startDate, out startDay, out startMonth, out startYear);
                }
            }

            while (!validEndDate)
            {
                var startDate = PromptForEndDate();

                if (startDate != null)
                {
                    validEndDate = ParseDate(startDate, out endtDay, out endMonth, out endYear);
                }
            }


            var dayDifference = 0;
            var swap = SwapDates(startYear, endYear, startMonth, endMonth, startDay, endtDay);
            if (swap)
            {
                dayDifference = YearDiffInDays(endYear, startYear) -
                                DaysFromStartOfTheYear(endtDay, endMonth, endYear) +
                                DaysFromStartOfTheYear(startDay, startMonth, startYear);
            }
            else
            {
                dayDifference = YearDiffInDays(startYear, endYear) -
                                DaysFromStartOfTheYear(startDay, startMonth, startYear) +
                                DaysFromStartOfTheYear(endtDay, endMonth, endYear);
            }

            if (dayDifference > 0)
            {
                dayDifference--;
            }

            Console.WriteLine("DIfference is {0} days.", dayDifference);
        }

        private static string PromptForStartDate()
        {
            Console.WriteLine("Please enter START date in this format DD/MM/YYYY");
            return Console.ReadLine();
        }

        private static string PromptForEndDate()
        {
            Console.WriteLine("Please enter END date in this format DD/MM/YYYY");
            return Console.ReadLine();
        }

        private static bool ParseDate(string dateString, out int day, out int month, out int year)
        {
            day = 0;
            month = 0;
            year = 0;

            var splitdata = dateString.Split('/');

            if (splitdata.Length != 3)
            {
                Console.WriteLine("Date must follow this format DD/MM/YYYY");
                return false;
            }

            //day validation
            try
            {
                if (!string.IsNullOrEmpty(splitdata[0]))
                {
                    if (splitdata[0].Length != 2)
                    {
                        Console.WriteLine("Day must have this format DD");
                        return false;
                    }
                    day = int.Parse(splitdata[0]);
                }
                else
                {
                    Console.WriteLine("Day is missing");
                    return false;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Day is not number");
                return false;
            }


            //month validation
            try
            {
                if (!string.IsNullOrEmpty(splitdata[1]))
                {
                    if (splitdata[1].Length != 2)
                    {
                        Console.WriteLine("Month must have this format MM");
                        return false;
                    }
                    month = int.Parse(splitdata[1]);
                }
                else
                {
                    Console.WriteLine("Month is missing");
                    return false;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Month is not number");
                return false;
            }

            if (month < 1 || month > 12)
            {
                Console.WriteLine("Month must be between 1 and 12");
                return false;
            }

            //year validation
            try
            {
                if (!string.IsNullOrEmpty(splitdata[2]))
                {
                    if (splitdata[2].Length != 4)
                    {
                        Console.WriteLine("Year must have this format YYYY");
                        return false;
                    }
                    year = int.Parse(splitdata[2]);
                }
                else
                {
                    Console.WriteLine("Year is missing");
                    return false;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Year is not number");
                return false;
            }
            if (!IsValidYear(year))
            {
                Console.WriteLine("Year must be in between 1901 and 2999");
                return false;
            }

            var maxNUmberOfDays = GetMaxDaysInMonth(month, year);
            if (day <= 0)
            {
                Console.WriteLine("Day must start from 1 in format DD");
                return false;
            }
            if (day > maxNUmberOfDays)
            {
                Console.WriteLine("Max number of days is: {0}", maxNUmberOfDays);
                return false;
            }

            return true;
        }

        //helper methods
        private static bool IsValidYear(int year)
        {
            return year >= 1091 && year <= 2999;
        }

        private static bool IsLeapYear(int year)
        {
            if (year%400 == 0)
            {
                return true;
            }
            if ((year%100 != 0) && (year%4 == 0))
            {
                return true;
            }
            return false;
        }

        private static int GetMaxDaysInMonth(int month, int year)
        {
            var sum = 0;

            if (month <= 0 || month > 12) return sum;
            sum += daysPerMonth[month - 1];

            if (month == 2 && IsLeapYear(year))
            {
                sum++;
            }

            return sum;
        }

        private static int NumberOfLeaps(int startYear, int endYear)
        {
            var sum = 0;

            for (var i = startYear; i <= endYear; i++)
            {
                if (IsLeapYear(i))
                {
                    sum++;
                }
            }

            return sum;
        }

        private static int YearDiffInDays(int startYear, int endYear)
        {
            if (startYear == endYear) return 0;

            return (endYear - startYear)*365 + NumberOfLeaps(startYear, endYear - 1);
        }

        private static int DaysFromStartOfTheYear(int day, int month, int year)
        {
            return DaysForPreviousMonths(month, year) + day;
        }

        private static int DaysForPreviousMonths(int month, int year)
        {
            var count = 0;

            //exclude current month
            for (var i = month - 1; i > 0; i--)
            {
                count += GetMaxDaysInMonth(i, year);
            }
            return count;
        }

        private static bool SwapDates(int startYear, int endYear, int startMonth, int endMonth, int startDay, int endDay)
        {
            if (startYear > endYear)
            {
                return true;
            }

            if (startYear != endYear) return false;

            if (startMonth > endMonth)
            {
                return true;
            }

            if (startMonth != endMonth) return false;

            return startDay > endDay;
        }
    }
}