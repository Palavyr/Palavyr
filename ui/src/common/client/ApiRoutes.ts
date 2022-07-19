import { SecretKey } from "@Palavyr-Types";

export class ApiRoutes {
    public Routes = {
        GetCustomerIdRoute: () => `payments/customer-id`,
        GetCustomerPortalRoute: () => `payments/customer-portal`,
        GetPrices: (productId: string) => `products/prices/get-prices/${productId}`,
        CreateCheckoutSession: () => `checkout/create-checkout-session`,
        GetProducts: () => `products/all`,
        ToggleIsEnabled: () => `intents/intent-toggle`,
        ToggleUseIntentFallbackEmail: () => `intents/use-fallback-email-toggle`,
        GetAllIntents: () => `intents`,
        CreateIntent: () => `intents/create`,
        UpdateIntentName: (intentId: string) => `intents/update/name/${intentId}`,
        DeleteIntent: (intentId: string) => `intents/delete/${intentId}`,
        ToggleSendPdfResponse: (intentId: string) => `intent/send-pdf/${intentId}`,
        GetShowPricingStrategyTotals: (intentId: string) => `intent/pricing-strategy-totals/${intentId}`,
        SetShowricingStrategyTotals: () => `intent/pricing-strategy-totals`,
        GetEstimateConfiguration: (intentId: string) => `response/configuration/${intentId}`,
        UpdatePrologue: () => `response/configuration/prologue`,
        UpdateEpilogue: () => `response/configuration/epilogue`,
        GetSupportedUnitIds: () => `configuration/unit-types`,
        GetWidgetState: () => `widget-config/widget-active-state`,
        SetWidgetState: (updatedWidgetState: boolean) => `widget-config/widget-active-state?state=${updatedWidgetState}`,
        GetPricingStrategyMetas: (intentId: string) => `tables/pricing-strategy/metas/${intentId}`,
        GetPricingStrategyTypes: () => `tables/pricing-strategy/table-name-map`,
        ModifyPricingStrategyMeta: () => `tables/pricing-strategy/modify`,
        CreatePricingStrategy: (intentId: string) => `tables/pricing-strategy/SimpleThresholdTableRow/create/${intentId}`,
        SavePricingStrategy: (intentId: string, tableType: string, tableId: string) => `tables/pricing-strategy/${tableType}/intent/${intentId}/table/${tableId}`,
        DeletePricingStrategy: (intentId: string, tableType: string, tableId: string) => `tables/pricing-strategy/${tableType}/intent/${intentId}/table/${tableId}`,
        GetPricingStrategyDataTemplate: (intentId: string, tableType: string, tableId: string) => `tables/pricing-strategy/${tableType}/intent/${intentId}/table/${tableId}/template`,
        GetPricingStrategyRows: (intentId: string, tableType: string, tableId: string) => `tables/pricing-strategy/${tableType}/intent/${intentId}/table/${tableId}`,
        UpdateStaticTableMetas: () => `response/configuration/static/tables/save`,
        GetStaticTablesMetaTemplate: (intentId: string) => `response/configuration/${intentId}/static/tables/template`,
        GetStaticTableRowTemplate: (intentId: string, tableId: string) => `response/configuration/${intentId}/static/tables/${tableId}/row/template`,
        FetchPreview: (intentId: string) => `preview/estimate/${intentId}`,

        GetVariableDetails: () => `email/variables`,

        GetIntentEmailTemplate: (intentId: string) => `email/${intentId}/email-template`,
        GetIntentFallbackEmailTemplate: (intentId: string) => `email/fallback/${intentId}/email-template`,
        GetDefaultFallbackEmailTemplate: () => `email/fallback/default-email-template`,

        SaveIntentEmailTemplate: () => `email/email-template`,
        SaveIntentFallbackEmailTemplate: () => `email/fallback/email-template`,
        SaveDefaultFallbackEmailTemplate: () => `email/fallback/default-email-template`,

        GetIntentSubject: (intentId: string) => `email/subject/${intentId}`,
        GetIntentFallbackSubject: (intentId: string) => `email/fallback/subject/${intentId}`,
        GetDefaultFallbackSubject: () => `email/default-fallback-subject`,

        SaveIntentSubject: () => `email/subject`,
        SaveIntentFallbackSubject: () => `email/fallback/subject`,
        SaveDefaultFallbackSubject: () => `email/fallback/default-subject`,

        GetAttachments: (intentId: string) => `attachments/${intentId}`,
        DeleteAttachment: () => `attachments`,
        UploadAttachments: (intentId: string) => `attachments/${intentId}/upload`,

        GetFileAssets: (fileIds: string[]) => (fileIds !== undefined && fileIds.length > 0 ? `file-assets?fileIds=${fileIds.join(",")}` : `file-assets`),
        UploadFileAssets: () => `file-assets/upload`,

        LinkFileAssetToNode: (fileId: string, nodeId: string) => `file-assets/link/${fileId}/node/${nodeId}`,
        LinkFileAssetToIntent: (fileId: string, intentId: string) => `file-assets/link/${fileId}/intent/${intentId}`,
        LinkFileAssetToLogo: (fileId: string) => `file-assets/link/${fileId}/logo`,
        DeleteFileAsset: (fileIds: string[]) => `file-assets?fileIds=${fileIds.join(",")}`,

        GetConversation: (intentId: string) => `configure-conversations/${intentId}`,
        GetConversationNode: (nodeId: string) => `configure-conversations/nodes/${nodeId}`,
        GetNodeOptionsList: (intentId: string) => `configure-conversations/${intentId}/node-type-options`,
        GetIntroNodeOptionsList: () => `configure-intro/node-type-options`,
        GetErrors: () => `configure-conversations/tree-errors`,
        GetIntroErrors: () => `configure-conversations/intro/tree-errors`,

        ModifyConversation: () => `configure-conversations`,
        ModifyConversationNode: (nodeId: string, intentId: string) => `configure-conversations/${intentId}/nodes/${nodeId}`,
        ModifyConversationNodeText: () => `configure-conversations/nodes/text`,

        RunConversationPrecheck: () => `widget-config/demo/pre-check`,
        GetWidetPreferences: () => `widget-config/preferences`,
        SaveWidgetPreferences: () => `widget-config/preferences`,

        GetCurrentPlanMeta: () => `account/settings/current-plan-meta`,

        CancelRegistration: () => `account/cancel-registration`,
        GetApiKey: () => `account/settings/api-key`,
        ConfirmEmailAddress: (authToken: string) => `account/confirmation/${authToken}/action/setup`,
        ResendConfirmationToken: () => `account/confirmation/token/resend`,
        CheckIsActive: () => `account/is-active`,

        UpdatePassword: () => `account/settings/password`,
        UpdateCompanyName: () => `account/settings/company-name`,
        UpdateEmail: () => `account/settings/email`,
        UpdatePhoneNumber: () => `account/settings/phone-number`,
        UpdateLocale: () => `account/settings/locale`,
        UpdateCompanyLogo: () => `account/settings/logo`,

        GetCompanyName: () => `account/settings/company-name`,
        GetEmail: () => `account/settings/email`,
        GetPhoneNumber: () => `account/settings/phone-number`,

        GetLocale: (readonly: boolean = false) => `account/settings/locale?read=${readonly}`,
        GetCompanyLogo: () => `account/settings/logo`,
        GetIntroductionId: () => `account/settings/intro-id`,
        UpdateIntroduction: () => `account/settings/intro-id`,

        DeleteCompanyLogo: () => `file-assets/unlink/logo`,
        DeleteAccount: () => `account/delete-account`,

        RequestEmailVerification: () => `verification/email`,
        CheckEmailVerificationStatus: () => `verification/email/status`,

        GetEnquiries: () => `enquiries`,
        GetEnquiryCount: () => `enquiries/count`,
        GetShowSeenEnquiries: () => `enquiries/show`,
        ToggleShowSeenEnquiries: () => `enquiries/toggle-show`,

        UpdateSeen: () => `enquiries/seen`,
        DeleteSelected: () => `enquiries/delete`,

        GetConversationHistory: (conversationId: string) => `enquiries/review/${conversationId}`,

        GetEnquiryInsights: () => `enquiry-insights`,

        UnselectAll: () => `enquiries/selectall`,
        SelectAll: () => `enquiries/unselectall`,

        // Widget
        precheck: (secretKey: SecretKey, isDemo: boolean) => `widget/pre-check?key=${secretKey}&demo=${isDemo}`,
        widgetPreferences: (secretKey: SecretKey) => `widget/preferences?key=${secretKey}`,
        locale: (secretKey: SecretKey) => `account/settings/locale/widget?key=${secretKey}`,
        intents: (secretKey: SecretKey) => `widget/intents?key=${secretKey}`,
        newConversationHistory: (secretKey: SecretKey, isDemo: boolean) => `widget/create?key=${secretKey}&demo=${isDemo}`,
        updateConvoHistory: (secretKey: SecretKey) => `widget/conversation?key=${secretKey}`,
        updateConvoRecord: (secretKey: SecretKey) => `widget/record?key=${secretKey}`,
        confirmationEmail: (secretKey: SecretKey, intentId: string, isDemo: boolean) => `widget/intent/${intentId}/email/send?key=${secretKey}&demo=${isDemo}`,
        FallbackEmail: (secretKey: SecretKey, intentId: string, isDemo: boolean) => `widget/intent/${intentId}/email/fallback/send?key=${secretKey}&demo=${isDemo}`,
        InternalCheck: (secretKey: SecretKey) => `widget/internal-check?key=${secretKey}`,
        GetIntroSequence: (secretKey: SecretKey) => `account/settings/intro-sequence?key=${secretKey}`,

        // Login
        RequestLogin: () => "authentication/login",
        CheckIfLoggedIn: () => `authentication/status`,
        RegisterNewAccount: () => `account/create/default`,
        ResetPasswordRequest: () => `authentication/password-reset-request`,
        VerifyResetIdentity: (token: string) => `authentication/verify-password-reset/${token}`,
        ResetPassword: (secretKey: string) => `authentication/reset-my-password?key=${secretKey}`,

        // Logout
        RequestLogout: () => `authentication/logout`,
    };
}
