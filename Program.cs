using System;
using System.Data.SqlClient;

namespace ElectronicDiaryConsole
{
    class Program
    {
        static string connectionString = "Server=DESKTOP-V3GVIBO\\SQLEXPRESS;Database=electronicdiary;Integrated Security=True;TrustServerCertificate=True;";

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.Title = "Electronic Diary Management System";

            while (true)
            {
                Console.Clear();
                Console.WriteLine("========================================");
                Console.WriteLine("   ELECTRONIC DIARY MANAGEMENT SYSTEM");
                Console.WriteLine("========================================");
                Console.WriteLine("1. Students Management");
                Console.WriteLine("2. Teachers Management");
                Console.WriteLine("3. Subjects Management");
                Console.WriteLine("4. Classes Management");
                Console.WriteLine("5. Enrollments Management");
                Console.WriteLine("6. Assignments Management");
                Console.WriteLine("7. Grades Management");
                Console.WriteLine("8. Attendance Management");
                Console.WriteLine("9. Users Management");
                Console.WriteLine("10. Reports & Analytics");
                Console.WriteLine("0. Exit");
                Console.WriteLine("========================================");
                Console.Write("Select option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": ManageStudents(); break;
                    case "2": ManageTeachers(); break;
                    case "3": ManageSubjects(); break;
                    case "4": ManageClasses(); break;
                    case "5": ManageEnrollments(); break;
                    case "6": ManageAssignments(); break;
                    case "7": ManageGrades(); break;
                    case "8": ManageAttendance(); break;
                    case "9": ManageUsers(); break;
                    case "10": ShowReports(); break;
                    case "0": return;
                    default: Console.WriteLine("Invalid option!"); break;
                }
            }
        }

        static void ManageStudents()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== STUDENTS MANAGEMENT ===");
                Console.WriteLine("1. View all students");
                Console.WriteLine("2. Add new student");
                Console.WriteLine("3. Update student");
                Console.WriteLine("4. Delete student");
                Console.WriteLine("5. Search student");
                Console.WriteLine("0. Back to main menu");
                Console.Write("Select option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": ViewAllStudents(); break;
                    case "2": AddStudent(); break;
                    case "3": UpdateStudent(); break;
                    case "4": DeleteStudent(); break;
                    case "5": SearchStudent(); break;
                    case "0": return;
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        static void ViewAllStudents()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT studentid, firstname, lastname, middlename, birthdate, email, phonenumber, isactive 
                    FROM students WHERE isactive = 1";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                Console.WriteLine("\n{0,-5} {1,-15} {2,-15} {3,-15} {4,-12} {5,-25} {6,-15} {7,-8}",
                    "ID", "First Name", "Last Name", "Middle Name", "Birth Date", "Email", "Phone", "Active");
                Console.WriteLine(new string('-', 110));

                while (reader.Read())
                {
                    Console.WriteLine("{0,-5} {1,-15} {2,-15} {3,-15} {4,-12:yyyy-MM-dd} {5,-25} {6,-15} {7,-8}",
                        reader["studentid"], reader["firstname"], reader["lastname"],
                        reader["middlename"] ?? "", reader["birthdate"] ?? "",
                        reader["email"] ?? "", reader["phonenumber"] ?? "", reader["isactive"]);
                }
                reader.Close();
            }
        }

        static void AddStudent()
        {
            Console.WriteLine("\n--- Add New Student ---");
            Console.Write("First Name: "); string firstName = Console.ReadLine();
            Console.Write("Last Name: "); string lastName = Console.ReadLine();
            Console.Write("Middle Name: "); string middleName = Console.ReadLine();
            Console.Write("Birth Date (YYYY-MM-DD): "); string birthDate = Console.ReadLine();
            Console.Write("Email: "); string email = Console.ReadLine();
            Console.Write("Phone Number: "); string phone = Console.ReadLine();
            Console.Write("Parent Phone: "); string parentPhone = Console.ReadLine();
            Console.Write("Parent Email: "); string parentEmail = Console.ReadLine();
            Console.Write("Address: "); string address = Console.ReadLine();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
                    INSERT INTO students (firstname, lastname, middlename, birthdate, email, phonenumber, 
                                          parentphone, parentemail, address, enrolleddate, isactive)
                    VALUES (@firstname, @lastname, @middlename, @birthdate, @email, @phonenumber, 
                            @parentphone, @parentemail, @address, GETDATE(), 1)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@firstname", firstName);
                cmd.Parameters.AddWithValue("@lastname", lastName);
                cmd.Parameters.AddWithValue("@middlename", string.IsNullOrEmpty(middleName) ? (object)DBNull.Value : middleName);
                cmd.Parameters.AddWithValue("@birthdate", string.IsNullOrEmpty(birthDate) ? (object)DBNull.Value : DateTime.Parse(birthDate));
                cmd.Parameters.AddWithValue("@email", string.IsNullOrEmpty(email) ? (object)DBNull.Value : email);
                cmd.Parameters.AddWithValue("@phonenumber", string.IsNullOrEmpty(phone) ? (object)DBNull.Value : phone);
                cmd.Parameters.AddWithValue("@parentphone", string.IsNullOrEmpty(parentPhone) ? (object)DBNull.Value : parentPhone);
                cmd.Parameters.AddWithValue("@parentemail", string.IsNullOrEmpty(parentEmail) ? (object)DBNull.Value : parentEmail);
                cmd.Parameters.AddWithValue("@address", string.IsNullOrEmpty(address) ? (object)DBNull.Value : address);

                int result = cmd.ExecuteNonQuery();
                Console.WriteLine(result > 0 ? "Student added successfully!" : "Failed to add student.");
            }
        }

        static void UpdateStudent()
        {
            Console.Write("\nEnter Student ID to update: ");
            int id = int.Parse(Console.ReadLine());

            Console.Write("New Phone Number: "); string phone = Console.ReadLine();
            Console.Write("New Address: "); string address = Console.ReadLine();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE students SET phonenumber = @phone, address = @address WHERE studentid = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@phone", phone);
                cmd.Parameters.AddWithValue("@address", address);
                cmd.Parameters.AddWithValue("@id", id);

                int result = cmd.ExecuteNonQuery();
                Console.WriteLine(result > 0 ? "Student updated successfully!" : "Student not found.");
            }
        }

        static void DeleteStudent()
        {
            Console.Write("\nEnter Student ID to delete: ");
            int id = int.Parse(Console.ReadLine());

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE students SET isactive = 0 WHERE studentid = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                int result = cmd.ExecuteNonQuery();
                Console.WriteLine(result > 0 ? "Student deleted (soft delete) successfully!" : "Student not found.");
            }
        }

        static void SearchStudent()
        {
            Console.Write("\nEnter search keyword (name or email): ");
            string keyword = Console.ReadLine();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT studentid, firstname, lastname, email, phonenumber 
                    FROM students 
                    WHERE firstname LIKE @keyword OR lastname LIKE @keyword OR email LIKE @keyword";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@keyword", "%" + keyword + "%");
                SqlDataReader reader = cmd.ExecuteReader();

                Console.WriteLine("\nSearch Results:");
                while (reader.Read())
                {
                    Console.WriteLine($"ID: {reader["studentid"]}, Name: {reader["firstname"]} {reader["lastname"]}, Email: {reader["email"]}, Phone: {reader["phonenumber"]}");
                }
                reader.Close();
            }
        }

        static void ManageTeachers()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== TEACHERS MANAGEMENT ===");
                Console.WriteLine("1. View all teachers");
                Console.WriteLine("2. Add new teacher");
                Console.WriteLine("3. Update teacher salary");
                Console.WriteLine("4. Delete teacher");
                Console.WriteLine("0. Back");
                Console.Write("Select option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": ViewAllTeachers(); break;
                    case "2": AddTeacher(); break;
                    case "3": UpdateTeacherSalary(); break;
                    case "4": DeleteTeacher(); break;
                    case "0": return;
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        static void ViewAllTeachers()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT teacherid, firstname, lastname, qualification, salary, email FROM teachers WHERE isactive = 1";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                Console.WriteLine("\n{0,-5} {1,-15} {2,-15} {3,-20} {4,-10} {5,-30}",
                    "ID", "First Name", "Last Name", "Qualification", "Salary", "Email");
                Console.WriteLine(new string('-', 100));

                while (reader.Read())
                {
                    Console.WriteLine("{0,-5} {1,-15} {2,-15} {3,-20} {4,-10:C} {5,-30}",
                        reader["teacherid"], reader["firstname"], reader["lastname"],
                        reader["qualification"] ?? "", reader["salary"] ?? 0, reader["email"] ?? "");
                }
                reader.Close();
            }
        }

        static void AddTeacher()
        {
            Console.WriteLine("\n--- Add New Teacher ---");
            Console.Write("First Name: "); string firstName = Console.ReadLine();
            Console.Write("Last Name: "); string lastName = Console.ReadLine();
            Console.Write("Email: "); string email = Console.ReadLine();
            Console.Write("Qualification: "); string qualification = Console.ReadLine();
            Console.Write("Salary: "); decimal salary = decimal.Parse(Console.ReadLine());

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
                    INSERT INTO teachers (firstname, lastname, email, qualification, salary, hiredate, isactive)
                    VALUES (@firstname, @lastname, @email, @qualification, @salary, GETDATE(), 1)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@firstname", firstName);
                cmd.Parameters.AddWithValue("@lastname", lastName);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@qualification", qualification);
                cmd.Parameters.AddWithValue("@salary", salary);

                int result = cmd.ExecuteNonQuery();
                Console.WriteLine(result > 0 ? "Teacher added successfully!" : "Failed to add teacher.");
            }
        }

        static void UpdateTeacherSalary()
        {
            Console.Write("\nEnter Teacher ID: ");
            int id = int.Parse(Console.ReadLine());
            Console.Write("New Salary: ");
            decimal salary = decimal.Parse(Console.ReadLine());

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE teachers SET salary = @salary WHERE teacherid = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@salary", salary);
                cmd.Parameters.AddWithValue("@id", id);

                int result = cmd.ExecuteNonQuery();
                Console.WriteLine(result > 0 ? "Salary updated!" : "Teacher not found.");
            }
        }

        static void DeleteTeacher()
        {
            Console.Write("\nEnter Teacher ID to delete: ");
            int id = int.Parse(Console.ReadLine());

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE teachers SET isactive = 0 WHERE teacherid = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                int result = cmd.ExecuteNonQuery();
                Console.WriteLine(result > 0 ? "Teacher deleted!" : "Teacher not found.");
            }
        }

        static void ManageSubjects()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== SUBJECTS MANAGEMENT ===");
                Console.WriteLine("1. View all subjects");
                Console.WriteLine("2. Add new subject");
                Console.WriteLine("3. Update subject");
                Console.WriteLine("4. Delete subject");
                Console.WriteLine("0. Back");
                Console.Write("Select option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": ViewAllSubjects(); break;
                    case "2": AddSubject(); break;
                    case "3": UpdateSubject(); break;
                    case "4": DeleteSubject(); break;
                    case "0": return;
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        static void ViewAllSubjects()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT subjectid, subjectname, subjectcode, credits, hoursperweek FROM subjects";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                Console.WriteLine("\n{0,-5} {1,-20} {2,-12} {3,-8} {4,-12}",
                    "ID", "Subject Name", "Code", "Credits", "Hours/Week");
                Console.WriteLine(new string('-', 60));

                while (reader.Read())
                {
                    Console.WriteLine("{0,-5} {1,-20} {2,-12} {3,-8} {4,-12}",
                        reader["subjectid"], reader["subjectname"],
                        reader["subjectcode"] ?? "", reader["credits"], reader["hoursperweek"]);
                }
                reader.Close();
            }
        }

        static void AddSubject()
        {
            Console.WriteLine("\n--- Add New Subject ---");
            Console.Write("Subject Name: "); string name = Console.ReadLine();
            Console.Write("Subject Code: "); string code = Console.ReadLine();
            Console.Write("Credits: "); int credits = int.Parse(Console.ReadLine());
            Console.Write("Hours per Week: "); int hours = int.Parse(Console.ReadLine());

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
                    INSERT INTO subjects (subjectname, subjectcode, credits, hoursperweek)
                    VALUES (@name, @code, @credits, @hours)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@code", code);
                cmd.Parameters.AddWithValue("@credits", credits);
                cmd.Parameters.AddWithValue("@hours", hours);

                int result = cmd.ExecuteNonQuery();
                Console.WriteLine(result > 0 ? "Subject added!" : "Failed.");
            }
        }

        static void UpdateSubject()
        {
            Console.Write("\nEnter Subject ID: ");
            int id = int.Parse(Console.ReadLine());
            Console.Write("New Hours per Week: ");
            int hours = int.Parse(Console.ReadLine());

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE subjects SET hoursperweek = @hours WHERE subjectid = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@hours", hours);
                cmd.Parameters.AddWithValue("@id", id);

                int result = cmd.ExecuteNonQuery();
                Console.WriteLine(result > 0 ? "Subject updated!" : "Subject not found.");
            }
        }

        static void DeleteSubject()
        {
            Console.Write("\nEnter Subject ID to delete: ");
            int id = int.Parse(Console.ReadLine());

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM subjects WHERE subjectid = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                try
                {
                    int result = cmd.ExecuteNonQuery();
                    Console.WriteLine(result > 0 ? "Subject deleted!" : "Subject not found.");
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Cannot delete subject because it has related records!");
                }
            }
        }

        static void ManageClasses()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== CLASSES MANAGEMENT ===");
                Console.WriteLine("1. View all classes");
                Console.WriteLine("2. Add new class");
                Console.WriteLine("3. Update class");
                Console.WriteLine("4. Delete class");
                Console.WriteLine("0. Back");
                Console.Write("Select option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": ViewAllClasses(); break;
                    case "2": AddClass(); break;
                    case "3": UpdateClass(); break;
                    case "4": DeleteClass(); break;
                    case "0": return;
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        static void ViewAllClasses()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT c.classid, c.classname, c.year, c.room, c.building, c.capacity, c.currentenrollment,
                           t.firstname + ' ' + t.lastname as teachername
                    FROM classes c
                    JOIN teachers t ON c.teacherid = t.teacherid";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                Console.WriteLine("\n{0,-5} {1,-8} {2,-6} {3,-8} {4,-12} {5,-10} {6,-10} {7,-20}",
                    "ID", "Class", "Year", "Room", "Building", "Capacity", "Enrolled", "Teacher");
                Console.WriteLine(new string('-', 90));

                while (reader.Read())
                {
                    Console.WriteLine("{0,-5} {1,-8} {2,-6} {3,-8} {4,-12} {5,-10} {6,-10} {7,-20}",
                        reader["classid"], reader["classname"], reader["year"],
                        reader["room"] ?? "", reader["building"] ?? "",
                        reader["capacity"], reader["currentenrollment"], reader["teachername"]);
                }
                reader.Close();
            }
        }

        static void AddClass()
        {
            Console.WriteLine("\n--- Add New Class ---");
            Console.Write("Class Name (e.g., 10a): "); string name = Console.ReadLine();
            Console.Write("Year: "); int year = int.Parse(Console.ReadLine());
            Console.Write("Teacher ID: "); int teacherId = int.Parse(Console.ReadLine());
            Console.Write("Room: "); string room = Console.ReadLine();
            Console.Write("Building: "); string building = Console.ReadLine();
            Console.Write("Capacity: "); int capacity = int.Parse(Console.ReadLine());

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
                    INSERT INTO classes (classname, year, teacherid, room, building, capacity, currentenrollment)
                    VALUES (@name, @year, @teacherid, @room, @building, @capacity, 0)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@teacherid", teacherId);
                cmd.Parameters.AddWithValue("@room", room);
                cmd.Parameters.AddWithValue("@building", building);
                cmd.Parameters.AddWithValue("@capacity", capacity);

                int result = cmd.ExecuteNonQuery();
                Console.WriteLine(result > 0 ? "Class added!" : "Failed.");
            }
        }

        static void UpdateClass()
        {
            Console.Write("\nEnter Class ID: ");
            int id = int.Parse(Console.ReadLine());
            Console.Write("New Room: "); string room = Console.ReadLine();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE classes SET room = @room WHERE classid = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@room", room);
                cmd.Parameters.AddWithValue("@id", id);

                int result = cmd.ExecuteNonQuery();
                Console.WriteLine(result > 0 ? "Class updated!" : "Class not found.");
            }
        }

        static void DeleteClass()
        {
            Console.Write("\nEnter Class ID to delete: ");
            int id = int.Parse(Console.ReadLine());

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM classes WHERE classid = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                try
                {
                    int result = cmd.ExecuteNonQuery();
                    Console.WriteLine(result > 0 ? "Class deleted!" : "Class not found.");
                }
                catch (SqlException)
                {
                    Console.WriteLine("Cannot delete class because it has related records!");
                }
            }
        }

        static void ManageEnrollments()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== ENROLLMENTS MANAGEMENT ===");
                Console.WriteLine("1. View all enrollments");
                Console.WriteLine("2. Enroll student in class");
                Console.WriteLine("3. Unenroll student");
                Console.WriteLine("4. View students in class");
                Console.WriteLine("0. Back");
                Console.Write("Select option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": ViewAllEnrollments(); break;
                    case "2": EnrollStudent(); break;
                    case "3": UnenrollStudent(); break;
                    case "4": ViewStudentsInClass(); break;
                    case "0": return;
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        static void ViewAllEnrollments()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT e.enrollmentid, s.firstname + ' ' + s.lastname as studentname, 
                           c.classname, e.enrollmentdate
                    FROM enrollments e
                    JOIN students s ON e.studentid = s.studentid
                    JOIN classes c ON e.classid = c.classid";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                Console.WriteLine("\n{0,-5} {1,-25} {2,-10} {3,-15}",
                    "ID", "Student Name", "Class", "Enrollment Date");
                Console.WriteLine(new string('-', 60));

                while (reader.Read())
                {
                    Console.WriteLine("{0,-5} {1,-25} {2,-10} {3,-15:yyyy-MM-dd}",
                        reader["enrollmentid"], reader["studentname"],
                        reader["classname"], reader["enrollmentdate"]);
                }
                reader.Close();
            }
        }

        static void EnrollStudent()
        {
            Console.Write("\nEnter Student ID: "); int studentId = int.Parse(Console.ReadLine());
            Console.Write("Enter Class ID: "); int classId = int.Parse(Console.ReadLine());

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string checkQuery = "SELECT COUNT(*) FROM enrollments WHERE studentid = @sid AND classid = @cid";
                SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@sid", studentId);
                checkCmd.Parameters.AddWithValue("@cid", classId);

                int exists = (int)checkCmd.ExecuteScalar();
                if (exists > 0)
                {
                    Console.WriteLine("Student already enrolled in this class!");
                    return;
                }

                string query = "INSERT INTO enrollments (studentid, classid, enrollmentdate) VALUES (@sid, @cid, GETDATE())";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@sid", studentId);
                cmd.Parameters.AddWithValue("@cid", classId);

                int result = cmd.ExecuteNonQuery();

                string updateCount = "UPDATE classes SET currentenrollment = (SELECT COUNT(*) FROM enrollments WHERE classid = @cid) WHERE classid = @cid";
                SqlCommand countCmd = new SqlCommand(updateCount, conn);
                countCmd.Parameters.AddWithValue("@cid", classId);
                countCmd.ExecuteNonQuery();

                Console.WriteLine(result > 0 ? "Student enrolled successfully!" : "Failed.");
            }
        }

        static void UnenrollStudent()
        {
            Console.Write("\nEnter Student ID: "); int studentId = int.Parse(Console.ReadLine());
            Console.Write("Enter Class ID: "); int classId = int.Parse(Console.ReadLine());

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM enrollments WHERE studentid = @sid AND classid = @cid";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@sid", studentId);
                cmd.Parameters.AddWithValue("@cid", classId);

                int result = cmd.ExecuteNonQuery();

                string updateCount = "UPDATE classes SET currentenrollment = (SELECT COUNT(*) FROM enrollments WHERE classid = @cid) WHERE classid = @cid";
                SqlCommand countCmd = new SqlCommand(updateCount, conn);
                countCmd.Parameters.AddWithValue("@cid", classId);
                countCmd.ExecuteNonQuery();

                Console.WriteLine(result > 0 ? "Student unenrolled successfully!" : "Enrollment not found.");
            }
        }

        static void ViewStudentsInClass()
        {
            Console.Write("\nEnter Class ID: ");
            int classId = int.Parse(Console.ReadLine());

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT s.studentid, s.firstname, s.lastname, s.email, e.enrollmentdate
                    FROM enrollments e
                    JOIN students s ON e.studentid = s.studentid
                    WHERE e.classid = @classid";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@classid", classId);
                SqlDataReader reader = cmd.ExecuteReader();

                Console.WriteLine($"\nStudents in Class {classId}:");
                Console.WriteLine("{0,-5} {1,-15} {2,-15} {3,-30} {4,-12}",
                    "ID", "First Name", "Last Name", "Email", "Enrolled Date");
                Console.WriteLine(new string('-', 80));

                while (reader.Read())
                {
                    Console.WriteLine("{0,-5} {1,-15} {2,-15} {3,-30} {4,-12:yyyy-MM-dd}",
                        reader["studentid"], reader["firstname"], reader["lastname"],
                        reader["email"] ?? "", reader["enrollmentdate"]);
                }
                reader.Close();
            }
        }

        static void ManageAssignments()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== ASSIGNMENTS MANAGEMENT ===");
                Console.WriteLine("1. View all assignments");
                Console.WriteLine("2. Add new assignment");
                Console.WriteLine("3. Update assignment due date");
                Console.WriteLine("4. Delete assignment");
                Console.WriteLine("0. Back");
                Console.Write("Select option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": ViewAllAssignments(); break;
                    case "2": AddAssignment(); break;
                    case "3": UpdateAssignmentDueDate(); break;
                    case "4": DeleteAssignment(); break;
                    case "0": return;
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        static void ViewAllAssignments()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT a.assignmentid, a.title, s.subjectname, c.classname, a.maxscore, a.duedate
                    FROM assignments a
                    JOIN subjects s ON a.subjectid = s.subjectid
                    JOIN classes c ON a.classid = c.classid
                    WHERE a.isactive = 1";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                Console.WriteLine("\n{0,-5} {1,-35} {2,-15} {3,-8} {4,-10} {5,-12}",
                    "ID", "Title", "Subject", "Class", "Max Score", "Due Date");
                Console.WriteLine(new string('-', 90));

                while (reader.Read())
                {
                    Console.WriteLine("{0,-5} {1,-35} {2,-15} {3,-8} {4,-10} {5,-12:yyyy-MM-dd}",
                        reader["assignmentid"], reader["title"], reader["subjectname"],
                        reader["classname"], reader["maxscore"], reader["duedate"]);
                }
                reader.Close();
            }
        }

        static void AddAssignment()
        {
            Console.WriteLine("\n--- Add New Assignment ---");
            Console.Write("Title: "); string title = Console.ReadLine();
            Console.Write("Subject ID: "); int subjectId = int.Parse(Console.ReadLine());
            Console.Write("Teacher ID: "); int teacherId = int.Parse(Console.ReadLine());
            Console.Write("Class ID: "); int classId = int.Parse(Console.ReadLine());
            Console.Write("Max Score: "); int maxScore = int.Parse(Console.ReadLine());
            Console.Write("Due Date (YYYY-MM-DD): "); DateTime dueDate = DateTime.Parse(Console.ReadLine());

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
                    INSERT INTO assignments (title, subjectid, teacherid, classid, maxscore, duedate, createddate, isactive)
                    VALUES (@title, @subjectid, @teacherid, @classid, @maxscore, @duedate, GETDATE(), 1)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@title", title);
                cmd.Parameters.AddWithValue("@subjectid", subjectId);
                cmd.Parameters.AddWithValue("@teacherid", teacherId);
                cmd.Parameters.AddWithValue("@classid", classId);
                cmd.Parameters.AddWithValue("@maxscore", maxScore);
                cmd.Parameters.AddWithValue("@duedate", dueDate);

                int result = cmd.ExecuteNonQuery();
                Console.WriteLine(result > 0 ? "Assignment added!" : "Failed.");
            }
        }

        static void UpdateAssignmentDueDate()
        {
            Console.Write("\nEnter Assignment ID: ");
            int id = int.Parse(Console.ReadLine());
            Console.Write("New Due Date (YYYY-MM-DD): ");
            DateTime dueDate = DateTime.Parse(Console.ReadLine());

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE assignments SET duedate = @duedate, updateddate = GETDATE() WHERE assignmentid = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@duedate", dueDate);
                cmd.Parameters.AddWithValue("@id", id);

                int result = cmd.ExecuteNonQuery();
                Console.WriteLine(result > 0 ? "Due date updated!" : "Assignment not found.");
            }
        }

        static void DeleteAssignment()
        {
            Console.Write("\nEnter Assignment ID to delete: ");
            int id = int.Parse(Console.ReadLine());

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE assignments SET isactive = 0 WHERE assignmentid = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                int result = cmd.ExecuteNonQuery();
                Console.WriteLine(result > 0 ? "Assignment deleted!" : "Assignment not found.");
            }
        }

        static void ManageGrades()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== GRADES MANAGEMENT ===");
                Console.WriteLine("1. View all grades");
                Console.WriteLine("2. Add grade");
                Console.WriteLine("3. Update grade");
                Console.WriteLine("4. Delete grade");
                Console.WriteLine("5. View student average");
                Console.WriteLine("0. Back");
                Console.Write("Select option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": ViewAllGrades(); break;
                    case "2": AddGrade(); break;
                    case "3": UpdateGrade(); break;
                    case "4": DeleteGrade(); break;
                    case "5": ViewStudentAverage(); break;
                    case "0": return;
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        static void ViewAllGrades()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT TOP 20 g.gradeid, s.firstname + ' ' + s.lastname as studentname, 
                           a.title, g.score, g.gradedate
                    FROM grades g
                    JOIN students s ON g.studentid = s.studentid
                    JOIN assignments a ON g.assignmentid = a.assignmentid
                    ORDER BY g.gradedate DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                Console.WriteLine("\n{0,-5} {1,-25} {2,-35} {3,-8} {4,-12}",
                    "ID", "Student", "Assignment", "Score", "Date");
                Console.WriteLine(new string('-', 90));

                while (reader.Read())
                {
                    Console.WriteLine("{0,-5} {1,-25} {2,-35} {3,-8} {4,-12:yyyy-MM-dd}",
                        reader["gradeid"], reader["studentname"],
                        reader["title"], reader["score"], reader["gradedate"]);
                }
                reader.Close();
            }
        }

        static void AddGrade()
        {
            Console.WriteLine("\n--- Add Grade ---");
            Console.Write("Student ID: "); int studentId = int.Parse(Console.ReadLine());
            Console.Write("Assignment ID: "); int assignmentId = int.Parse(Console.ReadLine());
            Console.Write("Score: "); int score = int.Parse(Console.ReadLine());
            Console.Write("Feedback: "); string feedback = Console.ReadLine();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Use stored procedure
                string query = "EXEC sp_addgrade @studentid, @assignmentid, @score";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@studentid", studentId);
                cmd.Parameters.AddWithValue("@assignmentid", assignmentId);
                cmd.Parameters.AddWithValue("@score", score);

                try
                {
                    cmd.ExecuteNonQuery();

                    string updateFeedback = "UPDATE grades SET feedback = @feedback WHERE studentid = @sid AND assignmentid = @aid";
                    SqlCommand feedbackCmd = new SqlCommand(updateFeedback, conn);
                    feedbackCmd.Parameters.AddWithValue("@feedback", feedback);
                    feedbackCmd.Parameters.AddWithValue("@sid", studentId);
                    feedbackCmd.Parameters.AddWithValue("@aid", assignmentId);
                    feedbackCmd.ExecuteNonQuery();

                    Console.WriteLine("Grade added successfully!");
                }
                catch (SqlException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        static void UpdateGrade()
        {
            Console.Write("\nEnter Grade ID: ");
            int id = int.Parse(Console.ReadLine());
            Console.Write("New Score: ");
            int score = int.Parse(Console.ReadLine());
            Console.Write("Reason for change: ");
            string reason = Console.ReadLine();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
                    UPDATE grades 
                    SET score = @score, isretake = 1, retakedate = GETDATE(), feedback = @reason 
                    WHERE gradeid = @id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@score", score);
                cmd.Parameters.AddWithValue("@reason", reason);
                cmd.Parameters.AddWithValue("@id", id);

                int result = cmd.ExecuteNonQuery();
                Console.WriteLine(result > 0 ? "Grade updated!" : "Grade not found.");
            }
        }

        static void DeleteGrade()
        {
            Console.Write("\nEnter Grade ID to delete: ");
            int id = int.Parse(Console.ReadLine());

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM grades WHERE gradeid = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                int result = cmd.ExecuteNonQuery();
                Console.WriteLine(result > 0 ? "Grade deleted (logged in history)!" : "Grade not found.");
            }
        }

        static void ViewStudentAverage()
        {
            Console.Write("\nEnter Student ID: ");
            int id = int.Parse(Console.ReadLine());

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT s.firstname, s.lastname, 
                           AVG(CAST(g.score AS FLOAT)) as average_score,
                           COUNT(g.gradeid) as total_grades
                    FROM grades g
                    JOIN students s ON g.studentid = s.studentid
                    WHERE s.studentid = @id
                    GROUP BY s.firstname, s.lastname";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    Console.WriteLine($"\nStudent: {reader["firstname"]} {reader["lastname"]}");
                    Console.WriteLine($"Average Score: {reader["average_score"]:F2}");
                    Console.WriteLine($"Total Grades: {reader["total_grades"]}");
                }
                else
                {
                    Console.WriteLine("Student not found or no grades.");
                }
                reader.Close();
            }
        }

        static void ManageAttendance()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== ATTENDANCE MANAGEMENT ===");
                Console.WriteLine("1. View attendance records");
                Console.WriteLine("2. Mark attendance");
                Console.WriteLine("3. Update attendance");
                Console.WriteLine("4. Delete attendance");
                Console.WriteLine("5. View attendance report");
                Console.WriteLine("0. Back");
                Console.Write("Select option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": ViewAttendance(); break;
                    case "2": MarkAttendance(); break;
                    case "3": UpdateAttendance(); break;
                    case "4": DeleteAttendance(); break;
                    case "5": ViewAttendanceReport(); break;
                    case "0": return;
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        static void ViewAttendance()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT TOP 20 a.attendanceid, s.firstname + ' ' + s.lastname as studentname,
                           c.classname, a.attendancedate, a.status, a.coment
                    FROM attendance a
                    JOIN students s ON a.studentid = s.studentid
                    JOIN classes c ON a.classid = c.classid
                    ORDER BY a.attendancedate DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                Console.WriteLine("\n{0,-5} {1,-25} {2,-8} {3,-12} {4,-8} {5,-20}",
                    "ID", "Student", "Class", "Date", "Status", "Comment");
                Console.WriteLine(new string('-', 85));

                while (reader.Read())
                {
                    Console.WriteLine("{0,-5} {1,-25} {2,-8} {3,-12:yyyy-MM-dd} {4,-8} {5,-20}",
                        reader["attendanceid"], reader["studentname"], reader["classname"],
                        reader["attendancedate"], reader["status"], reader["coment"] ?? "");
                }
                reader.Close();
            }
        }

        static void MarkAttendance()
        {
            Console.WriteLine("\n--- Mark Attendance ---");
            Console.Write("Student ID: "); int studentId = int.Parse(Console.ReadLine());
            Console.Write("Class ID: "); int classId = int.Parse(Console.ReadLine());
            Console.Write("Date (YYYY-MM-DD): "); DateTime date = DateTime.Parse(Console.ReadLine());
            Console.Write("Status (present/absent/late): "); string status = Console.ReadLine().ToLower();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
                    INSERT INTO attendance (studentid, classid, attendancedate, status)
                    VALUES (@sid, @cid, @date, @status)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@sid", studentId);
                cmd.Parameters.AddWithValue("@cid", classId);
                cmd.Parameters.AddWithValue("@date", date);
                cmd.Parameters.AddWithValue("@status", status);

                int result = cmd.ExecuteNonQuery();
                Console.WriteLine(result > 0 ? "Attendance marked!" : "Failed.");
            }
        }

        static void UpdateAttendance()
        {
            Console.Write("\nEnter Attendance ID: ");
            int id = int.Parse(Console.ReadLine());
            Console.Write("New Status (present/absent/late): ");
            string status = Console.ReadLine().ToLower();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE attendance SET status = @status WHERE attendanceid = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@status", status);
                cmd.Parameters.AddWithValue("@id", id);

                int result = cmd.ExecuteNonQuery();
                Console.WriteLine(result > 0 ? "Attendance updated!" : "Record not found.");
            }
        }

        static void DeleteAttendance()
        {
            Console.Write("\nEnter Attendance ID to delete: ");
            int id = int.Parse(Console.ReadLine());

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM attendance WHERE attendanceid = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                int result = cmd.ExecuteNonQuery();
                Console.WriteLine(result > 0 ? "Attendance record deleted!" : "Record not found.");
            }
        }

        static void ViewAttendanceReport()
        {
            Console.Write("\nEnter Class ID: ");
            int classId = int.Parse(Console.ReadLine());
            Console.Write("Start Date (YYYY-MM-DD): ");
            DateTime startDate = DateTime.Parse(Console.ReadLine());
            Console.Write("End Date (YYYY-MM-DD): ");
            DateTime endDate = DateTime.Parse(Console.ReadLine());

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT 
                        s.firstname + ' ' + s.lastname as studentname,
                        COUNT(CASE WHEN a.status = 'present' THEN 1 END) as present_days,
                        COUNT(CASE WHEN a.status = 'absent' THEN 1 END) as absent_days,
                        COUNT(CASE WHEN a.status = 'late' THEN 1 END) as late_days,
                        COUNT(*) as total_days
                    FROM attendance a
                    JOIN students s ON a.studentid = s.studentid
                    WHERE a.classid = @classid AND a.attendancedate BETWEEN @startdate AND @enddate
                    GROUP BY s.studentid, s.firstname, s.lastname
                    ORDER BY studentname";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@classid", classId);
                cmd.Parameters.AddWithValue("@startdate", startDate);
                cmd.Parameters.AddWithValue("@enddate", endDate);
                SqlDataReader reader = cmd.ExecuteReader();

                Console.WriteLine($"\nAttendance Report for Class {classId}");
                Console.WriteLine($"Period: {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}");
                Console.WriteLine("\n{0,-25} {1,-12} {2,-12} {3,-10} {4,-10}",
                    "Student", "Present", "Absent", "Late", "Total");
                Console.WriteLine(new string('-', 70));

                while (reader.Read())
                {
                    Console.WriteLine("{0,-25} {1,-12} {2,-12} {3,-10} {4,-10}",
                        reader["studentname"], reader["present_days"],
                        reader["absent_days"], reader["late_days"], reader["total_days"]);
                }
                reader.Close();
            }
        }

        static void ManageUsers()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== USERS MANAGEMENT ===");
                Console.WriteLine("1. View all users");
                Console.WriteLine("2. Add new user");
                Console.WriteLine("3. Deactivate user");
                Console.WriteLine("4. View user roles");
                Console.WriteLine("0. Back");
                Console.Write("Select option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": ViewAllUsers(); break;
                    case "2": AddUser(); break;
                    case "3": DeactivateUser(); break;
                    case "4": ViewUserRoles(); break;
                    case "0": return;
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        static void ViewAllUsers()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT u.userid, u.username, u.email, u.lastlogin, u.isactive,
                           CASE WHEN u.studentid IS NOT NULL THEN 'Student'
                                WHEN u.teacherid IS NOT NULL THEN 'Teacher'
                                ELSE 'Admin' END as usertype
                    FROM users u";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                Console.WriteLine("\n{0,-5} {1,-20} {2,-30} {3,-20} {4,-8} {5,-10}",
                    "ID", "Username", "Email", "Last Login", "Active", "Type");
                Console.WriteLine(new string('-', 100));

                while (reader.Read())
                {
                    Console.WriteLine("{0,-5} {1,-20} {2,-30} {3,-20} {4,-8} {5,-10}",
                        reader["userid"], reader["username"], reader["email"] ?? "",
                        reader["lastlogin"]?.ToString() ?? "Never", reader["isactive"], reader["usertype"]);
                }
                reader.Close();
            }
        }

        static void AddUser()
        {
            Console.WriteLine("\n--- Add New User ---");
            Console.Write("Username: "); string username = Console.ReadLine();
            Console.Write("Password Hash: "); string passwordHash = Console.ReadLine();
            Console.Write("Email: "); string email = Console.ReadLine();
            Console.Write("User Type (student/teacher/admin): "); string userType = Console.ReadLine().ToLower();

            int? studentId = null;
            int? teacherId = null;

            if (userType == "student")
            {
                Console.Write("Student ID: "); studentId = int.Parse(Console.ReadLine());
            }
            else if (userType == "teacher")
            {
                Console.Write("Teacher ID: "); teacherId = int.Parse(Console.ReadLine());
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
                    INSERT INTO users (username, passwordhash, email, studentid, teacherid, isactive, createdat)
                    VALUES (@username, @passwordhash, @email, @studentid, @teacherid, 1, GETDATE())";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@passwordhash", passwordHash);
                cmd.Parameters.AddWithValue("@email", string.IsNullOrEmpty(email) ? (object)DBNull.Value : email);
                cmd.Parameters.AddWithValue("@studentid", studentId ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@teacherid", teacherId ?? (object)DBNull.Value);

                int result = cmd.ExecuteNonQuery();
                Console.WriteLine(result > 0 ? "User added successfully!" : "Failed.");
            }
        }

        static void DeactivateUser()
        {
            Console.Write("\nEnter User ID to deactivate: ");
            int id = int.Parse(Console.ReadLine());

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE users SET isactive = 0 WHERE userid = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                int result = cmd.ExecuteNonQuery();
                Console.WriteLine(result > 0 ? "User deactivated!" : "User not found.");
            }
        }

        static void ViewUserRoles()
        {
            Console.Write("\nEnter User ID: ");
            int id = int.Parse(Console.ReadLine());

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT u.username, r.rolename
                    FROM userroles ur
                    JOIN users u ON ur.userid = u.userid
                    JOIN roles r ON ur.roleid = r.roleid
                    WHERE u.userid = @id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader reader = cmd.ExecuteReader();

                Console.WriteLine($"\nRoles for User ID {id}:");
                while (reader.Read())
                {
                    Console.WriteLine($"- {reader["rolename"]}");
                }
                reader.Close();
            }
        }

        static void ShowReports()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== REPORTS & ANALYTICS ===");
                Console.WriteLine("1. Class Average Report");
                Console.WriteLine("2. Top 10 Students by GPA");
                Console.WriteLine("3. Teacher Workload Report");
                Console.WriteLine("4. Assignment Statistics");
                Console.WriteLine("5. Attendance Summary");
                Console.WriteLine("0. Back");
                Console.Write("Select option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": ShowClassAverageReport(); break;
                    case "2": ShowTopStudentsReport(); break;
                    case "3": ShowTeacherWorkloadReport(); break;
                    case "4": ShowAssignmentStatistics(); break;
                    case "5": ShowAttendanceSummary(); break;
                    case "0": return;
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        static void ShowClassAverageReport()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT c.classname, AVG(CAST(g.score AS FLOAT)) as class_average
                    FROM grades g
                    JOIN students s ON g.studentid = s.studentid
                    JOIN enrollments e ON s.studentid = e.studentid
                    JOIN classes c ON e.classid = c.classid
                    GROUP BY c.classname
                    ORDER BY class_average DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                Console.WriteLine("\n=== Class Average Report ===");
                Console.WriteLine("{0,-10} {1,-15}", "Class", "Average Score");
                Console.WriteLine(new string('-', 30));

                while (reader.Read())
                {
                    Console.WriteLine("{0,-10} {1,-15:F2}", reader["classname"], reader["class_average"] ?? 0);
                }
                reader.Close();
            }
        }

        static void ShowTopStudentsReport()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT TOP 10 
                        s.firstname + ' ' + s.lastname as studentname,
                        AVG(CAST(g.score AS FLOAT)) as avg_score,
                        COUNT(g.gradeid) as total_grades
                    FROM grades g
                    JOIN students s ON g.studentid = s.studentid
                    GROUP BY s.studentid, s.firstname, s.lastname
                    ORDER BY avg_score DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                Console.WriteLine("\n=== TOP 10 STUDENTS ===");
                Console.WriteLine("{0,-5} {1,-25} {2,-12} {3,-10}", "#", "Student Name", "Average Score", "Grades");
                Console.WriteLine(new string('-', 55));

                int rank = 1;
                while (reader.Read())
                {
                    Console.WriteLine("{0,-5} {1,-25} {2,-12:F2} {3,-10}",
                        rank++, reader["studentname"], reader["avg_score"], reader["total_grades"]);
                }
                reader.Close();
            }
        }

        static void ShowTeacherWorkloadReport()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT 
                        t.firstname + ' ' + t.lastname as teachername,
                        COUNT(DISTINCT ts.classid) as classes_count,
                        COUNT(DISTINCT ts.subjectid) as subjects_count,
                        COUNT(DISTINCT a.assignmentid) as assignments_count
                    FROM teachers t
                    LEFT JOIN teachersubjects ts ON t.teacherid = ts.teacherid
                    LEFT JOIN assignments a ON t.teacherid = a.teacherid
                    WHERE t.isactive = 1
                    GROUP BY t.teacherid, t.firstname, t.lastname
                    ORDER BY classes_count DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                Console.WriteLine("\n=== TEACHER WORKLOAD REPORT ===");
                Console.WriteLine("{0,-25} {1,-12} {2,-14} {3,-12}",
                    "Teacher", "Classes", "Subjects", "Assignments");
                Console.WriteLine(new string('-', 65));

                while (reader.Read())
                {
                    Console.WriteLine("{0,-25} {1,-12} {2,-14} {3,-12}",
                        reader["teachername"], reader["classes_count"],
                        reader["subjects_count"], reader["assignments_count"]);
                }
                reader.Close();
            }
        }

        static void ShowAssignmentStatistics()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT 
                        s.subjectname,
                        COUNT(a.assignmentid) as total_assignments,
                        AVG(a.maxscore) as avg_maxscore,
                        MIN(a.duedate) as earliest_duedate,
                        MAX(a.duedate) as latest_duedate
                    FROM subjects s
                    LEFT JOIN assignments a ON s.subjectid = a.subjectid
                    GROUP BY s.subjectname
                    ORDER BY total_assignments DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                Console.WriteLine("\n=== ASSIGNMENT STATISTICS BY SUBJECT ===");
                Console.WriteLine("{0,-20} {1,-10} {2,-12} {3,-12} {4,-12}",
                    "Subject", "Assignments", "Avg Max Score", "Earliest Due", "Latest Due");
                Console.WriteLine(new string('-', 70));

                while (reader.Read())
                {
                    Console.WriteLine("{0,-20} {1,-10} {2,-12:F0} {3,-12:yyyy-MM-dd} {4,-12:yyyy-MM-dd}",
                        reader["subjectname"], reader["total_assignments"] ?? 0,
                        reader["avg_maxscore"] ?? 0, reader["earliest_duedate"] ?? "",
                        reader["latest_duedate"] ?? "");
                }
                reader.Close();
            }
        }

        static void ShowAttendanceSummary()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT 
                        c.classname,
                        COUNT(*) as total_records,
                        SUM(CASE WHEN a.status = 'present' THEN 1 ELSE 0 END) as present_count,
                        SUM(CASE WHEN a.status = 'absent' THEN 1 ELSE 0 END) as absent_count,
                        SUM(CASE WHEN a.status = 'late' THEN 1 ELSE 0 END) as late_count,
                        CAST(SUM(CASE WHEN a.status = 'present' THEN 1 ELSE 0 END) * 100.0 / COUNT(*) AS DECIMAL(5,2)) as attendance_rate
                    FROM attendance a
                    JOIN classes c ON a.classid = c.classid
                    GROUP BY c.classname
                    ORDER BY attendance_rate DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                Console.WriteLine("\n=== ATTENDANCE SUMMARY BY CLASS ===");
                Console.WriteLine("{0,-10} {1,-8} {2,-8} {3,-8} {4,-8} {5,-12}",
                    "Class", "Total", "Present", "Absent", "Late", "Rate %");
                Console.WriteLine(new string('-', 60));

                while (reader.Read())
                {
                    Console.WriteLine("{0,-10} {1,-8} {2,-8} {3,-8} {4,-8} {5,-12:F2}",
                        reader["classname"], reader["total_records"],
                        reader["present_count"], reader["absent_count"],
                        reader["late_count"], reader["attendance_rate"]);
                }
                reader.Close();
            }
        }
    }
}




