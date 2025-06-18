-- Создание таблицы Role
CREATE TABLE "Roles" (
    "Id" SERIAL PRIMARY KEY,
    "Name" VARCHAR(50) NOT NULL UNIQUE
);

-- Создание таблицы Users
CREATE TABLE "Users" (
    "Id" SERIAL PRIMARY KEY,
    "Username" VARCHAR(50) NOT NULL UNIQUE,
    "Password" VARCHAR(255) NOT NULL,
    "Surname" VARCHAR(100) NOT NULL,
    "Name" VARCHAR(100) NOT NULL,
    "Patronymic" VARCHAR(100),
    "RoleId" INT NOT NULL REFERENCES "Roles"("Id"),
    "IsBanned" BOOLEAN NOT NULL DEFAULT FALSE,
    "IsFirstLogin" BOOLEAN NOT NULL DEFAULT TRUE
);

-- Создание таблицы EnterLog
CREATE TABLE "EnterLog" (
    "Id" SERIAL PRIMARY KEY,
    "Date" TIMESTAMP NOT NULL,
    "UserId" INT NOT NULL,
    CONSTRAINT fk_user FOREIGN KEY ("UserId") REFERENCES "Users"("Id") ON DELETE CASCADE
);

-- Вставка ролей
INSERT INTO "Roles" ("Name") VALUES
('Пользователь'),
('Администратор');

-- Вставка пользователей
INSERT INTO "Users" ("Username", "Password", "Surname", "Name", "Patronymic", "RoleId", "IsBanned", "IsFirstLogin") 
VALUES
('admin', 'admin', 'Конев', 'Игорь', 'Яковлевич', (SELECT "Id" FROM "Roles" WHERE "Name" = 'Администратор'), FALSE, FALSE),
('user', 'user', 'Булкина', 'Мария', 'Антоновна', (SELECT "Id" FROM "Roles" WHERE "Name" = 'Пользователь'), FALSE, FALSE),
('blockedUser', 'blockedUser', 'Заблокированное имя', 'Заблокированная фамилия', 'Заблокированное отчество', (SELECT "Id" FROM "Roles" WHERE "Name" = 'Пользователь'), TRUE, FALSE),
('firstAuthUser', 'firstAuthUser', 'Перепроходов', 'Марат', 'Сергеевич', (SELECT "Id" FROM "Roles" WHERE "Name" = 'Пользователь'), FALSE, TRUE);

-- Таблица автомобилей
CREATE TABLE "Cars" (
    "Id" SERIAL PRIMARY KEY,
    "Model" VARCHAR(100) NOT NULL,
    "LicensePlate" VARCHAR(20) NOT NULL UNIQUE,
    "IsActive" BOOLEAN NOT NULL DEFAULT TRUE
);

-- Таблица использования автомобилей
CREATE TABLE "CarUsage" (
    "Id" SERIAL PRIMARY KEY,
    "CarId" INT NOT NULL REFERENCES "Cars"("Id") ON DELETE CASCADE,
    "UserId" INT NOT NULL REFERENCES "Users"("Id") ON DELETE SET NULL,
    "StartTime" TIMESTAMP NOT NULL,
    "EndTime" TIMESTAMP NOT NULL,
    CHECK ("EndTime" > "StartTime")
);

-- Вставляем машины
INSERT INTO "Cars" ("Model", "LicensePlate") VALUES
('Toyota Camry', 'A123BC77'),
('Ford Transit', 'B456DE99'),
('Volkswagen Golf', 'C789FG88'),
('Hyundai Solaris', 'D012HI77');

-- Вставляем использование автомобилей
INSERT INTO "CarUsage" ("CarId", "UserId", "StartTime", "EndTime") VALUES
(1, 2, '2025-06-10 08:00', '2025-06-10 12:00'),
(1, 2, '2025-06-12 14:00', '2025-06-12 18:00'); 

INSERT INTO "CarUsage" ("CarId", "UserId", "StartTime", "EndTime") VALUES
(2, 3, '2025-06-10 06:00', '2025-06-10 16:00'),
(2, 3, '2025-06-11 07:00', '2025-06-11 17:00');

INSERT INTO "CarUsage" ("CarId", "UserId", "StartTime", "EndTime") VALUES
(3, 2, '2025-06-09 00:00', '2025-06-10 00:00');
