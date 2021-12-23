﻿CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);


DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20211118083855_initial_migration') THEN
    CREATE TABLE "ConversationHistories" (
        "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
        "ConversationId" text NULL,
        "Prompt" text NULL,
        "UserResponse" text NULL,
        "NodeId" text NULL,
        "NodeCritical" boolean NOT NULL,
        "NodeType" text NULL,
        "TimeStamp" timestamp without time zone NOT NULL,
        "AccountId" text NULL,
        CONSTRAINT "PK_ConversationHistories" PRIMARY KEY ("Id")
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20211118083855_initial_migration') THEN
    CREATE TABLE "ConversationRecords" (
        "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
        "ConversationId" text NULL,
        "ResponsePdfId" text NULL,
        "TimeStamp" timestamp without time zone NOT NULL,
        "AccountId" text NULL,
        "AreaName" text NULL,
        "EmailTemplateUsed" text NULL,
        "Seen" boolean NOT NULL,
        "Name" text NULL,
        "Email" text NULL,
        "PhoneNumber" text NULL,
        "AreaIdentifier" text NULL,
        "IsDeleted" boolean NOT NULL,
        "IsFallback" boolean NOT NULL,
        "Locale" text NULL,
        "IsComplete" boolean NOT NULL,
        CONSTRAINT "PK_ConversationRecords" PRIMARY KEY ("Id")
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20211118083855_initial_migration') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20211118083855_initial_migration', '3.1.13');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20211220085324_Optional_totals_row_in_tables') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20211220085324_Optional_totals_row_in_tables', '3.1.13');
    END IF;
END $$;