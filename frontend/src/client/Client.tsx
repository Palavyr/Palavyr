import axios, { AxiosResponse, AxiosInstance } from "axios";
import { SelectOneFlatData, TableData } from "dashboard/content/responseConfiguration/response/tables/dynamicTable/tableComponents/SelectOneFlat/SelectOneFlatTypes";
import { serverUrl, getSessionIdFromLocalStorage, SPECIAL_HEADERS, getJwtTokenFromLocalStorage } from "./clientUtils";
import { DynamicTableMetas, DynamicTableMeta, StaticTableMetas, staticTableMetaTemplate, Conversation, ConvoTableRow, Areas, Prices, EmailVerificationResponse, AreaTable, FileLink, ConvoNode } from "@Palavyr-Types";
import { PreCheckResult, WidgetPreferences } from "dashboard/content/demo/ChatDemo";

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
        // submitToken: async (token: Token): Promise<AxiosResponse> => this.client.post(`purchase/submit`, { Token: token.id }),
        Subscription: {
            CancelSubscription: async (): Promise<AxiosResponse<string>> => this.client.post(`products/cancel-subscription`),
        },
        Products: {
            // GetAll: async (): Promise<AxiosResponse> => this.client.get(`products/get-products`),
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
            // SendSuccessfulConfirmation: async (successSessionId: string): Promise<AxiosResponse> => this.client.post(`checkout/confirm-purchase`),
        },
    };

    public Area = {
        // GetAreasReactQuery: async (): Promise<AxiosResponse<Areas>> => this.client.get("areas"),

        GetAreas: async (): Promise<AxiosResponse<Areas>> => this.client.get("areas"),
        GetArea: async (areaIdentifier: string): Promise<AxiosResponse<AreaTable>> => this.client.get(`areas/${areaIdentifier}`),
        createArea: (areaName: string): Promise<AxiosResponse<AreaTable>> => this.client.post(`areas/create/`, { areaName: areaName }), // get creates and gets new area
        updateArea: (areaIdentifier: string, areaName: string | null = null, areaDisplayTitle: string | null = null): Promise<AxiosResponse> =>
            this.client.put(`areas/update/${areaIdentifier}`, { areaName: areaName, areaDisplayTitle: areaDisplayTitle }),
        deleteArea: (areaIdentifier: string): Promise<AxiosResponse> => this.client.delete(`areas/delete/${areaIdentifier}`),
    };

    public Configuration = {
        getEstimateConfiguration: async (areaIdentifier: string): Promise<AxiosResponse> => this.client.get(`response/configuration/${areaIdentifier}`),
        updatePrologue: async (areaIdentifier: string, prologue: string): Promise<AxiosResponse> => this.client.put(`response/configuration/${areaIdentifier}/prologue`, { prologue: prologue }),
        updateEpilogue: async (areaIdentifier: string, epilogue: string): Promise<AxiosResponse> => this.client.put(`response/configuration/${areaIdentifier}/epilogue`, { epilogue: epilogue }),

        Tables: {
            Dynamic: {

                getDynamicTableMetas: async (areaIdentifier: string): Promise<AxiosResponse<DynamicTableMetas>> => this.client.get(`tables/dynamic/type/${areaIdentifier}`),

                getDynamicTableTypes: async (areaIdentifier: string): Promise<AxiosResponse<string[]>> => this.client.get(`tables/dynamic/type/${areaIdentifier}`),
                getAvailableTablesPrettyNames: async (): Promise<AxiosResponse<string[]>> => this.client.get(`tables/dynamic/available-tables-pretty-names`),
                modifyDynamicTableMeta: async (dynamicTableMeta: DynamicTableMeta): Promise<AxiosResponse<DynamicTableMeta>> => this.client.put(`tables/dynamic/modify`, dynamicTableMeta),


                getDynamicTableData: async (areaIdentifier: string, tableType: string, tableId: string): Promise<AxiosResponse<TableData>> => this.client.get(`tables/dynamic/${tableType}/tableId/${tableId}/data/${areaIdentifier}/`),
                getDynamicTableDataTempate: async (areaIdentifier: string, tableType: string, tableId: string): Promise<AxiosResponse<TableData>> => this.client.get(`tables/dynamic/${tableType}/data/template/${areaIdentifier}/${tableId}`),
                setDynamicTableType: async (areaIdentifier: string, tableType: string): Promise<AxiosResponse> => this.client.put(`tables/dynamic/type/${areaIdentifier}/${tableType}`),
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
            GetEmailTemplate: async (areaIdentifier: string): Promise<AxiosResponse> => this.client.get(`email/${areaIdentifier}/emailtemplate`),
            SaveEmailTemplate: async (areaIdentifier: string, content: string): Promise<AxiosResponse<string>> => this.client.put(`email/${areaIdentifier}/emailtemplate`, { emailtemplate: content }),
        },

        Attachments: {
            fetchAttachmentLinks: async (areaIdentifier: string) => this.client.get(`attachments/${areaIdentifier}`),
            // getAttachmentUrl: async (areaIdentifier: string, fileName: string) => this.client.get(`attachments/${areaIdentifier}/filelink`, { data: { fileName: fileName } }),
            removeAttachment: async (areaIdentifier: string, fileId: string): Promise<AxiosResponse> => this.client.delete(`attachments/${areaIdentifier}/file-link`, { data: { fileId: fileId } }),

            saveSingleAttachment: async (areaIdentifier: string, formData: FormData): Promise<AxiosResponse> =>
                this.client.post(`attachments/${areaIdentifier}/save-one`, formData, {
                    headers: {
                        Accept: "application/json",
                        "Content-Type": "multipart/form-data",
                    },
                }),
            saveManyAttachments: async (areaIdentifier: string, formData: FormData): Promise<AxiosResponse> =>
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
        ModifyConversation: async (nodelist: Conversation, areaIdentifier: string, idsToDelete: Array<string>): Promise<AxiosResponse> => this.client.put(`configure-conversations/${areaIdentifier}`, { IdsToDelete: idsToDelete, Transactions: nodelist }),

        GetConversationNode: async (nodeId: string): Promise<AxiosResponse<ConvoNode>> => this.client.get(`configure-conversations/nodes/${nodeId}`),
        ModifyConversationNode: async (nodeId: string, updatedNode: ConvoTableRow): Promise<ConvoTableRow> => this.client.put(`configure-conversations/nodes/${nodeId}`, updatedNode),
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
            getApiKey: async (): Promise<AxiosResponse<string>> => this.client.get(`account/settings/apikey`),
            confirmEmailAddress: async (authToken: string): Promise<AxiosResponse<boolean>> => this.client.post(`account/confirmation/${authToken}/action/setup`),
            checkIsActive: async (): Promise<AxiosResponse<boolean>> => this.client.get(`account/is-active`),

            UpdatePassword: async (oldPassword: string, newPassword: string): Promise<AxiosResponse> => this.client.put(`account/settings/update/password`, { OldPassword: oldPassword, Password: newPassword }),
            updateCompanyName: async (companyName: string): Promise<AxiosResponse> => this.client.put(`account/settings/update/company-name`, { CompanyName: companyName }),
            updateEmail: async (newEmail: string): Promise<AxiosResponse> => this.client.put(`account/settings/update/email`, { EmailAddress: newEmail }),
            updateUserName: async (newUserName: string): Promise<AxiosResponse> => this.client.put(`account/settings/update/username/`, { UserName: newUserName }),
            updatePhoneNumber: async (newPhoneNumber: string): Promise<AxiosResponse> => this.client.put(`account/settings/update/phone-number`, { PhoneNumber: newPhoneNumber }),
            updateLocale: async (newLocale: string): Promise<AxiosResponse> => this.client.put(`account/settings/update/locale`, { Locale: newLocale }),
            updateCompanyLogo: async (formData: FormData): Promise<AxiosResponse> =>
                this.client.put(`account/settings/update/logo`, formData, {
                    headers: {
                        Accept: "application/json",
                        "Content-Type": "multipart/form-data",
                    },
                }),

            getCompanyName: async (): Promise<AxiosResponse> => this.client.get(`account/settings/company-name`),
            getEmail: async (): Promise<AxiosResponse> => this.client.get(`account/settings/email`),
            getUserName: async (): Promise<AxiosResponse> => this.client.get(`account/settings/username`),
            getPhoneNumber: async (): Promise<AxiosResponse> => this.client.get(`account/settings/phone-number`),
            getLocale: async (): Promise<AxiosResponse> => this.client.get(`account/settings/locale`),
            getCompanyLogo: async (): Promise<AxiosResponse> => this.client.get(`account/settings/logo`),
            getCurrentPlan: async (): Promise<AxiosResponse> => this.client.get(`account/settings/current-plan`),
            DeleteAccount: async (): Promise<AxiosResponse> => this.client.post(`account/delete-account`),
        },
        Groups: {
            GetGroups: async (): Promise<AxiosResponse> => this.client.get(`group/`),
            AddGroup: async (parentId: string | null, groupName: string): Promise<AxiosResponse> => this.client.post(`group/`, { groupName: groupName, parentId: parentId }),
            UpdateGroupName: async (groupName: string, groupId: string): Promise<AxiosResponse> => this.client.put(`group/${groupId}`, { groupName: groupName }),
            UpdateAreaGroup: async (areaIdentifier: string, groupId: string | null): Promise<AxiosResponse> => this.client.put(`group/area/${areaIdentifier}/${groupId}`),

            RemoveGroup: async (groupId: string): Promise<AxiosResponse> => this.client.delete(`group/${groupId}`),
            DeleteAreaGroup: async (areaIdentifier: string): Promise<AxiosResponse> => this.client.delete(`group/area/${areaIdentifier}`),
        },
        EmailVerification: {
            RequestEmailVerification: async (emailAddress: string, areaIdentifier: string): Promise<AxiosResponse<EmailVerificationResponse>> => this.client.post(`verification/email/${areaIdentifier}`, { EmailAddress: emailAddress }),
        },
    };

    public Enquiries = {
        getEnquiries: async (): Promise<AxiosResponse> => this.client.get(`enquiries`),
        updateEnquiry: async (conversationId: string): Promise<AxiosResponse> => this.client.put(`enquiries/update/${conversationId}`),
    };
}
