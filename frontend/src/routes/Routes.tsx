import React from "react";
import { BrowserRouter as Router, Route } from "react-router-dom";
import { LandingPage } from "@landing/Landing";
import { ProtectedRoute } from "@protected-routes";
import { DashboardLayout } from "dashboard/layouts/DashboardLayout";
import { Success } from "dashboard/content/purchse/success/Success";
import { Cancel } from "dashboard/content/purchse/cancel/Cancel";
import { Subscribe } from "dashboard/content/subscribe/Subscribe";
import { SubscribeHelp } from "dashboard/content/help/SubscribeHelp";
import { Enquires } from "dashboard/content/enquiries/Enquiries";
import { GetWidget } from "dashboard/content/getWidget/GetWidget";
import { WelcomeToTheDashboard } from "dashboard/content/welcome/WelcomeToTheDashboard";
import { ConversationHelp } from "dashboard/content/help/ConversationHelp";
import { EmailHelp } from "dashboard/content/help/EmailHelp";
import { ResponseConfigurationHelp } from "dashboard/content/help/ResponseConfigurationHelp";
import { AttachmentsHelp } from "dashboard/content/help/AttachmentsHelp";
import { AreaSettingsHelp } from "dashboard/content/help/AreaSettingsHelp";
import { PreviewHelp } from "dashboard/content/help/PreviewHelp";
import { PleaseConfirmYourEmail } from "dashboard/content/welcome/PleaseConfirmYourEmail";
import { Purchase } from "dashboard/content/purchse/Purchase";
import { ChatDemoHelp } from "dashboard/content/help/ChatDemoHelp";
import { ChatDemo } from "dashboard/content/demo/ChatDemo";
import { GetWidgetHelp } from "dashboard/content/help/GetWidgetHelp";
import { EnquiriesHelp } from "dashboard/content/help/EnquiriesHelp";
import { PurchaseHelp } from "dashboard/content/help/PurchaseHelp";
import { SuccessHelp } from "dashboard/content/help/SuccessHelp";
import { WelcomeToTheDashboardHelp } from "dashboard/content/help/WelcomeToTheDashboardHelp";
import { CancelHelp } from "dashboard/content/help/CancelHelp";
import { PleaseConfirmYourEmailHelp } from "dashboard/content/help/PleaseConfirmYourEmailHelp";
import { AuthContext } from "dashboard/layouts/DashboardContext";
import Auth from "auth/Auth";
import { EmailConfiguration } from "dashboard/content/responseConfiguration/uploadable/emailTemplates/EmailConfiguration";
import { ResponseConfiguration } from "dashboard/content/responseConfiguration/response/ResponseConfiguration";
import { AttachmentConfiguration } from "dashboard/content/responseConfiguration/uploadable/attachments/AttachmentConfiguration";
import { ConvoTree } from "dashboard/content/responseConfiguration/conversation/ConvoTree";
import { AreaSettings } from "dashboard/content/responseConfiguration/areaSettings/AreaSettings";
import { ConfigurationPreview } from "dashboard/content/responseConfiguration/previews/ConfigurationPreview";
import { AreaContent } from "dashboard/content/responseConfiguration/AreaContent";
import { ChangeCompanyName } from "dashboard/content/settings/account/ChangeCompanyName";
import { ChangePhoneNumber } from "dashboard/content/settings/account/ChangePhoneNumber";
import { ChangeLogoImage } from "dashboard/content/settings/account/ChangeCompanyLogo";
import { ChangePassword } from "dashboard/content/settings/security/changePassword";
import { ChangeEmail } from "dashboard/content/settings/account/ChangeEmail";
import { ChangeLocale } from "dashboard/content/settings/account/ChangeLocale";
import { ChangeCompanyNameHelp } from "dashboard/content/help/ChangeCompanyNameHelp";
import { ChangeDefaultEmailHelp } from "dashboard/content/help/ChangeDefaultEmailHelp";
import { ChangeLocaleHelp } from "dashboard/content/help/ChangeLocaleHelp";
import { ChangePasswordHelp } from "dashboard/content/help/ChangePasswordHelp";
import { ChangePhoneNumberHelp } from "dashboard/content/help/ChangePhoneNumberHelp";
import { ChangeImageLogoHelp } from "dashboard/content/help/ChangeImageLogoHelp";
import { SettingsContent } from "dashboard/content/settings/SettingsContent";
import { DeleteAccount } from "dashboard/content/settings/account/DeleteAccount";
import { DeleteAccountHelp } from "dashboard/content/help/DeleteAccountHelp";
import { TermsOfServiceDialog } from "legal/TermsOfService";
import { PrivacyPolicy } from "legal/PrivacyPolicy";
import { ConversationReview } from "dashboard/content/enquiries/ConversationReview";
import { ConversationReviewHelp } from "dashboard/content/help/ConversationReviewHelp";
import { ConfirmYourResetLink } from "@landing/components/passwordReset/ConfirmYourResetLink";
import { RESET_PASSWORD_FORM, RESET_PASSWORD_VERIFY, RESET_PASSWORD_SUCCESS } from "@constants";
import { RenderPasswordDialog } from "@landing/components/passwordReset/SubmitNewPassword";
import { RenderResetSuccess } from "@landing/components/passwordReset/PasswordResetSuccess";
import { EnableAreas } from "dashboard/content/responseConfiguration/areaSettings/enableAreas/EnableAreas";
import { SetAreasHelp } from "dashboard/content/help/SetAreasHelp";
import { DefaultEmailTemplate } from "dashboard/content/settings/account/DefaultEmailTemplate";
import { DefaultEmailTemplateHelp } from "dashboard/content/help/DefaultEmailTemplateHelp";
import { ImageReview } from "dashboard/content/images/ImageReview";
import { ImageReviewHelp } from "dashboard/content/help/ImageReviewHelp";

const withLayout = (ContentComponent: () => JSX.Element, helpComponent: JSX.Element[] | JSX.Element) => {
    const ComponentWithHelp = () => {
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

const withAreaTabs = (ContentComponent: JSX.Element[] | JSX.Element): (() => JSX.Element) => () => <AreaContent>{ContentComponent}</AreaContent>;
const withSettingsTabs = (ContentComponent: JSX.Element[] | JSX.Element): (() => JSX.Element) => () => <SettingsContent>{ContentComponent}</SettingsContent>;

export const Routes = () => {
    return (
        <Router>
            <Route exact path="/" component={LandingPage} />
            <Route exact path="/privacy-policy" component={PrivacyPolicy} />
            <Route exact path="/terms-of-service" component={TermsOfServiceDialog} />
            <Route exact path={RESET_PASSWORD_VERIFY} component={ConfirmYourResetLink} />
            <Route exact path={RESET_PASSWORD_FORM} component={RenderPasswordDialog} />
            <Route exact path={RESET_PASSWORD_SUCCESS} component={RenderResetSuccess} />

            <ProtectedRoute exact path="/dashboard/" component={withLayout(WelcomeToTheDashboard, <WelcomeToTheDashboardHelp />)} />
            <ProtectedRoute exact path="/dashboard/welcome" component={withLayout(WelcomeToTheDashboard, <WelcomeToTheDashboardHelp />)} />
            <ProtectedRoute exact path="/dashboard/editor/email/:areaIdentifier" component={withLayout(withAreaTabs(<EmailConfiguration />), <EmailHelp />)} />
            <ProtectedRoute exact path="/dashboard/editor/response/:areaIdentifier" component={withLayout(withAreaTabs(<ResponseConfiguration />), <ResponseConfigurationHelp />)} />
            <ProtectedRoute exact path="/dashboard/editor/attachments/:areaIdentifier" component={withLayout(withAreaTabs(<AttachmentConfiguration />), <AttachmentsHelp />)} />
            <ProtectedRoute exact path="/dashboard/editor/conversation/:areaIdentifier" component={withLayout(withAreaTabs(<ConvoTree />), <ConversationHelp />)} />
            <ProtectedRoute exact path="/dashboard/editor/settings/:areaIdentifier" component={withLayout(withAreaTabs(<AreaSettings />), <AreaSettingsHelp />)} />
            <ProtectedRoute exact path="/dashboard/editor/preview/:areaIdentifier" component={withLayout(withAreaTabs(<ConfigurationPreview />), <PreviewHelp />)} />

            <ProtectedRoute exact path="/dashboard/set-areas" component={withLayout(EnableAreas, <SetAreasHelp />)} />

            <ProtectedRoute exact path="/dashboard/settings/password" component={withLayout(withSettingsTabs(<ChangePassword />), <ChangePasswordHelp />)} />
            <ProtectedRoute exact path="/dashboard/settings/email" component={withLayout(withSettingsTabs(<ChangeEmail />), <ChangeDefaultEmailHelp />)} />
            <ProtectedRoute exact path="/dashboard/settings/companyName" component={withLayout(withSettingsTabs(<ChangeCompanyName />), <ChangeCompanyNameHelp />)} />
            <ProtectedRoute exact path="/dashboard/settings/phoneNumber" component={withLayout(withSettingsTabs(<ChangePhoneNumber />), <ChangePhoneNumberHelp />)} />
            <ProtectedRoute exact path="/dashboard/settings/companyLogo" component={withLayout(withSettingsTabs(<ChangeLogoImage />), <ChangeImageLogoHelp />)} />
            <ProtectedRoute exact path="/dashboard/settings/locale" component={withLayout(withSettingsTabs(<ChangeLocale />), <ChangeLocaleHelp />)} />
            <ProtectedRoute exact path="/dashboard/settings/default_email_template" component={withLayout(withSettingsTabs(<DefaultEmailTemplate />), <DefaultEmailTemplateHelp />)} />
            <ProtectedRoute exact path="/dashboard/settings/deleteaccount" component={withLayout(withSettingsTabs(<DeleteAccount />), <DeleteAccountHelp />)} />

            <ProtectedRoute exact path="/dashboard/demo/" component={withLayout(ChatDemo, <ChatDemoHelp />)} />
            <ProtectedRoute exact path="/dashboard/getWidget/" component={withLayout(GetWidget, <GetWidgetHelp />)} />
            <ProtectedRoute exact path="/dashboard/enquiries/" component={withLayout(Enquires, <EnquiriesHelp />)} />
            <ProtectedRoute exact path="/dashboard/enquiries/conversation" component={withLayout(ConversationReview, <ConversationReviewHelp />)} />
            <ProtectedRoute exact path="/dashboard/images" component={withLayout(ImageReview, <ImageReviewHelp />)} />

            <ProtectedRoute exact path="/dashboard/subscribe" component={withLayout(Subscribe, <SubscribeHelp />)} />
            <ProtectedRoute exact path="/dashboard/subscribe/purchase/" component={withLayout(Purchase, <PurchaseHelp />)} />
            <ProtectedRoute exact path="/dashboard/subscribe/success" component={withLayout(Success, <SuccessHelp />)} />
            <ProtectedRoute exact path="/dashboard/subscribe/cancelled" component={withLayout(Cancel, <CancelHelp />)} />
            <ProtectedRoute exact path="/dashboard/confirm" component={withLayout(PleaseConfirmYourEmail, <PleaseConfirmYourEmailHelp />)} />
        </Router>
    );
};
