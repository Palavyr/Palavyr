import { Images, StripeProduct } from "@Palavyr-Types";
import { FileAssetResource } from "./EntityResources";
import { NodeTypeCodeEnum, UnitGroups, UnitIdEnum, UnitPrettyNames } from "./Enums";

export type PreCheckErrorResource = {
    intentName: string;
    reasons: string[];
};

export type PreCheckResultResource = {
    isReady: boolean;
    preCheckErrors: PreCheckErrorResource[];
    apiKeyExists: boolean;
};

export type SendLiveEmailResultResource = {
    nextNodeId: string;
    result: boolean;
    fileAsset: FileAssetResource;
};

export type EmailVerificationResource = {
    title: string;
    message: string;
    status: "Success" | "Pending" | "Failed";
};

export type ResponseVariable = {
    name: string;
    pattern: string;
    details: string;
};

export type ResponseVariableResource = {
    responseVariables: ResponseVariable;
};

export type QuantUnitDefinition = {
    unitGroup: UnitGroups;
    unitPrettyName: UnitPrettyNames;
    unitIdEnum: UnitIdEnum;
};

export type QuantityUnitResource = {
    unitGroup: string;
    unitPrettyName: string;
    unitIdEnum: UnitIdEnum;
};

export type NodeTypeOptionResources = NodeTypeOptionResource[];

export type NodeTypeOptionResource = {
    nodeTypeCodeEnum: NodeTypeCodeEnum;
    value: string;
    text: string;
    pathOptions: string[];
    valueOptions: string[];
    isMultiOptionType: boolean;
    isTerminalType: boolean;
    shouldRenderChildren: boolean;
    shouldShowMultiOption: boolean;
    isSplitMergeType: boolean;
    isAnabranchType: boolean;
    isAnabranchMergePoint: boolean;
    isPricingStrategyType: boolean;
    nodeComponentType: string;
    isCurrency: boolean;
    isMultiOptionEditable: boolean;
    groupName: string;
    resolveOrder: number;
    pricingStrategyType: string;
    isImageNode: boolean;
    isLoopbackAnchor: boolean;
    stringName: string;
};

export type ProductIdResource = {
    freeProductId: string;
    lyteProductId: string;
    premiumProductId: string;
    proProductId: string;
};

export type PlanStatusResource = {
    status: string;
    hasUpgraded: boolean;
};

export type EnquiryInsightsResource = {
    intentName: string;
    intentIdentifier: string;
    numRecords: number;
    sentEmailCount: number;
    completed: number;
    averageIntentCompletion: number;
    intentCompletePerIntent: number[];
};

export type TreeErrorsResource = {
    missingNodes: string[];
    outOfOrder: string[];
    anyErrors: boolean;
};

export type CredentialsResource = {
    apiKey: string;
    sessionId: string;
    authenticated: boolean;
    message: string;
    jwtToken: string;
    emailAddress: string;
};

export type PasswordResetRequestResource = {
    message: string;
    status: boolean;
};

export type ResetPasswordResource = {
    message: string;
    status: boolean;
};

export type PasswordVerificationResource = {
    message: string;
    status: boolean;
    apiKey: string;
};

export type Product = StripeProduct & {
    attributes: Array<string>;
    caption: string | null;
    deactivateOn: Date | null;
    description: string;
    images: Images;
    name: string;
    packageDimensions: string | null;
    shippable: boolean | null;
    statementDescriptor: string | null;
    unitLabel: string | null;
    updated: Date;
    url: string | null;
};

export type Products = Product[];
export type PriceResources = PriceResource[];
export type PriceResource = StripeProduct & {
    billingScheme: string;
    currency: "usd" | "aud" | "can" | "eur";
    lookupKey: string | null;
    nickname: string | null;
    productId: string; // equals Product type id
    product: string | null;
    recurring: {
        aggregateUsage: string | null;
        interval: "month" | "year";
        intervalCount: number;
        trialPeriodDays: number | null;
        usageType: string;
        rawJObject: object;
        stripeResponse: null;
    };
    tiers: null; // TODO
    tiersMode: null; // TODO
    transformQuantity: null; // TODO
    unitAmount: number; // cents
    unitAmountDecimal: number; // in cents
};

// export type PriceResource = {
//     id: string;
//     object: string;
//     active: boolean;
//     billingScheme: string;
//     created: Date;
//     currency: string;
//     deleted: boolean[];
//     livemode: boolean;
//     lookupKey: string;
//     metadata: string[];
//     nickname: string;
//     productId: string;
//     product: Product;
//     recurring: PriceRecurring;
//     tiers: PriceTier[];
//     tiersMode: string;
//     transformQuantity: PriceTransformQuantity;
//     type: string;
//     unitAmount: Int64[];
//     unitAmountDecimal: Decimal[];
//     rawJObject: JObject;
//     stripeResponse: StripeResponse;
// };

export type PlanTypeMetaResource = {
    allowedAttachments: number;
    allowedStaticTables: number;
    allowedPricingStrategyTables: number;
    allowedIntents: number;
    allowedFileUpload: boolean;
    allowedEmailNotifications: boolean;
    allowedInlineEmailEditor: boolean;
    allowedSmsNotifications: boolean;
    planType: string;
    isFreePlan: boolean;
};

export type AccountEmailSettingsResource = {
    emailAddress: string;
    isVerified: boolean;
    awaitingVerification: boolean;
};

export type LocaleMetaResource = {
    currentLocale: LocaleResource;
    localeMap: LocaleResource[];
};

export type PhoneDetailsResource = {
    phoneNumber: string;
    locale: string;
};

export type LocaleResource = {
    currentLocale: LocaleResource;
    localeMap: LocaleResource[];
};
