import { isNullOrUndefinedOrWhitespace } from "@common/utils";
import {
    PricingStrategyMetas,
    PricingStrategyMeta,
    StaticTableMetas,
    StaticTableMetaTemplate,
    Intents,
    PriceResources,
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
    PricingStrategyData,
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
import { ApiRoutes } from "./ApiRoutes";

export class PalavyrRepository extends ApiRoutes {
    private client: AxiosClient;

    private formDataHeaders: { [key: string]: string } = {
        Accept: "application/json",
        "Content-Type": "multipart/form-data",
    };

    constructor(apiErrors?: ApiErrors, loaders?: Loaders) {
        super();
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
            GetCustomerId: async () => this.client.get<string>(this.Routes.GetCustomerIdRoute(), CacheIds.CustomerId),
            GetCustomerPortal: async (customerId: string, returnUrl: string) => {
                SessionStorage.clearCacheValue(CacheIds.CurrentPlanMeta);
                return this.client.post<string, {}>(this.Routes.GetCustomerPortalRoute(), { CustomerId: customerId, ReturnUrl: returnUrl });
            },
        },
        Prices: {
            GetPrices: async (productId: string) => this.client.get<PriceResources>(this.Routes.GetPrices(productId)),
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
        GetProducts: async () => this.client.get<ProductIds>(this.Routes.GetProducts()),
    };

    public Intent = {
        GetAllIntents: async () => this.client.get<Intents>(this.Routes.GetAllIntents(), CacheIds.Intents),
        GetShowDynamicTotals: (intentId: string) => this.client.get<boolean>(this.Routes.GetShowPricingStrategyTotals(intentId)),

        ToggleIsEnabled: async (intentToggleStateUpdate: boolean, intentId: string) => {
            const update = this.client.put<boolean, {}>(this.Routes.ToggleIsEnabled(), { IsEnabled: intentToggleStateUpdate, IntentId: intentId });
            SessionStorage.clearCacheValue(CacheIds.Intents);
            return update;
        },
        SetShowDynamicTotals: (intentId: string, shouldShow: boolean) => this.client.put<boolean, {}>(this.Routes.SetShowricingStrategyTotals(), { ShowDynamicTotals: shouldShow, IntentId: intentId }),
        ToggleUseAreaFallbackEmail: async (useAreaFallbackEmailUpdate: boolean, intentId: string) =>
            this.client.put<boolean, {}>(this.Routes.ToggleUseIntentFallbackEmail(), { UseFallback: useAreaFallbackEmailUpdate, IntentId: intentId }),
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
                GetPricingStrategyMetas: async (intentId: string) => this.client.get<PricingStrategyMetas>(this.Routes.GetPricingStrategyMetas(intentId)), // todo - cache

                GetPricingStrategyTypes: async () => this.client.get<TableNameMap>(this.Routes.GetPricingStrategyTypes()),

                ModifyPricingStrategyMeta: async (PricingStrategyMeta: PricingStrategyMeta) => {
                    return this.client.put<PricingStrategyMeta, {}>(this.Routes.ModifyPricingStrategyMeta(), PricingStrategyMeta);
                    // SessionStorage.clearCacheValue([CacheIds.PalavyrConfiguration, PricingStrategyMeta.intentId].join("-"));
                    // return response;
                },

                CreatePricingStrategy: async (intentId: string) => {
                    const response = this.client.post<PricingStrategyMeta, {}>(this.Routes.CreatePricingStrategy(intentId));
                    // SessionStorage.clearCacheValue([CacheIds.PalavyrConfiguration, intentId].join("-"));
                    return response;
                },

                DeletePricingStrategy: async (intentId: string, tableType: string, tableId: string) => {
                    const response = this.client.delete(this.Routes.DeletePricingStrategy(intentId, tableType, tableId));
                    SessionStorage.clearCacheValue([CacheIds.PalavyrConfiguration, intentId].join("-"));
                    return response;
                },
                GetPricingStrategyDataTemplate: async <T>(intentId: string, tableType: string, tableId: string) => this.client.get<T>(this.Routes.GetPricingStrategyDataTemplate(intentId, tableType, tableId)),
                GetPricingStrategyRows: async (intentId: string, tableType: string, tableId: string) => this.client.get<PricingStrategyData>(this.Routes.GetPricingStrategyRows(intentId, tableType, tableId)),

                SavePricingStrategy: async <T>(intentId: string, tableType: string, tableData: TableData, tableId: string, tableTag: string) => {
                    const response = this.client.put<T, {}>(this.Routes.SavePricingStrategy(intentId, tableType, tableId), { TableTag: tableTag, TableData: tableData });
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
            GetVariableDetails: async () => this.client.get<VariableDetail[]>(this.Routes.GetVariableDetails()),

            GetAreaEmailTemplate: async (intentId: string) => this.client.get<string>(this.Routes.GetIntentEmailTemplate(intentId)),
            GetAreaFallbackEmailTemplate: async (intentId: string) => this.client.get<string>(this.Routes.GetIntentFallbackEmailTemplate(intentId)),
            GetDefaultFallbackEmailTemplate: async () => this.client.get<string>(this.Routes.GetDefaultFallbackEmailTemplate()),

            SaveAreaEmailTemplate: async (intentId: string, EmailTemplate: string) => this.client.put<string, {}>(this.Routes.SaveIntentEmailTemplate(), { EmailTemplate, IntentId: intentId }),
            SaveAreaFallbackEmailTemplate: async (intentId: string, EmailTemplate: string) => this.client.put<string, {}>(this.Routes.SaveIntentFallbackEmailTemplate(), { EmailTemplate, IntentId: intentId }),
            SaveDefaultFallbackEmailTemplate: async (EmailTemplate: string) => this.client.put<string, {}>(this.Routes.SaveDefaultFallbackEmailTemplate(), { EmailTemplate }),

            GetAreaSubject: (intentId: string) => this.client.get<string>(this.Routes.GetIntentSubject(intentId)),
            GetAreaFallbackSubject: (intentId: string) => this.client.get<string>(this.Routes.GetIntentFallbackSubject(intentId)),
            GetDefaultFallbackSubject: async () => this.client.get<string>(this.Routes.GetDefaultFallbackSubject()),

            SaveAreaSubject: async (intentId: string, subject: string) => this.client.put<string, {}>(this.Routes.SaveIntentSubject(), { Subject: subject, IntentId: intentId }),
            SaveAreaFallbackSubject: async (intentId: string, subject: string) => this.client.put<string, {}>(this.Routes.SaveIntentFallbackSubject(), { Subject: subject, IntentId: intentId }),
            SaveDefaultFallbackSubject: async (subject: string) => this.client.put<string, {}>(this.Routes.SaveDefaultFallbackSubject(), { Subject: subject }),
        },

        Attachments: {
            GetAttachments: async (intentId: string) => this.client.get<FileAssetResource[]>(this.Routes.GetAttachments(intentId), undefined),
            DeleteAttachment: async (intentId: string, fileId: string) => this.client.delete<FileAssetResource[]>(this.Routes.DeleteAttachment(), undefined, { data: { fileId: fileId, IntentId: intentId } }),
            UploadAttachments: async (intentId: string, formData: FormData) =>
                this.client.post<FileAssetResource[], {}>(this.Routes.UploadAttachments(intentId), formData, undefined, {
                    headers: {
                        Accept: "application/json",
                        "Content-Type": "multipart/form-data",
                    },
                }),
        },

        FileAssets: {
            GetFileAssets: async (fileIds: string[] = []) => this.client.get<FileAssetResource[]>(this.Routes.GetFileAssets(fileIds)),
            // DO NOT USE WITH NODE
            UploadFileAssets: async (formData: FormData) => {
                const result = await this.client.post<FileAssetResource[], {}>(this.Routes.UploadFileAssets(), formData, undefined, { headers: this.formDataHeaders });
                // const currentCache = SessionStorage.getCacheValue(CacheIds.FileAssets) as FileAssetResource[];
                // currentCache.push(...result);
                // SessionStorage.setCacheValue(CacheIds.FileAssets, currentCache);
                return result;
            },

            LinkFileAssetToNode: async (fileId: string, nodeId: string) => this.client.post<ConvoNode, {}>(this.Routes.LinkFileAssetToNode(fileId, nodeId)),
            LinkFileAssetToIntent: async (fileId: string, intentId: string) => this.client.post<FileAssetResource, {}>(this.Routes.LinkFileAssetToIntent(fileId, intentId)),
            LinkFileAssetToLogo: async (fileId: string) => this.client.post<FileAssetResource, {}>(this.Routes.LinkFileAssetToLogo(fileId)),
            DeleteFileAsset: async (fileIds: string[]) => this.client.delete<FileAssetResource[]>(this.Routes.DeleteFileAsset(fileIds)), // CacheIds.FileAssets), // takes a querystring command delimited of fileIds
        },
    };

    public Conversations = {
        GetConversation: async (intentId: string) => this.client.get<ConvoNode[]>(this.Routes.GetConversation(intentId), [CacheIds.PalavyrConfiguration, intentId].join("-") as CacheIds),
        GetConversationNode: async (nodeId: string) => this.client.get<ConvoNode>(this.Routes.GetConversationNode(nodeId)),
        GetNodeOptionsList: async (intentId: string, planTypeMeta: PlanTypeMeta) =>
            filterNodeTypeOptionsOnSubscription(await this.client.get<NodeTypeOptions>(this.Routes.GetNodeOptionsList(intentId)), planTypeMeta),
        GetIntroNodeOptionsList: async (introId: string) => this.client.get<NodeTypeOptions>(this.Routes.GetIntroNodeOptionsList(introId)),

        GetErrors: async (intentId: string, nodeList: ConvoNode[]) => this.client.post<TreeErrors, {}>(this.Routes.GetErrors(), { Transactions: nodeList, IntentId: intentId }),
        GetIntroErrors: async (introId: string, nodeList: ConvoNode[]) => this.client.post<TreeErrors, {}>(this.Routes.GetIntroErrors(), { Transactions: nodeList, IntroId: introId }),

        ModifyConversation: async (nodelist: ConvoNode[], intentId: string) =>
            this.client.put<ConvoNode[], {}>(this.Routes.ModifyConversation(), { Transactions: nodelist, IntentId: intentId }, [CacheIds.PalavyrConfiguration, intentId].join("-") as CacheIds),
        ModifyConversationNode: async (nodeId: string, intentId: string, updatedNode: ConvoNode) =>
            this.client.put<ConvoNode[], {}>(this.Routes.ModifyConversationNode(nodeId, intentId), updatedNode, [CacheIds.PalavyrConfiguration, intentId].join("-") as CacheIds),
        ModifyConversationNodeText: async (nodeId: string, intentId: string, updatedNodeText: string) => {
            const result = await this.client.put<ConvoNode | null, {}>(this.Routes.ModifyConversationNodeText(), { UpdatedNodeText: updatedNodeText, IntentId: intentId, NodeId: nodeId });
            SessionStorage.clearCacheValue([CacheIds.PalavyrConfiguration, intentId].join("-"));
            if (isNullOrUndefinedOrWhitespace(result)) {
                return Promise.resolve(null);
            }
            return Promise.resolve(result);
        },
    };
    public WidgetDemo = {
        RunConversationPrecheck: async () => this.client.get<PreCheckResult>(this.Routes.RunConversationPrecheck()),
        GetWidetPreferences: async () => this.client.get<WidgetPreferences>(this.Routes.GetWidetPreferences(), CacheIds.WidgetPrefs),
        SaveWidgetPreferences: async (prefs: WidgetPreferences) => this.client.put<WidgetPreferences, WidgetPreferences>(this.Routes.SaveWidgetPreferences(), prefs, CacheIds.WidgetPrefs),
    };

    public Settings = {
        Subscriptions: {
            GetCurrentPlanMeta: async () => this.client.get<PlanTypeMeta>(this.Routes.GetCurrentPlanMeta(), CacheIds.CurrentPlanMeta),
        },

        Account: {
            CancelRegistration: async (emailAddress: string) => this.client.post<{}, {}>(this.Routes.CancelRegistration(), { EmailAddress: emailAddress }),

            GetApiKey: async () => this.client.get<string>(this.Routes.GetApiKey()),
            ConfirmEmailAddress: async (authToken: string) => this.client.post<boolean, {}>(this.Routes.ConfirmEmailAddress(authToken)),
            ResendConfirmationToken: async (emailAddress: string) => this.client.post<boolean, {}>(this.Routes.ResendConfirmationToken(), { EmailAddress: emailAddress }),
            CheckIsActive: async () => await this.client.get<boolean>(this.Routes.CheckIsActive()),

            UpdatePassword: async (oldPassword: string, newPassword: string) => this.client.put<boolean, {}>(this.Routes.UpdatePassword(), { OldPassword: oldPassword, Password: newPassword }),
            UpdateCompanyName: async (companyName: string) => this.client.put<string, {}>(this.Routes.UpdateCompanyName(), { CompanyName: companyName }),
            UpdateEmail: async (newEmail: string) => this.client.put<EmailVerificationResponse, {}>(this.Routes.UpdateEmail(), { EmailAddress: newEmail }),
            UpdatePhoneNumber: async (newPhoneNumber: string) => this.client.put<string, {}>(this.Routes.UpdatePhoneNumber(), { PhoneNumber: newPhoneNumber }),
            UpdateLocale: async (newLocaleId: string) => this.client.put<LocaleResponse, {}>(this.Routes.UpdateLocale(), { LocaleId: newLocaleId }),
            UpdateCompanyLogo: async (formData: FormData) =>
                this.client.put<FileAssetResource, {}>(this.Routes.UpdateCompanyLogo(), formData, undefined, {
                    headers: {
                        Accept: "application/json",
                        "Content-Type": "multipart/form-data",
                    },
                }),

            GetCompanyName: async () => this.client.get<string>(this.Routes.GetCompanyName()),
            GetEmail: async () => this.client.get<AccountEmailSettingsResponse>(this.Routes.GetEmail()),
            GetPhoneNumber: async () => this.client.get<PhoneSettingsResponse>(this.Routes.GetPhoneNumber()),

            GetLocale: async (readonly: boolean = false) => this.client.get<LocaleResponse>(this.Routes.GetLocale()),
            GetCompanyLogo: async () => this.client.get<FileAssetResource>(this.Routes.GetCompanyLogo()),

            GetIntroductionId: async () => this.client.get<string>(this.Routes.GetIntroductionId()),
            UpdateIntroduction: async (introId: string, update: ConvoNode[]) =>
                this.client.post<ConvoNode[], {}>(this.Routes.UpdateIntroduction(), { Transactions: update }, [CacheIds.PalavyrConfiguration, introId].join("-") as CacheIds),

            DeleteCompanyLogo: async () => this.client.delete(this.Routes.DeleteCompanyLogo()),
            DeleteAccount: async () => {
                const result = this.client.delete(this.Routes.DeleteAccount());
                SessionStorage.ClearAllCacheValues();
                return result;
            },
        },
        EmailVerification: {
            RequestEmailVerification: async (emailAddress: string, intentId: string) =>
                this.client.post<EmailVerificationResponse, {}>(this.Routes.RequestEmailVerification(), { EmailAddress: emailAddress, IntentId: intentId }),
            CheckEmailVerificationStatus: async (emailAddress: string) => this.client.post<boolean, {}>(this.Routes.CheckEmailVerificationStatus(), { EmailAddress: emailAddress }),
        },
    };

    public Enquiries = {
        GetEnquiries: async () => this.client.get<Enquiries>(this.Routes.GetEnquiries()),
        GetEnquiryCount: async () => this.client.get<number>(this.Routes.GetEnquiryCount()),
        GetShowSeenEnquiries: async () => this.client.get<boolean>(this.Routes.GetShowSeenEnquiries()),
        ToggleShowSeenEnquiries: async () => this.client.put<boolean, {}>(this.Routes.ToggleShowSeenEnquiries()),

        UpdateSeen: async (updates: MarkAsSeenUpdate[]) => this.client.put<{}, {}>(this.Routes.UpdateSeen(), { Updates: updates }),
        DeleteSelected: async (conversationIds: string[]) => this.client.put<Enquiries, {}>(this.Routes.DeleteSelected(), { ConversationIds: conversationIds }),

        GetConversation: async (conversationId: string) => this.client.get<CompletedConversation>(this.Routes.GetConversation(conversationId), [CacheIds.Conversation, conversationId].join("-") as CacheIds),

        GetEnquiryInsights: async () => this.client.get<EnquiryActivtyResource[]>(this.Routes.GetEnquiryInsights()),

        UnselectAll: async () => this.client.post(this.Routes.UnselectAll()),
        SelectAll: async () => this.client.post(this.Routes.SelectAll()),
    };
}
