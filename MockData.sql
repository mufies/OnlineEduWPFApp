-- Mock Data for Student Management System
-- Run this script after creating the database

USE OnlineEduTask;
GO

-- Clear existing data
DELETE FROM StudentTasks;
DELETE FROM ClassTasks;
DELETE FROM Tasks;
DELETE FROM ClassSubjects;
DELETE FROM Students;
DELETE FROM Subjects;
DELETE FROM StudentClasses;
DELETE FROM UserAccounts;
GO

-- Reset identity columns
DBCC CHECKIDENT ('StudentTasks', RESEED, 0);
DBCC CHECKIDENT ('ClassTasks', RESEED, 0);
DBCC CHECKIDENT ('Tasks', RESEED, 0);
DBCC CHECKIDENT ('Students', RESEED, 0);
DBCC CHECKIDENT ('Subjects', RESEED, 0);
DBCC CHECKIDENT ('StudentClasses', RESEED, 0);
DBCC CHECKIDENT ('UserAccounts', RESEED, 0);
GO

-- 1. Insert Classes
SET IDENTITY_INSERT StudentClasses ON;
INSERT INTO StudentClasses (ClassId, ClassCode, ClassName) VALUES
(1, 'SE1801', 'Software Engineering 18.01'),
(2, 'SE1802', 'Software Engineering 18.02'),
(3, 'SE1803', 'Software Engineering 18.03'),
(4, 'AI1801', 'Artificial Intelligence 18.01');
SET IDENTITY_INSERT StudentClasses OFF;
GO

-- 2. Insert Subjects
SET IDENTITY_INSERT Subjects ON;
INSERT INTO Subjects (SubjectId, SubjectCode, SubjectName, Credits) VALUES
(1, 'PRN212', 'Basic Cross-Platform Application Programming with .NET', 3),
(2, 'PRJ301', 'Java Web Application Development', 3),
(3, 'SWP391', 'Software Development Project', 3),
(4, 'PRN221', 'Advanced Cross-Platform Application Programming with .NET', 3),
(5, 'SWE201c', 'Introduction to Software Engineering', 3),
(6, 'DBI202', 'Introduction to Databases', 3);
SET IDENTITY_INSERT Subjects OFF;
GO

-- 3. Insert User Accounts (Admin + Teachers + Students)
SET IDENTITY_INSERT UserAccounts ON;
INSERT INTO UserAccounts (UserAccountId, Username, Password, Role, StudentId, TeacherId) VALUES
-- Admin
(1, 'admin', '123456', 'Admin', NULL, NULL),
-- Teachers
(2, 'teacher1', '123456', 'Teacher', NULL, 1),
(3, 'teacher2', '123456', 'Teacher', NULL, 2),
(4, 'teacher3', '123456', 'Teacher', NULL, 3),
-- Students (will be linked after creating Student records)
(5, 'student1', '123456', 'Student', 1, NULL),
(6, 'student2', '123456', 'Student', 2, NULL),
(7, 'student3', '123456', 'Student', 3, NULL),
(8, 'student4', '123456', 'Student', 4, NULL),
(9, 'student5', '123456', 'Student', 5, NULL),
(10, 'student6', '123456', 'Student', 6, NULL);
SET IDENTITY_INSERT UserAccounts OFF;
GO

-- 4. Insert Students
SET IDENTITY_INSERT Students ON;
INSERT INTO Students (StudentId, StudentCode, FullName, Email, ClassId) VALUES
(1, 'SE180001', 'Trần Thị Học', 'student1@school.com', 1),
(2, 'SE180002', 'Lê Văn Thông', 'student2@school.com', 1),
(3, 'SE180003', 'Nguyễn Văn Minh', 'student3@school.com', 2),
(4, 'SE180004', 'Phạm Thị Lan', 'student4@school.com', 2),
(5, 'SE180005', 'Hoàng Văn Nam', 'student5@school.com', 3),
(6, 'AI180001', 'Đỗ Thị Hương', 'student6@school.com', 4);
SET IDENTITY_INSERT Students OFF;
GO

-- 5. Insert ClassSubjects (Assign subjects to classes with teachers)
INSERT INTO ClassSubjects (ClassId, SubjectId, TeacherId) VALUES
-- SE1801: PRN212, PRJ301, DBI202
(1, 1, 1),  -- Teacher 1 teaches PRN212 to SE1801
(1, 2, 2),  -- Teacher 2 teaches PRJ301 to SE1801
(1, 6, 3),  -- Teacher 3 teaches DBI202 to SE1801

-- SE1802: PRN221, SWP391, PRJ301
(2, 4, 1),  -- Teacher 1 teaches PRN221 to SE1802
(2, 3, 2),  -- Teacher 2 teaches SWP391 to SE1802
(2, 2, 3),  -- Teacher 3 teaches PRJ301 to SE1802

-- SE1803: SWE201c, DBI202
(3, 5, 1),  -- Teacher 1 teaches SWE201c to SE1803
(3, 6, 2),  -- Teacher 2 teaches DBI202 to SE1803

-- AI1801: PRN212, PRN221
(4, 1, 2),  -- Teacher 2 teaches PRN212 to AI1801
(4, 4, 3);  -- Teacher 3 teaches PRN221 to AI1801
GO

-- 6. Insert Tasks (created by teachers)
SET IDENTITY_INSERT Tasks ON;
INSERT INTO Tasks (TaskId, Title, Description, CreatedByTeacherId, CreatedDate) VALUES
(1, 'Assignment 1: WPF Basics', 'Create a simple WPF application with login form', 1, '2024-10-01'),
(2, 'Assignment 2: Entity Framework', 'Implement CRUD operations using EF Core', 1, '2024-10-08'),
(3, 'Lab 1: JSP Basics', 'Create a simple JSP web application', 2, '2024-10-02'),
(4, 'Project Proposal', 'Submit your project proposal document', 2, '2024-10-05'),
(5, 'Lab 2: Database Design', 'Design database for e-commerce system', 3, '2024-10-03');
SET IDENTITY_INSERT Tasks OFF;
GO

-- 7. Insert ClassTasks (assign tasks to classes)
SET IDENTITY_INSERT ClassTasks ON;
INSERT INTO ClassTasks (ClassTaskId, TaskId, ClassId, SubjectId, DueDate, MaxScore) VALUES
-- Task 1 for SE1801 PRN212
(1, 1, 1, 1, '2024-10-15', 10),
-- Task 2 for SE1801 PRN212
(2, 2, 1, 1, '2024-10-22', 10),
-- Task 3 for SE1801 PRJ301
(3, 3, 1, 2, '2024-10-16', 10),
-- Task 4 for SE1802 SWP391
(4, 4, 2, 3, '2024-10-20', 10),
-- Task 5 for SE1801 DBI202
(5, 5, 1, 6, '2024-10-18', 10);
SET IDENTITY_INSERT ClassTasks OFF;
GO

-- 8. Insert StudentTasks (auto-created when ClassTask is assigned)
SET IDENTITY_INSERT StudentTasks ON;
INSERT INTO StudentTasks (StudentTaskId, ClassTaskId, StudentId, IsSubmitted, SubmissionContent, SubmittedDate, Score, Feedback) VALUES
-- ClassTask 1 (Task 1 for SE1801) - 2 students
(1, 1, 1, 1, 'Here is my WPF login form project. I implemented validation and modern UI.', '2024-10-14', 9.5, 'Good work! Clean code structure.'),
(2, 1, 2, 1, 'My WPF application with MVVM pattern.', '2024-10-15', 8.0, 'Good but needs improvement on UI design.'),

-- ClassTask 2 (Task 2 for SE1801) - 2 students
(3, 2, 1, 1, 'CRUD operations implemented with EF Core and SQL Server.', '2024-10-21', 10, 'Excellent! Perfect implementation.'),
(4, 2, 2, 0, NULL, NULL, NULL, NULL),  -- Not submitted yet

-- ClassTask 3 (Task 3 for SE1801) - 2 students
(5, 3, 1, 1, 'JSP application with login and CRUD features.', '2024-10-16', 8.5, 'Good work, but missing some validation.'),
(6, 3, 2, 1, 'Simple JSP web app.', '2024-10-16', 7.0, 'Basic implementation, needs more features.'),

-- ClassTask 4 (Task 4 for SE1802) - 2 students
(7, 4, 3, 1, 'Project proposal for online learning platform.', '2024-10-19', 9.0, 'Great idea and well-structured proposal.'),
(8, 4, 4, 0, NULL, NULL, NULL, NULL),  -- Not submitted yet

-- ClassTask 5 (Task 5 for SE1801) - 2 students
(9, 5, 1, 0, NULL, NULL, NULL, NULL),  -- Not submitted yet
(10, 5, 2, 1, 'E-commerce database design with ER diagram.', '2024-10-17', 8.5, 'Good design but missing some tables.');
SET IDENTITY_INSERT StudentTasks OFF;
GO

-- Verify data
SELECT 'Classes' as TableName, COUNT(*) as RowCount FROM StudentClasses
UNION ALL
SELECT 'Subjects', COUNT(*) FROM Subjects
UNION ALL
SELECT 'UserAccounts', COUNT(*) FROM UserAccounts
UNION ALL
SELECT 'Students', COUNT(*) FROM Students
UNION ALL
SELECT 'ClassSubjects', COUNT(*) FROM ClassSubjects
UNION ALL
SELECT 'Tasks', COUNT(*) FROM Tasks
UNION ALL
SELECT 'ClassTasks', COUNT(*) FROM ClassTasks
UNION ALL
SELECT 'StudentTasks', COUNT(*) FROM StudentTasks;
GO

-- Show ClassSubjects with Teacher assignments
SELECT 
    sc.ClassCode,
    sc.ClassName,
    s.SubjectCode,
    s.SubjectName,
    CASE 
        WHEN cs.TeacherId IS NOT NULL THEN 'Teacher ' + CAST(cs.TeacherId AS VARCHAR)
        ELSE 'No Teacher Assigned'
    END as AssignedTeacher
FROM ClassSubjects cs
JOIN StudentClasses sc ON cs.ClassId = sc.ClassId
JOIN Subjects s ON cs.SubjectId = s.SubjectId
ORDER BY sc.ClassCode, s.SubjectCode;
GO
