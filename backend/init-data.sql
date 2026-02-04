mysqldump: [Warning] Using a password on the command line interface can be insecure.
-- MySQL dump 10.13  Distrib 8.0.45, for Linux (x86_64)
--
-- Host: localhost    Database: family_meal
-- ------------------------------------------------------
-- Server version	8.0.45

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `Condiments`
--

DROP TABLE IF EXISTS `Condiments`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Condiments` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Unit` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Condiments`
--

LOCK TABLES `Condiments` WRITE;
/*!40000 ALTER TABLE `Condiments` DISABLE KEYS */;
INSERT INTO `Condiments` VALUES (1,'盐','g','2026-02-03 11:31:44.662934'),(2,'料酒','勺','2026-02-03 11:58:13.354911'),(3,'生抽','勺','2026-02-03 11:58:13.416028'),(4,'老抽','勺','2026-02-03 11:58:13.459050'),(5,'冰糖','g','2026-02-03 11:58:13.504436'),(6,'八角','个','2026-02-03 11:58:13.546848'),(7,'桂皮','块','2026-02-03 11:58:13.591883'),(8,'香叶','片','2026-02-03 11:58:13.641241'),(9,'盐','g','2026-02-03 11:58:13.690561'),(10,'食用油','勺','2026-02-03 11:58:13.735508');
/*!40000 ALTER TABLE `Condiments` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Ingredients`
--

DROP TABLE IF EXISTS `Ingredients`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Ingredients` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Category` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Unit` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Ingredients`
--

LOCK TABLES `Ingredients` WRITE;
/*!40000 ALTER TABLE `Ingredients` DISABLE KEYS */;
INSERT INTO `Ingredients` VALUES (1,'测试','肉类','g','2026-02-03 11:31:20.486072'),(2,'五花肉','肉类','g','2026-02-03 11:56:26.277039'),(3,'葱','蔬菜','根','2026-02-03 11:56:26.381730'),(4,'姜','蔬菜','片','2026-02-03 11:56:26.436720'),(5,'蒜','蔬菜','瓣','2026-02-03 11:56:26.492898'),(6,'鸡腿肉','肉类','g','2026-02-03 12:06:04.759949'),(7,'青椒','蔬菜','个','2026-02-03 12:06:04.815559'),(8,'花椒','调料','g','2026-02-03 12:06:04.859805'),(9,'干辣椒','调料','个','2026-02-03 12:06:04.902690'),(10,'生粉','调料','勺','2026-02-03 12:06:04.946316'),(11,'土鸡','肉','只','2026-02-04 03:09:40.000000');
/*!40000 ALTER TABLE `Ingredients` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `RecipeSteps`
--

DROP TABLE IF EXISTS `RecipeSteps`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `RecipeSteps` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `RecipeId` int NOT NULL,
  `StepOrder` int NOT NULL,
  `Description` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ImageUrl` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_RecipeSteps_RecipeId` (`RecipeId`),
  CONSTRAINT `FK_RecipeSteps_Recipes_RecipeId` FOREIGN KEY (`RecipeId`) REFERENCES `Recipes` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=36 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `RecipeSteps`
--

LOCK TABLES `RecipeSteps` WRITE;
/*!40000 ALTER TABLE `RecipeSteps` DISABLE KEYS */;
INSERT INTO `RecipeSteps` VALUES (32,7,1,'首先准备一只剁好的土鸡，放入一个大盘里面，然后朝盘中加入2勺生粉（或面粉），把鸡肉抓捏一下。尽量让每一块鸡肉都包裹上生粉，生粉有很强的吸附能力，能够附着鸡肉表面的血水和脏东西。','/uploads/9d894a61-ed58-478f-be26-9f36acc636bd.jpg'),(33,7,2,'冷冻的鸡块必须焯水处理方法：生粉清洗干净后需要焯水，冷水下入鸡块，开中小火，水沸腾时开大火撇去浮沫，焯水时间不宜过长，撇去浮沫立即关火捞出鸡块，用热水清洗干净备用。','/uploads/663d17e7-ebce-402b-958a-d0ec782f427a.jpg'),(34,7,3,'锅中加入适量食用油，放入葱段、姜片、蒜瓣、八角、桂皮、香叶炒香，然后放入鸡块翻炒均匀，加入料酒去腥。','/uploads/58e900b5-3e03-417c-b641-4e5261995a04.jpg'),(35,7,4,'加入生抽、老抽翻炒上色，加入没过鸡块的热水，放入冰糖，大火烧开后转小火焖煮40分钟左右，最后大火收汁至浓稠即可。','/uploads/832d408e-2134-46d3-bf3e-ff9606eb18b9.jpg');
/*!40000 ALTER TABLE `RecipeSteps` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Recipes`
--

DROP TABLE IF EXISTS `Recipes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Recipes` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Description` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `ImageUrl` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `MealType` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Recipes`
--

LOCK TABLES `Recipes` WRITE;
/*!40000 ALTER TABLE `Recipes` DISABLE KEYS */;
INSERT INTO `Recipes` VALUES (7,'红烧鸡块','家常红烧鸡块，色泽红亮，肉质软嫩入味，酱香浓郁，是一道经典的家常菜。','/uploads/5c201331-8f80-4802-bfce-71792df8497f.jpg','午餐','2026-02-04 03:09:48.000000');
/*!40000 ALTER TABLE `Recipes` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Reservations`
--

DROP TABLE IF EXISTS `Reservations`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Reservations` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `ReservationDate` datetime(6) NOT NULL,
  `MealType` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `RecipeId` int NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  `RecipeId1` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Reservations_RecipeId` (`RecipeId`),
  KEY `IX_Reservations_RecipeId1` (`RecipeId1`),
  CONSTRAINT `FK_Reservations_Recipes_RecipeId` FOREIGN KEY (`RecipeId`) REFERENCES `Recipes` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_Reservations_Recipes_RecipeId1` FOREIGN KEY (`RecipeId1`) REFERENCES `Recipes` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Reservations`
--

LOCK TABLES `Reservations` WRITE;
/*!40000 ALTER TABLE `Reservations` DISABLE KEYS */;
INSERT INTO `Reservations` VALUES (8,'2026-02-04 00:00:00.000000','午餐',7,'2026-02-04 03:21:39.974566',NULL);
/*!40000 ALTER TABLE `Reservations` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `StepCondiments`
--

DROP TABLE IF EXISTS `StepCondiments`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `StepCondiments` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `StepId` int NOT NULL,
  `CondimentId` int NOT NULL,
  `Amount` double NOT NULL,
  `Unit` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_StepCondiments_CondimentId` (`CondimentId`),
  KEY `IX_StepCondiments_StepId` (`StepId`),
  CONSTRAINT `FK_StepCondiments_Condiments_CondimentId` FOREIGN KEY (`CondimentId`) REFERENCES `Condiments` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_StepCondiments_RecipeSteps_StepId` FOREIGN KEY (`StepId`) REFERENCES `RecipeSteps` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=63 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `StepCondiments`
--

LOCK TABLES `StepCondiments` WRITE;
/*!40000 ALTER TABLE `StepCondiments` DISABLE KEYS */;
INSERT INTO `StepCondiments` VALUES (56,34,6,2,'个'),(57,34,7,1,'段'),(58,34,8,2,'片'),(59,34,2,2,'勺'),(60,35,3,3,'勺'),(61,35,4,2,'勺'),(62,35,5,5,'颗');
/*!40000 ALTER TABLE `StepCondiments` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `StepIngredients`
--

DROP TABLE IF EXISTS `StepIngredients`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `StepIngredients` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `StepId` int NOT NULL,
  `IngredientId` int NOT NULL,
  `Amount` double NOT NULL,
  `Unit` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_StepIngredients_IngredientId` (`IngredientId`),
  KEY `IX_StepIngredients_StepId` (`StepId`),
  CONSTRAINT `FK_StepIngredients_Ingredients_IngredientId` FOREIGN KEY (`IngredientId`) REFERENCES `Ingredients` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_StepIngredients_RecipeSteps_StepId` FOREIGN KEY (`StepId`) REFERENCES `RecipeSteps` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=43 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `StepIngredients`
--

LOCK TABLES `StepIngredients` WRITE;
/*!40000 ALTER TABLE `StepIngredients` DISABLE KEYS */;
INSERT INTO `StepIngredients` VALUES (38,32,11,1,'只'),(39,32,10,2,'勺'),(40,34,3,3,'根'),(41,34,4,5,'片'),(42,34,5,10,'瓣');
/*!40000 ALTER TABLE `StepIngredients` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-02-04  3:28:01
