import {
    AreaTable,
    ConversationRecordUpdate,
    ConversationRecordUpdate as ConvoRecord,
    WidgetConversationUpdate,
    DynamicResponse,
    KeyValues,
    LocaleResource,
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
        areas: (secretKey: SecretKey) => `widget/areas?key=${secretKey}`,
        newConvo: (secretKey: SecretKey, areaId: string) => `widget/${areaId}/create?key=${secretKey}`,
        updateConvoHistory: (secretKey: SecretKey) => `widget/conversation?key=${secretKey}`,
        updateConvoRecord: (secretKey: SecretKey) => `widget/record?key=${secretKey}`,
        confirmationEmail: (secretKey: SecretKey, areaIdentifier: string) => `widget/area/${areaIdentifier}/email/send?key=${secretKey}`,
        fallbackEmail: (secretKey: SecretKey, areaIdentifier: string) => `widget/area/${areaIdentifier}/email/fallback/send?key=${secretKey}`,
        internalCheck: (secretKey: SecretKey) => `widget/internal-check?key=${secretKey}`,
        nodeImage: (secretKey: SecretKey, nodeId: string) => `widget/node-image/${nodeId}?key=${secretKey}`,
        getIntroSequence: (secretKey: SecretKey) => `account/settings/intro-sequence?key=${secretKey}`,
    };

    public Widget = {
        Get: {
            PreCheck: async (isDemo: boolean) => this.client.get<PreCheckResult>(this.Routes.precheck(this.secretKey, isDemo)),
            WidgetPreferences: async () => this.client.get<WidgetPreferences>(this.Routes.widgetPreferences(this.secretKey)),
            Locale: async () => this.client.get<LocaleResponse>(this.Routes.locale(this.secretKey)),
            Areas: async () => this.client.get<Array<AreaTable>>(this.Routes.areas(this.secretKey)),
            NewConversation: async (areaId: string, recordUpdateDto: Partial<ConversationRecordUpdate>) =>
                this.client.post<NewConversation, {}>(this.Routes.newConvo(this.secretKey, areaId), recordUpdateDto),
            NodeImage: async (nodeId: string) => this.client.get<string>(this.Routes.nodeImage(this.secretKey, nodeId)),
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
                areaIdentifier: string,
                emailAddress: string,
                name: string,
                phone: string,
                numIndividuals: number,
                dynamicResponses: Array<{ [key: string]: string }>,
                keyValues: KeyValues,
                convoId: string
            ) =>
                this.client.post<SendEmailResultResponse, {}>(this.Routes.confirmationEmail(this.secretKey, areaIdentifier), {
                    ConversationId: convoId,
                    EmailAddress: emailAddress,
                    DynamicResponses: dynamicResponses,
                    KeyValues: keyValues,
                    Name: name,
                    Phone: phone,
                    NumIndividuals: numIndividuals,
                }),
            FallbackEmail: async (areaIdentifier: string, emailAddress: string, name: string, phone: string, convoId: string) =>
                this.client.post<SendEmailResultResponse, {}>(this.Routes.fallbackEmail(this.secretKey, areaIdentifier), {
                    ConversationId: convoId,
                    EmailAddress: emailAddress,
                    Name: name,
                    Phone: phone,
                }),
        },
    };
}