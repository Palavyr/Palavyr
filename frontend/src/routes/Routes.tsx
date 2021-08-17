import React from "react";
import { BrowserRouter as Router, Redirect, Route, Switch } from "react-router-dom";
import { LandingPage } from "@landing/Landing";
import { ProtectedRoute } from "@protected-routes";
import { DashboardLayout } from "dashboard/layouts/DashboardLayout";
import { Success } from "dashboard/content/purchse/success/Success";
import { Cancel } from "dashboard/content/purchse/cancel/Cancel";
import { Subscribe } from "dashboard/content/subscribe/Subscribe";
import { SubscribeHelp } from "dashboard/content/help/SubscribeHelp";
import { Enquires } from "dashboard/content/enquiries/Enquiries";
import { GetWidget } from "dashboard/content/getWidget/GetWidget";
import { QuickStartGuide } from "dashboard/content/welcome/quickStartGuide/QuickStartGuide";
import { ConversationHelp } from "dashboard/content/help/ConversationHelp";
import { EmailHelp } from "dashboard/content/help/EmailHelp";
import { ResponseConfigurationHelp } from "dashboard/content/help/ResponseConfigurationHelp";
import { AttachmentsHelp } from "dashboard/content/help/AttachmentsHelp";
import { AreaSettingsHelp } from "dashboard/content/help/AreaSettingsHelp";
import { PreviewHelp } from "dashboard/content/help/PreviewHelp";
import { PleaseConfirmYourEmail } from "dashboard/content/welcome/PleaseConfirmYourEmail";
import { Purchase } from "dashboard/content/purchse/Purchase";
import { ChatDemoHelp } from "dashboard/content/help/ChatDemoHelp";
import { ChatDemoPage } from "dashboard/content/demo/ChatDemo";
import { GetWidgetHelp } from "dashboard/content/help/GetWidgetHelp";
import { EnquiriesHelp } from "dashboard/content/help/EnquiriesHelp";
import { PurchaseHelp } from "dashboard/content/help/PurchaseHelp";
import { SuccessHelp } from "dashboard/content/help/SuccessHelp";
import { QuickStartGuideHelp } from "dashboard/content/help/QuickStartGuideHelp";
import { CancelHelp } from "dashboard/content/help/CancelHelp";
import { PleaseConfirmYourEmailHelp } from "dashboard/content/help/PleaseConfirmYourEmailHelp";
import { AuthContext } from "dashboard/layouts/DashboardContext";
import Auth from "auth/Auth";
import { EmailConfiguration } from "dashboard/content/responseConfiguration/uploadable/emailTemplates/EmailConfiguration";
import { ResponseConfiguration } from "dashboard/content/responseConfiguration/response/ResponseConfiguration";
import { AttachmentConfiguration } from "dashboard/content/responseConfiguration/uploadable/attachments/AttachmentConfiguration";
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
import { PleaseSubscribe } from "dashboard/content/purchse/pleaseSubscribe/PleaseSubscribe";
import { PleaseSubscribeHelp } from "dashboard/content/help/PleaseSubscribeHelp";
import { StructuredConvoTree } from "dashboard/content/responseConfiguration/conversation/PalavyrConfiguration";
import { TutorialPage } from "@landing/tutorialPage/TutorialPage";
import { TermsOfUsePage } from "legal/terms-of-use/TermsOfUsePage";
import { PrivacyPolicyPage } from "legal/privacy-policy/PrivacyPolicy";
import { ActivityDashboardPage } from "dashboard/content/activityDashboard/components/ActivityDashboardPage";
import { ActivityDashboardHelp } from "dashboard/content/help/DataDashboardHelp";
import { ToursPage } from "dashboard/content/welcome/OnboardingTour/tours/ToursPage";
import { ToursPageHelp } from "dashboard/content/help/ToursPageHelp";
import { blogPosts } from "@landing/blog/blogPosts";
import { BlogPostRouteMeta } from "@Palavyr-Types";
import { BlogPost } from "@landing/blog/components/BlogPost";
import { BlogPage } from "@landing/blog/BlogPage";
import { OurStoryPage } from "@landing/ourStory/OutStoryPage";
import { OurTeamPage } from "@landing/ourTeam/OurTeamPage";
import { GA4R } from "ga-4-react";
import { WidgetDesignerPage } from "dashboard/content/designer/WidgetDesigner";

const withLayout = (ContentComponent: () => JSX.Element, helpComponent: JSX.Element[] | JSX.Element) => {
    const ComponentWithHelp = () => {
        return (
            <AuthContext.Provider value={{ isActive: Auth.accountIsActive, isAuthenticated: Auth.accountIsAuthenticated }}>
                <DashboardLayout helpComponent={helpComponent}>
                    <GA4R code="G-9RFNBGK7HW">
                        <ContentComponent />
                    </GA4R>
                </DashboardLayout>
            </AuthContext.Provider>
        );
    };
    return ComponentWithHelp;
};

const withAreaTabs = (ContentComponent: JSX.Element[] | JSX.Element): (() => JSX.Element) => () => <AreaContent>{ContentComponent}</AreaContent>;
const withSettingsTabs = (ContentComponent: JSX.Element[] | JSX.Element): (() => JSX.Element) => () => <SettingsContent>{ContentComponent}</SettingsContent>;

const convertTitleToUriCompatible = (rawTitle: string) => {
    let title = rawTitle;
    title = title.toLowerCase();
    /* Remove unwanted characters, only accept alphanumeric and space */
    title = title.replace(/[^A-Za-z0-9 ]/g, "");
    /* Replace multi spaces with a single space */
    title = title.replace(/\s{2,}/g, " ");
    /* Replace space with a '-' symbol */
    title = title.replace(/\s/g, "-");
    return title;
};

const createPostUrl = (title: string) => {
    return `/blog/post/${title}`;
};

const createPostParam = (id: number) => {
    return `?id=${id}`;
};

export const Routes = () => {
    const blogPostRouteMetas: BlogPostRouteMeta[] = blogPosts.map(blogPost => {
        const titleSlug = convertTitleToUriCompatible(blogPost.title);
        const url = createPostUrl(titleSlug);
        const params = createPostParam(blogPost.id);

        return {
            ...blogPost,
            url,
            params,
        };
    });
    return (
        <Router>
            <Switch>
                <Route exact path="/" component={LandingPage} />
                <Route exact path="/tutorial" component={TutorialPage} />
                <Route exact path="/privacy-policy" component={PrivacyPolicyPage} />
                <Route exact path="/terms-of-use" component={TermsOfUsePage} />
                <Route exact path="/our-story" component={OurStoryPage} />
                <Route exact path="/team" component={OurTeamPage} />
                <Route exact path={RESET_PASSWORD_VERIFY} component={ConfirmYourResetLink} />
                <Route exact path={RESET_PASSWORD_FORM} component={RenderPasswordDialog} />
                <Route exact path={RESET_PASSWORD_SUCCESS} component={RenderResetSuccess} />

                {blogPostRouteMetas.map((post: BlogPostRouteMeta) => {
                    return (
                        <Route
                            key={post.url}
                            exact
                            path={post.url}
                            render={() => <BlogPost date={post.date} title={post.title} url={post.url} img={post.src} content={post.content} otherArticles={blogPostRouteMetas.filter(m => m.id !== post.id)} />}
                        />
                    );
                })}
                <Route exact path="/blog" render={() => <BlogPage blogPosts={blogPostRouteMetas} />} />

                <ProtectedRoute exact path="/dashboard" component={withLayout(QuickStartGuide, <QuickStartGuideHelp />)} />
                <ProtectedRoute exact path="/dashboard/welcome" component={withLayout(QuickStartGuide, <QuickStartGuideHelp />)} />
                <ProtectedRoute exact path="/dashboard/tour" component={withLayout(ToursPage, <ToursPageHelp />)} />

                <ProtectedRoute exact path="/dashboard/activity" component={withLayout(ActivityDashboardPage, <ActivityDashboardHelp />)} />
                <ProtectedRoute exact path="/dashboard/editor/email/:areaIdentifier" component={withLayout(withAreaTabs(<EmailConfiguration />), <EmailHelp />)} />
                <ProtectedRoute exact path="/dashboard/editor/response/:areaIdentifier" component={withLayout(withAreaTabs(<ResponseConfiguration />), <ResponseConfigurationHelp />)} />
                <ProtectedRoute exact path="/dashboard/editor/attachments/:areaIdentifier" component={withLayout(withAreaTabs(<AttachmentConfiguration />), <AttachmentsHelp />)} />
                <ProtectedRoute exact path="/dashboard/editor/conversation/:areaIdentifier" component={withLayout(withAreaTabs(<StructuredConvoTree />), <ConversationHelp />)} />
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

                <ProtectedRoute exact path="/dashboard/demo" component={withLayout(ChatDemoPage, <ChatDemoHelp />)} />
                <ProtectedRoute exact path="/dashboard/designer" component={withLayout(WidgetDesignerPage, <ChatDemoHelp />)} />

                <ProtectedRoute exact path="/dashboard/getWidget" component={withLayout(GetWidget, <GetWidgetHelp />)} />
                <ProtectedRoute exact path="/dashboard/enquiries" component={withLayout(Enquires, <EnquiriesHelp />)} />
                <ProtectedRoute exact path="/dashboard/enquiries/conversation" component={withLayout(ConversationReview, <ConversationReviewHelp />)} />
                <ProtectedRoute exact path="/dashboard/images" component={withLayout(ImageReview, <ImageReviewHelp />)} />

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
