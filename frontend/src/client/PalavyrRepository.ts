import {
    DynamicTableMetas,
    DynamicTableMeta,
    StaticTableMetas,
    StaticTableMetaTemplate,
    ConvoTableRow,
    Areas,
    Prices,
    EmailVerificationResponse,
    AreaTable,
    FileLink,
    ConvoNode,
    ResponseConfigurationType,
    AccountEmailSettingsResponse,
    Enquiries,
    PhoneSettingsResponse,
    NodeTypeOptions,
    CompletedConversation,
    PreCheckResult,
    WidgetPreferences,
    VariableDetail,
    LocaleDefinition,
    ProductIds,
    PlanStatus,
    TableData,
    TableNameMap,
    TreeErrors,
    StaticTableRow,
    PlanTypeMeta,
} from "@Palavyr-Types";
import { filterNodeTypeOptionsOnSubscription } from "dashboard/subscriptionFilters/filterConvoNodeTypes";
import { SessionStorage } from "localStorage/sessionStorage";
import { AxiosClient, CacheIds } from "./AxiosClient";
import { getJwtTokenFromLocalStorage, getSessionIdFromLocalStorage } from "./clientUtils";

export class PalavyrRepository {
    private client: AxiosClient;

    private formDataHeaders: { [key: string]: string } = {
        Accept: "application/json",
        "Content-Type": "multipart/form-data",
    };

    constructor() {
        this.client = new AxiosClient("tubmcgubs", getSessionIdFromLocalStorage, getJwtTokenFromLocalStorage);
    }

    public AuthenticationCheck = {
        check: async () => {
            let result: boolean;
            try {
                await this.client.get<boolean>("");
                result = true;
            } catch {
                result = false;
            }
            return result;
        },
    };

    public Purchase = {
        Customer: {
            GetCustomerId: async () => this.client.get<string>(`payments/customer-id`, CacheIds.CustomerId),
            GetCustomerPortal: async (customerId: string, returnUrl: string) => {
                SessionStorage.clearCacheValue(CacheIds.CurrentPlanMeta);
                return this.client.post<string, {}>(`payments/customer-portal`, { CustomerId: customerId, ReturnUrl: returnUrl });
            },
        },
        Subscription: {
            CancelSubscription: async () => this.client.post<string, {}>(`products/cancel-subscription`),
        },
        Prices: {
            GetPrices: async (productId: string) => this.client.get<Prices>(`products/prices/get-prices/${productId}`),
        },
        Checkout: {
            CreateCheckoutSession: async (priceId: string, cancelUrl: string, successUrl: string) =>
                this.client.post<string, {}>(`checkout/create-checkout-session`, {
                    PriceId: priceId,
                    CancelUrl: cancelUrl,
                    SuccessUrl: successUrl,
                }),
        },
    };

    public Products = {
        getProducts: async () => this.client.get<ProductIds>(`products/all`),
    };

    public Area = {
        UpdateIsEnabled: async (areaToggleStateUpdate: boolean, areaIdentifier: string) => {
            const update = this.client.put<boolean, {}>(`areas/${areaIdentifier}/area-toggle`, { IsEnabled: areaToggleStateUpdate });
            SessionStorage.clearCacheValue(CacheIds.Areas);
            return update;
        },
        UpdateUseAreaFallbackEmail: async (useAreaFallbackEmailUpdate: boolean, areaIdentifier: string) =>
            this.client.put<boolean, {}>(`areas/${areaIdentifier}/use-fallback-email-toggle`, { UseFallback: useAreaFallbackEmailUpdate }),
        GetAreas: async () => this.client.get<Areas>("areas", CacheIds.Areas),
        createArea: (areaName: string) => this.client.post<AreaTable, {}>(`areas/create/`, { AreaName: areaName }, CacheIds.Areas), // get creates and gets new area

        updateAreaName: (areaIdentifier: string, areaName: string) => {
            const result = this.client.put<string, {}>(`areas/update/name/${areaIdentifier}`, { AreaName: areaName });
            SessionStorage.clearCacheValue(CacheIds.Areas);
            return result;
        },
        updateDisplayTitle: (areaIdentifier: string, displayTitle: string) => {
            const result = this.client.put<string, {}>(`areas/update/display-title/${areaIdentifier}`, { AreaDisplayTitle: displayTitle });
            SessionStorage.clearCacheValue(CacheIds.Areas);
            return result;
        },

        deleteArea: (areaIdentifier: string) => this.client.delete<void>(`areas/delete/${areaIdentifier}`, CacheIds.Areas),
        toggleSendPdfResponse: (areaIdentifier: string) => this.client.post<boolean, {}>(`area/send-pdf/${areaIdentifier}`),
    };

    public Configuration = {
        getEstimateConfiguration: async (areaIdentifier: string) => this.client.get<ResponseConfigurationType>(`response/configuration/${areaIdentifier}`),
        updatePrologue: async (areaIdentifier: string, prologue: string) => this.client.put<string, {}>(`response/configuration/${areaIdentifier}/prologue`, { prologue: prologue }),
        updateEpilogue: async (areaIdentifier: string, epilogue: string) => this.client.put<string, {}>(`response/configuration/${areaIdentifier}/epilogue`, { epilogue: epilogue }),

        WidgetState: {
            GetWidgetState: async () => this.client.get<boolean>(`widget-config/widget-active-state`),
            SetWidgetState: async (updatedWidgetState: boolean) => this.client.post<boolean, {}>(`widget-config/widget-active-state?state=${updatedWidgetState}`),
        },
        Tables: {
            Dynamic: {
                getDynamicTableMetas: async (areaIdentifier: string) => this.client.get<DynamicTableMetas>(`tables/dynamic/type/${areaIdentifier}`), // todo - cache

                getDynamicTableTypes: async () => this.client.get<TableNameMap>(`tables/dynamic/table-name-map`),

                modifyDynamicTableMeta: async (dynamicTableMeta: DynamicTableMeta) => {
                    const response = this.client.put<DynamicTableMeta, {}>(`tables/dynamic/modify`, dynamicTableMeta);
                    SessionStorage.clearCacheValue([CacheIds.PalavyrConfiguration, dynamicTableMeta.areaId].join("-"));
                    return response;
                },

                createDynamicTable: async (areaIdentifier: string) => {
                    const response = this.client.post<DynamicTableMeta, {}>(`tables/dynamic/${areaIdentifier}`);
                    SessionStorage.clearCacheValue([CacheIds.PalavyrConfiguration, areaIdentifier].join("-"));
                    return response;
                },

                deleteDynamicTable: async (areaIdentifier: string, tableType: string, tableId: string) => {
                    const response = this.client.delete(`tables/dynamic/${tableType}/area/${areaIdentifier}/table/${tableId}`);
                    SessionStorage.clearCacheValue([CacheIds.PalavyrConfiguration, areaIdentifier].join("-"));
                    return response;
                },

                getDynamicTableDataTemplate: async <T>(areaIdentifier: string, tableType: string, tableId: string) =>
                    this.client.get<T>(`tables/dynamic/${tableType}/area/${areaIdentifier}/table/${tableId}/template`),

                getDynamicTableRows: async (areaIdentifier: string, tableType: string, tableId: string) => this.client.get<TableData>(`tables/dynamic/${tableType}/area/${areaIdentifier}/table/${tableId}`),

                saveDynamicTable: async <T>(areaIdentifier: string, tableType: string, tableData: TableData, tableId: string, tableTag: string) => {
                    const response = this.client.put<T, {}>(`tables/dynamic/${tableType}/area/${areaIdentifier}/table/${tableId}`, { TableTag: tableTag, [tableType]: tableData });
                    SessionStorage.clearCacheValue([CacheIds.PalavyrConfiguration, areaIdentifier].join("-"));
                    return response;
                },
            },
            Static: {
                updateStaticTablesMetas: async (areaIdentifier: string, staticTablesMetas: StaticTableMetas) =>
                    this.client.put<StaticTableMetas, {}>(`response/configuration/${areaIdentifier}/static/tables/save`, staticTablesMetas),
                getStaticTablesMetaTemplate: async (areaIdentifier: string) => this.client.get<StaticTableMetaTemplate>(`response/configuration/${areaIdentifier}/static/tables/template`),
                getStaticTableRowTemplate: async (areaIdentifier: string, tableOrder: number) =>
                    this.client.get<StaticTableRow>(`response/configuration/${areaIdentifier}/static/tables/${tableOrder}/row/template`),
            },
        },

        Preview: {
            fetchPreview: async (areaIdentifier: string) => this.client.get<FileLink>(`preview/estimate/${areaIdentifier}`),
        },

        Email: {
            GetVariableDetails: async () => this.client.get<VariableDetail[]>(`email/variables`),

            GetAreaEmailTemplate: async (areaIdentifier: string) => this.client.get<string>(`email/${areaIdentifier}/email-template`),
            GetAreaFallbackEmailTemplate: async (areaIdentifier: string) => this.client.get<string>(`email/fallback/${areaIdentifier}/email-template`),
            GetDefaultFallbackEmailTemplate: async () => this.client.get<string>(`email/fallback/default-email-template`),

            SaveAreaEmailTemplate: async (areaIdentifier: string, EmailTemplate: string) => this.client.put<string, {}>(`email/${areaIdentifier}/email-template`, { EmailTemplate }),
            SaveAreaFallbackEmailTemplate: async (areaIdentifier: string, EmailTemplate: string) => this.client.put<string, {}>(`email/fallback/${areaIdentifier}/email-template`, { EmailTemplate }),
            SaveDefaultFallbackEmailTemplate: async (EmailTemplate: string) => this.client.put<string, {}>(`email/fallback/default-email-template`, { EmailTemplate }),

            GetAreaSubject: (areaIdentifier: string) => this.client.get<string>(`email/subject/${areaIdentifier}`),
            GetAreaFallbackSubject: (areaIdentifier: string) => this.client.get<string>(`email/fallback/subject/${areaIdentifier}`),
            GetDefaultFallbackSubject: async () => this.client.get<string>(`email/default-fallback-subject`),

            SaveAreaSubject: (areaIdentifier: string, subject: string) => this.client.put<string, {}>(`email/subject/${areaIdentifier}`, { Subject: subject }),
            SaveAreaFallbackSubject: (areaIdentifier: string, subject: string) => this.client.put<string, {}>(`email/fallback/subject/${areaIdentifier}`, { Subject: subject }),
            SaveDefaultFallbackSubject: async (subject: string) => this.client.put<string, {}>(`email/fallback/default-subject`, { Subject: subject }),
        },

        Attachments: {
            fetchAttachmentLinks: async (areaIdentifier: string) => this.client.get<FileLink[]>(`attachments/${areaIdentifier}`, CacheIds.Attachments),
            removeAttachment: async (areaIdentifier: string, fileId: string) => this.client.delete<FileLink[]>(`attachments/${areaIdentifier}/file-link`, CacheIds.Attachments, { data: { fileId: fileId } }),

            saveSingleAttachment: async (areaIdentifier: string, formData: FormData) =>
                this.client.post<FileLink[], {}>(`attachments/${areaIdentifier}/save-one`, formData, CacheIds.Attachments, {
                    headers: {
                        Accept: "application/json",
                        "Content-Type": "multipart/form-data",
                    },
                }),
            saveManyAttachments: async (areaIdentifier: string, formData: FormData) =>
                this.client.post<FileLink[], {}>(`attachments/${areaIdentifier}/save-many`, formData, CacheIds.Attachments, {
                    headers: {
                        Accept: "application/json",
                        "Content-Type": "multipart/form-data",
                    },
                }),
        },

        Images: {
            // Node Editor Flow
            saveSingleImage: async (formData: FormData) => this.client.post<FileLink[], {}>(`images/save-one`, formData, undefined, { headers: this.formDataHeaders }),
            saveImageUrl: async (url: string, nodeId: string) => this.client.post<FileLink[], {}>(`images/use-link/${nodeId}`, { Url: url }),
            getImages: async (imageIds?: string[]) => this.client.get<FileLink[]>(`images${imageIds !== undefined ? `?imageIds=${imageIds.join(",")}` : ""}`), // takes a querystring comma delimieted of imageIds
            savePreExistingImage: async (imageId: string, nodeId: string) => this.client.post<ConvoNode, {}>(`images/pre-existing/${imageId}/${nodeId}`),
            // DO NOT USE WITH NODE
            saveMultipleImages: async (formData: FormData) => this.client.post<FileLink[], {}>(`images/save-many`, formData, undefined, { headers: this.formDataHeaders }),
            deleteImage: async (imageIds: string[]) => this.client.delete<FileLink[]>(`images?imageIds=${imageIds.join(",")}`), // takes a querystring command delimited of imageIds
            getSignedUrl: async (s3Key: string) => this.client.post<string, {}>(`images/link`, { s3Key: s3Key }),
        },
    };

    public Conversations = {
        GetConversation: async (areaIdentifier: string) => this.client.get<ConvoNode[]>(`configure-conversations/${areaIdentifier}`, [CacheIds.PalavyrConfiguration, areaIdentifier].join("-") as CacheIds),
        GetConversationNode: async (nodeId: string) => this.client.get<ConvoNode>(`configure-conversations/nodes/${nodeId}`),
        GetNodeOptionsList: async (areaIdentifier: string, planTypeMeta: PlanTypeMeta) =>
            filterNodeTypeOptionsOnSubscription(await this.client.get<NodeTypeOptions>(`configure-conversations/${areaIdentifier}/node-type-options`), planTypeMeta),
        GetErrors: async (areaIdentifier: string, nodeList: ConvoNode[]) => this.client.post<TreeErrors, {}>(`configure-conversations/${areaIdentifier}/tree-errors`, { Transactions: nodeList }),

        ModifyConversation: async (nodelist: ConvoNode[], areaIdentifier: string) =>
            this.client.put<ConvoNode[], {}>(`configure-conversations/${areaIdentifier}`, { Transactions: nodelist }, [CacheIds.PalavyrConfiguration, areaIdentifier].join("-") as CacheIds),
        ModifyConversationNode: async (nodeId: string, areaIdentifier: string, updatedNode: ConvoTableRow) =>
            this.client.put<ConvoNode[], {}>(`configure-conversations/${areaIdentifier}/nodes/${nodeId}`, updatedNode, [CacheIds.PalavyrConfiguration, areaIdentifier].join("-") as CacheIds),

        // TODO: Deprecate eventually
        EnsureDBIsValid: async () => this.client.post(`configure-conversations/ensure-db-valid`),
    };

    public WidgetDemo = {
        RunConversationPrecheck: async () => this.client.get<PreCheckResult>(`widget-config/demo/pre-check`),
        GetWidetPreferences: async () => this.client.get<WidgetPreferences>(`widget-config/preferences`, CacheIds.WidgetPrefs),
        SaveWidgetPreferences: async (prefs: WidgetPreferences) => this.client.put<WidgetPreferences, WidgetPreferences>(`widget-config/preferences`, prefs, CacheIds.WidgetPrefs),
    };

    public Settings = {
        Subscriptions: {
            getCurrentPlanMeta: async () => this.client.get<PlanTypeMeta>(`account/settings/current-plan-meta`, CacheIds.CurrentPlanMeta),
        },

        Account: {
            getApiKey: async () => this.client.get<string>(`account/settings/api-key`),
            confirmEmailAddress: async (authToken: string) => this.client.post<boolean, {}>(`account/confirmation/${authToken}/action/setup`),
            resendConfirmationToken: async (emailAddress: string) => this.client.post<boolean, {}>(`account/confirmation/token/resend`, { EmailAddress: emailAddress }),
            checkIsActive: async () => this.client.get<boolean>(`account/is-active`),

            UpdatePassword: async (oldPassword: string, newPassword: string) => this.client.put<boolean, {}>(`account/settings/password`, { OldPassword: oldPassword, Password: newPassword }),
            updateCompanyName: async (companyName: string) => this.client.put<string, {}>(`account/settings/company-name`, { CompanyName: companyName }, CacheIds.CompanyName),
            updateEmail: async (newEmail: string) => this.client.put<EmailVerificationResponse, {}>(`account/settings/email`, { EmailAddress: newEmail }, CacheIds.Email),
            updatePhoneNumber: async (newPhoneNumber: string) => this.client.put<string, {}>(`account/settings/phone-number`, { PhoneNumber: newPhoneNumber }, CacheIds.PhoneNumber),
            updateLocale: async (newLocaleId: string) => this.client.put<LocaleDefinition, {}>(`account/settings/locale`, { LocaleId: newLocaleId }, CacheIds.Locale),
            updateCompanyLogo: async (formData: FormData) =>
                this.client.put<string, {}>(`account/settings/logo`, formData, CacheIds.Logo, {
                    headers: {
                        Accept: "application/json",
                        "Content-Type": "multipart/form-data",
                    },
                }),

            getCompanyName: async () => this.client.get<string>(`account/settings/company-name`, CacheIds.CompanyName),
            getEmail: async () => this.client.get<AccountEmailSettingsResponse>(`account/settings/email`, CacheIds.Email),
            getPhoneNumber: async () => this.client.get<PhoneSettingsResponse>(`account/settings/phone-number`, CacheIds.PhoneNumber),

            GetLocale: async () => this.client.get<LocaleDefinition>(`account/settings/locale`, CacheIds.Locale),
            getCompanyLogo: async () => this.client.get<string>(`account/settings/logo`, CacheIds.Logo),

            deleteCompanyLogo: async () => this.client.delete(`account/settings/logo`, CacheIds.Logo),
            DeleteAccount: async () => this.client.post(`account/delete-account`),
            CheckNeedsPassword: async () => this.client.get<boolean>(`account/needs-password`, CacheIds.NeedsPassword),
        },
        EmailVerification: {
            RequestEmailVerification: async (emailAddress: string, areaIdentifier: string) =>
                this.client.post<EmailVerificationResponse, {}>(`verification/email/${areaIdentifier}`, { EmailAddress: emailAddress }),
            CheckEmailVerificationStatus: async (emailAddress: string) => this.client.post<boolean, {}>(`verification/email/status`, { EmailAddress: emailAddress }),
        },
    };

    public Enquiries = {
        getEnquiries: async () => this.client.get<Enquiries>(`enquiries`, CacheIds.Enquiries),
        getShowSeenEnquiries: async () => this.client.get<boolean>(`enquiries/show`, CacheIds.ShowSeenQueries),
        toggleShowSeenEnquiries: async () => this.client.put<boolean, {}>(`enquiries/toggle-show`, CacheIds.ShowSeenQueries),

        updateEnquiry: async (conversationId: string) => this.client.put<Enquiries, {}>(`enquiries/update/${conversationId}`, CacheIds.Enquiries),
        deleteSelectedEnquiries: async (fileReferences: string[]) => this.client.put<Enquiries, {}>(`enquiries/selected`, { FileReferences: fileReferences }, CacheIds.Enquiries),

        getSignedUrl: async (fileId: string) => this.client.get<string>(`enquiries/link/${fileId}`),
        getConversation: async (conversationId: string) => this.client.get<CompletedConversation>(`enquiries/review/${conversationId}`, CacheIds.Conversation),
    };
}
