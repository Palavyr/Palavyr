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

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201017042718_AddMoreWidgetPrefs') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20201017042718_AddMoreWidgetPrefs', '3.1.6');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201017134703_AddSupportForEmailVerification') THEN
    ALTER TABLE "Accounts" ADD "DefaultEmailIsVerified" boolean NOT NULL DEFAULT FALSE;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201017134703_AddSupportForEmailVerification') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20201017134703_AddSupportForEmailVerification', '3.1.6');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201024230145_addAccountTypeColumn') THEN
    ALTER TABLE "Accounts" ADD "AccountType" integer NOT NULL DEFAULT 0;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201024230145_addAccountTypeColumn') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20201024230145_addAccountTypeColumn', '3.1.6');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201107043713_AddSubscriptionDetailsToAccountsTable') THEN
    ALTER TABLE "Accounts" ADD "HasUpgraded" boolean NOT NULL DEFAULT FALSE;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201107043713_AddSubscriptionDetailsToAccountsTable') THEN
    ALTER TABLE "Accounts" ADD "PaymentInterval" integer NOT NULL DEFAULT 0;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201107043713_AddSubscriptionDetailsToAccountsTable') THEN
    ALTER TABLE "Accounts" ADD "PlanType" integer NOT NULL DEFAULT 0;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201107043713_AddSubscriptionDetailsToAccountsTable') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20201107043713_AddSubscriptionDetailsToAccountsTable', '3.1.6');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201108044344_AddStripeCustomerIdColumn') THEN
    ALTER TABLE "Accounts" ADD "StripeCustomerId" text NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201108044344_AddStripeCustomerIdColumn') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20201108044344_AddStripeCustomerIdColumn', '3.1.6');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201129002131_AddIsMultiOptionType') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20201129002131_AddIsMultiOptionType', '3.1.6');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201129110203_AddIsTerminalType') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20201129110203_AddIsTerminalType', '3.1.6');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201226234334_0010') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20201226234334_0010', '3.1.6');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201227033934_0011') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20201227033934_0011', '3.1.6');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210101115748_AddWidgetStateColumn') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210101115748_AddWidgetStateColumn', '3.1.6');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210102141203_AddSubjectColumnToArea') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210102141203_AddSubjectColumnToArea', '3.1.6');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210103003109_AddMoreWidgetPreferenceColumns') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210103003109_AddMoreWidgetPreferenceColumns', '3.1.6');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210107114142_AddCurrentPeriodEndToAccount') THEN
    ALTER TABLE "Accounts" ADD "CurrentPeriodEnd" timestamp without time zone NOT NULL DEFAULT TIMESTAMP '0001-01-01 00:00:00';
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210107114142_AddCurrentPeriodEndToAccount') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210107114142_AddCurrentPeriodEndToAccount', '3.1.6');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210122123625_AddbackupTableToRecordLatestBackupS3Keys') THEN
    CREATE TABLE "Backups" (
        "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
        "LatestFullDbBackup" text NULL,
        "LatestUserDataBackup" text NULL,
        CONSTRAINT "PK_Backups" PRIMARY KEY ("Id")
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210122123625_AddbackupTableToRecordLatestBackupS3Keys') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210122123625_AddbackupTableToRecordLatestBackupS3Keys', '3.1.6');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210206122244_AddFallbackEmailAndDefaultFallbackEmail') THEN
    ALTER TABLE "Accounts" ADD "GeneralFallbackEmailTemplate" text NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210206122244_AddFallbackEmailAndDefaultFallbackEmail') THEN
    ALTER TABLE "Accounts" ADD "GeneralFallbackSubject" text NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210206122244_AddFallbackEmailAndDefaultFallbackEmail') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210206122244_AddFallbackEmailAndDefaultFallbackEmail', '3.1.6');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210220033333_AddWebhookPayloadTable') THEN
    CREATE TABLE "StripeWebHookRecords" (
        "Id" text NOT NULL,
        "PayloadSignature" text NULL,
        CONSTRAINT "PK_StripeWebHookRecords" PRIMARY KEY ("Id")
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210220033333_AddWebhookPayloadTable') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210220033333_AddWebhookPayloadTable', '3.1.6');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210220070356_AddPerPersonRequiredStaticTableMetaColumn') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210220070356_AddPerPersonRequiredStaticTableMetaColumn', '3.1.6');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210222092505_0020') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210222092505_0020', '3.1.6');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210223104159_RemoveAndRenameUpdates') THEN
    ALTER TABLE "Accounts" DROP COLUMN "UserName";
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210223104159_RemoveAndRenameUpdates') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210223104159_RemoveAndRenameUpdates', '3.1.6');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210301120736_RemoveGroupedPropFromWidgetPrefs') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210301120736_RemoveGroupedPropFromWidgetPrefs', '3.1.6');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210304124806_0023') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210304124806_0023', '3.1.6');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210306001229_AddRowOrderToCurrentDynamicTables') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210306001229_AddRowOrderToCurrentDynamicTables', '3.1.6');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210307010223_AddShouldNotRenderPropertyToConvoNodeType') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210307010223_AddShouldNotRenderPropertyToConvoNodeType', '3.1.6');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210307014149_AddIsSplitMergeTypeToConveNode') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210307014149_AddIsSplitMergeTypeToConveNode', '3.1.6');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210307035928_FixRenderChildrenName') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210307035928_FixRenderChildrenName', '3.1.6');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210310095929_AddShouldShowMultioptionPropToConvoNode') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210310095929_AddShouldShowMultioptionPropToConvoNode', '3.1.6');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210314041227_AddAnabranchPropertiesToConvoNode') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210314041227_AddAnabranchPropertiesToConvoNode', '3.1.6');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210326222406_AddRequiredDynamicTableNodes') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210326222406_AddRequiredDynamicTableNodes', '3.1.6');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210328091301_AddPropertiesRequiredForComplexNodeResolution') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210328091301_AddPropertiesRequiredForComplexNodeResolution', '3.1.6');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210328093102_AddCategorySelectCountsDbSet') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210328093102_AddCategorySelectCountsDbSet', '3.1.6');
    END IF;
END $$;
