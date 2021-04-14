import {
    AreaTable,
    CompleteConverationDetails,
    ConversationUpdate,
    KeyValues,
    LocaleDefinition,
    NewConversation,
    PreCheckResult,
    SecretKey,
    SendEmailResultResponse,
    WidgetPreferences,
} from "@Palavyr-Types";
import axios, { AxiosResponse, AxiosInstance } from "axios";
import { serverUrl } from "./clientUtils";

export class WidgetClient {
    private client: AxiosInstance;
    public secretKey: string | null;

    constructor(secretKey: SecretKey = null) {
        if (secretKey === null) throw new Error("ApiKey not set correctly");
        this.secretKey = secretKey;

        this.client = axios.create({
            headers: {
                action: "apiKeyAccess",
            },
        });
        this.client.defaults.baseURL = serverUrl + "/api/";
    }

    public Routes = {
        precheck: (secretKey: SecretKey, isDemo: boolean) => `widget/pre-check?key=${secretKey}&demo=${isDemo}`,
        widgetPreferences: (secretKey: SecretKey) => `widget/preferences?key=${secretKey}`,
        locale: (secretKey: SecretKey) => `account/settings/locale/widget?key=${secretKey}`,
        areas: (secretKey: SecretKey) => `widget/areas?key=${secretKey}`,
        newConvo: (secretKey: SecretKey, areaId: string) => `widget/${areaId}/create?key=${secretKey}`,
        replyUpdate: (secretKey: SecretKey) => `widget/conversation?key=${secretKey}`,
        completeConvo: (secretKey: SecretKey) => `widget/complete?key=${this.secretKey}`,
        confirmationEmail: (secretKey: SecretKey, areaIdentifier: string) => `widget/area/${areaIdentifier}/email/send?key=${secretKey}`,
        fallbackEmail: (secretKey: SecretKey, areaIdentifier: string) => `widget/area/${areaIdentifier}/email/fallback/send?key=${secretKey}`,
    };

    public Widget = {
        Get: {
            PreCheck: async (isDemo: boolean): Promise<AxiosResponse<PreCheckResult>> => this.client.get(this.Routes.precheck(this.secretKey, isDemo)),
            WidgetPreferences: async (): Promise<AxiosResponse<WidgetPreferences>> => this.client.get(this.Routes.widgetPreferences(this.secretKey)),
            Locale: async (): Promise<AxiosResponse<LocaleDefinition>> => this.client.get(this.Routes.locale(this.secretKey)),
            Areas: async (): Promise<AxiosResponse<Array<AreaTable>>> => this.client.get(this.Routes.areas(this.secretKey)),
            NewConversation: async (areaId: string): Promise<AxiosResponse<NewConversation>> => this.client.get(this.Routes.newConvo(this.secretKey, areaId)),
        },

        Post: {
            ReplyUpdate: async (update: ConversationUpdate): Promise<AxiosResponse> => this.client.post(this.Routes.replyUpdate(this.secretKey), update),
            CompletedConversation: async (completeConvo: CompleteConverationDetails) => this.client.post(this.Routes.completeConvo(this.secretKey), completeConvo),
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
                this.client.post(this.Routes.confirmationEmail(this.secretKey, areaIdentifier), {
                    ConversationId: convoId,
                    EmailAddress: emailAddress,
                    DynamicResponses: dynamicResponses,
                    KeyValues: keyValues,
                    Name: name,
                    Phone: phone,
                    NumIndividuals: numIndividuals,
                }),
            FallbackEmail: async (areaIdentifier: string, emailAddress: string, name: string, phone: string, convoId: string) =>
                this.client.post(this.Routes.fallbackEmail(this.secretKey, areaIdentifier), {
                    ConversationId: convoId,
                    EmailAddress: emailAddress,
                    Name: name,
                    Phone: phone,
                }),
        },
    };
}
