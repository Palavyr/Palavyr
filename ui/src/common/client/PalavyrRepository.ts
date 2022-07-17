import { isNullOrUndefinedOrWhitespace } from "@common/utils";
import {
    DynamicTableMetas,
    DynamicTableMeta,
    StaticTableMetas,
    StaticTableMetaTemplate,
    Intents,
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

class ApiRoutes {}

export class PalavyrRepository extends ApiRoutes {
    public Purchase = {
        Customer: {
            GetCustomerId: async () => this.client.get<string>(this.Routes.GetCustomerIdRoute(), CacheIds.CustomerId),
            GetCustomerPortal: async (customerId: string, returnUrl: string) => {
                SessionStorage.clearCacheValue(CacheIds.CurrentPlanMeta);
                return this.client.post<string, {}>(this.Routes.GetCustomerPortalRoute(), { CustomerId: customerId, ReturnUrl: returnUrl });
            },
        },
        Prices: {
            GetPrices: async (productId: string) => this.client.get<Prices>(this.Routes.GetPrices(productId)),
        },
        Checkout: {
            CreateCheckoutSession: async (priceId: string, cancelUrl: string, successUrl: string) =>
                this.client.post<string, {}>(this.Routes.CreateCheckoutSession(), {
                    PriceId: priceId,
                    CancelUrl: cancelUrl,
                    SuccessUrl: successUrl,
                }),
        },
    };

    public Products = {
        getProducts: async () => this.client.get<ProductIds>(this.Routes.GetProducts()),
    };

    public Intent = {
        GetAllIntents: async () => this.client.get<Intents>(this.Routes.GetAllIntents(), CacheIds.Intents),
        GetShowDynamicTotals: (intentId: string) => this.client.get<boolean>(this.Routes.GetShowDynamicTotals(intentId)),

        ToggleIsEnabled: async (intentToggleStateUpdate: boolean, intentId: string) => {
            const update = this.client.put<boolean, {}>(this.Routes.ToggleIsEnabled(), { IsEnabled: intentToggleStateUpdate, IntentId: intentId });
            SessionStorage.clearCacheValue(CacheIds.Intents);
            return update;
        },
        SetShowDynamicTotals: (intentId: string, shouldShow: boolean) => this.client.put<boolean, {}>(this.Routes.SetShowDynamicTotals(), { ShowDynamicTotals: shouldShow, IntentId: intentId }),
        ToggleUseAreaFallbackEmail: async (useAreaFallbackEmailUpdate: boolean, intentId: string) =>
            this.client.put<boolean, {}>(this.Routes.ToggleUseAreaFallbackEmail(), { UseFallback: useAreaFallbackEmailUpdate, IntentId: intentId }),
        CreateIntent: async (intentName: string) => {
            const newArea = await this.client.post<AreaTable, {}>(this.Routes.CreateIntent(), { intentName });
            const areas = SessionStorage.getCacheValue(CacheIds.Intents) as Intents;
            areas.push(newArea);
            SessionStorage.setCacheValue(CacheIds.Intents, areas);
            return newArea;
        },
        UpdateIntentName: (intentId: string, intentName: string) => {
            const result = this.client.put<string, {}>(this.Routes.UpdateIntentName(intentId), { intentName });
            SessionStorage.clearCacheValue(CacheIds.Intents);
            return result;
        },

        DeleteIntent: (intentId: string) => this.client.delete<void>(this.Routes.DeleteIntent(intentId), CacheIds.Intents),
        ToggleSendPdfResponse: (intentId: string) => this.client.post<boolean, {}>(this.Routes.ToggleSendPdfResponse(intentId)),
    };

    public Routes = {
        GetCustomerIdRoute: () => `payments/customer-id`,
        GetCustomerPortalRoute: () => `payments/customer-portal`,
        GetPrices: (prodId: string) => `products/prices/get-prices/${prodId}`,
        CreateCheckoutSession: () => `checkout/create-checkout-session`,
        GetProducts: () => `products/all`,
        ToggleIsEnabled: () => `intents/intent-toggle`,
        ToggleUseAreaFallbackEmail: () => `intents/use-fallback-email-toggle`,
        GetAllIntents: () => `intents`,
        CreateIntent: () => `intents/create`,
        UpdateIntentName: (intentId: string) => `intents/update/name/${intentId}`,
        DeleteIntent: (intentId: string) => `intents/delete/${intentId}`,
        ToggleSendPdfResponse: (intentId: string) => `intent/send-pdf/${intentId}`,
        GetShowDynamicTotals: (intentId: string) => `intent/dynamic-totals/${intentId}`,
        SetShowDynamicTotals: () => `area/dynamic-totals`,
        GetEstimateConfiguration: (intentId: string) => `response/configuration/${intentId}`,
        UpdatePrologue: () => `response/configuration/prologue`,
        UpdateEpilogue: () => `response/configuration/epilogue`,
        GetSupportedUnitIds: () => `configuration/unit-types`,
        GetWidgetState: () => `widget-config/widget-active-state`,
        SetWidgetState: (updatedWidgetState: boolean) => `widget-config/widget-active-state?state=${updatedWidgetState}`,
        GetDynamicTableMetas: (intentId: string) => `tables/dynamic/metas/${intentId}`,
        GetDynamicTableTypes: () => `tables/dynamic/table-name-map`,
        ModifyDynamicTableMeta: () => `tables/dynamic/modify`,
        CreateDynamicTable: (intentId: string) => `tables/dynamic/SimpleThresholdTableRow/create/${intentId}`,
        DeleteDynamicTable: (intentId: string, tableType: string, tableId: string) => `tables/dynamic/${tableType}/intent/${intentId}/table/${tableId}`,
        GetDynamicTableDataTemplate: (intentId: string, tableType: string, tableId: string) => `tables/dynamic/${tableType}/intent/${intentId}/table/${tableId}/template`,
        GetDynamicTableRows: (intentId: string, tableType: string, tableId: string) => `tables/dynamic/${tableType}/intent/${intentId}/table/${tableId}`,
        UpdateStaticTableMetas: () => `response/configuration/static/tables/save`,
        GetStaticTablesMetaTemplate: (intentId: string) => `response/configuration/${intentId}/static/tables/template`,
        GetStaticTableRowTemplate: (intentId: string, tableOrder: number) => `response/configuration/${intentId}/static/tables/${tableOrder}/row/template`,
        FetchPreview: (intentId: string) => `preview/estimate/${intentId}`



    };

    public Configuration = {
        getEstimateConfiguration: async (intentId: string) => this.client.get<ResponseConfigurationType>(this.Routes.GetEstimateConfiguration(intentId)),
        UpdatePrologue: async (intentId: string, prologue: string) => this.client.put<string, {}>(this.Routes.UpdatePrologue(), { prologue: prologue, IntentId: intentId }),
        UpdateEpilogue: async (intentId: string, epilogue: string) => this.client.put<string, {}>(this.Routes.UpdateEpilogue(), { epilogue: epilogue, IntentId: intentId }),
        Units: {
            GetSupportedUnitIds: async () => this.client.get<QuantUnitDefinition[]>(this.Routes.GetSupportedUnitIds()), //, CacheIds.SupportedUnitIds),
        },

        WidgetState: {
            GetWidgetState: async () => this.client.get<boolean>(this.Routes.GetWidgetState()),
            SetWidgetState: async (updatedWidgetState: boolean) => this.client.post<boolean, {}>(this.Routes.SetWidgetState(updatedWidgetState)),
        },
        Tables: {
            Dynamic: {
                GetDynamicTableMetas: async (intentId: string) => this.client.get<DynamicTableMetas>(this.Routes.GetDynamicTableMetas(intentId)), // todo - cache

                GetDynamicTableTypes: async () => this.client.get<TableNameMap>(this.Routes.GetDynamicTableTypes()),

                ModifyDynamicTableMeta: async (dynamicTableMeta: DynamicTableMeta) => {
                    console.log(dynamicTableMeta);
                    return this.client.put<DynamicTableMeta, {}>(this.Routes.ModifyDynamicTableMeta(), dynamicTableMeta);
                    // SessionStorage.clearCacheValue([CacheIds.PalavyrConfiguration, dynamicTableMeta.intentId].join("-"));
                    // return response;
                },

                CreateDynamicTable: async (intentId: string) => {
                    const response = this.client.post<DynamicTableMeta, {}>(this.Routes.CreateDynamicTable(intentId));
                    // SessionStorage.clearCacheValue([CacheIds.PalavyrConfiguration, intentId].join("-"));
                    return response;
                },

                DeleteDynamicTable: async (intentId: string, tableType: string, tableId: string) => {
                    const response = this.client.delete(this.Routes.DeleteDynamicTable(intentId, tableType, tableId));
                    SessionStorage.clearCacheValue([CacheIds.PalavyrConfiguration, intentId].join("-"));
                    return response;
                },
                GetDynamicTableDataTemplate: async <T>(intentId: string, tableType: string, tableId: string) => this.client.get<T>(this.Routes.GetDynamicTableDataTemplate(intentId, tableType, tableId)),
                GetDynamicTableRows: async (intentId: string, tableType: string, tableId: string) => this.client.get<DynamicTableData>(this.Routes.GetDynamicTableRows(intentId, tableType, tableId)),

                saveDynamicTable: async <T>(intentId: string, tableType: string, tableData: TableData, tableId: string, tableTag: string) => {
                    const response = this.client.put<T, {}>(`tables/dynamic/${tableType}/intent/${intentId}/table/${tableId}`, { TableTag: tableTag, TableData: tableData });
                    SessionStorage.clearCacheValue([CacheIds.PalavyrConfiguration, intentId].join("-"));
                    return response;
                },
            },
            Static: {
                UpdateStaticTablesMetas: async (intentId: string, staticTablesMetas: StaticTableMetas) =>
                    this.client.put<StaticTableMetas, {}>(this.Routes.UpdateStaticTableMetas(), { StaticTableMetaUpdate: staticTablesMetas, IntentId: intentId }),
                GetStaticTablesMetaTemplate: async (intentId: string) => this.client.get<StaticTableMetaTemplate>(this.Routes.GetStaticTablesMetaTemplate(intentId)),
                GetStaticTableRowTemplate: async (intentId: string, tableOrder: number) => this.client.get<StaticTableRow>(this.Routes.GetStaticTableRowTemplate(intentId, tableOrder)),
            },
        },

        Preview: {
            FetchPreview: async (intentId: string) => this.client.get<FileAssetResource>(this.Routes.FetchPreview(intentId)),
        },

        Email: {
            GetVariableDetails: async () => this.client.get<VariableDetail[]>(`email/variables`),

            GetAreaEmailTemplate: async (intentId: string) => this.client.get<string>(`email/${intentId}/email-template`),
            GetAreaFallbackEmailTemplate: async (intentId: string) => this.client.get<string>(`email/fallback/${intentId}/email-template`),
            GetDefaultFallbackEmailTemplate: async () => this.client.get<string>(`email/fallback/default-email-template`),

            SaveAreaEmailTemplate: async (intentId: string, EmailTemplate: string) => this.client.put<string, {}>(`email/email-template`, { EmailTemplate, IntentId: intentId }),
            SaveAreaFallbackEmailTemplate: async (intentId: string, EmailTemplate: string) => this.client.put<string, {}>(`email/fallback/email-template`, { EmailTemplate, IntentId: intentId }),
            SaveDefaultFallbackEmailTemplate: async (EmailTemplate: string) => this.client.put<string, {}>(`email/fallback/default-email-template`, { EmailTemplate }),

            GetAreaSubject: (intentId: string) => this.client.get<string>(`email/subject/${intentId}`),
            GetAreaFallbackSubject: (intentId: string) => this.client.get<string>(`email/fallback/subject/${intentId}`),
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
        GetConversation: async (intentId: string) => this.client.get<ConvoNode[]>(`configure-conversations/${intentId}`, [CacheIds.PalavyrConfiguration, intentId].join("-") as CacheIds),
        GetConversationNode: async (nodeId: string) => this.client.get<ConvoNode>(`configure-conversations/nodes/${nodeId}`),
        GetNodeOptionsList: async (intentId: string, planTypeMeta: PlanTypeMeta) =>
            filterNodeTypeOptionsOnSubscription(await this.client.get<NodeTypeOptions>(`configure-conversations/${intentId}/node-type-options`), planTypeMeta),
        GetIntroNodeOptionsList: async (introId: string) => this.client.get<NodeTypeOptions>(`configure-intro/${introId}/node-type-options`),

        GetErrors: async (intentId: string, nodeList: ConvoNode[]) => this.client.post<TreeErrors, {}>(`configure-conversations/tree-errors`, { Transactions: nodeList, IntentId: intentId }),
        GetIntroErrors: async (introId: string, nodeList: ConvoNode[]) => this.client.post<TreeErrors, {}>(`configure-conversations/intro/tree-errors`, { Transactions: nodeList, IntroId: introId }),

        ModifyConversation: async (nodelist: ConvoNode[], intentId: string) =>
            this.client.put<ConvoNode[], {}>(`configure-conversations`, { Transactions: nodelist, IntentId: intentId }, [CacheIds.PalavyrConfiguration, intentId].join("-") as CacheIds),
        ModifyConversationNode: async (nodeId: string, intentId: string, updatedNode: ConvoNode) =>
            this.client.put<ConvoNode[], {}>(`configure-conversations/${intentId}/nodes/${nodeId}`, updatedNode, [CacheIds.PalavyrConfiguration, intentId].join("-") as CacheIds),
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
}
