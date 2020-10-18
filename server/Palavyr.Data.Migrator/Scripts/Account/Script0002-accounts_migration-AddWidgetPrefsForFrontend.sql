﻿CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);


DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20200924031318_initialCreate') THEN
    CREATE TABLE "Accounts" (
        "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
        "UserName" text NULL,
        "Password" text NULL,
        "EmailAddress" text NULL,
        "AccountId" text NULL,
        "CompanyName" text NULL,
        "PhoneNumber" text NULL,
        "CreationDate" timestamp without time zone NOT NULL,
        "AccountLogoUri" text NULL,
        "ApiKey" text NULL,
        "Active" boolean NOT NULL,
        "Locale" text NULL,
        CONSTRAINT "PK_Accounts" PRIMARY KEY ("Id")
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20200924031318_initialCreate') THEN
    CREATE TABLE "EmailVerifications" (
        "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
        "AuthenticationToken" text NULL,
        "EmailAddress" text NULL,
        "AccountId" text NULL,
        CONSTRAINT "PK_EmailVerifications" PRIMARY KEY ("Id")
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20200924031318_initialCreate') THEN
    CREATE TABLE "Sessions" (
        "SessionId" text NOT NULL,
        "AccountId" text NULL,
        "ApiKey" text NULL,
        "Expiration" timestamp without time zone NOT NULL,
        CONSTRAINT "PK_Sessions" PRIMARY KEY ("SessionId")
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20200924031318_initialCreate') THEN
    CREATE TABLE "Subscriptions" (
        "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
        "AccountId" text NULL,
        "ApiKey" text NULL,
        "NumAreas" integer NOT NULL,
        CONSTRAINT "PK_Subscriptions" PRIMARY KEY ("Id")
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20200924031318_initialCreate') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20200924031318_initialCreate', '3.1.6');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201016235532_AddWidgetPrefsForFrontend') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20201016235532_AddWidgetPrefsForFrontend', '3.1.6');
    END IF;
END $$;
