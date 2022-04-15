import React from "react";
import { BrowserRouter as Router, Redirect, Route, Switch, useLocation } from "react-router-dom";
import { LoginPage } from "@landing/LoginPage";
import { ProtectedRoute } from "@protected-routes";
import { DashboardLayout } from "frontend/dashboard/layouts/DashboardLayout";
import { Success } from "frontend/dashboard/content/purchse/success/Success";
import { Cancel } from "frontend/dashboard/content/purchse/cancel/Cancel";
import { Subscribe } from "frontend/dashboard/content/subscribe/Subscribe";
import { SubscribeHelp } from "frontend/dashboard/content/help/SubscribeHelp";
import { EnquiresPage } from "frontend/dashboard/content/enquiries/EnquiriesPage";
import { GetWidget } from "frontend/dashboard/content/getWidget/GetWidget";
import { QuickStartGuide } from "frontend/dashboard/content/welcome/quickStartGuide/QuickStartGuide";
import { ConversationHelp } from "frontend/dashboard/content/help/ConversationHelp";
import { EmailHelp } from "frontend/dashboard/content/help/EmailHelp";
import { ResponseConfigurationHelp } from "frontend/dashboard/content/help/ResponseConfigurationHelp";
import { AttachmentsHelp } from "frontend/dashboard/content/help/AttachmentsHelp";
import { PreviewHelp } from "frontend/dashboard/content/help/PreviewHelp";
import { PleaseConfirmYourEmail } from "frontend/dashboard/content/welcome/PleaseConfirmYourEmail";
import { Purchase } from "frontend/dashboard/content/purchse/Purchase";
import { ChatDemoHelp } from "frontend/dashboard/content/help/ChatDemoHelp";
import { ChatDemoPage } from "frontend/dashboard/content/demo/ChatDemo";
import { GetWidgetHelp } from "frontend/dashboard/content/help/GetWidgetHelp";
import { EnquiriesHelp } from "frontend/dashboard/content/help/EnquiriesHelp";
import { PurchaseHelp } from "frontend/dashboard/content/help/PurchaseHelp";
import { SuccessHelp } from "frontend/dashboard/content/help/SuccessHelp";
import { QuickStartGuideHelp } from "frontend/dashboard/content/help/QuickStartGuideHelp";
import { CancelHelp } from "frontend/dashboard/content/help/CancelHelp";
import { PleaseConfirmYourEmailHelp } from "frontend/dashboard/content/help/PleaseConfirmYourEmailHelp";
import { AuthContext } from "frontend/dashboard/layouts/DashboardContext";
import Auth from "frontend/auth/Auth";
import { EmailConfiguration } from "frontend/dashboard/content/responseConfiguration/uploadable/emailTemplates/EmailConfiguration";
import { ResponseConfiguration } from "frontend/dashboard/content/responseConfiguration/response/ResponseConfiguration";
import { AttachmentConfiguration } from "frontend/dashboard/content/responseConfiguration/uploadable/attachments/AttachmentConfiguration";
import { IntentSettings } from "@frontend/dashboard/content/responseConfiguration/areaSettings/IntentSettings";
import { ConfigurationPreview } from "frontend/dashboard/content/responseConfiguration/previews/ConfigurationPreview";
import { IntentContent } from "@frontend/dashboard/content/responseConfiguration/IntentContent";
import { ChangeCompanyName } from "frontend/dashboard/content/settings/account/ChangeCompanyName";
import { ChangePhoneNumber } from "frontend/dashboard/content/settings/account/ChangePhoneNumber";
import { ChangeLogoImage } from "frontend/dashboard/content/settings/account/ChangeCompanyLogo";
import { ChangePassword } from "frontend/dashboard/content/settings/security/changePassword";
import { ChangeEmail } from "frontend/dashboard/content/settings/account/ChangeEmail";
import { ChangeLocale } from "frontend/dashboard/content/settings/account/ChangeLocale";
import { ChangeCompanyNameHelp } from "frontend/dashboard/content/help/ChangeCompanyNameHelp";
import { ChangeDefaultEmailHelp } from "frontend/dashboard/content/help/ChangeDefaultEmailHelp";
import { ChangeLocaleHelp } from "frontend/dashboard/content/help/ChangeLocaleHelp";
import { ChangePasswordHelp } from "frontend/dashboard/content/help/ChangePasswordHelp";
import { ChangePhoneNumberHelp } from "frontend/dashboard/content/help/ChangePhoneNumberHelp";
import { ChangeImageLogoHelp } from "frontend/dashboard/content/help/ChangeImageLogoHelp";
import { GeneralSettingsTabs } from "@frontend/dashboard/content/settings/GeneralSettingsTabs";
import { DeleteAccount } from "frontend/dashboard/content/settings/account/DeleteAccount";
import { DeleteAccountHelp } from "frontend/dashboard/content/help/DeleteAccountHelp";
import { ConversationReview } from "frontend/dashboard/content/enquiries/ConversationReview";
import { ConversationReviewHelp } from "frontend/dashboard/content/help/ConversationReviewHelp";
import { ConfirmYourResetLink } from "@landing/components/passwordReset/ConfirmYourResetLink";
import { RESET_PASSWORD_FORM, RESET_PASSWORD_VERIFY, RESET_PASSWORD_SUCCESS } from "@constants";
import { RenderPasswordDialog } from "@landing/components/passwordReset/SubmitNewPassword";
import { RenderResetSuccess } from "@landing/components/passwordReset/PasswordResetSuccess";
import { SetIntentsHelp } from "frontend/dashboard/content/help/SetAreasHelp";
import { DefaultEmailTemplate } from "frontend/dashboard/content/settings/account/DefaultEmailTemplate";
import { DefaultEmailTemplateHelp } from "frontend/dashboard/content/help/DefaultEmailTemplateHelp";
import { FileAssetReview } from "@frontend/dashboard/content/fileAssetReview/FileAssetReview";
import { ImageReviewHelp } from "frontend/dashboard/content/help/ImageReviewHelp";
import { PleaseSubscribe } from "frontend/dashboard/content/purchse/pleaseSubscribe/PleaseSubscribe";
import { PleaseSubscribeHelp } from "frontend/dashboard/content/help/PleaseSubscribeHelp";
import { ConversationConfigurationPage } from "frontend/dashboard/content/responseConfiguration/conversation/ConversationConfigurationPage";
import { IntroConversationConfigurationPage } from "frontend/dashboard/content/responseConfiguration/conversation/IntroConversationConfigurationPage";
import { ActivityDashboardPage } from "frontend/dashboard/content/activityDashboard/components/ActivityDashboardPage";
import { ActivityDashboardHelp } from "frontend/dashboard/content/help/DataDashboardHelp";
import { ToursPage } from "frontend/dashboard/content/welcome/OnboardingTour/tours/ToursPage";
import { ToursPageHelp } from "frontend/dashboard/content/help/ToursPageHelp";
import { WidgetDesignerPage } from "frontend/dashboard/content/designer/WidgetDesigner";
import { AppPageView } from "@common/Analytics/gtag";
import { IntentSettingsHelp } from "@frontend/dashboard/content/help/IntentSettingsHelp";
import { SignupPage } from "@landing/SignupPage";
import { EnableIntents } from "@frontend/dashboard/content/responseConfiguration/areaSettings/enableAreas/EnableIntents";

const withLayout = (ContentComponent: () => JSX.Element, helpComponent: JSX.Element[] | JSX.Element) => {
    const ComponentWithHelp = () => {
        const location = useLocation();
        AppPageView(location.pathname);
        return (
            <AuthContext.Provider value={{ isActive: Auth.accountIsActive, isAuthenticated: Auth.accountIsAuthenticated }}>
                <DashboardLayout helpComponent={helpComponent}>
                    <ContentComponent />
                </DashboardLayout>
            </AuthContext.Provider>
        );
    };
    return ComponentWithHelp;
};

const withIntentSettingsTabs = (ContentComponent: JSX.Element[] | JSX.Element): (() => JSX.Element) => () => {
    const location = useLocation();
    AppPageView(location.pathname);
    return <IntentContent>{ContentComponent}</IntentContent>;
};
const withGeneralSettingsTabs = (ContentComponent: JSX.Element[] | JSX.Element): (() => JSX.Element) => () => {
    const location = useLocation();
    AppPageView(location.pathname);
    return <GeneralSettingsTabs>{ContentComponent}</GeneralSettingsTabs>;
};

export const Routes = () => {
    return (
        <Router>
            <Switch>
                <Route exact path="/" component={LoginPage} />
                <Route exact path="/login" component={LoginPage} />
                <Route exact path="/signup" component={SignupPage} />

                <Route exact path={RESET_PASSWORD_VERIFY} component={ConfirmYourResetLink} />
                <Route exact path={RESET_PASSWORD_FORM} component={RenderPasswordDialog} />
                <Route exact path={RESET_PASSWORD_SUCCESS} component={RenderResetSuccess} />

                <ProtectedRoute exact path="/dashboard" component={withLayout(QuickStartGuide, <QuickStartGuideHelp />)} />
                <ProtectedRoute exact path="/dashboard/welcome" component={withLayout(QuickStartGuide, <QuickStartGuideHelp />)} />
                <ProtectedRoute exact path="/dashboard/tour" component={withLayout(ToursPage, <ToursPageHelp />)} />

                <ProtectedRoute exact path="/dashboard/activity" component={withLayout(ActivityDashboardPage, <ActivityDashboardHelp />)} />
                <ProtectedRoute exact path="/dashboard/editor/email/:areaIdentifier" component={withLayout(withIntentSettingsTabs(<EmailConfiguration />), <EmailHelp />)} />
                <ProtectedRoute exact path="/dashboard/editor/pricing/:areaIdentifier" component={withLayout(withIntentSettingsTabs(<ResponseConfiguration />), <ResponseConfigurationHelp />)} />
                <ProtectedRoute exact path="/dashboard/editor/attachments/:areaIdentifier" component={withLayout(withIntentSettingsTabs(<AttachmentConfiguration />), <AttachmentsHelp />)} />

                <ProtectedRoute exact path="/dashboard/editor/conversation/:areaIdentifier" component={withLayout(withIntentSettingsTabs(<ConversationConfigurationPage />), <ConversationHelp />)} />
                <ProtectedRoute exact path="/dashboard/editor/conversation/intro/:areaIdentifier" component={withLayout(IntroConversationConfigurationPage, <ConversationHelp />)} />

                <ProtectedRoute exact path="/dashboard/editor/settings/:areaIdentifier" component={withLayout(withIntentSettingsTabs(<IntentSettings />), <IntentSettingsHelp />)} />
                <ProtectedRoute exact path="/dashboard/editor/pricingpreview/:areaIdentifier" component={withLayout(withIntentSettingsTabs(<ConfigurationPreview />), <PreviewHelp />)} />

                <ProtectedRoute exact path="/dashboard/set-areas" component={withLayout(EnableIntents, <SetIntentsHelp />)} />

                <ProtectedRoute exact path="/dashboard/settings/password" component={withLayout(withGeneralSettingsTabs(<ChangePassword />), <ChangePasswordHelp />)} />
                <ProtectedRoute exact path="/dashboard/settings/email" component={withLayout(withGeneralSettingsTabs(<ChangeEmail />), <ChangeDefaultEmailHelp />)} />
                <ProtectedRoute exact path="/dashboard/settings/companyName" component={withLayout(withGeneralSettingsTabs(<ChangeCompanyName />), <ChangeCompanyNameHelp />)} />
                <ProtectedRoute exact path="/dashboard/settings/phoneNumber" component={withLayout(withGeneralSettingsTabs(<ChangePhoneNumber />), <ChangePhoneNumberHelp />)} />
                <ProtectedRoute exact path="/dashboard/settings/companyLogo" component={withLayout(withGeneralSettingsTabs(<ChangeLogoImage />), <ChangeImageLogoHelp />)} />
                <ProtectedRoute exact path="/dashboard/settings/locale" component={withLayout(withGeneralSettingsTabs(<ChangeLocale />), <ChangeLocaleHelp />)} />
                <ProtectedRoute exact path="/dashboard/settings/default_email_template" component={withLayout(withGeneralSettingsTabs(<DefaultEmailTemplate />), <DefaultEmailTemplateHelp />)} />
                <ProtectedRoute exact path="/dashboard/settings/deleteaccount" component={withLayout(withGeneralSettingsTabs(<DeleteAccount />), <DeleteAccountHelp />)} />

                <ProtectedRoute exact path="/dashboard/demo" component={withLayout(ChatDemoPage, <ChatDemoHelp />)} />
                <ProtectedRoute exact path="/dashboard/designer" component={withLayout(WidgetDesignerPage, <ChatDemoHelp />)} />

                <ProtectedRoute exact path="/dashboard/getWidget" component={withLayout(GetWidget, <GetWidgetHelp />)} />
                <ProtectedRoute exact path="/dashboard/enquiries" component={withLayout(EnquiresPage, <EnquiriesHelp />)} />
                <ProtectedRoute exact path="/dashboard/enquiries/conversation" component={withLayout(ConversationReview, <ConversationReviewHelp />)} />
                <ProtectedRoute exact path="/dashboard/file-assets" component={withLayout(FileAssetReview, <ImageReviewHelp />)} />

                <ProtectedRoute exact path="/dashboard/subscribe" component={withLayout(Subscribe, <SubscribeHelp />)} />
                <ProtectedRoute exact path="/dashboard/subscribe/purchase" component={withLayout(Purchase, <PurchaseHelp />)} />
                <ProtectedRoute exact path="/dashboard/subscribe/success" component={withLayout(Success, <SuccessHelp />)} />
                <ProtectedRoute exact path="/dashboard/subscribe/cancelled" component={withLayout(Cancel, <CancelHelp />)} />
                <ProtectedRoute exact path="/dashboard/confirm" component={withLayout(PleaseConfirmYourEmail, <PleaseConfirmYourEmailHelp />)} />
                <ProtectedRoute exact path="/dashboard/please-subscribe" component={withLayout(PleaseSubscribe, <PleaseSubscribeHelp />)} />

                <Redirect to="/" />
            </Switch>
        </Router>
    );
};
