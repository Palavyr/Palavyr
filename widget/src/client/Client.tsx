import axios, { AxiosResponse, AxiosInstance } from "axios";
import { ConversationUpdate, AreaTable } from "../types";

const serverUrl = "https://localhost:5001/api/";
// const env = process.env.PUBLIC_URL;

export interface IClient {
    Widget: {
        Access: {
            runPreCheck: () => Promise<AxiosResponse>;
            fetchGroups: () => Promise<AxiosResponse>;
            fetchNodes: (areaId: string) => Promise<AxiosResponse>;
            fetchPreferences: () => Promise<AxiosResponse>;
            fetchAreas: () => Promise<AxiosResponse>;
            postUpdateAsync: (update: ConversationUpdate) => Promise<AxiosResponse>;
            sendConfirmationEmail: (areaIdentifier: string, emailAddress: string, dynamicResponse: string, keyValues: KeyValues) => Promise<AxiosResponse>;
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
    AxiosClient.defaults.baseURL = serverUrl;

    let Client = {
        Widget: {
            Access: {
                runPreCheck: async (): Promise<AxiosResponse> => AxiosClient.get(`widget/${secretKey}/precheck`),
                fetchGroups: async (): Promise<AxiosResponse> => AxiosClient.get(`widget/${secretKey}/groups`),
                fetchNodes: async (areaId: string): Promise<AxiosResponse> => AxiosClient.get(`widget/${secretKey}/${areaId}/nodes`),
                fetchPreferences: async (): Promise<AxiosResponse> => AxiosClient.get(`widget/${secretKey}/preferences`),
                fetchAreas: async (): Promise<AxiosResponse<Array<AreaTable>>> => AxiosClient.get(`widget/${secretKey}/areas`),
                postUpdateAsync: async(update: ConversationUpdate): Promise<AxiosResponse> => AxiosClient.post(`widget/${secretKey}/conversation`, update),
                sendConfirmationEmail: async(areaIdentifier: string, emailAddress: string, dynamicResponse: string, keyValues: KeyValues): Promise<AxiosResponse> => AxiosClient.post(`widget/${secretKey}/area/${areaIdentifier}/email/send`, {
                    EmailAddress: emailAddress,
                    DynamicResponse: dynamicResponse,
                    KeyValues: keyValues
                })
            }
        },
        secretKey: secretKey
    }

    return Client
}

export default CreateClient;