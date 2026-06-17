# Electronic Diary Management System

[![GitHub license](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
[![Database](https://img.shields.io/badge/database-SQL%20Server-red.svg)](https://www.microsoft.com/sql-server)
[![.NET](https://img.shields.io/badge/.NET-6.0-purple.svg)](https://dotnet.microsoft.com/)

## Description

**Electronic Diary** is a comprehensive school electronic diary management system. The system allows managing students, teachers, subjects, grades, attendance and more.

### Key Features

- Student and teacher management
- Subject and class management
- Grade tracking and grading
- Attendance tracking
- Access control with roles (Admin, Teacher, Student, Parent)
- Stored procedures and triggers for business logic
- Transactions for data integrity
- Console application in C# for database interaction

## Database Structure

### Entities (12 main tables)

| Table | Description |
|-------|-------------|
| Students | Student information |
| Teachers | Teacher information |
| Subjects | List of subjects |
| Classes | Classes and groups |
| Enrollments | Student enrollment in classes |
| TeacherSubjects | Teacher assignments to subjects |
| Assignments | Homework and tests |
| Grades | Student grades |
| Attendance | Attendance records |
| Users | System users |
| Roles | User roles |
| UserRoles | User-role relationships |

### Additional Tables

- SemesterGrades - Semester final grades
- GradeHistory - Grade change history
- GradeLog - Grade deletion log

## Installation and Setup

### Prerequisites

- SQL Server (2019 or newer) or SQL Server Express
- .NET 6.0 SDK or newer
- Git (optional)


```bash
git clone https://github.com/k1llushka/-_-.git
cd -_-
dotnet run
```
