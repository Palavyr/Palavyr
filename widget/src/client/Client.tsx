import axios, { AxiosResponse, AxiosInstance } from "axios";
import { ConversationUpdate, AreaTable, CompleteConverationDetails } from "../types";
import { serverUrl } from "./clientUtils";

export interface IClient {
    Widget: {
        Access: {
            runPreCheck: () => Promise<AxiosResponse>;
            fetchGroups: () => Promise<AxiosResponse>;
            createConvo: (areaId: string) => Promise<AxiosResponse>;
            fetchAreas: () => Promise<AxiosResponse>;
            postUpdateAsync: (update: ConversationUpdate) => Promise<AxiosResponse>;
            sendConfirmationEmail: (areaIdentifier: string, emailAddress: string, dynamicResponse: string, keyValues: KeyValues, conviId: string) => Promise<AxiosResponse>;
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
                runPreCheck: async (): Promise<AxiosResponse> => AxiosClient.get(`widget/${secretKey}/precheck`),
                fetchGroups: async (): Promise<AxiosResponse> => AxiosClient.get(`widget/${secretKey}/groups`),
                createConvo: async (areaId: string): Promise<AxiosResponse> => AxiosClient.get(`widget/${secretKey}/${areaId}/create`),
                fetchAreas: async (): Promise<AxiosResponse<Array<AreaTable>>> => AxiosClient.get(`widget/${secretKey}/areas`),
                postUpdateAsync: async(update: ConversationUpdate): Promise<AxiosResponse> => AxiosClient.post(`widget/${secretKey}/conversation`, update),
                sendConfirmationEmail: async(areaIdentifier: string, emailAddress: string, dynamicResponse: string, keyValues: KeyValues, convoId: string): Promise<AxiosResponse> => AxiosClient.post(`widget/${secretKey}/area/${areaIdentifier}/email/send`, {
                    ConversationId: convoId,
                    EmailAddress: emailAddress,
                    DynamicResponse: dynamicResponse,
                    KeyValues: keyValues
                }),
                postCompleteConversation: async(completeConvo: CompleteConverationDetails) => AxiosClient.post(`widget/${secretKey}/complete`, completeConvo)
            }
        },
        secretKey: secretKey
    }

    return Client
}

export default CreateClient;