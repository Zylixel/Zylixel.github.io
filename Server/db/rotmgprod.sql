/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;


CREATE DATABASE IF NOT EXISTS `rotmg` /*!40100 DEFAULT CHARACTER SET latin1 */;
USE `rotmg`;

CREATE TABLE IF NOT EXISTS `accounts` (
  `id` bigint(255) NOT NULL AUTO_INCREMENT,
  `uuid` varchar(128) NOT NULL,
  `password` varchar(256) NOT NULL,
  `name` varchar(64) NOT NULL DEFAULT 'DEFAULT',
  `rank` int(1) NOT NULL DEFAULT '0',
  `namechosen` tinyint(1) NOT NULL DEFAULT '0',
  `verified` tinyint(1) NOT NULL DEFAULT '1',
  `guild` int(11) NOT NULL,
  `guildRank` int(11) NOT NULL,
  `guildFame` int(11) NOT NULL DEFAULT '0',
  `lastip` varchar(128) NOT NULL DEFAULT '',
  `vaultCount` int(11) NOT NULL DEFAULT '1',
  `maxCharSlot` int(11) NOT NULL DEFAULT '3',
  `regTime` datetime NOT NULL DEFAULT '0000-00-00 00:00:00',
  `guest` tinyint(1) NOT NULL DEFAULT '0',
  `banned` tinyint(1) NOT NULL DEFAULT '0',
  `publicMuledump` int(1) NOT NULL DEFAULT '1',
  `muted` tinyint(1) NOT NULL,
  `prodAcc` tinyint(1) NOT NULL DEFAULT '0',
  `locked` varchar(512) NOT NULL,
  `ignored` varchar(512) NOT NULL,
  `gifts` varchar(10000) NOT NULL DEFAULT '',
  `isAgeVerified` tinyint(1) NOT NULL DEFAULT '0',
  `petYardType` int(11) NOT NULL DEFAULT '1',
  `ownedSkins` varchar(2048) NOT NULL DEFAULT '913, 846, 29809, 839, 899, 914, 837, 849, 852, 838, 853, 840, 854, 841, 842, 843, 844, 835, 855, 888, 29790, 845, 847, 848, 836, 0, 9013, 8976, 8855, 29771, 8969, 917, 8979, 9014, 29817, 8968, 9026, 29815, 8977, 9027, 29789, 29801, 29810, 898, 9028, 29770, 9029, 912, 29791, 29799, 29814, 9030, 29800, 903, 902, 8964, 29818, 883, 8965, 8967, 9032, 29811, 915, 885, 29816, 850, 905, 884, 9012, 29813, 916, 8966, 9031, 29808, 872, 904, 834, 5860, 901, 5861, 5866, 5865, 5864, 5863, 5862, 24728, 24729, 10959, 24868, 24849, 24864, 24865, 24850, 24855, 24854, 24856, 24857, 24867, 24852, 24853, 24866, 24851, 24858, 24880, 24872, 24882, 24873, 24871, 24881, 24883, 24884, 24885, 24886, 24887, 24888, 24869, 24870, 24833, 24836, 5859, 24839, 24840, 24841, 24848, 24834, 24835, 24832, 24837, 24889, 24838, 19385, 19152, 19153, 19154, 19155, 19158, 19157, 19156, 19159, 19370, 19371, 19372, 19373, 19374, 19375',
  `authToken` varchar(128) NOT NULL DEFAULT '',
  `acceptedNewTos` tinyint(1) NOT NULL DEFAULT '1',
  `lastSeen` datetime NOT NULL DEFAULT '0000-00-00 00:00:00',
  `accountInUse` tinyint(1) NOT NULL,
  PRIMARY KEY (`id`,`uuid`,`guild`,`lastip`,`banned`)
) ENGINE=MyISAM AUTO_INCREMENT=53 DEFAULT CHARSET=utf8;

CREATE TABLE IF NOT EXISTS `arenalb` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `wave` int(11) NOT NULL,
  `accid` int(11) NOT NULL,
  `charid` int(11) NOT NULL,
  `petid` int(11) DEFAULT NULL,
  `time` varchar(256) NOT NULL,
  `date` datetime NOT NULL,
  PRIMARY KEY (`id`,`wave`)
) ENGINE=MyISAM AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

CREATE TABLE IF NOT EXISTS `backpacks` (
  `accId` int(11) NOT NULL,
  `charId` int(11) NOT NULL,
  `items` varchar(128) NOT NULL DEFAULT '-1, -1, -1, -1, -1, -1, -1, -1',
  PRIMARY KEY (`accId`,`charId`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

CREATE TABLE IF NOT EXISTS `boards` (
  `guildId` int(11) NOT NULL,
  `text` varchar(1024) NOT NULL,
  PRIMARY KEY (`guildId`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

CREATE TABLE IF NOT EXISTS `characters` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `accId` int(11) NOT NULL,
  `charId` int(11) NOT NULL,
  `charType` int(11) NOT NULL DEFAULT '782',
  `level` int(11) NOT NULL DEFAULT '1',
  `exp` int(11) NOT NULL DEFAULT '0',
  `fame` int(11) NOT NULL DEFAULT '0',
  `items` varchar(128) NOT NULL DEFAULT '-1, -1, -1, -1',
  `hpPotions` int(11) NOT NULL DEFAULT '0',
  `mpPotions` int(11) NOT NULL DEFAULT '0',
  `hp` int(11) NOT NULL DEFAULT '1',
  `mp` int(11) NOT NULL DEFAULT '1',
  `stats` varchar(128) NOT NULL DEFAULT '1, 1, 1, 1, 1, 1, 1, 1',
  `dead` tinyint(1) NOT NULL DEFAULT '0',
  `tex1` int(11) NOT NULL DEFAULT '0',
  `tex2` int(11) NOT NULL DEFAULT '0',
  `pet` int(11) NOT NULL DEFAULT '-1',
  `petId` int(11) NOT NULL DEFAULT '-1',
  `hasBackpack` int(11) NOT NULL DEFAULT '0',
  `skin` int(11) NOT NULL DEFAULT '0',
  `xpBoosterTime` int(11) NOT NULL DEFAULT '0',
  `ldTimer` int(11) NOT NULL DEFAULT '0',
  `ltTimer` int(11) NOT NULL DEFAULT '0',
  `fameStats` varchar(512) NOT NULL,
  `createTime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `deathTime` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
  `totalFame` int(11) NOT NULL DEFAULT '0',
  `lastSeen` datetime NOT NULL,
  `lastLocation` varchar(128) NOT NULL,
  PRIMARY KEY (`id`,`accId`,`dead`)
) ENGINE=MyISAM AUTO_INCREMENT=38 DEFAULT CHARSET=utf8;

CREATE TABLE IF NOT EXISTS `classstats` (
  `accId` int(11) NOT NULL,
  `objType` int(11) NOT NULL,
  `bestLv` int(11) NOT NULL DEFAULT '1',
  `bestFame` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`accId`,`objType`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

CREATE TABLE IF NOT EXISTS `dailyquests` (
  `accId` int(11) NOT NULL,
  `goals` varchar(512) NOT NULL,
  `tier` int(11) NOT NULL DEFAULT '1',
  `time` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`accId`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE IF NOT EXISTS `death` (
  `accId` int(11) NOT NULL,
  `chrId` int(11) NOT NULL,
  `name` varchar(64) NOT NULL DEFAULT 'DEFAULT',
  `charType` int(11) NOT NULL DEFAULT '782',
  `tex1` int(11) NOT NULL DEFAULT '0',
  `tex2` int(11) NOT NULL DEFAULT '0',
  `skin` int(11) NOT NULL DEFAULT '0',
  `items` varchar(128) NOT NULL DEFAULT '-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1',
  `fame` int(11) NOT NULL DEFAULT '0',
  `exp` int(11) NOT NULL,
  `fameStats` varchar(256) NOT NULL,
  `totalFame` int(11) NOT NULL DEFAULT '0',
  `firstBorn` tinyint(1) NOT NULL,
  `killer` varchar(128) NOT NULL,
  `time` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`accId`,`chrId`,`time`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

CREATE TABLE IF NOT EXISTS `giftcodes` (
  `code` varchar(128) NOT NULL,
  `content` varchar(512) NOT NULL,
  `accId` int(11) NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE IF NOT EXISTS `globalnews` (
  `slot` int(11) NOT NULL,
  `linkType` int(11) NOT NULL,
  `title` varchar(65) NOT NULL,
  `image` text NOT NULL,
  `priority` int(11) NOT NULL,
  `linkDetail` text NOT NULL,
  `platform` varchar(128) NOT NULL DEFAULT 'kabam.com,kongregate,steam,rotmg',
  `startTime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `endTime` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
  PRIMARY KEY (`slot`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE IF NOT EXISTS `guilds` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(128) NOT NULL DEFAULT 'DEFAULT_GUILD',
  `members` varchar(128) NOT NULL,
  `guildFame` int(11) NOT NULL,
  `totalGuildFame` int(11) NOT NULL,
  `level` int(11) NOT NULL DEFAULT '1',
  PRIMARY KEY (`id`,`members`)
) ENGINE=MyISAM AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;

CREATE TABLE IF NOT EXISTS `market` (
  `itemid` int(11) DEFAULT NULL,
  `fame` int(11) DEFAULT NULL,
  `playerid` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE IF NOT EXISTS `mysteryboxes` (
  `id` int(11) NOT NULL,
  `title` varchar(128) NOT NULL,
  `weight` int(11) NOT NULL,
  `description` varchar(128) NOT NULL,
  `contents` text NOT NULL,
  `priceAmount` int(11) NOT NULL,
  `priceCurrency` int(11) NOT NULL,
  `image` text NOT NULL,
  `icon` text NOT NULL,
  `salePrice` int(11) NOT NULL,
  `saleCurrency` int(11) NOT NULL,
  `saleEnd` datetime NOT NULL DEFAULT '0000-00-00 00:00:00',
  `startTime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `boxEnd` datetime NOT NULL DEFAULT '0000-00-00 00:00:00',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE IF NOT EXISTS `news` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `icon` varchar(16) NOT NULL DEFAULT 'info',
  `title` varchar(128) NOT NULL DEFAULT 'Default news title',
  `text` varchar(128) NOT NULL DEFAULT 'Default news text',
  `link` varchar(256) NOT NULL DEFAULT 'http://mmoe.net/',
  `date` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`,`text`)
) ENGINE=MyISAM AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

CREATE TABLE IF NOT EXISTS `packages` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(128) NOT NULL,
  `maxPurchase` int(11) NOT NULL DEFAULT '-1',
  `weight` int(11) NOT NULL DEFAULT '0',
  `contents` text NOT NULL,
  `bgUrl` varchar(512) NOT NULL,
  `price` int(11) NOT NULL,
  `quantity` int(11) NOT NULL DEFAULT '-1',
  `endDate` datetime NOT NULL DEFAULT '0000-00-00 00:00:00',
  `usersClaimed` text NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE IF NOT EXISTS `pets` (
  `accId` int(11) NOT NULL,
  `petId` int(11) NOT NULL AUTO_INCREMENT,
  `objType` smallint(5) NOT NULL,
  `skinName` varchar(50) NOT NULL,
  `skin` int(11) NOT NULL,
  `family` int(11) NOT NULL,
  `rarity` int(11) NOT NULL,
  `maxLevel` int(11) NOT NULL DEFAULT '30',
  `abilities` varchar(128) NOT NULL,
  `levels` varchar(128) NOT NULL,
  `xp` varchar(128) NOT NULL DEFAULT '0, 0, 0',
  `size` int(11) NOT NULL,
  PRIMARY KEY (`accId`,`petId`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

CREATE TABLE IF NOT EXISTS `seriallist` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `type` varchar(50) NOT NULL,
  `firstUser` int(11) NOT NULL,
  `currentUser` int(11) NOT NULL,
  `droppedIn` text NOT NULL,
  `soulbound` tinyint(4) NOT NULL,
  `banned` tinyint(4) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=525 DEFAULT CHARSET=latin1;

CREATE TABLE IF NOT EXISTS `stats` (
  `accId` int(11) NOT NULL,
  `fame` int(11) NOT NULL,
  `totalFame` int(11) NOT NULL,
  `credits` int(11) NOT NULL,
  `totalCredits` int(11) NOT NULL,
  `fortuneTokens` int(11) NOT NULL,
  `totalFortuneTokens` int(11) NOT NULL,
  PRIMARY KEY (`accId`,`fame`,`totalFame`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

CREATE TABLE IF NOT EXISTS `thealchemist` (
  `id` int(11) NOT NULL,
  `title` varchar(512) NOT NULL,
  `description` varchar(512) DEFAULT NULL,
  `image` varchar(512) NOT NULL,
  `icon` varchar(512) NOT NULL,
  `contents` text NOT NULL,
  `priceFirstInGold` int(11) NOT NULL DEFAULT '51',
  `priceFirstInToken` int(11) NOT NULL DEFAULT '1',
  `priceSecondInGold` int(11) NOT NULL DEFAULT '75',
  `startTime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `endTime` datetime NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE IF NOT EXISTS `unlockedclasses` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `accId` int(11) NOT NULL,
  `class` varchar(128) NOT NULL,
  `available` varchar(128) NOT NULL DEFAULT 'unrestricted',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=687 DEFAULT CHARSET=latin1;

CREATE TABLE IF NOT EXISTS `vaults` (
  `accId` int(11) NOT NULL,
  `chestId` int(11) NOT NULL AUTO_INCREMENT,
  `items` varchar(128) NOT NULL,
  PRIMARY KEY (`accId`,`chestId`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
