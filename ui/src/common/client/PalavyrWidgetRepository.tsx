import {
    AreaTable,
    ConversationRecordUpdate,
    ConversationRecordUpdate as ConvoRecord,
    WidgetConversationUpdate,
    DynamicResponse,
    KeyValues,
    LocaleResponse,
    NewConversation,
    PreCheckResult,
    SecretKey,
    SendEmailResultResponse,
    WidgetNodeResource,
    WidgetNodes,
    WidgetPreferences,
} from "@Palavyr-Types";
import { AxiosClient } from "./WidgetAxiosClient";

export class PalavyrWidgetRepository {
    private client: AxiosClient;
    public secretKey: string | null;

    constructor(secretKey: SecretKey = null) {
        this.client = new AxiosClient("apiKeyAccess");
        if (secretKey === null) throw new Error("ApiKey not set correctly");
        this.secretKey = secretKey;
    }

    public Routes = {
        precheck: (secretKey: SecretKey, isDemo: boolean) => `widget/pre-check?key=${secretKey}&demo=${isDemo}`,
        widgetPreferences: (secretKey: SecretKey) => `widget/preferences?key=${secretKey}`,
        locale: (secretKey: SecretKey) => `account/settings/locale/widget?key=${secretKey}`,
        intents: (secretKey: SecretKey) => `widget/intents?key=${secretKey}`,
        newConversationHistory: (secretKey: SecretKey, isDemo: boolean) => `widget/create?key=${secretKey}&demo=${isDemo}`,
        updateConvoHistory: (secretKey: SecretKey) => `widget/conversation?key=${secretKey}`,
        updateConvoRecord: (secretKey: SecretKey) => `widget/record?key=${secretKey}`,
        confirmationEmail: (secretKey: SecretKey, intentId: string, isDemo: boolean) => `widget/area/${intentId}/email/send?key=${secretKey}&demo=${isDemo}`,
        fallbackEmail: (secretKey: SecretKey, intentId: string, isDemo: boolean) => `widget/area/${intentId}/email/fallback/send?key=${secretKey}&demo=${isDemo}`,
        internalCheck: (secretKey: SecretKey) => `widget/internal-check?key=${secretKey}`,
        getIntroSequence: (secretKey: SecretKey) => `account/settings/intro-sequence?key=${secretKey}`,
    };

    public Widget = {
        Get: {
            PreCheck: async (isDemo: boolean) => this.client.get<PreCheckResult>(this.Routes.precheck(this.secretKey, isDemo)),
            WidgetPreferences: async () => this.client.get<WidgetPreferences>(this.Routes.widgetPreferences(this.secretKey)),
            Locale: async () => this.client.get<LocaleResponse>(this.Routes.locale(this.secretKey)),
            Intents: async () => this.client.get<Array<AreaTable>>(this.Routes.intents(this.secretKey)),
            NewConversationHistory: async (recordUpdateDto: Partial<ConversationRecordUpdate>, isDemo: boolean) =>
                this.client.post<NewConversation, {}>(this.Routes.newConversationHistory(this.secretKey, isDemo), recordUpdateDto),
            IntroSequence: async () => this.client.get<WidgetNodes>(this.Routes.getIntroSequence(this.secretKey)),
        },

        Post: {
            InternalCheck: async (node: WidgetNodeResource, response: string, currentDynamicResponseState: DynamicResponse) =>
                this.client.post<boolean, {}>(this.Routes.internalCheck(this.secretKey), {
                    Node: node,
                    Response: response,
                    CurrentDynamicResponseState: currentDynamicResponseState,
                }),
            UpdateConvoHistory: async (update: WidgetConversationUpdate) => this.client.post(this.Routes.updateConvoHistory(this.secretKey), update),
            UpdateConvoRecord: async (updatedConvoRecord: Partial<ConvoRecord>) => this.client.post(this.Routes.updateConvoRecord(this.secretKey), updatedConvoRecord),
        },

        Send: {
            ConfirmationEmail: async (
                intentId: string,
                emailAddress: string,
                name: string,
                phone: string,
                numIndividuals: number,
                dynamicResponses: Array<{ [key: string]: string }>,
                keyValues: KeyValues,
                convoId: string,
                isDemo: boolean
            ) =>
                this.client.post<SendEmailResultResponse, {}>(this.Routes.confirmationEmail(this.secretKey, intentId, isDemo), {
                    ConversationId: convoId,
                    EmailAddress: emailAddress,
                    DynamicResponses: dynamicResponses,
                    KeyValues: keyValues,
                    Name: name,
                    Phone: phone,
                    NumIndividuals: numIndividuals,
                }),
            FallbackEmail: async (intentId: string, emailAddress: string, name: string, phone: string, convoId: string, isDemo: boolean) =>
                this.client.post<SendEmailResultResponse, {}>(this.Routes.fallbackEmail(this.secretKey, intentId, isDemo), {
                    ConversationId: convoId,
                    EmailAddress: emailAddress,
                    Name: name,
                    Phone: phone,
                }),
        },
    };
}
