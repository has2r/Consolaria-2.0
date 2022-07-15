using System;

namespace Consolaria.Common {
    public static class SeasonalEventsHelper {
        public static bool CheckEaster () {
            DateTime now = DateTime.Now;
            int day = now.Day;
            int month = now.Month;
            if (day >= 1 && month == 4)
                return true;
            return false;
        }

        public static bool CheckThanksgiving () {
            DateTime now = DateTime.Now;
            int day = now.Day;
            int month = now.Month;
            if (day >= 1 && month == 11)
                return true;
            return false;
        }

        public static bool CheckChineseNewYear () {
            DateTime now = DateTime.Now;
            int day = now.Day;
            int month = now.Month;
            if ((day >= 20 && month == 1) && (day <= 15 && month == 2))
                return true;
            return false;
        }

        public static bool CheckOktoberfest () {
            DateTime now = DateTime.Now;
            int day = now.Day;
            int month = now.Month;
            if ((day >= 27 && month == 9) && (day <= 31 && month == 10))
                return true;
            return false;
        }

        public static bool CheckSaintPatricksDay () {
            DateTime now = DateTime.Now;
            int day = now.Day;
            int month = now.Month;
            if ((day >= 5 && month == 3) && (day <= 31 && month == 3))
                return true;
            return false;
        }

        public static bool CheckValentinesDay () {
            DateTime now = DateTime.Now;
            int day = now.Day;
            int month = now.Month;
            if ((day >= 1 && month == 2) && (day <= 29 && month == 2))
                return true;
            return false;
        }
    }
}