namespace QienUrenMachien.Translation
{
    public static class Translator
    {
        public static string TranslateDay(string day)
        {
            if (day == "Monday"){return "Maandag";}
            else if (day == "Tuesday") {return "Dinsdag";}
            else if (day == "Wednesday") {return "Woensdag";}
            else if (day == "Thursday") {return "Donderdag";}
            else if (day == "Friday") {return "Vrijdag";}
            else if (day == "Saturday") {return "Zaterdag";}
            else if (day == "Sunday") {return "Zondag";}
            else {return "Onjuiste dag";}
        }

        public static string TranslateMonth(string month)
        {
            if (month == "January"){return "Januari";}
            else if (month == "February") {return "Februari";}
            else if (month == "March") {return "Maart";}
            else if (month == "April") {return "April";}
            else if (month == "May") {return "Mei";}
            else if (month == "June") {return "Juni";}
            else if (month == "July") {return "Juli";}
            else if (month == "August") {return "Augustus";}
            else if (month == "September") {return "September";}
            else if (month == "October") {return "Oktober";}
            else if (month == "November") {return "November";}
            else if (month == "December") {return "December";}
            else {return "Onjuiste maand";}
        }
    }
}