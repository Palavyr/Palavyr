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
    VALUES ('20200924031324_initialCreate', '3.1.6');
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
    VALUES ('20201016235539_AddWidgetPrefsForFrontend', '3.1.6');
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
    VALUES ('20201017042724_AddMoreWidgetPrefs', '3.1.6');
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
    VALUES ('20201017134708_AddSupportForEmailVerification', '3.1.6');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201024230151_addAccountTypeColumn') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20201024230151_addAccountTypeColumn', '3.1.6');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201107043719_AddSubscriptionDetailsToAccountsTable') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20201107043719_AddSubscriptionDetailsToAccountsTable', '3.1.6');
    END IF;
END $$;
