-- phpMyAdmin SQL Dump
-- version 3.4.11.1deb2+deb7u1
-- http://www.phpmyadmin.net
--
-- Host: localhost
-- Generation Time: Apr 05, 2015 at 09:24 AM
-- Server version: 5.5.41
-- PHP Version: 5.6.7-1~dotdeb.2

SET SQL_MODE="NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

--
-- Database: `studis`
--

-- --------------------------------------------------------

--
-- Table structure for table `predmet`
--

CREATE TABLE IF NOT EXISTS `predmet` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `ime` varchar(45) COLLATE utf8_unicode_ci NOT NULL,
  `opis` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `kreditne` int(11) NOT NULL,
  `semester` int(11) NOT NULL,
  `koda` varchar(45) COLLATE utf8_unicode_ci DEFAULT NULL,
  `letnik` int(1) NOT NULL,
  `obvezen` tinyint(1) NOT NULL,
  `modulId` int(10) unsigned DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `predmet_modul_fk_idx` (`modulId`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci AUTO_INCREMENT=154 ;

--
-- Dumping data for table `predmet`
--

INSERT INTO `predmet` (`id`, `ime`, `opis`, `kreditne`, `semester`, `koda`, `letnik`, `obvezen`, `modulId`) VALUES
(1, 'Aktualno raziskovalno področje II', NULL, 6, 2, '63546B', 0, 0, NULL),
(2, 'Aktualno raziskovalno področje II', NULL, 6, 2, '63546A', 0, 0, NULL),
(3, 'Algoritmi', NULL, 6, 2, '63508', 0, 0, NULL),
(4, 'Algoritmi in podatkovne strukture 2', NULL, 6, 2, '63723', 0, 0, NULL),
(5, 'Algoritmi in podatkovne strukture 2', NULL, 6, 2, '63280', 0, 0, NULL),
(6, 'Angleški jezik nivo A', NULL, 6, 2, '63222', 0, 0, NULL),
(7, 'Angleški jezik nivo C', NULL, 6, 2, '63224', 0, 0, NULL),
(8, 'Arhitekture in algoritmi', NULL, 6, 2, '63812', 0, 0, NULL),
(9, 'Brezžična in mobilna omrežja', NULL, 6, 2, '63259', 3, 0, 4),
(10, 'Brezžična senzorska omrežja', NULL, 6, 2, '63511', 0, 0, NULL),
(11, 'Digitalno procesiranje signalov', NULL, 6, 2, '63744', 0, 0, NULL),
(12, 'Diskretna matematika', NULL, 6, 2, '63532', 0, 0, NULL),
(13, 'Do pasu v blatu', NULL, 6, 2, '133000', 0, 0, NULL),
(14, 'Ekonomika in podjetništvo', NULL, 6, 2, '63248', 3, 1, NULL),
(15, 'Inovativnost in razvoj novih izdelkov (EF)', NULL, 6, 2, '19003', 0, 0, NULL),
(16, 'Interaktivnost in oblikovanje informacij', NULL, 6, 2, '63527', 0, 0, NULL),
(17, 'Izbrana poglavja iz računalništva in informat', NULL, 6, 2, '63536', 0, 0, NULL),
(18, 'Izbrana poglavja iz računalništva in informat', NULL, 6, 2, '63818', 0, 0, NULL),
(19, 'Izvedbe algoritmov (TP)', NULL, 6, 2, '63754', 0, 0, NULL),
(20, 'Izvedbe algoritmov (TP A)', NULL, 6, 2, '63275A', 0, 0, NULL),
(21, 'Izvedbe algoritmov (TP B)', NULL, 6, 2, '63275B', 0, 0, NULL),
(22, 'Komunikacija človek računalnik', NULL, 6, 2, '63210', 0, 0, NULL),
(23, 'Linearna algebra', NULL, 6, 2, '63207', 0, 0, NULL),
(24, 'Magistrsko delo', NULL, 6, 2, '63548', 0, 0, NULL),
(25, 'Matematične metode v računalništvu', NULL, 6, 2, '63814', 0, 0, NULL),
(26, 'Matematično modeliranje', NULL, 6, 2, '63219', 0, 0, NULL),
(27, 'Načrtovanje digitalnih naprav', NULL, 6, 2, '63729', 0, 0, NULL),
(28, 'Numerična matematika', NULL, 6, 2, '63522', 0, 0, NULL),
(29, 'Obvladovanje informatike', NULL, 6, 2, '63526', 0, 0, NULL),
(30, 'Odkrivanje znanj iz podatkov', NULL, 6, 2, '63525', 0, 0, NULL),
(31, 'Operacijski sistemi', NULL, 6, 2, '63709', 0, 0, NULL),
(32, 'Organizacija in management', NULL, 6, 2, '63250', 3, 0, 1),
(33, 'Organizacija računalniških sistemov', NULL, 6, 2, '63218', 0, 0, NULL),
(34, 'Osnove oblikovanja', NULL, 6, 2, '63271', 3, 0, 8),
(35, 'Osnove podatkovnih baz', NULL, 6, 2, '63208', 0, 0, NULL),
(36, 'Osnove verjetnosti in statistike', NULL, 6, 2, '63710', 0, 0, NULL),
(37, 'Planiranje in upravljanje informatike', NULL, 6, 2, '63253', 0, 0, NULL),
(38, 'Podatkovne baze', NULL, 6, 2, '63707', 0, 0, NULL),
(39, 'Podatkovno rudarjenje', NULL, 6, 2, '63765', 0, 0, NULL),
(40, 'Prevajalniki', NULL, 6, 2, '63265', 3, 0, 6),
(41, 'Prevajalniki in navidezni stroji', NULL, 6, 2, '63722', 0, 0, NULL),
(42, 'Principi programskih jezikov', NULL, 6, 2, '63220', 0, 0, NULL),
(43, 'Produkcija multimedijskih gradiv', NULL, 6, 2, '63726', 0, 0, NULL),
(44, 'Programiranje 2', NULL, 6, 2, '63706', 0, 0, NULL),
(45, 'Programiranje 2', NULL, 6, 2, '63278', 0, 0, NULL),
(46, 'Računalniška forenzika', NULL, 6, 2, '63530', 0, 0, NULL),
(47, 'Računalniške komunikacije', NULL, 6, 2, '63708', 0, 0, NULL),
(48, 'Računalniške komunikacije', NULL, 6, 2, '63209', 0, 0, NULL),
(49, 'Računalniške tehnologije', NULL, 6, 2, '63221', 0, 0, NULL),
(50, 'Računalniški sistemi', NULL, 6, 2, '63509', 0, 0, NULL),
(51, 'Računska topologija', NULL, 6, 2, '63542', 0, 0, NULL),
(52, 'Razvoj informacijskih sistemov', NULL, 6, 2, '63725', 3, 0, 2),
(53, 'Razvoj inteligentnih sistemov', NULL, 6, 2, '63268', 3, 0, 7),
(54, 'Seminar 2 in Seminar 4', NULL, 6, 2, '6', 0, 0, NULL),
(55, 'Sodobne metode razvoja programske opreme', NULL, 6, 2, '63515', 0, 0, NULL),
(56, 'Spletne tehnologije', NULL, 6, 2, '63727', 0, 0, NULL),
(57, 'Tehnologija programske opreme', NULL, 6, 2, '63256', 0, 0, NULL),
(58, 'Teorija informacij in sistemov', NULL, 6, 2, '63216', 0, 0, NULL),
(59, 'Testiranje in kakovost', NULL, 6, 2, '63724', 0, 0, NULL),
(60, 'Umetna inteligenca', NULL, 6, 2, '63510', 0, 0, NULL),
(61, 'Umetna inteligenca', NULL, 6, 2, '63813', 0, 0, NULL),
(62, 'Umetna inteligenca 1', NULL, 6, 2, '63501', 0, 0, NULL),
(63, 'Uporabniški vmesniki', NULL, 6, 2, '63721', 0, 0, NULL),
(64, 'Veščine v znanstvenem delu II', NULL, 6, 2, '63803', 0, 0, NULL),
(65, 'Vhodno-izhodne naprave', NULL, 6, 2, '63728', 0, 0, NULL),
(66, 'Zagovori diplomskih nalog', NULL, 6, 2, '7', 0, 0, NULL),
(67, 'Zanesljivost in zmogljivost računalniških sis', NULL, 6, 2, '63262', 3, 0, 5),
(68, 'Aktualno raziskovalno področje I', NULL, 6, 1, '63545', 0, 0, NULL),
(69, 'Algoritmi in podatkovne strukture 1', NULL, 6, 1, '63711', 0, 0, NULL),
(70, 'Algoritmi in podatkovne strukture 1', NULL, 6, 1, '63279', 0, 0, NULL),
(71, 'Angleški jezik nivo B', NULL, 6, 1, '63223', 0, 0, NULL),
(72, 'Angleški jezik nivo B', NULL, 6, 1, '63746', 0, 0, NULL),
(73, 'Arhitektura računalniških sistemov', NULL, 6, 1, '63212', 0, 0, NULL),
(74, 'Digitalna vezja', NULL, 6, 1, '63718', 0, 0, NULL),
(75, 'Digitalno načrtovanje', NULL, 6, 1, '63260', 3, 0, 5),
(76, 'Digitalno procesiranje signalov', NULL, 6, 1, '63516', 0, 0, NULL),
(77, 'Diskretne strukture', NULL, 6, 1, '63203', 0, 0, NULL),
(78, 'Diskretne strukture', NULL, 6, 1, '63705', 0, 0, NULL),
(79, 'Do pasu v blatu', NULL, 6, 1, '133000', 0, 0, NULL),
(80, 'E-izobraževanje', NULL, 6, 1, '63518', 0, 0, NULL),
(81, 'Elektronsko in mobilno poslovanje', NULL, 6, 1, '63712', 0, 0, NULL),
(82, 'Elektronsko poslovanje', NULL, 6, 1, '63249', 3, 0, 1),
(83, 'Fizika', NULL, 6, 1, '63205', 0, 0, NULL),
(84, 'Grafično oblikovanje', NULL, 6, 1, '63715', 0, 0, NULL),
(85, 'Informacijska varnost in zasebnost', NULL, 6, 1, '63521', 0, 0, NULL),
(86, 'Informacijski sistemi', NULL, 6, 1, '63714', 0, 0, NULL),
(87, 'Informatika', NULL, 6, 1, '1', 0, 0, NULL),
(88, 'Inteligentni sistemi', NULL, 6, 1, '63266', 3, 0, 7),
(89, 'Izbrana poglavja iz računalništva in informat', NULL, 6, 1, '63225', 0, 0, NULL),
(90, 'Izbrana poglavja iz računalništva in informat', NULL, 6, 1, '63749B', 0, 0, NULL),
(91, 'Izbrana poglavja iz računalništva in informat', NULL, 6, 1, '63749A', 0, 0, NULL),
(92, 'Izračunljivost in računska zahtevnost', NULL, 6, 1, '63517', 3, 0, 6),
(93, 'Komuniciranje in vodenje projektov', NULL, 6, 1, '63246', 3, 1, NULL),
(94, 'Komunikacijski protokoli', NULL, 6, 1, '63258', 3, 0, 4),
(95, 'Komunikacijski protokoli in omrežna varnost', NULL, 6, 1, '63716', 0, 0, NULL),
(96, 'Kriptografija in računalniška varnost', NULL, 6, 1, '63528', 0, 0, NULL),
(97, 'Management proizvodnih in storitvenih proceso', NULL, 6, 1, '63533', 0, 0, NULL),
(98, 'Matematika', NULL, 6, 1, '63704', 0, 0, NULL),
(99, 'Matematika II', NULL, 6, 1, '63506', 0, 0, NULL),
(100, 'Modeliranje računalniških omrežij', NULL, 6, 1, '63257', 3, 0, 4),
(101, 'Multimedijske tehnologije', NULL, 6, 1, '63734', 0, 0, NULL),
(102, 'Multimedijski sistemi', NULL, 6, 1, '63270', 3, 0, 8),
(103, 'Nekonvencionalne platforme in metode procesir', NULL, 6, 1, '63512', 0, 0, NULL),
(104, 'Numerične metode', NULL, 6, 1, '63742', 0, 0, NULL),
(105, 'Obdelava biomedicinskih signalov in slik', NULL, 6, 1, '63514', 0, 0, NULL),
(106, 'Odločitveni sistemi', NULL, 6, 1, '63741', 0, 0, NULL),
(107, 'Operacijski sistemi', NULL, 6, 1, '63217', 0, 0, NULL),
(108, 'Organizacija računalnikov', NULL, 6, 1, '63717', 0, 0, NULL),
(109, 'Osnove digitalnih vezij', NULL, 6, 1, '63204', 0, 0, NULL),
(110, 'Osnove informacijskih sistemov', NULL, 6, 1, '63215', 0, 0, NULL),
(111, 'Osnove matematične analize', NULL, 6, 1, '63202', 0, 0, NULL),
(112, 'Osnove umetne inteligence', NULL, 6, 1, '63214', 0, 0, NULL),
(113, 'Podatkovne baze 2', NULL, 6, 1, '63713', 0, 0, NULL),
(114, 'Porazdeljeni sistemi', NULL, 6, 1, '63261', 3, 0, 5),
(115, 'Poslovna inteligenca', NULL, 6, 1, '63251', 3, 0, 1),
(116, 'Postopki razvoja programske opreme', NULL, 6, 1, '63254', 3, 0, 3),
(117, 'Poučevanje algoritmičnega razmišljanja', NULL, 6, 1, '63547', 0, 0, NULL),
(118, 'Procesna avtomatika', NULL, 6, 1, '63737', 0, 0, NULL),
(119, 'Programiranje', NULL, 6, 1, '63507', 0, 0, NULL),
(120, 'Programiranje', NULL, 6, 1, '63502', 0, 0, NULL),
(121, 'Programiranje 1', NULL, 6, 1, '63702', 0, 0, NULL),
(122, 'Programiranje 1', NULL, 6, 1, '63277', 0, 0, NULL),
(123, 'Projektni praktikum', NULL, 6, 1, '63755', 0, 0, NULL),
(124, 'Računalniška arhitektura', NULL, 6, 1, '63703', 0, 0, NULL),
(125, 'Računalniška grafika', NULL, 6, 1, '63719', 3, 0, 8),
(126, 'Računalniška grafika in tehnologija iger', NULL, 6, 1, '63269', 0, 0, NULL),
(127, 'Računalniška orodja, jeziki in okolja (TP A)', NULL, 6, 1, '63764A', 0, 0, NULL),
(128, 'Računalniška orodja, jeziki in okolja (TP B)', NULL, 6, 1, '63764B', 0, 0, NULL),
(129, 'Računalniški sistemi', NULL, 6, 1, '4', 0, 0, NULL),
(130, 'Računska zahtevnost in hevristično programira', NULL, 6, 1, '63263', 0, 0, NULL),
(131, 'Razvoj informacijskih sistemov', NULL, 6, 1, '63252', 0, 0, NULL),
(132, 'Razvoj programske opreme', NULL, 6, 1, '2', 0, 0, NULL),
(133, 'Robotika in računalniško zaznavanje', NULL, 6, 1, '63739', 0, 0, NULL),
(134, 'Seminar 1 in Seminar 3', NULL, 6, 1, '5', 0, 0, NULL),
(135, 'Sistemska programska oprema', NULL, 6, 1, '63264', 3, 0, 6),
(136, 'Sistemska programska oprema', NULL, 6, 1, '63736', 0, 0, NULL),
(137, 'Spletno programiranje', NULL, 6, 1, '63255', 3, 0, 3),
(138, 'Strateško planiranje informatike', NULL, 6, 1, '63733', 3, 0, 2),
(139, 'Strojno učenje', NULL, 6, 1, '63519', 0, 0, NULL),
(140, 'Športna vzgoja', NULL, 6, 1, '63750', 0, 0, NULL),
(141, 'Tehnologija iger in navidezna resničnost', NULL, 6, 1, '63740', 0, 0, NULL),
(142, 'Tehnologija programske opreme', NULL, 6, 1, '63732', 3, 0, 3),
(143, 'Tehnologija upravljanja podatkov', NULL, 6, 1, '63226', 3, 0, 2),
(144, 'Umetna inteligenca', NULL, 6, 1, '63720', 0, 0, NULL),
(145, 'Umetno zaznavanje', NULL, 6, 1, '63267', 3, 0, 7),
(146, 'Uporaba IKT v naravoslovju in tehniki (UN-FKK', NULL, 6, 1, '90071', 0, 0, NULL),
(147, 'Uvod v bioinformatiko', NULL, 6, 1, '63520', 0, 0, NULL),
(148, 'Uvod v računalništvo', NULL, 6, 1, '63701', 0, 0, NULL),
(149, 'Verjetnost in statistika', NULL, 6, 1, '63213', 0, 0, NULL),
(150, 'Veščine v znanstvenem delu 1', NULL, 6, 1, '3', 0, 0, NULL),
(151, 'Vgrajeni sistemi', NULL, 6, 1, '63738', 0, 0, NULL),
(152, 'Vzporedni in porazdeljeni sistemi in algoritm', NULL, 6, 1, '63735', 0, 0, NULL),
(153, 'Zagovori diplomskih nalog', NULL, 6, 1, '7', 0, 0, NULL),
(154, 'Diplomsko delo', NULL, 6, 2, '63532', 3, 1, NULL);
--
-- Constraints for dumped tables
--

--
-- Constraints for table `predmet`
--
ALTER TABLE `predmet`
  ADD CONSTRAINT `predmet_modul_fk` FOREIGN KEY (`modulId`) REFERENCES `modul` (`id`) ON DELETE CASCADE ON UPDATE CASCADE;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
