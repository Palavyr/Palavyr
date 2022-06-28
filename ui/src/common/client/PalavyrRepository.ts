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
    QuantUnitDefinition,
    FileAssetResource,
    MarkAsSeenUpdate,
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
        updatePrologue: async (intentId: string, prologue: string) => this.client.put<string, {}>(`response/configuration/prologue`, { prologue: prologue, IntentId: intentId }),
        updateEpilogue: async (intentId: string, epilogue: string) => this.client.put<string, {}>(`response/configuration/epilogue`, { epilogue: epilogue, IntentId: intentId }),

        Units: {
            GetSupportedUnitIds: async () => this.client.get<QuantUnitDefinition[]>(`configuration/unit-types`), //, CacheIds.SupportedUnitIds),
        },

        WidgetState: {
            GetWidgetState: async () => this.client.get<boolean>(`widget-config/widget-active-state`),
            SetWidgetState: async (updatedWidgetState: boolean) => this.client.post<boolean, {}>(`widget-config/widget-active-state?state=${updatedWidgetState}`),
        },
        Tables: {
            Dynamic: {
                getDynamicTableMetas: async (areaIdentifier: string) => this.client.get<DynamicTableMetas>(`tables/dynamic/metas/${areaIdentifier}`), // todo - cache

                getDynamicTableTypes: async () => this.client.get<TableNameMap>(`tables/dynamic/table-name-map`),

                modifyDynamicTableMeta: async (dynamicTableMeta: DynamicTableMeta) => {
                    console.log(dynamicTableMeta);
                    return this.client.put<DynamicTableMeta, {}>(`tables/dynamic/modify`, dynamicTableMeta);
                    // SessionStorage.clearCacheValue([CacheIds.PalavyrConfiguration, dynamicTableMeta.areaIdentifier].join("-"));
                    // return response;
                },

                createDynamicTable: async (intentId: string) => {
                    const response = this.client.post<DynamicTableMeta, {}>(`tables/dynamic/${intentId}`);
                    // SessionStorage.clearCacheValue([CacheIds.PalavyrConfiguration, intentId].join("-"));
                    return response;
                },

                deleteDynamicTable: async (intent: string, tableType: string, tableId: string) => {
                    const response = this.client.delete(`tables/dynamic/${tableType}/intent/${intent}/table/${tableId}`);
                    SessionStorage.clearCacheValue([CacheIds.PalavyrConfiguration, intent].join("-"));
                    return response;
                },

                getDynamicTableDataTemplate: async <T>(intentId: string, tableType: string, tableId: string) =>
                    this.client.get<T>(`tables/dynamic/${tableType}/intent/${intentId}/table/${tableId}/template`),

                getDynamicTableRows: async (intentId: string, tableType: string, tableId: string) =>
                    this.client.get<DynamicTableData>(`tables/dynamic/${tableType}/intent/${intentId}/table/${tableId}`),

                saveDynamicTable: async <T>(intentId: string, tableType: string, tableData: TableData, tableId: string, tableTag: string) => {
                    const response = this.client.put<T, {}>(`tables/dynamic/${tableType}/intent/${intentId}/table/${tableId}`, { TableTag: tableTag, TableData: tableData });
                    SessionStorage.clearCacheValue([CacheIds.PalavyrConfiguration, intentId].join("-"));
                    return response;
                },
            },
            Static: {
                updateStaticTablesMetas: async (intentId: string, staticTablesMetas: StaticTableMetas) =>
                    this.client.put<StaticTableMetas, {}>(`response/configuration/static/tables/save`, { StaticTableMetaUpdate: staticTablesMetas, IntentId: intentId }),
                getStaticTablesMetaTemplate: async (intentId: string) => this.client.get<StaticTableMetaTemplate>(`response/configuration/${intentId}/static/tables/template`),
                getStaticTableRowTemplate: async (areaIdentifier: string, tableOrder: number) =>
                    this.client.get<StaticTableRow>(`response/configuration/${areaIdentifier}/static/tables/${tableOrder}/row/template`),
            },
        },

        Preview: {
            FetchPreview: async (intentId: string) => this.client.get<FileAssetResource>(`preview/estimate/${intentId}`),
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
            GetAttachments: async (intentId: string) => this.client.get<FileAssetResource[]>(`attachments/${intentId}`, undefined),
            DeleteAttachment: async (intentId: string, fileId: string) => this.client.delete<FileAssetResource[]>(`attachments`, undefined, { data: { fileId: fileId, IntentId: intentId } }),
            UploadAttachments: async (intentId: string, formData: FormData) =>
                this.client.post<FileAssetResource[], {}>(`attachments/${intentId}/upload`, formData, undefined, {
                    headers: {
                        Accept: "application/json",
                        "Content-Type": "multipart/form-data",
                    },
                }),
        },

        FileAssets: {
            GetFileAssets: async (fileIds: string[] = []) => {
                if (fileIds !== undefined && fileIds.length > 0) {
                    // if specifying 1 image
                    // const currentCache = SessionStorage.getCacheValue(CacheIds.FileAssets);
                    // if (currentCache === null) {
                    //     return this.client.get<FileAssetResource[]>(`file-assets`, CacheIds.FileAssets);
                    // }
                    // const availableImages = currentCache.filter((x: FileAssetResource) => fileIds.includes(x.fileId)) as FileAssetResource[];
                    // if (availableImages.length === fileIds.length) {
                    //     return Promise.resolve(availableImages);
                    // } else {
                    return await this.client.get<FileAssetResource[]>(`file-assets?fileIds=${fileIds.join(",")}`);
                    // }
                } else {
                    return await this.client.get<FileAssetResource[]>(`file-assets`); //, CacheIds.FileAssets);
                }
            }, // takes a querystring comma delimieted of fileIds

            // DO NOT USE WITH NODE
            UploadFileAssets: async (formData: FormData) => {
                const result = await this.client.post<FileAssetResource[], {}>(`file-assets/upload`, formData, undefined, { headers: this.formDataHeaders });
                // const currentCache = SessionStorage.getCacheValue(CacheIds.FileAssets) as FileAssetResource[];
                // currentCache.push(...result);
                // SessionStorage.setCacheValue(CacheIds.FileAssets, currentCache);
                return result;
            },

            LinkFileAssetToNode: async (fileId: string, nodeId: string) => this.client.post<ConvoNode, {}>(`file-assets/link/${fileId}/node/${nodeId}`),
            LinkFileAssetToIntent: async (fileId: string, intentId: string) => this.client.post<FileAssetResource, {}>(`file-assets/link/${fileId}/intent/${intentId}`),
            LinkFileAssetToLogo: async (fileId: string) => this.client.post<FileAssetResource, {}>(`file-assets/link/${fileId}/logo`),
            DeleteFileAsset: async (fileIds: string[]) => this.client.delete<FileAssetResource[]>(`file-assets?fileIds=${fileIds.join(",")}`), // CacheIds.FileAssets), // takes a querystring command delimited of fileIds
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
            CancelRegistration: async (emailAddress: string) => this.client.post<{}, {}>("account/cancel-registration", { EmailAddress: emailAddress }),

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
                this.client.put<FileAssetResource, {}>(`account/settings/logo`, formData, undefined, {
                    headers: {
                        Accept: "application/json",
                        "Content-Type": "multipart/form-data",
                    },
                }),

            getCompanyName: async () => this.client.get<string>(`account/settings/company-name`),
            getEmail: async () => this.client.get<AccountEmailSettingsResponse>(`account/settings/email`),
            getPhoneNumber: async () => this.client.get<PhoneSettingsResponse>(`account/settings/phone-number`),

            GetLocale: async (readonly: boolean = false) => this.client.get<LocaleResponse>(`account/settings/locale?read=${readonly}`),
            getCompanyLogo: async () => this.client.get<FileAssetResource>(`account/settings/logo`),

            getIntroductionId: async () => this.client.get<string>(`account/settings/intro-id`),
            updateIntroduction: async (introId: string, update: ConvoNode[]) =>
                this.client.post<ConvoNode[], {}>(`account/settings/intro-id`, { Transactions: update }, [CacheIds.PalavyrConfiguration, introId].join("-") as CacheIds),

            deleteCompanyLogo: async () => this.client.delete(`file-assets/unlink/logo`),
            DeleteAccount: async () => {
                const result = this.client.delete(`account/delete-account`);
                SessionStorage.ClearAllCacheValues();
                return result;
            },
        },
        EmailVerification: {
            RequestEmailVerification: async (emailAddress: string, intentId: string) => this.client.post<EmailVerificationResponse, {}>(`verification/email`, { EmailAddress: emailAddress, IntentId: intentId }),
            CheckEmailVerificationStatus: async (emailAddress: string) => this.client.post<boolean, {}>(`verification/email/status`, { EmailAddress: emailAddress }),
        },
    };

    public Enquiries = {
        getEnquiries: async () => this.client.get<Enquiries>(`enquiries`),
        getEnquiryCount: async () => this.client.get<number>(`enquiries/count`),
        getShowSeenEnquiries: async () => this.client.get<boolean>(`enquiries/show`),
        toggleShowSeenEnquiries: async () => this.client.put<boolean, {}>(`enquiries/toggle-show`),

        UpdateSeen: async (updates: MarkAsSeenUpdate[]) => this.client.put<{}, {}>(`enquiries/seen`, { Updates: updates }),
        DeleteSelected: async (conversationIds: string[]) => this.client.put<Enquiries, {}>(`enquiries/delete`, { ConversationIds: conversationIds }),

        getConversation: async (conversationId: string) => this.client.get<CompletedConversation>(`enquiries/review/${conversationId}`, [CacheIds.Conversation, conversationId].join("-") as CacheIds),

        getEnquiryInsights: async () => this.client.get<EnquiryActivtyResource[]>("enquiry-insights"),

        UnselectAll: async () => this.client.post(`enquiries/selectall`),
        SelectAll: async () => this.client.post(`enquiries/unselectall`),
    };
}
