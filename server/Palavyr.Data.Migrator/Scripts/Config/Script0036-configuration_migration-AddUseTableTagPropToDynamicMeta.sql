﻿CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);


DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20200924031324_initialCreate') THEN
    CREATE TABLE "Areas" (
        "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
        "AreaIdentifier" text NULL,
        "AreaName" text NULL,
        "AreaDisplayTitle" text NULL,
        "Prologue" text NULL,
        "Epilogue" text NULL,
        "EmailTemplate" text NULL,
        "IsComplete" boolean NOT NULL,
        "GroupId" text NULL,
        "AccountId" text NULL,
        CONSTRAINT "PK_Areas" PRIMARY KEY ("Id")
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20200924031324_initialCreate') THEN
    CREATE TABLE "FileNameMaps" (
        "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
        "SafeName" text NULL,
        "RiskyName" text NULL,
        "AccountId" text NULL,
        "AreaIdentifier" text NULL,
        CONSTRAINT "PK_FileNameMaps" PRIMARY KEY ("Id")
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20200924031324_initialCreate') THEN
    CREATE TABLE "Groups" (
        "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
        "GroupId" text NULL,
        "ParentId" text NULL,
        "GroupName" text NULL,
        "AccountId" text NULL,
        CONSTRAINT "PK_Groups" PRIMARY KEY ("Id")
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20200924031324_initialCreate') THEN
    CREATE TABLE "SelectOneFlats" (
        "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
        "AccountId" text NULL,
        "AreaIdentifier" text NULL,
        "TableId" text NULL,
        "Option" text NULL,
        "ValueMin" double precision NOT NULL,
        "ValueMax" double precision NOT NULL,
        "Range" boolean NOT NULL,
        "TableTag" text NULL,
        CONSTRAINT "PK_SelectOneFlats" PRIMARY KEY ("Id")
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20200924031324_initialCreate') THEN
    CREATE TABLE "StaticFees" (
        "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
        "Min" double precision NOT NULL,
        "Max" double precision NOT NULL,
        "FeeId" text NULL,
        "AccountId" text NULL,
        "AreaIdentifier" text NULL,
        CONSTRAINT "PK_StaticFees" PRIMARY KEY ("Id")
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20200924031324_initialCreate') THEN
    CREATE TABLE "WidgetPreferences" (
        "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
        "Title" text NULL,
        "Subtitle" text NULL,
        "Placeholder" text NULL,
        "ShouldGroup" boolean NOT NULL,
        "AccountId" text NULL,
        CONSTRAINT "PK_WidgetPreferences" PRIMARY KEY ("Id")
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20200924031324_initialCreate') THEN
    CREATE TABLE "ConversationNodes" (
        "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
        "NodeId" text NULL,
        "NodeType" text NULL,
        "Fallback" boolean NOT NULL,
        "Text" text NULL,
        "IsRoot" boolean NOT NULL,
        "AreaIdentifier" text NULL,
        "OptionPath" text NULL,
        "IsCritical" boolean NOT NULL,
        "ValueOptions" text NULL,
        "AccountId" text NULL,
        "NodeChildrenString" text NULL,
        "AreaId" integer NULL,
        CONSTRAINT "PK_ConversationNodes" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_ConversationNodes_Areas_AreaId" FOREIGN KEY ("AreaId") REFERENCES "Areas" ("Id") ON DELETE RESTRICT
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20200924031324_initialCreate') THEN
    CREATE TABLE "DynamicTableMetas" (
        "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
        "TableTag" text NULL,
        "PrettyName" text NULL,
        "TableType" text NULL,
        "TableId" text NULL,
        "AccountId" text NULL,
        "AreaIdentifier" text NULL,
        "ValuesAsPaths" boolean NOT NULL,
        "AreaId" integer NULL,
        CONSTRAINT "PK_DynamicTableMetas" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_DynamicTableMetas_Areas_AreaId" FOREIGN KEY ("AreaId") REFERENCES "Areas" ("Id") ON DELETE RESTRICT
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20200924031324_initialCreate') THEN
    CREATE TABLE "StaticTablesMetas" (
        "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
        "TableOrder" integer NOT NULL,
        "Description" text NULL,
        "AreaIdentifier" text NULL,
        "AccountId" text NULL,
        "AreaId" integer NULL,
        CONSTRAINT "PK_StaticTablesMetas" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_StaticTablesMetas_Areas_AreaId" FOREIGN KEY ("AreaId") REFERENCES "Areas" ("Id") ON DELETE RESTRICT
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20200924031324_initialCreate') THEN
    CREATE TABLE "StaticTablesRows" (
        "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
        "RowOrder" integer NOT NULL,
        "Description" text NULL,
        "FeeId" integer NULL,
        "Range" boolean NOT NULL,
        "PerPerson" boolean NOT NULL,
        "TableOrder" integer NOT NULL,
        "AreaIdentifier" text NULL,
        "AccountId" text NULL,
        "StaticTablesMetaId" integer NULL,
        CONSTRAINT "PK_StaticTablesRows" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_StaticTablesRows_StaticFees_FeeId" FOREIGN KEY ("FeeId") REFERENCES "StaticFees" ("Id") ON DELETE RESTRICT,
        CONSTRAINT "FK_StaticTablesRows_StaticTablesMetas_StaticTablesMetaId" FOREIGN KEY ("StaticTablesMetaId") REFERENCES "StaticTablesMetas" ("Id") ON DELETE RESTRICT
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20200924031324_initialCreate') THEN
    CREATE INDEX "IX_ConversationNodes_AreaId" ON "ConversationNodes" ("AreaId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20200924031324_initialCreate') THEN
    CREATE INDEX "IX_DynamicTableMetas_AreaId" ON "DynamicTableMetas" ("AreaId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20200924031324_initialCreate') THEN
    CREATE INDEX "IX_StaticTablesMetas_AreaId" ON "StaticTablesMetas" ("AreaId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20200924031324_initialCreate') THEN
    CREATE INDEX "IX_StaticTablesRows_FeeId" ON "StaticTablesRows" ("FeeId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20200924031324_initialCreate') THEN
    CREATE INDEX "IX_StaticTablesRows_StaticTablesMetaId" ON "StaticTablesRows" ("StaticTablesMetaId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20200924031324_initialCreate') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20200924031324_initialCreate', '3.1.13');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201016235539_AddWidgetPrefsForFrontend') THEN
    ALTER TABLE "WidgetPreferences" ADD "FontFamily" text NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201016235539_AddWidgetPrefsForFrontend') THEN
    ALTER TABLE "WidgetPreferences" ADD "Header" text NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201016235539_AddWidgetPrefsForFrontend') THEN
    ALTER TABLE "WidgetPreferences" ADD "HeaderColor" text NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201016235539_AddWidgetPrefsForFrontend') THEN
    ALTER TABLE "WidgetPreferences" ADD "SelectListColor" text NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201016235539_AddWidgetPrefsForFrontend') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20201016235539_AddWidgetPrefsForFrontend', '3.1.13');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201017042724_AddMoreWidgetPrefs') THEN
    ALTER TABLE "WidgetPreferences" ADD "HeaderFontColor" text NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201017042724_AddMoreWidgetPrefs') THEN
    ALTER TABLE "WidgetPreferences" ADD "ListFontColor" text NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201017042724_AddMoreWidgetPrefs') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20201017042724_AddMoreWidgetPrefs', '3.1.13');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201017134708_AddSupportForEmailVerification') THEN
    ALTER TABLE "Areas" ADD "AreaSpecificEmail" text NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201017134708_AddSupportForEmailVerification') THEN
    ALTER TABLE "Areas" ADD "EmailIsVerified" boolean NOT NULL DEFAULT FALSE;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201017134708_AddSupportForEmailVerification') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20201017134708_AddSupportForEmailVerification', '3.1.13');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201024230151_addAccountTypeColumn') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20201024230151_addAccountTypeColumn', '3.1.13');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201107043719_AddSubscriptionDetailsToAccountsTable') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20201107043719_AddSubscriptionDetailsToAccountsTable', '3.1.13');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201108044353_AddStripeCustomerIdColumn') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20201108044353_AddStripeCustomerIdColumn', '3.1.13');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201129002150_AddIsMultiOptionType') THEN
    ALTER TABLE "ConversationNodes" ADD "IsMultiOptionType" boolean NOT NULL DEFAULT FALSE;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201129002150_AddIsMultiOptionType') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20201129002150_AddIsMultiOptionType', '3.1.13');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201129110218_AddIsTerminalType') THEN
    ALTER TABLE "ConversationNodes" ADD "IsTerminalType" boolean NOT NULL DEFAULT FALSE;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201129110218_AddIsTerminalType') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20201129110218_AddIsTerminalType', '3.1.13');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201226234349_0010') THEN
    ALTER TABLE "SelectOneFlats" DROP COLUMN "TableTag";
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201226234349_0010') THEN
    CREATE TABLE "PercentOfThresholds" (
        "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
        "AccountId" text NULL,
        "AreaIdentifier" text NULL,
        "TableId" text NULL,
        "ItemId" text NULL,
        "ItemName" text NULL,
        "RowId" text NULL,
        "Threshold" double precision NOT NULL,
        "ValueMin" double precision NOT NULL,
        "ValueMax" double precision NOT NULL,
        "Modifier" double precision NOT NULL,
        "PosNeg" boolean NOT NULL,
        CONSTRAINT "PK_PercentOfThresholds" PRIMARY KEY ("Id")
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201226234349_0010') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20201226234349_0010', '3.1.13');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201227033941_0011') THEN
    ALTER TABLE "PercentOfThresholds" ADD "Range" boolean NOT NULL DEFAULT FALSE;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201227033941_0011') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20201227033941_0011', '3.1.13');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210101115807_AddWidgetStateColumn') THEN
    ALTER TABLE "WidgetPreferences" ADD "WidgetState" boolean NOT NULL DEFAULT FALSE;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210101115807_AddWidgetStateColumn') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210101115807_AddWidgetStateColumn', '3.1.13');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210102141219_AddSubjectColumnToArea') THEN
    ALTER TABLE "Areas" ADD "Subject" text NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210102141219_AddSubjectColumnToArea') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210102141219_AddSubjectColumnToArea', '3.1.13');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210103003120_AddMoreWidgetPreferenceColumns') THEN
    ALTER TABLE "WidgetPreferences" ADD "ChatBubbleColor" text NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210103003120_AddMoreWidgetPreferenceColumns') THEN
    ALTER TABLE "WidgetPreferences" ADD "ChatFontColor" text NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210103003120_AddMoreWidgetPreferenceColumns') THEN
    ALTER TABLE "WidgetPreferences" ADD "OptionsHeaderColor" text NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210103003120_AddMoreWidgetPreferenceColumns') THEN
    ALTER TABLE "WidgetPreferences" ADD "OptionsHeaderFontColor" text NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210103003120_AddMoreWidgetPreferenceColumns') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210103003120_AddMoreWidgetPreferenceColumns', '3.1.13');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210107114148_AddCurrentPeriodEndToAccount') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210107114148_AddCurrentPeriodEndToAccount', '3.1.13');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210122122226_AddbackupTableToRecordLatestBackupS3Keys') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210122122226_AddbackupTableToRecordLatestBackupS3Keys', '3.1.13');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210206122252_AddFallbackEmailAndDefaultFallbackEmail') THEN
    ALTER TABLE "Areas" ADD "FallbackEmailTemplate" text NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210206122252_AddFallbackEmailAndDefaultFallbackEmail') THEN
    ALTER TABLE "Areas" ADD "FallbackSubject" text NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210206122252_AddFallbackEmailAndDefaultFallbackEmail') THEN
    ALTER TABLE "Areas" ADD "UseAreaFallbackEmail" boolean NOT NULL DEFAULT FALSE;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210206122252_AddFallbackEmailAndDefaultFallbackEmail') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210206122252_AddFallbackEmailAndDefaultFallbackEmail', '3.1.13');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210220033341_AddWebhookPayloadTable') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210220033341_AddWebhookPayloadTable', '3.1.13');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210220070411_AddPerPersonRequiredStaticTableMetaColumn') THEN
    ALTER TABLE "StaticTablesRows" ADD "PerPersonInputRequired" boolean NOT NULL DEFAULT FALSE;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210220070411_AddPerPersonRequiredStaticTableMetaColumn') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210220070411_AddPerPersonRequiredStaticTableMetaColumn', '3.1.13');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210222092513_0020') THEN
    ALTER TABLE "StaticTablesRows" DROP COLUMN "PerPersonInputRequired";
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210222092513_0020') THEN
    ALTER TABLE "Areas" DROP COLUMN "GroupId";
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210222092513_0020') THEN
    ALTER TABLE "Areas" DROP COLUMN "IsComplete";
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210222092513_0020') THEN
    ALTER TABLE "StaticTablesMetas" ADD "PerPersonInputRequired" boolean NOT NULL DEFAULT FALSE;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210222092513_0020') THEN
    ALTER TABLE "Areas" ADD "IsEnabled" boolean NOT NULL DEFAULT FALSE;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210222092513_0020') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210222092513_0020', '3.1.13');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210223104207_RemoveAndRenameUpdates') THEN
    DROP TABLE "Groups";
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210223104207_RemoveAndRenameUpdates') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210223104207_RemoveAndRenameUpdates', '3.1.13');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210301120744_RemoveGroupedPropFromWidgetPrefs') THEN
    ALTER TABLE "WidgetPreferences" DROP COLUMN "ShouldGroup";
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210301120744_RemoveGroupedPropFromWidgetPrefs') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210301120744_RemoveGroupedPropFromWidgetPrefs', '3.1.13');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210304124822_0023') THEN
    CREATE TABLE "BasicThresholds" (
        "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
        "AccountId" text NULL,
        "AreaIdentifier" text NULL,
        "TableId" text NULL,
        "RowId" text NULL,
        "Threshold" double precision NOT NULL,
        "ValueMin" double precision NOT NULL,
        "ValueMax" double precision NOT NULL,
        "ItemName" text NULL,
        "Range" boolean NOT NULL,
        CONSTRAINT "PK_BasicThresholds" PRIMARY KEY ("Id")
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210304124822_0023') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210304124822_0023', '3.1.13');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210306001236_AddRowOrderToCurrentDynamicTables') THEN
    ALTER TABLE "SelectOneFlats" ADD "RowOrder" integer NOT NULL DEFAULT 0;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210306001236_AddRowOrderToCurrentDynamicTables') THEN
    ALTER TABLE "PercentOfThresholds" ADD "RowOrder" integer NOT NULL DEFAULT 0;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210306001236_AddRowOrderToCurrentDynamicTables') THEN
    ALTER TABLE "BasicThresholds" ADD "RowOrder" integer NOT NULL DEFAULT 0;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210306001236_AddRowOrderToCurrentDynamicTables') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210306001236_AddRowOrderToCurrentDynamicTables', '3.1.13');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210307010231_AddShouldNotRenderPropertyToConvoNodeType') THEN
    ALTER TABLE "ConversationNodes" ADD "ShouldRenderChild" boolean NOT NULL DEFAULT FALSE;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210307010231_AddShouldNotRenderPropertyToConvoNodeType') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210307010231_AddShouldNotRenderPropertyToConvoNodeType', '3.1.13');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210307014157_AddIsSplitMergeTypeToConveNode') THEN
    ALTER TABLE "ConversationNodes" ADD "IsSplitMergeType" boolean NOT NULL DEFAULT FALSE;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210307014157_AddIsSplitMergeTypeToConveNode') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210307014157_AddIsSplitMergeTypeToConveNode', '3.1.13');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210307035936_FixRenderChildrenName') THEN
    ALTER TABLE "ConversationNodes" DROP COLUMN "ShouldRenderChild";
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210307035936_FixRenderChildrenName') THEN
    ALTER TABLE "ConversationNodes" ADD "ShouldRenderChildren" boolean NOT NULL DEFAULT FALSE;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210307035936_FixRenderChildrenName') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210307035936_FixRenderChildrenName', '3.1.13');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210310095939_AddShouldShowMultioptionPropToConvoNode') THEN
    ALTER TABLE "ConversationNodes" ADD "ShouldShowMultiOption" boolean NOT NULL DEFAULT FALSE;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210310095939_AddShouldShowMultioptionPropToConvoNode') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210310095939_AddShouldShowMultioptionPropToConvoNode', '3.1.13');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210314041235_AddAnabranchPropertiesToConvoNode') THEN
    ALTER TABLE "ConversationNodes" ADD "IsAnabranchMergePoint" boolean NOT NULL DEFAULT FALSE;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210314041235_AddAnabranchPropertiesToConvoNode') THEN
    ALTER TABLE "ConversationNodes" ADD "IsAnabranchType" boolean NOT NULL DEFAULT FALSE;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210314041235_AddAnabranchPropertiesToConvoNode') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210314041235_AddAnabranchPropertiesToConvoNode', '3.1.13');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210326222425_AddRequiredDynamicTableNodes') THEN
    ALTER TABLE "DynamicTableMetas" ADD "RequiredNodeTypes" text NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210326222425_AddRequiredDynamicTableNodes') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210326222425_AddRequiredDynamicTableNodes', '3.1.13');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210328091310_AddPropertiesRequiredForComplexNodeResolution') THEN
    ALTER TABLE "ConversationNodes" ADD "IsCurrency" boolean NOT NULL DEFAULT FALSE;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210328091310_AddPropertiesRequiredForComplexNodeResolution') THEN
    ALTER TABLE "ConversationNodes" ADD "IsDynamicTableNode" boolean NOT NULL DEFAULT FALSE;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210328091310_AddPropertiesRequiredForComplexNodeResolution') THEN
    ALTER TABLE "ConversationNodes" ADD "IsMultiOptionEditable" boolean NOT NULL DEFAULT FALSE;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210328091310_AddPropertiesRequiredForComplexNodeResolution') THEN
    ALTER TABLE "ConversationNodes" ADD "NodeComponentType" text NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210328091310_AddPropertiesRequiredForComplexNodeResolution') THEN
    ALTER TABLE "ConversationNodes" ADD "ResolveOrder" integer NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210328091310_AddPropertiesRequiredForComplexNodeResolution') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210328091310_AddPropertiesRequiredForComplexNodeResolution', '3.1.13');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210328093112_AddCategorySelectCountsDbSet') THEN
    CREATE TABLE "CategorySelectCounts" (
        "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
        "AccountId" text NULL,
        "AreaIdentifier" text NULL,
        "TableId" text NULL,
        "ValueMin" double precision NOT NULL,
        "ValueMax" double precision NOT NULL,
        "Range" boolean NOT NULL,
        "RowId" text NULL,
        "RowOrder" integer NOT NULL,
        "ItemName" text NULL,
        "ItemId" text NULL,
        "ItemOrder" integer NOT NULL,
        "Count" text NULL,
        CONSTRAINT "PK_CategorySelectCounts" PRIMARY KEY ("Id")
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210328093112_AddCategorySelectCountsDbSet') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210328093112_AddCategorySelectCountsDbSet', '3.1.13');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210328214039_RemoveRequiredNodesCols') THEN
    ALTER TABLE "DynamicTableMetas" DROP COLUMN "RequiredNodeTypes";
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210328214039_RemoveRequiredNodesCols') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210328214039_RemoveRequiredNodesCols', '3.1.13');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210329092002_RenameToTwoNestedCategory') THEN
    DROP TABLE "CategorySelectCounts";
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210329092002_RenameToTwoNestedCategory') THEN
    CREATE TABLE "TwoNestedCategories" (
        "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
        "AccountId" text NULL,
        "AreaIdentifier" text NULL,
        "TableId" text NULL,
        "ValueMin" double precision NOT NULL,
        "ValueMax" double precision NOT NULL,
        "Range" boolean NOT NULL,
        "RowId" text NULL,
        "RowOrder" integer NOT NULL,
        "ItemId" text NULL,
        "ItemOrder" integer NOT NULL,
        "Category" text NULL,
        "SubCategory" text NULL,
        CONSTRAINT "PK_TwoNestedCategories" PRIMARY KEY ("Id")
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210329092002_RenameToTwoNestedCategory') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210329092002_RenameToTwoNestedCategory', '3.1.13');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210406133301_addDynamicTypePropToConvoNode') THEN
    ALTER TABLE "ConversationNodes" ADD "DynamicType" text NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210406133301_addDynamicTypePropToConvoNode') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210406133301_addDynamicTypePropToConvoNode', '3.1.13');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210406225409_AddUseTableTagPropToDynamicMeta') THEN
    ALTER TABLE "DynamicTableMetas" ADD "UseTableTagAsResponseDescription" boolean NOT NULL DEFAULT FALSE;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210406225409_AddUseTableTagPropToDynamicMeta') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210406225409_AddUseTableTagPropToDynamicMeta', '3.1.13');
    END IF;
END $$;
