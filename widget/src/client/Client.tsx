import { AreaTable, CompleteConverationDetails, ConversationUpdate, KeyValues, LocaleDefinition, NewConversation, PreCheckResult, SendEmailResultResponse, WidgetPreferences } from "@Palavyr-Types";
import axios, { AxiosResponse, AxiosInstance } from "axios";
import { serverUrl } from "./clientUtils";

export class WidgetClient {
    private client: AxiosInstance;
    public secretKey: string | null;

    constructor(secretKey: string | null) {
        if (secretKey === null) throw new Error("ApiKey not set correctly")
        this.secretKey = secretKey;

        this.client = axios.create({
            headers: {
                action: "apiKeyAccess",
            },
        });
        this.client.defaults.baseURL = serverUrl + "/api/";
    }

    public Widget = {
        Get: {
            PreCheck: async (isDemo: boolean): Promise<AxiosResponse<PreCheckResult>> => this.client.get(`widget/pre-check?key=${this.secretKey}&demo=${isDemo}`),
            WidgetPreferences: async (): Promise<AxiosResponse<WidgetPreferences>> => this.client.get(`widget/preferences?key=${this.secretKey}`),
            Locale: async (): Promise<AxiosResponse<LocaleDefinition>> => this.client.get(`account/settings/locale/widget?key=${this.secretKey}`),
            Areas: async (): Promise<AxiosResponse<Array<AreaTable>>> => this.client.get(`widget/areas?key=${this.secretKey}`),
            NewConversation: async (areaId: string): Promise<AxiosResponse<NewConversation>> => this.client.get(`widget/${areaId}/create?key=${this.secretKey}`),
        },

        Post: {
            ReplyUpdate: async (update: ConversationUpdate): Promise<AxiosResponse> => this.client.post(`widget/conversation?key=${this.secretKey}`, update),
            CompletedConversation: async (completeConvo: CompleteConverationDetails) => this.client.post(`widget/complete?key=${this.secretKey}`, completeConvo),
        },

        Send: {
            ConfirmationEmail: async (
                areaIdentifier: string,
                emailAddress: string,
                name: string,
                phone: string,
                numIndividuals: number,
                dynamicResponses: Array<{ [key: string]: string }>,
                keyValues: KeyValues,
                convoId: string
            ): Promise<AxiosResponse<SendEmailResultResponse>> =>
                this.client.post(`widget/area/${areaIdentifier}/email/send?key=${this.secretKey}`, {
                    ConversationId: convoId,
                    EmailAddress: emailAddress,
                    DynamicResponses: dynamicResponses,
                    KeyValues: keyValues,
                    Name: name,
                    Phone: phone,
                    NumIndividuals: numIndividuals,
                }),
            FallbackEmail: async (areaIdentifier: string, emailAddress: string, name: string, phone: string, convoId: string) =>
                this.client.post(`widget/area/${areaIdentifier}/email/fallback/send?key=${this.secretKey}`, {
                    ConversationId: convoId,
                    EmailAddress: emailAddress,
                    Name: name,
                    Phone: phone,
                }),
        },
    };
}
