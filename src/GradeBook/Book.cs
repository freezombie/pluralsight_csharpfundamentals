using System;
using System.Collections.Generic;
using System.IO;

namespace GradeBook
{
    public delegate void GradeAddedDelegate(object sender, EventArgs args); // aina sender ekana. tai se on se convention

    public class NamedObject
    {
        public NamedObject(string name)
        {
            Name = name;
        }

        public string Name
        {
            get;
            set;
        }
    }

    public interface IBook
    {
        void AddGrade(double grade);
        void ShowStatistics();
        Statistics GetStatistics();
        string Name { get; }
        event GradeAddedDelegate GradeAdded;
    }

    public abstract class Book : NamedObject, IBook
    {
        public Book(string name) : base(name)
        {
        }

        public abstract event GradeAddedDelegate GradeAdded;
        public abstract void AddGrade(double grade);
        public abstract Statistics GetStatistics();
        internal List<double> grades;
        internal double total;

        public void ShowStatistics()
        {
            Statistics stats = GetStatistics();
            Console.WriteLine($"Average Grade: {stats.Average:N2}");
            Console.WriteLine($"Highest Grade: {stats.High:N2}");
            Console.WriteLine($"Lowest Grade: {stats.Low:N2}");
            Console.WriteLine($"Letter Grade: {stats.Letter}");
        }
    }

    public class DiskBook : Book
    {
        public DiskBook(string name) : base(name)
        {
            grades = new List<double>();
            Name = name;
        }

        public override event GradeAddedDelegate GradeAdded;

        public override void AddGrade(double grade)
        {
            using (StreamWriter sw = File.AppendText($"{Name}.txt"))
            {
                sw.WriteLine(grade);
                if (GradeAdded != null)
                {
                    GradeAdded(this, new EventArgs());
                }
            }
            // ylempi varmistaa että closetaan/disposetaan käyttämisen jälkeen            
            //sw.Close();
            //sw.Dispose();
        }

        public override Statistics GetStatistics()
        {
            var result = new Statistics();

            using (StreamReader sr = File.OpenText($"{Name}.txt"))
            {
                while (sr.Peek() > -1)
                {
                    result.Add(double.Parse(sr.ReadLine()));
                }
                /*
                tässä kurssin oma
                var line = reader.ReadLine();
                while(line != null)
                {
                    var number = double.Parse(line);
                    result.Add(number);
                    line = reader.ReadLine();
                }
                */
            }

            return result;
        }
    }
    public class InMemoryBook : Book
    {
        public InMemoryBook(string name) : base(name)
        {
            // ylh. base() sanoo että käytä inherited luokan constructoria, ja lähetä sille asioita mitä se tarvii.
            grades = new List<double>();
            Name = name;
        }

        public void AddGrade(char letter)
        {
            switch (letter)
            {
                case 'A':
                    AddGrade(90);
                    break;
                case 'B':
                    AddGrade(80);
                    break;
                case 'C':
                    AddGrade(70);
                    break;
                case 'D':
                    AddGrade(60);
                    break;
                default:
                    AddGrade(0);
                    break;
            }
        }
        public override void AddGrade(double grade)
        {
            if (grade <= 100 && grade >= 0)
            {
                total += grade;
                grades.Add(grade);
                if (GradeAdded != null) // kuunteleeko kukaan.
                {
                    GradeAdded(this, new EventArgs());
                }
            }
            else
            {
                throw new ArgumentException($"Invalid {nameof(grade)}");
            }
        }

        public override event GradeAddedDelegate GradeAdded;

        public override Statistics GetStatistics()
        {
            var result = new Statistics();

            for (int i = 0; i < grades.Count; i += 1)
            {
                result.Add(grades[i]);
            }

            return result;
        }

        // readonly toimii vain constructorissa ja initializessa.
        readonly string category = "Science";
    }
}