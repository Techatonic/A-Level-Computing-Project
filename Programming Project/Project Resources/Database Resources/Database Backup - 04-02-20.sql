-- phpMyAdmin SQL Dump
-- version 4.9.4
-- https://www.phpmyadmin.net/
--
-- Host: localhost
-- Generation Time: Feb 04, 2020 at 08:46 AM
-- Server version: 10.3.16-MariaDB
-- PHP Version: 7.3.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `id9177556_alevelprojectdatabase`
--

-- --------------------------------------------------------

--
-- Table structure for table `Classes`
--

CREATE TABLE `Classes` (
  `ID` int(11) NOT NULL,
  `TeacherUsername` varchar(40) COLLATE utf8_unicode_ci NOT NULL,
  `ClassName` varchar(40) COLLATE utf8_unicode_ci NOT NULL,
  `SchoolYear` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Dumping data for table `Classes`
--

INSERT INTO `Classes` (`ID`, `TeacherUsername`, `ClassName`, `SchoolYear`) VALUES
(2, 'dale1', 'NewClass', 5),
(3, 'dale1', 'NewClass2', 5),
(4, 'dale1', 'class', 5),
(5, 'dale1', 'afjkdas', 5),
(6, 'dale1', '15315', 5),
(7, 'dale1', 'asj51fds515sdf', 5),
(8, 'dale1', 'hello', 5),
(9, 'dale1', '51648510', 5),
(10, 'jobe1', '5ab1', 5),
(11, 'JoBe1', 'Year 7 Class', 7);

-- --------------------------------------------------------

--
-- Table structure for table `Enrolments`
--

CREATE TABLE `Enrolments` (
  `StudentUsername` varchar(40) COLLATE utf8_unicode_ci NOT NULL,
  `ClassID` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Dumping data for table `Enrolments`
--

INSERT INTO `Enrolments` (`StudentUsername`, `ClassID`) VALUES
('JoBe2', 10),
('JoBl2', 2),
('JoBl2', 3),
('JoBl2', 4),
('JoBl2', 5),
('JoBl2', 6),
('JoBl2', 7),
('JoBl2', 8),
('JoBl2', 9),
('JoBl2', 10);

-- --------------------------------------------------------

--
-- Table structure for table `GameTypes`
--

CREATE TABLE `GameTypes` (
  `ID` int(11) NOT NULL,
  `Name` varchar(40) COLLATE utf8_unicode_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Dumping data for table `GameTypes`
--

INSERT INTO `GameTypes` (`ID`, `Name`) VALUES
(1, 'Linear Spaceship Wars'),
(2, 'Speed Maths'),
(3, 'Algebra Millionaire');

-- --------------------------------------------------------

--
-- Table structure for table `Homeworks`
--

CREATE TABLE `Homeworks` (
  `StudentUsername` varchar(40) COLLATE utf8_unicode_ci NOT NULL,
  `HomeworkID` int(11) NOT NULL,
  `TimeSpent` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Dumping data for table `Homeworks`
--

INSERT INTO `Homeworks` (`StudentUsername`, `HomeworkID`, `TimeSpent`) VALUES
('jobe2', 1, 8493),
('jobe2', 2, 62),
('jobe2', 4, 62),
('jobe2', 5, 62),
('jobe2', 6, 62),
('jobe2', 7, 62),
('jobe2', 8, 62),
('jobe2', 9, 62),
('JoBe2', 10, 302),
('jobe2', 11, 62),
('JoBl2', 10, 1000);

-- --------------------------------------------------------

--
-- Table structure for table `HomeworksSet`
--

CREATE TABLE `HomeworksSet` (
  `ID` int(11) NOT NULL,
  `ClassID` int(11) NOT NULL,
  `HomeworkName` varchar(40) COLLATE utf8_unicode_ci NOT NULL,
  `GameID` int(11) NOT NULL,
  `HomeworkLength` int(11) NOT NULL,
  `DueDate` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Dumping data for table `HomeworksSet`
--

INSERT INTO `HomeworksSet` (`ID`, `ClassID`, `HomeworkName`, `GameID`, `HomeworkLength`, `DueDate`) VALUES
(1, 10, 'hi', 2, 30, '2020-12-02'),
(2, 10, 'New Homework 1', 1, 35, '2020-02-01'),
(3, 11, '4', 1, 40, '2020-02-10'),
(4, 10, '45', 1, 40, '2020-02-10'),
(5, 10, 'd', 1, 40, '2020-02-10'),
(6, 10, 'ddsjkfa', 1, 40, '2020-02-10'),
(7, 10, 'hello world', 1, 45, '2020-02-10'),
(8, 10, 'Test Homework Name', 1, 5, '2020-02-10'),
(9, 10, 'Test Homework Name', 1, 119, '2020-02-10'),
(10, 10, 'Test Homework Name', 1, 10, '2020-02-10'),
(11, 10, 'Test Homework Name', 1, 45, '2020-02-20');

-- --------------------------------------------------------

--
-- Table structure for table `Scores`
--

CREATE TABLE `Scores` (
  `ID` int(11) NOT NULL,
  `StudentUsername` varchar(40) COLLATE utf8_unicode_ci NOT NULL,
  `GameType` int(11) NOT NULL,
  `Score` int(11) NOT NULL,
  `DateCompleted` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Dumping data for table `Scores`
--

INSERT INTO `Scores` (`ID`, `StudentUsername`, `GameType`, `Score`, `DateCompleted`) VALUES
(1, 'jobe2', 3, 1000000, '2020-02-03'),
(2, 'jobe2', 1, 10, '2020-01-12'),
(3, 'jobe2', 2, 31, '2020-01-30');

-- --------------------------------------------------------

--
-- Table structure for table `Students`
--

CREATE TABLE `Students` (
  `FirstName` varchar(40) COLLATE utf8_unicode_ci NOT NULL,
  `LastName` varchar(40) COLLATE utf8_unicode_ci NOT NULL,
  `AccountUsername` varchar(40) COLLATE utf8_unicode_ci NOT NULL,
  `AccountPassword` varchar(40) COLLATE utf8_unicode_ci NOT NULL,
  `SchoolName` varchar(40) COLLATE utf8_unicode_ci NOT NULL,
  `SchoolYear` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Dumping data for table `Students`
--

INSERT INTO `Students` (`FirstName`, `LastName`, `AccountUsername`, `AccountPassword`, `SchoolName`, `SchoolYear`) VALUES
('Josh', 'Beach', 'JoBe2', 'joshbeach', 'Jfs', 5),
('Joe', 'Bloggs', 'JoBl1', 'TestPassword123', 'Jfs', 1),
('Joe', 'Bloggs', 'JoBl2', 'TestPassword123', 'Jfs', 5),
('Joe', 'Bloggs', 'JoBl3', 'TestPassword123', 'Jfs', 15);

-- --------------------------------------------------------

--
-- Table structure for table `Teachers`
--

CREATE TABLE `Teachers` (
  `FirstName` varchar(40) COLLATE utf8_unicode_ci NOT NULL,
  `LastName` varchar(40) COLLATE utf8_unicode_ci NOT NULL,
  `AccountUsername` varchar(40) COLLATE utf8_unicode_ci NOT NULL,
  `AccountPassword` varchar(40) COLLATE utf8_unicode_ci NOT NULL,
  `SchoolName` varchar(40) COLLATE utf8_unicode_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Dumping data for table `Teachers`
--

INSERT INTO `Teachers` (`FirstName`, `LastName`, `AccountUsername`, `AccountPassword`, `SchoolName`) VALUES
('Bob', 'Bobbington', 'BoBo1', 'testpassword', 'Jfs'),
('Dan', 'Leb', 'DaLe1', 'Password123', 'Jfs'),
('Dan', 'Leb', 'DaLe2', 'Password123', 'Jfs'),
('Dan', 'Leb', 'DaLe3', 'Password123', 'W'),
('Dan', 'Leb', 'DaLe4', 'Password123', 'Jfs'),
('Dan', 'Leb', 'DaLe5', '42465181', 'Jfs'),
('Dan', 'Leb', 'DaLe6', 'Password', 'Jfs'),
('Dan', 'Leb', 'DaLe7', 'TestPassword123', 'Jfs'),
('Dan', 'Leb', 'DaLe8', 'testpassword', 'Jfs'),
('Josh', 'Beach', 'JoBe1', 'joshbeach', 'Jfs'),
('John', 'Smith', 'JoSm1', 'Test Password', 'Jfs'),
('John', 'Smith', 'JoSm2', 'Password)£$%^$£', 'Jfs');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `Classes`
--
ALTER TABLE `Classes`
  ADD PRIMARY KEY (`ID`),
  ADD KEY `TeacherUsername` (`TeacherUsername`);

--
-- Indexes for table `Enrolments`
--
ALTER TABLE `Enrolments`
  ADD PRIMARY KEY (`StudentUsername`,`ClassID`),
  ADD KEY `ClassID` (`ClassID`);

--
-- Indexes for table `GameTypes`
--
ALTER TABLE `GameTypes`
  ADD PRIMARY KEY (`ID`);

--
-- Indexes for table `Homeworks`
--
ALTER TABLE `Homeworks`
  ADD PRIMARY KEY (`StudentUsername`,`HomeworkID`),
  ADD KEY `HomeworkID` (`HomeworkID`);

--
-- Indexes for table `HomeworksSet`
--
ALTER TABLE `HomeworksSet`
  ADD PRIMARY KEY (`ID`),
  ADD KEY `ClassID` (`ClassID`),
  ADD KEY `GameID` (`GameID`);

--
-- Indexes for table `Scores`
--
ALTER TABLE `Scores`
  ADD PRIMARY KEY (`ID`),
  ADD KEY `StudentUsername` (`StudentUsername`),
  ADD KEY `GameType` (`GameType`);

--
-- Indexes for table `Students`
--
ALTER TABLE `Students`
  ADD PRIMARY KEY (`AccountUsername`);

--
-- Indexes for table `Teachers`
--
ALTER TABLE `Teachers`
  ADD PRIMARY KEY (`AccountUsername`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `Classes`
--
ALTER TABLE `Classes`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=12;

--
-- AUTO_INCREMENT for table `GameTypes`
--
ALTER TABLE `GameTypes`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT for table `HomeworksSet`
--
ALTER TABLE `HomeworksSet`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=12;

--
-- AUTO_INCREMENT for table `Scores`
--
ALTER TABLE `Scores`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `Classes`
--
ALTER TABLE `Classes`
  ADD CONSTRAINT `Classes_ibfk_1` FOREIGN KEY (`TeacherUsername`) REFERENCES `Teachers` (`AccountUsername`);

--
-- Constraints for table `Enrolments`
--
ALTER TABLE `Enrolments`
  ADD CONSTRAINT `Enrolments_ibfk_1` FOREIGN KEY (`ClassID`) REFERENCES `Classes` (`ID`),
  ADD CONSTRAINT `Enrolments_ibfk_2` FOREIGN KEY (`StudentUsername`) REFERENCES `Students` (`AccountUsername`);

--
-- Constraints for table `Homeworks`
--
ALTER TABLE `Homeworks`
  ADD CONSTRAINT `Homeworks_ibfk_1` FOREIGN KEY (`HomeworkID`) REFERENCES `HomeworksSet` (`ID`),
  ADD CONSTRAINT `Homeworks_ibfk_2` FOREIGN KEY (`StudentUsername`) REFERENCES `Students` (`AccountUsername`);

--
-- Constraints for table `HomeworksSet`
--
ALTER TABLE `HomeworksSet`
  ADD CONSTRAINT `HomeworksSet_ibfk_1` FOREIGN KEY (`ClassID`) REFERENCES `Classes` (`ID`),
  ADD CONSTRAINT `HomeworksSet_ibfk_2` FOREIGN KEY (`GameID`) REFERENCES `GameTypes` (`ID`);

--
-- Constraints for table `Scores`
--
ALTER TABLE `Scores`
  ADD CONSTRAINT `Scores_ibfk_1` FOREIGN KEY (`StudentUsername`) REFERENCES `Students` (`AccountUsername`),
  ADD CONSTRAINT `Scores_ibfk_2` FOREIGN KEY (`GameType`) REFERENCES `GameTypes` (`ID`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
