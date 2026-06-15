create database electronicdiary;
go

use electronicdiary;
go

-- 1. students
create table students (
    studentid int identity(1,1) primary key,
    firstname nvarchar(50) not null,
    lastname nvarchar(50) not null,
    birthdate date,
    email nvarchar(100) unique
);

-- 2. teachers
create table teachers (
    teacherid int identity(1,1) primary key,
    firstname nvarchar(50) not null,
    lastname nvarchar(50) not null,
    email nvarchar(100) unique,
    hiredate date not null default getdate()
);

-- 3. subjects
create table subjects (
    subjectid int identity(1,1) primary key,
    subjectname nvarchar(100) not null unique
);

-- 4. classes
create table classes (
    classid int identity(1,1) primary key,
    classname nvarchar(20) not null unique,
    year int not null,
    teacherid int foreign key references teachers(teacherid)
);

-- 5. enrollments
create table enrollments (
    enrollmentid int identity(1,1) primary key,
    studentid int foreign key references students(studentid),
    classid int foreign key references classes(classid),
    enrollmentdate date not null default getdate(),
    unique(studentid, classid)
);

-- 6. teachersubjects
create table teachersubjects (
    teachersubjectid int identity(1,1) primary key,
    teacherid int foreign key references teachers(teacherid),
    subjectid int foreign key references subjects(subjectid),
    classid int foreign key references classes(classid),
    unique(teacherid, subjectid, classid)
);

-- 7. assignments
create table assignments (
    assignmentid int identity(1,1) primary key,
    subjectid int foreign key references subjects(subjectid),
    teacherid int foreign key references teachers(teacherid),
    classid int foreign key references classes(classid),
    title nvarchar(200) not null,
    maxscore int not null check (maxscore > 0),
    duedate date not null
);

-- 8. grades
create table grades (
    gradeid int identity(1,1) primary key,
    studentid int foreign key references students(studentid),
    assignmentid int foreign key references assignments(assignmentid),
    score int check (score >= 0),
    gradedate date not null default getdate(),
    unique(studentid, assignmentid)
);

-- 9. attendance
create table attendance (
    attendanceid int identity(1,1) primary key,
    studentid int foreign key references students(studentid),
    classid int foreign key references classes(classid),
    attendancedate date not null,
    status nvarchar(10) check (status in ('present', 'absent', 'late')),
    unique(studentid, classid, attendancedate)
);

-- 10. roles
create table roles (
    roleid int identity(1,1) primary key,
    rolename nvarchar(50) not null unique
);

-- 11. users
create table users (
    userid int identity(1,1) primary key,
    username nvarchar(50) not null unique,
    passwordhash nvarchar(255) not null,
    studentid int null foreign key references students(studentid),
    teacherid int null foreign key references teachers(teacherid),
    isactive bit not null default 1
);

-- 12. userroles
create table userroles (
    userroleid int identity(1,1) primary key,
    userid int foreign key references users(userid),
    roleid int foreign key references roles(roleid),
    unique(userid, roleid)
);
go

-- 1. roles
insert into roles (rolename) values 
('admin'),
('teacher'),
('student'),
('parent');
go

-- 2. subjects
insert into subjects (subjectname) values 
('mathematics'),
('physics'),
('chemistry'),
('biology'),
('english literature'),
('history'),
('geography'),
('computer science'),
('physical education'),
('art');
go

-- 3. teachers (10 учителей)
insert into teachers (firstname, lastname, email, hiredate) values 
('john', 'smith', 'john.smith@school.com', '2020-09-01'),
('sarah', 'johnson', 'sarah.johnson@school.com', '2019-08-15'),
('michael', 'williams', 'michael.williams@school.com', '2021-01-10'),
('emily', 'brown', 'emily.brown@school.com', '2018-11-20'),
('james', 'jones', 'james.jones@school.com', '2022-03-05'),
('jessica', 'garcia', 'jessica.garcia@school.com', '2020-07-12'),
('david', 'miller', 'david.miller@school.com', '2019-09-30'),
('lisa', 'davis', 'lisa.davis@school.com', '2021-06-18'),
('robert', 'rodriguez', 'robert.rodriguez@school.com', '2017-04-25'),
('jennifer', 'martinez', 'jennifer.martinez@school.com', '2020-12-03');
go

-- 4. classes (8 классов)
insert into classes (classname, year, teacherid) values 
('10a', 2025, 1),   -- class teacher: john smith
('10b', 2025, 2),   -- class teacher: sarah johnson
('10c', 2025, 3),   -- class teacher: michael williams
('11a', 2025, 4),   -- class teacher: emily brown
('11b', 2025, 5),   -- class teacher: james jones
('9a', 2025, 6),    -- class teacher: jessica garcia
('9b', 2025, 7),    -- class teacher: david miller
('12a', 2025, 8);   -- class teacher: lisa davis
go

-- 5. students (30 студентов)
insert into students (firstname, lastname, birthdate, email) values 
('tom', 'rogers', '2008-05-12', 'tom.rogers@student.com'),
('lisa', 'ray', '2008-08-23', 'lisa.ray@student.com'),
('alex', 'turner', '2008-03-17', 'alex.turner@student.com'),
('emma', 'watson', '2008-11-05', 'emma.watson@student.com'),
('oliver', 'clark', '2009-01-29', 'oliver.clark@student.com'),
('sophia', 'lewis', '2008-07-14', 'sophia.lewis@student.com'),
('liam', 'walker', '2008-09-08', 'liam.walker@student.com'),
('mia', 'hall', '2009-02-20', 'mia.hall@student.com'),
('noah', 'allen', '2008-12-03', 'noah.allen@student.com'),
('isabella', 'young', '2008-04-18', 'isabella.young@student.com'),
('ethan', 'king', '2009-06-25', 'ethan.king@student.com'),
('amelia', 'wright', '2008-10-11', 'amelia.wright@student.com'),
('logan', 'scott', '2009-03-07', 'logan.scott@student.com'),
('charlotte', 'green', '2008-01-30', 'charlotte.green@student.com'),
('benjamin', 'adams', '2009-08-19', 'benjamin.adams@student.com'),
('ava', 'baker', '2008-05-22', 'ava.baker@student.com'),
('lucas', 'gonzalez', '2009-11-13', 'lucas.gonzalez@student.com'),
('mia', 'nelson', '2008-07-09', 'mia.nelson@student.com'),
('henry', 'carter', '2009-04-27', 'henry.carter@student.com'),
('ella', 'mitchell', '2008-12-15', 'ella.mitchell@student.com'),
('daniel', 'perez', '2009-02-01', 'daniel.perez@student.com'),
('grace', 'roberts', '2008-09-30', 'grace.roberts@student.com'),
('jack', 'phillips', '2009-05-04', 'jack.phillips@student.com'),
('lily', 'campbell', '2008-06-28', 'lily.campbell@student.com'),
('sebastian', 'parker', '2009-10-17', 'sebastian.parker@student.com'),
('zoe', 'evans', '2008-03-03', 'zoe.evans@student.com'),
('mateo', 'edwards', '2009-07-21', 'mateo.edwards@student.com'),
('natalie', 'collins', '2008-11-09', 'natalie.collins@student.com'),
('carter', 'stewart', '2009-01-12', 'carter.stewart@student.com'),
('victoria', 'morris', '2008-05-26', 'victoria.morris@student.com');
go

-- 6. enrollments (привязка студентов к классам)
insert into enrollments (studentid, classid, enrollmentdate) values 
-- 10a (classid 1): students 1-8
(1, 1, '2024-09-01'), (2, 1, '2024-09-01'), (3, 1, '2024-09-01'), (4, 1, '2024-09-01'),
(5, 1, '2024-09-01'), (6, 1, '2024-09-01'), (7, 1, '2024-09-01'), (8, 1, '2024-09-01'),
-- 10b (classid 2): students 9-16
(9, 2, '2024-09-01'), (10, 2, '2024-09-01'), (11, 2, '2024-09-01'), (12, 2, '2024-09-01'),
(13, 2, '2024-09-01'), (14, 2, '2024-09-01'), (15, 2, '2024-09-01'), (16, 2, '2024-09-01'),
-- 10c (classid 3): students 17-24
(17, 3, '2024-09-01'), (18, 3, '2024-09-01'), (19, 3, '2024-09-01'), (20, 3, '2024-09-01'),
(21, 3, '2024-09-01'), (22, 3, '2024-09-01'), (23, 3, '2024-09-01'), (24, 3, '2024-09-01'),
-- 11a (classid 4): students 25-30
(25, 4, '2024-09-01'), (26, 4, '2024-09-01'), (27, 4, '2024-09-01'), (28, 4, '2024-09-01'),
(29, 4, '2024-09-01'), (30, 4, '2024-09-01');
go

-- 7. teachersubjects (какие учителя ведут какие предметы в каких классах)
insert into teachersubjects (teacherid, subjectid, classid) values 
-- john smith (teacher 1) teaches mathematics in 10a, 10b, 10c
(1, 1, 1), (1, 1, 2), (1, 1, 3),
-- sarah johnson (teacher 2) teaches physics in 10a, 10b, 11a
(2, 2, 1), (2, 2, 2), (2, 2, 4),
-- michael williams (teacher 3) teaches chemistry in 10a, 10b, 10c
(3, 3, 1), (3, 3, 2), (3, 3, 3),
-- emily brown (teacher 4) teaches english literature in 10a, 10b, 11a
(4, 5, 1), (4, 5, 2), (4, 5, 4),
-- james jones (teacher 5) teaches history in 10a, 10b, 10c
(5, 6, 1), (5, 6, 2), (5, 6, 3),
-- jessica garcia (teacher 6) teaches biology in 9a, 9b, 10a
(6, 4, 6), (6, 4, 7), (6, 4, 1),
-- david miller (teacher 7) teaches geography in 9a, 9b, 10b
(7, 7, 6), (7, 7, 7), (7, 7, 2),
-- lisa davis (teacher 8) teaches computer science in 10a, 10b, 10c
(8, 8, 1), (8, 8, 2), (8, 8, 3),
-- robert rodriguez (teacher 9) teaches physical education in all classes
(9, 9, 1), (9, 9, 2), (9, 9, 3), (9, 9, 4), (9, 9, 5), (9, 9, 6), (9, 9, 7), (9, 9, 8),
-- jennifer martinez (teacher 10) teaches art in 9a, 9b, 10a
(10, 10, 6), (10, 10, 7), (10, 10, 1);
go

-- 8. assignments (задания по разным предметам)
insert into assignments (subjectid, teacherid, classid, title, maxscore, duedate) values 
-- mathematics assignments (subject 1)
(1, 1, 1, 'algebra test: quadratic equations', 100, '2025-02-15'),
(1, 1, 1, 'geometry homework: triangles', 50, '2025-02-20'),
(1, 1, 2, 'calculus quiz: derivatives', 75, '2025-02-18'),
(1, 1, 3, 'statistics project', 100, '2025-03-01'),
-- physics assignments (subject 2)
(2, 2, 1, 'newton''s laws worksheet', 50, '2025-02-14'),
(2, 2, 2, 'kinematics lab report', 100, '2025-02-21'),
(2, 2, 4, 'electricity test', 80, '2025-02-25'),
-- chemistry assignments (subject 3)
(3, 3, 1, 'periodic table quiz', 30, '2025-02-16'),
(3, 3, 2, 'chemical bonding homework', 60, '2025-02-19'),
(3, 3, 3, 'stoichiometry problems', 90, '2025-02-28'),
-- english literature assignments (subject 5)
(5, 4, 1, 'essay: shakespeare analysis', 100, '2025-02-22'),
(5, 4, 2, 'poetry presentation', 70, '2025-02-24'),
(5, 4, 4, 'creative writing assignment', 85, '2025-03-03'),
-- history assignments (subject 6)
(6, 5, 1, 'wwii research paper', 100, '2025-02-17'),
(6, 5, 2, 'ancient rome quiz', 40, '2025-02-23'),
(6, 5, 3, 'cold war timeline', 60, '2025-02-27'),
-- biology assignments (subject 4)
(4, 6, 6, 'cell structure test', 80, '2025-02-19'),
(4, 6, 7, 'photosynthesis lab', 90, '2025-02-26'),
(4, 6, 1, 'genetics homework', 70, '2025-03-02'),
-- computer science assignments (subject 8)
(8, 8, 1, 'python programming project', 100, '2025-02-28'),
(8, 8, 2, 'database design quiz', 50, '2025-03-05'),
(8, 8, 3, 'web development assignment', 75, '2025-03-10'),
-- physical education (subject 9)
(9, 9, 1, 'fitness test', 40, '2025-02-25'),
(9, 9, 2, 'team sports evaluation', 50, '2025-03-01'),
-- art assignments (subject 10)
(10, 10, 6, 'still life drawing', 60, '2025-02-28'),
(10, 10, 7, 'color theory project', 80, '2025-03-04');
go

-- вставить исправленные оценки (скрипт выше)
insert into grades (studentid, assignmentid, score, gradedate) values 
(1, 1, 85, '2025-02-16'), (1, 2, 42, '2025-02-21'), (1, 5, 48, '2025-02-15'),
(1, 8, 28, '2025-02-17'), (1, 11, 92, '2025-02-23'), (1, 14, 88, '2025-02-18'),
(2, 1, 91, '2025-02-16'), (2, 2, 47, '2025-02-21'), (2, 5, 45, '2025-02-15'),
(2, 8, 30, '2025-02-17'), (2, 11, 95, '2025-02-23'), (2, 14, 76, '2025-02-18'),
(3, 1, 78, '2025-02-16'), (3, 2, 38, '2025-02-21'), (3, 5, 50, '2025-02-15'),
(3, 8, 25, '2025-02-17'), (3, 11, 82, '2025-02-23'), (3, 14, 91, '2025-02-18'),
(4, 1, 95, '2025-02-16'), (4, 2, 49, '2025-02-21'), (4, 5, 42, '2025-02-15'),
(4, 8, 29, '2025-02-17'), (4, 11, 98, '2025-02-23'), (4, 14, 84, '2025-02-18'),
(5, 1, 67, '2025-02-16'), (5, 2, 35, '2025-02-21'), (5, 5, 38, '2025-02-15'),
(5, 8, 22, '2025-02-17'), (5, 11, 75, '2025-02-23'), (5, 14, 79, '2025-02-18'),
(1, 15, 55, '2025-02-20'), (2, 16, 72, '2025-02-22'), (3, 20, 85, '2025-03-01'),
(4, 15, 68, '2025-02-20'), (6, 16, 45, '2025-02-22'), (7, 20, 90, '2025-03-01'),
(8, 15, 48, '2025-02-20'), (9, 16, 88, '2025-02-22'), (10, 20, 76, '2025-03-01'),
(11, 21, 65, '2025-03-02'), (12, 22, 82, '2025-03-06'), (13, 23, 78, '2025-03-04'),
(14, 24, 92, '2025-03-03'), (15, 21, 45, '2025-03-02'), (16, 22, 71, '2025-03-06'),
(17, 23, 88, '2025-03-04'), (18, 24, 67, '2025-03-03'), (19, 25, 55, '2025-03-01'),
(20, 25, 49, '2025-03-01'), (21, 26, 75, '2025-03-05'), (22, 26, 82, '2025-03-05');
go

-- 10. attendance (посещаемость - для января и февраля 2025)
insert into attendance (studentid, classid, attendancedate, status) values 
-- students from class 10a (classid 1) - january 2025
(1, 1, '2025-01-13', 'present'), (2, 1, '2025-01-13', 'present'), (3, 1, '2025-01-13', 'late'),
(4, 1, '2025-01-13', 'present'), (5, 1, '2025-01-13', 'absent'), (6, 1, '2025-01-13', 'present'),
(7, 1, '2025-01-13', 'present'), (8, 1, '2025-01-13', 'late'),
-- january 20, 2025
(1, 1, '2025-01-20', 'present'), (2, 1, '2025-01-20', 'absent'), (3, 1, '2025-01-20', 'present'),
(4, 1, '2025-01-20', 'present'), (5, 1, '2025-01-20', 'present'), (6, 1, '2025-01-20', 'late'),
(7, 1, '2025-01-20', 'present'), (8, 1, '2025-01-20', 'present'),
-- february 3, 2025
(1, 1, '2025-02-03', 'present'), (2, 1, '2025-02-03', 'present'), (3, 1, '2025-02-03', 'present'),
(4, 1, '2025-02-03', 'late'), (5, 1, '2025-02-03', 'absent'), (6, 1, '2025-02-03', 'present'),
(7, 1, '2025-02-03', 'present'), (8, 1, '2025-02-03', 'present'),
-- class 10b (classid 2) attendance
(9, 2, '2025-01-14', 'present'), (10, 2, '2025-01-14', 'present'), (11, 2, '2025-01-14', 'absent'),
(12, 2, '2025-01-14', 'present'), (13, 2, '2025-01-14', 'late'), (14, 2, '2025-01-14', 'present'),
(15, 2, '2025-01-14', 'present'), (16, 2, '2025-01-14', 'present'),
-- class 10c (classid 3) attendance
(17, 3, '2025-01-15', 'present'), (18, 3, '2025-01-15', 'absent'), (19, 3, '2025-01-15', 'present'),
(20, 3, '2025-01-15', 'present'), (21, 3, '2025-01-15', 'late'), (22, 3, '2025-01-15', 'present'),
(23, 3, '2025-01-15', 'present'), (24, 3, '2025-01-15', 'present'),
-- class 11a (classid 4) attendance
(25, 4, '2025-01-16', 'present'), (26, 4, '2025-01-16', 'present'), (27, 4, '2025-01-16', 'present'),
(28, 4, '2025-01-16', 'late'), (29, 4, '2025-01-16', 'absent'), (30, 4, '2025-01-16', 'present'),
-- february 2025 attendance for class 10a
(1, 1, '2025-02-10', 'present'), (2, 1, '2025-02-10', 'present'), (3, 1, '2025-02-10', 'present'),
(4, 1, '2025-02-10', 'present'), (5, 1, '2025-02-10', 'late'), (6, 1, '2025-02-10', 'absent'),
(7, 1, '2025-02-10', 'present'), (8, 1, '2025-02-10', 'present');
go

-- 11. users (пользователи для авторизации)
insert into users (username, passwordhash, studentid, teacherid, isactive) values 
-- admin user
('admin', 'hash_admin_123', null, null, 1),
-- teachers
('teacher.john', 'hash_john_123', null, 1, 1),
('teacher.sarah', 'hash_sarah_123', null, 2, 1),
('teacher.michael', 'hash_michael_123', null, 3, 1),
('teacher.emily', 'hash_emily_123', null, 4, 1),
('teacher.james', 'hash_james_123', null, 5, 1),
('teacher.jessica', 'hash_jessica_123', null, 6, 1),
('teacher.david', 'hash_david_123', null, 7, 1),
('teacher.lisa', 'hash_lisa_123', null, 8, 1),
('teacher.robert', 'hash_robert_123', null, 9, 1),
('teacher.jennifer', 'hash_jennifer_123', null, 10, 1),
-- students (первые 10 студентов)
('student.tom', 'hash_tom_123', 1, null, 1),
('student.lisa', 'hash_lisa_123', 2, null, 1),
('student.alex', 'hash_alex_123', 3, null, 1),
('student.emma', 'hash_emma_123', 4, null, 1),
('student.oliver', 'hash_oliver_123', 5, null, 1),
('student.sophia', 'hash_sophia_123', 6, null, 1),
('student.liam', 'hash_liam_123', 7, null, 1),
('student.mia', 'hash_mia_123', 8, null, 1),
('student.noah', 'hash_noah_123', 9, null, 1),
('student.isabella', 'hash_isabella_123', 10, null, 1),
-- parents (условно связаны со студентами)
('parent.tom', 'hash_parent_tom', 1, null, 1),
('parent.lisa', 'hash_parent_lisa', 2, null, 1),
('parent.alex', 'hash_parent_alex', 3, null, 1),
('parent.emma', 'hash_parent_emma', 4, null, 1);
go

-- 12. userroles (назначение ролей пользователям)
insert into userroles (userid, roleid) values 
-- admin user (user 1) -> role 1 (admin)
(1, 1),
-- teachers (users 2-11) -> role 2 (teacher)
(2, 2), (3, 2), (4, 2), (5, 2), (6, 2), (7, 2), (8, 2), (9, 2), (10, 2), (11, 2),
-- students (users 12-21) -> role 3 (student)
(12, 3), (13, 3), (14, 3), (15, 3), (16, 3), (17, 3), (18, 3), (19, 3), (20, 3), (21, 3),
-- parents (users 22-25) -> role 4 (parent)
(22, 4), (23, 4), (24, 4), (25, 4);
go

-- роли и права доступа
create role adminrole;
create role teacherrole;
create role studentrole;
create role parentrole;

grant select, insert, update, delete on all tables in schema dbo to adminrole;

grant select, insert, update on grades to teacherrole;
grant select, insert, update on attendance to teacherrole;
grant select, insert, update on assignments to teacherrole;
grant select on students to teacherrole;
grant select on classes to teacherrole;
grant select on subjects to teacherrole;

grant select on grades to studentrole;
grant select on attendance to studentrole;
grant select on grades to parentrole;
grant select on attendance to parentrole;
go

-- представления (views)
create view v_studentgrades as
select 
    s.firstname, s.lastname, sub.subjectname, a.title, g.score, g.gradedate
from grades g
join students s on g.studentid = s.studentid
join assignments a on g.assignmentid = a.assignmentid
join subjects sub on a.subjectid = sub.subjectid;
go

create view v_parentgrades as
select 
    u.username as parentlogin,
    s.firstname as studentfirstname,
    s.lastname as studentlastname,
    sub.subjectname,
    g.score,
    g.gradedate
from users u
join students s on u.studentid = s.studentid
join grades g on s.studentid = g.studentid
join assignments a on g.assignmentid = a.assignmentid
join subjects sub on a.subjectid = sub.subjectid;
go

create view v_classaverage as
select 
    c.classname,
    sub.subjectname,
    avg(cast(g.score as float)) as avgscore
from classes c
join enrollments e on c.classid = e.classid
join students s on e.studentid = s.studentid
join grades g on s.studentid = g.studentid
join assignments a on g.assignmentid = a.assignmentid
join subjects sub on a.subjectid = sub.subjectid
group by c.classname, sub.subjectname;
go

-- триггер 1: проверка максимального балла
create trigger trg_checkscore
on grades
instead of insert
as
begin
    if exists (
        select 1 from inserted i
        join assignments a on i.assignmentid = a.assignmentid
        where i.score > a.maxscore
    )
    begin
        raiserror('score cannot exceed maxscore for this assignment', 16, 1);
        rollback;
    end
    else
    begin
        insert into grades (studentid, assignmentid, score, gradedate)
        select studentid, assignmentid, score, gradedate from inserted;
    end
end;
go

-- таблица логов и триггер 2
create table gradelog (
    logid int identity primary key,
    gradeid int,
    deletedat datetime default getdate(),
    deletedby nvarchar(100) default suser_name()
);
go

create trigger trg_loggradedelete
on grades
after delete
as
begin
    insert into gradelog (gradeid)
    select gradeid from deleted;
end;
go

-- хранимые процедуры
create procedure sp_addgrade
    @studentid int,
    @assignmentid int,
    @score int
as
begin
    begin try
        begin transaction;
        
        if not exists (select 1 from assignments where assignmentid = @assignmentid)
            throw 50001, 'assignment not found', 1;
        
        if @score < 0
            throw 50002, 'score cannot be negative', 1;
        
        insert into grades (studentid, assignmentid, score, gradedate)
        values (@studentid, @assignmentid, @score, getdate());
        
        commit transaction;
    end try
    begin catch
        rollback;
        throw;
    end catch
end;
go

create procedure sp_finalgradebysubject
    @studentid int,
    @subjectid int
as
begin
    select 
        avg(cast(g.score as float)) as finalgrade
    from grades g
    join assignments a on g.assignmentid = a.assignmentid
    where g.studentid = @studentid and a.subjectid = @subjectid;
end;
go

create procedure sp_markattendanceforclass
    @classid int,
    @date date,
    @status nvarchar(10)
as
begin
    begin transaction;
    insert into attendance (studentid, classid, attendancedate, status)
    select studentid, @classid, @date, @status
    from enrollments
    where classid = @classid;
    commit;
end;
go

