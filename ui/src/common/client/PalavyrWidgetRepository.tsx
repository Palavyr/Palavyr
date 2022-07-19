import { PreCheckResultResource, SendLiveEmailResultResource } from "@common/types/api/ApiContracts";
import { IntentResource, NewConversationResource, WidgetNodeResource, WidgetNodeResources, WidgetPreferencesResource } from "@common/types/api/EntityResources";
import { ConversationRecordUpdate, ConversationRecordUpdate as ConvoRecord, WidgetConversationUpdate, DynamicResponse, KeyValues, LocaleResponse, SecretKey } from "@Palavyr-Types";
import { ApiRoutes } from "./ApiRoutes";
import { AxiosClient } from "./WidgetAxiosClient";

export class PalavyrWidgetRepository extends ApiRoutes {
    private client: AxiosClient;
    public secretKey: string | null;

    constructor(secretKey: SecretKey = null) {
        super();
        this.client = new AxiosClient("apiKeyAccess");
        if (secretKey === null) throw new Error("ApiKey not set correctly");
        this.secretKey = secretKey;
    }

    public Widget = {
        Get: {
            PreCheck: async (isDemo: boolean) => this.client.get<PreCheckResultResource>(this.Routes.precheck(this.secretKey, isDemo)),
            WidgetPreferences: async () => this.client.get<WidgetPreferencesResource>(this.Routes.widgetPreferences(this.secretKey)),
            Locale: async () => this.client.get<LocaleResponse>(this.Routes.locale(this.secretKey)),
            Intents: async () => this.client.get<Array<IntentResource>>(this.Routes.intents(this.secretKey)),
            NewConversationHistory: async (recordUpdateDto: Partial<ConversationRecordUpdate>, isDemo: boolean) =>
                this.client.post<NewConversationResource, {}>(this.Routes.newConversationHistory(this.secretKey, isDemo), recordUpdateDto),
            IntroSequence: async () => this.client.get<WidgetNodeResources>(this.Routes.GetIntroSequence(this.secretKey)),
        },

        Post: {
            InternalCheck: async (node: WidgetNodeResource, response: string, currentDynamicResponseState: DynamicResponse) =>
                this.client.post<boolean, {}>(this.Routes.InternalCheck(this.secretKey), {
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
                this.client.post<SendLiveEmailResultResource, {}>(this.Routes.confirmationEmail(this.secretKey, intentId, isDemo), {
                    ConversationId: convoId,
                    EmailAddress: emailAddress,
                    DynamicResponses: dynamicResponses,
                    KeyValues: keyValues,
                    Name: name,
                    Phone: phone,
                    NumIndividuals: numIndividuals,
                }),
            FallbackEmail: async (intentId: string, emailAddress: string, name: string, phone: string, convoId: string, isDemo: boolean) =>
                this.client.post<SendLiveEmailResultResource, {}>(this.Routes.FallbackEmail(this.secretKey, intentId, isDemo), {
                    ConversationId: convoId,
                    EmailAddress: emailAddress,
                    Name: name,
                    Phone: phone,
                }),
        },
    };
}
