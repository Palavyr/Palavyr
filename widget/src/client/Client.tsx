import axios, { AxiosResponse, AxiosInstance } from "axios";
import { ConversationUpdate, AreaTable, CompleteConverationDetails, PreCheckResult, WidgetPreferences, SendEmailResultResponse, LocaleDefinition } from "../types";
import { serverUrl } from "./clientUtils";

export interface IClient {
    Widget: {
        Access: {
            getLocale: () => Promise<AxiosResponse<LocaleDefinition>>,
            runPreCheck: (isDemo: boolean) => Promise<AxiosResponse<PreCheckResult>>;
            fetchWidgetState: () => Promise<AxiosResponse<boolean>>;
            fetchGroups: () => Promise<AxiosResponse>;
            createConvo: (areaId: string) => Promise<AxiosResponse>;
            fetchAreas: () => Promise<AxiosResponse>;
            fetchPreferences: () => Promise<AxiosResponse<WidgetPreferences>>;
            postUpdateAsync: (update: ConversationUpdate) => Promise<AxiosResponse>;
            sendConfirmationEmail: (areaIdentifier: string, emailAddress: string, name: string, phone: string, numIndividuals: number, dynamicResponses: Array<{[key: string]: string}>, keyValues: KeyValues, convoId: string) => Promise<AxiosResponse<SendEmailResultResponse>>;
            postCompleteConversation: (completeConvo: CompleteConverationDetails) => Promise<AxiosResponse>;
            sendFallbackEmail: (areaIdentifier: string, emailAddress: string, name: string, phone: string, convoId: string) => Promise<AxiosResponse<SendEmailResultResponse>>;
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
                "action": "apiKeyAccess"
            }
        }
    )
    AxiosClient.defaults.baseURL = serverUrl + "/api/"

    let Client = {
        Widget: {
            Access: {
                getLocale: async (): Promise<AxiosResponse<LocaleDefinition>> => AxiosClient.get(`account/settings/locale/widget?key=${secretKey}`),
                fetchAreas: async (): Promise<AxiosResponse<Array<AreaTable>>> => AxiosClient.get(`widget/areas?key=${secretKey}`),
                runPreCheck: async (isDemo: boolean): Promise<AxiosResponse<PreCheckResult>> => AxiosClient.get(`widget/pre-check?key=${secretKey}&demo=${isDemo}`),
                fetchWidgetState: async (): Promise<AxiosResponse<boolean>> => AxiosClient.get(`widget/widget-active-state?key=${secretKey}`),
                fetchGroups: async (): Promise<AxiosResponse> => AxiosClient.get(`widget/groups?key=${secretKey}`),
                createConvo: async (areaId: string): Promise<AxiosResponse> => AxiosClient.get(`widget/${areaId}/create?key=${secretKey}`),
                fetchPreferences: async (): Promise<AxiosResponse> =>AxiosClient.get(`widget/preferences?key=${secretKey}`),
                postUpdateAsync: async(update: ConversationUpdate): Promise<AxiosResponse> => AxiosClient.post(`widget/conversation?key=${secretKey}`, update),
                postCompleteConversation: async(completeConvo: CompleteConverationDetails) => AxiosClient.post(`widget/complete?key=${secretKey}`, completeConvo),
                sendConfirmationEmail: async (areaIdentifier: string, emailAddress: string, name: string, phone: string, numIndividuals: number, dynamicResponses: Array<{[key: string]: string}>, keyValues: KeyValues, convoId: string): Promise<AxiosResponse<SendEmailResultResponse>> => AxiosClient.post(`widget/area/${areaIdentifier}/email/send?key=${secretKey}`, {
                    ConversationId: convoId,
                    EmailAddress: emailAddress,
                    DynamicResponses: dynamicResponses,
                    KeyValues: keyValues,
                    Name: name,
                    Phone: phone,
                    NumIndividuals: numIndividuals
                }),
                sendFallbackEmail: async (areaIdentifier: string, emailAddress: string, name: string, phone: string, convoId: string) => AxiosClient.post(`widget/area/${areaIdentifier}/email/fallback/send?key=${secretKey}`,
                {
                    ConversationId: convoId,
                    EmailAddress: emailAddress,
                    Name: name,
                    Phone: phone
                })
            }
        },
        secretKey: secretKey
    }

    return Client
}

export default CreateClient;