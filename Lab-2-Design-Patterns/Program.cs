using System.Data;
using System.Transactions;

Departament departament = new Departament("Lnu goverment");
Enrollment enrollment = new Enrollment(departament);

Course course = new Course("Course #1");
Student student = new Student(1, "Amigo");

Console.WriteLine(student.AskForLeave(departament, new Request(1, "Please let me leave, i fill myself not well today"))); ;

Course course1 = new Course("Course #2");
Student student2 = new Student(2, "Amigos");
Student student3 = new Student(3, "Serhiy");
Student student4 = new Student(4, "Olehandro");
Student student5 = new Student(5, "Vitaliy");
Student student6 = new Student(6, "Dima");

Professor professor = new Professor(1, "Alexis", 228);
Console.WriteLine(professor.AskForLeave(departament, new Request(2, "Hehe siuuuu")));

enrollment.Enroll(student, course, professor);
enrollment.Enroll(student, course, professor); // Добавляєм того самого студента в той самий курс
enrollment.Enroll(student3, course, professor);
enrollment.Enroll(student4, course, professor);
enrollment.Enroll(student5, course, professor);
enrollment.Enroll(student6, course, professor);
enrollment.Enroll(student2, course, professor);
enrollment.Enroll(student, course1, professor);

Seminar seminar = new Seminar(1, "Working in groups");
professor.AddSeminar(seminar, course);
professor.AddSeminar(seminar, course1);
professor.AddAsignmentToSeminar(seminar, "Do single page application");

Seminar seminar2 = new Seminar(2, "Solo project");
professor.AddSeminar(seminar2, course);
professor.AddSeminar(seminar2, course1);
professor.AddAsignmentToSeminar(seminar2, "Do restful api");

enrollment.Unenroll(student, course);
enrollment.Unenroll(student, course); // Enroll вже енролнутого студента

enrollment.ShowAllInfo();
Console.WriteLine();
departament.ShowInfo();

public abstract class Staff
{
    public abstract PersonalInfo PersonalInfo { get; set; }
    public abstract bool AskForLeave(Departament departament, Request request);
    public abstract bool SendRequest(Departament departament, Request request);
}

public class Seminar
{
    public Seminar(int id, string title)
    {
        Id = id;
        Title = title;
    }

    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public Dictionary<string, bool> Assignments { get; set; } = new Dictionary<string, bool>();
    public string CourseName { get; set; } = string.Empty;
}

public class PersonalInfo
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public float Salary { get; set; } = 0;
    public string Email { get; set; } = string.Empty;
}

public class Student : Staff
{
    public Student(int id, string name)
    {
        this.PersonalInfo.Id = id;
        this.PersonalInfo.Name = name;
    }

    public bool IsPostGraduate { get; set; }

    public override PersonalInfo PersonalInfo { get; set; } = new PersonalInfo();

    public override bool AskForLeave(Departament departament, Request request)
    {
        if (departament.Requests.ContainsKey(request.Id) == false)
        {
            departament.Requests.Add(request.Id, request);
            return departament.ProceedRequest(request.Id);
        }

        return departament.ProceedRequest(request.Id);
    }

    public override bool SendRequest(Departament departament, Request request)
    {
        if (departament.Requests.ContainsKey(request.Id) == false)
        {
            departament.Requests.Add(request.Id, request);
            return departament.ProceedRequest(request.Id);
        }

        return departament.ProceedRequest(request.Id);
    }
}

public class Professor : Staff
{
    public Professor(int id, string name, float salary)
    {
        this.PersonalInfo.Id = id;
        this.PersonalInfo.Name = name;
        this.PersonalInfo.Salary = salary;
    }

    public void AddPostGraduateStudent(Student student)
    {
        Console.WriteLine($"Student {student.PersonalInfo.Name} postgradueted");
        student.IsPostGraduate = true;
    }

    public override bool AskForLeave(Departament departament, Request request)
    {
        if (departament.Requests.ContainsKey(request.Id) == false)
        {
            departament.Requests.Add(request.Id, request);
            return departament.ProceedRequest(request.Id);
        }

        return departament.ProceedRequest(request.Id);
    }

    public bool AddSeminar(Seminar seminar, Course course)
    {
        if (course.Seminars.ContainsKey(seminar.Id) == false)
        {
            seminar.CourseName = course.Title;
            course.Seminars.Add(seminar.Id, seminar);
            Console.WriteLine($"Seminar - {seminar.Title} added succesfully to {course.Title}");
            return true;
        }
        Console.WriteLine($"Seminar - {seminar.Title} has already added to {course.Title}");
        return false;

    }

    public void AddAsignmentToSeminar(Seminar seminar, string assignment)
    {
        if (seminar.Assignments.ContainsKey(assignment) == false)
        {
            Console.WriteLine($"Assignment - {assignment} succeffully added to Seminar - {seminar.Title}");
            seminar.Assignments.Add(assignment, false);
            return;
        }

        Console.WriteLine($"Assignment - {assignment} has alredy added to Seminar - {seminar.Title}");
    }

    public override bool SendRequest(Departament departament, Request request)
    {
        if (departament.Requests.ContainsKey(request.Id) == false)
        {
            departament.Requests.Add(request.Id, request);
            return departament.ProceedRequest(request.Id);
        }

        return departament.ProceedRequest(request.Id);
    }

    public override PersonalInfo PersonalInfo { get; set; } = new PersonalInfo();
}

public class Request
{
    public Request(int id, string message)
    {
        this.Id = id;
        this.Message = message;
    }
    public int Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool IsDone { get; set; } = false;
}

public class Departament
{
    public Departament(string title)
    {
        this.Title = title;
    }

    public string Title { get; set; } = string.Empty;
    public List<Student> Students { get; set; } = new List<Student>();
    public List<Professor> Professors { get; set; } = new List<Professor>();
    public List<string> CoursesByTitle { get; set; } = new List<string>();
    public Dictionary<int, Request> Requests { get; set; } = new Dictionary<int, Request>();

    public bool ProceedRequests()
    {
        var newList = this.Requests.Where(x => x.Value.IsDone == false);
        if (newList != null)
        {
            foreach (var item in newList.ToList())
                this.Requests[item.Key].IsDone = true;

            return true;
        }

        return false;
    }

    public bool ProceedRequest(int requestId)
    {
        if (this.Requests.ContainsKey(requestId))
        {
            int number = new Random().Next(1, 3);
            switch (number)
            {
                case 1:
                    this.Requests[requestId].IsDone = true;
                    break;
                case 2:
                    this.Requests[requestId].IsDone = false;
                    break;
            }

            return this.Requests[requestId].IsDone;
        }

        return false;
    }

    public void ShowInfo()
    {
        Console.WriteLine($"Departament {this.Title} info");
        Console.WriteLine("Students: ");
        foreach (var item in this.Students)
        {
            Console.WriteLine($"\t{item.PersonalInfo.Name}");
        }
        Console.WriteLine();
        Console.WriteLine("Professors: ");
        foreach (var item in this.Professors)
        {
            Console.WriteLine($"\t{item.PersonalInfo.Name}");
        }
        Console.WriteLine();
        Console.WriteLine("Courses: ");
        foreach (var item in this.CoursesByTitle)
        {
            Console.WriteLine($"\t{item}");
        }

        Console.WriteLine("Requests: ");
        foreach (var item in this.Requests)
        {
            Console.WriteLine($"\t{item.Key} - Message: {item.Value.Message}, Is done: {item.Value.IsDone}");
        }
    }
}

public class Course
{
    public Course(string title)
    {
        Title = title;
    }

    public string Title { get; set; } = string.Empty;
    public Dictionary<int, Seminar> Seminars { get; set; } = new Dictionary<int, Seminar>();

}

public class Enrollment
{
    public Enrollment(Departament departament)
    {
        this.Departament = departament;
    }

    private List<Course> Courses { get; set; } = new List<Course>();
    private List<Student> Students { get; set; } = new List<Student>();
    private List<Professor> Professors { get; set; } = new List<Professor>();

    public Departament Departament { get; set; } = new Departament("");
    public Dictionary<string, Professor> CourseAndProfessor { get; set; } = new Dictionary<string, Professor>();
    public Dictionary<int, List<Course>> ProfessorAndCourses { get; set; } = new Dictionary<int, List<Course>>();

    public Dictionary<string, List<Student>> CourseStudents { get; set; } = new Dictionary<string, List<Student>>();
    public Dictionary<int, List<Course>> StudentsCourse { get; set; } = new Dictionary<int, List<Course>>();

    public void Enroll(Student student, Course course, Professor professor)
    {
        if (this.CourseStudents.AddCustom(course.Title, student) && this.StudentsCourse.AddCustom(student.PersonalInfo.Id, course))
        {
            Console.WriteLine($"Student {student.PersonalInfo.Name} successfully enroled on course - {course.Title}");
            if (this.Students.Contains(student) == false)
                this.Students.Add(student);

            if (this.Courses.Contains(course) == false)
                this.Courses.Add(course);
        }
        else
            Console.WriteLine($"Student has already enroled on this course");


        this.ProfessorAndCourses.AddCustom(professor.PersonalInfo.Id, course);
        this.Professors.Add(professor);

        this.CourseAndProfessor.AddCustom(course.Title, professor);


        if (this.Departament.Students.Contains(student) == false)
            this.Departament.Students.Add(student);

        if (this.Departament.CoursesByTitle.Contains(course.Title) == false)
            this.Departament.CoursesByTitle.Add(course.Title);

        if (this.Departament.Professors.Contains(professor) == false)
            this.Departament.Professors.Add(professor);
    }

    public void Unenroll(Student student, Course course)
    {
        if (this.CourseStudents.RemoveCustom(course.Title, student) && this.StudentsCourse.RemoveCustom(student.PersonalInfo.Id, course))
            Console.WriteLine($"Student {student.PersonalInfo.Name} successfully unenrolled");
        else
            Console.WriteLine($"Student or course not found :(");
    }

    public void ShowAllInfo()
    {
        Console.WriteLine("Courses and his students");
        foreach (var item in this.CourseStudents.Keys)
        {
            Console.WriteLine($"\tCourse title {item}");
            Console.WriteLine($"\tProfessor of course: Name - " +
                $"{this.CourseAndProfessor[item].PersonalInfo.Name} " +
                $"| Salary - {this.CourseAndProfessor[item].PersonalInfo.Salary}");


            Console.WriteLine("\tSeminars And His Assignments");
            foreach (var seminar in this.Courses.Where(x => x.Title == item).FirstOrDefault().Seminars)
            {
                Console.WriteLine($"\t\tId: {seminar.Value.Id}, Title: {seminar.Value.Title}");
                Console.WriteLine($"\t\tAssignments: ");

                foreach (var assignment in seminar.Value.Assignments)
                {
                    Console.WriteLine($"\t\t\t{assignment.Value} - {assignment.Key}");
                }
            }
            Console.WriteLine("\t\tStudents of this course:");
            foreach (var student in this.CourseStudents[item])
            {
                Console.WriteLine($"\t\t\tId: {student.PersonalInfo.Id}, Name: {student.PersonalInfo.Name}");
            }
        }
    }
}

public static class DictionaryExtensions
{
    public static bool AddCustom(this Dictionary<int, List<Course>> targetDictionary, int key, Course course)
    {
        if (targetDictionary.ContainsKey(key) == false)
        {
            targetDictionary.Add(key, new List<Course>());
            targetDictionary[key].Add(course);
            return true;
        }
        else
        {
            if (targetDictionary[key].Contains(course))
            {
                return false;
            }

            targetDictionary[key].Add(course);
            return true;
        }
    }

    public static bool AddCustom(this Dictionary<string, List<Student>> targetDictionary, string key, Student student)
    {
        if (targetDictionary.ContainsKey(key) == false)
        {
            targetDictionary.Add(key, new List<Student>());
            targetDictionary[key].Add(student);
            return true;
        }
        else
        {
            if (targetDictionary[key].Contains(student))
            {
                return false;
            }

            targetDictionary[key].Add(student);
            return true;
        }
    }

    public static bool AddCustom(this Dictionary<string, Professor> targetDictionary, string key, Professor professor)
    {
        if (targetDictionary.ContainsKey(key) == false)
        {
            targetDictionary.Add(key, professor);
            return true;
        }
        return false;
    }

    public static bool RemoveCustom(this Dictionary<string, List<Student>> targetDictionary, string key, Student student)
    {
        if (targetDictionary.ContainsKey(key))
        {
            if (targetDictionary[key].Contains(student))
            {
                targetDictionary[key].Remove(student);
                return true;
            }
            return false;
        }
        return false;
    }

    public static bool RemoveCustom(this Dictionary<int, List<Course>> targetDictionary, int key, Course course)
    {
        if (targetDictionary.ContainsKey(key))
        {
            if (targetDictionary[key].Contains(course))
            {
                targetDictionary[key].Remove(course);
                return true;
            }
            return false;
        }
        return false;
    }
}