export type NewConversationResource = {
    conversationId: String
    conversationNodes: WidgetNodeResource
    }


    export type IntentResource = {
    intentId: String
    intentName: String
    prologue: String
    epilogue: String
    emailTemplate: String
    isEnabled: Boolean
    staticTablesMetas: StaticTablesMeta[]
    conversationNodes: ConversationNode[]
    pricingStrategyTableMetas: PricingStrategyTableMeta[]
    intentSpecificEmail: String
    emailIsVerified: Boolean
    attachmentRecords: AttachmentLinkRecord[]
    useIntentFallbackEmail: Boolean
    fallbackSubject: String
    fallbackEmailTemplate: String
    sendAttachmentsOnFallback: Boolean
    sendPdfResponse: Boolean
    includePricingStrategyTableTotals: Boolean
    subject: String
    }


    export type FileAssetResource = {
    fileName: String
    fileId: String
    link: String
    }


    export type WidgetPreferenceResource = {
    placeholder: String
    accountId: String
    landingHeader: String
    chatHeader: String
    selectListColor: String
    listFontColor: String
    headerColor: String
    headerFontColor: String
    fontFamily: String
    optionsHeaderColor: String
    optionsHeaderFontColor: String
    chatFontColor: String
    chatBubbleColor: String
    buttonColor: String
    buttonFontColor: String
    widgetState: Boolean
    }


    export type PreCheckResultResource = {
    isReady: Boolean
    preCheckErrors: PreCheckError
    apiKeyExists: Boolean
    }


    export type SendLiveEmailResultResource = {
    nextNodeId: String
    result: Boolean
    fileAsset: FileAssetResource
    }


    export type EmailVerificationResponse = {
    title: String
    message: String
    status: String
    }


    export type String = {
    chars: Char
    length: Int32
    }


    export type ResponseVariableResource = {
    responseVariables: ResponseVariable
    }


    export type QuantityUnitResource = {
    unitGroup: String
    unitPrettyName: String
    unitId: UnitIds
    }


    export type StaticTablesMetaResource = {
    tableOrder: Int32
    description: String
    intentId: String
    staticTableRows: StaticTableRowResource[]
    accountId: String
    perPersonInputRequired: Boolean
    includeTotals: Boolean
    }


    export type StaticTableRowResource = {
    rowOrder: Int32
    description: String
    fee: StaticFeeResource
    range: Boolean
    perPerson: Boolean
    tableOrder: Int32
    intentId: String
    accountId: String
    }


    export type NodeTypeOptionResource = {
    nodeTypeCode: NodeTypeCode
    value: String
    text: String
    pathOptions: String[]
    valueOptions: String[]
    isMultiOptionType: Boolean
    isTerminalType: Boolean
    shouldRenderChildren: Boolean
    shouldShowMultiOption: Boolean
    isSplitMergeType: Boolean
    isAnabranchType: Boolean
    isAnabranchMergePoint: Boolean
    isPricingStrategyType: Boolean
    nodeComponentType: String
    isCurrency: Boolean
    isMultiOptionEditable: Boolean
    groupName: String
    resolveOrder: Int32[]
    pricingStrategyType: String
    isImageNode: Boolean
    isLoopbackAnchor: Boolean
    stringName: String
    }


    export type PricingStrategyTableTypeResource = {
    prettyName: String
    tableType: String
    }


    export type BasicThresholdResource = {
    accountId: String
    intentId: String
    rowId: String
    threshold: Double
    valueMin: Double
    valueMax: Double
    range: Boolean
    itemName: String
    rowOrder: Int32
    triggerFallback: Boolean
    id: Int32
    tableId: String
    }


    export type PricingStrategyTableMetaResource = {
    id: Int32
    tableTag: String
    tableType: String
    tableId: String
    intentId: String
    prettyName: String
    unitPrettyName: String
    unitGroup: String
    unitId: UnitIds
    accountId: String
    valuesAsPaths: Boolean
    }


    export type CategoryNestedThresholdResource = {
    accountId: String
    intentId: String
    valueMin: Double
    valueMax: Double
    range: Boolean
    rowId: String
    rowOrder: Int32
    itemId: String
    itemOrder: Int32
    itemName: String
    threshold: Double
    triggerFallback: Boolean
    id: Int32
    tableId: String
    }


    export type PercentOfThresholdResource = {
    accountId: String
    intentId: String
    rowId: String
    threshold: Double
    valueMin: Double
    valueMax: Double
    range: Boolean
    modifier: Double
    posNeg: Boolean
    rowOrder: Int32
    triggerFallback: Boolean
    itemOrder: Int32
    itemId: String
    itemName: String
    id: Int32
    tableId: String
    }


    export type SelectOneFlatResource = {
    accountId: String
    intentId: String
    category: String
    valueMin: Double
    valueMax: Double
    range: Boolean
    rowOrder: Int32
    id: Int32
    tableId: String
    }


    export type TwoNestedCategoryResource = {
    accountId: String
    intentId: String
    valueMin: Double
    valueMax: Double
    range: Boolean
    rowId: String
    rowOrder: Int32
    itemId: String
    itemOrder: Int32
    itemName: String
    innerItemName: String
    id: Int32
    tableId: String
    }


    export type PricingStrategyTableMetaResource[] = {
    length: Int32
    longLength: Int64
    rank: Int32
    syncRoot: Object
    isReadOnly: Boolean
    isFixedSize: Boolean
    isSynchronized: Boolean
    }


    export type ProductIdResource = {
    freeProductId: String
    lyteProductId: String
    premiumProductId: String
    proProductId: String
    }


    export type Intent = {
    intentId: String
    intentName: String
    prologue: String
    epilogue: String
    emailTemplate: String
    isEnabled: Boolean
    staticTablesMetas: StaticTablesMeta
    conversationNodes: ConversationNode
    accountId: String
    pricingStrategyTableMetas: PricingStrategyTableMeta
    intentSpecificEmail: String
    emailIsVerified: Boolean
    attachmentRecords: AttachmentLinkRecord
    useIntentFallbackEmail: Boolean
    fallbackSubject: String
    fallbackEmailTemplate: String
    sendAttachmentsOnFallback: Boolean
    sendPdfResponse: Boolean
    includePricingStrategyTableTotals: Boolean
    subject: String
    awaitingVerification: Boolean
    id: Int32
    }


    export type EnquiryResource = {
    id: Int32[]
    conversationId: String
    fileAssetResource: FileAssetResource
    timeStamp: String
    accountId: String
    intentName: String
    emailTemplateUsed: String
    seen: Boolean
    name: String
    email: String
    phoneNumber: String
    hasResponse: Boolean
    }


    export type EnquiryInsightsResource[] = {
    length: Int32
    longLength: Int64
    rank: Int32
    syncRoot: Object
    isReadOnly: Boolean
    isFixedSize: Boolean
    isSynchronized: Boolean
    }


    export type ConversationRowsResource[] = {
    length: Int32
    longLength: Int64
    rank: Int32
    syncRoot: Object
    isReadOnly: Boolean
    isFixedSize: Boolean
    isSynchronized: Boolean
    }


    export type ConversationDesignerNodeResource = {
    intentId: String
    accountId: String
    nodeId: String
    text: String
    isRoot: Boolean
    isCritical: Boolean
    isMultiOptionType: Boolean
    isTerminalType: Boolean
    shouldRenderChildren: Boolean
    isLoopbackAnchorType: Boolean
    isAnabranchType: Boolean
    isAnabranchMergePoint: Boolean
    shouldShowMultiOption: Boolean
    isPricingStrategyNode: Boolean
    isMultiOptionEditable: Boolean
    isImageNode: Boolean
    fileId: String
    optionPath: String
    valueOptions: String
    nodeType: String
    pricingStrategyType: String
    nodeComponentType: String
    resolveOrder: Int32
    isCurrency: Boolean
    nodeChildrenString: String
    nodeTypeCode: NodeTypeCode
    }


    export type TreeErrorsResource = {
    missingNodes: String[]
    outOfOrder: String[]
    anyErrors: Boolean
    }


    export type CredentialsResource = {
    apiKey: String
    sessionId: String
    authenticated: Boolean
    message: String
    jwtToken: String
    emailAddress: String
    }


    export type PasswordResetRequestResource = {
    message: String
    status: Boolean
    }


    export type ResetPasswordResource = {
    message: String
    status: Boolean
    }


    export type PasswordVerificationResource = {
    message: String
    status: Boolean
    apiKey: String
    }


    export type PriceResource = {
    id: String
    object: String
    active: Boolean
    billingScheme: String
    created: DateTime
    currency: String
    deleted: Boolean[]
    livemode: Boolean
    lookupKey: String
    metadata: String[]
    nickname: String
    productId: String
    product: Product
    recurring: PriceRecurring
    tiers: PriceTier[]
    tiersMode: String
    transformQuantity: PriceTransformQuantity
    type: String
    unitAmount: Int64[]
    unitAmountDecimal: Decimal[]
    rawJObject: JObject
    stripeResponse: StripeResponse
    }


    export type PlanStatusResource = {
    status: String
    hasUpgraded: Boolean
    }


    export type PlanTypeMetaResource = {
    allowedAttachments: Int32
    allowedStaticTables: Int32
    allowedPricingStrategyTables: Int32
    allowedIntents: Int32
    allowedFileUpload: Boolean
    allowedEmailNotifications: Boolean
    allowedInlineEmailEditor: Boolean
    allowedSmsNotifications: Boolean
    planType: String
    isFreePlan: Boolean
    }


    export type AccountEmailSettingsResource = {
    emailAddress: String
    isVerified: Boolean
    awaitingVerification: Boolean
    }


    export type LocaleMetaResource = {
    currentLocale: LocaleResource
    localeMap: LocaleResource[]
    }


    export type PhoneDetailsResource = {
    phoneNumber: String
    locale: String
    }


    export type ConversationDesignerNodeResource[] = {
    length: Int32
    longLength: Int64
    rank: Int32
    syncRoot: Object
    isReadOnly: Boolean
    isFixedSize: Boolean
    isSynchronized: Boolean
    }


    export type LocaleResource = {
    currentLocale: LocaleResource
    localeMap: LocaleResource[]
    }


