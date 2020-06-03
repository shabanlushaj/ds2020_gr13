-- phpMyAdmin SQL Dump
-- version 5.0.2
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Jun 03, 2020 at 06:20 PM
-- Server version: 10.4.11-MariaDB
-- PHP Version: 7.4.5

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `csharp`
--

-- --------------------------------------------------------

--
-- Table structure for table `db`
--

CREATE TABLE `db` (
  `ID` int(11) NOT NULL,
  `User` varchar(50) NOT NULL,
  `Password` varchar(1000) NOT NULL,
  `Salt` varchar(1000) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `db`
--

INSERT INTO `db` (`ID`, `User`, `Password`, `Salt`) VALUES
(3, 'Fiek', 'Kqu3illZYOg75RTBmgqlhDR6rsdgBZh/aS+So+aG7rQ=', 'O0txJwChNNkrXHalsASduA==');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `db`
--
ALTER TABLE `db`
  ADD PRIMARY KEY (`ID`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `db`
--
ALTER TABLE `db`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
