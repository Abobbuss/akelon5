using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        Dictionary<string, List<DateTime>> vacationDictionary = new Dictionary<string, List<DateTime>>
        {
            {"Иванов Иван Иванович", new List<DateTime>()},
            {"Петров Петр Петрович", new List<DateTime>()},
            {"Сидоров Сидор Сидорович", new List<DateTime>()}
        };
        List<DateTime> aviableWorkingDaysOfWeekWithoutWeekends = GenerateWorkingDaysOfWeek();

        foreach (var kvp in vacationDictionary)
        {
            List<DateTime> vacation = GenerateRandomVacation(aviableWorkingDaysOfWeekWithoutWeekends, vacationDictionary);
            kvp.Value.AddRange(vacation);
        }

        Console.WriteLine("Отпуска сотрудников:");

        foreach (var kvp in vacationDictionary)
        {
            Console.WriteLine(kvp.Key + ":");

            foreach (var vacationDate in kvp.Value)
            {
                Console.WriteLine(" - " + vacationDate.ToShortDateString());
            }
        }

        Console.ReadLine();
    }

    static List<DateTime> GenerateRandomVacation(List<DateTime> aviableWorkingDaysOfWeekWithoutWeekends, Dictionary<string, List<DateTime>> vacationDictionary)
    {
        int totalVacationDays = 28;
        int shortVacationDuration = 7;
        int longVacationDuration = 14;

        List<DateTime> vacation = new List<DateTime>();
        Random random = new Random();

        DateTime startDate = aviableWorkingDaysOfWeekWithoutWeekends[random.Next(aviableWorkingDaysOfWeekWithoutWeekends.Count)].Date;
        startDate = GetValidStartDate(startDate, shortVacationDuration, vacation, vacationDictionary);

        while (vacation.Count < totalVacationDays)
        {
            int remainingDays = totalVacationDays - vacation.Count;
            int vacationDuration = remainingDays >= longVacationDuration ? (random.Next(2) == 0 ? shortVacationDuration : longVacationDuration) : shortVacationDuration;

            for (int i = 0; i < vacationDuration; i++)
            {
                vacation.Add(startDate.AddDays(i).Date);
            }

            startDate = GetValidStartDate(startDate.AddDays(vacationDuration), shortVacationDuration, vacation, vacationDictionary);
        }

        return vacation;
    }

    static DateTime GetValidStartDate(DateTime startDate, int duration, List<DateTime> vacation, Dictionary<string, List<DateTime>> vacationDictionary)
    {
        while (true)
        {
            bool intersectsWithOtherVacations = vacationDictionary.Any(kvp => kvp.Value.Any(vacationDate => startDate <= vacationDate && startDate.AddDays(duration - 1) >= vacationDate));

            if (intersectsWithOtherVacations)
            {
                startDate = startDate.AddDays(1);

                continue;
            }

            DateTime previousVacationEndDate = vacation.Any() ? vacation.Max() : DateTime.MinValue;

            if (previousVacationEndDate != DateTime.MinValue && startDate < previousVacationEndDate.AddMonths(1))
            {
                startDate = previousVacationEndDate.AddMonths(1);

                continue;
            }

            return startDate;
        }
    }

    static List<DateTime> GenerateWorkingDaysOfWeek()
    {
        const int CurrentYear = 2024;

        List<DateTime> workingDaysOfWeek = new List<DateTime>();
        DateTime currentDate = new DateTime(CurrentYear, 1, 1);

        while (currentDate.Year == CurrentYear)
        {
            if (currentDate.DayOfWeek != DayOfWeek.Saturday && currentDate.DayOfWeek != DayOfWeek.Sunday)
                workingDaysOfWeek.Add(currentDate.Date);

            currentDate = currentDate.AddDays(1);
        }

        return workingDaysOfWeek;
    }
}