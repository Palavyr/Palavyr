import axios, { AxiosResponse, AxiosInstance } from "axios";
import { serverUrl, getSessionIdFromLocalStorage, SPECIAL_HEADERS, getJwtTokenFromLocalStorage } from "./clientUtils";
import {
    DynamicTableMetas,
    DynamicTableMeta,
    StaticTableMetas,
    staticTableMetaTemplate,
    Conversation,
    ConvoTableRow,
    Areas,
    Prices,
    EmailVerificationResponse,
    AreaTable,
    FileLink,
    ConvoNode,
    ResponseConfigurationType,
    AccountEmailSettingsResponse,
    GroupTable,
    Groups,
    Enquiries,
    PhoneSettingsResponse,
    NodeTypeOptions,
    RequiredDetails,
    PlanType,
    CompletedConversation,
    PreCheckResult,
    WidgetPreferences,
    VariableDetail,
    LocaleDefinition,
    ProductOptions,
    ProductIds,
    PlanStatus,
} from "@Palavyr-Types";
import { TableData } from "dashboard/content/responseConfiguration/response/tables/dynamicTable/DynamicTableTypes";
import { TableNameMap } from "dashboard/content/responseConfiguration/response/tables/dynamicTable/DynamicTableConfiguration";

export class ApiClient {
    private client: AxiosInstance;
    constructor(serverURL: string = serverUrl) {
        var sessionId = getSessionIdFromLocalStorage();
        var authToken = getJwtTokenFromLocalStorage();

        this.client = axios.create({
            headers: {
                action: "tubmcgubs",
                sessionId: sessionId,
                Authorization: "Bearer " + authToken, //include space after Bearer
                ...SPECIAL_HEADERS,
            },
        });
        this.client.defaults.baseURL = serverUrl + "/api/";
    }

    public Purchase = {
        Customer: {
            GetCustomerId: async (): Promise<AxiosResponse<string>> => this.client.get(`payments/customer-id`),
            GetCustomerPortal: async (customerId: string, returnUrl: string): Promise<AxiosResponse> => this.client.post(`payments/customer-portal`, { CustomerId: customerId, ReturnUrl: returnUrl }),
        },
        Subscription: {
            CancelSubscription: async (): Promise<AxiosResponse<string>> => this.client.post(`products/cancel-subscription`),
        },
        Prices: {
            GetPrices: async (productId: string): Promise<AxiosResponse<Prices>> => this.client.get(`products/prices/get-prices/${productId}`),
        },
        Checkout: {
            CreateCheckoutSession: async (priceId: string, cancelUrl: string, successUrl: string): Promise<AxiosResponse<string>> =>
                this.client.post(`checkout/create-checkout-session`, {
                    PriceId: priceId,
                    CancelUrl: cancelUrl,
                    SuccessUrl: successUrl,
                }),
        },
    };

    public Products = {
        getProducts: async (): Promise<AxiosResponse<ProductIds>> => this.client.get(`products/all`),
    };

    public Area = {
        UpdateIsEnabled: async (areaToggleStateUpdate: boolean, areaIdentifier: string): Promise<AxiosResponse<boolean>> => this.client.put(`areas/${areaIdentifier}/area-toggle`, { IsEnabled: areaToggleStateUpdate }),
        UpdateUseAreaFallbackEmail: async (useAreaFallbackEmailUpdate: boolean, areaIdentifier: string): Promise<AxiosResponse<boolean>> =>
            this.client.put(`areas/${areaIdentifier}/use-fallback-email-toggle`, { UseFallback: useAreaFallbackEmailUpdate }),
        GetAreas: async (): Promise<AxiosResponse<Areas>> => this.client.get("areas"),
        GetArea: async (areaIdentifier: string): Promise<AxiosResponse<AreaTable>> => this.client.get(`areas/${areaIdentifier}`),
        createArea: (areaName: string): Promise<AxiosResponse<AreaTable>> => this.client.post(`areas/create/`, { AreaName: areaName }), // get creates and gets new area
        updateAreaName: (areaIdentifier: string, areaName: string): Promise<AxiosResponse<string>> => this.client.put(`areas/update/name/${areaIdentifier}`, { AreaName: areaName }),
        updateDisplayTitle: (areaIdentifier: string, displayTitle: string): Promise<AxiosResponse<string>> => this.client.put(`areas/update/display-title/${areaIdentifier}`, { AreaDisplayTitle: displayTitle }),
        deleteArea: (areaIdentifier: string): Promise<AxiosResponse> => this.client.delete(`areas/delete/${areaIdentifier}`),
    };

    public Configuration = {
        getEstimateConfiguration: async (areaIdentifier: string): Promise<AxiosResponse<ResponseConfigurationType>> => this.client.get(`response/configuration/${areaIdentifier}`),
        updatePrologue: async (areaIdentifier: string, prologue: string): Promise<AxiosResponse<string>> => this.client.put(`response/configuration/${areaIdentifier}/prologue`, { prologue: prologue }),
        updateEpilogue: async (areaIdentifier: string, epilogue: string): Promise<AxiosResponse<string>> => this.client.put(`response/configuration/${areaIdentifier}/epilogue`, { epilogue: epilogue }),

        WidgetState: {
            GetWidgetState: async (): Promise<AxiosResponse<boolean>> => this.client.get(`widget-config/widget-active-state`),
            SetWidgetState: async (updatedWidgetState: boolean): Promise<AxiosResponse<boolean>> => this.client.post(`widget-config/widget-active-state?state=${updatedWidgetState}`),
        },
        Tables: {
            Dynamic: {
                getDynamicTableMetas: async (areaIdentifier: string): Promise<AxiosResponse<DynamicTableMetas>> => this.client.get(`tables/dynamic/type/${areaIdentifier}`),
                getDynamicTableTypes: async (): Promise<AxiosResponse<TableNameMap>> => this.client.get(`tables/dynamic/table-name-map`),

                getDynamicTableData: async (areaIdentifier: string, tableType: string, tableId: string): Promise<AxiosResponse<TableData>> => this.client.get(`tables/dynamic/${tableType}/tableId/${tableId}/data/${areaIdentifier}/`),

                getDynamicTableDataTemplate: async (areaIdentifier: string, tableType: string, tableId: string): Promise<AxiosResponse<TableData>> => this.client.get(`tables/dynamic/${tableType}/data/template/${areaIdentifier}/${tableId}`),

                modifyDynamicTableMeta: async (dynamicTableMeta: DynamicTableMeta): Promise<AxiosResponse<DynamicTableMeta>> => this.client.put(`tables/dynamic/modify`, dynamicTableMeta),
                saveDynamicTable: async (areaIdentifier: string, tableType: string, tableData: TableData, tableId: string, tableTag: string): Promise<AxiosResponse> =>
                    this.client.put(`tables/dynamic/${tableType}/data/save/tableId/${tableId}/${areaIdentifier}/`, { TableTag: tableTag, [tableType]: tableData }),

                createDynamicTable: async (areaIdentifier: string): Promise<AxiosResponse<DynamicTableMeta>> => this.client.post(`tables/dynamic/${areaIdentifier}`),
                deleteDynamicTable: async (areaIdentifier: string, tableType: string, tableId: string): Promise<AxiosResponse> => this.client.delete(`tables/dynamic/${tableType}/${areaIdentifier}/tableId/${tableId}`),
            },
            Static: {
                updateStaticTablesMetas: async (areaIdentifier: string, staticTablesMetas: StaticTableMetas): Promise<AxiosResponse<StaticTableMetas>> =>
                    this.client.put(`response/configuration/${areaIdentifier}/static/tables/save`, staticTablesMetas),
                getStaticTablesMetaTemplate: async (areaIdentifier: string): Promise<AxiosResponse<staticTableMetaTemplate>> => this.client.get(`response/configuration/${areaIdentifier}/static/tables/template`),
            },
        },

        Preview: {
            fetchPreview: async (areaIdentifier: string): Promise<AxiosResponse<FileLink>> => this.client.get(`preview/estimate/${areaIdentifier}`),
        },

        Email: {
            GetVariableDetails: async (): Promise<AxiosResponse<VariableDetail[]>> => this.client.get(`email/variables`),

            // Templates
            GetAreaEmailTemplate: async (areaIdentifier: string): Promise<AxiosResponse<string>> => this.client.get(`email/${areaIdentifier}/email-template`),
            GetAreaFallbackEmailTemplate: async (areaIdentifier: string): Promise<AxiosResponse<string>> => this.client.get(`email/fallback/${areaIdentifier}/email-template`),
            GetDefaultFallbackEmailTemplate: async (): Promise<AxiosResponse<string>> => this.client.get(`email/fallback/default-email-template`),

            SaveAreaEmailTemplate: async (areaIdentifier: string, EmailTemplate: string): Promise<AxiosResponse<string>> => this.client.put(`email/${areaIdentifier}/email-template`, { EmailTemplate }),
            SaveAreaFallbackEmailTemplate: async (areaIdentifier: string, EmailTemplate: string): Promise<AxiosResponse<string>> => this.client.put(`email/fallback/${areaIdentifier}/email-template`, { EmailTemplate }),
            SaveDefaultFallbackEmailTemplate: async (EmailTemplate: string): Promise<AxiosResponse<string>> => this.client.put(`email/fallback/default-email-template`, { EmailTemplate }),

            // Subjects
            GetAreaSubject :(areaIdentifier: string): Promise<AxiosResponse<string>> => this.client.get(`email/subject/${areaIdentifier}`),
            GetAreaFallbackSubject: (areaIdentifier: string): Promise<AxiosResponse<string>> => this.client.get(`email/fallback/subject/${areaIdentifier}`),
            GetDefaultFallbackSubject: async (): Promise<AxiosResponse<string>> => this.client.get(`email/default-fallback-subject`),

            SaveAreaSubject: (areaIdentifier: string, subject: string): Promise<AxiosResponse<string>> => this.client.put(`email/subject/${areaIdentifier}`, { Subject: subject }),
            SaveAreaFallbackSubject: (areaIdentifier: string, subject: string): Promise<AxiosResponse<string>> => this.client.put(`email/fallback/subject/${areaIdentifier}`, { Subject: subject }),
            SaveDefaultFallbackSubject: async (subject: string): Promise<AxiosResponse<string>> => this.client.put(`email/fallback/default-subject`, { Subject: subject }),
        },

        Attachments: {
            fetchAttachmentLinks: async (areaIdentifier: string): Promise<AxiosResponse<FileLink[]>> => this.client.get(`attachments/${areaIdentifier}`),
            removeAttachment: async (areaIdentifier: string, fileId: string): Promise<AxiosResponse<FileLink[]>> => this.client.delete(`attachments/${areaIdentifier}/file-link`, { data: { fileId: fileId } }),

            saveSingleAttachment: async (areaIdentifier: string, formData: FormData): Promise<AxiosResponse<FileLink[]>> =>
                this.client.post(`attachments/${areaIdentifier}/save-one`, formData, {
                    headers: {
                        Accept: "application/json",
                        "Content-Type": "multipart/form-data",
                    },
                }),
            saveManyAttachments: async (areaIdentifier: string, formData: FormData): Promise<AxiosResponse<FileLink[]>> =>
                this.client.post(`attachments/${areaIdentifier}/save-many`, formData, {
                    headers: {
                        Accept: "application/json",
                        "Content-Type": "multipart/form-data",
                    },
                }),
        },
    };

    public Conversations = {
        GetConversation: async (areaIdentifier: string): Promise<AxiosResponse<Conversation>> => this.client.get(`configure-conversations/${areaIdentifier}`),
        GetConversationNode: async (nodeId: string): Promise<AxiosResponse<ConvoNode>> => this.client.get(`configure-conversations/nodes/${nodeId}`),
        GetNodeOptionsList: async (areaIdentifier: string): Promise<AxiosResponse<NodeTypeOptions>> => this.client.get(`configure-conversations/${areaIdentifier}/node-type-options`),
        GetMissingNodes: async (areaIdentifier: string): Promise<AxiosResponse<string[]>> => this.client.get(`configure-conversations/${areaIdentifier}/missing-nodes`),

        CheckIfIsMultiOptionType: async (nodeType: string): Promise<AxiosResponse<boolean>> => this.client.get(`configure-conversations/check-multi-option/${nodeType}`),
        CheckIfIsTerminalType: async (nodeType: string): Promise<AxiosResponse<boolean>> => this.client.get(`configure-conversations/check-terminal/${nodeType}`),

        //TODO : Return from API
        ModifyConversation: async (nodelist: Conversation, areaIdentifier: string, idsToDelete: Array<string>): Promise<AxiosResponse> =>
            this.client.put(`configure-conversations/${areaIdentifier}`, { IdsToDelete: idsToDelete, Transactions: nodelist }),

        ModifyConversationNode: async (nodeId: string, areaIdentifier: string, updatedNode: ConvoTableRow): Promise<AxiosResponse<Conversation>> => this.client.put(`configure-conversations/${areaIdentifier}/nodes/${nodeId}`, updatedNode),

        // TODO: Deprecate eventually
        EnsureDBIsValid: async (): Promise<AxiosResponse> => this.client.post(`configure-conversations/ensure-db-valid`),
    };

    public WidgetDemo = {
        RunConversationPrecheck: async (): Promise<AxiosResponse<PreCheckResult>> => this.client.get(`widget-config/demo/pre-check`),
        GetWidetPreferences: async (): Promise<AxiosResponse<WidgetPreferences>> => this.client.get(`widget-config/preferences`),
        SaveWidgetPreferences: async (prefs: WidgetPreferences): Promise<AxiosResponse> => this.client.put(`widget-config/preferences`, prefs),
    };

    public Settings = {
        Subscriptions: {
            getNumAreas: async (): Promise<AxiosResponse<number>> => this.client.get(`subscriptions/count`),
        },

        Account: {
            getApiKey: async (): Promise<AxiosResponse<string>> => this.client.get(`account/settings/api-key`),
            confirmEmailAddress: async (authToken: string): Promise<AxiosResponse<boolean>> => this.client.post(`account/confirmation/${authToken}/action/setup`),
            checkIsActive: async (): Promise<AxiosResponse<boolean>> => this.client.get(`account/is-active`),

            UpdatePassword: async (oldPassword: string, newPassword: string): Promise<AxiosResponse<boolean>> => this.client.put(`account/settings/password`, { OldPassword: oldPassword, Password: newPassword }),
            updateCompanyName: async (companyName: string): Promise<AxiosResponse<string>> => this.client.put(`account/settings/company-name`, { CompanyName: companyName }),
            updateEmail: async (newEmail: string): Promise<AxiosResponse<EmailVerificationResponse>> => this.client.put(`account/settings/email`, { EmailAddress: newEmail }),
            updateUserName: async (newUserName: string): Promise<AxiosResponse<string>> => this.client.put(`account/settings/user-name/`, { UserName: newUserName }),
            updatePhoneNumber: async (newPhoneNumber: string): Promise<AxiosResponse<string>> => this.client.put(`account/settings/phone-number`, { PhoneNumber: newPhoneNumber }),

            // TODO: Stronger type for locale
            updateLocale: async (newLocaleId: string): Promise<AxiosResponse<LocaleDefinition>> => this.client.put(`account/settings/locale`, { LocaleId: newLocaleId }),

            updateCompanyLogo: async (formData: FormData): Promise<AxiosResponse<string>> =>
                this.client.put(`account/settings/logo`, formData, {
                    headers: {
                        Accept: "application/json",
                        "Content-Type": "multipart/form-data",
                    },
                }),

            getCompanyName: async (): Promise<AxiosResponse<string>> => this.client.get(`account/settings/company-name`),
            getEmail: async (): Promise<AxiosResponse<AccountEmailSettingsResponse>> => this.client.get(`account/settings/email`),
            getUserName: async (): Promise<AxiosResponse<string>> => this.client.get(`account/settings/user-name`),
            getPhoneNumber: async (): Promise<AxiosResponse<PhoneSettingsResponse>> => this.client.get(`account/settings/phone-number`),

            GetLocale: async (): Promise<AxiosResponse<LocaleDefinition>> => this.client.get(`account/settings/locale`),
            getCompanyLogo: async (): Promise<AxiosResponse<string>> => this.client.get(`account/settings/logo`),
            getCurrentPlan: async (): Promise<AxiosResponse<PlanStatus>> => this.client.get(`account/settings/current-plan`),

            DeleteAccount: async (): Promise<AxiosResponse> => this.client.post(`account/delete-account`),
        },
        Groups: {
            GetGroups: async (): Promise<AxiosResponse<GroupTable>> => this.client.get(`group/`),
            AddGroup: async (parentId: string | null, groupName: string): Promise<AxiosResponse<GroupTable>> => this.client.post(`group/`, { groupName: groupName, parentId: parentId }),
            UpdateGroupName: async (groupName: string, groupId: string): Promise<AxiosResponse<Groups>> => this.client.put(`group/${groupId}`, { groupName: groupName }),
            UpdateAreaGroup: async (areaIdentifier: string, groupId: string | null): Promise<AxiosResponse<Groups>> => this.client.put(`group/area/${areaIdentifier}/${groupId}`),

            RemoveGroup: async (groupId: string): Promise<AxiosResponse<Groups>> => this.client.delete(`group/${groupId}`),
            DeleteAreaGroup: async (areaIdentifier: string): Promise<AxiosResponse<Groups>> => this.client.delete(`group/area/${areaIdentifier}`),
        },
        EmailVerification: {
            RequestEmailVerification: async (emailAddress: string, areaIdentifier: string): Promise<AxiosResponse<EmailVerificationResponse>> => this.client.post(`verification/email/${areaIdentifier}`, { EmailAddress: emailAddress }),
            CheckEmailVerificationStatus: async (emailAddress: string): Promise<AxiosResponse<boolean>> => this.client.post(`verification/email/status`, { EmailAddress: emailAddress }),
        },
    };

    public Enquiries = {
        getEnquiries: async (): Promise<AxiosResponse<Enquiries>> => this.client.get(`enquiries`),
        updateEnquiry: async (conversationId: string): Promise<AxiosResponse<Enquiries>> => this.client.put(`enquiries/update/${conversationId}`),
        getConversation: async (conversationId: string): Promise<AxiosResponse<CompletedConversation>> => this.client.get(`enquiries/review/${conversationId}`),
    };
}
