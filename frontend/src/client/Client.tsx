import axios, { AxiosResponse, AxiosInstance } from "axios";
import { TableData } from "dashboard/content/responseConfiguration/response/tables/dynamicTable/tableComponents/SelectOneFlat/SelectOneFlatTypes";
import { serverUrl, getSessionIdFromLocalStorage, SPECIAL_HEADERS } from "./clientUtils";
import { DynamicTableMetas, DynamicTableMeta, StaticTableMetas, staticTableMetaTemplate, Conversation, ConvoTableRow } from "@Palavyr-Types";


export class ApiClient {
    private client: AxiosInstance
    constructor(serverURL: string = serverUrl) {

        var sessionId = getSessionIdFromLocalStorage();

        this.client = axios.create(
            {
                headers: {
                    action: "tubmcgubs",
                    sessionId: sessionId,
                    ...SPECIAL_HEADERS
                }
            }
        )
        this.client.defaults.baseURL = serverUrl + "/api/";
    }

    public Area = {
        GetAreas: async (): Promise<AxiosResponse> => this.client.get("areas"),
        GetArea: async (areaIdentifier: string): Promise<AxiosResponse> => this.client.get(`areas/${areaIdentifier}`),
        createArea: (areaName: string): Promise<AxiosResponse> => this.client.post(`areas/create/`, { areaName: areaName }), // get creates and gets new area
        updateArea: (areaIdentifier: string, areaName: string | null = null, areaDisplayTitle: string | null = null): Promise<AxiosResponse> => this.client.put(`areas/update/${areaIdentifier}`, { areaName: areaName, areaDisplayTitle: areaDisplayTitle }),
        deleteArea: (areaIdentifier: string): Promise<AxiosResponse> => this.client.delete(`areas/delete/${areaIdentifier}`)
    }

    public Configuration = {
        getEstimateConfiguration: async (areaIdentifier: string): Promise<AxiosResponse> => this.client.get(`estimate/configuration/${areaIdentifier}`),
        updatePrologue: async (areaIdentifier: string, prologue: string): Promise<AxiosResponse> => this.client.put(`estimate/configuration/${areaIdentifier}/logue`, { prologue: prologue }),
        updateEpilogue: async (areaIdentifier: string, epilogue: string): Promise<AxiosResponse> => this.client.put(`estimate/configuration/${areaIdentifier}/logue`, { epilogue: epilogue }),

        Tables: {
            Dynamic: {
                getDynamicTableTypes: async (areaIdentifier: string): Promise<AxiosResponse<string[]>> => this.client.get(`tables/dynamic/type/${areaIdentifier}`),
                getDynamicTableMetas: async (areaIdentifier: string): Promise<AxiosResponse<DynamicTableMetas>> => this.client.get(`tables/dynamic/type/${areaIdentifier}`),
                getAvailableTables: async (): Promise<AxiosResponse> => this.client.get(`tables/dynamic/availabletables`),

                getDynamicTableData: async (areaIdentifier: string, tableType: string, tableId: string): Promise<AxiosResponse> => this.client.get(`tables/dynamic/${tableType}/tableId/${tableId}/data/${areaIdentifier}/`),
                getDynamicTableDataTempate: async (areaIdentifier: string, tableType: string, tableId: string): Promise<AxiosResponse> => this.client.get(`tables/dynamic/${tableType}/data/template/${areaIdentifier}/${tableId}`),
                setDynamicTableType: async (areaIdentifier: string, tableType: string): Promise<AxiosResponse> => this.client.put(`tables/dynamic/type/${areaIdentifier}/${tableType}`),
                saveDynamicTable: async (areaIdentifier: string, tableType: string, tableData: TableData, tableId: string, tableTag: string): Promise<AxiosResponse> => this.client.put(`tables/dynamic/${tableType}/data/save/tableId/${tableId}/${areaIdentifier}/`, { TableTag: tableTag, [tableType]: tableData }),
                createDynamicTable: async (areaIdentifier: string): Promise<AxiosResponse> => this.client.post(`tables/dynamic/${areaIdentifier}`),
                deleteDynamicTable: async (areaIdentifier: string, tableType: string, tableId: string): Promise<AxiosResponse> => this.client.delete(`tables/dynamic/${tableType}/${areaIdentifier}/tableId/${tableId}`),
                updateDynamicTableMeta: async (dynamicTableMeta: DynamicTableMeta): Promise<AxiosResponse> => this.client.put(`tables/dynamic/update`, dynamicTableMeta),
            },
            Static: {
                updateStaticTablesMetas: async (areaIdentifier: string, staticTablesMetas: StaticTableMetas): Promise<AxiosResponse<StaticTableMetas>> => this.client.put(`estimate/configuration/${areaIdentifier}/static/tables/save`, staticTablesMetas),
                getStaticTablesMetaTemplate: async (areaIdentifier: string): Promise<AxiosResponse<staticTableMetaTemplate>> => this.client.get(`estimate/configuration/${areaIdentifier}/static/tables/template`),
            }
        },

        Preview: {
            fetchPreview: async (areaIdentifier: string): Promise<AxiosResponse> => this.client.get(`preview/estimate/${areaIdentifier}`)
        },

        Email: {
            GetEmailTemplate: async (areaIdentifier: string): Promise<AxiosResponse> => this.client.get(`email/${areaIdentifier}/emailtemplate`),
            SaveEmailTemplate: async (areaIdentifier: string, content: string): Promise<AxiosResponse<string>> => this.client.put(`email/${areaIdentifier}/emailtemplate`, { emailtemplate: content }),
        },

        Attachments: {
            fetchAttachmentLinks: async (areaIdentifier: string) => this.client.get(`files/${areaIdentifier}`),
            // getAttachmentUrl: async (areaIdentifier: string, fileName: string) => this.client.get(`files/${areaIdentifier}/filelink`, { data: { fileName: fileName } }),
            removeAttachment: async (areaIdentifier: string, fileId: string): Promise<AxiosResponse> => this.client.delete(`files/${areaIdentifier}/filelink`, { data: { fileId: fileId } }),

            saveSingleAttachment: async (areaIdentifier: string, formData: FormData): Promise<AxiosResponse> => this.client.post(
                `files/${areaIdentifier}/saveone`,
                formData,
                {
                    headers: {
                        'Accept': 'application/json',
                        'Content-Type': 'multipart/form-data'
                    }
                }
            ),
            saveManyAttachments: async (areaIdentifier: string, formData: FormData): Promise<AxiosResponse> => this.client.post(
                `files/${areaIdentifier}/savemany`,
                formData,
                {
                    headers: {
                        'Accept': 'application/json',
                        'Content-Type': 'multipart/form-data'
                    }
                }
            )
        }
    }

    public Conversations = {
        GetConversation: async (areaIdentifier: string): Promise<AxiosResponse> => this.client.get(`convos/${areaIdentifier}`),
        GetConversationNode: async (nodeId: string): Promise<AxiosResponse> => this.client.get(`convos/nodes/${nodeId}`),
        PostConversation: async (nodelist: Conversation, areaIdentifier: string, idsToDelete: Array<string>): Promise<AxiosResponse> => this.client.post(`convos/${areaIdentifier}`, { IdsToDelete: idsToDelete, Transactions: nodelist }),
        DeleteConversationNodesByIds: async (nodeIds: string): Promise<AxiosResponse> => this.client.delete("convos/nodes", { data: { nodeIds: nodeIds.split(",") } }),
        UpdateConversation: async (areaIdentifier: string, nodelist: Conversation): Promise<AxiosResponse> => this.client.put(`convos/update/${areaIdentifier}`, nodelist),
        PutConversationNode: async (nodeId: string, updatedNode: ConvoTableRow): Promise<ConvoTableRow> => this.client.put(`convos/nodes/${nodeId}`, updatedNode),
    }

    public WidgetDemo = {
        RunConversationPrecheck: async (): Promise<AxiosResponse> => this.client.get(`widget/demo/precheck`),
    }

    public Settings = {

        Subscriptions: {
            getNumAreas: async (): Promise<AxiosResponse> => this.client.get(`subscriptions/count`)
        },

        Account: {

            getApiKey: async (): Promise<AxiosResponse> => this.client.get(`account/settings/apikey`),
            confirmEmailAddress: async (authToken: string): Promise<AxiosResponse> => this.client.post(`account/confirmation/${authToken}/action/setup`),
            checkIsActive: async (): Promise<AxiosResponse> => this.client.get(`account/isActive`),

            UpdatePassword: async (oldPassword: string, newPassword: string): Promise<AxiosResponse> => this.client.put(`account/settings/update/password`, { OldPassword: oldPassword, Password: newPassword }),
            updateCompanyName: async (companyName: string): Promise<AxiosResponse> => this.client.put(`account/settings/update/companyname`, { CompanyName: companyName }),
            updateEmail: async (newEmail: string): Promise<AxiosResponse> => this.client.put(`account/settings/update/email`, { EmailAddress: newEmail }),
            updateUserName: async (newUserName: string): Promise<AxiosResponse> => this.client.put(`account/settings/update/username/`, { UserName: newUserName }),
            updatePhoneNumber: async (newPhoneNumber: string): Promise<AxiosResponse> => this.client.put(`account/settings/update/phonenumber`, { PhoneNumber: newPhoneNumber }),
            updateLocale: async (newLocale: string): Promise<AxiosResponse> => this.client.put(`account/settings/update/locale`, {Locale: newLocale}),

            getCompanyName: async (): Promise<AxiosResponse> => this.client.get(`account/settings/companyname`),
            getEmail: async (): Promise<AxiosResponse> => this.client.get(`account/settings/email`),
            getUserName: async (): Promise<AxiosResponse> => this.client.get(`account/settings/username`),
            getPhoneNumber: async (): Promise<AxiosResponse> => this.client.get(`account/settings/phonenumber`),
            getLocale: async (): Promise<AxiosResponse> => this.client.get(`account/settings/locale`)
        },
        Groups: {
            GetGroups: async (): Promise<AxiosResponse> => this.client.get(`group/`),
            AddGroup: async (parentId: string | null, groupName: string): Promise<AxiosResponse> => this.client.post(`group/`, { groupName: groupName, parentId: parentId }),
            RemoveGroup: async (groupId: string): Promise<AxiosResponse> => this.client.delete(`group/${groupId}`),
            UpdateGroupName: async (groupName: string, groupId: string): Promise<AxiosResponse> => this.client.put(`group/${groupId}`, { groupName: groupName }),
            UpdateAreaGroup: async (areaIdentifier: string, groupId: string | null): Promise<AxiosResponse> => this.client.put(`group/area/${areaIdentifier}/${groupId}`),
            DeleteAreaGroup: async (areaIdentifier: string): Promise<AxiosResponse> => this.client.delete(`group/area/${areaIdentifier}`),
        },
    }

    public Enquiries = {
        getEnquiries: async (): Promise<AxiosResponse> => this.client.get(`enquiries`),
        updateEnquiry: async (conversationId: string): Promise<AxiosResponse> => this.client.put(`enquiries/update/${conversationId}`)
    }
}
