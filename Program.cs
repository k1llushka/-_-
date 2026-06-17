using System;
using System.Data.SqlClient;

namespace ElectronicDiaryConsole
{
    class Program
    {
        static string connStr = "Server=localhost;Database=electronicdiary;Integrated Security=True;TrustServerCertificate=True;";

        static void Main()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== ELECTRONIC DIARY ===");
                Console.WriteLine("1. Students");
                Console.WriteLine("2. Teachers");
                Console.WriteLine("3. Subjects");
                Console.WriteLine("4. Classes");
                Console.WriteLine("5. Grades");
                Console.WriteLine("6. Attendance");
                Console.WriteLine("7. Reports");
                Console.WriteLine("0. Exit");
                Console.Write("Choose: ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1": MenuStudents(); break;
                    case "2": MenuTeachers(); break;
                    case "3": MenuSubjects(); break;
                    case "4": MenuClasses(); break;
                    case "5": MenuGrades(); break;
                    case "6": MenuAttendance(); break;
                    case "7": ShowReports(); break;
                    case "0": return;
                }
            }
        }

        // ==================== STUDENTS ====================
        static void MenuStudents()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== STUDENTS ===");
                Console.WriteLine("1. View all");
                Console.WriteLine("2. Add");
                Console.WriteLine("3. Update");
                Console.WriteLine("4. Delete");
                Console.WriteLine("5. Search");
                Console.WriteLine("0. Back");
                Console.Write("Choose: ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1": ViewStudents(); break;
                    case "2": AddStudent(); break;
                    case "3": UpdateStudent(); break;
                    case "4": DeleteStudent(); break;
                    case "5": SearchStudent(); break;
                    case "0": return;
                }
                Console.WriteLine("\nPress any key...");
                Console.ReadKey();
            }
        }

        static void ViewStudents()
        {
            ExecuteReader("SELECT studentid, firstname, lastname, email, isactive FROM students WHERE isactive=1",
                r => Console.WriteLine($"{r["studentid"],-5} {r["firstname"],-15} {r["lastname"],-15} {r["email"],-25} {r["isactive"]}"));
        }

        static void AddStudent()
        {
            Console.Write("First Name: "); string fn = Console.ReadLine();
            Console.Write("Last Name: "); string ln = Console.ReadLine();
            Console.Write("Email: "); string email = Console.ReadLine();

            ExecuteNonQuery(
                "INSERT INTO students (firstname, lastname, email, isactive) VALUES (@fn, @ln, @email, 1)",
                cmd => { cmd.Parameters.AddWithValue("@fn", fn); cmd.Parameters.AddWithValue("@ln", ln); cmd.Parameters.AddWithValue("@email", email); },
                "Student added!");
        }

        static void UpdateStudent()
        {
            Console.Write("Student ID: "); int id = int.Parse(Console.ReadLine());
            Console.Write("New Email: "); string email = Console.ReadLine();

            ExecuteNonQuery(
                "UPDATE students SET email=@email WHERE studentid=@id",
                cmd => { cmd.Parameters.AddWithValue("@email", email); cmd.Parameters.AddWithValue("@id", id); },
                "Student updated!");
        }

        static void DeleteStudent()
        {
            Console.Write("Student ID: "); int id = int.Parse(Console.ReadLine());
            ExecuteNonQuery("UPDATE students SET isactive=0 WHERE studentid=@id",
                cmd => cmd.Parameters.AddWithValue("@id", id), "Student deleted!");
        }

        static void SearchStudent()
        {
            Console.Write("Search: "); string keyword = Console.ReadLine();
            ExecuteReader(
                "SELECT studentid, firstname, lastname, email FROM students WHERE firstname LIKE @k OR lastname LIKE @k",
                r => Console.WriteLine($"{r["studentid"],-5} {r["firstname"],-15} {r["lastname"],-15} {r["email"],-25}"),
                cmd => cmd.Parameters.AddWithValue("@k", "%" + keyword + "%"));
        }

        // ==================== TEACHERS ====================
        static void MenuTeachers()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== TEACHERS ===");
                Console.WriteLine("1. View all");
                Console.WriteLine("2. Add");
                Console.WriteLine("3. Update salary");
                Console.WriteLine("4. Delete");
                Console.WriteLine("0. Back");
                Console.Write("Choose: ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1": ViewTeachers(); break;
                    case "2": AddTeacher(); break;
                    case "3": UpdateTeacherSalary(); break;
                    case "4": DeleteTeacher(); break;
                    case "0": return;
                }
                Console.WriteLine("\nPress any key...");
                Console.ReadKey();
            }
        }

        static void ViewTeachers()
        {
            ExecuteReader("SELECT teacherid, firstname, lastname, qualification, salary FROM teachers WHERE isactive=1",
                r => Console.WriteLine($"{r["teacherid"],-5} {r["firstname"],-15} {r["lastname"],-15} {r["qualification"],-20} {r["salary"],-10:C}"));
        }

        static void AddTeacher()
        {
            Console.Write("First Name: "); string fn = Console.ReadLine();
            Console.Write("Last Name: "); string ln = Console.ReadLine();
            Console.Write("Email: "); string email = Console.ReadLine();
            Console.Write("Qualification: "); string qual = Console.ReadLine();
            Console.Write("Salary: "); decimal salary = decimal.Parse(Console.ReadLine());

            ExecuteNonQuery(
                "INSERT INTO teachers (firstname, lastname, email, qualification, salary, hiredate, isactive) VALUES (@fn, @ln, @email, @qual, @salary, GETDATE(), 1)",
                cmd => { cmd.Parameters.AddWithValue("@fn", fn); cmd.Parameters.AddWithValue("@ln", ln); cmd.Parameters.AddWithValue("@email", email); cmd.Parameters.AddWithValue("@qual", qual); cmd.Parameters.AddWithValue("@salary", salary); },
                "Teacher added!");
        }

        static void UpdateTeacherSalary()
        {
            Console.Write("Teacher ID: "); int id = int.Parse(Console.ReadLine());
            Console.Write("New Salary: "); decimal salary = decimal.Parse(Console.ReadLine());

            ExecuteNonQuery("UPDATE teachers SET salary=@salary WHERE teacherid=@id",
                cmd => { cmd.Parameters.AddWithValue("@salary", salary); cmd.Parameters.AddWithValue("@id", id); },
                "Salary updated!");
        }

        static void DeleteTeacher()
        {
            Console.Write("Teacher ID: "); int id = int.Parse(Console.ReadLine());
            ExecuteNonQuery("UPDATE teachers SET isactive=0 WHERE teacherid=@id",
                cmd => cmd.Parameters.AddWithValue("@id", id), "Teacher deleted!");
        }

        // ==================== SUBJECTS ====================
        static void MenuSubjects()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== SUBJECTS ===");
                Console.WriteLine("1. View all");
                Console.WriteLine("2. Add");
                Console.WriteLine("3. Update");
                Console.WriteLine("4. Delete");
                Console.WriteLine("0. Back");
                Console.Write("Choose: ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1": ViewSubjects(); break;
                    case "2": AddSubject(); break;
                    case "3": UpdateSubject(); break;
                    case "4": DeleteSubject(); break;
                    case "0": return;
                }
                Console.WriteLine("\nPress any key...");
                Console.ReadKey();
            }
        }

        static void ViewSubjects()
        {
            ExecuteReader("SELECT subjectid, subjectname, credits, hoursperweek FROM subjects",
                r => Console.WriteLine($"{r["subjectid"],-5} {r["subjectname"],-20} {r["credits"],-8} {r["hoursperweek"],-12}"));
        }

        static void AddSubject()
        {
            Console.Write("Subject Name: "); string name = Console.ReadLine();
            Console.Write("Credits: "); int credits = int.Parse(Console.ReadLine());
            Console.Write("Hours per Week: "); int hours = int.Parse(Console.ReadLine());

            ExecuteNonQuery(
                "INSERT INTO subjects (subjectname, credits, hoursperweek) VALUES (@name, @credits, @hours)",
                cmd => { cmd.Parameters.AddWithValue("@name", name); cmd.Parameters.AddWithValue("@credits", credits); cmd.Parameters.AddWithValue("@hours", hours); },
                "Subject added!");
        }

        static void UpdateSubject()
        {
            Console.Write("Subject ID: "); int id = int.Parse(Console.ReadLine());
            Console.Write("New Hours per Week: "); int hours = int.Parse(Console.ReadLine());

            ExecuteNonQuery("UPDATE subjects SET hoursperweek=@hours WHERE subjectid=@id",
                cmd => { cmd.Parameters.AddWithValue("@hours", hours); cmd.Parameters.AddWithValue("@id", id); },
                "Subject updated!");
        }

        static void DeleteSubject()
        {
            Console.Write("Subject ID: "); int id = int.Parse(Console.ReadLine());
            ExecuteNonQuery("DELETE FROM subjects WHERE subjectid=@id",
                cmd => cmd.Parameters.AddWithValue("@id", id), "Subject deleted!");
        }

        // ==================== CLASSES ====================
        static void MenuClasses()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== CLASSES ===");
                Console.WriteLine("1. View all");
                Console.WriteLine("2. Add");
                Console.WriteLine("3. Update");
                Console.WriteLine("4. Delete");
                Console.WriteLine("5. View students in class");
                Console.WriteLine("0. Back");
                Console.Write("Choose: ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1": ViewClasses(); break;
                    case "2": AddClass(); break;
                    case "3": UpdateClass(); break;
                    case "4": DeleteClass(); break;
                    case "5": ViewStudentsInClass(); break;
                    case "0": return;
                }
                Console.WriteLine("\nPress any key...");
                Console.ReadKey();
            }
        }

        static void ViewClasses()
        {
            ExecuteReader(
                "SELECT c.classid, c.classname, c.room, c.capacity, c.currentenrollment, t.firstname + ' ' + t.lastname as teacher FROM classes c JOIN teachers t ON c.teacherid = t.teacherid",
                r => Console.WriteLine($"{r["classid"],-5} {r["classname"],-10} {r["room"],-8} {r["capacity"],-8} {r["currentenrollment"],-8} {r["teacher"],-20}"));
        }

        static void AddClass()
        {
            Console.Write("Class Name: "); string name = Console.ReadLine();
            Console.Write("Year: "); int year = int.Parse(Console.ReadLine());
            Console.Write("Teacher ID: "); int teacherId = int.Parse(Console.ReadLine());
            Console.Write("Room: "); string room = Console.ReadLine();
            Console.Write("Capacity: "); int capacity = int.Parse(Console.ReadLine());

            ExecuteNonQuery(
                "INSERT INTO classes (classname, year, teacherid, room, capacity) VALUES (@name, @year, @tid, @room, @capacity)",
                cmd => { cmd.Parameters.AddWithValue("@name", name); cmd.Parameters.AddWithValue("@year", year); cmd.Parameters.AddWithValue("@tid", teacherId); cmd.Parameters.AddWithValue("@room", room); cmd.Parameters.AddWithValue("@capacity", capacity); },
                "Class added!");
        }

        static void UpdateClass()
        {
            Console.Write("Class ID: "); int id = int.Parse(Console.ReadLine());
            Console.Write("New Room: "); string room = Console.ReadLine();

            ExecuteNonQuery("UPDATE classes SET room=@room WHERE classid=@id",
                cmd => { cmd.Parameters.AddWithValue("@room", room); cmd.Parameters.AddWithValue("@id", id); },
                "Class updated!");
        }

        static void DeleteClass()
        {
            Console.Write("Class ID: "); int id = int.Parse(Console.ReadLine());
            ExecuteNonQuery("DELETE FROM classes WHERE classid=@id",
                cmd => cmd.Parameters.AddWithValue("@id", id), "Class deleted!");
        }

        static void ViewStudentsInClass()
        {
            Console.Write("Class ID: "); int id = int.Parse(Console.ReadLine());
            ExecuteReader(
                "SELECT s.studentid, s.firstname, s.lastname, s.email FROM students s JOIN enrollments e ON s.studentid = e.studentid WHERE e.classid = @id",
                r => Console.WriteLine($"{r["studentid"],-5} {r["firstname"],-15} {r["lastname"],-15} {r["email"],-25}"),
                cmd => cmd.Parameters.AddWithValue("@id", id));
        }

        // ==================== GRADES ====================
        static void MenuGrades()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== GRADES ===");
                Console.WriteLine("1. View all");
                Console.WriteLine("2. Add");
                Console.WriteLine("3. Update");
                Console.WriteLine("4. Delete");
                Console.WriteLine("5. Student average");
                Console.WriteLine("0. Back");
                Console.Write("Choose: ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1": ViewGrades(); break;
                    case "2": AddGrade(); break;
                    case "3": UpdateGrade(); break;
                    case "4": DeleteGrade(); break;
                    case "5": StudentAverage(); break;
                    case "0": return;
                }
                Console.WriteLine("\nPress any key...");
                Console.ReadKey();
            }
        }

        static void ViewGrades()
        {
            ExecuteReader(
                "SELECT TOP 20 g.gradeid, s.firstname + ' ' + s.lastname as student, a.title, g.score, g.gradedate FROM grades g JOIN students s ON g.studentid = s.studentid JOIN assignments a ON g.assignmentid = a.assignmentid ORDER BY g.gradedate DESC",
                r => Console.WriteLine($"{r["gradeid"],-5} {r["student"],-25} {r["title"],-30} {r["score"],-8} {r["gradedate"],-12:yyyy-MM-dd}"));
        }

        static void AddGrade()
        {
            Console.Write("Student ID: "); int sid = int.Parse(Console.ReadLine());
            Console.Write("Assignment ID: "); int aid = int.Parse(Console.ReadLine());
            Console.Write("Score: "); int score = int.Parse(Console.ReadLine());

            ExecuteNonQuery(
                "INSERT INTO grades (studentid, assignmentid, score, gradedate) VALUES (@sid, @aid, @score, GETDATE())",
                cmd => { cmd.Parameters.AddWithValue("@sid", sid); cmd.Parameters.AddWithValue("@aid", aid); cmd.Parameters.AddWithValue("@score", score); },
                "Grade added!");
        }

        static void UpdateGrade()
        {
            Console.Write("Grade ID: "); int id = int.Parse(Console.ReadLine());
            Console.Write("New Score: "); int score = int.Parse(Console.ReadLine());

            ExecuteNonQuery("UPDATE grades SET score=@score WHERE gradeid=@id",
                cmd => { cmd.Parameters.AddWithValue("@score", score); cmd.Parameters.AddWithValue("@id", id); },
                "Grade updated!");
        }

        static void DeleteGrade()
        {
            Console.Write("Grade ID: "); int id = int.Parse(Console.ReadLine());
            ExecuteNonQuery("DELETE FROM grades WHERE gradeid=@id",
                cmd => cmd.Parameters.AddWithValue("@id", id), "Grade deleted!");
        }

        static void StudentAverage()
        {
            Console.Write("Student ID: "); int id = int.Parse(Console.ReadLine());
            ExecuteReader(
                "SELECT s.firstname, s.lastname, AVG(CAST(g.score AS FLOAT)) as avg FROM grades g JOIN students s ON g.studentid = s.studentid WHERE s.studentid=@id GROUP BY s.firstname, s.lastname",
                r => Console.WriteLine($"Student: {r["firstname"]} {r["lastname"]}, Average: {r["avg"]:F2}"),
                cmd => cmd.Parameters.AddWithValue("@id", id));
        }

        // ==================== ATTENDANCE ====================
        static void MenuAttendance()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== ATTENDANCE ===");
                Console.WriteLine("1. View all");
                Console.WriteLine("2. Mark");
                Console.WriteLine("3. Update");
                Console.WriteLine("4. Delete");
                Console.WriteLine("0. Back");
                Console.Write("Choose: ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1": ViewAttendance(); break;
                    case "2": MarkAttendance(); break;
                    case "3": UpdateAttendance(); break;
                    case "4": DeleteAttendance(); break;
                    case "0": return;
                }
                Console.WriteLine("\nPress any key...");
                Console.ReadKey();
            }
        }

        static void ViewAttendance()
        {
            ExecuteReader(
                "SELECT TOP 20 a.attendanceid, s.firstname + ' ' + s.lastname as student, c.classname, a.attendancedate, a.status FROM attendance a JOIN students s ON a.studentid = s.studentid JOIN classes c ON a.classid = c.classid ORDER BY a.attendancedate DESC",
                r => Console.WriteLine($"{r["attendanceid"],-5} {r["student"],-25} {r["classname"],-10} {r["attendancedate"],-12:yyyy-MM-dd} {r["status"],-8}"));
        }

        static void MarkAttendance()
        {
            Console.Write("Student ID: "); int sid = int.Parse(Console.ReadLine());
            Console.Write("Class ID: "); int cid = int.Parse(Console.ReadLine());
            Console.Write("Date (YYYY-MM-DD): "); DateTime date = DateTime.Parse(Console.ReadLine());
            Console.Write("Status (present/absent/late): "); string status = Console.ReadLine();

            ExecuteNonQuery(
                "INSERT INTO attendance (studentid, classid, attendancedate, status) VALUES (@sid, @cid, @date, @status)",
                cmd => { cmd.Parameters.AddWithValue("@sid", sid); cmd.Parameters.AddWithValue("@cid", cid); cmd.Parameters.AddWithValue("@date", date); cmd.Parameters.AddWithValue("@status", status); },
                "Attendance marked!");
        }

        static void UpdateAttendance()
        {
            Console.Write("Attendance ID: "); int id = int.Parse(Console.ReadLine());
            Console.Write("New Status: "); string status = Console.ReadLine();

            ExecuteNonQuery("UPDATE attendance SET status=@status WHERE attendanceid=@id",
                cmd => { cmd.Parameters.AddWithValue("@status", status); cmd.Parameters.AddWithValue("@id", id); },
                "Attendance updated!");
        }

        static void DeleteAttendance()
        {
            Console.Write("Attendance ID: "); int id = int.Parse(Console.ReadLine());
            ExecuteNonQuery("DELETE FROM attendance WHERE attendanceid=@id",
                cmd => cmd.Parameters.AddWithValue("@id", id), "Attendance deleted!");
        }

        // ==================== REPORTS ====================
        static void ShowReports()
        {
            Console.Clear();
            Console.WriteLine("=== REPORTS ===");
            Console.WriteLine("1. Class averages");
            Console.WriteLine("2. Top 10 students");
            Console.WriteLine("3. Attendance summary");
            Console.Write("Choose: ");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1": ExecuteReader("SELECT c.classname, AVG(CAST(g.score AS FLOAT)) as avg FROM grades g JOIN students s ON g.studentid=s.studentid JOIN enrollments e ON s.studentid=e.studentid JOIN classes c ON e.classid=c.classid GROUP BY c.classname", r => Console.WriteLine($"{r["classname"],-10} {r["avg"],-10:F2}")); break;
                case "2": ExecuteReader("SELECT TOP 10 s.firstname + ' ' + s.lastname as name, AVG(CAST(g.score AS FLOAT)) as avg FROM grades g JOIN students s ON g.studentid=s.studentid GROUP BY s.studentid, s.firstname, s.lastname ORDER BY avg DESC", r => Console.WriteLine($"{r["name"],-25} {r["avg"],-10:F2}")); break;
                case "3": ExecuteReader("SELECT c.classname, COUNT(*) as total, SUM(CASE WHEN a.status='present' THEN 1 ELSE 0 END)*100.0/COUNT(*) as rate FROM attendance a JOIN classes c ON a.classid=c.classid GROUP BY c.classname", r => Console.WriteLine($"{r["classname"],-10} {r["total"],-8} {r["rate"],-10:F2}%")); break;
            }
            Console.WriteLine("\nPress any key...");
            Console.ReadKey();
        }

        // ==================== HELPER METHODS ====================
        static void ExecuteReader(string query, Action<SqlDataReader> display, Action<SqlCommand>? paramAction = null)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                paramAction?.Invoke(cmd);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read()) display(reader);
                }
            }
        }

        static void ExecuteNonQuery(string query, Action<SqlCommand> paramAction, string successMsg)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                paramAction(cmd);

                int rows = cmd.ExecuteNonQuery();
                Console.WriteLine(rows > 0 ? successMsg : "Operation failed!");
            }
        }
    }
}