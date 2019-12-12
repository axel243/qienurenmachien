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
            else {return "";}
        }
    }
}