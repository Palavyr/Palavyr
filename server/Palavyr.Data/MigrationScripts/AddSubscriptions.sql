CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" TEXT NOT NULL CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY,
    "ProductVersion" TEXT NOT NULL
);

CREATE TABLE "Accounts" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Accounts" PRIMARY KEY AUTOINCREMENT,
    "UserName" TEXT NULL,
    "Password" TEXT NULL,
    "EmailAddress" TEXT NULL,
    "AccountId" TEXT NULL,
    "CompanyName" TEXT NULL,
    "PhoneNumber" TEXT NULL,
    "CreationDate" TEXT NOT NULL,
    "AccountLogoUri" TEXT NULL,
    "ApiKey" TEXT NULL,
    "Active" INTEGER NOT NULL,
    "Locale" TEXT NULL
);

CREATE TABLE "EmailVerifications" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_EmailVerifications" PRIMARY KEY AUTOINCREMENT,
    "AuthenticationToken" TEXT NULL,
    "EmailAddress" TEXT NULL,
    "AccountId" TEXT NULL
);

CREATE TABLE "Sessions" (
    "SessionId" TEXT NOT NULL CONSTRAINT "PK_Sessions" PRIMARY KEY,
    "AccountId" TEXT NULL,
    "ApiKey" TEXT NULL,
    "Expiration" TEXT NOT NULL
);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20200916081741_init', '3.1.6');

CREATE TABLE "Subscriptions" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Subscriptions" PRIMARY KEY AUTOINCREMENT,
    "AccountId" TEXT NULL,
    "ApiKey" TEXT NULL,
    "NumAreas" INTEGER NOT NULL
);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20200917051514_AddSubcriptionDB', '3.1.6');

