import axios, { AxiosResponse, AxiosInstance } from "axios";
import { ConversationUpdate, AreaTable, CompleteConverationDetails, PreCheckResult, WidgetPreferences } from "../types";
import { serverUrl } from "./clientUtils";

export interface IClient {
    Widget: {
        Access: {
            runPreCheck: () => Promise<AxiosResponse<PreCheckResult>>;
            fetchGroups: () => Promise<AxiosResponse>;
            createConvo: (areaId: string) => Promise<AxiosResponse>;
            fetchAreas: () => Promise<AxiosResponse>;
            fetchPreferences: () => Promise<AxiosResponse<WidgetPreferences>>;
            postUpdateAsync: (update: ConversationUpdate) => Promise<AxiosResponse>;
            sendConfirmationEmail: (areaIdentifier: string, emailAddress: string, dynamicResponses: Array<{[key: string]: string}>, keyValues: KeyValues, conviId: string) => Promise<AxiosResponse>;
            postCompleteConversation: (completeConvo: CompleteConverationDetails) => Promise<AxiosResponse>;

        }
    },
}

export type KeyValues = {
    [x: string]: string
}

const CreateClient = (secretKey: string): IClient => {

    var AxiosClient: AxiosInstance = axios.create(
        {
            headers: {
                "action": "widgetAccess"
            }
        }
    )
    AxiosClient.defaults.baseURL = serverUrl + "/api/"

    let Client = {
        Widget: {
            Access: {
                fetchAreas: async (): Promise<AxiosResponse<Array<AreaTable>>> => AxiosClient.get(`widget/areas?key=${secretKey}`),
                runPreCheck: async (): Promise<AxiosResponse<PreCheckResult>> => AxiosClient.get(`widget/pre-check?key=${secretKey}`),
                fetchGroups: async (): Promise<AxiosResponse> => AxiosClient.get(`widget/groups?key=${secretKey}`),
                createConvo: async (areaId: string): Promise<AxiosResponse> => AxiosClient.get(`widget/${areaId}/create?key=${secretKey}`),
                fetchPreferences: async (): Promise<AxiosResponse> =>AxiosClient.get(`widget/preferences?key=${secretKey}`),
                postUpdateAsync: async(update: ConversationUpdate): Promise<AxiosResponse> => AxiosClient.post(`widget/conversation?key=${secretKey}`, update),
                sendConfirmationEmail: async(areaIdentifier: string, emailAddress: string, dynamicResponses: Array<{[key: string]: string}>, keyValues: KeyValues, convoId: string): Promise<AxiosResponse> => AxiosClient.post(`widget/area/${areaIdentifier}/email/send?key=${secretKey}`, {
                    ConversationId: convoId,
                    EmailAddress: emailAddress,
                    DynamicResponses: dynamicResponses,
                    KeyValues: keyValues
                }),
                postCompleteConversation: async(completeConvo: CompleteConverationDetails) => AxiosClient.post(`widget/complete?key=${secretKey}`, completeConvo)
            }
        },
        secretKey: secretKey
    }

    return Client
}

export default CreateClient;