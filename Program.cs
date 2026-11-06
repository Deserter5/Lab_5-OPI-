using System;
using System.Text;

namespace LabWork5
{
    public class Person
    {
        private string _firstName;
        private string _lastName;
        private DateTime _birthDate;

        public Person(string firstName, string lastName, DateTime birthDate)
        {
            _firstName = firstName;
            _lastName = lastName;
            _birthDate = birthDate;
        }

        public Person() : this("Maksym", "Kovalenko", new DateTime(2004, 3, 15))
        {
        }

        public string FirstName
        {
            get => _firstName;
            set => _firstName = value;
        }

        public string LastName
        {
            get => _lastName;
            set => _lastName = value;
        }

        public DateTime BirthDate
        {
            get => _birthDate;
            set => _birthDate = value;
        }

        public int BirthYear
        {
            get => _birthDate.Year;
            set => _birthDate = new DateTime(value, _birthDate.Month, _birthDate.Day);
        }

        public override string ToString()
        {
            return $"{_firstName} {_lastName}, Date of birth: {_birthDate.ToShortDateString()}";
        }

        public virtual string ToShortString()
        {
            return $"{_firstName} {_lastName}";
        }
    }

    public enum Frequency
    {
        Weekly,
        Monthly,
        Yearly
    }

    public class Article
    {
        public Person Author { get; set; }
        public string Title { get; set; }
        public double Rating { get; set; }

        public Article(Person author, string title, double rating)
        {
            Author = author;
            Title = title;
            Rating = rating;
        }

        public Article() : this(new Person(), "Default Research", 5.0)
        {
        }

        public override string ToString()
        {
            return $"Article: \"{Title}\", Author: [{Author.ToShortString()}], Rating: {Rating:F1}";
        }
    }

    public class Magazine
    {
        private string _title;
        private Frequency _frequency;
        private DateTime _releaseDate;
        private int _circulation;
        private Article[] _articles;

        public Magazine(string title, Frequency frequency, DateTime releaseDate, int circulation)
        {
            _title = title;
            _frequency = frequency;
            _releaseDate = releaseDate;
            _circulation = circulation;
            _articles = new Article[0];
        }

        public Magazine() : this("Tech Horizons", Frequency.Monthly, new DateTime(2024, 1, 1), 5000)
        {
        }

        public string Title
        {
            get => _title;
            set => _title = value;
        }

        public Frequency Frequency
        {
            get => _frequency;
            set => _frequency = value;
        }

        public DateTime ReleaseDate
        {
            get => _releaseDate;
            set => _releaseDate = value;
        }

        public int Circulation
        {
            get => _circulation;
            set => _circulation = value;
        }

        public Article[] Articles
        {
            get => _articles;
            set => _articles = value ?? new Article[0];
        }

        public double AverageRating
        {
            get
            {
                if (_articles == null || _articles.Length == 0)
                    return 0.0;

                double sum = 0;
                foreach (var article in _articles)
                {
                    sum += article.Rating;
                }
                return sum / _articles.Length;
            }
        }

        public bool this[Frequency freq]
        {
            get => _frequency == freq;
        }

        public void AddArticles(params Article[] newArticles)
        {
            if (newArticles == null || newArticles.Length == 0) return;

            int oldLength = _articles?.Length ?? 0;
            Article[] temp = new Article[oldLength + newArticles.Length];

            if (_examsExist())
            {
                Array.Copy(_articles, temp, oldLength);
            }

            Array.Copy(newArticles, 0, temp, oldLength, newArticles.Length);
            _articles = temp;

            bool _examsExist() => _articles != null && oldLength > 0;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Magazine: \"{_title}\"");
            sb.AppendLine($"Frequency: {_frequency}, Release Date: {_releaseDate.ToShortDateString()}, Circulation: {_circulation}");
            sb.AppendLine("Articles list:");
            if (_articles == null || _articles.Length == 0)
            {
                sb.AppendLine("  [No articles yet]");
            }
            else
            {
                foreach (var article in _articles)
                {
                    sb.AppendLine($"  - {article}");
                }
            }
            sb.AppendLine($"Average rating: {AverageRating:F2}");
            return sb.ToString();
        }

        public virtual string ToShortString()
        {
            return $"Magazine: \"{_title}\", Frequency: {_frequency}, Date: {_releaseDate.ToShortDateString()}, Circulation: {_circulation}, Average rating: {AverageRating:F2}";
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine("Step 1: Default magazine and ToShortString()");
            Magazine magazine = new Magazine();
            Console.WriteLine(magazine.ToShortString());
            Console.WriteLine();

            Console.WriteLine("Step 2: Indexer check");
            Console.WriteLine($"Indexer [Weekly]: {magazine[Frequency.Weekly]}");
            Console.WriteLine($"Indexer [Monthly]: {magazine[Frequency.Monthly]}");
            Console.WriteLine($"Indexer [Yearly]: {magazine[Frequency.Yearly]}");
            Console.WriteLine();

            Console.WriteLine("Step 3: Modifying properties and ToString()");
            Person initialAuthor = new Person("Bohdan", "Melnyk", new DateTime(2004, 7, 21));

            magazine.Title = "Modern Science";
            magazine.Frequency = Frequency.Weekly;
            magazine.ReleaseDate = new DateTime(2024, 6, 1);
            magazine.Circulation = 12000;
            magazine.Articles = new Article[]
            {
                new Article(initialAuthor, "Intro to Machine Learning", 8.5)
            };

            Console.WriteLine(magazine.ToString());

            Console.WriteLine("Step 4: Adding articles via AddArticles()");
            Person author2 = new Person("Maksym", "Kovalenko", new DateTime(2004, 3, 15));

            Article art1 = new Article(initialAuthor, "Neural Networks in 2024", 9.4);
            Article art2 = new Article(author2, "Clean Architecture in C#", 8.8);

            magazine.AddArticles(art1, art2);
            Console.WriteLine(magazine.ToString());

            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();
        }
    }
}