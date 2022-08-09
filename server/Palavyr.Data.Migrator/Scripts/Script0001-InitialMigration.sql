﻿CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220809101004_InitialMigration') THEN
    CREATE TABLE "Accounts" (
        "Id" integer GENERATED BY DEFAULT AS IDENTITY,
        "Password" text NOT NULL,
        "EmailAddress" text NOT NULL,
        "DefaultEmailIsVerified" boolean NOT NULL,
        "AccountId" text NOT NULL,
        "CompanyName" text NOT NULL,
        "PhoneNumber" text NOT NULL,
        "CreationDate" timestamp with time zone NOT NULL,
        "AccountLogoUri" text NOT NULL,
        "GeneralFallbackSubject" text NOT NULL,
        "GeneralFallbackEmailTemplate" text NOT NULL,
        "ApiKey" text NOT NULL,
        "Active" boolean NOT NULL,
        "Locale" text NOT NULL,
        "AccountType" integer NOT NULL,
        "PlanType" integer NOT NULL,
        "PaymentInterval" integer NULL,
        "HasUpgraded" boolean NOT NULL,
        "StripeCustomerId" text NOT NULL,
        "CurrentPeriodEnd" timestamp with time zone NOT NULL,
        "IntroIntentId" text NOT NULL,
        "ShowSeenEnquiries" boolean NOT NULL,
        CONSTRAINT "PK_Accounts" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220809101004_InitialMigration') THEN
    CREATE TABLE "CategoryNestedThresholdTableRows" (
        "Id" integer GENERATED BY DEFAULT AS IDENTITY,
        "AccountId" text NOT NULL,
        "IntentId" text NOT NULL,
        "TableId" text NOT NULL,
        "ValueMin" double precision NOT NULL,
        "ValueMax" double precision NOT NULL,
        "Range" boolean NOT NULL,
        "RowId" text NOT NULL,
        "RowOrder" integer NOT NULL,
        "ItemId" text NOT NULL,
        "ItemOrder" integer NOT NULL,
        "Category" text NOT NULL,
        "Threshold" double precision NOT NULL,
        "TriggerFallback" boolean NOT NULL,
        CONSTRAINT "PK_CategoryNestedThresholdTableRows" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220809101004_InitialMigration') THEN
    CREATE TABLE "ConversationHistoryRows" (
        "Id" integer GENERATED BY DEFAULT AS IDENTITY,
        "ConversationId" text NOT NULL,
        "Prompt" text NOT NULL,
        "UserResponse" text NOT NULL,
        "NodeId" text NOT NULL,
        "NodeCritical" boolean NOT NULL,
        "NodeType" text NOT NULL,
        "TimeStamp" timestamp with time zone NOT NULL,
        "AccountId" text NOT NULL,
        CONSTRAINT "PK_ConversationHistoryRows" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220809101004_InitialMigration') THEN
    CREATE TABLE "ConversationRecords" (
        "Id" integer GENERATED BY DEFAULT AS IDENTITY,
        "ConversationId" text NOT NULL,
        "ResponsePdfId" text NOT NULL,
        "TimeStamp" timestamp with time zone NOT NULL,
        "AccountId" text NOT NULL,
        "IntentName" text NOT NULL,
        "EmailTemplateUsed" text NOT NULL,
        "Seen" boolean NOT NULL,
        "Name" text NOT NULL,
        "Email" text NOT NULL,
        "PhoneNumber" text NOT NULL,
        "IntentId" text NOT NULL,
        "IsDeleted" boolean NOT NULL,
        "IsFallback" boolean NOT NULL,
        "Locale" text NOT NULL,
        "IsComplete" boolean NOT NULL,
        CONSTRAINT "PK_ConversationRecords" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220809101004_InitialMigration') THEN
    CREATE TABLE "EmailVerifications" (
        "Id" integer GENERATED BY DEFAULT AS IDENTITY,
        "AuthenticationToken" text NOT NULL,
        "EmailAddress" text NOT NULL,
        "AccountId" text NOT NULL,
        CONSTRAINT "PK_EmailVerifications" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220809101004_InitialMigration') THEN
    CREATE TABLE "FileAssets" (
        "Id" integer GENERATED BY DEFAULT AS IDENTITY,
        "AccountId" text NOT NULL,
        "FileId" text NOT NULL,
        "RiskyNameStem" text NOT NULL,
        "Extension" text NOT NULL,
        "LocationKey" text NOT NULL,
        CONSTRAINT "PK_FileAssets" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220809101004_InitialMigration') THEN
    CREATE TABLE "Intents" (
        "Id" integer GENERATED BY DEFAULT AS IDENTITY,
        "IntentId" text NOT NULL,
        "IntentName" text NOT NULL,
        "Prologue" text NOT NULL,
        "Epilogue" text NOT NULL,
        "EmailTemplate" text NOT NULL,
        "IsEnabled" boolean NOT NULL,
        "AccountId" text NOT NULL,
        "IntentSpecificEmail" text NOT NULL,
        "EmailIsVerified" boolean NOT NULL,
        "UseIntentFallbackEmail" boolean NOT NULL,
        "FallbackSubject" text NOT NULL,
        "FallbackEmailTemplate" text NOT NULL,
        "SendAttachmentsOnFallback" boolean NOT NULL,
        "SendPdfResponse" boolean NOT NULL,
        "IncludePricingStrategyTableTotals" boolean NOT NULL,
        "Subject" text NOT NULL,
        CONSTRAINT "PK_Intents" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220809101004_InitialMigration') THEN
    CREATE TABLE "Logos" (
        "Id" integer GENERATED BY DEFAULT AS IDENTITY,
        "AccountId" text NOT NULL,
        "AccountLogoFileId" text NOT NULL,
        CONSTRAINT "PK_Logos" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220809101004_InitialMigration') THEN
    CREATE TABLE "PercentOfThresholds" (
        "Id" integer GENERATED BY DEFAULT AS IDENTITY,
        "AccountId" text NOT NULL,
        "IntentId" text NOT NULL,
        "TableId" text NOT NULL,
        "RowId" text NOT NULL,
        "Threshold" double precision NOT NULL,
        "ValueMin" double precision NOT NULL,
        "ValueMax" double precision NOT NULL,
        "Range" boolean NOT NULL,
        "Modifier" double precision NOT NULL,
        "PosNeg" boolean NOT NULL,
        "RowOrder" integer NOT NULL,
        "TriggerFallback" boolean NOT NULL,
        "ItemOrder" integer NOT NULL,
        "ItemId" text NOT NULL,
        "Category" text NOT NULL,
        CONSTRAINT "PK_PercentOfThresholds" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220809101004_InitialMigration') THEN
    CREATE TABLE "Sessions" (
        "Id" integer GENERATED BY DEFAULT AS IDENTITY,
        "SessionId" text NOT NULL,
        "AccountId" text NOT NULL,
        "ApiKey" text NOT NULL,
        "Expiration" timestamp with time zone NOT NULL,
        CONSTRAINT "PK_Sessions" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220809101004_InitialMigration') THEN
    CREATE TABLE "SimpleSelectTableRows" (
        "Id" integer GENERATED BY DEFAULT AS IDENTITY,
        "AccountId" text NOT NULL,
        "IntentId" text NOT NULL,
        "TableId" text NOT NULL,
        "Category" text NOT NULL,
        "ValueMin" double precision NOT NULL,
        "ValueMax" double precision NOT NULL,
        "Range" boolean NOT NULL,
        "RowOrder" integer NOT NULL,
        CONSTRAINT "PK_SimpleSelectTableRows" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220809101004_InitialMigration') THEN
    CREATE TABLE "SimpleThresholdTableRows" (
        "Id" integer GENERATED BY DEFAULT AS IDENTITY,
        "AccountId" text NOT NULL,
        "IntentId" text NOT NULL,
        "TableId" text NOT NULL,
        "RowId" text NOT NULL,
        "Threshold" double precision NOT NULL,
        "ValueMin" double precision NOT NULL,
        "ValueMax" double precision NOT NULL,
        "Range" boolean NOT NULL,
        "ItemName" text NOT NULL,
        "RowOrder" integer NOT NULL,
        "TriggerFallback" boolean NOT NULL,
        CONSTRAINT "PK_SimpleThresholdTableRows" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220809101004_InitialMigration') THEN
    CREATE TABLE "StaticFees" (
        "Id" integer GENERATED BY DEFAULT AS IDENTITY,
        "Min" double precision NOT NULL,
        "Max" double precision NOT NULL,
        "FeeId" text NOT NULL,
        "AccountId" text NOT NULL,
        "IntentId" text NOT NULL,
        CONSTRAINT "PK_StaticFees" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220809101004_InitialMigration') THEN
    CREATE TABLE "StripeWebhookReceivedRecords" (
        "Id" integer GENERATED BY DEFAULT AS IDENTITY,
        "RecordId" text NOT NULL,
        "PayloadSignature" text NOT NULL,
        CONSTRAINT "PK_StripeWebhookReceivedRecords" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220809101004_InitialMigration') THEN
    CREATE TABLE "Subscriptions" (
        "Id" integer GENERATED BY DEFAULT AS IDENTITY,
        "AccountId" text NOT NULL,
        "ApiKey" text NOT NULL,
        "NumIntents" integer NOT NULL,
        CONSTRAINT "PK_Subscriptions" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220809101004_InitialMigration') THEN
    CREATE TABLE "TwoNestedSelectTableRows" (
        "Id" integer GENERATED BY DEFAULT AS IDENTITY,
        "AccountId" text NOT NULL,
        "IntentId" text NOT NULL,
        "TableId" text NOT NULL,
        "ValueMin" double precision NOT NULL,
        "ValueMax" double precision NOT NULL,
        "Range" boolean NOT NULL,
        "RowId" text NOT NULL,
        "RowOrder" integer NOT NULL,
        "ItemId" text NOT NULL,
        "ItemOrder" integer NOT NULL,
        "Category" text NOT NULL,
        "InnerItemName" text NOT NULL,
        CONSTRAINT "PK_TwoNestedSelectTableRows" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220809101004_InitialMigration') THEN
    CREATE TABLE "WidgetPreferences" (
        "Id" integer GENERATED BY DEFAULT AS IDENTITY,
        "Placeholder" text NOT NULL,
        "AccountId" text NOT NULL,
        "LandingHeader" text NOT NULL,
        "ChatHeader" text NOT NULL,
        "SelectListColor" text NOT NULL,
        "ListFontColor" text NOT NULL,
        "HeaderColor" text NOT NULL,
        "HeaderFontColor" text NOT NULL,
        "FontFamily" text NOT NULL,
        "OptionsHeaderColor" text NOT NULL,
        "OptionsHeaderFontColor" text NOT NULL,
        "ChatFontColor" text NOT NULL,
        "ChatBubbleColor" text NOT NULL,
        "ButtonColor" text NOT NULL,
        "ButtonFontColor" text NOT NULL,
        "WidgetState" boolean NOT NULL,
        CONSTRAINT "PK_WidgetPreferences" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220809101004_InitialMigration') THEN
    CREATE TABLE "AttachmentRecords" (
        "Id" integer GENERATED BY DEFAULT AS IDENTITY,
        "AccountId" text NOT NULL,
        "IntentId" text NOT NULL,
        "FileId" text NOT NULL,
        "IntentId1" integer NULL,
        CONSTRAINT "PK_AttachmentRecords" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_AttachmentRecords_Intents_IntentId1" FOREIGN KEY ("IntentId1") REFERENCES "Intents" ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220809101004_InitialMigration') THEN
    CREATE TABLE "ConversationNodes" (
        "Id" integer GENERATED BY DEFAULT AS IDENTITY,
        "IntentId" text NOT NULL,
        "AccountId" text NOT NULL,
        "NodeId" text NOT NULL,
        "Text" text NOT NULL,
        "IsRoot" boolean NOT NULL,
        "IsCritical" boolean NOT NULL,
        "IsMultiOptionType" boolean NOT NULL,
        "IsTerminalType" boolean NOT NULL,
        "ShouldRenderChildren" boolean NOT NULL,
        "IsLoopbackAnchorType" boolean NOT NULL,
        "IsAnabranchType" boolean NOT NULL,
        "IsAnabranchMergePoint" boolean NOT NULL,
        "ShouldShowMultiOption" boolean NOT NULL,
        "IsPricingStrategyTableNode" boolean NOT NULL,
        "IsMultiOptionEditable" boolean NOT NULL,
        "IsImageNode" boolean NOT NULL,
        "FileId" text NOT NULL,
        "OptionPath" text NOT NULL,
        "ValueOptions" text NOT NULL,
        "NodeType" text NOT NULL,
        "PricingStrategyType" text NOT NULL,
        "NodeComponentType" text NOT NULL,
        "ResolveOrder" integer NOT NULL,
        "IsCurrency" boolean NOT NULL,
        "NodeChildrenString" text NOT NULL,
        "NodeTypeCodeEnum" integer NOT NULL,
        "IntentId1" integer NULL,
        CONSTRAINT "PK_ConversationNodes" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_ConversationNodes_Intents_IntentId1" FOREIGN KEY ("IntentId1") REFERENCES "Intents" ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220809101004_InitialMigration') THEN
    CREATE TABLE "PricingStrategyTableMetas" (
        "Id" integer GENERATED BY DEFAULT AS IDENTITY,
        "TableTag" text NOT NULL,
        "PrettyName" text NOT NULL,
        "TableType" text NOT NULL,
        "TableId" text NOT NULL,
        "AccountId" text NOT NULL,
        "IntentId" text NOT NULL,
        "ValuesAsPaths" boolean NOT NULL,
        "UseTableTagAsResponseDescription" boolean NOT NULL,
        "UnitIdEnum" integer NOT NULL,
        "IntentId1" integer NULL,
        CONSTRAINT "PK_PricingStrategyTableMetas" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_PricingStrategyTableMetas_Intents_IntentId1" FOREIGN KEY ("IntentId1") REFERENCES "Intents" ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220809101004_InitialMigration') THEN
    CREATE TABLE "StaticTablesMetas" (
        "Id" integer GENERATED BY DEFAULT AS IDENTITY,
        "TableOrder" integer NOT NULL,
        "Description" text NOT NULL,
        "IntentId" text NOT NULL,
        "AccountId" text NOT NULL,
        "PerPersonInputRequired" boolean NOT NULL,
        "IncludeTotals" boolean NOT NULL,
        "TableId" text NOT NULL,
        "IntentId1" integer NULL,
        CONSTRAINT "PK_StaticTablesMetas" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_StaticTablesMetas_Intents_IntentId1" FOREIGN KEY ("IntentId1") REFERENCES "Intents" ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220809101004_InitialMigration') THEN
    CREATE TABLE "StaticTablesRows" (
        "Id" integer GENERATED BY DEFAULT AS IDENTITY,
        "RowOrder" integer NOT NULL,
        "Description" text NOT NULL,
        "FeeId" integer NOT NULL,
        "Range" boolean NOT NULL,
        "PerPerson" boolean NOT NULL,
        "TableOrder" integer NOT NULL,
        "IntentId" text NOT NULL,
        "AccountId" text NOT NULL,
        "StaticTablesMetaId" integer NULL,
        CONSTRAINT "PK_StaticTablesRows" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_StaticTablesRows_StaticFees_FeeId" FOREIGN KEY ("FeeId") REFERENCES "StaticFees" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_StaticTablesRows_StaticTablesMetas_StaticTablesMetaId" FOREIGN KEY ("StaticTablesMetaId") REFERENCES "StaticTablesMetas" ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220809101004_InitialMigration') THEN
    CREATE INDEX "IX_AttachmentRecords_IntentId1" ON "AttachmentRecords" ("IntentId1");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220809101004_InitialMigration') THEN
    CREATE INDEX "IX_ConversationNodes_IntentId1" ON "ConversationNodes" ("IntentId1");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220809101004_InitialMigration') THEN
    CREATE INDEX "IX_PricingStrategyTableMetas_IntentId1" ON "PricingStrategyTableMetas" ("IntentId1");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220809101004_InitialMigration') THEN
    CREATE INDEX "IX_StaticTablesMetas_IntentId1" ON "StaticTablesMetas" ("IntentId1");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220809101004_InitialMigration') THEN
    CREATE INDEX "IX_StaticTablesRows_FeeId" ON "StaticTablesRows" ("FeeId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220809101004_InitialMigration') THEN
    CREATE INDEX "IX_StaticTablesRows_StaticTablesMetaId" ON "StaticTablesRows" ("StaticTablesMetaId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220809101004_InitialMigration') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20220809101004_InitialMigration', '6.0.7');
    END IF;
END $EF$;
COMMIT;

