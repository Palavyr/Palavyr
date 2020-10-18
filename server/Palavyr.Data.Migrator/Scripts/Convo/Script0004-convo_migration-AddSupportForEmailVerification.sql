﻿CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);


DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20200924031329_initialCreate') THEN
    CREATE TABLE "CompletedConversations" (
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
        CONSTRAINT "PK_CompletedConversations" PRIMARY KEY ("Id")
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20200924031329_initialCreate') THEN
    CREATE TABLE "Conversations" (
        "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
        "ConversationId" text NULL,
        "Prompt" text NULL,
        "UserResponse" text NULL,
        "NodeId" text NULL,
        "NodeCritical" boolean NOT NULL,
        "NodeType" text NULL,
        "TimeStamp" timestamp without time zone NOT NULL,
        "IsCompleted" boolean NOT NULL,
        "AccountId" text NULL,
        CONSTRAINT "PK_Conversations" PRIMARY KEY ("Id")
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20200924031329_initialCreate') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20200924031329_initialCreate', '3.1.6');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201016235546_AddWidgetPrefsForFrontend') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20201016235546_AddWidgetPrefsForFrontend', '3.1.6');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201017042730_AddMoreWidgetPrefs') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20201017042730_AddMoreWidgetPrefs', '3.1.6');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201017134714_AddSupportForEmailVerification') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20201017134714_AddSupportForEmailVerification', '3.1.6');
    END IF;
END $$;
