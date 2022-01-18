import { isNullOrUndefinedOrWhitespace } from "@common/utils";
import {
    DynamicTableMetas,
    DynamicTableMeta,
    StaticTableMetas,
    StaticTableMetaTemplate,
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
    ProductIds,
    TableData,
    TableNameMap,
    TreeErrors,
    StaticTableRow,
    PlanTypeMeta,
    DynamicTableData,
    EnquiryActivtyResource,
    LocaleResponse,
} from "@Palavyr-Types";
import { ApiErrors } from "frontend/dashboard/layouts/Errors/ApiErrors";
import { filterNodeTypeOptionsOnSubscription } from "frontend/dashboard/subscriptionFilters/filterConvoNodeTypes";
import { SessionStorage } from "@localStorage/sessionStorage";
import { AxiosClient, CacheIds } from "./FrontendAxiosClient";
import { getJwtTokenFromLocalStorage, getSessionIdFromLocalStorage } from "./clientUtils";
import { Loaders } from "./Loaders";

export class PalavyrRepository {
    private client: AxiosClient;

    private formDataHeaders: { [key: string]: string } = {
        Accept: "application/json",
        "Content-Type": "multipart/form-data",
    };

    constructor(apiErrors?: ApiErrors, loaders?: Loaders) {
        this.client = new AxiosClient(apiErrors, loaders, undefined, getSessionIdFromLocalStorage, getJwtTokenFromLocalStorage);
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
        UpdateIsEnabled: async (areaToggleStateUpdate: boolean, intentId: string) => {
            const update = this.client.put<boolean, {}>(`areas/area-toggle`, { IsEnabled: areaToggleStateUpdate, IntentId: intentId });
            SessionStorage.clearCacheValue(CacheIds.Areas);
            return update;
        },
        UpdateUseAreaFallbackEmail: async (useAreaFallbackEmailUpdate: boolean, intentId: string) =>
            this.client.put<boolean, {}>(`intents/use-fallback-email-toggle`, { UseFallback: useAreaFallbackEmailUpdate, IntentId: intentId }),
        GetAreas: async () => this.client.get<Areas>("areas", CacheIds.Areas),
        createArea: async (areaName: string) => {
            const newArea = await this.client.post<AreaTable, {}>(`intents/create`, { AreaName: areaName });
            const areas = SessionStorage.getCacheValue(CacheIds.Areas) as Areas;
            areas.push(newArea);
            SessionStorage.setCacheValue(CacheIds.Areas, areas);
            return newArea;
        },
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

        deleteArea: (intentId: string) => this.client.delete<void>(`intents/delete/${intentId}`, CacheIds.Areas),
        toggleSendPdfResponse: (areaIdentifier: string) => this.client.post<boolean, {}>(`area/send-pdf/${areaIdentifier}`),
        getShowDynamicTotals: (intentId: string) => this.client.get<boolean>(`area/dynamic-totals/${intentId}`),
        setShowDynamicTotals: (intentId: string, shouldShow: boolean) => this.client.put<boolean, {}>(`area/dynamic-totals`, { ShowDynamicTotals: shouldShow, IntentId: intentId }),
    };

    public Configuration = {
        getEstimateConfiguration: async (intentId: string) => this.client.get<ResponseConfigurationType>(`response/configuration/${intentId}`),
        updatePrologue: async (intentId: string, prologue: string) => this.client.put<string, {}>(`response/configuration/prologue`, { prologue: prologue, IntentId: intentId}),
        updateEpilogue: async (intentId: string, epilogue: string) => this.client.put<string, {}>(`response/configuration/epilogue`, { epilogue: epilogue, IntentId: intentId}),

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

                getDynamicTableRows: async (areaIdentifier: string, tableType: string, tableId: string) =>
                    this.client.get<DynamicTableData>(`tables/dynamic/${tableType}/area/${areaIdentifier}/table/${tableId}`),

                saveDynamicTable: async <T>(areaIdentifier: string, tableType: string, tableData: TableData, tableId: string, tableTag: string) => {
                    const response = this.client.put<T, {}>(`tables/dynamic/${tableType}/area/${areaIdentifier}/table/${tableId}`, { TableTag: tableTag, [tableType]: tableData });
                    SessionStorage.clearCacheValue([CacheIds.PalavyrConfiguration, areaIdentifier].join("-"));
                    return response;
                },
            },
            Static: {
                updateStaticTablesMetas: async (intentId: string, staticTablesMetas: StaticTableMetas) =>
                    this.client.put<StaticTableMetas, {}>(`response/configuration/static/tables/save`, {StaticTableMetaUpdate: staticTablesMetas, IntentId: intentId}),
                getStaticTablesMetaTemplate: async (intentId: string) => this.client.get<StaticTableMetaTemplate>(`response/configuration/${intentId}/static/tables/template`),
                getStaticTableRowTemplate: async (areaIdentifier: string, tableOrder: number) =>
                    this.client.get<StaticTableRow>(`response/configuration/${areaIdentifier}/static/tables/${tableOrder}/row/template`),
            },
        },

        Preview: {
            fetchPreview: async (intentId: string) => this.client.get<FileLink>(`preview/estimate/${intentId}`),
        },

        Email: {
            GetVariableDetails: async () => this.client.get<VariableDetail[]>(`email/variables`),

            GetAreaEmailTemplate: async (areaIdentifier: string) => this.client.get<string>(`email/${areaIdentifier}/email-template`),
            GetAreaFallbackEmailTemplate: async (areaIdentifier: string) => this.client.get<string>(`email/fallback/${areaIdentifier}/email-template`),
            GetDefaultFallbackEmailTemplate: async () => this.client.get<string>(`email/fallback/default-email-template`),

            SaveAreaEmailTemplate: async (intentId: string, EmailTemplate: string) => this.client.put<string, {}>(`email/email-template`, { EmailTemplate, IntentId: intentId }),
            SaveAreaFallbackEmailTemplate: async (intentId: string, EmailTemplate: string) => this.client.put<string, {}>(`email/fallback/email-template`, { EmailTemplate, IntentId: intentId }),
            SaveDefaultFallbackEmailTemplate: async (EmailTemplate: string) => this.client.put<string, {}>(`email/fallback/default-email-template`, { EmailTemplate }),

            GetAreaSubject: (intentId: string) => this.client.get<string>(`email/subject/${intentId}`),
            GetAreaFallbackSubject: (areaIdentifier: string) => this.client.get<string>(`email/fallback/subject/${areaIdentifier}`),
            GetDefaultFallbackSubject: async () => this.client.get<string>(`email/default-fallback-subject`),

            SaveAreaSubject: (intentId: string, subject: string) => this.client.put<string, {}>(`email/subject`, { Subject: subject, IntentId: intentId }),
            SaveAreaFallbackSubject: (intentId: string, subject: string) => this.client.put<string, {}>(`email/fallback/subject`, { Subject: subject, IntentId: intentId }),
            SaveDefaultFallbackSubject: async (subject: string) => this.client.put<string, {}>(`email/fallback/default-subject`, { Subject: subject }),
        },

        Attachments: {
            GetAttachmentLinks: async (intentId: string) => this.client.get<FileLink[]>(`attachments/${intentId}`, CacheIds.Attachments),
            removeAttachment: async (intentId: string, fileId: string) => this.client.delete<FileLink[]>(`attachments/file-link`, CacheIds.Attachments, { data: { fileId: fileId, IntentId: intentId } }),

            saveSingleAttachment: async (areaIdentifier: string, formData: FormData) =>
                this.client.post<FileLink[], {}>(`attachments/${areaIdentifier}/save-one`, formData, CacheIds.Attachments, {
                    headers: {
                        Accept: "application/json",
                        "Content-Type": "multipart/form-data",
                    },
                }),
            saveMultipleAttachments: async (intentId: string, formData: FormData) =>
                this.client.post<FileLink[], {}>(`attachments/${intentId}/save-many`, formData, CacheIds.Attachments, {
                    headers: {
                        Accept: "application/json",
                        "Content-Type": "multipart/form-data",
                    },
                }),
        },

        Images: {
            // Node Editor Flow
            saveSingleImage: async (formData: FormData) => {
                const result = await this.client.post<FileLink[], {}>(`images/save-one`, formData, undefined, { headers: this.formDataHeaders });
                const currentCache = SessionStorage.getCacheValue(CacheIds.Images) as FileLink[];
                currentCache.push(...result);
                SessionStorage.setCacheValue(CacheIds.Images, currentCache);
                return result;
            },
            // saveImageUrl: async (url: string, nodeId: string) => this.client.post<FileLink[], {}>(`images/use-link/`, { Url: url, NodeId: nodeId }),
            getImages: async (imageIds?: string[]) => {
                if (imageIds !== undefined && imageIds.length > 0) {
                    // if specifying 1 image
                    const currentCache = SessionStorage.getCacheValue(CacheIds.Images);
                    if (currentCache === null) {
                        return this.client.get<FileLink[]>(`images`, CacheIds.Images);
                    }
                    const availableImages = currentCache.filter((x: FileLink) => imageIds.includes(x.fileId)) as FileLink[];
                    if (availableImages.length === imageIds.length) {
                        return Promise.resolve(availableImages);
                    } else {
                        return this.client.get<FileLink[]>(`images?imageIds=${imageIds.join(",")}`);
                    }
                } else {
                    return this.client.get<FileLink[]>(`images`, CacheIds.Images);
                }
            }, // takes a querystring comma delimieted of imageIds
            savePreExistingImage: async (imageId: string, nodeId: string) => this.client.post<ConvoNode, {}>(`images/pre-existing/${imageId}/${nodeId}`),

            // DO NOT USE WITH NODE
            saveMultipleImages: async (formData: FormData) => {
                const result = await this.client.post<FileLink[], {}>(`images/save-many`, formData, CacheIds.Images, { headers: this.formDataHeaders });
                const currentCache = SessionStorage.getCacheValue(CacheIds.Images) as FileLink[];
                currentCache.push(...result);
                SessionStorage.setCacheValue(CacheIds.Images, currentCache);
                return result;
            },
            deleteImage: async (imageIds: string[]) => this.client.delete<FileLink[]>(`images?imageIds=${imageIds.join(",")}`, CacheIds.Images), // takes a querystring command delimited of imageIds
            getSignedUrl: async (s3Key: string, fileId: string) => this.client.post<string, {}>(`images/link`, { s3Key: s3Key }),
        },
    };

    public Conversations = {
        GetConversation: async (areaIdentifier: string) => this.client.get<ConvoNode[]>(`configure-conversations/${areaIdentifier}`, [CacheIds.PalavyrConfiguration, areaIdentifier].join("-") as CacheIds),
        GetConversationNode: async (nodeId: string) => this.client.get<ConvoNode>(`configure-conversations/nodes/${nodeId}`),
        GetNodeOptionsList: async (areaIdentifier: string, planTypeMeta: PlanTypeMeta) =>
            filterNodeTypeOptionsOnSubscription(await this.client.get<NodeTypeOptions>(`configure-conversations/${areaIdentifier}/node-type-options`), planTypeMeta),
        GetIntroNodeOptionsList: async () => this.client.get<NodeTypeOptions>(`configure-intro/{introId}/node-type-options`),

        GetErrors: async (intentId: string, nodeList: ConvoNode[]) => this.client.post<TreeErrors, {}>(`configure-conversations/tree-errors`, { Transactions: nodeList, IntentId: intentId }),
        GetIntroErrors: async (introId: string, nodeList: ConvoNode[]) => this.client.post<TreeErrors, {}>(`configure-conversations/intro/tree-errors`, { Transactions: nodeList, IntroId: introId }),

        ModifyConversation: async (nodelist: ConvoNode[], intentId: string) =>
            this.client.put<ConvoNode[], {}>(`configure-conversations`, { Transactions: nodelist, IntentId: intentId }, [CacheIds.PalavyrConfiguration, intentId].join("-") as CacheIds),
        ModifyConversationNode: async (nodeId: string, areaIdentifier: string, updatedNode: ConvoNode) =>
            this.client.put<ConvoNode[], {}>(`configure-conversations/${areaIdentifier}/nodes/${nodeId}`, updatedNode, [CacheIds.PalavyrConfiguration, areaIdentifier].join("-") as CacheIds),
        ModifyConversationNodeText: async (nodeId: string, intentId: string, updatedNodeText: string) => {
            const result = await this.client.put<ConvoNode | null, {}>(`configure-conversations/nodes/text`, { UpdatedNodeText: updatedNodeText, IntentId: intentId, NodeId: nodeId });
            SessionStorage.clearCacheValue([CacheIds.PalavyrConfiguration, intentId].join("-"));
            if (isNullOrUndefinedOrWhitespace(result)) {
                return Promise.resolve(null);
            }
            return Promise.resolve(result);
        },

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
            checkIsActive: async () => await this.client.get<boolean>(`account/is-active`),

            UpdatePassword: async (oldPassword: string, newPassword: string) => this.client.put<boolean, {}>(`account/settings/password`, { OldPassword: oldPassword, Password: newPassword }),
            updateCompanyName: async (companyName: string) => this.client.put<string, {}>(`account/settings/company-name`, { CompanyName: companyName }),
            updateEmail: async (newEmail: string) => this.client.put<EmailVerificationResponse, {}>(`account/settings/email`, { EmailAddress: newEmail }),
            updatePhoneNumber: async (newPhoneNumber: string) => this.client.put<string, {}>(`account/settings/phone-number`, { PhoneNumber: newPhoneNumber }),
            updateLocale: async (newLocaleId: string) => this.client.put<LocaleResponse, {}>(`account/settings/locale`, { LocaleId: newLocaleId }),
            updateCompanyLogo: async (formData: FormData) =>
                this.client.put<string, {}>(`account/settings/logo`, formData, undefined, {
                    headers: {
                        Accept: "application/json",
                        "Content-Type": "multipart/form-data",
                    },
                }),

            getCompanyName: async () => this.client.get<string>(`account/settings/company-name`),
            getEmail: async () => this.client.get<AccountEmailSettingsResponse>(`account/settings/email`),
            getPhoneNumber: async () => this.client.get<PhoneSettingsResponse>(`account/settings/phone-number`),

            GetLocale: async () => this.client.get<LocaleResponse>(`account/settings/locale`),
            getCompanyLogo: async () => this.client.get<string>(`account/settings/logo`),

            getIntroductionId: async () => this.client.get<string>(`account/settings/intro-id`),
            updateIntroduction: async (introId: string, update: ConvoNode[]) =>
                this.client.post<ConvoNode[], {}>(`account/settings/intro-id`, { Transactions: update }, [CacheIds.PalavyrConfiguration, introId].join("-") as CacheIds),

            deleteCompanyLogo: async () => this.client.delete(`account/settings/logo`),
            DeleteAccount: async () => {
                const result = this.client.post(`account/delete-account`);
                SessionStorage.ClearAllCacheValues();
                return result;
            },
            CheckNeedsPassword: async () => this.client.get<boolean>(`account/needs-password`),
        },
        EmailVerification: {
            RequestEmailVerification: async (emailAddress: string, intentId: string) =>
                this.client.post<EmailVerificationResponse, {}>(`verification/email`, { EmailAddress: emailAddress, IntentId: intentId}),
            CheckEmailVerificationStatus: async (emailAddress: string) => this.client.post<boolean, {}>(`verification/email/status`, { EmailAddress: emailAddress }),
        },
    };

    public Enquiries = {
        getEnquiries: async () => this.client.get<Enquiries>(`enquiries`),
        getShowSeenEnquiries: async () => this.client.get<boolean>(`enquiries/show`),
        toggleShowSeenEnquiries: async () => this.client.put<boolean, {}>(`enquiries/toggle-show`),

        updateEnquiry: async (conversationId: string) => {
            const result = this.client.put<Enquiries, {}>(`enquiries/update/${conversationId}`);
            return result;
        },
        deleteSelectedEnquiries: async (fileReferences: string[]) => {
            const result = this.client.put<Enquiries, {}>(`enquiries/selected`, { FileReferences: fileReferences });
            return result;
        },

        getSignedUrl: async (fileId: string) => this.client.get<string>(`enquiries/link/${fileId}`),
        getConversation: async (conversationId: string) => this.client.get<CompletedConversation>(`enquiries/review/${conversationId}`, [CacheIds.Conversation, conversationId].join("-") as CacheIds),

        getEnquiryInsights: async () => this.client.get<EnquiryActivtyResource[]>("enquiry-insights"),

        UnselectAll: async () => this.client.post(`enquiries/selectall`),
        SelectAll: async () => this.client.post(`enquiries/unselectall`),
    };
}
